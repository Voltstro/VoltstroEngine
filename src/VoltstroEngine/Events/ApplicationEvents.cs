namespace VoltstroEngine.Events
{
	/// <summary>
	/// A window resized event
	/// </summary>
	public class WindowResizedEvent : IEvent
	{
		public WindowResizedEvent(int width, int height)
		{
			Width = width;
			Height = height;
		}

		public int Width { get; }
		public int Height { get; }

		public EventType GetEventType()
		{
			return EventType.WindowResize;
		}
	}

	/// <summary>
	/// Window close event
	/// </summary>
	public class WindowCloseEvent : IEvent
	{
		public EventType GetEventType()
		{
			return EventType.WindowClose;
		}
	}
}