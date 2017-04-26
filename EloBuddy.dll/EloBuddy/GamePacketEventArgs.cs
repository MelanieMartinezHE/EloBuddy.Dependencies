namespace EloBuddy
{
    using EloBuddy.Networking;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class GamePacketEventArgs : EventArgs
    {
        private PacketChannel m_channel;
        private PacketProtocolFlags m_flag;
        private IntPtr m_hashAlgorithm;
        private uint m_networkId;
        private short m_opCode;
        private byte[] m_packetData;
        private bool m_process;
        private byte[] m_rawPacket;

        public GamePacketEventArgs(short opCode, uint networkId, byte[] packetData, byte[] rawPacketData, IntPtr hashAlgorithm, PacketChannel channel, PacketProtocolFlags flag)
        {
            this.m_channel = channel;
            this.m_flag = flag;
            this.m_opCode = opCode;
            this.m_networkId = networkId;
            this.m_process = true;
            this.m_packetData = packetData;
            this.m_rawPacket = rawPacketData;
            this.m_hashAlgorithm = hashAlgorithm;
        }

        public PacketChannel Channel =>
            this.m_channel;

        public EloBuddy.Networking.GamePacket GamePacket =>
            new EloBuddy.Networking.GamePacket(this.m_rawPacket);

        public IntPtr HashAlgorithm =>
            this.m_hashAlgorithm;

        public uint NetworkId =>
            this.m_networkId;

        public short OpCode =>
            this.m_opCode;

        public byte[] PacketData =>
            this.m_packetData;

        public bool Process
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get => 
                this.m_process;
            [param: MarshalAs(UnmanagedType.U1)]
            set
            {
                this.m_process = value;
            }
        }

        public PacketProtocolFlags ProtocolFlag =>
            this.m_flag;

        public byte[] RawPacketData =>
            this.m_rawPacket;

        public delegate void GamePacketEvent(EloBuddy.GamePacketEventArgs args);
    }
}

