using System;
using Xamarin.Forms;
using DonorPlusLib;
using DonorPlus.Renderers;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;

namespace DonorPlus
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfirmPage : ContentPage
    {
        ActivityIndicatorRenderer activityIndicator = new ActivityIndicatorRenderer();

        public ConfirmPage()
        {
            InitializeComponent();

            SetColors();

            AppearenceSettings.CheckAndSetColors += SetColors;
        }

        public void SetColors()
        {
            BackgroundColor = Storage.BackColor;
            ConfirmButton.BackgroundColor = Storage.BackColor;

            ConfirmLabel.TextColor = ConfirmButton.TextColor =
                CodeEntry.TextColor = Storage.TextColor;

            CodeEntry.PlaceholderColor = Color.LightGray;
        }

        public async void ConfirmButton_Clicked(object sender, EventArgs e)
        {
            if (CodeEntry.Text != Storage.ConfirmCode)
            {
                await DisplayAlert("Ошибка", "Неверный код!\nПопробуйте ещё раз", "OK");
            }
            else
            {
                activityIndicator.Start();
                string res = "", res1 = "";
                if (Storage.IsEntering)
                {
                    Storage.User = Storage.temporaryData;
                    Storage.IsEntering = false;
                    App.Current.Properties.Clear();
                    App.Current.Properties.Add("user", Storage.User.Id);
                    App.LogIn(true);
                    await Navigation.PushAsync(new MainPage());
                    return;
                }
                else
                {
                    res = await Task.Run(() => Registration.Add(Storage.temporaryData));
                    res1 = await Task.Run(() => BloodData.Push(Storage.temporaryData.Id, Storage.temporaryData.BloodGroup,
                        Storage.temporaryData.RFactor));
                }
                activityIndicator.Stop();
                if (res == "OK" && res1 == "OK")
                {
                    await Navigation.PopToRootAsync();
                }
                else if (res == "OK")
                {
                    await DisplayAlert("Предупреждение", "Возможно данные крови не были добавлены", "OK");
                    await Navigation.PopToRootAsync();
                }
                else
                {
                    await DisplayAlert("Ошибка", "Некорректные данные или проблемы с интернетом", "OK");
                }
            }
        }
    }
}