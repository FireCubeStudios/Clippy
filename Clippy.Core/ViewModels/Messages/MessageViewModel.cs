using CommunityToolkit.Mvvm.ComponentModel;
using Clippy.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Clippy.Core.ViewModels
{
	public abstract partial class MessageViewModel : ObservableObject
	{
		[ObservableProperty]
		private string messageText = "";

		protected IMessage Message;
		public MessageViewModel(IMessage message) 
		{ 
			this.Message = message;
			MessageText = this.Message.MessageText;
		}
	}
}
