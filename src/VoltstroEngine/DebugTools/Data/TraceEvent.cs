#if PROFILE

namespace VoltstroEngine.DebugTools.Data
{
	internal struct TraceEvent
	{

		public string cat;
		public long dur;
		public string name;
		public string ph;
		public int pid;
		public int tid;
		public long ts;

	}
}

#endif