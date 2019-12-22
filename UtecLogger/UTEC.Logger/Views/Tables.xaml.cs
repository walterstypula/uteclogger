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

using MVVM;
using System.Windows;
using System.Windows.Controls;

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