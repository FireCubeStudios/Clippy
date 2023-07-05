using System;
using System.Collections.Generic;
using System.Text;

namespace Clippy.Core.Classes
{
    public class AnnouncementMessage : IMessage // Message shows up on Clippy side with no MessageTriangle
    {
        public string? Message { get; set; }

        public AnnouncementMessage(string message) => Message = message;
    }
}
