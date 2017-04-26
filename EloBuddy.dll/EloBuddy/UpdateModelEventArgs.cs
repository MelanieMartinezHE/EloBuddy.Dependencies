namespace EloBuddy
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class UpdateModelEventArgs : EventArgs
    {
        private string m_model;
        private bool m_process;
        private int m_skinId;

        public UpdateModelEventArgs(string model, int skinId)
        {
            this.m_model = model;
            this.m_skinId = skinId;
            this.m_process = true;
        }

        public string Model =>
            this.m_model;

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

        public int SkinId =>
            this.m_skinId;

        public delegate void UpdateModelEvent(Obj_AI_Base sender, UpdateModelEventArgs args);
    }
}

