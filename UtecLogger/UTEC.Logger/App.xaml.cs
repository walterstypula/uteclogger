using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using System.Xml;
using UTEC.Logger.ViewModels;
using UTEC.Logger.Views;

namespace UTEC.Logger
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public App()
		{
			this.DispatcherUnhandledException+= App_DispatcherUnhandledException;
		}
		
		void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			MessageBox.Show(String.Format("{0}{1}{2}",e.Exception.Message,Environment.NewLine,e.Exception.StackTrace));
			e.Handled = true;
		}
		
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			MainWindow window = new MainWindow();

			// Create the ViewModel to which
			// the main window binds.
			string path = "Settings/settings.xml";
			EventHandler closeHandler = null;
			MainWindowVM.ShowErrorHandler showErrorHandler = null;
			
			showErrorHandler = delegate(object sender, ErrorArgs args)
			{
				WriteErrorLog(args.Exception,args.UtecData);
				MessageBox.Show(args.Exception.Message);
			};
			//viewModel.ShowError += showErrorHandler;
			
			
			var viewModel = new MainWindowVM(path, showErrorHandler);
			
			closeHandler = delegate
			{
				viewModel.RequestClose -= closeHandler;
				viewModel.ShowError -= showErrorHandler;
				
				viewModel.ApplicationExitCloseConnections();
				Application.Current.Shutdown();
			};
			viewModel.RequestClose += closeHandler;


			// Allow all controls in the window to
			// bind to the ViewModel by setting the
			// DataContext, which propagates down
			// the element tree.
			window.DataContext = viewModel;

			window.Show();
		}
		
		private void WriteErrorLog(Exception ex, string utecData)
		{
			TextWriter tw = new StreamWriter("ErrorLog.txt", true, Encoding.ASCII);
			
			StringBuilder errorSB = new StringBuilder();
			errorSB.AppendLine(string.Format("{0}: {1}","Utec Data causing error", utecData));
			GetExceptionMessages(errorSB,ex);
			tw.WriteLine(errorSB.ToString());
			tw.Close();
		}
		
		private void GetExceptionMessages(StringBuilder errorSB, Exception ex)
		{
			
			errorSB.AppendLine("ERROR_MESSAGE: " + ex.Message);
			errorSB.AppendLine("ERROR_STACKTRACE: " +ex.StackTrace);
			errorSB.AppendLine(Environment.NewLine);
			
			if(ex.InnerException != null)
			{
				errorSB.AppendLine("ERROR_INNER_EXCEPTION:");
				GetExceptionMessages(errorSB,ex.InnerException);
			}
		}
	}
}