/*
 * Created by SharpDevelop.
 * User: Walter
 * Date: 05/28/2011
 * Time: 10:54
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Xml.Serialization;

using MVVM;
using UTEC.DAL;
using UTEC.Logger.Model;

namespace UTEC.Logger.ViewModels
{
	/// <summary>
	/// Description of UserSettingsVM.
	/// </summary>
	public class UserSettingsVM : ViewModelBase
	{		
		#region Constructors
		public UserSettingsVM(string path)
		{
			this.PropertyChanged+= UserSettingsVM_PropertyChanged;
			
			_path = path;
			this.Settings = LoadSettings(path);
			_linesPerSecondOptions.Add(10);
			_linesPerSecondOptions.Add(15);
			_linesPerSecondOptions.Add(20);
			_linesPerSecondOptions.Add(25);
		}

		void UserSettingsVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case "IsSettingsPanelVisible":
					OnPropertyChanged("ComPorts");
					break;
				default:
					
					break;
			}
		}

		#endregion
		
		#region Private Fields
		private string _path = string.Empty;

		List<int> _linesPerSecondOptions = new List<int>();
		#endregion
		
		#region Public Properties
		
		public List<string> WBDataSources
		{
			get{return GetWBDataSources();}
		}
		
		List<string> GetWBDataSources()
		{
			return new List<string>()
			{
				"PLX",
				"LM1",
				"LC1",
				"LM2",
				"ZT2",
				"AEM",
				"No Wideband"
			};
		}
		
		public List<int> LinesPerSecondOptions
		{
			get{return _linesPerSecondOptions;}
		}
		
		public bool IsSettingsPanelVisible
		{
			get{return this.Get<bool>("IsSettingsPanelVisible");}
			set{this.Set<bool>("IsSettingsPanelVisible",value);}
		}
		
		public string[] ComPorts
		{
			get{return SerialPortHelper.GetPortNames();}
		}
		
		public Settings Settings
		{
			get{return this.Get<Settings>("Settings");}
			set{this.Set<Settings>("Settings",value);}
		}
		#endregion
		
		#region Private Methods
		
		private void SaveSettings(string path, Settings settings)
		{
			FileStream fs = new FileStream(path, FileMode.Create);
			XmlSerializer xs = new XmlSerializer(typeof(Settings));
			xs.Serialize(fs, settings);
			fs.Close();
			
			this.IsSettingsPanelVisible = false;
		}
		
		private Settings LoadSettings(string path)
		{
			if(!File.Exists(path))
			{
				return GenerateDefaultSettings(path);
			}
			
			FileStream fs = new FileStream(path, FileMode.Open);
			try
			{
				XmlSerializer xs = new XmlSerializer(typeof(Settings));
				Settings settings = (Settings)xs.Deserialize(fs);
				fs.Close();
				return settings;
			}
			catch(Exception)
			{
				fs.Close();
				return GenerateDefaultSettings(path);
			}
		}
		
		private void CancelSettings()
		{
			this.Settings = LoadSettings(this._path);
			this.IsSettingsPanelVisible = false;
		}
		
		private void ShowFilePicker(object param)
		{
			
			
			switch (param.ToString())
			{
				case "PERSISTENT_TABLES":
					string persistentTables = Common.SelectFile("CSV |*.csv");
					if(!String.IsNullOrEmpty(persistentTables))
					{
						this.Settings.GeneralSettings.PersistentTables = persistentTables;
					}
					break;
				case "AFR_TARGET_TABLE":
					string afrTargetTable = Common.SelectFile("CSV |*.csv");
					if(!String.IsNullOrEmpty(afrTargetTable))
					{
						this.Settings.GeneralSettings.AfrTargetTable = afrTargetTable;
					}
					break;
				case "LOG_OUTPUT_DIRECTORY":
					string logOutputPath = Common.SelectDirectory();
					if(!String.IsNullOrEmpty(logOutputPath))
					{
						this.Settings.GeneralSettings.LogOutputDirectory = logOutputPath;
					}
					break;
				default:
					break;
			}
		}
		
		private Settings GenerateDefaultSettings(string path)
		{
			Settings settings = new Settings();
			SaveSettings(path,settings);
			return settings;
		}
		#endregion
		
		#region Commands
		private RelayCommand _showFilePickerCommand;
		public ICommand ShowFilePickerCommand
		{
			get
			{
				if(_showFilePickerCommand == null)
				{
					_showFilePickerCommand = new RelayCommand(parm => ShowFilePicker(parm));
				}
				
				return _showFilePickerCommand;
			}
		}
		
		private RelayCommand _saveSettingsCommand;
		public ICommand SaveSettingsCommand
		{
			get
			{
				if(_saveSettingsCommand == null)
				{
					_saveSettingsCommand = new RelayCommand(parm => SaveSettings(this._path, this.Settings));
				}
				
				return _saveSettingsCommand;
			}
		}
		
		private RelayCommand _cancelSettingsCommand;
		public ICommand CancelSettingsCommand
		{
			get
			{
				if(_cancelSettingsCommand == null)
				{
					_cancelSettingsCommand = new RelayCommand(parm => CancelSettings());
				}
				
				return _cancelSettingsCommand;
			}
		}
		#endregion
		
		
	}
}
