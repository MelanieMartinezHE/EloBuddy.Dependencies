namespace EloBuddy
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class AudioManagerPlaySoundEventArgs : EventArgs
    {
        private bool m_process;
        private string m_soundFile;

        public AudioManagerPlaySoundEventArgs(string soundFile)
        {
            this.m_soundFile = soundFile;
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

        public string SoundFile =>
            this.m_soundFile;

        public delegate void AudioManagerPlaySoundEvent(AudioManagerPlaySoundEventArgs args);
    }
}

