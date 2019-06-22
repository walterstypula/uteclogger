/*
 * Created by SharpDevelop.
 * User: Walter
 * Date: 11/17/2011
 * Time: 10:45 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace UTEC.Controls
{
	/// <summary>
	/// Description of DoubleTextBox.
	/// </summary>
	public class DoubleTextBox : TextBox
	{

		private string previousString = string.Empty;
		private bool pasteOp = false;
		
		protected override void OnTextInput(TextCompositionEventArgs e)
		{
			e.Handled = !DoubleCharChecker(e.Text);
			base.OnTextInput(e);
		}

		protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{
			this.SelectAll();
			base.OnGotKeyboardFocus(e);
		}
		
		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			e.Handled = (e.Key == Key.Space);
			//Checks for paste.
			if(Key.V == e.Key && Keyboard.Modifiers == ModifierKeys.Control)
			{
				previousString = this.Text;
				pasteOp = true;
			}
			
			base.OnPreviewKeyDown(e);
		}

		protected override void OnTextChanged(TextChangedEventArgs e)
		{
			if (true == pasteOp)
			{
				if (false == this.IsDouble(this.Text))
				{
					this.Text = previousString;
					e.Handled = true;
				}
				pasteOp = false;
			}

			
			base.OnTextChanged(e);
		}
		
		private bool DoubleCharChecker(string str)
		{
			foreach (char c in str)
			{
				if (c.Equals('-'))
					return true;

				else if (c.Equals('.'))
					return true;
				
				else if (Char.IsNumber(c))
					return true;
			}
			return false;
		}
		
		
		private bool IsDouble(string text)
		{
			double number;
		
			if (!(double.TryParse(text, out number)))
			{
				return false;
			}
			return true;
		}

	}

}
