using Plugin.Settings;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DonorPlus
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelfSettings : ContentPage
    {
        public SelfSettings()
        {
            InitializeComponent();

            MailsSwitch.IsToggled = Storage.IsMailsAvaliable;

            SetColors();
            AppearenceSettings.CheckAndSetColors += SetColors;
        }

        public void SetColors()
        {
            BackgroundColor = Storage.BackColor;
            NotifLabel.TextColor = Storage.TextColor;
            MailsLabel.TextColor = Storage.TextColor;
            PrivateLabel.TextColor = Storage.TextColor;
        }

        private void MailsSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            Storage.IsMailsAvaliable = MailsSwitch.IsToggled;
            CrossSettings.Current.AddOrUpdateValue("IsMailsAvaliable", MailsSwitch.IsToggled);
        }

        private void PrivateSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            //В разработке
        }
    }
}