namespace EloBuddy.Sandbox
{
    using System;
    using System.Security.Permissions;

    internal static class Logs
    {
        internal static void Log(string text, params object[] args)
        {
            Console.WriteLine(text, args);
        }

        [PermissionSet(SecurityAction.Assert, Unrestricted=true)]
        internal static void PrintException(object exceptionObject)
        {
            Log("", new object[0]);
            Log("===================================================", new object[0]);
            Log("An exception ocurred! EloBuddy might crash!", new object[0]);
            Log("", new object[0]);
            Exception innerException = exceptionObject as Exception;
            if (innerException != null)
            {
                object[] args = new object[] { innerException.GetType().FullName };
                Log("Type: {0}", args);
                object[] objArray2 = new object[] { innerException.Message };
                Log("Message: {0}", objArray2);
                Log("", new object[0]);
                Log("Stracktrace:", new object[0]);
                Log(innerException.StackTrace, new object[0]);
                innerException = innerException.InnerException;
                if (innerException != null)
                {
                    Log("", new object[0]);
                    Log("InnerException(s):", new object[0]);
                    do
                    {
                        Log("---------------------------------------------------", new object[0]);
                        object[] objArray3 = new object[] { innerException.GetType().FullName };
                        Log("Type: {0}", objArray3);
                        object[] objArray4 = new object[] { innerException.Message };
                        Log("Message: {0}", objArray4);
                        Log("", new object[0]);
                        Log("Stracktrace:", new object[0]);
                        Log(innerException.StackTrace, new object[0]);
                        innerException = innerException.InnerException;
                    }
                    while (innerException != null);
                    Log("---------------------------------------------------", new object[0]);
                }
            }
            Log("===================================================", new object[0]);
            Log("", new object[0]);
        }
    }
}

