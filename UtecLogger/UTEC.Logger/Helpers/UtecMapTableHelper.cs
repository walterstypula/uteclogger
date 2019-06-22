/*
 * Created by SharpDevelop.
 * User: Walter
 * Date: 09/28/2011
 * Time: 21:54
 * 
 * $
{
res:XML.StandardHeader.HowToChangeTemplateInformation
}
 */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

using UTEC.Logger.ViewModels;

namespace UTEC.Logger.Helpers
{
	/// <summary>
	/// Description of UtecMapHelper.
	/// </summary>
	public class UtecMapTableHelper
	{
			
		
		
		
		public UtecMapTableHelper(int rows, int columns)
		{
			_rows = rows;
			_columns = columns;
		}
		
		int _rows = 0;
		int _columns = 0;
			
		public string MapName{get;set;}
		public string MapComments{get;set;}
		
		
		private void PlotUtecMap(string s, int row, ObservableCollection<double>[,] table)
		{
			if (s != null)
			{
				s = RemoveDoubleSpace(s);

				string[] arr = s.Split(" ".ToCharArray());

				for (int i = 0; i < arr.Length; i++)
				{
					double cellvalue = Convert.ToDouble(arr[i].Trim("[]".ToCharArray()));
					TableHelper.SetTableData(table,row,i,cellvalue);
				}
			}
		}
		
		private string RemoveDoubleSpace(string s)
		{
			if (s.IndexOf("  ") > -1)
			{
				s = s.Replace("  ", " ");
				s = RemoveDoubleSpace(s);
			}

			return s.Trim();
		}
		
		
		public Dictionary<string,object[]> LoadMap(String path)
		{
			Dictionary<string,object[]> mapTables = new Dictionary<string,object[]>();
			
			foreach (Table table in TableHelper.MapTableNames)
			{
				mapTables.Add(table.TableName,new object[]{TableHelper.CreateTable(_rows,_columns), table.TableOperation});
			}	
			
			StreamReader sr;
			string s;
			sr = File.OpenText(path);
			s = sr.ReadLine();

			string tableName = string.Empty;
			
			Int32 row = 0;
			while (s != null)
			{
				s = sr.ReadLine();
				if (!string.IsNullOrEmpty(s))
				{
					switch (s)
					{
						case "Fuel Map":
							tableName = "MapFuel";
							TableHelper.ClearTable(TableHelper.GetTable(tableName, mapTables));
							row = 1;
							break;
						case "Timing Map":
							tableName = "MapTiming";
							TableHelper.ClearTable(TableHelper.GetTable(tableName, mapTables));
							row = 1;
							break;
						case "Boost Map":
							tableName = "MapBoost";
							TableHelper.ClearTable(TableHelper.GetTable(tableName, mapTables));
							row = 1;
							break;
						default:
							if (s != null)
							{
								if (s.Substring(0, 10) == "Map Name:-")
								{
									MapName = s.Substring(10, s.Length - 10).Trim("[]".ToCharArray());
								}
								else if (s.Substring(0, 14) == "Map Comments:-")
								{
									MapComments = s.Substring(14, s.Length - 14).Trim("[]".ToCharArray());
								}

								if (!string.IsNullOrEmpty(tableName))
								{
									if (row >= 3 && row <= 37)
									{
										PlotUtecMap(s, row - 3, TableHelper.GetTable(tableName, mapTables));
									}
									row++;
								}
							}
							
							break;
					}
				}
			}
			sr.Close();
			
			return mapTables;
		}
		
		public void SaveMap(string path, Dictionary<string,object[]> saveMap)
		{
			using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, false))
			{
				file.WriteLine("[START][MAPGROUP1][MAPGROUP]");
				file.WriteLine(String.Format("Map Name:-[{0}]", MapName));
				file.WriteLine(String.Format("Map Comments:-[{0}]", MapComments));
				file.WriteLine("Fuel Map");
				WriteTable(file, TableHelper.GetTable("MapFuel", saveMap), "0");
				file.WriteLine();
				file.WriteLine("Timing Map");
				WriteTable(file, TableHelper.GetTable("MapTiming", saveMap), "-100");
				file.WriteLine();
				file.WriteLine("Boost Map");
				WriteTable(file, TableHelper.GetTable("MapBoost", saveMap), "0");
				file.WriteLine(String.Format("[END][MAPGROUP][{0}][EOF]", CalcCheckSum(saveMap)));
			}
		}
		
		
		private string CalcCheckSum(Dictionary<string,object[]> saveMap)
		{

			Int32 fuel = 0;
			Int32 timing = 0;
			Int32 boost = 0;

			for (int j = 0; j < _rows ; j++)
			{
				for (int i = 0; i < _columns; i++)
				{
					
					var mapFuel = saveMap["MapFuel"][0] as ObservableCollection<double>[,];
					var mapTiming = saveMap["MapTiming"][0] as ObservableCollection<double>[,];
					var mapBoost = saveMap["MapBoost"][0] as ObservableCollection<double>[,];
					
					fuel += Convert.ToInt32(mapFuel[j,i].FirstOrDefault() * 10);
					timing += Convert.ToInt32(mapTiming[j, i].FirstOrDefault() * 10);
					boost += Convert.ToInt32(mapBoost[j, i].FirstOrDefault() * 100);
				}
			}

			Double total = fuel + timing + boost + 39888;

			System.Text.Encoding ascii = System.Text.Encoding.ASCII;

			Byte[] encodedBytes = ascii.GetBytes(MapComments + MapName);
			foreach (Byte b in encodedBytes)
			{
				total += b;
			}

			String hex = Convert.ToInt32(total).ToString("X");


			return String.Format("0{0}", hex.Substring(hex.Length - 4, 4));
		}
		
		
		private void WriteNonEditValues(Int32 lines, StreamWriter file, String value)
		{
			for (int i = 0; i < lines; i++)
			{
				if (value == "0")
				{
					file.WriteLine("      [0]      [0]      [0]      [0]      [0]      [0]      [0]      [0]      [0]      [0]      [0]");
				}
				else if (value == "-100")
				{
					file.WriteLine("   [-100]   [-100]   [-100]   [-100]   [-100]   [-100]   [-100]   [-100]   [-100]   [-100]   [-100]");
				}
			}
		}

		private void WriteTable(StreamWriter file, ObservableCollection<double>[,] table, string nonEditableValue)
		{

			WriteNonEditValues(2, file, nonEditableValue);

			for (int j = 0; j < _rows; j++)
			{
				for (int i = 0; i < _columns; i++)
				{
					//var table = tables[tableName][0] as ObservableCollection<double>[,];
					
					String value = String.Format("[{0}]",  Math.Round(table[j, i].FirstOrDefault(),1));
					String padder = String.Empty;
					Int32 padding = 9 - value.Length;
					for (int pad = 0; pad < padding; ++pad)
					{
						padder = String.Format("{0}{1}", padder, " ");
					}

					value = String.Format("{0}{1}", padder, value);

					if (i == 10)
					{
						file.Write(value + Environment.NewLine);
					}
					else
					{
						file.Write(value);
					}
				}
			}

			WriteNonEditValues(3, file, nonEditableValue);
		}
	}
}
