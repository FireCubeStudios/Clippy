using Clippy.Core.Classes;
using OpenAI;
using OpenAI.Managers;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Clippy.Core.Services
{
    public class ChatGPTService : IChatService
    {
        private const string ClippyKey = "Hello there! To use this app you need a valid OpenAI API key. You can obtain one by visiting https://beta.openai.com/signup/ and following the instructions to create an account. Once you have an account, you can generate your API key and enter it into the API field in Settings then refreshing this chat dialog!";
        private const string ClippyStart = "Hi! I'm Clippy, your Windows assistant. Would you like to get some assistance?";
        private const string Instruction = "You are in an app that revives Microsoft Clippy in Windows. Speak in a Clippy style and try to stay as concise/short as possible and not output long messages.";
        public ObservableCollection<IMessage> Messages { get; } = new ObservableCollection<IMessage>();

        private OpenAIService AI;

        private ISettingsService Settings;
        private IKeyService KeyService;

        public ChatGPTService(ISettingsService settings, IKeyService keys)
        {
            Settings = settings;
            KeyService = keys;
            if (SetAPI() || Settings.HasKey) // Refresh API key
                Add(new ClippyMessage(ClippyStart, true));
        }
             
        public void Refresh()
        {
            Messages.Clear();
            if(SetAPI() || Settings.HasKey) // Refresh API key
                Add(new ClippyMessage(ClippyStart, true));
        }

        public async Task SendAsync(IMessage message) /// Send a message
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
                Response.Message = completionResult.Choices.First().Message.Content;
            }
            else
            {
                Response.Message = $"Unfortunately an error occured `{completionResult.Error.Message}`";
                Response.IsLatest = false;
            }
        }

        private void Add(IMessage Message) /// Add a message
        {
            foreach(IMessage message in Messages) /// Remove any editable message
            {
                if (message is ClippyMessage)
                    ((ClippyMessage)message).IsLatest = false;
            }
            Messages.Add(Message);
        }

        /// <summary>
        /// Initialise the OpenAI API and refresh API key
        /// </summary>
        private bool SetAPI()
        {
            try
            {
                AI = new OpenAIService(new OpenAiOptions()
                {
                    ApiKey = KeyService.GetKey()
                });
                return true;
            }
            catch
            {
                Add(new ClippyMessage(ClippyKey, false));
                return false;
            }
        }
    }
}
