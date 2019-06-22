/*
 * Copyright 2009 George Daswani
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;

namespace WidebandSupport
{

	public abstract class WidebandReader : IDisposable
	{
		public delegate void ErrorReceivedEventHandler(object sender, Exception ex);
		public event ErrorReceivedEventHandler ErrorReceived;
		
		private Thread worker;
		protected bool continueRunning = false;
		protected SerialPort comPort;
		
		protected double latestReading;
		public double LatestReading
		{
			get { return latestReading; }
		}

		protected bool testMode = false; // if true, test mode
		public bool TestMode
		{
			get { return testMode; }
			set { testMode = value; }
		}

		public virtual void Start()
		{
			if (continueRunning == false)
			{
				comPort.Open();
				continueRunning = true;
				worker = new Thread(InitiateReading);
				worker.Start();
			}
			else
			{
				throw new InvalidOperationException("Already started.");
			}
		}
		public virtual void  Stop()
		{
			if (continueRunning == true)
			{
				continueRunning = false;
				worker.Join();
				comPort.Close();
			}
		}

		protected virtual void InitiateReading()
		{
			throw new NotImplementedException();
		}
		
		public void Dispose()
		{
			Stop();
		}
		
		protected bool IsSerialPortNameValid(String comPortName)
		{

			bool serialPortNameValid = false;

			foreach (String serialPortName in SerialPort.GetPortNames())
			{
				if (serialPortName.Equals(comPortName))
				{
					serialPortNameValid = true;
				}
			}

			return serialPortNameValid;
		}
		
		protected void OnErrorReceived(Exception ex)
		{
			if(ErrorReceived != null)
			{
				this.ErrorReceived(this,ex);
			}
		}
	}

}
