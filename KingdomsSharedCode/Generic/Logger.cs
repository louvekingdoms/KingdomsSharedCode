﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;

namespace KingdomsSharedCode.Generic
{
    public class Logger
    {
        public enum LEVEL { TRACE, DEBUG, WARNING, INFO, ERROR};

        LEVEL level;
        CultureInfo culture = new CultureInfo("fr-FR");
        Dictionary<LEVEL, ConsoleColor> colors = new Dictionary<LEVEL, ConsoleColor>()
        {
            {LEVEL.TRACE, ConsoleColor.Magenta },
            {LEVEL.DEBUG, ConsoleColor.Gray },
            {LEVEL.INFO, ConsoleColor.White },
            {LEVEL.WARNING, ConsoleColor.Yellow },
            {LEVEL.ERROR, ConsoleColor.Red }
        };
        int flushEvery = 1000;

        bool outputToFile = false;
        bool outputToConsole = true;

        string logFilePath = @"logs/{0}.log";
        FileStream logFileStream = null;
        string programName;
        Timer flushTimer;
        Action<object> logFunction = (Action<object>)Console.WriteLine;

        public Logger(string programName=null, bool outputToFile = false, bool outputToConsole = true)
        {
            if (programName == null) programName = Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName);

            Initialize(programName, outputToFile, outputToConsole);
        }

        void Initialize(string programName, bool outputToFile=false, bool outputToConsole=true)
        {
            this.programName = programName;
            this.outputToFile = outputToFile;
            this.outputToConsole = outputToConsole;

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

        public void SetLevel(LEVEL level)
        {
            this.level = level;
        }

        public void SetConsoleFunction(Action<object> function)
        {
            this.logFunction = function;
        }
        
        public void Trace(params object[] msgs) { LogMessage(LEVEL.TRACE, msgs); }
        public void Debug(params object[] msgs) { LogMessage(LEVEL.DEBUG, msgs); }
        public void Info(params object[] msgs) { LogMessage(LEVEL.INFO, msgs); }
        public void Warn(params object[] msgs) { LogMessage(LEVEL.WARNING, msgs); }
        public void Error(params object[] msgs) { LogMessage(LEVEL.ERROR, msgs); }
        public void Fatal(Exception e)
        {
            LogMessage(LEVEL.ERROR, new string[1] { "================== FATAL ==================" });
            LogMessage(LEVEL.ERROR, e);
            Console.ReadKey();
            Environment.Exit(1);
        }

        void LogMessage(LEVEL msgLevel, params object[] msgs)
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
