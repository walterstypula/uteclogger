/*
 * Created by SharpDevelop.
 * User: Walter
 * Date: 5/31/2011
 * Time: 11:07 PM
 * 
 * $
{
res:XML.StandardHeader.HowToChangeTemplateInformation
}
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

using MVVM;
using UTEC.Controls;

namespace UTEC.Logger.Views
{
	/// <summary>
	/// Interaction logic for Tables.xaml
	/// </summary>
	public partial class Tables : UserControl
	{
		
		private ActionInvoker _actionHandler = null;
		
		public Tables()
		{
			InitializeComponent();

			
			
			for(int i = 0 ; i<=100; i+=10)
			{
				var col = new FlexColumn(){ColumnName = i.ToString()};
				
				flexGrid.Columns.Add(col);
			}
			
			for(int i = 500 ; i<=9000; i+=250)
			{
				var row = new FlexRow(){RowName = i.ToString()};
				
				flexGrid.Rows.Add(row);
			}
			
			_actionHandler = new ActionInvoker(OnAction);
			MessageBus.Instance.Subscribe(Actions.TABLES_FOCUS_CONTROL,	_actionHandler);
			MessageBus.Instance.Subscribe(Actions.SHOW_EDITOR,	_actionHandler);
			
		}
		
		
		private void OnAction(ActionItem action)
		{
			switch (action.ActionName)
			{
				case Actions.TABLES_FOCUS_CONTROL:
					this.btnClose.Focus();
					break;
				case Actions.SHOW_EDITOR:
					Editor.Visibility = Visibility.Visible;
					ModifyValue.Focus();
					break;
				default:
					
					break;
			}
		}
	}
}