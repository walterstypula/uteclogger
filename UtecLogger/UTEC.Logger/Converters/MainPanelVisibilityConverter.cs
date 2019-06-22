/*
 * Created by SharpDevelop.
 * User: Walter
 * Date: 05/29/2011
 * Time: 17:36
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Globalization;
using System.Runtime;
using System.Windows;
using System.Windows.Data;

namespace UTEC.Logger.Converters
{
	/// <summary>
	/// Description of InverseVisibilityConverter.
	/// </summary>
	/// 
	public class MainPanelVisibilityConverter : IMultiValueConverter
	{
		public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
		{
			
			bool showSettingsPanel;
			bool showTablesPanel;

			bool.TryParse(value[0].ToString(), out showSettingsPanel);
			bool.TryParse(value[1].ToString(), out showTablesPanel);

			bool flag = false;
			if(showSettingsPanel != showTablesPanel)
			{
				flag = true;
			}
			
			return flag ? Visibility.Collapsed :  Visibility.Visible;
		}
	
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
//			if (value is Visibility)
//			{
//				return (Visibility)value == Visibility.Collapsed;
//			}
//			return false;
			
			return null;
		}
	}
}
