using Clippy.Core.Classes;
using Clippy.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Clippy.Core.Interfaces;
using Clippy.Core.Enums;
using Clippy.Core.ViewModels.Messages;
using System.Collections.ObjectModel;
using Clippy.Core.Factories;
using System.Linq.Expressions;
using System.Diagnostics;

namespace Clippy.Core.ViewModels
{
    public partial class ClippyViewModel : ObservableObject
	{
		public ObservableCollection<MessageViewModel> MessagesVM = new();
		public ObservableCollection<IMessage> Messages = new();

		[ObservableProperty]
        private bool isClippyEnabled = true;

        [ObservableProperty]
        private bool isPinned = true;

		[ObservableProperty]
		private string currentText = "";

		[ObservableProperty]
		private DateTime updatedAt = DateTime.Now;

		public IChatService ChatService;

        public ISettingsService SettingsService;

        public ClippyViewModel(IChatService chatService, ISettingsService settingsService)
        {
            ChatService = chatService;
            SettingsService = settingsService;
            isPinned = SettingsService.AutoPin;
			SetupChat();
        }

		private void SetupChat()
		{
			AddMessage(new Message(Role.System, Constants.DEFAULT_SYSTEM_PROMPT));
			AddMessage(new Message(Role.Assistant, Constants.FIRST_CLIPPY_MESSAGE));
		}

		private MessageViewModel AddMessage(IMessage message)
		{
			var ViewModel = MessageFactory.GetMessageViewModel(message);
			MessagesVM.Add(ViewModel);
			Messages.Add(message);
			return ViewModel;
		}

		[RelayCommand(IncludeCancelCommand = true)]
		public async Task SendPrompt(CancellationToken cancellationToken)
		{
			try
			{
				if (!String.IsNullOrEmpty(CurrentText))
				{
					AddMessage(new Message(Role.User, CurrentText)); // update UI here
					CurrentText = "";
					await Task.Delay(300);

					var messageVM = AddMessage(new Message(Role.Assistant, "")) as ClippyMessageViewModel;
					UpdatedAt = DateTime.Now;
					messageVM?.StartStreamText(cancellationToken);

					try
					{
						await foreach (var chunk in ChatService.StreamChatAsync(Messages, cancellationToken))
							messageVM?.AddStreamText(chunk);
					}
					catch (TaskCanceledException)
					{
						return; // Task cancelled so it is ok
					}
					catch (Exception e)
					{
						messageVM?.Exception(e);
					}

					messageVM?.EndStreamText();
				}
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.Message);
			}
		}

		[RelayCommand]
        private void RefreshChat()
		{
			MessagesVM.Clear();
			Messages.Clear();
			SetupChat();
		}
    }
}
