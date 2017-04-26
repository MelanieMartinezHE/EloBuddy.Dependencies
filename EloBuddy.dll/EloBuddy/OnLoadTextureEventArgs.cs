namespace EloBuddy
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class OnLoadTextureEventArgs : EventArgs
    {
        private bool m_process;
        private string m_textureName;

        public OnLoadTextureEventArgs(string textureName)
        {
            this.m_textureName = textureName;
            this.m_process = true;
        }

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

        public string Texture =>
            this.m_textureName;

        public delegate void OnLoadTextureEvent(EventArgs A_0);
    }
}

