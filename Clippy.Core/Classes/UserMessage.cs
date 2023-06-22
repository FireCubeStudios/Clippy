using System;
using System.Collections.Generic;
using System.Text;

namespace Clippy.Core.Classes
{
    public class UserMessage : IMessage
    {
        public string? Message { get; set; }

        public UserMessage(string message) => Message = message;
    }
}
