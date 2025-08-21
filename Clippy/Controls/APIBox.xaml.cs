using Clippy.Core.Services;
using Clippy.Services;
using CubeKit.UI.Icons;
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
    public sealed partial class APIBox : UserControl
    {
        private KeyService Keys = (KeyService)App.Current.Services.GetService<IKeyService>();

        public APIBox()
        {
            this.InitializeComponent();
            KeyBox.Password = Keys.GetKey();
        }

        private void AddApi()
        {
            if (string.IsNullOrEmpty(KeyBox.Password))
            {
                Reject();
                return;
            }
            try
            {
               /* OpenAIService AI = new OpenAIService(new OpenAiOptions()
                {
                    ApiKey = KeyBox.Password
                });*/
                Keys.SetKey(KeyBox.Password);
            }
            catch
            {
                Reject();
            }
        }

        private FluentSymbol PrivacyToIcon(bool? boolean) => (boolean ?? false) ? FluentSymbol.EyeShow20 : FluentSymbol.EyeHide20;

        private PasswordRevealMode PrivacyToPassword(bool? boolean) => (boolean ?? false) ? PasswordRevealMode.Visible : PasswordRevealMode.Hidden;

        private void ApiBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
                AddApi();
        }

        private void Submit_Click(object sender, RoutedEventArgs e) => AddApi();

        private void Accept() => KeyBox.Foreground = GreenLinearGradientBrush;

        private void Reject()
        {
            KeyBox.Foreground = RedLinearGradientBrush;
            KeyBox.Focus(FocusState.Programmatic);
            PasswordLoadAnimation.Start();
        }
    }
}
