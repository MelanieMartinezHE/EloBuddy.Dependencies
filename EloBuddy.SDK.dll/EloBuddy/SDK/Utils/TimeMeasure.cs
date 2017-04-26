namespace EloBuddy.SDK.Utils
{
    using EloBuddy;
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class TimeMeasure : IDisposable
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Name>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <OutputChat>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <OutputConsole>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Stopwatch <Timer>k__BackingField;

        public TimeMeasure(string name = "TimeMeasure", bool outputConsole = true, bool outputChat = false)
        {
            this.Name = name;
            this.OutputConsole = outputConsole;
            this.OutputChat = outputChat;
            this.Timer = new Stopwatch();
            this.Timer.Start();
        }

        public void Dispose()
        {
            this.Timer.Stop();
            if (this.OutputChat)
            {
                object[] @params = new object[] { this.Name, this.Timer.Elapsed };
                Chat.Print("{0}: {1}", @params);
            }
            if (this.OutputConsole)
            {
                object[] args = new object[] { this.Name, this.Timer.Elapsed };
                Logger.Info("{0}: Action took {1}", args);
            }
        }

        public string Name { get; set; }

        public bool OutputChat { get; set; }

        public bool OutputConsole { get; set; }

        public Stopwatch Timer { get; set; }
    }
}

