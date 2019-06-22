/*
 * Created by SharpDevelop.
 * User: Walter
 * Date: 05/28/2011
 * Time: 11:08
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

using MVVM;
using UTEC.Logger.ViewModels;

namespace UTEC.Logger.Model
{
	/// <summary>
	/// Description of Settings.
	/// </summary>
	[Serializable]
	public class Settings : ViewModelBase
	{
		public Settings()
		{
		}
		
		GeneralSettings _generalSettings = new GeneralSettings();
		public GeneralSettings GeneralSettings
		{	
			get{return _generalSettings;}
			set{_generalSettings = value;}
		}
		
		AutoLogSettings _autoLogSettings = new AutoLogSettings();		
		public AutoLogSettings AutoLogSettings
		{	
			get{return _autoLogSettings;}
			set{_autoLogSettings = value;}
		}
		
		ComPortSettings _comPortSettings = new ComPortSettings();
		public ComPortSettings ComPortSettings
		{	
			get{return _comPortSettings;}
			set{_comPortSettings = value;}
		}
	}
	
	[Serializable]
	public class GeneralSettings : ViewModelBase
	{
		string _persistentTables = @"Settings\Tables.csv";
		public string PersistentTables
		{	
			get{return _persistentTables;}
			set
			{
				_persistentTables = value;
				OnPropertyChanged("PersistentTables");
			}
		}
		
		string _afrTargetTable = @"Settings\TargetAFR.csv";
		public string AfrTargetTable
		{	
			get{return _afrTargetTable;}
			set
			{
				_afrTargetTable = value;
				OnPropertyChanged("AfrTargetTable");
			}
		}
		
		string _logOutputDirectory = "Logs";
		public string LogOutputDirectory
		{	
			get{return _logOutputDirectory;}
			set
			{
				_logOutputDirectory = value;
				OnPropertyChanged("LogOutputDirectory");
			}
		}
	}
	
	[Serializable]
	public class AutoLogSettings : ViewModelBase
	{
		LogTrigger _autoLogConditions = new LogTrigger();
		public LogTrigger AutoLogConditions
		{
			get{return _autoLogConditions;}
			set{_autoLogConditions = value;}
		}
		
		LogTrigger _logSaveConditions = new LogTrigger();
		public LogTrigger LogSaveConditions
		{
			get{return _logSaveConditions;}
			set{_logSaveConditions = value;}
		}
		
		public bool AutoLogKnock
		{get;set;}
		
		public int KnockThreshold
		{get;set;}
		
		public bool  FastLogging
		{get;set;}
		
		public bool RpmSmoothing
		{get;set;}
		
		public int LinesPerSecond
		{get;set;}
		
		public bool TablePullsOnly
		{get;set;}
		
		public double RichAfrOffset
		{get;set;}
		
		public double LeanAfrOffset
		{get;set;}
	}
	
	[Serializable]
	public class LogTrigger : ViewModelBase
	{
		public int RPM
		{get;set;}
		
		public double Boost
		{get;set;}
		
		public int Load
		{get;set;}
		
		public int TPS
		{get;set;}
	}
	
	[Serializable]
	public class ComPortSettings : ViewModelBase
	{
		string _comPort = string.Empty;
		public string ComPort
		{
			get{return _comPort;}
			set{_comPort = value;}
		}
		
		public bool TXSTuner
		{
			get{return this.Get<bool>("TXSTuner");}
			set{this.Set<bool>("TXSTuner",value);}
		}
		
		public string ExternalWB
		{
			get;set;
		}
						
		string _externalWBComPort = string.Empty;
		public string ExternalWBComPort
		{
			get{return _externalWBComPort;}
			set{_externalWBComPort = value;}
		}
	}
	
}
