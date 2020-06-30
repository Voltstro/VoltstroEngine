using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using VoltstroEngine.Core.Logging;
using VoltstroEngine.DebugTools.Data;

namespace VoltstroEngine.DebugTools
{
	public class Instrumentor
	{
		private InstrumentationSession session;
		private List<ProfileResult> profileResults;

		public static Instrumentor Instance { get; private set; }

		/// <summary>
		/// Begins a new session
		/// </summary>
		/// <param name="name"></param>
		/// <param name="filePath"></param>
		[Conditional("DEBUG")]
		public void BeginSession(string name, string filePath = "results.json")
		{
			session = new InstrumentationSession
			{
				Name = name,
				FilePath = filePath
			};

			profileResults = new List<ProfileResult>();
			Instance = this;

			Logger.Log($"Started a new instrumentor session {name}", LogVerbosity.Debug);
		}

		/// <summary>
		/// Ends the session and writes the profile file
		/// </summary>
		[Conditional("DEBUG")]
		public void EndSession()
		{
			//Convert all of the profile results a traced event
			TraceEvent[] events = profileResults.Select(profileResult => profileResult.ToTraceEvent()).ToArray();

			//Write file
			string json = JsonConvert.SerializeObject(new TracingFile
			{
				OtherData = new TraceOtherData(),
				TraceEvents = events
			});

			File.WriteAllText(session.FilePath, json);

			//Clean up
			Instance = null;
			profileResults.Clear();
			profileResults = null;

			Logger.Log($"Ended instrumentor session {session.Name}", LogVerbosity.Debug);
		}

		/// <summary>
		/// Adds a new profile
		/// </summary>
		/// <param name="result"></param>
		[Conditional("DEBUG")]
		internal void AddProfile(ProfileResult result)
		{
			profileResults.Add(result);
		}

		private class TracingFile
		{
			[JsonProperty("otherData")] public TraceOtherData OtherData;
			[JsonProperty("traceEvents")] public TraceEvent[] TraceEvents;
		}
	}
}