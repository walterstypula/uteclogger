/*
 * Created by SharpDevelop.
 * User: Walter
 * Date: 09/28/2011
 * Time: 22:28
 * 
 * $
{
res:XML.StandardHeader.HowToChangeTemplateInformation
}
 */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using UTEC.Logger.ViewModels;

namespace UTEC.Logger.Helpers
{
	/// <summary>
	/// Description of TableHelper.
	/// </summary>
	internal class TableHelper
	{
		
		private static Table[] _mapTableNames = new Table[]
		{
			new Table("MapFuel", ""),
			new Table("MapBoost", ""),
			new Table("MapTiming", "")
		};
		
		
		private static 	Table[] _dataTableNames = new Table[]
		{
			new Table("CELLHITS","COUNT"),
			new Table("BOOST","AVG"),
			new Table("TPS","AVG"),
			new Table("KNOCK","SUM"),	
			new Table("IGN","AVG"),
			new Table("ECUIGN","AVG"),
			new Table("MAFV", "AVG"),
			new Table("MODMAFV", "AVG"),
			new Table("IDC","AVG"),
			new Table("AFR", "AVG"),
			new Table("TARGETAFR", ""),			
			new Table("MAPVE","AVG"),
			new Table("AUTOTUNE", "")
		};
		
		internal static Table[] MapTableNames
		{
			get{return _mapTableNames;}
		}
		
		internal static Table[] DataTableNames
		{
			get{return _dataTableNames;}
		}
		
		
		internal static void SetTableData(ObservableCollection<double>[,] table, int rpmTableIndex, int loadTableIndex, double? value)
		{
			if(value != null && value.HasValue)
			{
				table[rpmTableIndex,loadTableIndex].Add(value.Value);
			}
		}
		
		
		
		internal static void ClearTable(ObservableCollection<double>[,] table)
		{
			for(int i=0; i <= table.GetUpperBound(0); i++)
			{
				for(int j=0; j <= table.GetUpperBound(1); j++)
				{
					table[i,j].Clear();
				}
			}
		}
		
		internal static ObservableCollection<double>[,] GetTable(string tableName, Dictionary<string,object[]> tables)
		{
			var table = tables[tableName][0] as ObservableCollection<double>[,];
			
			return table;
		}
		
		internal static  ObservableCollection<double>[,] CreateTable(int rows, int columns)
		{
			ObservableCollection<double>[,] table = new ObservableCollection<double>[rows,columns];
			
			for(int r =0; r < rows; r++)
			{
				for(int c = 0; c < columns; c++)
				{
					table[r,c] = new ObservableCollection<double>();
				}
			}
			
			return table;
		}
		
		internal static Dictionary<string,object[]> GenDataTables(int rows, int columns)
		{
			Dictionary<string,object[]> dataTables = new Dictionary<string,object[]>();
			
			foreach (Table table in TableHelper.DataTableNames)
			{
				dataTables.Add(table.TableName,new object[]{TableHelper.CreateTable(rows,columns), table.TableOperation});
			}
			
			return dataTables;
		}

	}
	
	public class Table
	{
		public Table(string tableName, string tableOperation)
		{
			TableName = tableName;
			TableOperation = tableOperation;
		}
		
		public string TableName{get;set;}
		public string TableOperation{get;set;}
	}
}
