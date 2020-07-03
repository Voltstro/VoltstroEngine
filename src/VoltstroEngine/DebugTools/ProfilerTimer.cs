using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using VoltstroEngine.DebugTools.Data;

namespace VoltstroEngine.DebugTools
{
	/// <summary>
	/// A timer used for the <see cref="Profiler"/>
	/// </summary>
	public sealed class ProfilerTimer
	{
		public ProfilerTimer(string name)
		{
			startTime = DateTime.Now;
			this.name = name;
			stopwatch = new Stopwatch();

			stopwatch.Start();
		}

		private readonly DateTime startTime;
		private readonly string name;

		private readonly Stopwatch stopwatch;

		/// <summary>
		/// Stops the timer, and records it
		/// </summary>
		[Conditional("PROFILE")]
		private void Stop()
		{
#if PROFILE
			stopwatch.Stop();

			long start = startTime.Ticks / (TimeSpan.TicksPerMillisecond / 1000);
			int threadId = Thread.CurrentThread.ManagedThreadId;

			Profiler.Instance.AddProfile(new ProfileResult(name, threadId, start, stopwatch.ElapsedTicks * 1000000 / Stopwatch.Frequency));
#endif
		}

		public static void Profile(Action action, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "")
		{
			//Probs a better way doing this, but idgaf at the moment
			Profile($"{Path.GetFileNameWithoutExtension(filePath)}.{memberName}", action);
		}

		public static void Profile(string name, Action action)
		{
#if PROFILE
			ProfilerTimer timer = new ProfilerTimer(name);
#endif
			action();
#if PROFILE
			timer.Stop();
#endif
		}
	}
}