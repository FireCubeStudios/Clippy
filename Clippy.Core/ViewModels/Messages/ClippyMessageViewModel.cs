using Clippy.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Clippy.Core.ViewModels.Messages
{
	public partial class ClippyMessageViewModel : MessageViewModel
	{
		// Run code on the UI Thread in the ViewModel
		// This is useful to update ObservableProperties from background threads
		public Action<Action>? UIThread;

		public ClippyMessageViewModel(IMessage message) : base(message)
		{
		}

		public void Exception(Exception exception)
		{
			// Show error UI
		}
	}
}
