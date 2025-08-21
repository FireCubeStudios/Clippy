using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Clippy.Core.ViewModels.Messages
{
	public partial class ClippyMessageViewModel
	{
		[ObservableProperty]
		private string streamingText = "";

		[ObservableProperty]
		private bool isStreaming = false;

		private StringBuilder _streamBuffer = new();   // Holds text waiting to be typed
		private Task? _typingTask;
		private string TotalText = "";
		/// <summary>
		/// Starts the typing animation loop.
		/// </summary>
		public void StartStreamText(CancellationToken cancellationToken, int batchSize = 3, int delayMs = 15)
		{
			IsStreaming = true;
			_typingTask = Task.Run(async () =>
			{
				try
				{
					while (!cancellationToken.IsCancellationRequested)
					{
						if (_streamBuffer.Length >= batchSize)
						{
							string chunk = _streamBuffer.ToString(0, batchSize);
							_streamBuffer.Remove(0, batchSize);

							UIThread?.Invoke(() => StreamingText += chunk);

							await Task.Delay(delayMs, cancellationToken);
						}
						else
						{
							await Task.Delay(10, cancellationToken); // Wait for more input
						}
					}
				}
				catch (TaskCanceledException) { }
			});
		}

		/// <summary>
		/// Adds new text to the animation buffer.
		/// </summary>
		public void AddStreamText(string text)
		{
			TotalText += text;
			_streamBuffer.Append(text);
		}

		/// <summary>
		/// Ends the typing stream and flushes remaining text instantly.
		/// </summary>
		public void EndStreamText()
		{
			if (_streamBuffer.Length > 0)
			{
				StreamingText += _streamBuffer.ToString();
				_streamBuffer.Clear();
			}
			MessageText = TotalText;
			TotalText = "";
			IsStreaming = false;
		}
	}
}
