using DonorPlus.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DonorPlus
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Menu : ContentPage
    {
        public Menu()
        {
            InitializeComponent();

            SetColors();

            AppearenceSettings.CheckAndSetColors += SetColors;

            MenuStack.Children.Add(new MenuFrameModel(null, "Меню", 2));

            ProfileStack.Children.Add(new MenuFrameModel("user.png", "Профиль", 0));

            RequestStack.Children.Add(new MenuFrameModel("edit.png", "Добавить запрос", 0));

            InfoStack.Children.Add(new MenuFrameModel("info.png", "О программе", 0));

            SettingsStack.Children.Add(new MenuFrameModel("settings.png", "Настройки", 0));
        }

        public void SetColors()
        {
            BackgroundColor = Storage.BackColor;
        }

        private async void GoToProfile(object sender, System.EventArgs e)
        {
            ProfileButton.Tapped -= GoToProfile;
            Storage.IsUserNow = true;
            await Navigation.PushModalAsync(new Profile());
            ProfileButton.Tapped += GoToProfile;
        }

        private async void GoToRequest(object sender, System.EventArgs e)
        {
            RequestButton.Tapped -= GoToRequest;
            await Navigation.PushModalAsync(new RequestPage());
            RequestButton.Tapped += GoToRequest;
        }

        private async void GoToInfo(object sender, System.EventArgs e)
        {
            InfoButton.Tapped -= GoToInfo;
            await Navigation.PushModalAsync(new Info());
            InfoButton.Tapped += GoToInfo;
        }

        private async void GoToSettings(object sender, System.EventArgs e)
        {
            SettingsButton.Tapped -= GoToSettings;
            await Navigation.PushModalAsync(new Settings());
            SettingsButton.Tapped += GoToSettings;
        }
    }
}