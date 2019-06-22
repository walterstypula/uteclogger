/*
 * Created by SharpDevelop.
 * User: Walter
 * Date: 05/28/2011
 * Time: 10:55
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Input;

namespace UTEC.Logger
{
	/// <summary>
	/// Description of RelayCommand.
	/// </summary>
	public class RelayCommand : ICommand
	{
	    #region Fields
	
	    readonly Action<object> _execute;
	    readonly Predicate<object> _canExecute;        
	
	    #endregion // Fields
	
	    #region Constructors
	
	    public RelayCommand(Action<object> execute)
	    : this(execute, null)
	    {
	    }
	
	    public RelayCommand(Action<object> execute, Predicate<object> canExecute)
	    {
	        if (execute == null)
	            throw new ArgumentNullException("execute");
	
	        _execute = execute;
	        _canExecute = canExecute;           
	    }
	    #endregion // Constructors
	
	    #region ICommand Members
	
	    public bool CanExecute(object parameter)
	    {
	        return _canExecute == null ? true : _canExecute(parameter);
	    }
	
	    public event EventHandler CanExecuteChanged
	    {
	        add { CommandManager.RequerySuggested += value; }
	        remove { CommandManager.RequerySuggested -= value; }
	    }
	
	    public void Execute(object parameter)
	    {
	        _execute(parameter);
	    }
	
	    #endregion // ICommand Members
	}

}
