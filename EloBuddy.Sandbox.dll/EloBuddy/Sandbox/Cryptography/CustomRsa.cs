namespace EloBuddy.Sandbox.Cryptography
{
    using System;
    using System.IO;
    using System.Numerics;

    internal static class CustomRsa
    {
        internal static byte[] Decode(byte[] array, BigInteger exponent, BigInteger modulus) => 
            BigInteger.ModPow(new BigInteger(array), exponent, modulus).ToByteArray();

        internal static byte[] DecodeBlock(byte[] array, BigInteger exponent, BigInteger modulus)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    using (MemoryStream stream2 = new MemoryStream(array))
                    {
                        using (BinaryReader reader = new BinaryReader(stream2))
                        {
                            while (reader.PeekChar() != -1)
                            {
                                ushort count = BitConverter.ToUInt16(reader.ReadBytes(2), 0);
                                if (count == 0)
                                {
                                    break;
                                }
                                byte[] buffer = reader.ReadBytes(count);
                                writer.Write(Decode(buffer, exponent, modulus));
                            }
                            return stream.ToArray();
                        }
                    }
                }
            }
        }
    }
}

