﻿using DiscordRPC.Logging;
using VoltstroEngine.Core.Logging;

namespace DiscordRPCExample
{
	/// <summary>
	/// This is the custom <see cref="ILogger"/> for the <see cref="DiscordRPC.DiscordRpcClient"/> using VE's
	/// <see cref="Logger"/>
	/// </summary>
	public sealed class VEDiscordLogger : ILogger
	{
		public LogLevel Level { get; set; }

		public void Trace(string message, params object[] args)
		{
			if (Level != LogLevel.Trace) return;
			Logger.Debug($"[IPC Trace] {(args.Length > 0 ? string.Format(message, args) : message)}");
		}

		public void Info(string message, params object[] args)
		{
			if (Level != LogLevel.Info) return;
			Logger.Info($"[IPC] {(args.Length > 0 ? string.Format(message, args) : message)}");
		}

		public void Warning(string message, params object[] args)
		{
			if (Level != LogLevel.Warning) return;
			Logger.Warn($"[IPC] {(args.Length > 0 ? string.Format(message, args) : message)}");
		}

		public void Error(string message, params object[] args)
		{
			if (Level != LogLevel.Error) return;
			Logger.Error($"[IPC] {(args.Length > 0 ? string.Format(message, args) : message)}");
		}
	}
}