namespace EloBuddy
{
    using System;
    using System.Runtime.CompilerServices;

    public static class Utility
    {
        public static byte Add(this byte num1, byte num2) => 
            ((byte) (num1 + num2));

        public static byte Dec(this byte num1) => 
            ((byte) (num1 - 1));

        public static byte Inc(this byte num1) => 
            ((byte) (num1 + 1));

        public static byte Not(this byte num) => 
            ~num;

        public static byte Rol(this byte value, int count) => 
            ((byte) ((value >> ((byte) (8 - count))) | ((byte) (value << count))));

        public static short Rol(this short value, int count) => 
            ((short) ((value >> ((short) (0x10 - count))) | ((short) (value << count))));

        public static int Rol(this int value, int count) => 
            ((value >> (0x20 - count)) | (value << count));

        public static uint Rol(this uint value, int count) => 
            ((value >> (0x20 - count)) | (value << count));

        public static byte Ror(this byte value, int count) => 
            ((byte) (((byte) (value << (8 - count))) | (value >> ((byte) count))));

        public static short Ror(this short value, int count)
        {
            int num = (int) (value < (0x10 - count));
            return (short) ((value >> ((short) count)) | ((short) num));
        }

        public static int Ror(this int value, int count) => 
            ((value << (0x20 - count)) | (value >> count));

        public static uint Ror(this uint value, int count) => 
            ((value << (0x20 - count)) | (value >> count));

        public static int Ror(this int num, int position, int length) => 
            ((num >> position) & ((1 << length) - 1));

        public static byte Sub(this byte num1, byte num2) => 
            ((byte) (num1 - num2));

        public static byte Xor(this byte num1, byte num2) => 
            ((byte) (num1 ^ num2));
    }
}

