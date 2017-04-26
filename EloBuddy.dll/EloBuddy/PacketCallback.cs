namespace EloBuddy
{
    using System;

    public class PacketCallback
    {
        public Action<GamePacket> m_action;
        public PacketCallbackType m_callbackType;

        public PacketCallback(PacketCallbackType type, Action<GamePacket> action)
        {
            this.m_callbackType = type;
            this.m_action = action;
        }
    }
}

