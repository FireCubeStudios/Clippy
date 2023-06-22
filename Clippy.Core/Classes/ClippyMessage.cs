using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clippy.Core.Classes
{
    public partial class ClippyMessage : ObservableObject, IMessage
    {
        [ObservableProperty]
        private bool isLatest = false; // Used for submitting new messages

        [ObservableProperty]
        private string? message;

        public ClippyMessage(string message) => Message = message;

        public ClippyMessage(string message, bool isLatest)
        {
            Message = message;
            IsLatest = isLatest;
        }

        public ClippyMessage(bool isLatest) => IsLatest = isLatest;
    }
}
