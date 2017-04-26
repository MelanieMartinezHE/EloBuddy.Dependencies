namespace EloBuddy.SDK.Menu
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public static class ThemeManager
    {
        internal static Theme _currentTheme = DefaultTheme;
        public const string DefaultFontFaceName = "Gill Sans MT Pro Book";
        public static readonly Theme DefaultTheme = Theme.FromMemory("Default", Resources.Theme, Resources.Config);

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public static  event ThemeChangedHandler OnThemeChanged;

        internal static Theme.DynamicRectangle GetDynamicRectangle(DynamicControl control)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }
            switch (control.Type)
            {
                case SpriteType.ButtonExit:
                    return CurrentTheme.Config.SpriteAtlas.Controls.Buttons.Exit;

                case SpriteType.ButtonAddon:
                    return CurrentTheme.Config.SpriteAtlas.Controls.Buttons.Addon;

                case SpriteType.ButtonNormal:
                    return CurrentTheme.Config.SpriteAtlas.Controls.Buttons.Normal;

                case SpriteType.ButtonConfirm:
                    return CurrentTheme.Config.SpriteAtlas.Controls.Buttons.Confirm;

                case SpriteType.ButtonMini:
                    return CurrentTheme.Config.SpriteAtlas.Controls.Buttons.Mini;

                case SpriteType.ControlCheckBox:
                    return CurrentTheme.Config.SpriteAtlas.Controls.CheckBox;

                case SpriteType.ControlSlider:
                    return CurrentTheme.Config.SpriteAtlas.Controls.Slider;

                case SpriteType.ControlComboBox:
                    return CurrentTheme.Config.SpriteAtlas.Controls.ComboBox;
            }
            Console.WriteLine("ThemeManager.GetDynamicRectangle: {0} is not handled yet!", control.Type);
            return null;
        }

        internal static Theme.StaticRectangle GetStaticRectangle(Control control)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }
            switch (control.Type)
            {
                case SpriteType.FormComplete:
                    return CurrentTheme.Config.SpriteAtlas.MainForm.Complete;

                case SpriteType.FormHeader:
                    return CurrentTheme.Config.SpriteAtlas.MainForm.Header;

                case SpriteType.FormFooter:
                    return CurrentTheme.Config.SpriteAtlas.MainForm.Footer;

                case SpriteType.FormAddonButtonContainer:
                    return CurrentTheme.Config.SpriteAtlas.MainForm.AddonButtonContainer;

                case SpriteType.FormContentHeader:
                    return CurrentTheme.Config.SpriteAtlas.MainForm.ContentHeader;

                case SpriteType.FormContentContainer:
                    return CurrentTheme.Config.SpriteAtlas.MainForm.ContentContainer;

                case SpriteType.BackgroundSlider:
                    return CurrentTheme.Config.SpriteAtlas.Backgrounds.Slider;
            }
            Console.WriteLine("ThemeManager.GetStaticRectangle: {0} is not handled yet!", control.Type);
            return null;
        }

        public static Theme CurrentTheme
        {
            get => 
                _currentTheme;
            set
            {
                _currentTheme = value;
                if (OnThemeChanged > null)
                {
                    OnThemeChanged(EventArgs.Empty);
                }
            }
        }

        public enum SpriteType
        {
            Empty,
            FormComplete,
            FormHeader,
            FormFooter,
            FormAddonButtonContainer,
            FormContentHeader,
            FormContentContainer,
            BackgroundSlider,
            ButtonExit,
            ButtonAddon,
            ButtonNormal,
            ButtonConfirm,
            ButtonMini,
            ControlCheckBox,
            ControlSlider,
            ControlComboBox
        }

        public delegate void ThemeChangedHandler(EventArgs args);
    }
}

