namespace EloBuddy.SDK.Menu.Values
{
    using System;

    public class ValueDeserializationException : ArgumentException
    {
        public ValueDeserializationException(string key) : base($"Serialized data does not contain key '{key}'")
        {
        }
    }
}

