namespace EloBuddy.Networking
{
    using EloBuddy;
    using System;
    using System.IO;
    using System.Text;

    public class GamePacket
    {
        private BinaryReader br;
        private BinaryWriter bw;
        private PacketChannel m_packetChannel;
        private PacketProtocolFlags m_packetFlags;
        private PacketHeader m_packetHeader;
        private MemoryStream ms;

        public GamePacket()
        {
            this.m_packetChannel = PacketChannel.C2S;
            this.m_packetFlags = PacketProtocolFlags.Reliable;
        }

        public GamePacket(EloBuddy.Networking.GamePacketEventArgs args)
        {
        }

        public GamePacket(byte[] data)
        {
            this.LoadData(data);
        }

        public GamePacket(PacketHeader header)
        {
            this.SetHeader(header);
        }

        public GamePacket(int hashAlgorithm, short header)
        {
            this..ctor();
            byte[] data = new byte[0];
            this.LoadData(data);
            this.Write<int>(hashAlgorithm);
            this.Write<short>(header);
            this.Write<int>(0);
        }

        public GamePacket(IntPtr hashAlgorithm, short header)
        {
            this..ctor();
            byte[] data = new byte[0];
            this.LoadData(data);
            this.Write<int>((int) hashAlgorithm);
            this.Write<short>(header);
            this.Write<int>(0);
        }

        public GamePacket(HashAlgorithm algorithm, short header, uint networkId)
        {
            this..ctor();
            byte[] data = new byte[0];
            this.LoadData(data);
            IntPtr address = algorithm.Address;
            this.Write<int>((int) address);
            this.Write<short>(header);
            this.Write<int>((int) networkId);
        }

        public GamePacket(int hashAlgorithm, short header, int networkId)
        {
            this..ctor();
            byte[] data = new byte[0];
            this.LoadData(data);
            this.Write<int>(hashAlgorithm);
            this.Write<short>(header);
            this.Write<int>(networkId);
        }

        public GamePacket(IntPtr hashAlgorithm, short header, int networkId)
        {
            this..ctor();
            byte[] data = new byte[0];
            this.LoadData(data);
            this.Write<int>((int) hashAlgorithm);
            this.Write<short>(header);
            this.Write<int>(networkId);
        }

        public void Dump()
        {
            Console.WriteLine(this.ToString());
        }

        internal void LoadData(byte[] data)
        {
            this..ctor();
            if (data.Length > 0)
            {
                this.ms = new MemoryStream(data);
            }
            else
            {
                this.ms = new MemoryStream();
            }
            this.br = new BinaryReader(this.ms);
            this.bw = new BinaryWriter(this.ms);
            this.br.BaseStream.Position = 0L;
            this.bw.BaseStream.Position = 0L;
        }

        public void Process()
        {
            Game.ProcessPacket(this.ms.ToArray(), PacketChannel.S2C);
        }

        public void Process(PacketChannel channel)
        {
            Game.ProcessPacket(this.ms.ToArray(), channel);
        }

        public T Read<T>() where T: struct
        {
            Type type = typeof(T);
            if ((type == typeof(bool)) || (type == typeof(byte)))
            {
                return (T) this.br.ReadBytes(1)[0];
            }
            if ((type == typeof(short)) || (type == typeof(short)))
            {
                return (T) BitConverter.ToInt16(this.br.ReadBytes(2), 0);
            }
            if (type == typeof(int))
            {
                return (T) BitConverter.ToInt32(this.br.ReadBytes(4), 0);
            }
            if (type == typeof(long))
            {
                return (T) BitConverter.ToInt64(this.br.ReadBytes(8), 0);
            }
            if (type == typeof(float))
            {
                return (T) BitConverter.ToSingle(this.br.ReadBytes(4), 0);
            }
            return default(T);
        }

        public T Read<T>(int position) where T: struct
        {
            this.Position = position;
            return this.Read<T>();
        }

        public void Send()
        {
            Game.SendPacket(this.ms.ToArray(), PacketChannel.C2S, PacketProtocolFlags.Reliable);
        }

        public void Send(PacketChannel channel, PacketProtocolFlags flags)
        {
            Game.SendPacket(this.ms.ToArray(), channel, flags);
        }

        public void SetHeader(PacketHeader header)
        {
            this.bw.BaseStream.Position = 0L;
            IntPtr address = header.Algorithm.Address;
            this.Write<int>((int) address);
            this.Write<short>(header.OpCode);
            this.Write<uint>(header.NetworkId);
        }

