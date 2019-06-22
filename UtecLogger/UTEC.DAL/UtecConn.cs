/*
 * Created by SharpDevelop.
 * User: Walter
 * Date: 5/23/2011
 * Time: 5:43 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;

namespace UTEC.DAL
{
	public class UtecWideBandConnection
	{
		public delegate void DataReceivedEventHandler(object sender, string e);
		public event DataReceivedEventHandler DataReceived;
		
		public delegate void ErrorReceivedEventHandler(object sender, Exception ex);
		public event ErrorReceivedEventHandler ErrorReceived;
		
		WidebandSupport.WidebandReader _externalWB;
		UtecSerialConn _utecConn;
		
		public UtecWideBandConnection()
		{
		}

		void _utecConn_DataReceived(object sender, string e)
		{

			if(!TXSTunerPresent)
			{
				e = e.Trim().Replace(Environment.NewLine,string.Empty);
				e = String.Format("{0} {1}", e , Math.Round(_externalWB.LatestReading, 2));
			}
			
			if(!String.IsNullOrEmpty(e))
			{
				OnDataReceived(e);
			}

		}
		
		public bool TXSTunerPresent{get;set;}
		
		public void OpenConnection(string portName, bool fastLogging, int linesPerSecond, bool testMode)
		{
			CloseConnnection();
			
			_utecConn = new UtecSerialConn(portName);
			_utecConn.FastLogging = fastLogging;
			_utecConn.LinesPerSecond = linesPerSecond;
			_utecConn.DataReceived += _utecConn_DataReceived;
			_utecConn.ErrorReceived += SerialConnection_ErrorReceived;
			_utecConn.TestMode = testMode;
			_utecConn.Start();
			
			if(!TXSTunerPresent)
			{
				if(ExternalWBComPort == portName && ExternalWB != "No Wideband")
				{
					throw new ArgumentException(String.Format("UTEC Com Port and External Wideband Com Port cannot be the same."));
				}
				
				_externalWB = new WidebandSupport.WidebandFactory(ExternalWB, ExternalWBComPort, _utecConn.TestMode).CreateInstance();
				_externalWB.ErrorReceived += SerialConnection_ErrorReceived;
				_externalWB.Start();
			}
		}

		void SerialConnection_ErrorReceived(object sender, Exception ex)
		{
			OnErrorReceived(ex);
		}
		
		private void OnErrorReceived(Exception ex)
		{
			if(ErrorReceived != null)
			{
				ErrorReceived(this, ex);
			}
		}
		
		public bool IsOpen
		{
			get
			{
				if(_utecConn!= null)
				{
					return _utecConn.IsOpen;
				}
				
				return false;
			}
		}
		
		public void CloseConnnection()
		{
			if(_utecConn != null)
			{
				_utecConn.DataReceived -=_utecConn_DataReceived;
				_utecConn.ErrorReceived -= SerialConnection_ErrorReceived;
				_utecConn.Stop();
				_utecConn.Dispose();
				_utecConn = null;
			}
			
			if(_externalWB != null)
			{
				_externalWB.ErrorReceived -= SerialConnection_ErrorReceived;
				_externalWB.Stop();
				_externalWB.Dispose();
				_externalWB = null;
			}
		}
		
		protected virtual void OnDataReceived(string e)
		{
			if (DataReceived != null)
			{
				DataReceived(this, e);
			}
		}
		
		public string ExternalWB {get;set;}
		public string ExternalWBComPort{get;set;}
	}
}