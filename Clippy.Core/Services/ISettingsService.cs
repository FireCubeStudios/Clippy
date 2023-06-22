using System;
using System.Collections.Generic;
using System.Text;

namespace Clippy.Core.Services
{
    public interface ISettingsService
    {
        public bool AutoPin { get; set; }
        public bool TrayClippy { get; set; }
        public bool TranslucentBackground { get; set; }
        public bool KeyboardEnabled { get; set; }
        public int Tokens { get; set; }
    }
}
