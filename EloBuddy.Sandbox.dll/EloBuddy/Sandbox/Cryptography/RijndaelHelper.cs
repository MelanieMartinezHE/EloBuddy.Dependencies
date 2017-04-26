namespace EloBuddy.Sandbox.Cryptography
{
    using System;
    using System.IO;
    using System.Security.Cryptography;

    internal static class RijndaelHelper
    {
        internal static byte[] Decrypt(byte[] buffer, byte[] password, byte[] salt, int iterations)
        {
            byte[] buffer2;
            using (Rijndael rijndael = Rijndael.Create())
            {
                Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, salt, iterations);
                rijndael.Key = bytes.GetBytes(0x20);
                rijndael.IV = bytes.GetBytes(0x10);
                using (MemoryStream stream = new MemoryStream())
                {
                    using (CryptoStream stream2 = new CryptoStream(stream, rijndael.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        stream2.Write(buffer, 0, buffer.Length);
                        stream2.Close();
                        buffer2 = stream.ToArray();
                    }
                }
            }
            return buffer2;
        }
    }
}

