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
using System.Data;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace UTEC.Logger.Converters
{
    public class DataGridRowDataContextToRowHeaderValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dataGridRow = (DataGridRow)value;
            var row = (DataRowView)dataGridRow.DataContext;
            return row.Row.ItemArray[0];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}