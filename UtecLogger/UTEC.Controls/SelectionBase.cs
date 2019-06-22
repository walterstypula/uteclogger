/*
 * Created by SharpDevelop.
 * User: Walter
 * Date: 06/05/2011
 * Time: 16:51
 * 
 * $
{
res:XML.StandardHeader.HowToChangeTemplateInformation
}
 */
using System;
using System.Windows;
using System.Windows.Controls;

namespace UTEC.Controls
{
	/// <summary>
	/// Description of SelectionBase.
	/// </summary>
	public class SelectionBase : Control
	{
		public static readonly DependencyProperty SelectionRangeColumnStartProperty =
			DependencyProperty.Register("SelectionRangeColumnStart", typeof(int), typeof(SelectionBase), 
			                            new FrameworkPropertyMetadata(-1,OnSelectedRangeChanged));
		
		public int SelectionRangeColumnStart {
			get { return (int)GetValue(SelectionRangeColumnStartProperty); }
			set { SetValue(SelectionRangeColumnStartProperty, value); }
		}
		
		public static readonly DependencyProperty SelectionRangeRowStartProperty =
			DependencyProperty.Register("SelectionRangeRowStart", typeof(int), typeof(SelectionBase),
			                            new FrameworkPropertyMetadata(-1,OnSelectedRangeChanged));
		
		public int SelectionRangeRowStart {
			get { return (int)GetValue(SelectionRangeRowStartProperty); }
			set { SetValue(SelectionRangeRowStartProperty, value); }
		}
		
		public static readonly DependencyProperty SelectionRangeColumnEndProperty =
			DependencyProperty.Register("SelectionRangeColumnEnd", typeof(int), typeof(SelectionBase),
			                            new FrameworkPropertyMetadata(-1,OnSelectedRangeChanged));
		
		public int SelectionRangeColumnEnd {
			get { return (int)GetValue(SelectionRangeColumnEndProperty); }
			set { SetValue(SelectionRangeColumnEndProperty, value); }
		}
		
		public static readonly DependencyProperty SelectionRangeRowEndProperty =
			DependencyProperty.Register("SelectionRangeRowEnd", typeof(int), typeof(SelectionBase),
			                            new FrameworkPropertyMetadata(-1,OnSelectedRangeChanged));
		
		public int SelectionRangeRowEnd {
			get { return (int)GetValue(SelectionRangeRowEndProperty); }
			set { SetValue(SelectionRangeRowEndProperty, value); }
		}
		
		public static readonly DependencyProperty TextProperty =
			DependencyProperty.Register("Text", typeof(string), typeof(SelectionBase),
			                            new FrameworkPropertyMetadata());
		
		public string Text {
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}
		
		public static readonly DependencyProperty IsMouseDownProperty =
			DependencyProperty.Register("IsMouseDown", typeof(bool), typeof(SelectionBase),
			                            new FrameworkPropertyMetadata(false,OnMouseDown));
		
		public bool IsMouseDown {
			get { return (bool)GetValue(IsMouseDownProperty); }
			set { SetValue(IsMouseDownProperty, value); }
		}
		
		public static readonly DependencyProperty InitialColumnStartProperty =
			DependencyProperty.Register("InitialColumnStart", typeof(int), typeof(SelectionBase),
			                            new FrameworkPropertyMetadata());
		
		public int InitialColumnStart {
			get { return (int)GetValue(InitialColumnStartProperty); }
			set { SetValue(InitialColumnStartProperty, value); }
		}
		
		public static readonly DependencyProperty InitialRowStartProperty =
			DependencyProperty.Register("InitialRowStart", typeof(int), typeof(SelectionBase),
			                            new FrameworkPropertyMetadata());
		
		public int InitialRowStart {
			get { return (int)GetValue(InitialRowStartProperty); }
			set { SetValue(InitialRowStartProperty, value); }
		}
		
		private static void OnMouseDown(DependencyObject dObj, DependencyPropertyChangedEventArgs args)
		{
			  var selectionBase = (dObj as SelectionBase);
            selectionBase.TheMouseIsDown();
		}
		
		public virtual void TheMouseIsDown()
		{
			Console.WriteLine("asdfasfsdfas");
		}
		
		
		private static void OnSelectedRangeChanged(DependencyObject dObj, DependencyPropertyChangedEventArgs args)
        {
            var selectionBase = (dObj as SelectionBase);
            selectionBase.IsInSelectionRange();
        }
		
		public virtual void IsInSelectionRange()
		{
			
		}
	}
}
