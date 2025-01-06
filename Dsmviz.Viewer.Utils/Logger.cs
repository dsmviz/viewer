using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Dsmviz.Viewer.Utils
{
    /// <summary>
    /// Provides logging to be used for diagnostic purposes
    /// </summary>
    public class Logger
    {
        private static Assembly? _assembly;
        private static string? _logPath;

        public static DirectoryInfo? LogDirectory { get; private set; }

        public static void Init(Assembly assembly, bool logInCurrentDirectory)
        {
            _assembly = assembly;
            _logPath = logInCurrentDirectory ? Directory.GetCurrentDirectory() : @"C:\Temp\DsmvizLogging\";

            LogLevel = LogLevel.None;
        }

        public static LogLevel LogLevel { get; set; }

        public static void LogResourceUsage()
        {
            Process currentProcess = Process.GetCurrentProcess();
            const long million = 1000000;
            long peakPagedMemMb = currentProcess.PeakPagedMemorySize64 / million;
            long peakVirtualMemMb = currentProcess.PeakVirtualMemorySize64 / million;
            long peakWorkingSetMb = currentProcess.PeakWorkingSet64 / million;
            LogUserMessage($"Peak physical memory usage {peakWorkingSetMb:0.000}MB");
            LogUserMessage($"Peak paged memory usage    {peakPagedMemMb:0.000}MB");
            LogUserMessage($"Peak virtual memory usage  {peakVirtualMemMb:0.000}MB");
        }

        public static void LogAssemblyInfo()
        {
            string name = _assembly?.GetName().Name ?? string.Empty;
            string version = _assembly?.GetName().Version?.ToString() ?? string.Empty;
            DateTime buildDate = (_assembly != null) ? new FileInfo(_assembly.Location).LastWriteTime : DateTime.Now;
            LogUserMessage(name + " version =" + version + " build=" + buildDate);
        }

        public static void LogUserMessage(string message,
            [CallerFilePath] string sourceFile = "",
            [CallerMemberName] string method = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            Console.WriteLine(message);
            LogToFile(LogLevel.User, "userMessages.log", message);
        }

        public static void LogError(string message,
            [CallerFilePath] string sourceFile = "",
            [CallerMemberName] string method = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            LogToFile(LogLevel.Error, "errorMessages.log", FormatLine(sourceFile, method, lineNumber, "error", message));
        }

        public static void LogException(string message, Exception e,
            [CallerFilePath] string sourceFile = "",
            [CallerMemberName] string method = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            LogToFile(LogLevel.Error, "exceptions.log", FormatLine(sourceFile, method, lineNumber, message, e.Message));
            LogToFile(LogLevel.Error, "exceptions.log", e.StackTrace ?? string.Empty);
            LogToFile(LogLevel.Error, "exceptions.log", "");
        }

        public static void LogWarning(string message,
            [CallerFilePath] string sourceFile = "",
            [CallerMemberName] string method = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            LogToFile(LogLevel.Warning, "warningMessages.log", FormatLine(sourceFile, method, lineNumber, "error", message));
        }

        public static void LogInfo(string message,
            [CallerFilePath] string sourceFile = "",
            [CallerMemberName] string method = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            LogToFile(LogLevel.Info, "infoMessages.log", FormatLine(sourceFile, method, lineNumber, "info", message));
        }

        public static void LogDataModelMessage(string message,
            [CallerFilePath] string sourceFile = "",
            [CallerMemberName] string method = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            LogToFile(LogLevel.Data, "dataModelMessages.log", FormatLine(sourceFile, method, lineNumber, "info", message));
        }

        public static void LogToFile(LogLevel logLevel, string logFilename, string line)
        {
            if (LogLevel >= logLevel)
            {
                string path = GetLogFullPath(logFilename);
                FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write);
                using StreamWriter writer = new StreamWriter(fs);
                writer.WriteLine(line);
            }
        }

        private static DirectoryInfo CreateLogDirectory()
        {
            DateTime now = DateTime.Now;
            string timestamp = $"{now.Year:0000}-{now.Month:00}-{now.Day:00}-{now.Hour:00}-{now.Minute:00}-{now.Second:00}";
            string assemblyName = _assembly?.GetName().Name ?? "Dsmviz";
            return Directory.CreateDirectory($@"{_logPath}\{assemblyName}_{timestamp}\");
        }

        private static string GetLogFullPath(string logFilename)
        {
            LogDirectory ??= CreateLogDirectory();

            return Path.GetFullPath(Path.Combine(LogDirectory.FullName, logFilename));
        }

        private static string FormatLine(string sourceFile, string method, int lineNumber, string category, string text)
        {
            return StripPath(sourceFile) + " " + method + "() line=" + lineNumber + " " + category + "=" + text;
        }

        private static string StripPath(string sourceFile)
        {
            char[] separators = ['\\'];
            string[] parts = sourceFile.Split(separators);
            return parts[^1];
        }
    }
}
