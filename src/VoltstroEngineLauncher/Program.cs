using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Eto;
using VoltstroEngine.Core;
using VoltstroEngine.Core.Logging;
using Application = Eto.Forms.Application;

namespace VoltstroEngineLauncher
{
	public class Program
	{
		private const string DefaultGame = "DiscordRPCExample";

		[STAThread]
		public static void Main(string[] args)
		{
			//Create our Eto.Forms app, so we can show message boxes
			//We shut this down before we run the engine
			Platform.AllowReinitialize = true;
			Application app = new Application();

			IEntryPoint entryPoint = null;
			string dllPath = Path.GetFullPath($"{DefaultGame}/bin/{DefaultGame}.dll");
			try
			{
				//Load the game assembly
				AssemblyLoad assemblyLoad = new AssemblyLoad();
				Assembly gameDll = assemblyLoad.LoadAssembly(Path.GetFullPath($"{DefaultGame}/bin"), $"{DefaultGame}.dll");

				//Find a class the inherits from IEntryPoint so that we can create the game
				foreach (Type type in gameDll.GetTypes().Where(x => x.IsPublic && x.IsClass)) //Needs to be public
				{
					if (!typeof(IEntryPoint).IsAssignableFrom(type)) continue;

					if (!(Activator.CreateInstance(type) is IEntryPoint point)) continue;
					entryPoint = point;
					break;
				}
			}
			catch (FileNotFoundException ex) //The DLL wasn't found
			{
				Debug.Assert(false, $"The game DLL for '{DefaultGame}' wasn't found in '{dllPath}'!\n{ex}");
#if !DEBUG
				Eto.Forms.MessageBox.Show($"The game DLL for '{DefaultGame}' wasn't found in '{dllPath}'!", "Engine Error",
					Eto.Forms.MessageBoxButtons.OK, Eto.Forms.MessageBoxType.Error);
				app.Dispose();
				Environment.Exit(0);
#endif
			}
			catch (Exception ex) //Some other error
			{
				Debug.Assert(false, $"An unknown error occured while preparing the game for launching!\n{ex}");
#if !DEBUG
				Eto.Forms.MessageBox.Show($"An unknown error occured while preparing the game for launching!", "Engine Error",
					Eto.Forms.MessageBoxButtons.OK, Eto.Forms.MessageBoxType.Error);
				app.Dispose();
				Environment.Exit(0);
#endif
			}

			//The entry point wasn't found
			Debug.Assert(entryPoint != null, "The game DLL doesn't contain an entry point!");
#if !DEBUG
			if (entryPoint == null)
			{
				Eto.Forms.MessageBox.Show("The game DLL didn't contain an entry point!", "Engine Error", Eto.Forms.MessageBoxButtons.OK, 
					Eto.Forms.MessageBoxType.Error);
				app.Dispose();
				Environment.Exit(0);
				return;
			}
#endif

			//Dispose of the Eto.Forms app
			app.Quit();
			app.Dispose();

			//Tell the engine to init, and use the game entry point.
			//This is were we actually start to render and run the game.
			try
			{
				Engine.Init(entryPoint);
			}
			catch (Exception ex)
			{
				if(Logger.IsLoggerInitialized)
					Logger.Log(ex.ToString(), LogVerbosity.Error);
				//If the logger isn't initialized, then the only option we got to log the error is to dump it into console, as we disposed of Eto.Forms Application earlier
				//and can't create message boxes with out it
				else 
					Console.WriteLine(ex.ToString());
			}
		}
	}
}