namespace EloBuddy.Sandbox.Shared
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;

    [DataContract]
    public class SharedAddon
    {
        [DataMember]
        public string PathToBinary { get; set; }
    }
}

