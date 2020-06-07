using System;
using System.IO;
using System.Linq;
using System.Reflection;
using VoltstroEngine;

namespace VoltstroEngineLauncher
{
	public class Program
	{
		private static string defaultGame = "Sandbox";

		[STAThread]
		public static void Main(string[] args)
		{
			try
			{
				string dllPath = Path.GetFullPath($"{defaultGame}/bin/{defaultGame}.dll");
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
					Console.WriteLine("The game DLL doesn't contain an entry point!");
					Console.ReadLine();
					Environment.Exit(0);
				}

				entryPoint.CreateApplication().Run();
			}
			catch (FileNotFoundException)
			{
				Console.WriteLine("The game DLL failed to load!");
				Console.ReadLine();
				Environment.Exit(0);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				Console.ReadLine();
				Environment.Exit(0);
			}
		}
	}
}
