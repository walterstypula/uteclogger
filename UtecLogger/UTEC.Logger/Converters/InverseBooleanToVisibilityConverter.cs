/*
 * Created by SharpDevelop.
 * User: Walter
 * Date: 10/9/2011
 * Time: 1:44 AM
 * 
 * $
{
res:XML.StandardHeader.HowToChangeTemplateInformation
}
 */
using System;
using System.Windows;
using System.Windows.Data;

namespace UTEC.Logger.Converters
{
	/// <summary>
	/// Description of InverseVisibilityConverter.
	/// </summary>
	public class InverseBooleanToVisibilityConverter : IValueConverter
	{
		public InverseBooleanToVisibilityConverter()
		{
		}
		
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			bool flag = false;
			if (value is bool)
			{
				flag = (bool)value;
			}
			else
			{
				if (value is bool?)
				{
					bool? flag2 = (bool?)value;
					flag = (flag2.HasValue && flag2.Value);
				}
			}
			return flag ? Visibility.Collapsed : Visibility.Visible;
		}
		
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
