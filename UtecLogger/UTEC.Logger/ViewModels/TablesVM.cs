/*
 * Created by SharpDevelop.
 * User: Walter
 * Date: 06/06/2011
 * Time: 00:07
 *
 * $
{
res:XML.StandardHeader.HowToChangeTemplateInformation
}
 */

using MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using UTEC.Controls;
using UTEC.Logger.Helpers;

namespace UTEC.Logger.ViewModels
{
    /// <summary>
    /// Description of TablesVM.
    /// </summary>
    public class TablesVM : ViewModelBase
    {
        private const int _rows = (9000 - 500) / 250 + 1;
        private const int _columns = 11;

        private UtecMapTableHelper UtecMapTableHelper
        { get; set; }

        public TablesVM()
        {
            this.PropertyChanged += TablesVM_PropertyChanged;

            UtecMapTableHelper = new UtecMapTableHelper(_rows, _columns);

            DataTables = TableHelper.GenDataTables(_rows, _columns);

            SelectedCellIndexes = new ObservableCollection<int[]>();
        }

        public double RichAfrOffset
        { get; set; }

        public double LeanAfrOffset
        { get; set; }

        public string MapName
        { get; set; }

        public string MapComments
        { get; set; }

        private void SelectLastTable()
        {
            this.Data = null;
            if (SelectedUtecMapTable != null)
            {
                SelectTable(SelectedUtecMapTable);
            }
            else if (SelectedDataTable != null)
            {
                SelectTable(SelectedDataTable);
            }
            else
            {
                var selectedTable = DataTables.FirstOrDefault(p => p.Key == "CELLHITS");
                SelectedDataTable = selectedTable;
            }
        }

        private void TablesVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(IsTablesPanelVisible):
                    if (this.IsTablesPanelVisible == true)
                    {
                        SelectLastTable();
                    }
                    break;

                case nameof(TargetAFRPath):
                    OpenTargetAFR(TargetAFRPath);
                    break;

