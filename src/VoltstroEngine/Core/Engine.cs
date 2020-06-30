﻿using System;
using VoltstroEngine.DebugTools;
using VoltstroEngine.EtoForms;
using VoltstroEngine.Rendering.Renderer;

namespace VoltstroEngine.Core
{
	public static class Engine
	{
		/// <summary>
		/// The name and location of the game
		/// <para>E.G: Sandbox</para>
		/// </summary>
		public static string GameName { get; private set; }

		/// <summary>
		/// Init, and runs the game
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="noWindow"></param>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="NullReferenceException"></exception>
		public static void Init(IEntryPoint entry, bool noWindow = false)
		{
			//Make sure the entry isn't null, or that there is no game name
			if(entry == null)
				throw new ArgumentNullException(nameof(entry), "Entry cannot be null!");

			if(string.IsNullOrWhiteSpace(entry.GetGameName()))
				throw new NullReferenceException("Game name cannot be null!");

			Instrumentor.BeginSession("Startup", "VoltstroEngineProfile-Startup.json");
			var engineInitTimer = InstrumentationTimer.Create("Engine Init");

			//Setup render
			RenderingAPI.Create();

			//Set game name, since we load all our game related files from that path
			//So if the game requests for a texture, we load it from the game's bin parent directory, allowing for multiple games, but all using the same copy of the engine

			//E.G:
			// - Engine Stuff (Launcher/VoltstroEngine.dll)
			// - |
			// - Sandbox
			// - - Textures/

			GameName = entry.GetGameName();

			//We may want no window, E.G: For when we are testing
			if (!noWindow)
			{
				//Init our forms system
				EtoFormsSystem.Init();

				//Create the app
				Application app = entry.CreateApplication();
				if(app == null)
					throw new NullReferenceException("The app cannot be null!");

				//Init the render
				Renderer.Init();

				engineInitTimer.Stop();
				Instrumentor.EndSession();

				//Run the main loop
				Instrumentor.BeginSession("Runtime", "VoltstroEngineProfile-Runtime.json");
				{
					InstrumentationTimer gameLoopTimer = InstrumentationTimer.Create("App Run");
					app.Run();
					gameLoopTimer.Stop();
				}
				Instrumentor.EndSession();

				//Shutdown stuff
				EtoFormsSystem.Shutdown();
			}
			else
			{
				Renderer.Init();
				Instrumentor.EndSession();
			}
		}
	}
}