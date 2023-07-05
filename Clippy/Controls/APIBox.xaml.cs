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
using OpenAI.Managers;
using OpenAI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Clippy.Controls
{
    public sealed partial class APIBox : UserControl
    {
        private const string KeyInfo = "Unfortunately this key does not have enough OpenAI Tokens left. This could be because the free trial ran out or you are not using a paid OpenAI key with tokens. OpenAI API keys are seperate from ChatGPTPlus subscriptions and have different payment methods. For more information you can read the API key documentation at: https://platform.openai.com/docs/guides/production-best-practices/api-keys";
        private KeyService Keys = (KeyService)App.Current.Services.GetService<IKeyService>();
        private SettingsService Settings = (SettingsService)App.Current.Services.GetService<ISettingsService>();
        public APIBox()
        {
            this.InitializeComponent();
            if(Settings.HasKey)
                KeyBox.Password = Keys.GetKey();
        }

        private async void AddApi()
        {
            if (string.IsNullOrEmpty(KeyBox.Password))
            {
                Reject();
                return;
            }
            try
            {
                Ring.Visibility = Visibility.Visible;
                OpenAIService AI = new OpenAIService(new OpenAiOptions()
                {
                    ApiKey = KeyBox.Password
                });
                var completionResult = await AI.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
                {
                    Messages = new List<ChatMessage>
                    {
                        ChatMessage.FromSystem("Test")
                    },
                    Model = Models.ChatGpt3_5Turbo,
                    MaxTokens = 10,
                });

                if (completionResult.Successful)
                {
                    new KeyService().SetKey(KeyBox.Password);
                    Settings.HasKey = true;
                    ErrorBlock.Text = "";
                    Accept();
                }
                else
                {
                    ErrorBlock.Text = completionResult.Error.Type == "insufficient_quota" ? KeyInfo : completionResult.Error.Message;
                    Reject();
                }
                Ring.Visibility = Visibility.Collapsed;
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

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            new KeyService().SetKey("");
            Settings.HasKey = false;
        }

        private void Accept()
        {
            KeyBox.Foreground = GreenLinearGradientBrush;
            KeyBox.Focus(FocusState.Programmatic);
        }

        private void Reject()
        {
            KeyBox.Foreground = RedLinearGradientBrush;
            KeyBox.Focus(FocusState.Programmatic);
            KeyLoadAnimation.Start();
        }
    }
}
