#if PROFILE

namespace VoltstroEngine.DebugTools.Data
{
	internal struct TraceEvent
	{

		public string cat;
		public double dur;
		public string name;
		public string ph;
		public int pid;
		public int tid;
		public double ts;

	}
}

#endif