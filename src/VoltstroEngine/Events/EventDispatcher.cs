using System;

namespace VoltstroEngine.Events
{
	/// <summary>
	/// Handles dispatching events
	/// </summary>
	public class EventDispatcher
	{
		/// <summary>
		/// Dispatches a event
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="e"></param>
		/// <param name="result"></param>
		public void DispatchEvent<T>(IEvent e, Action result)
		{
			if (e.GetType() == typeof(T))
			{
				result.Invoke();
			}
		}
	}
}