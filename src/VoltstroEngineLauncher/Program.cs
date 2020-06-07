using System;
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
			Logger.InitiateLogger();

			try
			{
				string dllPath = Path.GetFullPath($"{DefaultGame}/bin/{DefaultGame}.dll");
				Assembly gameDll = Assembly.LoadFile(dllPath);

				IEntryPoint entryPoint = null;

				foreach (Type type in gameDll.GetTypes().Where(x => x.IsPublic))
				{
					if (typeof(IEntryPoint).IsAssignableFrom(type))
					{
						if (Activator.CreateInstance(type) is IEntryPoint point)
						{
							entryPoint = point;
							break;
						}
					}
				}

				if (entryPoint == null)
				{
					Logger.Log("The game DLL doesn't contain an entry point!", LogVerbosity.Error);
					Console.ReadLine();
					Environment.Exit(0);
				}

				entryPoint.CreateApplication().Run();
			}
			catch (FileNotFoundException)
			{
				Logger.Log("The game DLL failed to load!", LogVerbosity.Error);
				Console.ReadLine();
				Environment.Exit(0);
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