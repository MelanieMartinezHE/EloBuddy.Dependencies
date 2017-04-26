namespace EloBuddy.SDK.ThirdParty.Glide
{
    using System;

    public static class Ease
    {
        private const float B1 = 0.3636364f;
        private const float B2 = 0.7272727f;
        private const float B3 = 0.5454546f;
        private const float B4 = 0.9090909f;
        private const float B5 = 0.8181818f;
        private const float B6 = 0.9545454f;
        private const float PI = 3.14159f;
        private const float PI2 = 1.570795f;

        public static float BackIn(float t) => 
            ((float) ((t * t) * ((2.70158 * t) - 1.70158)));

        public static float BackInOut(float t)
        {
            t *= 2f;
            if (t < 1f)
            {
                return (float) (((t * t) * ((2.70158 * t) - 1.70158)) / 2.0);
            }
            t--;
            return (float) (((1.0 - ((--t * t) * ((-2.70158 * t) - 1.70158))) / 2.0) + 0.5);
        }

        public static float BackOut(float t) => 
            ((float) (1.0 - ((--t * t) * ((-2.70158 * t) - 1.70158))));

        public static float BounceIn(float t)
        {
            t = 1f - t;
            if (t < 0.3636364f)
            {
                return (float) (1.0 - ((7.5625 * t) * t));
            }
            if (t < 0.7272727f)
            {
                return (float) (1.0 - (((7.5625 * (t - 0.5454546f)) * (t - 0.5454546f)) + 0.75));
            }
            if (t < 0.9090909f)
            {
                return (float) (1.0 - (((7.5625 * (t - 0.8181818f)) * (t - 0.8181818f)) + 0.9375));
            }
            return (float) (1.0 - (((7.5625 * (t - 0.9545454f)) * (t - 0.9545454f)) + 0.984375));
        }

        public static float BounceInOut(float t)
        {
            if (t < 0.5)
            {
                t = 1f - (t * 2f);
                if (t < 0.3636364f)
                {
                    return (float) ((1.0 - ((7.5625 * t) * t)) / 2.0);
                }
                if (t < 0.7272727f)
                {
                    return (float) ((1.0 - (((7.5625 * (t - 0.5454546f)) * (t - 0.5454546f)) + 0.75)) / 2.0);
                }
                if (t < 0.9090909f)
                {
                    return (float) ((1.0 - (((7.5625 * (t - 0.8181818f)) * (t - 0.8181818f)) + 0.9375)) / 2.0);
                }
                return (float) ((1.0 - (((7.5625 * (t - 0.9545454f)) * (t - 0.9545454f)) + 0.984375)) / 2.0);
            }
            t = (t * 2f) - 1f;
            if (t < 0.3636364f)
            {
                return (float) ((((7.5625 * t) * t) / 2.0) + 0.5);
            }
            if (t < 0.7272727f)
            {
                return (float) (((((7.5625 * (t - 0.5454546f)) * (t - 0.5454546f)) + 0.75) / 2.0) + 0.5);
            }
            if (t < 0.9090909f)
            {
                return (float) (((((7.5625 * (t - 0.8181818f)) * (t - 0.8181818f)) + 0.9375) / 2.0) + 0.5);
            }
            return (float) (((((7.5625 * (t - 0.9545454f)) * (t - 0.9545454f)) + 0.984375) / 2.0) + 0.5);
        }

        public static float BounceOut(float t)
        {
            if (t < 0.3636364f)
            {
                return (float) ((7.5625 * t) * t);
            }
            if (t < 0.7272727f)
            {
                return (float) (((7.5625 * (t - 0.5454546f)) * (t - 0.5454546f)) + 0.75);
            }
            if (t < 0.9090909f)
            {
                return (float) (((7.5625 * (t - 0.8181818f)) * (t - 0.8181818f)) + 0.9375);
            }
            return (float) (((7.5625 * (t - 0.9545454f)) * (t - 0.9545454f)) + 0.984375);
        }

        public static float CircIn(float t) => 
            ((float) -(Math.Sqrt((double) (1f - (t * t))) - 1.0));

        public static float CircInOut(float t) => 
            ((t <= 0.5) ? ((float) ((Math.Sqrt((double) (1f - ((t * t) * 4f))) - 1.0) / -2.0)) : ((float) ((Math.Sqrt((double) (1f - (((t * 2f) - 2f) * ((t * 2f) - 2f)))) + 1.0) / 2.0)));

        public static float CircOut(float t) => 
            ((float) Math.Sqrt((double) (1f - ((t - 1f) * (t - 1f)))));

        public static float CubeIn(float t) => 
            ((t * t) * t);

        public static float CubeInOut(float t) => 
            ((t <= 0.5) ? (((t * t) * t) * 4f) : (1f + (((--t * t) * t) * 4f)));

        public static float CubeOut(float t) => 
            (1f + ((--t * t) * t));

        public static float ElasticIn(float t) => 
            ((float) (Math.Sin((double) (20.42034f * t)) * Math.Pow(2.0, (double) (10f * (t - 1f)))));

        public static float ElasticInOut(float t)
        {
            if (t < 0.5)
            {
                return (float) ((0.5 * Math.Sin((double) (20.42034f * (2f * t)))) * Math.Pow(2.0, (double) (10f * ((2f * t) - 1f))));
            }
            return (float) (0.5 * ((Math.Sin((double) (-20.42034f * (((2f * t) - 1f) + 1f))) * Math.Pow(2.0, (double) (-10f * ((2f * t) - 1f)))) + 2.0));
        }

        public static float ElasticOut(float t)
        {
            if (t == 1f)
            {
                return 1f;
            }
            return (float) ((Math.Sin((double) (-20.42034f * (t + 1f))) * Math.Pow(2.0, (double) (-10f * t))) + 1.0);
        }

        public static float ExpoIn(float t) => 
            ((float) Math.Pow(2.0, (double) (10f * (t - 1f))));

        public static float ExpoInOut(float t)
        {
            if (t == 1f)
            {
                return 1f;
            }
            return ((t < 0.5) ? ((float) (Math.Pow(2.0, (double) (10f * ((t * 2f) - 1f))) / 2.0)) : ((float) ((-Math.Pow(2.0, (double) (-10f * ((t * 2f) - 1f))) + 2.0) / 2.0)));
        }

        public static float ExpoOut(float t)
        {
            if (t == 1f)
            {
                return 1f;
            }
            return (float) (-Math.Pow(2.0, (double) (-10f * t)) + 1.0);
        }

        public static float QuadIn(float t) => 
            (t * t);

        public static float QuadInOut(float t) => 
            ((t <= 0.5) ? ((t * t) * 2f) : (1f - ((--t * t) * 2f)));

        public static float QuadOut(float t) => 
            (-t * (t - 2f));

        public static float QuartIn(float t) => 
            (((t * t) * t) * t);

        public static float QuartInOut(float t) => 
            ((t <= 0.5) ? ((((t * t) * t) * t) * 8f) : (((1f - ((((t = (t * 2f) - 2f) * t) * t) * t)) / 2f) + ((float) 0.5)));

        public static float QuartOut(float t) => 
            (1f - (((--t * t) * t) * t));

        public static float QuintIn(float t) => 
            ((((t * t) * t) * t) * t);

        public static float QuintInOut(float t) => 
            (((t *= 2f) < 1f) ? (((((t * t) * t) * t) * t) / 2f) : (((((((t -= 2f) * t) * t) * t) * t) + 2f) / 2f));

        public static float QuintOut(float t) => 
            (((((--t * t) * t) * t) * t) + 1f);

        public static float SineIn(float t)
        {
            if (t == 1f)
            {
                return 1f;
            }
            return (float) (-Math.Cos((double) (1.570795f * t)) + 1.0);
        }

        public static float SineInOut(float t) => 
            ((float) ((-Math.Cos((double) (3.14159f * t)) / 2.0) + 0.5));

        public static float SineOut(float t) => 
            ((float) Math.Sin((double) (1.570795f * t)));

        public static Func<float, float> ToAndFro(Func<float, float> easer) => 
            t => ToAndFro(easer(t));

        public static float ToAndFro(float t) => 
            ((t < 0.5f) ? (t * 2f) : (1f + (((t - 0.5f) / 0.5f) * -1f)));
    }
}