        internal void SetPacketHeader()
        {
            this.m_packetChannel = PacketChannel.C2S;
            this.m_packetFlags = PacketProtocolFlags.Reliable;
        }

        internal void SetPacketHeader(PacketChannel channel, PacketProtocolFlags flags)
        {
            this.m_packetChannel = channel;
            this.m_packetFlags = flags;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            byte[] data = this.Data;
            object[] args = new object[] { this.Header.OpCode.ToString("x2"), this.m_packetChannel, this.m_packetFlags, data.Length, ((this.Data.Length < 4) ? 0 : BitConverter.ToInt32(this.Data, 0)).ToString("x8") };
            builder.Append(string.Format("OpCode: {0} Channel: {1} - Flags: {2} - Length: {3} - HashAlgorithm: {4}", args));
            builder.Append(Environment.NewLine);
            int index = 0;
            if (0 < data.Length)
            {
                do
                {
                    byte num2 = data[index];
                    builder.Append($"{num2.ToString("x2")} ");
                    index++;
                }
                while (index < data.Length);
            }
            return builder.ToString();
        }

        public void Write<T>(T value) where T: struct
        {
            Type type = typeof(T);
            if (type == typeof(bool))
            {
                this.bw.Write((bool) value);
            }
            if (type == typeof(byte))
            {
                this.bw.Write((byte) value);
            }
            if (type == typeof(short))
            {
                this.bw.Write((short) value);
            }
            if (type == typeof(int))
            {
                this.bw.Write((int) value);
            }
            if (type == typeof(uint))
            {
                this.bw.Write((uint) value);
            }
            if (type == typeof(long))
            {
                this.bw.Write((long) value);
            }
            if (type == typeof(float))
            {
                this.bw.Write((float) value);
            }
            if (type == typeof(string))
            {
                this.bw.Write(Encoding.GetEncoding("UTF-8").GetBytes((string) Convert.ChangeType(value, typeof(string))));
            }
        }

        public void Write<T>(T value, int repeat) where T: struct
        {
            Type type = typeof(T);
            if ((type == typeof(bool)) && (0 < repeat))
            {
                int num7 = repeat;
                do
                {
                    this.bw.Write((bool) value);
                    num7--;
                }
                while (num7 > 0);
            }
            if ((type == typeof(byte)) && (0 < repeat))
            {
                int num6 = repeat;
                do
                {
                    this.bw.Write((byte) value);
                    num6--;
                }
                while (num6 > 0);
            }
            if ((type == typeof(short)) && (0 < repeat))
            {
                int num5 = repeat;
                do
                {
                    this.bw.Write((short) value);
                    num5--;
                }
                while (num5 > 0);
            }
            if ((type == typeof(int)) && (0 < repeat))
            {
                int num4 = repeat;
                do
                {
                    this.bw.Write((int) value);
                    num4--;
                }
                while (num4 > 0);
            }
            if ((type == typeof(uint)) && (0 < repeat))
            {
                int num3 = repeat;
                do
                {
                    this.bw.Write((uint) value);
                    num3--;
                }
                while (num3 > 0);
            }
            if ((type == typeof(long)) && (0 < repeat))
            {
                int num2 = repeat;
                do
                {
                    this.bw.Write((long) value);
                    num2--;
                }
                while (num2 > 0);
            }
            if ((type == typeof(float)) && (0 < repeat))
            {
                int num = repeat;
                do
                {
                    this.bw.Write((float) value);
                    num--;
                }
                while (num > 0);
            }
            if (type == typeof(string))
            {
                this.bw.Write(Encoding.GetEncoding("UTF-8").GetBytes((string) Convert.ChangeType(value, typeof(string))));
            }
        }

        public void Write<T>(T value, int repeat, int position) where T: struct
        {
            this.Position = position;
            this.Write<T>(value, repeat);
        }

        public byte[] Data
        {
            get
            {
                MemoryStream ms = this.ms;
                return ((ms == null) ? new byte[0] : ms.ToArray());
            }
        }

        public PacketHeader Header
        {
            get
            {
                if (this.m_packetHeader == null)
                {
                    this.m_packetHeader = new PacketHeader(this);
                }
                return this.m_packetHeader;
            }
        }

        public int Position
        {
            get => 
                ((int) this.br.BaseStream.Position);
            set
            {
                if (value > 0)
                {
                    this.br.BaseStream.Position = value;
                }
            }
        }
    }
}

