using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using VoltstroEngine;
using VoltstroEngine.Logging;

namespace VoltstroEngineLauncher
{
	public class Program
	{
		private const string DefaultGame = "Sandbox";

		[STAThread]
		public static void Main(string[] args)
		{
			//Initiate the logger first
			Logger.InitiateLogger();

			IEntryPoint entryPoint = null;
			string dllPath = Path.GetFullPath($"{DefaultGame}/bin/{DefaultGame}.dll");
			try
			{
				//Load the game assembly
				Assembly gameDll = Assembly.LoadFile(dllPath);

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
				Logger.Log($"The game DLL for '{DefaultGame}' wasn't found in '{dllPath}'!\n{ex.Message}", LogVerbosity.Error);
				Environment.Exit(0);
#endif
			}
			catch (Exception ex) //Some other error
			{
				Debug.Assert(false, $"An unknown error occured while preparing the game for launching!\n{ex}");
#if !DEBUG
				Logger.Log($"An unknown error occured while preparing the game for launching!\n{ex.Message}", LogVerbosity.Error);
				Environment.Exit(0);
#endif
			}

			//The entry point wasn't found
			Debug.Assert(entryPoint != null, "The game DLL doesn't contain an entry point!");
#if !DEBUG
			if (entryPoint == null)
			{
				Logger.Log("The game DLL doesn't contain an entry point!", LogVerbosity.Error);
				Console.ReadLine();
				Environment.Exit(0);
				return;
			}
#endif

			//Create the app and run it
			try
			{
				entryPoint.CreateApplication().Run();
			}
			catch (Exception ex)
			{
				Logger.Log(ex.ToString(), LogVerbosity.Error);
				Console.ReadLine();
				Environment.Exit(0);
			}

			Logger.EndLogger();
		}
	}
}