using Clippy.Core.Classes;
using OpenAI;
using OpenAI.Managers;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clippy.Core.Services
{
    public class ChatGPTService : IChatService
    {
        private const string ClippyKey = "Hello there! To use this app you need an OpenAI API key. You can obtain one by visiting https://beta.openai.com/signup/ and following the instructions to create an account. Once you have an account, you can generate your API key and enter it into the provided field to start using the app. Don't hesitate to let me know if you have any other questions or concerns!";
        private const string ClippyStart = "Hi! I'm Clippy, your Windows assistant. Would you like to get some assistance?";
        private const string Instruction = "You are in an app that revives Microsoft Clippy in Windows. Speak in a Clippy style and try to stay as concise/short as possible and not output long messages.";
        public ObservableCollection<IMessage> Messages { get; } = new ObservableCollection<IMessage>();

         private OpenAIService AI = new OpenAIService(new OpenAiOptions()
         {
             ApiKey = "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"
         });

        private ISettingsService Settings;

        public ChatGPTService(ISettingsService settings)
        {
            Settings = settings;
            Add(new ClippyMessage(ClippyStart, true));
        }
             
        public void Refresh()
        {
            Messages.Clear();
            Add(new ClippyMessage(ClippyStart, true));
        }

        public async Task SendAsync(IMessage message)
        {
            Add(message); // Send user message to UI
            List<ChatMessage> GPTMessages = new List<ChatMessage>
            {
                ChatMessage.FromSystem(Instruction)
            };
            foreach (IMessage m in Messages) // Remove any editable message
            {
                if (message is ClippyMessage)
                    GPTMessages.Add(ChatMessage.FromAssistant(m.Message));
                else
                    GPTMessages.Add(ChatMessage.FromUser(m.Message));
            }
            await Task.Delay(300);
            ClippyMessage Response = new ClippyMessage(true);
            Add(Response); // Send empty message and update text later to show preview UI

            GPTMessages.Add(ChatMessage.FromUser(message.Message));

            var completionResult = await AI.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
            {
                Messages = GPTMessages,
                Model = Models.ChatGpt3_5Turbo,
                MaxTokens = Settings.Tokens,
            });
            if (completionResult.Successful)
            {
                Console.WriteLine(completionResult.Choices);
                Response.Message = completionResult.Choices.First().Message.Content;
            }
            else
            {
                Response.Message = $"Unfortunately an error occured `{completionResult.Error}`";
            }
        }

        private void Add(IMessage Message)
        {
            foreach(IMessage message in Messages) // Remove any editable message
            {
                if (message is ClippyMessage)
                    ((ClippyMessage)message).IsLatest = false;
            }
            Messages.Add(Message);
        }
    }
}
