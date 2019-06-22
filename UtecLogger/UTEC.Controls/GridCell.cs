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
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace UTEC.Controls
{
	/// <summary>
	/// Description of GridCell.
	/// </summary>
	[TemplateVisualState(GroupName = GridCell.SelectionGroup, Name = GridCell.SelectedState)]
	[TemplateVisualState(GroupName = GridCell.SelectionGroup, Name = GridCell.UnSelectedState)]
	[TemplatePart(Name = GridCell.PART_Label, Type = typeof(TextBlock))]
	public class GridCell : SelectionBase
	{
		#region Template Parts
		protected const string PART_Label = "PART_Label";
		#endregion
		
		internal const string SelectionGroup = "Selection";
		internal const string SelectedState = "Selected";
		internal const string UnSelectedState = "UnSelected";
		
		
		public GridCell(FlexGrid flexGrid)
		{
			DefaultStyleKey = typeof(GridCell);
			FlexGrid = flexGrid;
		}
		
		private FlexGrid FlexGrid
		{
			get;set;
		}
		
		public override void IsInSelectionRange()
		{
			base.IsInSelectionRange();
			
			int gridColumn = (int)this.GetValue(Grid.ColumnProperty);
			int gridRow = (int)this.GetValue(Grid.RowProperty);
			
			var cellPosition = FlexGrid.SelectedCellIndexes.FirstOrDefault(p=>p[0] == gridRow-1 && p[1] == gridColumn-1);
			
			if(gridColumn >= this.SelectionRangeColumnStart
			   && gridColumn <= this.SelectionRangeColumnEnd
			   && gridRow >= this.SelectionRangeRowStart
			   && gridRow <= this.SelectionRangeRowEnd)
			{
				VisualStateManager.GoToState(this, GridCell.SelectedState, false);
				
				if(cellPosition == null)
				{
					FlexGrid.SelectedCellIndexes.Add(new int[]{gridRow-1,gridColumn-1});
				}
			}
			else
			{
				VisualStateManager.GoToState(this, GridCell.UnSelectedState, false);
				
				if(cellPosition != null)
				{
					FlexGrid.SelectedCellIndexes.Remove(cellPosition);
				}
			}
		}
		
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			TextBlock text = base.GetTemplateChild(GridCell.PART_Label) as TextBlock;
			
			if(text!=null)
			{
				ControlsHelper.CreateControlBinding(text,this,"Text",TextBlock.TextProperty,BindingMode.TwoWay);
			}
			
			this.IsInSelectionRange();
		}
		
		protected override void OnMouseDown(System.Windows.Input.MouseButtonEventArgs e)
		{
			base.OnMouseDown(e);
			this.Focus();
			this.IsMouseDown = true;
			SelectionOnMouseDown();
		}
		
		protected override void OnMouseUp(System.Windows.Input.MouseButtonEventArgs e)
		{
			base.OnMouseUp(e);
			this.IsMouseDown = false;
		}
		
		protected override void OnMouseEnter(System.Windows.Input.MouseEventArgs e)
		{
			base.OnMouseEnter(e);
			if(IsMouseDown)
			{
				SelectionOnMouseMove();
			}
		}
		
		private void SelectionOnMouseMove()
		{
			if(IsMouseDown)
			{
				int gridColumn = (int)this.GetValue(Grid.ColumnProperty);
				int gridRow = (int)this.GetValue(Grid.RowProperty);
				
				
				if(gridColumn <= InitialColumnStart)
				{
					this.SelectionRangeColumnStart = gridColumn;
				}
				
				if(gridColumn >= InitialColumnStart)
				{
					this.SelectionRangeColumnEnd = gridColumn;
				}
				
				if(gridRow <= InitialRowStart)
				{
					this.SelectionRangeRowStart = gridRow;
				}
				
				if(gridRow >= InitialRowStart)
				{
					this.SelectionRangeRowEnd = gridRow;
				}
				
				
			}
		}
		
		
		private void SelectionOnMouseDown()
		{
			if(IsMouseDown)
			{
				int gridColumn = (int)this.GetValue(Grid.ColumnProperty);
				int gridRow = (int)this.GetValue(Grid.RowProperty);
				
				this.InitialColumnStart = gridColumn;
				this.InitialRowStart = gridRow;
				
				this.SelectionRangeColumnStart = gridColumn;
				this.SelectionRangeColumnEnd = gridColumn;
				this.SelectionRangeRowStart = gridRow;
				this.SelectionRangeRowEnd = gridRow;
			}
		}
	}
}
