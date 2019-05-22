using DonorPlusLib;
using Plugin.Connectivity;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DonorPlus
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EnterPage : ContentPage
    {
        private bool IsCheck;

        public EnterPage()
        {
            InitializeComponent();

            SetColors();

            IsCheck = false;
            EnterButton.Text = "Проверить";
            Password.IsVisible = false;

            AppearenceSettings.CheckAndSetColors += SetColors;
        }

        public void SetColors()
        {
            BackgroundColor = Storage.BackColor;

            RegButton.BackgroundColor = EnterButton.BackgroundColor =
                Foggot.BackgroundColor = Storage.BackColor;

            Login.PlaceholderColor = Password.PlaceholderColor = Color.LightGray;

            Login.TextColor = Password.TextColor =
                RegLabel.TextColor = EnterButton.TextColor =
                Foggot.TextColor = RegButton.TextColor = Storage.TextColor;
        }

        private async void Foggot_Clicked(object sender, EventArgs e)
        {
            Foggot.IsEnabled = false;
            IsBusy = true;
            CheckParams();

            bool res;
            try
            {
                res = await SendCode();
            }
            catch
            {
                res = false;
            }

            if (res)
            {
                Storage.temporaryData = Users.GetFromPhoneOrMail(Login.Text.Trim()).User;
                await Navigation.PushAsync(new ConfirmPage());
            }
            else
            {
                await DisplayAlert("Ошибка", "Что-то пошло не так", "OK");
            }

            IsBusy = false;
            Foggot.IsEnabled = true;
        }

        public async Task<bool> SendCode()
        {
            RegistrationPage.GenerateCode();
            if (CrossConnectivity.Current.IsConnected)
            {
                return await RememberCodeAsync(Login.Text.Trim());
            }
            else
            {
                await DisplayAlert("Ошибка", "Введённые данные неверны или отсутствует подключение к интернету", "ОК");
                return false;
            }
        }

        private async void EnterButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                EnterButton.IsEnabled = false;
                IsBusy = true;

                if (!IsCheck)
                {
                    EnterButton.Text = "Войти";
                    CheckParams();
                    Password.PlaceholderColor = Color.LightGray;
                    Password.IsVisible = true;
                    IsCheck = true;
                }
                else
                {
                    if (CrossConnectivity.Current.IsConnected)
                    {
                        if (CheckParams())
                        {
                            ResultObj result = await Task.Run(() => DonorPlusLib.Login.CheckLogin(Login.Text.Trim(), Password.Text));
                            if (result.ErrorMessage == "OK")
                            {
                                Storage.User = result.User;
                                Storage.User.AddPhoto(Photo.Get(result.User.Id).Image);
                                ResultObj blood = BloodData.Get(result.User.Id);
                                Storage.User.AddBloodGroup(blood.BloodGroup);
                                Storage.User.AddRFactor(blood.RFactor);


                                if (Storage.IsMailsAvaliable)
                                {
                                    SendEmailAsync(Storage.User.Email).GetAwaiter();
                                }

                                App.Current.Properties.Clear();

                                App.Current.Properties.Add("user", result.User.Id);

                                if (Navigation.NavigationStack.Count == 1)
                                {
                                    App.Current.MainPage = new NavigationPage(new MainPage());
                                }
                                else
                                {
                                    await Navigation.PushModalAsync(new MainPage());
                                }
                                App.LogIn(true);
                            }
                            else
                            {
                                IsBusy = false;
                                await DisplayAlert("Ошибка", "Некорректные данные или проблемы с интернетом", "ОК");
                            }
                        }
                    }
                    else
                    {
                        await DisplayAlert("Интернет отсутствует", "Подключите интернет", "ОК");
                    }
                }
                IsBusy = false;
                EnterButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", $"{ex.Message}", "OK");
                EnterButton_Clicked(sender, e);
            }
        }

        private async void RegButton_Clicked(object sender, EventArgs e)
        {
            RegButton.IsEnabled = false;
            IsBusy = true;
            await Navigation.PushAsync(new RegistrationPage(false));
            IsBusy = false;
            RegButton.IsEnabled = true;
        }

        private bool CheckParams()
        {
            if (Login.Text == null)
            {
                Login.PlaceholderColor = Color.Red;
                return false;
            }
            if (Password.Text == null)
            {
                Password.PlaceholderColor = Color.Red;
                return false;
            }
            return true;
        }

        private static async Task SendEmailAsync(string email)
        {
            MailAddress from = new MailAddress(Storage.Email, "DonorPlus");
            MailAddress to = new MailAddress(email);
            MailMessage message = new MailMessage(from, to)
            {
                Subject = "Вход в аккаунт",
                Body = $"Здравствуйте {Storage.User.Name} {Storage.User.SecondName}!\n" +
                    $"Вы зашли в свой аккаунт Donor+\n" +
                    $"Если это не Вы, напишите в нашу поддержку '{Storage.Email}'\n\n" +
                    $"С уважением,\nКоманда Donor+"
            };
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(Storage.Email, Storage.MailPasswd),
                EnableSsl = true
            };
            await smtp.SendMailAsync(message);
        }

        private static async Task<bool> RememberCodeAsync(string email)
        {
            ResultObj result = await Task.Run(() => Users.GetFromPhoneOrMail(email));
            if (result.User != null)
            {
                Storage.temporaryData = result.User;
                Storage.IsEntering = true;
                MailAddress from = new MailAddress(Storage.Email, "DonorPlus");
                MailAddress to = new MailAddress(result.User.Email);
                MailMessage message = new MailMessage(from, to)
                {
                    Subject = "Восстановление пароля",
                    Body = $"Здравствуйте, {result.User.Name} {result.User.SecondName}!\n" +
                        $"Подтвердите ваш аккаунт в Donor+\n" +
                        $"Код для потверждения: {Storage.ConfirmCode}.\n" +
                        $"Если это не Вы, напишите в нашу поддержку '{Storage.Email}'\n\n" +
                        $"С уважением,\nКоманда Donor+"
                };
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential(Storage.Email, Storage.MailPasswd),
                    EnableSsl = true
                };
                await smtp.SendMailAsync(message);
            }
            else
            {
                return false;
            }
            return true;
        }
    }
}