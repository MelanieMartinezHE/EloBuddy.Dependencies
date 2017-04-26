namespace EloBuddy.Sandbox.Shared
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;

    [DataContract]
    public class FileData
    {
        [DataMember]
        public string Hash { get; set; }

        [DataMember]
        public string Path { get; set; }

        [DataMember]
        public bool RequiresBuddy { get; set; }
    }
}

