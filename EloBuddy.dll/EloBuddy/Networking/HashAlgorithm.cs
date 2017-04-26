namespace EloBuddy.Networking
{
    using System;

    public class HashAlgorithm
    {
        private IntPtr m_address;

        public HashAlgorithm(IntPtr address)
        {
            this.m_address = address;
        }

        public byte[] Simulate() => 
            null;

        public IntPtr Address
        {
            get => 
                this.m_address;
            set
            {
                this.m_address = value;
            }
        }
    }
}

