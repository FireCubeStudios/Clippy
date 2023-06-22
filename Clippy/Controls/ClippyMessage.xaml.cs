using Clippy.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Clippy.Controls
{
    public sealed partial class ClippyMessage : UserControl
    {
        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set
            {
                SetValue(MessageProperty, value);
                if (value.Length == 0)
                    IsSendEnabled = false;
                else
                    IsSendEnabled = true;
            }
        }

        public static readonly DependencyProperty MessageProperty =
                   DependencyProperty.Register("Message", typeof(string), typeof(UserMessage), null);

        public bool IsSendEnabled
        {
            get => (bool)GetValue(IsSendEnabledProperty);
            set => SetValue(IsSendEnabledProperty, value);
        }

        public static readonly DependencyProperty IsSendEnabledProperty =
                   DependencyProperty.Register("IsSendEnabled", typeof(bool), typeof(UserMessage), null);

        private IChatService Chat = App.Current.Services.GetService<IChatService>();

        public ClippyMessage()
        {
            this.InitializeComponent();
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            if (SendBox.Text is not null && !String.IsNullOrEmpty(Message))
                Chat.SendAsync(new Clippy.Core.Classes.UserMessage(SendBox.Text));
        }

        private void SendBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter && SendBox.Text is not null && !String.IsNullOrEmpty(Message))
                Chat.SendAsync(new Clippy.Core.Classes.UserMessage(SendBox.Text));
        }
    }
}
