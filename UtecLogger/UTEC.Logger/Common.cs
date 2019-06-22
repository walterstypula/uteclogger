/*
 * Created by SharpDevelop.
 * User: Walter
 * Date: 6/26/2011
 * Time: 6:18 PM
 * 
 * $
{
res:XML.StandardHeader.HowToChangeTemplateInformation
}
 */
using System;
using Microsoft.Win32;

namespace UTEC.Logger
{
	/// <summary>
	/// Description of Common.
	/// </summary>
	public class Common
	{				
		public static string SelectFile(string filter)
		{
			var dialog = new OpenFileDialog();
			dialog.Multiselect = false;
			dialog.Filter = filter;
			dialog.DefaultExt = filter;
					
			var result = dialog.ShowDialog();
						
			if(result.HasValue && result.Value == true)
			{
				return dialog.FileName;
			}
			
			return string.Empty;
		}
		
		public static string SelectDirectory()
		{
			var dialog = new System.Windows.Forms.FolderBrowserDialog();
			var result = dialog.ShowDialog();
			
			if(result == System.Windows.Forms.DialogResult.OK)
			{
				return dialog.SelectedPath;
			}
			
			return string.Empty;
		}
		
		public static string SaveFile()
		{
			var dialog = new SaveFileDialog();
			
			dialog.Filter = "text files|*.txt";
			dialog.DefaultExt = "text files|*.txt";
				
			var result = dialog.ShowDialog();
			
			if(result.HasValue && result.Value == true)
			{
				return dialog.FileName;
			}
			
			return string.Empty;
		}
	}
}
