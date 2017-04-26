namespace EloBuddy.SDK.ThirdParty.Glide
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class Tween
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <Paused>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private object <Target>k__BackingField;
        private Action begin;
        private Lerper.Behavior behavior;
        private Action complete;
        private float Delay;
        private float Duration;
        private Func<float, float> ease;
        private List<object> end;
        private bool firstUpdate;
        private List<Lerper> lerpers;
        private TweenerImpl Parent;
        private IRemoveTweens Remover;
        private int repeatCount;
        private float repeatDelay;
        private List<object> start;
        private float time;
        private int timesRepeated;
        private Action update;
        private Dictionary<string, int> varHash;
        private List<GlideInfo> vars;

        private Tween(object target, float duration, float delay, TweenerImpl parent)
        {
            this.Target = target;
            this.Duration = duration;
            this.Delay = delay;
            this.Parent = parent;
            this.Remover = parent;
            this.firstUpdate = true;
            this.varHash = new Dictionary<string, int>();
            this.vars = new List<GlideInfo>();
            this.lerpers = new List<Lerper>();
            this.start = new List<object>();
            this.end = new List<object>();
            this.behavior = Lerper.Behavior.None;
        }

        private void AddLerp(Lerper lerper, GlideInfo info, object from, object to)
        {
            this.varHash.Add(info.PropertyName, this.vars.Count);
            this.vars.Add(info);
            this.start.Add(from);
            this.end.Add(to);
            this.lerpers.Add(lerper);
        }

        public void Cancel()
        {
            this.Remover.Remove(this);
        }

        public void Cancel(params string[] properties)
        {
            int num = 0;
            for (int i = 0; i < properties.Length; i++)
            {
                int num3 = 0;
                if (this.varHash.TryGetValue(properties[i], out num3))
                {
                    this.varHash.Remove(properties[i]);
                    this.vars[num3] = null;
                    this.lerpers[num3] = null;
                    this.start[num3] = null;
                    this.end[num3] = null;
                    num++;
                }
            }
            if (num == this.vars.Count)
            {
                this.Cancel();
            }
        }

        public void CancelAndComplete()
        {
            this.time = this.Duration;
            this.update = null;
            this.Remover.Remove(this);
        }

        public Tween Ease(Func<float, float> ease)
        {
            this.ease = ease;
            return this;
        }

        public Tween From(object values)
        {
            foreach (PropertyInfo info in values.GetType().GetProperties())
            {
                object obj2 = info.GetValue(values, null);
                int num2 = -1;
                if (this.varHash.TryGetValue(info.Name, out num2))
                {
                    this.start[num2] = obj2;
                }
                GlideInfo info2 = new GlideInfo(this.Target, info.Name, true) {
                    Value = obj2
                };
            }
            return this;
        }

        public Tween OnBegin(Action callback)
        {
            if (this.begin == null)
            {
                this.begin = callback;
            }
            else
            {
                this.begin = (Action) Delegate.Combine(this.begin, callback);
            }
            return this;
        }

        public Tween OnComplete(Action callback)
        {
            if (this.complete == null)
            {
                this.complete = callback;
            }
            else
            {
                this.complete = (Action) Delegate.Combine(this.complete, callback);
            }
            return this;
        }

        public Tween OnUpdate(Action callback)
        {
            if (this.update == null)
            {
                this.update = callback;
            }
            else
            {
                this.update = (Action) Delegate.Combine(this.update, callback);
            }
            return this;
        }

        public void Pause()
        {
            this.Paused = true;
        }

        public void PauseToggle()
        {
            this.Paused = !this.Paused;
        }

        public Tween Reflect()
        {
            this.behavior |= Lerper.Behavior.Reflect;
            return this;
        }

        public Tween Repeat(int times = -1)
        {
            this.repeatCount = times;
            return this;
        }

        public Tween RepeatDelay(float delay)
        {
            this.repeatDelay = delay;
            return this;
        }

        public void Resume()
        {
            this.Paused = false;
        }

        public Tween Reverse()
        {
            int count = this.vars.Count;
            while (count-- > 0)
            {
                object toValue = this.start[count];
                object fromValue = this.end[count];
                this.start[count] = fromValue;
                this.end[count] = toValue;
                this.lerpers[count].Initialize(fromValue, toValue, this.behavior);
            }
            return this;
        }

        public Tween Rotation(RotationUnit unit = 0)
        {
            this.behavior |= Lerper.Behavior.Rotation;
            this.behavior |= (unit == RotationUnit.Degrees) ? Lerper.Behavior.RotationDegrees : Lerper.Behavior.RotationRadians;
            return this;
        }

        public Tween Round()
        {
            this.behavior |= Lerper.Behavior.Round;
            return this;
        }

        private void Update(float elapsed)
        {
            if (this.firstUpdate)
            {
                this.firstUpdate = false;
                int count = this.vars.Count;
                while (count-- > 0)
                {
                    if (this.lerpers[count] > null)
                    {
                        this.lerpers[count].Initialize(this.start[count], this.end[count], this.behavior);
                    }
                }
            }
            else if (!this.Paused)
            {
                if (this.Delay > 0f)
                {
                    this.Delay -= elapsed;
                    if (this.Delay > 0f)
                    {
                        return;
                    }
                }
                if (((this.time == 0f) && (this.timesRepeated == 0)) && (this.begin > null))
                {
                    this.begin();
                }
                this.time += elapsed;
                float time = this.time;
                float arg = this.time / this.Duration;
                bool flag4 = false;
                if (this.time >= this.Duration)
                {
                    if (this.repeatCount > 0)
                    {
                        time = 0f;
                        this.Delay = this.repeatDelay;
                        this.timesRepeated++;
                        if (this.repeatCount > 0)
                        {
                            this.repeatCount--;
                        }
                        if (this.repeatCount < 0)
                        {
                            flag4 = true;
                        }
                    }
                    else
                    {
                        this.time = this.Duration;
                        arg = 1f;
                        this.Remover.Remove(this);
                        flag4 = true;
                    }
                }
                if (this.ease > null)
                {
                    arg = this.ease(arg);
                }
                int num4 = this.vars.Count;
                while (num4-- > 0)
                {
                    if (this.vars[num4] > null)
                    {
                        this.vars[num4].Value = this.lerpers[num4].Interpolate(arg, this.vars[num4].Value, this.behavior);
                    }
                }
                this.time = time;
                if ((this.time == 0f) && this.behavior.HasFlag(Lerper.Behavior.Reflect))
                {
                    this.Reverse();
                }
                if (this.update > null)
                {
                    this.update();
                }
                if (flag4 && (this.complete > null))
                {
                    this.complete();
                }
            }
        }

        public float Completion
        {
            get
            {
                float num = this.time / this.Duration;
                return ((num < 0f) ? 0f : ((num > 1f) ? 1f : num));
            }
        }

        public bool Looping =>
            (this.repeatCount > 0);

        public bool Paused { get; private set; }

        public object Target { get; private set; }

        public float TimeRemaining =>
            (this.Duration - this.time);

        private interface IRemoveTweens
        {
            void Remove(Tween t);
        }

        [Flags]
        public enum RotationUnit
        {
            Degrees,
            Radians
        }

        public class TweenerImpl : EloBuddy.SDK.ThirdParty.Glide.Tween.IRemoveTweens
        {
            private List<EloBuddy.SDK.ThirdParty.Glide.Tween> allTweens = new List<EloBuddy.SDK.ThirdParty.Glide.Tween>();
            private static Dictionary<Type, ConstructorInfo> registeredLerpers = new Dictionary<Type, ConstructorInfo>();
            private List<EloBuddy.SDK.ThirdParty.Glide.Tween> toAdd = new List<EloBuddy.SDK.ThirdParty.Glide.Tween>();
            private List<EloBuddy.SDK.ThirdParty.Glide.Tween> toRemove = new List<EloBuddy.SDK.ThirdParty.Glide.Tween>();
            private Dictionary<object, List<EloBuddy.SDK.ThirdParty.Glide.Tween>> tweens = new Dictionary<object, List<EloBuddy.SDK.ThirdParty.Glide.Tween>>();

            static TweenerImpl()
            {
                Type[] typeArray = new Type[] { typeof(short), typeof(int), typeof(long), typeof(ushort), typeof(uint), typeof(ulong), typeof(float), typeof(double) };
                for (int i = 0; i < typeArray.Length; i++)
                {
                    SetLerper<NumericLerper>(typeArray[i]);
                }
            }

            protected TweenerImpl()
            {
            }

            private void AddAndRemove()
            {
                for (int i = 0; i < this.toAdd.Count; i++)
                {
                    EloBuddy.SDK.ThirdParty.Glide.Tween item = this.toAdd[i];
                    this.allTweens.Add(item);
                    if (item.Target != null)
                    {
                        List<EloBuddy.SDK.ThirdParty.Glide.Tween> list = null;
                        if (!this.tweens.TryGetValue(item.Target, out list))
                        {
                            this.tweens[item.Target] = list = new List<EloBuddy.SDK.ThirdParty.Glide.Tween>();
                        }
                        list.Add(item);
                    }
                }
                for (int j = 0; j < this.toRemove.Count; j++)
                {
                    EloBuddy.SDK.ThirdParty.Glide.Tween tween2 = this.toRemove[j];
                    this.allTweens.Remove(tween2);
                    if (tween2.Target != null)
                    {
                        List<EloBuddy.SDK.ThirdParty.Glide.Tween> list2 = null;
                        if (this.tweens.TryGetValue(tween2.Target, out list2))
                        {
                            list2.Remove(tween2);
                            if (list2.Count == 0)
                            {
                                this.tweens.Remove(tween2.Target);
                            }
                        }
                        this.allTweens.Remove(tween2);
                    }
                }
                this.toAdd.Clear();
                this.toRemove.Clear();
            }

            public void Cancel()
            {
                this.toRemove.AddRange(this.allTweens);
            }

            public void CancelAndComplete()
            {
                for (int i = 0; i < this.allTweens.Count; i++)
                {
                    this.allTweens[i].CancelAndComplete();
                }
            }

            private Lerper CreateLerper(Type propertyType)
            {
                ConstructorInfo info = null;
                if (!registeredLerpers.TryGetValue(propertyType, out info))
                {
                    throw new Exception($"No Lerper found for type {propertyType.FullName}.");
                }
                return (Lerper) info.Invoke(null);
            }

            void EloBuddy.SDK.ThirdParty.Glide.Tween.IRemoveTweens.Remove(EloBuddy.SDK.ThirdParty.Glide.Tween tween)
            {
                this.toRemove.Add(tween);
            }

            public void Pause()
            {
                for (int i = 0; i < this.allTweens.Count; i++)
                {
                    this.allTweens[i].Pause();
                }
            }

            public void PauseToggle()
            {
                for (int i = 0; i < this.allTweens.Count; i++)
                {
                    this.allTweens[i].PauseToggle();
                }
            }

            public void Resume()
            {
                for (int i = 0; i < this.allTweens.Count; i++)
                {
                    this.allTweens[i].Resume();
                }
            }

            public static void SetLerper<TLerper>(Type propertyType) where TLerper: Lerper, new()
            {
                registeredLerpers[propertyType] = typeof(TLerper).GetConstructor(Type.EmptyTypes);
            }

            public void TargetCancel(object target)
            {
                List<EloBuddy.SDK.ThirdParty.Glide.Tween> list;
                if (this.tweens.TryGetValue(target, out list))
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].Cancel();
                    }
                }
            }

            public void TargetCancel(object target, params string[] properties)
            {
                List<EloBuddy.SDK.ThirdParty.Glide.Tween> list;
                if (this.tweens.TryGetValue(target, out list))
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].Cancel(properties);
                    }
                }
            }

            public void TargetCancelAndComplete(object target)
            {
                List<EloBuddy.SDK.ThirdParty.Glide.Tween> list;
                if (this.tweens.TryGetValue(target, out list))
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].CancelAndComplete();
                    }
                }
            }

            public void TargetPause(object target)
            {
                List<EloBuddy.SDK.ThirdParty.Glide.Tween> list;
                if (this.tweens.TryGetValue(target, out list))
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].Pause();
                    }
                }
            }

            public void TargetPauseToggle(object target)
            {
                List<EloBuddy.SDK.ThirdParty.Glide.Tween> list;
                if (this.tweens.TryGetValue(target, out list))
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].PauseToggle();
                    }
                }
            }

            public void TargetResume(object target)
            {
                List<EloBuddy.SDK.ThirdParty.Glide.Tween> list;
                if (this.tweens.TryGetValue(target, out list))
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].Resume();
                    }
                }
            }

            public EloBuddy.SDK.ThirdParty.Glide.Tween Timer(float duration, float delay = 0f)
            {
                EloBuddy.SDK.ThirdParty.Glide.Tween item = new EloBuddy.SDK.ThirdParty.Glide.Tween(null, duration, delay, this);
                this.AddAndRemove();
                this.toAdd.Add(item);
                return item;
            }

            public EloBuddy.SDK.ThirdParty.Glide.Tween Tween<T>(T target, object values, float duration, float delay = 0f, bool overwrite = true) where T: class
            {
                if (target == null)
                {
                    throw new ArgumentNullException("target");
                }
                if (target.GetType().IsValueType)
                {
                    throw new Exception("Target of tween cannot be a struct!");
                }
                EloBuddy.SDK.ThirdParty.Glide.Tween item = new EloBuddy.SDK.ThirdParty.Glide.Tween(target, duration, delay, this);
                this.AddAndRemove();
                this.toAdd.Add(item);
                if (values != null)
                {
                    PropertyInfo[] properties = values.GetType().GetProperties();
                    for (int i = 0; i < properties.Length; i++)
                    {
                        List<EloBuddy.SDK.ThirdParty.Glide.Tween> list = null;
                        if (overwrite && this.tweens.TryGetValue(target, out list))
                        {
                            for (int j = 0; j < list.Count; j++)
                            {
                                string[] textArray1 = new string[] { properties[i].Name };
                                list[j].Cancel(textArray1);
                            }
                        }
                        PropertyInfo info = properties[i];
                        GlideInfo info2 = new GlideInfo(target, info.Name, true);
                        GlideInfo info3 = new GlideInfo(values, info.Name, false);
                        Lerper lerper = this.CreateLerper(info2.PropertyType);
                        item.AddLerp(lerper, info2, info2.Value, info3.Value);
                    }
                }
                return item;
            }

            public void Update(float secondsElapsed)
            {
                for (int i = 0; i < this.allTweens.Count; i++)
                {
                    this.allTweens[i].Update(secondsElapsed);
                }
                this.AddAndRemove();
            }

            private class NumericLerper : Lerper
            {
                private float from;
                private float range;
                private float to;

                public override void Initialize(object fromValue, object toValue, Lerper.Behavior behavior)
                {
                    this.from = Convert.ToSingle(fromValue);
                    this.to = Convert.ToSingle(toValue);
                    this.range = this.to - this.from;
                    if (behavior.HasFlag(Lerper.Behavior.Rotation))
                    {
                        float from = this.from;
                        if (behavior.HasFlag(Lerper.Behavior.RotationRadians))
                        {
                            from *= 57.29578f;
                        }
                        if (from < 0f)
                        {
                            from = 360f + from;
                        }
                        float num2 = from + this.range;
                        float num3 = num2 - from;
                        float num4 = Math.Abs(num3);
                        if (num4 >= 180f)
                        {
                            this.range = (360f - num4) * ((num3 > 0f) ? ((float) (-1)) : ((float) 1));
                        }
                        else
                        {
                            this.range = num3;
                        }
                    }
                }

                public override object Interpolate(float t, object current, Lerper.Behavior behavior)
                {
                    float num = this.from + (this.range * t);
                    if (behavior.HasFlag(Lerper.Behavior.Rotation))
                    {
                        if (behavior.HasFlag(Lerper.Behavior.RotationRadians))
                        {
                            num *= 57.29578f;
                        }
                        num = num % 360f;
                        if (num < 0f)
                        {
                            num += 360f;
                        }
                        if (behavior.HasFlag(Lerper.Behavior.RotationRadians))
                        {
                            num *= 0.01745329f;
                        }
                    }
                    if (behavior.HasFlag(Lerper.Behavior.Round))
                    {
                        num = (float) Math.Round((double) num);
                    }
                    Type conversionType = current.GetType();
                    return Convert.ChangeType(num, conversionType);
                }
            }
        }
    }
}

