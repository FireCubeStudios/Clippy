using Clippy.Core.Services;
using Clippy.Services;
using Clippy.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using WinUIEx;
using WinRT;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Clippy
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsWindow : WindowEx
    {
        private SettingsService Settings = (SettingsService)App.Current.Services.GetService<ISettingsService>();

        public SettingsWindow()
        {
            this.InitializeComponent();
            this.ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);
            MicaWindow Mica = new();
            Mica.TrySetMicaBackdrop(this);
        }

        private async void Hub_Click(object sender, RoutedEventArgs e) => await Launcher.LaunchUriAsync(new Uri("https://discord.gg/3WYcKat"));

        private async void GitHub_Click(object sender, RoutedEventArgs e) => await Launcher.LaunchUriAsync(new Uri("https://github.com/FireCubeStudios/Clippy"));

        private void Exit_Click(object sender, RoutedEventArgs e) => Application.Current.Exit();

      
    }
}
