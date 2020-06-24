namespace VoltstroEngine.Core
{
	/// <summary>
	/// A <see cref="TimeStep"/>
	/// </summary>
	public struct TimeStep
	{
		public TimeStep(float time)
		{
			Seconds = time;
		}

		/// <summary>
		/// The delta time
		/// <para>Time in seconds since the last frame</para>
		/// </summary>
		public float Seconds { get; }

		/// <summary>
		/// The dealt time (In Milliseconds)
		/// <para>Time in milliseconds since the last frame</para>
		/// </summary>
		public float Milliseconds => Seconds * 1000.0f;
	}
}