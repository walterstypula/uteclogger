/*
 * Created by SharpDevelop.
 * User: Walter
 * Date: 05/28/2011
 * Time: 11:21
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using MVVM;
using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using UTEC.DAL;
using UTEC.Logger.Features;

namespace UTEC.Logger.ViewModels
{
    /// <summary>
    /// Description of MainWindowVM.
    /// </summary>
    public class MainWindowVM : ViewModelBase
    {
        #region Constants

        private const string CONNECT = "Connect";
        private const string DISCONNECT = "Disconnect";
        private const string MANUAL_LOG = "Manual Log Start";
        private const string STOP_MANUAL_LOG = "Manual Log Stop";

        #endregion Constants

        #region Private Fields

        private UtecWideBandConnection _utecConn;
        private DashBoardVM _dashBoard;
        private UserSettingsVM _userSettings;
        private TablesVM _tables;
        private AutoLogger _logger;
        private string _connectBtnText = CONNECT;
        private string _manualLogBtnText = MANUAL_LOG;
        private bool _manualLogRunning = false;
        private bool _tablePullsOnly = false;
        private int _dataLength = 14;

        #endregion Private Fields

        #region Constructors
		public MainWindowVM(string path, ShowErrorHandler errorHandler)
		{
			this.ShowError+= errorHandler;
            
            _userSettings = new UserSettingsVM(path);
            _dashBoard = new DashBoardVM();
            _tables = new TablesVM();
            _utecConn = new UtecWideBandConnection();

            _utecConn.DataReceived += UtecConn_DataReceived;
            _utecConn.ErrorReceived += _utecConn_ErrorReceived;
            _userSettings.PropertyChanged += VM_PropertyChanged;

            _utecConn.TXSTunerPresent = _userSettings.Settings.ComPortSettings.TXSTuner;

            _utecConn.ExternalWB = _userSettings.Settings.ComPortSettings.ExternalWB;
            _utecConn.ExternalWBComPort = _userSettings.Settings.ComPortSettings.ExternalWBComPort;

            _tablePullsOnly = this.UserSettings.Settings.AutoLogSettings.TablePullsOnly;
            _tables.LeanAfrOffset = this.UserSettings.Settings.AutoLogSettings.LeanAfrOffset;
            _tables.RichAfrOffset = this.UserSettings.Settings.AutoLogSettings.RichAfrOffset;

            try
            {
                _tables.TargetAFRPath = _userSettings.Settings.GeneralSettings.AfrTargetTable;
            }
            catch (Exception ex)
            {
                this.OnShowError(ex, "");
            }
        }

        private void _utecConn_ErrorReceived(object sender, Exception ex)
        {
            this.OnShowError(ex, "Error Reading Data");
        }

        private void VM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(UserSettings.IsSettingsPanelVisible):
                    if (_userSettings.IsSettingsPanelVisible == false)
                    {
                        _utecConn.TXSTunerPresent = _userSettings.Settings.ComPortSettings.TXSTuner;

                        _utecConn.ExternalWB = _userSettings.Settings.ComPortSettings.ExternalWB;
                        _utecConn.ExternalWBComPort = _userSettings.Settings.ComPortSettings.ExternalWBComPort;

                        _tablePullsOnly = this.UserSettings.Settings.AutoLogSettings.TablePullsOnly;
                        _tables.LeanAfrOffset = this.UserSettings.Settings.AutoLogSettings.LeanAfrOffset;
                        _tables.RichAfrOffset = this.UserSettings.Settings.AutoLogSettings.RichAfrOffset;

                        try
                        {
                            _tables.TargetAFRPath = _userSettings.Settings.GeneralSettings.AfrTargetTable;
                        }
                        catch (Exception ex)
                        {
                            this.OnShowError(ex, "");
                        }
                    }
                    break;

                default:

                    break;
            }
        }

        #endregion Constructors

        #region Public Properties

        public UtecWideBandConnection UtecConn
        {
            get { return _utecConn; }
        }

        public DashBoardVM DashBoard
        {
            get { return _dashBoard; }
        }

        public UserSettingsVM UserSettings
        {
            get { return _userSettings; }
        }

        public TablesVM Tables
        {
            get { return _tables; }
        }

        public string ConnectBtnText
        {
            get { return _connectBtnText; }
            set
            {
                _connectBtnText = value;
                OnPropertyChanged(nameof(ConnectBtnText));
            }
        }

        public string ManualLogBtnText
        {
            get { return _manualLogBtnText; }
            set
            {
                _manualLogBtnText = value;
                OnPropertyChanged(nameof(ManualLogBtnText));
            }
        }

        #endregion Public Properties

        #region Commands

        private RelayCommand _showTablesCommand;

        public ICommand ShowTablesCommand
        {
            get
            {
                if (_showTablesCommand == null)
                {
                    _showTablesCommand = new RelayCommand(parm => ShowTablesPanel());
                }

                return _showTablesCommand;
            }
        }

        private RelayCommand _closeCommand;

        public ICommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                {
                    _closeCommand = new RelayCommand(parm => OnRequestClose());
                }

                return _closeCommand;
            }
        }

        private RelayCommand _showSettingsPanelCommand;

        public ICommand ShowSettingsPanelCommand
        {
            get
            {
                if (_showSettingsPanelCommand == null)
                {
                    _showSettingsPanelCommand = new RelayCommand(parm => ShowSettingsPanel());
                }

                return _showSettingsPanelCommand;
            }
        }

        private RelayCommand _connectCommand;

        public ICommand ConnectCommand
        {
            get
            {
                if (_connectCommand == null)
                {
                    _connectCommand = new RelayCommand(parm => Connect());
                }

                return _connectCommand;
            }
        }

        private RelayCommand _manualLogCommand;

        public ICommand ManualLogCommand
        {
            get
            {
                if (_manualLogCommand == null)
                {
                    _manualLogCommand = new RelayCommand(parm => ManualLog());
                }

                return _manualLogCommand;
            }
        }

        #endregion Commands

        #region Private Methods

        private void ShowSettingsPanel()
        {
            this.UserSettings.IsSettingsPanelVisible = true;
        }

        private void ShowTablesPanel()
        {
            this.Tables.IsTablesPanelVisible = true;
        }

        private void Connect()
        {
            string comPort = _userSettings.Settings.ComPortSettings.ComPort;
            bool fastLogging = _userSettings.Settings.AutoLogSettings.FastLogging;
            int linesPerSecond = _userSettings.Settings.AutoLogSettings.LinesPerSecond;

            if (_utecConn == null || _utecConn.IsOpen == false)
            {
                try
                {
                    _utecConn.OpenConnection(comPort, fastLogging, linesPerSecond, false);
                    _logger = new AutoLogger(_userSettings.Settings.AutoLogSettings, _userSettings.Settings.GeneralSettings.LogOutputDirectory);
                    this.ConnectBtnText = DISCONNECT;
                }
                catch (Exception ex)
                {
                    _utecConn.CloseConnnection();
                    this.ConnectBtnText = CONNECT;
                    this.OnShowError(ex, "Error establishing connection.");
                }
            }
            else
            {
                _utecConn.CloseConnnection();
                this.ConnectBtnText = CONNECT;
            }
        }

        private void ManualLog()
        {
            if (_manualLogRunning)
            {
                if (_logger != null)
                {
                    _logger.SaveManualLog();
                }
                _manualLogRunning = false;
                this.ManualLogBtnText = MANUAL_LOG;
            }
            else
            {
                _manualLogRunning = true;
                this.ManualLogBtnText = STOP_MANUAL_LOG;
            }
        }

        private void UtecConn_DataReceived(object sender, string e)
        {
            try
            {
                e = FormatData(e);

                string[] data = e.Split(",".ToCharArray());

                if (data.Length < _dataLength)
                {
                    return;
                }

                this.DashBoard.UtecData = e;
                this.DashBoard.UtecDataArray = data;

                if (_logger != null)
                {
                    bool isPull = _logger.LogData(data, e);

                    if ((_tablePullsOnly && isPull) || _tablePullsOnly == false)
                    {
                        this.Tables.SetTableData(data);
                    }

                    if (_manualLogRunning)
                    {
                        _logger.ManualLog(e);
                    }
                }
            }
            catch (Exception ex)
            {
                OnShowError(ex, e);
            }
        }

        private string FormatData(string e)
        {
            e = e.Trim();
            while (e.Contains("  "))
            {
                e = e.Replace("  ", " ");
            }

            e = e.Replace(" ", ",");

            return e;
        }

        #endregion Private Methods

        #region Events

        public event EventHandler RequestClose;

        private void OnRequestClose()
        {
            EventHandler handler = this.RequestClose;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        public delegate void ShowErrorHandler(object sender, ErrorArgs args);

        public event ShowErrorHandler ShowError;

        private void OnShowError(Exception ex, string utecData)
        {
            if (this.ShowError != null)
                this.ShowError(this, new ErrorArgs(ex, utecData));
        }

        #endregion Events

        public void ApplicationExitCloseConnections()
        {
            _utecConn.CloseConnnection();
        }
    }

    public class ErrorArgs : EventArgs
    {
        private Exception _exception = null;
        private string _utecData = string.Empty;

        public ErrorArgs(Exception exception, string utecData)
        {
            _exception = exception;
            _utecData = utecData;
        }

        public Exception Exception
        {
            get { return _exception; }
        }

        public string UtecData
        {
            get { return _utecData; }
        }
    }
}