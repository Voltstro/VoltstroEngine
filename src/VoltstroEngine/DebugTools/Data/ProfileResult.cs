namespace VoltstroEngine.DebugTools.Data
{
	internal struct ProfileResult
	{
#if PROFILE
		internal ProfileResult(string name, int threadID, long startTime, long duration)
		{
			Name = name;
			ThreadID = threadID;
			Duration = duration;
			StartTime = startTime;
		}

		public readonly string Name;
		public readonly int ThreadID;
		public readonly long Duration, StartTime;

		public TraceEvent ToTraceEvent()
		{
			return new TraceEvent
			{
				cat = "function",
				dur = Duration,
				name = Name,
				ph = "X",
				pid = 0,
				tid = ThreadID,
				ts = StartTime
			};
		}
#endif
	}
}