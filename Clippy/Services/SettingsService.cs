using Clippy.Core.Services;
using Clippy.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Clippy.Services
{
    public class SettingsService : ObservableObject, ISettingsService
    {
        private static ApplicationDataContainer Settings = ApplicationData.Current.LocalSettings;

        private bool autoPin = (bool)(Settings.Values["AutoPin"] ?? false);
        public bool AutoPin
        {
            get => autoPin;
            set
            {
                Settings.Values["AutoPin"] = value;
                SetProperty(ref autoPin, value);
            }
        }

        private bool trayClippy = (bool)(Settings.Values["TrayClippy"] ?? true);
        public bool TrayClippy
        {
            get => trayClippy;
            set
            {
                Settings.Values["TrayClippy"] = value;
                SetProperty(ref trayClippy, value);
                if (value)
                    ClippyTrayListener.Recreate();
                else
                    ClippyTrayListener.Remove();
            }
        }

        private bool translucentBackground = (bool)(Settings.Values["TranslucentBackground"] ?? true);
        public bool TranslucentBackground
        {
            get => translucentBackground;
            set
            {
                Settings.Values["TranslucentBackground"] = value;
                SetProperty(ref translucentBackground, value);
            }
        }

        private int tokens = (int)(Settings.Values["Tokens"] ?? 100);
        public int Tokens
        {
            get => tokens;
            set
            {
                if (value > 50 && value < 2000)
                {
                    Settings.Values["Tokens"] = value;
                    SetProperty(ref tokens, value);
                }
                else
                    SetProperty(ref tokens, 100);
            }
        }

        private bool keyboardEnabled = (bool)(Settings.Values["KeyboardEnabled"] ?? true);
        public bool KeyboardEnabled
        {
            get => keyboardEnabled;
            set
            {
                Settings.Values["KeyboardEnabled"] = value;
                SetProperty(ref keyboardEnabled, value);
            }
        }

        private static bool hasKey = (bool)(Settings.Values["HasKey"] ?? false);
        public bool HasKey
        {
            get => hasKey;
            set
            {
                Settings.Values["HasKey"] = value;
                hasKey = value;
            }
        }
    }
}
