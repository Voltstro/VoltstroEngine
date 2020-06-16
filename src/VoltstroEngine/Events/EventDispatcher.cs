using System;

namespace VoltstroEngine.Events
{
	/// <summary>
	/// Handles dispatching events
	/// </summary>
	public static class EventDispatcher
	{
		/// <summary>
		/// Dispatches a event
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="e"></param>
		/// <param name="result"></param>
		public static void DispatchEvent<T>(IEvent e, Action<T> result)
		{
			if (e.GetType() == typeof(T))
			{
				result.Invoke((T)e);
			}
		}
	}
}