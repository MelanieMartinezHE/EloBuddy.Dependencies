namespace EloBuddy.Sandbox.Shared
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;

    [DataContract]
    public class LoaderFileData
    {
        [DataMember]
        public FileData[] AddonFiles { get; set; }

        [DataMember]
        public string LoaderPath { get; set; }

        [DataMember]
        public FileData[] SystemFiles { get; set; }
    }
}

