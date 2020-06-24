using VoltstroEngine.Core.Inputs;

namespace VoltstroEngine.Events
{
	/// <summary>
	/// Key pressed event
	/// </summary>
	public class KeyPressedEvent : IEvent
	{
		public KeyPressedEvent(KeyCode keyCode, int repeatCount = 0)
		{
			KeyCode = keyCode;
			RepeatCount = repeatCount;
		}

		public KeyCode KeyCode { get; }
		public int RepeatCount { get; }

		public EventType GetEventType()
		{
			return EventType.KeyPressed;
		}
	}

	/// <summary>
	/// Key released event
	/// </summary>
	public class KeyReleasedEvent : IEvent
	{
		public KeyReleasedEvent(KeyCode keyCode)
		{
			KeyCode = keyCode;
		}

		public KeyCode KeyCode { get; }

		public EventType GetEventType()
		{
			return EventType.KeyPressed;
		}
	}

	/// <summary>
	/// Key typed event
	/// </summary>
	public class KeyTypedEvent : IEvent
	{
		public KeyTypedEvent(KeyCode keyCode)
		{
			KeyCode = keyCode;
		}

		public KeyCode KeyCode { get; }

		public EventType GetEventType()
		{
			return EventType.KeyPressed;
		}
	}
}