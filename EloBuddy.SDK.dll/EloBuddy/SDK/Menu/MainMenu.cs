namespace EloBuddy.SDK.Menu
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Rendering;
    using EloBuddy.SDK.Utils;
    using Newtonsoft.Json;
    using SharpDX;
    using SharpDX.Direct3D9;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Timers;

    public static class MainMenu
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static AddonContainer <AddonButtonContainer>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static uint <CurrentKeyDown>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static Button <ExitButton>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ControlContainer <Instance>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static bool <IsLoaded>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static Vector2 <MoveOffset>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static Dictionary<string, Dictionary<string, List<Dictionary<string, object>>>> <SavedValues>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static System.Timers.Timer <SaveTimer>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static EloBuddy.SDK.Rendering.Sprite <Sprite>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static Control <TitleBar>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static Text <TitleText>k__BackingField;
        public static readonly Dictionary<string, List<EloBuddy.SDK.Menu.Menu>> MenuInstances = new Dictionary<string, List<EloBuddy.SDK.Menu.Menu>>();
        internal static readonly string SaveDataBackupFilePath = (SaveDataFilePath + "_backup");
        internal static readonly string SaveDataDirectoryPath = (DefaultSettings.EloBuddyPath + Path.DirectorySeparatorChar + "MenuSaveData");
        internal const string SaveDataFileName = "SerializedMenu.ebm";
        internal static readonly string SaveDataFilePath = (SaveDataDirectoryPath + Path.DirectorySeparatorChar + "SerializedMenu.ebm");
        internal static readonly EloBuddy.SDK.Rendering.TextureLoader TextureLoader = new EloBuddy.SDK.Rendering.TextureLoader();

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public static  event CloseHandler OnClose;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public static  event OpenHandler OnOpen;

        public static EloBuddy.SDK.Menu.Menu AddMenu(string displayName, string uniqueMenuId, string longTitle = null)
        {
            if (string.IsNullOrWhiteSpace(displayName))
            {
                throw new ArgumentNullException("displayName");
            }
            if (string.IsNullOrWhiteSpace(uniqueMenuId))
            {
                throw new ArgumentNullException("uniqueMenuId");
            }
            string addonId = Assembly.GetCallingAssembly().GetAddonId();
            LoadAddonSavedValues(addonId);
            if (MenuInstances.ContainsKey(addonId) && MenuInstances[addonId].Any<EloBuddy.SDK.Menu.Menu>(o => (o.UniqueMenuId == uniqueMenuId)))
            {
                throw new ArgumentException("The provided unique menu id is already given!", uniqueMenuId);
            }
            return new EloBuddy.SDK.Menu.Menu(displayName, addonId, uniqueMenuId, longTitle, null);
        }

        internal static void CallMouseMoveMethods(ControlContainerBase container, Vector2 mousePosition, bool isOnOverlay = false, bool overlayCheck = true)
        {
            while (true)
            {
                List<Control> list = new List<Control>();
                if (overlayCheck)
                {
                    list.AddRange(from o in Control.OverlayControls
                        where o.IsVisible
                        select o);
                }
                else if (container.IsVisible)
                {
                    list.Add(container);
                    list.AddRange(from o in container.Children
                        where o.IsVisible
                        select o);
                }
                foreach (Control control in list)
                {
                    if (!control.ExcludeFromParent)
                    {
                        bool flag3 = control.IsInside(mousePosition);
                        if (isOnOverlay && !control.IsOverlay)
                        {
                            if (control.IsMouseInside)
                            {
                                control.IsMouseInside = false;
                                control.CallMouseLeave();
                            }
                        }
                        else if (flag3 == control.IsMouseInside)
                        {
                            control.CallMouseMove();
                        }
                        else
                        {
                            control.IsMouseInside = flag3;
                            if (flag3)
                            {
                                control.CallMouseEnter();
                            }
                            else
                            {
                                control.CallMouseLeave();
                            }
                        }
                        if (control != container)
                        {
                            ControlContainerBase base2 = control as ControlContainerBase;
                            if (base2 > null)
                            {
                                CallMouseMoveMethods(base2, mousePosition, isOnOverlay, false);
                            }
                        }
                    }
                }
                if (!overlayCheck)
                {
                    return;
                }
                overlayCheck = false;
            }
        }

        internal static void CallMouseUpMethods(ControlContainerBase container, bool leftButton = true)
        {
            CallMouseUpMethods(leftButton, container);
            foreach (Control control in container.Children)
            {
                if (!control.ExcludeFromParent)
                {
                    ControlContainerBase base2 = control as ControlContainerBase;
                    if (base2 > null)
                    {
                        CallMouseUpMethods(base2, leftButton);
                    }
                    else
                    {
                        CallMouseUpMethods(leftButton, control);
                    }
                }
            }
        }

        internal static void CallMouseUpMethods(bool leftButton, Control control)
        {
            if (control.OverlayControl > null)
            {
                ControlContainerBase overlayControl = control.OverlayControl as ControlContainerBase;
                if (overlayControl > null)
                {
                    CallMouseUpMethods(overlayControl, leftButton);
                }
                else
                {
                    CallMouseUpMethods(leftButton, control.OverlayControl);
                }
            }
            if (leftButton && control.IsLeftMouseDown)
            {
                control.IsLeftMouseDown = false;
                control.CallLeftMouseUp();
            }
            if (!leftButton && control.IsRightMouseDown)
            {
                control.IsRightMouseDown = false;
                control.CallRightMouseUp();
            }
        }

        internal static string GetAddonId(this Assembly assembly)
        {
            string str;
            try
            {
                str = assembly.GetName().Name + "_" + ((GuidAttribute) assembly.GetCustomAttributes(typeof(GuidAttribute), true)[0]).Value;
            }
            catch (Exception)
            {
                throw new Exception("GUID attribute is not defined!");
            }
            return str;
        }

        public static EloBuddy.SDK.Menu.Menu GetMenu(string uniqueMenuId)
        {
            string addonId = Assembly.GetCallingAssembly().GetAddonId();
            return (MenuInstances.ContainsKey(addonId) ? MenuInstances[addonId].Find(o => o.UniqueMenuId == uniqueMenuId) : null);
        }

        internal static Control GetTopMostControl(ControlContainerBase container, Vector2 position, bool validatePosition = true, bool checkOverlay = true) => 
            GetTopMostControl<Control>(container, position, validatePosition, checkOverlay);

        internal static T GetTopMostControl<T>(ControlContainerBase container, Vector2 position, bool validatePosition = true, bool checkOverlay = true) where T: Control
        {
            if (checkOverlay)
            {
                for (int j = Control.OverlayControls.Count - 1; j >= 0; j--)
                {
                    Control control = Control.OverlayControls[j];
                    if (control.IsVisible && control.IsInside(position))
                    {
                        ControlContainerBase base2 = control as ControlContainerBase;
                        if ((base2 != null) && !base2.GetType().IsAssignableFrom(typeof(T)))
                        {
                            return GetTopMostControl<T>(base2, position, false, false);
                        }
                        T local = control as T;
                        if (local > null)
                        {
                            return local;
                        }
                    }
                }
            }
            if (validatePosition && !container.IsInside(position))
            {
                return default(T);
            }
            for (int i = container.Children.Count - 1; i >= 0; i--)
            {
                Control control2 = container.Children[i];
                if ((control2.IsVisible && !control2.ExcludeFromParent) && control2.IsInside(position))
                {
                    ControlContainerBase base3 = control2 as ControlContainerBase;
                    if ((base3 != null) && !base3.GetType().IsAssignableFrom(typeof(T)))
                    {
                        return GetTopMostControl<T>(base3, position, false, false);
                    }
                    T local4 = control2 as T;
                    if (local4 > null)
                    {
                        return local4;
                    }
                }
            }
            return (container as T);
        }

        internal static void Initialize()
        {
            SavedValues = new Dictionary<string, Dictionary<string, List<Dictionary<string, object>>>>();
            SimpleControlContainer container1 = new SimpleControlContainer(ThemeManager.SpriteType.FormComplete, false, true, false, false) {
                IsVisible = false
            };
            Instance = container1;
            Sprite = new EloBuddy.SDK.Rendering.Sprite(() => ThemeManager.CurrentTheme.Texture);
            AddonContainer control = new AddonContainer();
            AddonButtonContainer = control;
            Instance.Add(control);
            EmptyControl control1 = new EmptyControl(ThemeManager.SpriteType.FormHeader);
            TitleBar = control1;
            Instance.Add(control1);
            Button button1 = new Button(Button.ButtonType.Exit, null);
            ExitButton = button1;
            Instance.Add(button1);
            FontDescription fontDescription = new FontDescription {
                FaceName = "Gill Sans MT Pro Medium",
                Height = 0x1c,
                Quality = FontQuality.Antialiased,
                Weight = FontWeight.ExtraBold,
                Width = 12
            };
            Text item = new Text("ELOBUDDY", fontDescription) {
                TextAlign = Text.Align.Center,
                TextOrientation = Text.Orientation.Center,
                Color = System.Drawing.Color.FromArgb(0xff, 0x8f, 0x7a, 0x48),
                Padding = new Vector2(0f, 3f)
            };
            TitleText = item;
            TitleBar.TextObjects.Add(item);
            ExitButton.OnActiveStateChanged += delegate (DynamicControl <sender>, EventArgs <args>) {
                if (ExitButton.IsActive)
                {
                    IsVisible = false;
                    ExitButton.IsActive = false;
                }
            };
            TitleBar.OnLeftMouseDown += (<sender>, <args>) => (MoveOffset = Position - Game.CursorPos2D);
            TitleBar.OnLeftMouseUp += (<sender>, <args>) => (MoveOffset = Vector2.Zero);
            Messages.OnMessage += delegate (Messages.WindowMessage args) {
                if (IsMouseInside)
                {
                    args.Process = false;
                }
            };
            Position = (Vector2) ((new Vector2((float) Drawing.Width, (float) Drawing.Height) - Instance.Size) / 2f);
            SaveTimer = new System.Timers.Timer(60000.0);
            SaveTimer.Elapsed += new ElapsedEventHandler(MainMenu.OnSaveTimerElapsed);
            SaveTimer.Start();
            OnSaveTimerElapsed(null, null);
            ThemeManager.OnThemeChanged += new ThemeManager.ThemeChangedHandler(MainMenu.OnThemeChanged);
            EloBuddy.SDK.Rendering.Sprite.OnMenuDraw += new EloBuddy.SDK.Rendering.Sprite.MenuDrawHandler(MainMenu.OnMenuDraw);
            Messages.OnMessage += new Messages.MessageHandler(MainMenu.OnWndMessage);
            AppDomain.CurrentDomain.DomainUnload += new EventHandler(MainMenu.OnUnload);
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(MainMenu.OnUnload);
            IsLoaded = true;
        }

        internal static void LoadAddonSavedValues(string addonId)
        {
            if (!SavedValues.ContainsKey(addonId))
            {
                SavedValues.Add(addonId, new Dictionary<string, List<Dictionary<string, object>>>());
                Directory.CreateDirectory(SaveDataDirectoryPath);
                string path = Path.Combine(SaveDataDirectoryPath, addonId + ".json");
                if (File.Exists(path))
                {
                    Dictionary<string, List<Dictionary<string, object>>> dictionary = JsonConvert.DeserializeObject<Dictionary<string, List<Dictionary<string, object>>>>(File.ReadAllText(path));
                    SavedValues[addonId] = dictionary;
                }
            }
        }

        internal static void OnMenuDraw(EventArgs args)
        {
            if (IsVisible)
            {
                Instance.Draw();
                Instance.TextObjects.ForEach(o => o.Draw());
                Control.OverlayControls.ForEach(overlay => overlay.Draw());
            }
        }

        internal static void OnSaveTimerElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            Directory.CreateDirectory(SaveDataDirectoryPath);
            foreach (KeyValuePair<string, List<EloBuddy.SDK.Menu.Menu>> pair in MenuInstances)
            {
                Dictionary<string, List<Dictionary<string, object>>> dictionary = new Dictionary<string, List<Dictionary<string, object>>>();
                foreach (EloBuddy.SDK.Menu.Menu menu in pair.Value)
                {
                    dictionary[menu.UniqueMenuId] = menu.ValueContainer.Serialize();
                }
                if (dictionary.Count > 0)
                {
                    Dictionary<string, List<Dictionary<string, object>>> dictionary2 = SavedValues.ContainsKey(pair.Key) ? SavedValues[pair.Key] : new Dictionary<string, List<Dictionary<string, object>>>();
                    foreach (KeyValuePair<string, List<Dictionary<string, object>>> pair2 in dictionary)
                    {
                        dictionary2[pair2.Key] = pair2.Value;
                    }
                    string path = Path.Combine(SaveDataDirectoryPath, pair.Key + ".json");
                    string destFileName = path + ".backup";
                    if (File.Exists(path))
                    {
                        File.Copy(path, destFileName, true);
                    }
                    try
                    {
                        File.WriteAllText(path, JsonConvert.SerializeObject(dictionary2));
                    }
                    catch
                    {
                        if (File.Exists(destFileName))
                        {
                            Logger.Warn("Error during config file writing, restoring backup!", new object[0]);
                            File.Copy(destFileName, path, true);
                        }
                    }
                    File.Delete(destFileName);
                }
            }
        }

        internal static void OnThemeChanged(EventArgs args)
        {
            Sprite = new EloBuddy.SDK.Rendering.Sprite(() => ThemeManager.CurrentTheme.Texture);
            Instance.OnThemeChange();
        }

        internal static void OnUnload(object sender, EventArgs eventArgs)
        {
            if (SaveTimer > null)
            {
                OnSaveTimerElapsed(null, null);
                SaveTimer.Dispose();
                SaveTimer = null;
            }
            TextureLoader.Dispose();
            TitleText.Dispose();
        }

        internal static void OnWndMessage(Messages.WindowMessage args)
        {
            if (!Chat.IsOpen)
            {
                WindowMessages message = args.Message;
                if (((message == WindowMessages.KeyDown) || (message == WindowMessages.KeyUp)) && ((args.Handle.WParam == 0x10) && ((args.Message != WindowMessages.KeyDown) || (CurrentKeyDown != 0x10))))
                {
                    IsVisible = args.Message == WindowMessages.KeyDown;
                }
                switch (args.Message)
                {
                    case WindowMessages.KeyDown:
                        if (CurrentKeyDown != args.Handle.WParam)
                        {
                            CurrentKeyDown = args.Handle.WParam;
                            Instance.OnKeyDown((Messages.KeyDown) args);
                        }
                        break;

                    case WindowMessages.KeyUp:
                        CurrentKeyDown = 0;
                        Instance.OnKeyUp((Messages.KeyUp) args);
                        break;
                }
            }
            else
            {
                IsVisible = false;
            }
            if (IsVisible)
            {
                Messages.MouseEvent event2 = args as Messages.MouseEvent;
                if (event2 > null)
                {
                    switch (args.Message)
                    {
                        case WindowMessages.MouseMove:
                            if (TitleBar.IsLeftMouseDown)
                            {
                                Position = Game.CursorPos2D + MoveOffset;
                            }
                            break;

                        case WindowMessages.LeftButtonUp:
                        case WindowMessages.RightButtonUp:
                            CallMouseUpMethods(Instance, args.Message == WindowMessages.LeftButtonUp);
                            break;
                    }
                    CallMouseMoveMethods(Instance, event2.MousePosition, Control.IsOnOverlay(event2.MousePosition), true);
                    switch (args.Message)
                    {
                        case WindowMessages.LeftButtonDown:
                        case WindowMessages.RightButtonDown:
                        {
                            if (args.Message == WindowMessages.LeftButtonDown)
                            {
                                ControlContainerBase base2 = GetTopMostControl<ControlContainerBase>(Instance, event2.MousePosition, false, true);
                                if ((base2 != null) && base2.ContainerView.CheckScrollbarDown(event2.MousePosition))
                                {
                                    break;
                                }
                            }
                            Control control = GetTopMostControl(Instance, event2.MousePosition, false, true);
                            switch (args.Message)
                            {
                                case WindowMessages.LeftButtonDown:
                                    control.IsLeftMouseDown = true;
                                    control.CallLeftMouseDown();
                                    break;

                                case WindowMessages.RightButtonDown:
                                    control.IsRightMouseDown = true;
                                    control.CallRightMouseDown();
                                    break;
                            }
                            break;
                        }
                        case WindowMessages.MouseWheel:
                        {
                            ControlContainerBase base3 = GetTopMostControl<ControlContainerBase>(Instance, event2.MousePosition, false, true);
                            if (base3 > null)
                            {
                                base3.CallMouseWheel((Messages.MouseWheel) args);
                            }
                            break;
                        }
                    }
                }
            }
        }

        internal static void RemoveAllMouseInteractions(ControlContainerBase container)
        {
            container.IsLeftMouseDown = false;
            container.IsRightMouseDown = false;
            container.IsMouseInside = false;
            foreach (Control control in container.Children)
            {
                control.IsLeftMouseDown = false;
                control.IsRightMouseDown = false;
                control.IsMouseInside = false;
                ControlContainerBase base2 = control as ControlContainerBase;
                if (base2 > null)
                {
                    RemoveAllMouseInteractions(base2);
                }
                else
                {
                    DynamicControl control2 = control as DynamicControl;
                    if (control2 > null)
                    {
                        control2.SetDefaultState();
                    }
                }
            }
        }

        internal static AddonContainer AddonButtonContainer
        {
            [CompilerGenerated]
            get => 
                <AddonButtonContainer>k__BackingField;
            [CompilerGenerated]
            set
            {
                <AddonButtonContainer>k__BackingField = value;
            }
        }

        internal static uint CurrentKeyDown
        {
            [CompilerGenerated]
            get => 
                <CurrentKeyDown>k__BackingField;
            [CompilerGenerated]
            set
            {
                <CurrentKeyDown>k__BackingField = value;
            }
        }

        internal static Button ExitButton
        {
            [CompilerGenerated]
            get => 
                <ExitButton>k__BackingField;
            [CompilerGenerated]
            set
            {
                <ExitButton>k__BackingField = value;
            }
        }

        internal static ControlContainer Instance
        {
            [CompilerGenerated]
            get => 
                <Instance>k__BackingField;
            [CompilerGenerated]
            set
            {
                <Instance>k__BackingField = value;
            }
        }

        public static bool IsLoaded
        {
            [CompilerGenerated]
            get => 
                <IsLoaded>k__BackingField;
            [CompilerGenerated]
            internal set
            {
                <IsLoaded>k__BackingField = value;
            }
        }

        public static bool IsMouseInside =>
            (Instance.IsMouseInside || ExitButton.IsMouseInside);

        public static bool IsOpen =>
            IsVisible;

        public static bool IsVisible
        {
            get => 
                Instance.IsVisible;
            internal set
            {
                if (Instance.IsVisible != value)
                {
                    Instance.IsVisible = value;
                    if (!value)
                    {
                        RemoveAllMouseInteractions(Instance);
                        if (OnClose > null)
                        {
                            OnClose(null, EventArgs.Empty);
                        }
                    }
                    else if (OnOpen > null)
                    {
                        OnOpen(null, EventArgs.Empty);
                    }
                }
            }
        }

        internal static Vector2 MoveOffset
        {
            [CompilerGenerated]
            get => 
                <MoveOffset>k__BackingField;
            [CompilerGenerated]
            set
            {
                <MoveOffset>k__BackingField = value;
            }
        }

        public static Vector2 Position
        {
            get => 
                Instance.Position;
            internal set
            {
                Instance.Position = value;
            }
        }

        internal static Dictionary<string, Dictionary<string, List<Dictionary<string, object>>>> SavedValues
        {
            [CompilerGenerated]
            get => 
                <SavedValues>k__BackingField;
            [CompilerGenerated]
            set
            {
                <SavedValues>k__BackingField = value;
            }
        }

        internal static System.Timers.Timer SaveTimer
        {
            [CompilerGenerated]
            get => 
                <SaveTimer>k__BackingField;
            [CompilerGenerated]
            set
            {
                <SaveTimer>k__BackingField = value;
            }
        }

        internal static EloBuddy.SDK.Rendering.Sprite Sprite
        {
            [CompilerGenerated]
            get => 
                <Sprite>k__BackingField;
            [CompilerGenerated]
            set
            {
                <Sprite>k__BackingField = value;
            }
        }

        internal static Control TitleBar
        {
            [CompilerGenerated]
            get => 
                <TitleBar>k__BackingField;
            [CompilerGenerated]
            set
            {
                <TitleBar>k__BackingField = value;
            }
        }

        internal static Text TitleText
        {
            [CompilerGenerated]
            get => 
                <TitleText>k__BackingField;
            [CompilerGenerated]
            set
            {
                <TitleText>k__BackingField = value;
            }
        }

        public static List<string> UsedUniqueNames =>
            new List<string>(MenuInstances.Keys);

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly MainMenu.<>c <>9 = new MainMenu.<>c();
            public static Func<Texture> <>9__70_0;
            public static DynamicControl.DynamicControlHandler <>9__70_1;
            public static Control.ControlHandler <>9__70_2;
            public static Control.ControlHandler <>9__70_3;
            public static Messages.MessageHandler <>9__70_4;
            public static Func<Control, bool> <>9__79_0;
            public static Func<Control, bool> <>9__79_1;
            public static Func<Texture> <>9__80_0;
            public static Action<Text> <>9__81_0;
            public static Action<Control> <>9__81_1;

            internal bool <CallMouseMoveMethods>b__79_0(Control o) => 
                o.IsVisible;

            internal bool <CallMouseMoveMethods>b__79_1(Control o) => 
                o.IsVisible;

            internal Texture <Initialize>b__70_0() => 
                ThemeManager.CurrentTheme.Texture;

            internal void <Initialize>b__70_1(DynamicControl <sender>, EventArgs <args>)
            {
                if (MainMenu.ExitButton.IsActive)
                {
                    MainMenu.IsVisible = false;
                    MainMenu.ExitButton.IsActive = false;
                }
            }

            internal void <Initialize>b__70_2(Control <sender>, EventArgs <args>)
            {
                MainMenu.MoveOffset = MainMenu.Position - Game.CursorPos2D;
            }

            internal void <Initialize>b__70_3(Control <sender>, EventArgs <args>)
            {
                MainMenu.MoveOffset = Vector2.Zero;
            }

            internal void <Initialize>b__70_4(Messages.WindowMessage args)
            {
                if (MainMenu.IsMouseInside)
                {
                    args.Process = false;
                }
            }

            internal void <OnMenuDraw>b__81_0(Text o)
            {
                o.Draw();
            }

            internal void <OnMenuDraw>b__81_1(Control overlay)
            {
                overlay.Draw();
            }

            internal Texture <OnThemeChanged>b__80_0() => 
                ThemeManager.CurrentTheme.Texture;
        }

        public delegate void CloseHandler(object sender, EventArgs args);

        public delegate void OpenHandler(object sender, EventArgs args);
    }
}

