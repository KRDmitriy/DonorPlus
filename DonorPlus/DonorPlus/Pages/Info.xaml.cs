using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DonorPlus
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Info : ContentPage
    {
        public Info()
        {
            InitializeComponent();

            SetColors();

            AppearenceSettings.CheckAndSetColors += SetColors;
        }

        public void SetColors()
        {
            BackgroundColor = Storage.BackColor;
            InfoFrame.BackgroundColor = Info2Frame.BackgroundColor = Info3Frame.BackgroundColor =
                ExtraInfoFrame.BackgroundColor = Storage.BackColor;

            InfoLabel.TextColor = CreatorTextLabel.TextColor = CreatorLabel.TextColor =
                BossLabel.TextColor = BossTextLabel.TextColor =
                    ThanksToLabel.TextColor = ThanksToTextLabel.TextColor =
                    ExtraInfoLabel.TextColor = ExtraInfoTextLabel.TextColor = Storage.TextColor;

            SurnameLabelStack.BackgroundColor = BossStack.BackgroundColor =
                ThanksToLabelStack.BackgroundColor = ExtraInfoLabelStack.BackgroundColor =
                    Storage.HelpColor;
        }

        private void Link_Tapped(object sender, System.EventArgs e)
        {
            Device.OpenUri(new System.Uri(ExtraInfoLabel.Text));
        }
    }
}