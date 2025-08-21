using Clippy.Core.Interfaces;
using Clippy.Core.ViewModels.Messages;
using Clippy.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clippy.Core.Factories
{
	public class MessageFactory
	{
		public static MessageViewModel GetMessageViewModel(IMessage message)
		{
			switch (message.Role)
			{
				case Enums.Role.User:
					return new UserMessageViewModel(message);
				case Enums.Role.Assistant:
					return new ClippyMessageViewModel(message);
				default:
					return new SystemMessageViewModel(message);
			}
		}
	}
}
