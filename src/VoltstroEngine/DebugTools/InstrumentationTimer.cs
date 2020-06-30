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
			stopwatch = new Stopwatch();

			stopwatch.Start();
#endif
		}

#if PROFILE
		private readonly DateTime startTime;
		private readonly string name;

		private readonly Stopwatch stopwatch;
#endif

		[Conditional("PROFILE")]
		public void Stop()
		{
#if PROFILE
			stopwatch.Stop();

			long start = startTime.Ticks / (TimeSpan.TicksPerMillisecond / 1000);
			int threadId = Thread.CurrentThread.ManagedThreadId;

			Instrumentor.Instance.AddProfile(new ProfileResult(name, threadId, start, stopwatch.ElapsedTicks * 1000000 / Stopwatch.Frequency));
#endif
		}

		public static InstrumentationTimer Create(string name)
		{
#if PROFILE
			return new InstrumentationTimer(name);
#else
			return null;
#endif
		}
	}
}