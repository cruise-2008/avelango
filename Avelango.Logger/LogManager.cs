using System;
using System.IO;
using System.Text;
using NLog;
using NLog.Common;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;

using Avelango.Models.Abstractions.Accessory;


namespace Avelango.Logger
{
    /// <summary>
    /// NLog auto logger
    /// </summary>
    public class LogManager : ILog
    {

        private readonly ILogger _log = NLog.LogManager.GetCurrentClassLogger();

        public LogManager(string logFolder) {
            SetConfiguration(logFolder);
        }

        /// <summary>
        /// Add Info Event
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="data"></param>
        public void AddInfo(string eventName, string data) {
            _log.Info(eventName + " : " + data);
        }


        /// <summary>
        /// Add Error Event
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="data"></param>
        public void AddError(string eventName, string data) {
            _log.Error(eventName + " : " + data);
        }


        /// <summary>
        /// Add Fatal Event
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="data"></param>
        public void AddFatal(string eventName, string data) {
            _log.Fatal(eventName + " : " + data);
        }


        /// <summary>
        /// Add Debug Event
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="data"></param>
        public void AddDebug(string eventName, string data) {
            _log.Debug(eventName + " : " + data);
        }


        private static void SetConfiguration(string logFolder) {
            try {
                var config = new LoggingConfiguration();
                var filePath = logFolder + @"\" + DateTime.Now.Day.ToString("00") + "_" +
                               DateTime.Now.Month.ToString("00") + "_" +
                               DateTime.Now.Year.ToString("00") + ".log";
                var fileTarget = new FileTarget {
                    Name = "ErrorLog",
                    Layout = new SimpleLayout("${longdate} | ${level} | ${message}"),
                    Encoding = new UTF8Encoding(),
                    DeleteOldFileOnStartup = false,
                    ConcurrentWrites = true,
                    KeepFileOpen = false,
                    CreateDirs = true,
                    FileName = filePath,
                };
                config.AddTarget("file", fileTarget);

                var rule = new LoggingRule("*", LogLevel.Debug, fileTarget);
                rule.EnableLoggingForLevel(LogLevel.Debug);
                config.LoggingRules.Add(rule);

                NLog.LogManager.Configuration = config;
            }
            catch {
                InternalLogger.LogFile = logFolder + @"internal_exceptions.log";
                InternalLogger.LogWriter = new StringWriter();
                InternalLogger.LogLevel = LogLevel.Fatal;
            }
        }

    }
}
