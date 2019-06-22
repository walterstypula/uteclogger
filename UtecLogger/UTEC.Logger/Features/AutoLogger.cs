/*
 * Created by SharpDevelop.
 * User: Walter
 * Date: 5/30/2011
 * Time: 10:59 AM
 * 
 * $
{
res:XML.StandardHeader.HowToChangeTemplateInformation
}
 */
using System;
using System.IO;
using System.Text;

using UTEC.Logger.Model;

namespace UTEC.Logger.Features
{
	/// <summary>
	/// Description of 
	/// Logger.
	/// </summary>
	public class AutoLogger
	{
		private LogTrigger _logStartTrigger;
		private LogTrigger _logSaveTrigger;
		private int _logKnockThreshold = 0;
		private int _maxRPM = 0;
		private int _maxTPS = 0;
		private bool _autoLogKnock = false;
		private string _logSavePath = String.Empty;
		
		StringBuilder _log = new StringBuilder();
		StringBuilder _manualLog = new StringBuilder();
		bool _startLogging = false;
		bool _shouldLogSave = false;
		bool _knocked = false;
				
		public AutoLogger(AutoLogSettings autoLogSettings, string logSavePath)
		{
			_logStartTrigger = autoLogSettings.AutoLogConditions;
			_logSaveTrigger = autoLogSettings.LogSaveConditions;
			_logKnockThreshold = autoLogSettings.KnockThreshold;
			_autoLogKnock = autoLogSettings.AutoLogKnock;
			
			_logSavePath = logSavePath.Trim(@"\".ToCharArray());
			_logSavePath = _logSavePath.Trim();
		}
		
		public bool LogData(string[] data, string dataString)
		{	
			int rpm = 0;
			int load = 0;
			int tps = 0;
			int boost = 0;
			int knock = 0;
			
			int.TryParse(data[0], out rpm);
			int.TryParse(data[4], out load);
			int.TryParse(data[3], out tps);
			int.TryParse(data[1], out boost);
			int.TryParse(data[5], out knock);
			
			//record max tps and rpm
			_maxRPM = rpm > _maxRPM ? rpm : _maxRPM;
			_maxTPS = tps > _maxTPS ? tps : _maxTPS;
						
			if(rpm >= _logStartTrigger.RPM 
			   && load >= _logStartTrigger.Load
			   && tps >= _logStartTrigger.TPS
			   && boost >= _logStartTrigger.Boost
			   && _startLogging == false)
			{
				//AUTO LOG THRESHOLD
				_startLogging = true;
			}
			
			if(rpm >= _logSaveTrigger.RPM 
			   && load >= _logSaveTrigger.Load
			   && tps >= _logSaveTrigger.TPS
			   && boost >= _logSaveTrigger.Boost 
			   && _startLogging == true
			   && _shouldLogSave == false)
			{
				//SAVE LOG THRESHOLDS REACHED
				_shouldLogSave = true;
			}
			
			
			if(_autoLogKnock && knock >= _logKnockThreshold)
			{
				//KNOCK DETECTED
				_knocked = true;
			}
			
			
			if(_startLogging 
			   && _shouldLogSave == false
			   && tps < _maxTPS-10
			   && rpm < _maxRPM)
			{
				//RESET LOG SAVE, LOG SAVE THRESHOLDS NOT REARCHED
				_startLogging = false;
				_shouldLogSave = false;
				_maxTPS = 0;
				_maxRPM = 0;
			}
			
			if(_startLogging 
			   && _shouldLogSave
			   && tps < _maxTPS-10
			   && rpm < _maxRPM)
			{
				//SAVE LOG
				SavePullReport();
				_startLogging = false;
				_shouldLogSave = false;
				_maxTPS = 0;
				_maxRPM = 0;
				
				if(_knocked)
				{
					//SAVE KNOCK REPORT
					SaveKnockReport();
					_knocked= false;
				} 				
			}
			
			if(_startLogging == false && _shouldLogSave == false && _knocked == false)
			{
				//Clear log
				_log.Clear();
			}
			
			if(_startLogging)
			{
				_log.AppendLine(dataString);
				return true;
			}
			
			return false;
		}
		
		public void ManualLog(string s)
		{	
			_manualLog.AppendLine(s);
		}
		
		public void SaveManualLog()
		{
			string path = FormatPath("ManualReport");
			WriteLog(path, _manualLog);
			_manualLog.Clear();
		}
		
		private void SaveKnockReport()
		{
			string path =FormatPath("KnockReport");
			WriteLog(path, _log);
		}
		
		private void SavePullReport()
		{
			string path = FormatPath("PullReport");
			WriteLog(path, _log);
		}
		
		private string FormatPath(string logType)
		{
			return String.Format(@"{0}\{1}-{2}.{3}",_logSavePath,logType ,String.Format("{0:s}", DateTime.Now).Replace(":","-").Replace("T","_"),"txt");
		}
		
		private void WriteLog(string path, StringBuilder log)
		{
			TextWriter tw = new StreamWriter(path);
            tw.WriteLine(log.ToString());
            tw.Close();
		}
	}
}
