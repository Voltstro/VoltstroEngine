namespace VoltstroEngine.DebugTools.Data
{
	internal struct ProfileResult
	{
#if PROFILE
		internal ProfileResult(string name, int threadID, double start, double end)
		{
			Name = name;
			ThreadID = threadID;
			Start = start;
			End = end;
		}

		public readonly string Name;
		public readonly int ThreadID;
		public readonly double Start, End;

		public TraceEvent ToTraceEvent()
		{
			return new TraceEvent
			{
				cat = "function",
				dur = End - Start,
				name = Name,
				ph = "X",
				pid = 0,
				tid = ThreadID,
				ts = Start
			};
		}
#endif
	}
}