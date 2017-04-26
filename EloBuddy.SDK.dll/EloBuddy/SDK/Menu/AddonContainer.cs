namespace EloBuddy.SDK.Menu
{
    using EloBuddy.SDK.Rendering;
    using SharpDX;
    using SharpDX.Direct3D9;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public sealed class AddonContainer : ControlContainer<Button>
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Button <ActiveButton>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Dictionary<Button, Tuple<Control, ControlContainer<ValueBase>>> <LinkedContainers>k__BackingField;

        internal AddonContainer() : base(ThemeManager.SpriteType.FormAddonButtonContainer, true, false, true, false)
        {
            this.LinkedContainers = new Dictionary<Button, Tuple<Control, ControlContainer<ValueBase>>>();
            this.OnThemeChange();
        }

        internal void Add(Button button)
        {
        }

        internal ValueContainer AddMenu(Button button, string addonId, string uniqueMenuId, string longTitle = null, int index = -1)
        {
            if (button == null)
            {
                throw new ArgumentNullException("button");
            }
            if (uniqueMenuId == null)
            {
                throw new ArgumentNullException("uniqueMenuId");
            }
            if (!base.Children.Contains(button))
            {
                button.SetParent(this);
                base.Children.Insert((index >= 0) ? index : base.Children.Count, button);
                this.RecalculateAlignOffsets();
            }
            button.TextHandle.Size = button.Size - new Vector2(button.TextHandle.Padding.X * 2f, 0f);
            button.OnActiveStateChanged += new DynamicControl.DynamicControlHandler(this.OnActiveStateChanged);
            ValueContainer container = new ValueContainer(addonId, uniqueMenuId) {
                IsVisible = false
            };
            MainMenu.Instance.Add(container);
            EmptyControl control = new EmptyControl(ThemeManager.SpriteType.FormContentHeader) {
                IsVisible = false
            };
            FontDescription fontDescription = new FontDescription {
                FaceName = "Gill Sans MT Pro Medium",
                Height = 20,
                Quality = FontQuality.Antialiased,
                Weight = FontWeight.ExtraBold
            };
            Text item = new Text(longTitle ?? button.TextValue, fontDescription) {
                TextAlign = Text.Align.Left,
                TextOrientation = Text.Orientation.Center,
                Color = System.Drawing.Color.FromArgb(0xff, 0x8f, 0x7a, 0x48)
            };
            control.TextObjects.Add(item);
            MainMenu.Instance.Add(control);
            this.LinkedContainers.Add(button, new Tuple<Control, ControlContainer<ValueBase>>(control, container));
            return container;
        }

        internal ValueContainer AddSubMenu(Button parent, Button subButton, string addonId, string uniqueMenuId, string longTitle = null)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }
            if (subButton == null)
            {
                throw new ArgumentNullException("subButton");
            }
            if (!this.LinkedContainers.ContainsKey(parent))
            {
                throw new ArgumentException("parent was not added to the menu yet!", "parent");
            }
            if (parent.CurrentButtonType != Button.ButtonType.Addon)
            {
                throw new ArgumentException($"parent is of type {parent.CurrentButtonType}, which is invalid", "parent");
            }
            if (subButton.CurrentButtonType != Button.ButtonType.AddonSub)
            {
                throw new ArgumentException($"subButton is of type {subButton.CurrentButtonType}, which is invalid", "subButton");
            }
            int index = base.Children.FindIndex(o => o == parent) + 1;
            while ((base.Children.Count > index) && (base.Children[index].CurrentButtonType == Button.ButtonType.AddonSub))
            {
                index++;
            }
            return this.AddMenu(subButton, addonId, uniqueMenuId, $"{this.LinkedContainers[parent].Item1.TextObjects[0].TextValue} :: {longTitle ?? subButton.TextValue}", index);
        }

        internal List<Button> GetSubMenus(Button mainButton)
        {
            if (mainButton == null)
            {
                throw new ArgumentNullException("mainButton");
            }
            if (mainButton.CurrentButtonType != Button.ButtonType.Addon)
            {
                throw new ArgumentException($"Button is not of type {Button.ButtonType.Addon}!", "mainButton");
            }
            if (!base.Children.Contains(mainButton))
            {
                throw new ArgumentException("Button is not yet added to the main menu", "mainButton");
            }
            List<Button> list = new List<Button>();
            for (int i = base.Children.FindIndex(o => o == mainButton) + 1; (base.Children.Count > (i + 1)) && (base.Children[i].CurrentButtonType == Button.ButtonType.AddonSub); i++)
            {
                list.Add(base.Children[i]);
            }
            return list;
        }

        internal void OnActiveStateChanged(Control sender, EventArgs args)
        {
            Button button = (Button) sender;
            if (button.IsActive)
            {
                Button activeButton = this.ActiveButton;
                this.ActiveButton = button;
                if (activeButton > null)
                {
                    activeButton.IsActive = false;
                }
                this.SetContentView(this.ActiveButton, true);
                if (this.ActiveButton.CurrentButtonType != Button.ButtonType.AddonSub)
                {
                    this.SetSubMenuState(this.ActiveButton, true);
                }
            }
            else
            {
                this.SetContentView(button, false);
                if (this.ActiveButton.CurrentButtonType == Button.ButtonType.Addon)
                {
                    this.SetSubMenuState(button, false);
                }
                if (this.ActiveButton == button)
                {
                    this.ActiveButton = null;
                }
            }
        }

        public void Remove(Button button)
        {
            if (button > null)
            {
                button.OnActiveStateChanged -= new DynamicControl.DynamicControlHandler(this.OnActiveStateChanged);
                if (this.LinkedContainers.ContainsKey(button))
                {
                    if (button.CurrentButtonType == Button.ButtonType.Addon)
                    {
                        foreach (Button button2 in this.GetSubMenus(button))
                        {
                            this.Remove(button2);
                        }
                    }
                    base.Remove(button);
                    MainMenu.Instance.Remove(this.LinkedContainers[button].Item1);
                    MainMenu.Instance.Remove(this.LinkedContainers[button].Item2);
                    this.LinkedContainers.Remove(button);
                }
            }
        }

        internal void SetContentView(Button button, bool state)
        {
            if (this.LinkedContainers.ContainsKey(button))
            {
                this.LinkedContainers[button].Item1.IsVisible = state;
                this.LinkedContainers[button].Item2.IsVisible = state;
            }
        }

        internal void SetSubMenuState(Button button, bool state)
        {
            int num = base.Children.FindIndex(o => o == button);
            if (button.CurrentButtonType == Button.ButtonType.AddonSub)
            {
                do
                {
                    num--;
                }
                while (base.Children[num].CurrentButtonType == Button.ButtonType.AddonSub);
            }
            num++;
            if (base.Children.Count > num)
            {
                for (int i = num; i < base.Children.Count; i++)
                {
                    if (base.Children[i].CurrentButtonType == Button.ButtonType.AddonSub)
                    {
                        base.Children[i].ExcludeFromParent = !state;
                    }
                    else
                    {
                        break;
                    }
                }
                this.RecalculateAlignOffsets();
            }
        }

        public Button ActiveButton { get; internal set; }

        internal Dictionary<Button, Tuple<Control, ControlContainer<ValueBase>>> LinkedContainers { get; set; }

        public override Vector2 Position
        {
            get => 
                base.Position;
            internal set
            {
                base.Position = value;
                if (this.LinkedContainers > null)
                {
                    foreach (Tuple<Control, ControlContainer<ValueBase>> tuple in this.LinkedContainers.Values)
                    {
                        tuple.Item1.RecalculatePosition();
                        tuple.Item2.RecalculatePosition();
                    }
                }
            }
        }
    }
}

