using DonorPlus.ViewModels;
using System;
using Xamarin.Forms;

namespace DonorPlus.Views.Partials
{
    public partial class ChatInputBarView : ContentView
    {
        public ChatInputBarView()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS)
            {
                SetBinding(HeightRequestProperty, new Binding("Height",
                    BindingMode.OneWay, null, null, null, chatTextInput));
            }
        }
        public void Handle_Completed(object sender, EventArgs e)
        {
            (Parent.Parent.BindingContext as ChatPageViewModel).OnSendCommand.Execute(null);
            chatTextInput.Focus();
        }

        public void UnFocusEntry()
        {
            chatTextInput?.Unfocus();
        }
    }
}
