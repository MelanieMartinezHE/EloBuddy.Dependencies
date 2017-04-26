namespace EloBuddy.Sandbox.Utils
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;

    internal static class Md5Hash
    {
        internal static bool Compare(string hash1, string hash2, bool skipInvalidHash = false)
        {
            if (IsValid(hash1) && IsValid(hash2))
            {
                return string.Equals(hash1, hash2, StringComparison.CurrentCultureIgnoreCase);
            }
            return skipInvalidHash;
        }

        internal static string Compute(byte[] inputBytes)
        {
            byte[] buffer;
            using (MD5 md = MD5.Create())
            {
                buffer = md.ComputeHash(inputBytes);
            }
            StringBuilder builder = new StringBuilder();
            foreach (byte num2 in buffer)
            {
                builder.Append(num2.ToString("x2"));
            }
            return builder.ToString();
        }

        internal static string Compute(string input) => 
            Compute(Encoding.UTF8.GetBytes(input));

        internal static string Compute(string input, string salt)
        {
            if (string.IsNullOrEmpty(salt))
            {
                return Compute(input);
            }
            return Compute(Compute(input) + Compute(salt));
        }

        internal static string ComputeFromFile(string path)
        {
            if (!File.Exists(path))
            {
                return string.Empty;
            }
            return Compute(File.ReadAllBytes(path));
        }

        internal static bool IsValid(string hash) => 
            new Regex("[0-9a-f]{32}").Match(hash).Success;
    }
}

