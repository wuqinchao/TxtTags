using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TxtTags.Common;

namespace TxtTags.Service
{
    public static class LogService
    {
        public static void Fatal(string owner, String message, Exception exp = null)
        {
            if (Config.Instance.LogLevel <= LogLevel.Fatal)
            {
                WriteLog(LogLevel.Fatal, owner, message, exp);
            }
        }
        public static void Error(string owner, String message, Exception exp = null)
        {
            if (Config.Instance.LogLevel <= LogLevel.Error)
            {
                WriteLog(LogLevel.Error, owner, message, exp);
            }
        }
        public static void Warn(string owner, String message, Exception exp = null)
        {
            if (Config.Instance.LogLevel <= LogLevel.Warn)
            {
                WriteLog(LogLevel.Warn, owner, message, exp);
            }
        }
        public static void Info(string owner, String message, Exception exp = null)
        {
            if (Config.Instance.LogLevel <= LogLevel.Info)
            {
                WriteLog(LogLevel.Info, owner, message, exp);
            }
        }
        public static void Debug(string owner, String message, Exception exp = null)
        {
            if (Config.Instance.LogLevel <= LogLevel.Debug)
            {
                WriteLog(LogLevel.Debug, owner, message, exp);
            }
        }
        private static void WriteLog(LogLevel level, string owner, string message, Exception exp)
        {
            Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.fff")} [{owner}][{level}][{System.Threading.Thread.CurrentThread.ManagedThreadId}] {message}");
            if (exp != null)
            {
                Console.WriteLine(exp.ToString());
            }
        }
        private static string LogFormatException(Exception ex)
        {
            if (ex == null) return string.Empty;
            else
            {
                return ex.Message + "\r\n" + ex.ToString();
            }
        }
        public struct ConsoleItem
        {
            public LogLevel level;
            public string message;
            public Exception exp;
        }
    }
}
