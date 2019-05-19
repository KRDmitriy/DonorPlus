using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DonorPlus
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppearenceSettings : ContentPage
    {
        public static event GlobalSetColors CheckAndSetColors;

        public AppearenceSettings()
        {
            InitializeComponent();

            SetColors();
            CheckAndSetColors += SetColors;

            DarkThemeSwitch.IsToggled = Storage.IsDarkTheme;
        }

        private void DarkThemeSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            if (DarkThemeSwitch.IsToggled)
            {
                Storage.SetDarkTheme();
            }
            else
            {
                Storage.SetLightTheme();
            }
            CheckAndSetColors();
        }

        public void SetColors()
        {
            BackgroundColor = Storage.BackColor;
            SettingLabel.TextColor = Storage.TextColor;
            DarkThemeLabel.TextColor = Storage.TextColor;
        }
    }
}