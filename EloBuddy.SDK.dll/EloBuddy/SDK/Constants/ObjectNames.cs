namespace EloBuddy.SDK.Constants
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public static class ObjectNames
    {
        internal static readonly HashSet<string> BaseTurrets;
        internal static readonly HashSet<string> InvalidTargets;
        internal static readonly HashSet<string> LaneTurrets;
        internal static readonly string[] MinionList;

        static ObjectNames()
        {
            HashSet<string> set1 = new HashSet<string> { 
                "JarvanIVStandard",
                "ZyraSeed"
            };
            InvalidTargets = set1;
            HashSet<string> set2 = new HashSet<string> { 
                "SRUAP_Turret_Order1",
                "SRUAP_Turret_Order2",
                "SRUAP_Turret_Order3",
                "SRUAP_Turret_Chaos1",
                "SRUAP_Turret_Chaos2",
                "SRUAP_Turret_Chaos3"
            };
            LaneTurrets = set2;
            HashSet<string> set3 = new HashSet<string> { 
                "SRUAP_Turret_Order3",
                "SRUAP_Turret_Order4",
                "SRUAP_Turret_Chaos3",
                "SRUAP_Turret_Chaos4"
            };
            BaseTurrets = set3;
            string[] source = new string[] { "SRU", "HA" };
            string[] teams = new string[] { "Chaos", "Order" };
            string[] minions = new string[] { "Melee", "Ranged", "Siege", "Super" };
            string[] strArray2 = new string[] { "Blue", "Red" };
            string[] oldMinions = new string[] { "Basic", "MechCannon", "MechMelee", "Wizard" };
            List<string> list = source.SelectMany(((Func<string, IEnumerable<string>>) (map => teams)), new Func<string, string, <>f__AnonymousType0<string, string>>(<>c.<>9.<.cctor>b__6_1)).SelectMany(((Func<<>f__AnonymousType0<string, string>, IEnumerable<string>>) (<>h__TransparentIdentifier0 => minions)), new Func<<>f__AnonymousType0<string, string>, string, string>(<>c.<>9.<.cctor>b__6_3)).ToList<string>();
            string[] collection = new string[] { "OdinBlueSuperminion", "OdinRedSuperminion", "Odin_Blue_Minion_Caster", "Odin_Red_Minion_Caster" };
            list.AddRange(collection);
            list.AddRange(from team in strArray2
                from minion in oldMinions
                select $"{team}_Minion_{minions}");
            string[] strArray3 = new string[] { "Ocklepod", "Pludercrab", "Ironback", "Razorfin" };
            list.AddRange(strArray3.Select<string, string>(new Func<string, string>(<>c.<>9.<.cctor>b__6_6)));
            list.AddRange(teams.SelectMany<string, string, string>((Func<string, IEnumerable<string>>) (team => minions), new Func<string, string, string>(<>c.<>9.<.cctor>b__6_8)));
            list.AddRange(teams.Select<string, string>(new Func<string, string>(<>c.<>9.<.cctor>b__6_9)));
            MinionList = list.ToArray();
        }

        public static string[] Minions =>
            MinionList;

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly ObjectNames.<>c <>9 = new ObjectNames.<>c();

            internal <>f__AnonymousType0<string, string> <.cctor>b__6_1(string map, string team) => 
                new { 
                    map = map,
                    team = team
                };

            internal string <.cctor>b__6_3(<>f__AnonymousType0<string, string> <>h__TransparentIdentifier0, string minion) => 
                $"{<>h__TransparentIdentifier0.map}_{<>h__TransparentIdentifier0.team}Minion{minion}";

            internal string <.cctor>b__6_6(string name) => 
                $"BW_{name}";

            internal string <.cctor>b__6_8(string team, string minion) => 
                $"BilgeLane{minion}_{team}";

            internal string <.cctor>b__6_9(string team) => 
                $"BilgeLaneCannon_{team}";
        }
    }
}

