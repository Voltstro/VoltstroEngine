namespace VoltstroEngine
{
	public struct TimeStep
	{
		public TimeStep(float time)
		{
			this.Seconds = time;
		}

		public float Seconds { get; }
		public float Milliseconds => Seconds * 1000.0f;
	}
}