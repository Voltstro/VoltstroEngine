namespace VoltstroEngine.Events
{
	public class MouseMovedEvent : IEvent
	{
		/// <summary>
		/// Mouse moved event
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public MouseMovedEvent(float x, float y)
		{
			MouseX = x;
			MouseY = y;
		}

		public float MouseX { get; }
		public float MouseY { get; }

		public EventType GetEventType()
		{
			return EventType.MouseMoved;
		}
	}

	/// <summary>
	/// Mouse scroll event
	/// </summary>
	public class MouseScrollEvent : IEvent
	{
		public MouseScrollEvent(float xOffset, float yOffset)
		{
			OffsetX = xOffset;
			OffsetY = yOffset;
		}

		public float OffsetX { get; }
		public float OffsetY { get; }

		public EventType GetEventType()
		{
			return EventType.MouseScrolled;
		}
	}

	/// <summary>
	/// Mouse button pressed event
	/// </summary>
	public class MouseButtonPressedEvent : IEvent
	{
		public MouseButtonPressedEvent(int button)
		{
			Button = button;
		}

		public int Button { get; }

		public EventType GetEventType()
		{
			return EventType.MouseButtonPressed;
		}
	}

	/// <summary>
	/// Mouse button released event
	/// </summary>
	public class MouseButtonReleasedEvent : IEvent
	{
		public MouseButtonReleasedEvent(int button)
		{
			Button = button;
		}

		public int Button { get; }

		public EventType GetEventType()
		{
			return EventType.MouseButtonReleased;
		}
	}
}