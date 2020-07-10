﻿using System;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using VoltstroEngine.Exceptions;

namespace VoltstroEngine.Core.Logging
{
	/// <summary>
	/// Provides the ability to log stuff to a file and the console
	/// </summary>
	public static class Logger
	{
		private static Serilog.Core.Logger log;

		private static LoggerConfig loggerConfig;

		/// <summary>
		/// The logger's config, can only be set while the logger isn't running
		/// </summary>
		public static LoggerConfig LoggerConfig 
		{
			set
			{
				if(IsLoggerInitialized)
					throw new InitializationException("The logger is already initialized!");

				loggerConfig = value;
			}
			get => loggerConfig;
		}

		/// <summary>
		/// Is the logger initialized?
		/// <para>Returns true if it is</para>
		/// </summary>
		public static bool IsLoggerInitialized => log != null;

		/// <summary>
		/// Initializes the logger
		/// </summary>
		/// <exception cref="InitializationException"></exception>
		internal static void Init()
		{
			if(IsLoggerInitialized)
				throw new InitializationException("The logger is already initialized!");

			if(LoggerConfig == null)
				LoggerConfig = new LoggerConfig();

			LoggingLevelSwitch level = new LoggingLevelSwitch()
			{
#if DEBUG
				MinimumLevel = LogEventLevel.Debug
#endif
			};

			const string outPutTemplate = "{Timestamp:dd-MM hh:mm:ss tt} [{Level:u3}] {Message:lj}{NewLine}{Exception}";
			string logFileName = $"{loggerConfig.LogDirectory}{DateTime.Now.ToString(loggerConfig.LogFileDateTimeFormat)}.log";

			log = new LoggerConfiguration()
				.MinimumLevel.ControlledBy(level)
				.WriteTo.Console(outputTemplate: outPutTemplate)
				.WriteTo.Async(a => a.File(logFileName, outputTemplate: outPutTemplate, buffered: loggerConfig.BufferedFileWrite))
				.CreateLogger();

			log.Debug("Logger initialized at {@Date}", DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"));
		}

		/// <summary>
		/// Shuts down the logger
		/// </summary>
		/// <exception cref="InitializationException"></exception>
		internal static void Shutdown()
		{
			if(!IsLoggerInitialized)
				throw new InitializationException("The logger isn't initialized!");

			log.Debug("Logger shutting down at {@Date}", DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"));
			log.Dispose();
		}

		#region Debug Logging

		/// <summary>
		/// Writes a debug log 
		/// </summary>
		/// <param name="message"></param>
		public static void Debug(string message)
		{
			if(!IsLoggerInitialized)
				throw new InitializationException("The logger isn't initialized!");

			log.Debug(message);
		}

		/// <summary>
		/// Writes a debug log 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="value"></param>
		public static void Debug<T>(string message, T value)
		{
			if(!IsLoggerInitialized)
				throw new InitializationException("The logger isn't initialized!");

			log.Debug(message, value);
		}

		/// <summary>
		/// Writes a debug log 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="value0"></param>
		/// <param name="value1"></param>
		public static void Debug<T>(string message, T value0, T value1)
		{
			if(!IsLoggerInitialized)
				throw new InitializationException("The logger isn't initialized!");

			log.Debug(message, value0, value1);
		}

		/// <summary>
		/// Writes a debug log 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="value0"></param>
		/// <param name="value1"></param>
		/// <param name="value2"></param>
		public static void Debug<T>(string message, T value0, T value1, T value2)
		{
			if(!IsLoggerInitialized)
				throw new InitializationException("The logger isn't initialized!");

			log.Debug(message, value0, value1, value2);
		}

		/// <summary>
		/// Writes a debug log 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="values"></param>
		public static void Debug(string message, params object[] values)
		{
			if(!IsLoggerInitialized)
				throw new InitializationException("The logger isn't initialized!");

			log.Debug(message, values);
		}

		#endregion

		#region Information Logging

		/// <summary>
		/// Writes a information log
		/// </summary>
		/// <param name="message"></param>
		public static void Info(string message)
		{
			if(!IsLoggerInitialized)
				throw new InitializationException("The logger isn't initialized!");

			log.Information(message);
		}

		/// <summary>
		/// Writes a information log
		/// </summary>
		/// <param name="message"></param>
		/// <param name="value"></param>
		public static void Info<T>(string message, T value)
		{
			if(!IsLoggerInitialized)
				throw new InitializationException("The logger isn't initialized!");

			log.Information(message, value);
		}

		/// <summary>
		/// Writes a information log
		/// </summary>
		/// <param name="message"></param>
		/// <param name="value0"></param>
		/// <param name="value1"></param>
		public static void Info<T>(string message, T value0, T value1)
		{
			if(!IsLoggerInitialized)
				throw new InitializationException("The logger isn't initialized!");

			log.Information(message, value0, value1);
		}

		/// <summary>
		/// Writes a information log
		/// </summary>
		/// <param name="message"></param>
		/// <param name="value0"></param>
		/// <param name="value1"></param>
		/// <param name="value2"></param>
		public static void Info<T>(string message, T value0, T value1, T value2)
		{
			if(!IsLoggerInitialized)
				throw new InitializationException("The logger isn't initialized!");

			log.Information(message, value0, value1, value2);
		}

		/// <summary>
		/// Writes a information log
		/// </summary>
		/// <param name="message"></param>
		/// <param name="values"></param>
		public static void Info(string message, params object[] values)
		{
			if(!IsLoggerInitialized)
				throw new InitializationException("The logger isn't initialized!");

			log.Information(message, values);
		}

		#endregion

		#region Warning Logging

		/// <summary>
		/// Writes a warning log
		/// </summary>
		/// <param name="message"></param>
		public static void Warn(string message)
		{
			if(!IsLoggerInitialized)
				throw new InitializationException("The logger isn't initialized!");

			log.Warning(message);
		}

		/// <summary>
		/// Writes a warning log
		/// </summary>
		/// <param name="message"></param>
		/// <param name="value"></param>
		public static void Warn<T>(string message, T value)
		{
			if(!IsLoggerInitialized)
				throw new InitializationException("The logger isn't initialized!");

			log.Warning(message, value);
		}

		/// <summary>
		/// Writes a warning log
		/// </summary>
		/// <param name="message"></param>
		/// <param name="value0"></param>
		/// <param name="value1"></param>
		public static void Warn<T>(string message, T value0, T value1)
		{
			if(!IsLoggerInitialized)
				throw new InitializationException("The logger isn't initialized!");

			log.Warning(message, value0, value1);
		}

		/// <summary>
		/// Writes a warning log
		/// </summary>
		/// <param name="message"></param>
		/// <param name="value0"></param>
		/// <param name="value1"></param>
		/// <param name="value2"></param>
		public static void Warn<T>(string message, T value0, T value1, T value2)
		{
			if(!IsLoggerInitialized)
				throw new InitializationException("The logger isn't initialized!");

			log.Warning(message, value0, value1, value2);
		}

		/// <summary>
		/// Writes a warning log
		/// </summary>
		/// <param name="message"></param>
		/// <param name="values"></param>
		public static void Warn(string message, params object[] values)
		{
			if(!IsLoggerInitialized)
				throw new InitializationException("The logger isn't initialized!");

			log.Warning(message, values);
		}

		#endregion

		#region Warning Logging

		/// <summary>
		/// Writes a error log
		/// </summary>
		/// <param name="message"></param>
		public static void Error(string message)
		{
			if(!IsLoggerInitialized)
				throw new InitializationException("The logger isn't initialized!");

			log.Error(message);
		}

		/// <summary>
		/// Writes a error log
		/// </summary>
		/// <param name="message"></param>
		/// <param name="value"></param>
		public static void Error<T>(string message, T value)
		{
			if(!IsLoggerInitialized)
				throw new InitializationException("The logger isn't initialized!");

			log.Error(message, value);
		}

		/// <summary>
		/// Writes a error log
		/// </summary>
		/// <param name="message"></param>
		/// <param name="value0"></param>
		/// <param name="value1"></param>
		public static void Error<T>(string message, T value0, T value1)
		{
			if(!IsLoggerInitialized)
				throw new InitializationException("The logger isn't initialized!");

			log.Error(message, value0, value1);
		}

		/// <summary>
		/// Writes a error log
		/// </summary>
		/// <param name="message"></param>
		/// <param name="value0"></param>
		/// <param name="value1"></param>
		/// <param name="value2"></param>
		public static void Error<T>(string message, T value0, T value1, T value2)
		{
			if(!IsLoggerInitialized)
				throw new InitializationException("The logger isn't initialized!");

			log.Error(message, value0, value1, value2);
		}

		/// <summary>
		/// Writes a error log
		/// </summary>
		/// <param name="message"></param>
		/// <param name="values"></param>
		public static void Error(string message, params object[] values)
		{
			if(!IsLoggerInitialized)
				throw new InitializationException("The logger isn't initialized!");

			log.Error(message, values);
		}

		#endregion
	}
}