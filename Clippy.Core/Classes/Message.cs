using Clippy.Core.Enums;
using Clippy.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clippy.Core.Classes
{
	public record Message(Role Role, string MessageText, DateTime MessageDate) : IMessage
	{
		public Message(Role Role, string MessageText) : this(Role, MessageText, DateTime.Now) { }
	}
}
