using DonorPlus.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DonorPlus
{
    public delegate void GlobalSetColors();

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Settings : ContentPage
    {
        public Settings()
        {
            InitializeComponent();

            SetColors();

            AppearenceSettings.CheckAndSetColors += SetColors;

            MenuStack.Children.Add(new MenuFrameModel(null, "Настройки", 2));

            ProfileStack.Children.Add(new MenuFrameModel("appearence.png", "Внешний вид", 0));

            MapsStack.Children.Add(new MenuFrameModel("bell.png", "Уведомления", 0));

            InfoStack.Children.Add(new MenuFrameModel("exit.png", "Выйти", 0));
        }

        public void SetColors()
        {
            BackgroundColor = Storage.BackColor;
        }

        private async void GoToAppearence(object sender, System.EventArgs e)
        {
            AppearenceButton.Tapped -= GoToAppearence;
            await Navigation.PushModalAsync(new AppearenceSettings());
            AppearenceButton.Tapped += GoToAppearence;
        }

        private async void GoToNotifSettings(object sender, System.EventArgs e)
        {
            NotifSettingsButton.Tapped -= GoToNotifSettings;
            await Navigation.PushModalAsync(new SelfSettings());
            NotifSettingsButton.Tapped += GoToNotifSettings;
        }

        private void ToLeave()
        {
            Storage.User = Storage.Friend = null;
            App.Current.Properties.Clear();
            Navigation.PopModalAsync();
            App.Current.MainPage = new NavigationPage(new EnterPage());
        }

        private async void ExitButton_Tapped(object sender, System.EventArgs e)
        {
            ExitButton.Tapped -= ExitButton_Tapped;
            if (await DisplayAlert("Выход", "Вы действительно хотите выйти?\n" +
                "Всё ваши данные будут удалены из приложения", "Да", "Нет"))
            {
                ToLeave();
            }
            ExitButton.Tapped -= ExitButton_Tapped;
        }
    }
}