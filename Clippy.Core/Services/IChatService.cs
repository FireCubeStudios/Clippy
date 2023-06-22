using Clippy.Core.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Clippy.Core.Services
{
    public interface IChatService
    {
        public ObservableCollection<IMessage> Messages { get; }

        public Task SendAsync(IMessage message);
        public void Refresh();
    }
}
