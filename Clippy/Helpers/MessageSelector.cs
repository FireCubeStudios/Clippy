using Clippy.Core.Classes;
using Clippy.Core.ViewModels.Messages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clippy.Helpers
{
    public partial class MessageSelector : DataTemplateSelector
	{
		public DataTemplate ClippyMessageTemplate { get; set; }
		public DataTemplate UserMessageTemplate { get; set; }
		public DataTemplate SystemMessageTemplate { get; set; }

		protected override DataTemplate SelectTemplateCore(object item)
		{
			return item switch
			{
				ClippyMessageViewModel => ClippyMessageTemplate,
				UserMessageViewModel => UserMessageTemplate,
				SystemMessageViewModel => SystemMessageTemplate,
				_ => base.SelectTemplateCore(item)
			};
		}
	}
}
