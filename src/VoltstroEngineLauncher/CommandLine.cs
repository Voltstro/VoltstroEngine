using System.CommandLine;
using System.CommandLine.Invocation;

namespace VoltstroEngineLauncher
{
	/// <summary>
	/// Handles setting up command line parameters
	/// </summary>
	internal static class CommandLine
	{
		/// <summary>
		/// The game name to launch
		/// </summary>
		public static string GameName;

		/// <summary>
		/// Parses arguments and sets the supported argument types in their appropriate variable
		/// </summary>
		/// <param name="args"></param>
		public static void ParseArguments(string[] args)
		{
			RootCommand rootCommand = new RootCommand
			{
				new Option<string>(
					"-game",
					getDefaultValue: () => "Sandbox",
					description: "The game to launch")
			};

			rootCommand.Description = "Voltstro Engine Launcher";
			rootCommand.Handler = CommandHandler.Create<string>((game) =>
			{
				GameName = game;
			});

			rootCommand.InvokeAsync(args);
		}
	}
}