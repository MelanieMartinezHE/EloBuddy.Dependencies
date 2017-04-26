namespace EloBuddy.SDK.Spells
{
    using EloBuddy;
    using EloBuddy.SDK;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public static class SpellDatabase
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static Dictionary<string, List<SpellInfo>> <Database>k__BackingField;

        public static List<SpellInfo> GetSpellInfoList(Obj_AI_Base sender) => 
            GetSpellInfoList(sender.BaseSkinName);

        public static List<SpellInfo> GetSpellInfoList(string baseSkinName)
        {
            if (Database.ContainsKey(baseSkinName))
            {
                return Database[baseSkinName];
            }
            return new List<SpellInfo>();
        }

        internal static void Initialize()
        {
            Database = JsonConvert.DeserializeObject<Dictionary<string, List<SpellInfo>>>(DefaultSettings.SpellDatabase);
        }

        internal static Dictionary<string, List<SpellInfo>> Database
        {
            [CompilerGenerated]
            get => 
                <Database>k__BackingField;
            [CompilerGenerated]
            set
            {
                <Database>k__BackingField = value;
            }
        }
    }
}

