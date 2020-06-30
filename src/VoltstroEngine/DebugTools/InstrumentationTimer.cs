using System;
using System.Diagnostics;
using System.Threading;
using VoltstroEngine.DebugTools.Data;

namespace VoltstroEngine.DebugTools
{
	/// <summary>
	/// A timer used for the <see cref="Instrumentor"/>
	/// </summary>
	public sealed class InstrumentationTimer
	{
		public InstrumentationTimer(string name)
		{
#if PROFILE
			startTime = DateTime.Now;
			this.name = name;
#endif
		}

#if PROFILE
		~InstrumentationTimer()
		{
			if (!stopped)
				Stop();
		}

		private bool stopped;
		private readonly DateTime startTime;
		private readonly string name;
#endif

		[Conditional("PROFILE")]
		public void Stop()
		{
#if PROFILE
			DateTime endTime = DateTime.Now;

			double start = startTime.TimeOfDay.TotalMilliseconds;
			double end = endTime.TimeOfDay.TotalMilliseconds;

			int threadId = Thread.CurrentThread.ManagedThreadId;

			Instrumentor.Instance.AddProfile(new ProfileResult(name, threadId, start, end));

			stopped = true;
#endif
		}
	}
}