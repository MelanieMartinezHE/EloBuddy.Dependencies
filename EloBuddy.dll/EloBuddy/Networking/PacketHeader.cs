namespace EloBuddy.Networking
{
    using System;

    public class PacketHeader
    {
        private HashAlgorithm m_algorithm;
        private short m_header;
        private uint m_networkId;

        public PacketHeader(GamePacket packet)
        {
            if (packet.Data.Length < 10)
            {
                throw new Exception("Tried to access the header of a Packet that does not contain atleast 10 bytes.");
            }
            IntPtr address = new IntPtr(BitConverter.ToInt32(packet.Data, 0));
            this.m_algorithm = new HashAlgorithm(address);
            this.m_header = BitConverter.ToInt16(packet.Data, 4);
            this.m_networkId = BitConverter.ToUInt32(packet.Data, 6);
        }

        public HashAlgorithm Algorithm
        {
            get => 
                this.m_algorithm;
            set
            {
                this.m_algorithm = value;
            }
        }

        public uint NetworkId
        {
            get => 
                this.m_networkId;
            set
            {
                this.m_networkId = value;
            }
        }

        public short OpCode
        {
            get => 
                this.m_header;
            set
            {
                this.m_header = value;
            }
        }
    }
}

