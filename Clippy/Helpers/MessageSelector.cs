using Clippy.Core.Classes;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clippy.Helpers
{
    public class MessageSelector : DataTemplateSelector
    {
        public DataTemplate Clippy { get; set; }
        public DataTemplate User { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            if (item is ClippyMessage)
                return Clippy;
            else
                return User;
        }
    }
}
