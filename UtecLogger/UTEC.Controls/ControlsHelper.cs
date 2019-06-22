/*
 * Created by SharpDevelop.
 * User: Walter
 * Date: 06/05/2011
 * Time: 16:09
 * 
 * $
{
res:XML.StandardHeader.HowToChangeTemplateInformation
}
 */
using System;
using System.Windows;
using System.Windows.Data;

namespace UTEC.Controls
{
	/// <summary>
	/// Description of ControlsHelper.
	/// </summary>
	public static class ControlsHelper
	{
		public static void CreateControlBinding(FrameworkElement targetControl, Object source, String sourcePropertyPath, DependencyProperty dependencyProperty, BindingMode mode)
        {
            Binding binding = new Binding();
            binding.Source = source;
            binding.Mode = mode;
            binding.Path = new PropertyPath(sourcePropertyPath);
            targetControl.SetBinding(dependencyProperty, binding);
        }
		
		public static void CreateControlBinding(FrameworkElement targetControl, Object source, String sourcePropertyPath, 
		                                        DependencyProperty dependencyProperty, BindingMode mode, IValueConverter converter, object converterParameter)
        {
            Binding binding = new Binding();
            binding.Source = source;
            binding.Mode = mode;
            binding.Converter = converter;
            binding.ConverterParameter = converterParameter;
            binding.Path = new PropertyPath(sourcePropertyPath);
            targetControl.SetBinding(dependencyProperty, binding);
        }
	}
}
