using Clippy.Core.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Clippy.Core.Interfaces
{
	public interface IMessage
	{
		public Role Role { get; init; }
		public DateTime MessageDate { get; init; }
		public string MessageText { get; init; }
	}
}
