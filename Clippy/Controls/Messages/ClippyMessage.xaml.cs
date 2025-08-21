using Clippy.Core.Services;
using Clippy.Core.ViewModels.Messages;
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

namespace Clippy.Controls.Messages
{
    public sealed partial class ClippyMessage : UserControl
    {
		public ClippyMessageViewModel ViewModel
		{
			get => (ClippyMessageViewModel)GetValue(ViewModelProperty);
			set
			{
				if (value is not null)
					value.UIThread = action => DispatcherQueue.TryEnqueue(() => action());
				SetValue(ViewModelProperty, value);
			}
		}

		public static readonly DependencyProperty ViewModelProperty =
			DependencyProperty.Register(
				nameof(ViewModel),
				typeof(ClippyMessageViewModel),
				typeof(ClippyMessage),
				new PropertyMetadata(null));

		public bool IsSendEnabled
        {
            get => (bool)GetValue(IsSendEnabledProperty);
            set => SetValue(IsSendEnabledProperty, value);
        }

        public static readonly DependencyProperty IsSendEnabledProperty =
                   DependencyProperty.Register("IsSendEnabled", typeof(bool), typeof(UserMessage), null);

        public ClippyMessage()
        {
            this.InitializeComponent();
        }

		 private void Send_Click(object sender, RoutedEventArgs e)
		 {
			 //if (SendBox.Text is not null && !String.IsNullOrEmpty(Message))
				// Chat.SendAsync(new Clippy.Core.Classes.UserMessage(SendBox.Text));
		 }

		 private void SendBox_KeyDown(object sender, KeyRoutedEventArgs e)
		 {
			// if (e.Key == VirtualKey.Enter && SendBox.Text is not null && !String.IsNullOrEmpty(Message))
			//	 Chat.SendAsync(new Clippy.Core.Classes.UserMessage(SendBox.Text));
		 } 

		// Bool to Visibility
		public Visibility BoolToVis(bool b) => b ? Visibility.Visible : Visibility.Collapsed;

		// Bool to inverted visibility
		public Visibility InvertBoolToVis(bool b) => b ? Visibility.Collapsed : Visibility.Visible;
	}
}
