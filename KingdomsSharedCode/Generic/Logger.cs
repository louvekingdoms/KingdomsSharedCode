using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;

namespace KingdomsSharedCode.Generic
{
    static class Logger
    {
        public enum LEVEL { TRACE, DEBUG, WARNING, INFO, ERROR};

        static LEVEL level;
        static CultureInfo culture = new CultureInfo("fr-FR");
        static Dictionary<LEVEL, ConsoleColor> colors = new Dictionary<LEVEL, ConsoleColor>()
        {
            {LEVEL.TRACE, ConsoleColor.Magenta },
            {LEVEL.DEBUG, ConsoleColor.Gray },
            {LEVEL.INFO, ConsoleColor.White },
            {LEVEL.WARNING, ConsoleColor.Yellow },
            {LEVEL.ERROR, ConsoleColor.Red }
        };
        static int flushEvery = 1000;

        static bool outputToFile = false;
        static bool outputToConsole = true;

        static string logFilePath = @"logs/{0}.log";
        static FileStream logFileStream = null;
        static string programName;
        static Timer flushTimer;
        static Action<object> logFunction = (Action<object>)Console.WriteLine;

        static Logger()
        {
            Initialize(Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName));
        }

        public static void Initialize(string programName)
        {
            Logger.programName = programName;

            if (outputToFile)
            {
                Directory.CreateDirectory(
                Path.GetDirectoryName(
                        logFilePath
                    )
                );
                if (flushTimer != null) flushTimer.Dispose();
                if (logFileStream != null) logFileStream.Dispose();
                if (File.Exists(logFilePath)) File.Delete(logFilePath);

                logFileStream = new FileStream(string.Format(logFilePath, programName), FileMode.Create, FileAccess.Write, FileShare.Read);

                flushTimer = new Timer(
                    e =>
                    {
                        logFileStream.Flush();
                    },
                    null,
                    TimeSpan.Zero,
                    TimeSpan.FromMilliseconds(flushEvery));
            }
        }

        public static void SetLevel(LEVEL level)
        {
            Logger.level = level;
        }

        public static void SetConsoleFunction(Action<object> function)
        {
            logFunction = function;
        }
        
        public static void Trace(params string[] msgs) { LogMessage(LEVEL.TRACE, msgs); }
        public static void Debug(params string[] msgs) { LogMessage(LEVEL.DEBUG, msgs); }
        public static void Info(params string[] msgs) { LogMessage(LEVEL.INFO, msgs); }
        public static void Warn(params string[] msgs) { LogMessage(LEVEL.WARNING, msgs); }
        public static void Error(params string[] msgs) { LogMessage(LEVEL.ERROR, msgs); }
        public static void Fatal(Exception e)
        {
            LogMessage(LEVEL.ERROR, new string[1] { "================== FATAL ==================" });
            LogMessage(LEVEL.ERROR, e);
            Console.ReadKey();
            Environment.Exit(1);
        }

        private static void LogMessage(LEVEL msgLevel, params object[] msgs)
        {
            if (msgLevel < level)
            {
                return;
            }

            string caller = "---";

#if DEBUG
            StackFrame sf = new StackFrame(2, true);
            string file = sf.GetMethod().DeclaringType.Name;
            string method = sf.GetMethod().Name;
            try
            {
                caller = string.Format("{0}   {1}",
                    file.Substring(0, Math.Min(file.Length, 8)).PadRight(8),
                    method.Substring(0, Math.Min(method.Length, 14)).PadRight(14)
                );
            }
            catch (NullReferenceException)
            {
                caller = "???";
            }
#endif

            // Debug line formatting
            string line = "{0} [{1}] [{2}]:{3}";
            line = string.Format(line, DateTime.Now.ToString(culture.DateTimeFormat.LongTimePattern), msgLevel.ToString(), caller, string.Join(" ", msgs));

            if (outputToConsole)
            {
                Console.ForegroundColor = colors[msgLevel];
                logFunction(line);
            }

            if (outputToFile)
            {
                using (StreamWriter sw = new StreamWriter(logFileStream, Encoding.UTF8, 1024, leaveOpen:true))
                {
                    sw.WriteLine(line);
                }
            }
        }
    }
}
