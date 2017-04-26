namespace EloBuddy.SDK.Menu
{
    using System;
    using System.Collections.Generic;

    public interface ISerializeable
    {
        Dictionary<string, object> Serialize();

        string SerializationId { get; }

        bool ShouldSerialize { get; }
    }
}

