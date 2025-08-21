using CommunityToolkit.WinUI.UI.Controls;
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
    public sealed partial class ShineUITextblock : UserControl
    {
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set
            {
                SetValue(TextProperty, value);
                if(value.Length == 0)
                {
                    MarkdownBlock.Visibility = Visibility.Collapsed;
                    Shimmer.Visibility = Visibility.Visible;
                }
                else
                {
                    MarkdownBlock.Visibility = Visibility.Visible;
                    Shimmer.Visibility = Visibility.Collapsed;
                }
            }
        }

        public static readonly DependencyProperty TextProperty =
                   DependencyProperty.Register("Text", typeof(string), typeof(ShineUITextblock), null);

        public ShineUITextblock()
        {
            this.InitializeComponent();
        }

        private async void MarkdownTextBlock_LinkClicked(object sender, LinkClickedEventArgs e) => await Launcher.LaunchUriAsync(new Uri(e.Link));
    }
}
