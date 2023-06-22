using Clippy.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clippy.Core.ViewModels
{
    public partial class ClippyViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool isClippyEnabled = false;

        [ObservableProperty]
        private bool isPinned;

        public IChatService ChatService;

        public ISettingsService SettingsService;

        public ClippyViewModel(IChatService chatService, ISettingsService settingsService)
        {
            ChatService = chatService;
            SettingsService = settingsService;
            isPinned = SettingsService.AutoPin;
        }

        [RelayCommand]
        private void RefreshChat() => ChatService.Refresh();
    }
}
