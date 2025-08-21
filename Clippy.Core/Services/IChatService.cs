using Clippy.Core.Classes;
using Clippy.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Clippy.Core.Services
{
	public interface IChatService
	{
		Task<string> SendChatAsync(IEnumerable<IMessage> Messages);
		IAsyncEnumerable<string> StreamChatAsync(IEnumerable<IMessage> Messages, CancellationToken cancellationToken = default);
	}
}
