﻿using BepInEx.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System;

namespace Jotunn
{
    /// <summary>
    ///     A namespace wide Logger class, which automatically creates a ManualLogSource
    ///     for every Class from which it is being called.
    /// </summary>
    public class Logger
    {
        /// <summary>
        ///     Additional format prepended to the log entries header (between the severity/class and the text)
        /// </summary>
        public static string LogFormatExtra;

        private static Logger instance;

        private readonly Dictionary<string, ManualLogSource> logger = new Dictionary<string, ManualLogSource>();

        /// <summary>
        ///     Singleton init
        /// </summary>
        internal static void Init()
        {
            if (instance == null)
            {
                instance = new Logger();
            }
        }

        /// <summary>
        ///     Remove and clear all Logger instances
        /// </summary>
        internal static void Destroy()
        {
            LogDebug("Destroying Logger");

            foreach (var entry in instance.logger)
            {
                BepInEx.Logging.Logger.Sources.Remove(entry.Value);
            }

            instance.logger.Clear();
        }

        /// <summary>
        ///     Get or create a <see cref="ManualLogSource"/> with the callers <see cref="Type.FullName"/>
        /// </summary>
        /// <returns>A BepInEx <see cref="ManualLogSource"/></returns>
        private ManualLogSource GetLogger()
        {
            var type = new StackFrame(3).GetMethod().DeclaringType;

            ManualLogSource ret;
            if (!logger.TryGetValue(type.FullName, out ret))
            {
                ret = BepInEx.Logging.Logger.CreateLogSource(type.FullName);
                logger.Add(type.FullName, ret);
            }

            return ret;
        }

        private static void Log(LogLevel level, object data)
        {
            if (!string.IsNullOrEmpty(LogFormatExtra))
            {
                instance.GetLogger().Log(level, $"[{LogFormatExtra}] {data}");
            }
            else
            {
                instance.GetLogger().Log(level, data);
            }
        }

        /// <summary>
        ///     Logs a message with <see cref="BepInEx.Logging.LogLevel.Fatal"/> level.
        /// </summary>
        /// <param name="data">Data to log</param>
        public static void LogFatal(object data) => Log(LogLevel.Fatal, data);

        /// <summary>
        ///     Logs a message with <see cref="BepInEx.Logging.LogLevel.Fatal"/> level.
        /// </summary>
        /// <param name="data">Data to log</param>
        public static void LogError(object data) => Log(LogLevel.Error, data);

        /// <summary>
        ///     Logs a message with <see cref="BepInEx.Logging.LogLevel.Warning"/> level.
        /// </summary>
        /// <param name="data">Data to log</param>
        public static void LogWarning(object data) => Log(LogLevel.Warning, data);

        /// <summary>
        ///     Logs a message with <see cref="BepInEx.Logging.LogLevel.Message"/> level.
        /// </summary>
        /// <param name="data">Data to log</param>
        public static void LogMessage(object data) => Log(LogLevel.Message, data);

        /// <summary>
        ///     Logs a message with <see cref="BepInEx.Logging.LogLevel.Info"/> level.
        /// </summary>
        /// <param name="data">Data to log</param>
        public static void LogInfo(object data) => Log(LogLevel.Info, data);

        /// <summary>
        ///     Logs a message with <see cref="BepInEx.Logging.LogLevel.Debug"/> level.
        /// </summary>
        /// <param name="data">Data to log</param>
        public static void LogDebug(object data) => Log(LogLevel.Debug, data);
    }
}
