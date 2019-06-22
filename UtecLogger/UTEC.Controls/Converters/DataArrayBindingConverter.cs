/*
 * Created by SharpDevelop.
 * User: Walter
 * Date: 6/6/2011
 * Time: 9:05 PM
 * 
 * $
{
res:XML.StandardHeader.HowToChangeTemplateInformation
}
 */
using System;
using System.Diagnostics;
using System.Windows.Data;

namespace UTEC.Controls.Converters
{
	/// <summary>
	/// Description of DataArrayBindingConverter.
	/// </summary>
	public class DataArrayBindingConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var dataArray = value as string[,];
			var indexData = parameter as int[];
			
			if(dataArray != null && indexData != null)
			{			
				if((indexData[0]-1) <= dataArray.GetUpperBound(0)  &&   (indexData[1]-1) <=dataArray.GetUpperBound(1))
				{
					return dataArray[indexData[0]-1,indexData[1]-1];
				}
			}
		
			return null;
		}
		
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			Debugger.Break();
			return null;
		}
	}
}
