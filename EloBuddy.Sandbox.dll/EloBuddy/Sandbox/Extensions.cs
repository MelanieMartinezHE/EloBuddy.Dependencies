namespace EloBuddy.Sandbox
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    internal static class Extensions
    {
        internal static string GenerateToken(this AssemblyName assemblyName)
        {
            string[] second = new string[] { string.Empty };
            return (assemblyName.Name + (from o in assemblyName.GetPublicKeyToken() select o.ToString("x2")).Concat<string>(second).Aggregate<string>(new Func<string, string, string>(string.Concat)));
        }

        internal static bool IsDosExecutable(this byte[] buffer)
        {
            if (buffer.Length < 5)
            {
                return false;
            }
            return (((((buffer[0] == 0x4d) && (buffer[1] == 90)) && ((buffer[2] == 0x90) && (buffer[3] == 0))) && (buffer[4] == 3)) && (buffer[4] == 0));
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly Extensions.<>c <>9 = new Extensions.<>c();
            public static Func<byte, string> <>9__0_0;

            internal string <GenerateToken>b__0_0(byte o) => 
                o.ToString("x2");
        }
    }
}

