namespace EloBuddy.SDK.ThirdParty.Glide
{
    using System;

    public abstract class Lerper
    {
        protected const float DEG = 57.29578f;
        protected const float RAD = 0.01745329f;

        protected Lerper()
        {
        }

        public abstract void Initialize(object fromValue, object toValue, Behavior behavior);
        public abstract object Interpolate(float t, object currentValue, Behavior behavior);

        [Flags]
        public enum Behavior
        {
            None = 0,
            Reflect = 1,
            Rotation = 2,
            RotationDegrees = 8,
            RotationRadians = 4,
            Round = 0x10
        }
    }
}

