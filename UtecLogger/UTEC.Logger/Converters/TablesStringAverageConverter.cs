/*
 * Created by SharpDevelop.
 * User: Walter
 * Date: 06/06/2011
 * Time: 22:39
 * 
 * $
{
res:XML.StandardHeader.HowToChangeTemplateInformation
}
 */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;

namespace UTEC.Logger.Converters
{
	/// <summary>
	/// Description of TablesStringAverageConverter.
	/// </summary>
	public class TablesStringAverageConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var data = value as object[];
			
			if(data == null)
			{
				return Binding.DoNothing;
			}
			
			var input = data[0] as ObservableCollection<double>[,];
			var operationType = data[1];

			if(input == null || operationType == null)
			{
				return Binding.DoNothing;
			}
			
									
			int rows = input.GetUpperBound(0) + 1;
			int columns = input.GetUpperBound(1) + 1;
			
			string[,] output = new string[rows,columns];
			
			for(int row = 0; row < rows; row++)
			{
				for(int column = 0; column < columns; column++)
				{
					if(input[row,column].Count > 0)
					{
						switch (operationType.ToString())
						{
							case "AVG":
								output[row,column] = input[row,column].Average().ToString();
								break;
							case "SUM":
								output[row,column] = input[row,column].Sum().ToString();
								break;
							case "COUNT":
								output[row,column] = input[row,column].Count.ToString();
								break;
							default:
								output[row,column] = input[row,column].FirstOrDefault().ToString();
								break;
						}
					}
					
				}
			}
			
			return output;
	
		}
				
		public object ConvertBack(object value, Type targetTypes, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
