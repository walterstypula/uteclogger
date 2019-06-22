using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVVM
{
	public sealed class MessageBus
	{
		private Dictionary<string, List<ActionInvoker>> _subscribers = new Dictionary<string, List<ActionInvoker>>();

		public void Subscribe(string actionName, ActionInvoker handler)
		{
			if (_subscribers.ContainsKey(actionName))
			{
				var handlers = _subscribers[actionName];
				handlers.Add(handler);
			}
			else
			{
				var handlers = new List<ActionInvoker>();
				handlers.Add(handler);
				_subscribers[actionName] = handlers;
			}
		}

		public void Unsubscribe(string actionName, ActionInvoker handler)
		{
			if (_subscribers.ContainsKey(actionName))
			{
				var handlers = _subscribers[actionName];
				handlers.Remove(handler);

				if (handlers.Count == 0)
				{
					_subscribers.Remove(actionName);
				}
			}
		}

		public void Publish(ActionItem action)
		{
			if (_subscribers.ContainsKey(action.ActionName))
			{
				var handlers = _subscribers[action.ActionName];
				foreach (ActionInvoker handler in handlers)
				{
					handler.Invoke(action);
				}
			}
		}
		
		private static MessageBus _instance = null;
		public static MessageBus Instance
		{
			get
			{
				if(_instance == null)
					_instance = new MessageBus();
				
				return _instance;
			}
		}
		
		
	}
	
	public delegate void ActionInvoker(ActionItem action);
	
	public class ActionItem
	{
		public ActionItem(string actionName, object actionSource, object[] param)
		{
			this.ActionName = actionName;
			this.ActionSource = actionSource;
			this.Param = param;
		}
		
		public string ActionName
		{get;private set;}
		
		public object ActionSource
		{get;private set;}
		
		public object[] Param
		{get;private set;}
		
	}
}
