namespace EloBuddy.Sandbox.Shared
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;

    [ServiceContract]
    public interface ILoaderService
    {
        [OperationContract]
        List<SharedAddon> GetAssemblyList(int gameid);
        [OperationContract]
        Configuration GetConfiguration(int pid);
        [OperationContract]
        void Recompile(int pid);
    }
}

