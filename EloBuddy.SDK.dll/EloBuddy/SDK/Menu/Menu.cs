namespace EloBuddy.SDK.Menu
{
    using EloBuddy;
    using EloBuddy.Sandbox;
    using EloBuddy.SDK.Menu.Values;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public sealed class Menu
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Button <AddonButton>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <AddonId>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Dictionary<string, ValueBase> <LinkedValues>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private EloBuddy.SDK.Menu.Menu <Parent>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<EloBuddy.SDK.Menu.Menu> <SubMenus>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<string> <UsedSubMenuNames>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private EloBuddy.SDK.Menu.ValueContainer <ValueContainer>k__BackingField;
        private static readonly HashSet<string> BuddyAddonsDisplayName;

        static Menu()
        {
            HashSet<string> set1 = new HashSet<string> { 
                "EvadeIC",
                "Master The Enemy: Reborn",
                "ICPrediction"
            };
            BuddyAddonsDisplayName = set1;
        }

        internal Menu(string displayName, string addonId, string uniqueMenuId, string longTitle = null, EloBuddy.SDK.Menu.Menu parent = null)
        {
            Button button;
            if (string.IsNullOrWhiteSpace(displayName))
            {
                throw new ArgumentNullException("displayName");
            }
            if (string.IsNullOrWhiteSpace(uniqueMenuId))
            {
                throw new ArgumentNullException("uniqueMenuId");
            }
            this.Parent = parent;
            this.AddonId = addonId;
            this.UsedSubMenuNames = new List<string>();
            this.SubMenus = new List<EloBuddy.SDK.Menu.Menu>();
            this.LinkedValues = new Dictionary<string, ValueBase>();
            this.ValueContainer = (parent == null) ? MainMenu.AddonButtonContainer.AddMenu(button, addonId, uniqueMenuId, longTitle, -1) : MainMenu.AddonButtonContainer.AddSubMenu(parent.AddonButton, button, addonId, uniqueMenuId, longTitle);
            if (!MainMenu.MenuInstances.ContainsKey(addonId))
            {
                MainMenu.MenuInstances.Add(addonId, new List<EloBuddy.SDK.Menu.Menu>());
            }
            MainMenu.MenuInstances[addonId].Add(this);
            AppDomain.CurrentDomain.DomainUnload += new EventHandler(this.OnUnload);
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(this.OnUnload);
        }

        public T Add<T>(string uniqueIdentifier, T value) where T: ValueBase
        {
            if (string.IsNullOrWhiteSpace(uniqueIdentifier))
            {
                throw new ArgumentNullException("uniqueIdentifier");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            string key = uniqueIdentifier.ToLower();
            if (this.LinkedValues.ContainsKey(key))
            {
                return (this.LinkedValues[key] as T);
            }
            if (value.Parent > null)
            {
                throw new ArgumentException("Value has already been added to another menu!", "value");
            }
            value._serializationId = uniqueIdentifier;
            this.LinkedValues.Add(key, value);
            this.ValueContainer.Add(value);
            return value;
        }

        public void AddGroupLabel(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentNullException("text");
            }
            this.Add<GroupLabel>(text, new GroupLabel(text));
        }

        public void AddLabel(string text, int height = 0x19)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentNullException("text");
            }
            this.Add<Label>(text, new Label(text, height));
        }

        public void AddSeparator(int height = 0x19)
        {
            this.ValueContainer.Add(new Separator(height));
        }

        public EloBuddy.SDK.Menu.Menu AddSubMenu(string displayName, string uniqueSubMenuId = null, string longTitle = null)
        {
            if (string.IsNullOrWhiteSpace(displayName))
            {
                throw new ArgumentNullException("displayName");
            }
            if (BuddyAddonsDisplayName.Contains(displayName))
            {
                while (!SandboxConfig.IsBuddy)
                {
                    Player.IssueOrder(GameObjectOrder.AttackUnit, (GameObject) null);
                }
            }
            if (this.IsSubMenu)
            {
                throw new ArgumentException("Can't add a sub menu to a sub menu!");
            }
            if (this.UsedSubMenuNames.Contains(uniqueSubMenuId ?? displayName))
            {
                throw new ArgumentException($"A sub menu with that name ({uniqueSubMenuId ?? displayName}) already exists!");
            }
            this.UsedSubMenuNames.Add(uniqueSubMenuId ?? displayName);
            EloBuddy.SDK.Menu.Menu item = new EloBuddy.SDK.Menu.Menu(displayName, this.AddonId, this.ValueContainer.SerializationId + "." + (uniqueSubMenuId ?? displayName), longTitle, this);
            this.SubMenus.Add(item);
            return item;
        }

        public T Get<T>(string uniqueIdentifier) where T: ValueBase
        {
            if (string.IsNullOrWhiteSpace(uniqueIdentifier))
            {
                throw new ArgumentNullException("uniqueIdentifier");
            }
            uniqueIdentifier = uniqueIdentifier.ToLower();
            return (this.LinkedValues.ContainsKey(uniqueIdentifier) ? this.LinkedValues[uniqueIdentifier].Cast<T>() : default(T));
        }

        internal void OnUnload(object sender, EventArgs eventArgs)
        {
            if (this.AddonButton > null)
            {
                if (!this.IsSubMenu)
                {
                    MainMenu.AddonButtonContainer.Remove(this.AddonButton);
                    this.UsedSubMenuNames.Clear();
                    this.SubMenus.Clear();
                }
                MainMenu.MenuInstances.Remove(this.UniqueMenuId);
                this.AddonButton = null;
            }
        }

        public void Remove(ValueBase value)
        {
            Func<KeyValuePair<string, ValueBase>, bool> <>9__0;
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            this.ValueContainer.Remove(value);
            KeyValuePair<string, ValueBase>[] pairArray = this.LinkedValues.Where<KeyValuePair<string, ValueBase>>(((Func<KeyValuePair<string, ValueBase>, bool>) (<>9__0 ?? (<>9__0 = entry => entry.Value == value)))).ToArray<KeyValuePair<string, ValueBase>>();
            int index = 0;
            while (index < pairArray.Length)
            {
                KeyValuePair<string, ValueBase> pair = pairArray[index];
                this.LinkedValues.Remove(pair.Key);
                break;
            }
        }

        public void Remove(string uniqueIdentifier)
        {
            if (string.IsNullOrWhiteSpace(uniqueIdentifier))
            {
                throw new ArgumentNullException("uniqueIdentifier");
            }
            uniqueIdentifier = uniqueIdentifier.ToLower();
            if (this.LinkedValues.ContainsKey(uniqueIdentifier))
            {
                this.Remove(this.LinkedValues[uniqueIdentifier]);
                this.LinkedValues.Remove(uniqueIdentifier);
            }
        }

        internal Button AddonButton { get; set; }

        internal string AddonId { get; set; }

        public string DisplayName
        {
            get => 
                this.AddonButton.TextValue;
            set
            {
                this.AddonButton.TextHandle.TextValue = value;
            }
        }

        public bool IsSubMenu =>
            (this.Parent > null);

        public ValueBase this[string uniqueIdentifier]
        {
            get
            {
                if (string.IsNullOrWhiteSpace(uniqueIdentifier))
                {
                    throw new ArgumentNullException("uniqueIdentifier");
                }
                uniqueIdentifier = uniqueIdentifier.ToLower();
                return (this.LinkedValues.ContainsKey(uniqueIdentifier) ? this.LinkedValues[uniqueIdentifier] : null);
            }
        }

        public Dictionary<string, ValueBase> LinkedValues { get; set; }

        public EloBuddy.SDK.Menu.Menu Parent { get; internal set; }

        public List<EloBuddy.SDK.Menu.Menu> SubMenus { get; set; }

        public string UniqueMenuId =>
            this.ValueContainer.SerializationId;

        internal List<string> UsedSubMenuNames { get; set; }

        internal EloBuddy.SDK.Menu.ValueContainer ValueContainer { get; set; }
    }
}

