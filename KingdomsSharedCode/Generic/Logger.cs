using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

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
            {LEVEL.DEBUG, ConsoleColor.White },
            {LEVEL.INFO, ConsoleColor.White },
            {LEVEL.WARNING, ConsoleColor.Yellow },
            {LEVEL.ERROR, ConsoleColor.Red }
        };

        public static void SetLevel(LEVEL level)
        {
            Logger.level = level;
        }

        public static void Trace(params string[] msgs) { LogMessage(LEVEL.TRACE, msgs); }
        public static void Debug(params string[] msgs) { LogMessage(LEVEL.DEBUG, msgs); }
        public static void Info(params string[] msgs) { LogMessage(LEVEL.INFO, msgs); }
        public static void Warn(params string[] msgs) { LogMessage(LEVEL.WARNING, msgs); }
        public static void Error(params string[] msgs) { LogMessage(LEVEL.ERROR, msgs); }
        public static void Throw(Exception e)
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

            // Formatting the "X=>Y" file/method pattern
#if DEBUG
            StackFrame sf = new StackFrame(2, true);
            string file = sf.GetFileName();
            string method = sf.GetMethod().Name;
            try
            {
                caller =
                    file.Substring(0, Math.Min(file.Length, 10)).PadRight(10)
                    + "=>"
                    + method.Substring(0, Math.Min(method.Length, 14)).PadRight(14);
            }
            catch (NullReferenceException)
            {
                caller = "???";
            }
#endif

            // Debug line formatting
            string line = "{0} [{1}] [{2}]:{3}";
            string.Format(line, DateTime.Now.ToString(culture.DateTimeFormat.LongTimePattern), msgLevel.ToString(), caller, string.Join(" ", msgs));

        }
    }
}
