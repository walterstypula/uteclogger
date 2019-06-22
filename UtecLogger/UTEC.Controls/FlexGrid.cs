/*
 * Created by SharpDevelop.
 * User: Walter
 * Date: 06/05/2011
 * Time: 14:24
 * 
 * $
{
res:XML.StandardHeader.HowToChangeTemplateInformation
}
 */
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

using UTEC.Controls.Converters;

namespace UTEC.Controls
{
	/// <summary>
	/// Description of FlexGrid.
	/// </summary>
	/// <summary>
	/// Description of Grid.
	/// </summary>
	[TemplatePart(Name = "PART_Grid", Type = typeof(Grid))]
	public class FlexGrid : SelectionBase
	{
		public FlexGrid()
		{
			DefaultStyleKey = typeof(FlexGrid);
		}	
		
		public static readonly DependencyProperty DataProperty =
			DependencyProperty.Register("Data", typeof(object), typeof(FlexGrid),
			                            new FrameworkPropertyMetadata(OnDataChanged));
		
		public object Data {
			get { return (object)GetValue(DataProperty); }
			set { SetValue(DataProperty, value); }
		}
		
		private static void OnDataChanged(DependencyObject dObj, DependencyPropertyChangedEventArgs args)
        {
          	var flexGrid = dObj as FlexGrid;
          	if(flexGrid !=null)
          	{
          			
          	}
        }
		
		ObservableCollection<FlexRow> _rows;
		public ObservableCollection<FlexRow> Rows 
		{
			get
			{
				if(_rows==null)
				{
					_rows = new ObservableCollection<FlexRow>();
				}
				return _rows;
			}
		}
		
		ObservableCollection<FlexColumn> _columns;
		public ObservableCollection<FlexColumn> Columns 
		{
			get
			{
				if(_columns == null)
				{
					_columns = new ObservableCollection<FlexColumn>();
				}
				return _columns;
			}
		}
		
		
		public static readonly DependencyProperty SelectedCellIndexesProperty =
			DependencyProperty.Register("SelectedCellIndexes", typeof(ObservableCollection<int[]>), typeof(FlexGrid),
			                            new FrameworkPropertyMetadata(new ObservableCollection<int[]>()));
		
		public ObservableCollection<int[]> SelectedCellIndexes {
			get { return (ObservableCollection<int[]>)GetValue(SelectedCellIndexesProperty); }
			set { SetValue(SelectedCellIndexesProperty, value); }
		}
		

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			
			Grid grid = base.GetTemplateChild("PART_Grid") as Grid;

			BuildGrid(grid);
			PopulateGrid(grid);
			
		}
		
		private void PopulateGrid(Grid grid)
		{
			var converter = new DataArrayBindingConverter();
			
			for(int r = 1; r < grid.RowDefinitions.Count; r+=1)
			{
				for(int c = 1; c < grid.ColumnDefinitions.Count; c+=1)
				{
					GridCell cell = new GridCell(this);
					cell.Text = (r*c).ToString();
			 		cell.SetValue(Grid.RowProperty,r);
			 		cell.SetValue(Grid.ColumnProperty,c);
			 		
			 		ControlsHelper.CreateControlBinding(cell,this,"SelectionRangeColumnEnd",GridCell.SelectionRangeColumnEndProperty,BindingMode.TwoWay);
			 		ControlsHelper.CreateControlBinding(cell,this,"SelectionRangeColumnStart", GridCell.SelectionRangeColumnStartProperty,BindingMode.TwoWay);
			 		ControlsHelper.CreateControlBinding(cell,this,"SelectionRangeRowEnd",GridCell.SelectionRangeRowEndProperty,BindingMode.TwoWay);
			 		ControlsHelper.CreateControlBinding(cell,this,"SelectionRangeRowStart",GridCell.SelectionRangeRowStartProperty,BindingMode.TwoWay);
			 		ControlsHelper.CreateControlBinding(cell,this,"IsMouseDown",GridCell.IsMouseDownProperty,BindingMode.TwoWay);
			 		ControlsHelper.CreateControlBinding(cell,this,"InitialColumnStart",GridCell.InitialColumnStartProperty,BindingMode.TwoWay);
			 		ControlsHelper.CreateControlBinding(cell,this,"InitialRowStart",GridCell.InitialRowStartProperty,BindingMode.TwoWay);
			 		ControlsHelper.CreateControlBinding(cell,this,"Data", GridCell.TextProperty,BindingMode.TwoWay, converter , new int[]{r,c});
			 		
			 	 	grid.Children.Add(cell);
				}
			}
		}
		
		
		protected void BuildGridColumns()
		{
			Grid grid = base.GetTemplateChild("PART_Grid") as Grid;
			
			if(grid != null)
			{
			 	var emptyColumn = new ColumnDefinition();
			 	grid.ColumnDefinitions.Add(emptyColumn);
			 	
			 	int columnCounter=0;
			 	
			 	
			 	for (int i = 0; i < this.Columns.Count ; i++)
			 	{
			 		var columnDefintion = new ColumnDefinition();
			 		grid.ColumnDefinitions.Add(columnDefintion);

			 		var column = this.Columns[i];
			 					 	
			 		TextBlock textBlock = new TextBlock();
			 		textBlock.Text = column.ColumnName;
			 		textBlock.SetValue(Grid.ColumnProperty,grid.ColumnDefinitions.IndexOf(columnDefintion));
			 			
			 		grid.Children.Add(textBlock);
			 		columnCounter++;
			 		
			 	}
			}
			 	
		}
		
		protected void BuildGridRows()
		{
			Grid grid = base.GetTemplateChild("PART_Grid") as Grid;
			if(grid != null)
			{
				var emptyRow = new RowDefinition();
			 	grid.RowDefinitions.Add(emptyRow);
			 	
			 	int rowCounter = 0;
			 	
			 	for(int i = 0 ; i<this.Rows.Count ; i++)
			 	{
			 		var rowDefintion = new RowDefinition();
			 		grid.RowDefinitions.Add(rowDefintion);
			 		
			 		var row = this.Rows[i];
			 		
			 		TextBlock textBlock = new TextBlock();
			 		textBlock.Text = row.RowName;
			 		textBlock.SetValue(Grid.RowProperty,grid.RowDefinitions.IndexOf(rowDefintion));
			 			
			 		grid.Children.Add(textBlock);
			 		rowCounter++;
			 	}
			}
		}
			
		private void BuildGrid(Grid grid)
		{
			BuildGridColumns();
			BuildGridRows();
		}
		
	
	}
	
	public class FlexColumn : FrameworkElement
	{
		public static readonly DependencyProperty ColumnNameProperty =
			DependencyProperty.Register("ColumnName", typeof(string), typeof(FlexColumn),
			                            new FrameworkPropertyMetadata());
		
		public string ColumnName {
			get { return (string)GetValue(ColumnNameProperty); }
			set { SetValue(ColumnNameProperty, value); }
		}
	}
	
	public class FlexRow : FrameworkElement
	{
		public static readonly DependencyProperty RowNameProperty =
			DependencyProperty.Register("RowName", typeof(string), typeof(FlexRow),
			                            new FrameworkPropertyMetadata());
		
		public string RowName {
			get { return (string)GetValue(RowNameProperty); }
			set { SetValue(RowNameProperty, value); }
		}
	}
}
