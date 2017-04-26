namespace EloBuddy.Sandbox.Shared
{
    using System;
    using System.ServiceModel;

    public static class ServiceFactory
    {
        private const string PipeName = "EloBuddy";

        public static TInterfaceType CreateProxy<TInterfaceType>() where TInterfaceType: class
        {
            TInterfaceType local;
            try
            {
                local = new ChannelFactory<TInterfaceType>(new NetNamedPipeBinding(), new EndpointAddress("net.pipe://localhost/EloBuddy")).CreateChannel();
            }
            catch (Exception exception)
            {
                throw new Exception("Failed to connect to assembly pipe for communication. The targetted assembly may not be loaded yet. Desired interface: " + typeof(TInterfaceType).Name, exception);
            }
            return local;
        }
    }
}

