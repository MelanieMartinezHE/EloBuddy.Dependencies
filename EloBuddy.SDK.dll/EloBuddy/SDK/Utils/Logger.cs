namespace EloBuddy.SDK.Utils
{
    using EloBuddy.SDK.Enumerations;
    using System;

    public static class Logger
    {
        public static void Debug(string message, params object[] args)
        {
            Log(EloBuddy.SDK.Enumerations.LogLevel.Debug, message, args);
        }

        public static void Error(string message, params object[] args)
        {
            Log(EloBuddy.SDK.Enumerations.LogLevel.Error, message, args);
        }

        public static void Exception(string headerMessage, object exceptionObject, params object[] args)
        {
            Exception(EloBuddy.SDK.Enumerations.LogLevel.Error, headerMessage, exceptionObject, args);
        }

        public static void Exception(EloBuddy.SDK.Enumerations.LogLevel logLevel, string headerMessage, object exceptionObject, params object[] args)
        {
            Log(logLevel, "", new object[0]);
            Log(logLevel, "===================================================", new object[0]);
            Log(logLevel, headerMessage, args);
            Log(logLevel, "", new object[0]);
            Log(logLevel, "Stacktrace of the Exception:", new object[0]);
            Log(logLevel, "", new object[0]);
            System.Exception innerException = exceptionObject as System.Exception;
            if (innerException > null)
            {
                object[] objArray1 = new object[] { innerException.GetType().FullName };
                Log(logLevel, "Type: {0}", objArray1);
                object[] objArray2 = new object[] { innerException.Message };
                Log(logLevel, "Message: {0}", objArray2);
                Log(logLevel, "", new object[0]);
                Log(logLevel, "Stracktrace:", new object[0]);
                Log(logLevel, innerException.StackTrace, new object[0]);
                innerException = innerException.InnerException;
                if (innerException > null)
                {
                    Log(logLevel, "", new object[0]);
                    Log(logLevel, "InnerException(s):", new object[0]);
                    do
                    {
                        Log(logLevel, "---------------------------------------------------", new object[0]);
                        object[] objArray3 = new object[] { innerException.GetType().FullName };
                        Log(logLevel, "Type: {0}", objArray3);
                        object[] objArray4 = new object[] { innerException.Message };
                        Log(logLevel, "Message: {0}", objArray4);
                        Log(logLevel, "", new object[0]);
                        Log(logLevel, "Stracktrace:", new object[0]);
                        Log(logLevel, innerException.StackTrace, new object[0]);
                        innerException = innerException.InnerException;
                    }
                    while (innerException > null);
                    Log(logLevel, "---------------------------------------------------", new object[0]);
                }
            }
            Log(logLevel, "===================================================", new object[0]);
            Log(logLevel, "", new object[0]);
        }

        public static void Info(string message, params object[] args)
        {
            Log(EloBuddy.SDK.Enumerations.LogLevel.Info, message, args);
        }

        public static void Log(EloBuddy.SDK.Enumerations.LogLevel logLevel, string message, params object[] args)
        {
            ConsoleColor foregroundColor = Console.ForegroundColor;
            switch (logLevel)
            {
                case EloBuddy.SDK.Enumerations.LogLevel.Debug:
                    foregroundColor = ConsoleColor.Cyan;
                    break;

                case EloBuddy.SDK.Enumerations.LogLevel.Error:
                    foregroundColor = ConsoleColor.Red;
                    break;

                case EloBuddy.SDK.Enumerations.LogLevel.Warn:
                    foregroundColor = ConsoleColor.Magenta;
                    break;
            }
            Console.ForegroundColor = foregroundColor;
            Console.WriteLine("[{0:H:mm:ss} - {1}] {2}", DateTime.Now, logLevel, string.Format(message, args));
            Console.ResetColor();
        }

        public static void Warn(string message, params object[] args)
        {
            Log(EloBuddy.SDK.Enumerations.LogLevel.Warn, message, args);
        }
    }
}