                default:
                    break;
            }
        }

        private double? TryParse(string data)
        {
            double? output = null;
            double parseOutput;

            if (double.TryParse(data, out parseOutput))
            {
                output = parseOutput;
            }

            return (double?)output;
        }

        public void SetTableData(string[] data)
        {
            double? rpm = TryParse(data[0]);
            double? boost = TryParse(data[1]);
            double? mafv = TryParse(data[2]);
            double? tps = TryParse(data[3]);
            double? load = TryParse(data[4]);
            double? knock = TryParse(data[5]);
            double? ecuign = TryParse(data[7]);
            double? idc = TryParse(data[8]);
            double? ign = TryParse(data[9]);
            double? mapve = TryParse(data[10]);
            double? modmafv = TryParse(data[12]);

            int rpmTableIndex = (int)(rpm - 500) / 250;
            int loadTableIndex = (int)load / 10;

            if (rpmTableIndex < 0 || loadTableIndex < 0)
            {
                return;
            }

            if (data.Length == 14)
            {
                double? afr = TryParse(data[13]);
                TableHelper.SetTableData(TableHelper.GetTable("AFR", DataTables), rpmTableIndex, loadTableIndex, afr);
            }

            TableHelper.SetTableData(TableHelper.GetTable("IGN", DataTables), rpmTableIndex, loadTableIndex, ign);
            TableHelper.SetTableData(TableHelper.GetTable("BOOST", DataTables), rpmTableIndex, loadTableIndex, boost);
            TableHelper.SetTableData(TableHelper.GetTable("CELLHITS", DataTables), rpmTableIndex, loadTableIndex, 1);
            TableHelper.SetTableData(TableHelper.GetTable("MAFV", DataTables), rpmTableIndex, loadTableIndex, mafv);
            TableHelper.SetTableData(TableHelper.GetTable("MODMAFV", DataTables), rpmTableIndex, loadTableIndex, modmafv);
            TableHelper.SetTableData(TableHelper.GetTable("IDC", DataTables), rpmTableIndex, loadTableIndex, idc);
            TableHelper.SetTableData(TableHelper.GetTable("ECUIGN", DataTables), rpmTableIndex, loadTableIndex, ecuign);
            TableHelper.SetTableData(TableHelper.GetTable("TPS", DataTables), rpmTableIndex, loadTableIndex, tps);
            TableHelper.SetTableData(TableHelper.GetTable("KNOCK", DataTables), rpmTableIndex, loadTableIndex, knock);
            TableHelper.SetTableData(TableHelper.GetTable("MAPVE", DataTables), rpmTableIndex, loadTableIndex, mapve);
        }

        public Dictionary<string, object[]> DataTables
        {
            get { return this.Get<Dictionary<string, object[]>>(nameof(DataTables)); }
            set { this.Set<Dictionary<string, object[]>>(nameof(DataTables), value); }
        }

        public Dictionary<string, object[]> UtecMapTables
        {
            get { return this.Get<Dictionary<string, object[]>>(nameof(UtecMapTables)); }
            set { this.Set<Dictionary<string, object[]>>(nameof(UtecMapTables), value); }
        }

        public ObservableCollection<FlexColumn> Columns
        {
            get
            {
                var columns = new ObservableCollection<FlexColumn>();

                for (int i = 0; i <= 100; i += 10)
                {
                    var col = new FlexColumn() { ColumnName = i.ToString() };

                    columns.Add(col);
                }

                return columns;
            }
        }

        public ObservableCollection<FlexRow> Rows
        {
            get
            {
                var rows = new ObservableCollection<FlexRow>();

                for (int i = 500; i <= 9000; i += 250)
                {
                    var row = new FlexRow() { RowName = i.ToString() };

                    rows.Add(row);
                }

                return rows;
            }
        }

        public object Data
        {
            get { return this.Get<object>(nameof(Data)); }
            set { this.Set<object>(nameof(Data), value); }
        }

        public string ModifyButtonText
        {
            get { return this.Get<string>(nameof(ModifyButtonText)); }
            set { this.Set<string>(nameof(ModifyButtonText), value); }
        }

        public bool IsTablesPanelVisible
        {
            get { return this.Get<bool>(nameof(IsTablesPanelVisible)); }
            set
            {
                this.Set<bool>(nameof(IsTablesPanelVisible), value);
                MessageBus.Instance.Publish(new ActionItem(Actions.TABLES_FOCUS_CONTROL, this, null));
            }
        }

        public string TargetAFRPath
        {
            get { return this.Get<string>(nameof(TargetAFRPath)); }
            set { this.Set<string>(nameof(TargetAFRPath), value); }
        }

        private EditMapAction EditMapAction
        {
            get { return this.Get<EditMapAction>(nameof(EditMapAction)); }
            set { this.Set<EditMapAction>(nameof(EditMapAction), value); }
        }

        private RelayCommand _exitTablesCommand;

        public ICommand ExitTablesCommand
        {
            get
            {
                if (_exitTablesCommand == null)
                {
                    _exitTablesCommand = new RelayCommand(parm => ExitTables());
                }

                return _exitTablesCommand;
            }
        }

        private void ExitTables()
        {
            this.IsTablesPanelVisible = false;
        }

        private RelayCommand _saveMapCommand;

        public ICommand SaveMapCommand
        {
            get
            {
                if (_saveMapCommand == null)
                {
                    _saveMapCommand = new RelayCommand(parm => SaveMap());
                }

                return _saveMapCommand;
            }
        }

        private void SaveMap()
        {
            string path = Common.SaveFile();

            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            UtecMapTableHelper.SaveMap(path, UtecMapTables);
        }

        private RelayCommand _modifyMapAddCommand;

        public ICommand ModifyMapAddCommand
        {
            get
            {
                if (_modifyMapAddCommand == null)
                {
                    _modifyMapAddCommand = new RelayCommand(parm => ModifyMap(EditMapAction.Add));
                }

                return _modifyMapAddCommand;
            }
        }

        private RelayCommand _modifyMapMultiplyCommand;

        public ICommand ModifyMapMultiplyCommand
        {
            get
            {
                if (_modifyMapMultiplyCommand == null)
                {
                    _modifyMapMultiplyCommand = new RelayCommand(parm => ModifyMap(EditMapAction.Multiply));
                }

                return _modifyMapMultiplyCommand;
            }
        }

        private RelayCommand _modifyMapInterpolateVCommand;

        public ICommand ModifyMapInterpolateVCommand
        {
            get
            {
                if (_modifyMapInterpolateVCommand == null)
                {
                    _modifyMapInterpolateVCommand = new RelayCommand(parm => ModifyInterpolateVerticallySelectedCells());
                }

                return _modifyMapInterpolateVCommand;
            }
        }

        private RelayCommand _modifyMapInterpolateHCommand;

        public ICommand ModifyMapInterpolateHCommand
        {
            get
            {
                if (_modifyMapInterpolateHCommand == null)
                {
                    _modifyMapInterpolateHCommand = new RelayCommand(parm => ModifyInterpolateHorizontallySelectedCells());
                }

                return _modifyMapInterpolateHCommand;
            }
        }

        private void ModifyMap(EditMapAction editMapAction)
        {
            if (SelectedUtecMapTable == null)
                return;

            switch (editMapAction)
            {
                case UTEC.Logger.ViewModels.EditMapAction.Fill:
                    ModifyButtonText = "F";
                    break;

                case UTEC.Logger.ViewModels.EditMapAction.Add:
                    ModifyButtonText = "+";
                    break;

                case UTEC.Logger.ViewModels.EditMapAction.Multiply:
                    ModifyButtonText = "*";
                    break;

                default:
                    throw new Exception("Invalid value for EditMapAction");
            }

            EditMapAction = editMapAction;

            ActionItem action = new ActionItem(Actions.SHOW_EDITOR, this, null);
            MessageBus.Instance.Publish(action);
        }

        private RelayCommand _modifyMapActionCommand;

        public ICommand ModifyMapActionCommand
        {
            get
            {
                if (_modifyMapActionCommand == null)
                {
                    _modifyMapActionCommand = new RelayCommand(parm =>
                                                               {
                                                                   if (parm is String && !string.IsNullOrEmpty(parm as String))
                                                                   {
                                                                       double value;

                                                                       if (Double.TryParse(parm as String, out value))
                                                                       {
                                                                           ModifyMapAction(value);
                                                                       }
                                                                   }
                                                               });
                }

                return _modifyMapActionCommand;
            }
        }

        public void ModifyMapAction(double value)
        {
            if (SelectedUtecMapTable == null)
                return;

            var table = TableHelper.GetTable(((KeyValuePair<string, object[]>)SelectedUtecMapTable).Key, this.UtecMapTables);

            switch (EditMapAction)
            {
                case EditMapAction.Fill:
                    ModifyFillSelectedCells(value, table);
                    break;

                case EditMapAction.Add:
                    ModifyAddSelectedCells(value, table);
                    break;

                case EditMapAction.Multiply:
                    ModifyMultiplySelectedCells(value, table);
                    break;

                default:
                    throw new Exception("Invalid value for EditMapAction");
            }

            OnPropertyChanged(nameof(Data));
        }

        private void ModifyInterpolateVerticallySelectedCells()
        {
            if (SelectedUtecMapTable == null)
                return;

            var table = TableHelper.GetTable(((KeyValuePair<string, object[]>)SelectedUtecMapTable).Key, this.UtecMapTables);

            if (SelectedCellIndexes != null && SelectedCellIndexes.Count > 0)
            {
                List<int> columns = new List<int>();

                for (int i = 0; i < SelectedCellIndexes.Count; i++)
                {
                    var column = SelectedCellIndexes[i][1];

                    if (!columns.Contains(column))
                    {
                        columns.Add(column);
                    }
                }

                foreach (int c in columns)
                {
                    var rows = from p in SelectedCellIndexes
                               where p[1] == c
                               select p[0];

                    int minIndex = rows.Min();
                    int maxIndex = rows.Max();

                    var minIndexValue = table[minIndex, c].First();
                    var maxIndexValue = table[maxIndex, c].First();

                    if (minIndexValue <= maxIndexValue)
                    {
                        double interValue = (maxIndexValue - minIndexValue) / (rows.Count() - 1);

                        for (int index = minIndex + 1; index <= maxIndex; ++index)
                        {
                            double newVal = minIndexValue + interValue * (index - minIndex);

                            table[index, c].Clear();
                            table[index, c].Add(Math.Round(newVal, SINGLE_PRECISION));
                        }
                    }
                    else if (maxIndexValue <= minIndexValue)
                    {
                        double interValue = (minIndexValue - maxIndexValue) / (rows.Count() - 1);

                        for (int index = minIndex + 1; index <= maxIndex; ++index)
                        {
                            double newVal = minIndexValue - interValue * (index - minIndex);

                            table[index, c].Clear();
                            table[index, c].Add(Math.Round(newVal, SINGLE_PRECISION));
                        }
                    }
                }
            }

            OnPropertyChanged(nameof(Data));
        }

        private void ModifyInterpolateHorizontallySelectedCells()
        {
            if (SelectedUtecMapTable == null)
                return;

            var table = TableHelper.GetTable(((KeyValuePair<string, object[]>)SelectedUtecMapTable).Key, this.UtecMapTables);

            if (SelectedCellIndexes != null && SelectedCellIndexes.Count > 0)
            {
                List<int> rows = new List<int>();

                for (int i = 0; i < SelectedCellIndexes.Count; i++)
                {
                    var row = SelectedCellIndexes[i][0];

                    if (!rows.Contains(row))
                    {
                        rows.Add(row);
                    }
                }

                foreach (int r in rows)
                {
                    var columns = from p in SelectedCellIndexes
                                  where p[0] == r
                                  select p[1];

                    int minIndex = columns.Min();
                    int maxIndex = columns.Max();

                    var minIndexValue = table[r, minIndex].First();
                    var maxIndexValue = table[r, maxIndex].First();

                    if (minIndexValue <= maxIndexValue)
                    {
                        double interValue = (maxIndexValue - minIndexValue) / (columns.Count() - 1);

                        for (int index = minIndex + 1; index <= maxIndex; ++index)
                        {
                            double newVal = minIndexValue + interValue * (index - minIndex);

                            table[r, index].Clear();
                            table[r, index].Add(Math.Round(newVal, SINGLE_PRECISION));
                        }
                    }
                    else if (maxIndexValue <= minIndexValue)
                    {
                        double interValue = (minIndexValue - maxIndexValue) / (columns.Count() - 1);

                        for (int index = minIndex + 1; index <= maxIndex; ++index)
                        {
                            double newVal = minIndexValue - interValue * (index - minIndex);

                            table[r, index].Clear();
                            table[r, index].Add(Math.Round(newVal, SINGLE_PRECISION));
                        }
                    }
                }
            }

            OnPropertyChanged(nameof(Data));
        }

        private void ModifyMultiplySelectedCells(double value, ObservableCollection<double>[,] table)
        {
            foreach (int[] cordinate in SelectedCellIndexes)
            {
                int rpmIndex = cordinate[0];
                int loadIndex = cordinate[1];

                var cell = table[rpmIndex, loadIndex];

                double cellValue = 0;

                if (cell.Count > 0)
                {
                    cellValue = cell[0];
                    cell[0] = Math.Round(cellValue * value, SINGLE_PRECISION);
                }
                else
                {
                    cell.Add(0);
                }
            }
        }

        private void ModifyFillSelectedCells(double value, ObservableCollection<double>[,] table)
        {
            foreach (int[] cordinate in SelectedCellIndexes)
            {
                int rpmIndex = cordinate[0];
                int loadIndex = cordinate[1];

                var cell = table[rpmIndex, loadIndex];

                cell.Clear();
                cell.Add(Math.Round(value, SINGLE_PRECISION));
            }
        }

        private void ModifyAddSelectedCells(double value, ObservableCollection<double>[,] table)
        {
            foreach (int[] cordinate in SelectedCellIndexes)
            {
                int rpmIndex = cordinate[0];
                int loadIndex = cordinate[1];

                var cell = table[rpmIndex, loadIndex];

                double cellValue = 0;

                if (cell.Count > 0)
                {
                    cellValue = cell[0];
                    cell[0] = Math.Round(cellValue + value, SINGLE_PRECISION);
                }
                else
                {
                    cell.Add(value);
                }
            }
        }

        private RelayCommand _modifyMapFillCommand;

        public ICommand ModifyMapFillCommand
        {
            get
            {
                if (_modifyMapFillCommand == null)
                {
                    _modifyMapFillCommand = new RelayCommand(parm => ModifyMap(EditMapAction.Fill));
                }

                return _modifyMapFillCommand;
            }
        }

        private RelayCommand _openMapCommand;

        public ICommand OpenMapCommand
        {
            get
            {
                if (_openMapCommand == null)
                {
                    _openMapCommand = new RelayCommand(parm => OpenMap());
                }

                return _openMapCommand;
            }
        }

        private void OpenMap()
        {
            string path = Common.SelectFile("text files|*.txt");
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            UtecMapTables = UtecMapTableHelper.LoadMap(path);

            var fuelMap = UtecMapTables.FirstOrDefault(p => p.Key == "MapFuel");
            SelectedUtecMapTable = fuelMap;
        }

        private RelayCommand _autoTuneMapCommand;

        public ICommand AutoTuneMapCommand
        {
            get
            {
                if (_autoTuneMapCommand == null)
                {
                    _autoTuneMapCommand = new RelayCommand(parm => AutoTuneMap());
                }

                return _autoTuneMapCommand;
            }
        }

        private void AutoTuneMap()
        {
            var afrTable = DataTables["AFR"][0] as ObservableCollection<double>[,];
            var veTable = DataTables["MAPVE"][0] as ObservableCollection<double>[,];
            var targetAfrTable = DataTables["TARGETAFR"][0] as ObservableCollection<double>[,];
            var autoTuneTable = UtecMapTables["MapFuel"][0] as ObservableCollection<double>[,];

            AutoTune(false,
                     afrTable,
                     veTable,
                     targetAfrTable,
                     autoTuneTable);

            var fuelMap = UtecMapTables.FirstOrDefault(p => p.Key == "MapFuel");
            SelectedUtecMapTable = fuelMap;
        }

        public ObservableCollection<int[]> SelectedCellIndexes
        {
            get { return this.Get<ObservableCollection<int[]>>(nameof(SelectedCellIndexes)); }
            set { this.Set<ObservableCollection<int[]>>(nameof(SelectedCellIndexes), value); }
        }

        public object SelectedUtecMapTable
        {
            get { return this.Get<object>(nameof(SelectedUtecMapTable)); }
            set
            {
                this.Set<object>(nameof(SelectedUtecMapTable), value);

                if (SelectedDataTable != null && value != null)
                {
                    SelectedDataTable = null;
                }

                if (value != null)
                {
                    SelectTable(value);
                }
            }
        }

        public object SelectedDataTable
        {
            get
            {
                return this.Get<object>(nameof(SelectedDataTable));
            }

            set
            {
                this.Set<object>(nameof(SelectedDataTable), value);

                if (SelectedUtecMapTable != null && value != null)
                {
                    SelectedUtecMapTable = null;
                }

                if (value != null)
                {
                    SelectTable(value);
                }
            }
        }

        private void SelectTable(object parm)
        {
            if (parm != null)
            {
                if ("AUTOTUNE" == ((KeyValuePair<string, object[]>)parm).Key)
                {
                    ShowAutoTune();
                }
                else
                {
                    var selectedTable = ((KeyValuePair<string, object[]>)parm).Value;
                    ShowTable(selectedTable);
                }
            }
        }

        private void ShowTable(object[] table)
        {
            Data = table;
        }

        private RelayCommand _clearTablesCommand;

        public ICommand ClearTablesCommand
        {
            get
            {
                if (_clearTablesCommand == null)
                {
                    _clearTablesCommand = new RelayCommand(parm => ClearTables());
                }

                return _clearTablesCommand;
            }
        }

        private void ClearTables()
        {
            foreach (string key in DataTables.Keys)
            {
                if (key != "TARGETAFR")
                {
                    TableHelper.ClearTable(TableHelper.GetTable(key, DataTables));
                }
            }

            OnPropertyChanged(nameof(Data));
        }

        private RelayCommand _autoTuneCommand;

        public ICommand AutoTuneCommand
        {
            get
            {
                if (_autoTuneCommand == null)
                {
                    _autoTuneCommand = new RelayCommand(parm => ShowAutoTune());
                }

                return _autoTuneCommand;
            }
        }

        private const int DOUBLE_PRECISION = 2;
        private const int SINGLE_PRECISION = 1;

        private double GetDoubleAverage(ObservableCollection<double> cellData)
        {
            return Math.Round(cellData.Average(), DOUBLE_PRECISION);
        }

        private void AutoTuneSDCalc(int i, int j,
                                    ObservableCollection<double>[,] afrTable,
                                    ObservableCollection<double>[,] veTable,
                                    ObservableCollection<double>[,] targetAfrTable,
                                    ObservableCollection<double>[,] autoTuneTable,
                                    bool clearCell)
        {
            double afr = GetDoubleAverage(afrTable[i, j]);
            double ve = GetDoubleAverage(veTable[i, j]);
            double targetAfR = targetAfrTable[i, j].FirstOrDefault();

            double richOffset = targetAfR - this.RichAfrOffset;
            double leanOffset = targetAfR + this.LeanAfrOffset;

            if (clearCell)
            {
                autoTuneTable[i, j].Clear();
            }

            if (afr > richOffset && afr < leanOffset)
            {
                return;
            }

            //Calculated VE
            double autotune = Math.Round(ve * afr / targetAfR - 100, DOUBLE_PRECISION);

            autoTuneTable[i, j].Clear();
            autoTuneTable[i, j].Add(autotune);
        }

        private void ShowAutoTune()
        {
            var afrTable = DataTables["AFR"][0] as ObservableCollection<double>[,];
            var veTable = DataTables["MAPVE"][0] as ObservableCollection<double>[,];
            var targetAfrTable = DataTables["TARGETAFR"][0] as ObservableCollection<double>[,];
            var autoTuneTable = DataTables["AUTOTUNE"][0] as ObservableCollection<double>[,];

            AutoTune(true, afrTable, veTable, targetAfrTable, autoTuneTable);
            Data = DataTables["AUTOTUNE"];
        }

        private void AutoTune(bool clearCell,
                      ObservableCollection<double>[,] afrTable,
                      ObservableCollection<double>[,] veTable,
                      ObservableCollection<double>[,] targetAfrTable,
                      ObservableCollection<double>[,] autoTuneTable)
        {
            for (int i = 0; i <= afrTable.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= afrTable.GetUpperBound(1); j++)
                {
                    if (afrTable[i, j].Count > 0 &&
                       veTable[i, j].Count > 0 &&
                       targetAfrTable[i, j].Count > 0)
                    {
                        AutoTuneSDCalc(i, j, afrTable, veTable, targetAfrTable, autoTuneTable, clearCell);
                    }
                }
            }
        }

        public void OpenTargetAFR(string path)
        {
            if (!File.Exists(path))
            {
                throw new Exception(String.Format("{0} does not exist.", path));
            }
            using (StreamReader sr = File.OpenText(path))
            {
                var afrTargetTable = DataTables["TARGETAFR"][0] as ObservableCollection<double>[,];

                string input;
                while ((input = sr.ReadLine()) != null)
                {
                    string[] targetAFRData = input.Split(",".ToCharArray());

                    if (input.Length < 12)
                    {
                        throw new Exception(String.Format("Error reading target afr file {0}, ensure it is formatted correctly.", path));
                    }

                    double rpm = 0;
                    double.TryParse(targetAFRData[0], out rpm);

                    int rpmIndex = (int)(rpm - 500) / 250;

                    for (int i = 1; i < 12; i++)
                    {
                        double targetAFR = 0;
                        double.TryParse(targetAFRData[i], out targetAFR);

                        afrTargetTable[rpmIndex, i - 1].Clear();
                        afrTargetTable[rpmIndex, i - 1].Add(targetAFR);
                    }
                }
            }
        }
    }

    internal enum EditMapAction
    {
        Fill,
        Add,
        Multiply
    }
}