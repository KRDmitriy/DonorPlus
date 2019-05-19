using DonorPlus.Models;
using DonorPlus.Views.Cells;
using Xamarin.Forms;

namespace DonorPlus.Helpers
{
    internal class ChatTemplateSelector : DataTemplateSelector
    {
        private readonly DataTemplate incomingDataTemplate;
        private readonly DataTemplate outgoingDataTemplate;

        public ChatTemplateSelector()
        {
            incomingDataTemplate = new DataTemplate(typeof(IncomingViewCell));
            outgoingDataTemplate = new DataTemplate(typeof(OutgoingViewCell));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            Message messageVm = item as Message;
            if (messageVm == null)
            {
                return null;
            }

            return (messageVm.User == Storage.User.Id) ? outgoingDataTemplate : incomingDataTemplate;
        }

    }
}