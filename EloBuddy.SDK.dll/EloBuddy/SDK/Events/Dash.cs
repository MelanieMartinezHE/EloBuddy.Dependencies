namespace EloBuddy.SDK.Events
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Constants;
    using SharpDX;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public static class Dash
    {
        internal static readonly Dictionary<Obj_AI_Base, DashEventArgs> DashDictionary = new Dictionary<Obj_AI_Base, DashEventArgs>();

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public static  event OnDashDelegate OnDash;

        static Dash()
        {
            Game.OnTick += new GameTick(<>c.<>9.<.cctor>b__5_0);
            Obj_AI_Base.OnProcessSpellCast += new Obj_AI_ProcessSpellCast(Dash.Obj_AI_Base_OnProcessSpellCast);
            Obj_AI_Base.OnNewPath += new Obj_AI_BaseNewPath(Dash.OnNewPath);
        }

        public static DashEventArgs GetDashInfo(this Obj_AI_Base unit)
        {
            DashEventArgs args;
            DashDictionary.TryGetValue(unit, out args);
            return args;
        }

        public static bool IsDashing(this Obj_AI_Base unit)
        {
            Obj_AI_Base base2 = DashDictionary.Keys.FirstOrDefault<Obj_AI_Base>(o => o.NetworkId == unit.NetworkId);
            return ((base2 > null) && (DashDictionary[base2].EndTick > Core.GameTickCount));
        }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            AIHeroClient client = sender as AIHeroClient;
            if ((client != null) && args.IsDash(sender))
            {
                Dashes.DashSpell spell = args.DashData(sender).FirstOrDefault<Dashes.DashSpell>();
                if (spell != null)
                {
                    Vector3 end = args.End;
                    if (args.Target > null)
                    {
                        end = args.Target.Position;
                    }
                    if (args.Start.Distance(end, false) > spell.Range)
                    {
                        end = args.Start.Extend(end, ((float) spell.Range)).To3D(0);
                    }
                    if (spell.Inverted)
                    {
                        end = args.Start.Extend(end, ((float) -spell.Range)).To3DWorld();
                    }
                    DashEventArgs args2 = new DashEventArgs {
                        StartPos = sender.ServerPosition,
                        EndPos = end,
                        Speed = spell.Speed,
                        CastDelay = spell.CastDelay,
                        StartTick = Core.GameTickCount
                    };
                    args2.EndTick = (args2.StartTick + ((int) ((1000f * client.Path.Last<Vector3>().Distance(sender, false)) / args2.Speed))) + spell.CastDelay;
                    args2.Duration = args2.EndTick - args2.StartTick;
                    Obj_AI_Base key = DashDictionary.Keys.FirstOrDefault<Obj_AI_Base>(o => (o.NetworkId == sender.NetworkId)) ?? sender;
                    if (DashDictionary.ContainsKey(key))
                    {
                        DashDictionary[key].StartPos = args2.StartPos;
                        DashDictionary[key].EndPos = args2.EndPos;
                        DashDictionary[key].StartTick = args2.StartTick;
                        DashDictionary[key].EndTick = args2.EndTick;
                        DashDictionary[key].Duration = args2.Duration;
                    }
                    else
                    {
                        DashDictionary.Remove(key);
                        DashDictionary.Add(key, args2);
                    }
                    if (OnDash > null)
                    {
                        OnDash(sender, DashDictionary[key]);
                    }
                }
            }
        }

        private static void OnNewPath(Obj_AI_Base sender, GameObjectNewPathEventArgs args)
        {
            if (args.IsDash)
            {
                AIHeroClient target = sender as AIHeroClient;
                if ((target != null) && target.IsValid)
                {
                    Obj_AI_Base key = DashDictionary.Keys.FirstOrDefault<Obj_AI_Base>(o => (o.NetworkId == sender.NetworkId)) ?? sender;
                    Dashes.DashSpell spell = target.GetDashData().FirstOrDefault<Dashes.DashSpell>();
                    float num = (spell != null) ? ((float) spell.Speed) : args.Speed;
                    int num2 = (spell != null) ? spell.CastDelay : 0;
                    DashEventArgs args2 = new DashEventArgs {
                        StartPos = sender.ServerPosition,
                        EndPos = args.Path.Last<Vector3>(),
                        Speed = num,
                        CastDelay = num2,
                        StartTick = Core.GameTickCount - Game.Ping
                    };
                    args2.EndTick = (args2.StartTick + (((((1000f * args.Path.Last<Vector3>().Distance(sender, false)) / num) <= 0f) || (num >= 2.147484E+09f)) ? ((int) 2500f) : ((int) num))) + num2;
                    args2.Duration = args2.EndTick - args2.StartTick;
                    if (DashDictionary.ContainsKey(key))
                    {
                        DashDictionary[key].StartPos = args2.StartPos;
                        DashDictionary[key].EndPos = args2.EndPos;
                    }
                    else
                    {
                        DashDictionary.Remove(key);
                        DashDictionary.Add(key, args2);
                    }
                    if (OnDash > null)
                    {
                        OnDash(sender, DashDictionary[key]);
                    }
                }
            }
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly Dash.<>c <>9 = new Dash.<>c();
            public static Action<Obj_AI_Base> <>9__5_1;

            internal void <.cctor>b__5_0(EventArgs <args>)
            {
                Dash.DashDictionary.Keys.ToList<Obj_AI_Base>().ForEach(delegate (Obj_AI_Base o) {
                    if (!o.IsValidTarget(null, false, null) || (Core.GameTickCount > Dash.DashDictionary[o].EndTick))
                    {
                        Dash.DashDictionary.Remove(o);
                    }
                    else if (Dash.OnDash > null)
                    {
                        Dash.OnDash(o, Dash.DashDictionary[o]);
                    }
                });
            }

            internal void <.cctor>b__5_1(Obj_AI_Base o)
            {
                if (!o.IsValidTarget(null, false, null) || (Core.GameTickCount > Dash.DashDictionary[o].EndTick))
                {
                    Dash.DashDictionary.Remove(o);
                }
                else if (Dash.OnDash > null)
                {
                    Dash.OnDash(o, Dash.DashDictionary[o]);
                }
            }
        }

        public class DashEventArgs : EventArgs
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private int <CastDelay>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private int <Duration>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Vector3 <EndPos>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private int <EndTick>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private List<Vector2> <Path>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private float <Speed>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Vector3 <StartPos>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private int <StartTick>k__BackingField;

            public int CastDelay { get; set; }

            public int Duration { get; internal set; }

            public Vector3 EndPos { get; set; }

            public int EndTick { get; internal set; }

            public List<Vector2> Path { get; internal set; }

            public float Speed { get; set; }

            public Vector3 StartPos { get; set; }

            public int StartTick { get; internal set; }
        }

        public delegate void OnDashDelegate(Obj_AI_Base sender, Dash.DashEventArgs e);
    }
}

