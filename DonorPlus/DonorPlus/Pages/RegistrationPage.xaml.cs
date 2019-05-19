using DonorPlusLib;
using Plugin.Connectivity;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DonorPlus
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {
        public byte[] imageData = null;
        public bool IsChange;

        public RegistrationPage(bool isChange)
        {
            InitializeComponent();

            SetColors();

            AppearenceSettings.CheckAndSetColors += SetColors;

            IsChange = isChange;
            ChangeLayout.IsVisible = IsChange;
            RegLayout.IsVisible = !IsChange;

            if (IsChange)
            {
                RegLabel.Text = "Мои данные";
                SurnameEntry.Text = Storage.User.Surname;
                NameEntry.Text = Storage.User.Name;
                SecondNameEntry.Text = Storage.User.SecondName;
                MailEntry.Text = Storage.User.Email;
                PhoneEntry.Text = Storage.User.Phone;
                PhotoButton.Text = "Изменить фото";
                if (Storage.User.BloodGroup != "")
                    BloodGroupEntry.SelectedItem = Storage.User.BloodGroup;
                else
                    BloodGroupEntry.SelectedItem = "Нет данных";
                if (Storage.User.RFactor != "")
                    RFactorEntry.SelectedItem = Storage.User.RFactor;
                else
                    RFactorEntry.SelectedItem = "Нет данных";
                if (Storage.User.Photo != null)
                {
                    imageData = Storage.User.Photo;
                    Photo.Source = ImageSource.FromStream(() => new MemoryStream(Storage.User.Photo));
                    Photo.Effects.Clear();
                }
            }
            else
            {
                BloodGroupEntry.SelectedItem = "Нет данных";
                RFactorEntry.SelectedItem = "Нет данных";
            }
        }

        public void SetColors()
        {
            BackgroundColor = Storage.BackColor;

            RegLabel.TextColor = Storage.TextColor;

            SurnameEntry.BackgroundColor = NameEntry.BackgroundColor =
                SecondNameEntry.BackgroundColor = MailEntry.BackgroundColor =
                PhoneEntry.BackgroundColor = PasswordEntry.BackgroundColor =
                PasswordRepeatEntry.BackgroundColor = BloodGroupEntry.BackgroundColor =
                RFactorEntry.BackgroundColor = Storage.BackColor;

            SurnameEntry.PlaceholderColor = NameEntry.PlaceholderColor =
                SecondNameEntry.PlaceholderColor = MailEntry.PlaceholderColor =
                PhoneEntry.PlaceholderColor = PasswordEntry.PlaceholderColor =
                PasswordRepeatEntry.PlaceholderColor = Color.LightGray;

            SurnameLabelStack.BackgroundColor = NameLabelStack.BackgroundColor =
                SecondNameLabelStack.BackgroundColor = MailLabelStack.BackgroundColor =
                PhoneLabelStack.BackgroundColor = PasswordLabelStack.BackgroundColor =
                PasswordRepeatLabelStack.BackgroundColor = BloodGroupLabelStack.BackgroundColor =
                RFactorLabelStack.BackgroundColor = Storage.HelpColor;

            SurnameEntryStack.BackgroundColor = NameEntryStack.BackgroundColor =
                SecondNameEntryStack.BackgroundColor = MailEntryStack.BackgroundColor =
                PhoneEntryStack.BackgroundColor = PasswordEntryStack.BackgroundColor =
                PasswordRepeatEntryStack.BackgroundColor = BloodGroupEntryStack.BackgroundColor =
                RFactorEntryStack.BackgroundColor = Storage.BackColor;

            SurnameEntry.TextColor = NameEntry.TextColor =
                SecondNameEntry.TextColor = MailEntry.TextColor =
                PhoneEntry.TextColor = PasswordEntry.TextColor =
                PasswordRepeatEntry.TextColor = BloodGroupEntry.TextColor =
                RFactorEntry.TextColor = Storage.TextColor;

            SurnameLabel.TextColor = NameLabel.TextColor =
                SecondNameLabel.TextColor = MailLabel.TextColor =
                PhoneLabel.TextColor = PasswordLabel.TextColor =
                PasswordRepeatLabel.TextColor = BloodGroupLabel.TextColor =
                RFactorLabel.TextColor = Color.White;// Storage.TextColor;

            SaveButton.TextColor = CancelButton.TextColor =
                RegButton.TextColor = PhotoButton.TextColor = Storage.TextColor;

            PhotoButton.BackgroundColor = RegButton.BackgroundColor = Storage.BackColor;

            Photo.BorderColor = Color.FromRgb(200, 0, 0);

            PhotoEffect.TintColor = Storage.TextColor;

            SaveButton.BackgroundColor = Storage.HelpColor;
            CancelButton.BackgroundColor = Storage.BackColor;
            DeletePhotoButton.BackgroundColor = Storage.HelpColor;
            DeletePhotoButton.TextColor = SaveButton.TextColor = Color.White;
        }

        public async void CancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        public async void SaveButton_Clicked(object sender, EventArgs e)
        {
            if (CheckData())
            {
                Client client = CreateUser();
                string str = Registration.Change(client);
                string mess = BloodData.Push(client.Id, client.BloodGroup, client.RFactor);
                if (str == "OK" && mess == "OK")
                {
                    Storage.User = client;
                    await Navigation.PopModalAsync();
                }
                else
                {
                    await DisplayAlert("Ошибка", "Некорректные данные или проблемы с интернетом", "OK");
                }
            }
        }

        private async void PhotoButton_Clicked(object sender, EventArgs e)
        {
            PermissionStatus status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            if (status == PermissionStatus.Granted)
            {
                MediaFile file = null;
                if (CrossMedia.Current.IsPickPhotoSupported)
                {
                    file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                    {
                        PhotoSize = PhotoSize.Medium
                    });
                }

                if (file == null)
                {
                    return;
                }

                string filePath = file.Path;
                Queue<string> paths = new Queue<string>();
                paths.Enqueue(filePath);

                Photo.Effects.Clear();
                Photo.Source = ImageSource.FromStream(() => file.GetStream());
                
                imageData = new BinaryReader
                    (file.GetStream()).ReadBytes((int)(new FileInfo(filePath).Length));
            }
            else
            {
                await DisplayAlert("Нет доступа", "Вы не дали доступ к файлам", "OK");
            }
        }

        private void DeletePhotoButton_Clicked(object sender, EventArgs e)
        {
            imageData = null;
            Photo.Source = "Resources/emptyPhoto.png";
            Photo.Effects.Add(new Effects.TintImageEffect { TintColor = Storage.TextColor });
        }

        private bool CheckData()
        {
            if (string.IsNullOrEmpty(SurnameEntry.Text))
            {
                SurnameEntry.PlaceholderColor = Color.Red;
                return false;
            }
            if (string.IsNullOrEmpty(NameEntry.Text))
            {
                NameEntry.PlaceholderColor = Color.Red;
                return false;
            }
            if (string.IsNullOrEmpty(SecondNameEntry.Text))
            {
                SecondNameEntry.PlaceholderColor = Color.Red;
                return false;
            }
            if (string.IsNullOrEmpty(MailEntry.Text))
            {
                MailEntry.PlaceholderColor = Color.Red;
                return false;
            }
            if (string.IsNullOrEmpty(PhoneEntry.Text) ||
                !(PhoneEntry.Text.ToString()[0] == '+' || PhoneEntry.Text.ToString()[0] == '8'))
            {
                PhoneEntry.PlaceholderColor = Color.Red;
                PhoneEntry.TextColor = Color.Red;
                return false;
            }
            else
            {
                PhoneEntry.TextColor = Storage.TextColor;
            }
            if (string.IsNullOrEmpty(PasswordEntry.Text))
            {
                PasswordEntry.PlaceholderColor = Color.Red;
                return false;
            }
            if (PasswordEntry.Text != PasswordRepeatEntry.Text)
            {
                PasswordEntry.TextColor = PasswordRepeatEntry.TextColor = Color.Red;
                return false;
            }
            else
            {
                PasswordEntry.TextColor = PasswordRepeatEntry.TextColor = Storage.TextColor;
            }
            return true;
        }

        private Client CreateUser()
        {
            if (IsChange)
            {
                Client oldClient = new Client(
                    Storage.User.Id,
                    SurnameEntry.Text.Trim(),
                    NameEntry.Text.Trim(),
                    SecondNameEntry.Text.Trim(),
                    MailEntry.Text.Trim(),
                    PhoneEntry.Text.Trim(),
                    PasswordEntry.Text,
                    imageData);
                if (BloodGroupEntry.SelectedItem.ToString() != "Нет данных" &&
                !string.IsNullOrWhiteSpace(BloodGroupEntry.SelectedItem.ToString()))
                    oldClient.AddBloodGroup(BloodGroupEntry.SelectedItem.ToString());
                if (RFactorEntry.SelectedItem.ToString() != "Нет данных" &&
                    !string.IsNullOrWhiteSpace(RFactorEntry.SelectedItem.ToString()))
                    oldClient.AddRFactor(RFactorEntry.SelectedItem.ToString());
                return oldClient;
            }

            Client newClient = new Client(
                   0,
                   SurnameEntry.Text.Trim(),
                   NameEntry.Text.Trim(),
                   SecondNameEntry.Text.Trim(),
                   MailEntry.Text.Trim(),
                   PhoneEntry.Text.Trim(),
                   PasswordEntry.Text,
                   imageData);
            if (BloodGroupEntry.SelectedItem.ToString() != "Нет данных" &&
                !string.IsNullOrWhiteSpace(BloodGroupEntry.SelectedItem.ToString()))
                newClient.AddBloodGroup("");
            else
                newClient.AddBloodGroup(BloodGroupEntry.SelectedItem.ToString());
            if (RFactorEntry.SelectedItem.ToString() != "Нет данных" &&
                !string.IsNullOrWhiteSpace(RFactorEntry.SelectedItem.ToString()))
                newClient.AddRFactor("");
            else
                newClient.AddRFactor(RFactorEntry.SelectedItem.ToString());
            return newClient;
        }

        public static void GenerateCode()
        {
            Random random = new Random();
            string data = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890";
            string code = "";
            for (int i = 0; i < 6; i++)
            {
                code += data[random.Next(data.Length)];
            }
            Storage.ConfirmCode = code;
        }

        public static async Task SendEmailAsync(string email)
        {
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(Storage.Email, Storage.MailPasswd),
                EnableSsl = true
            };
            MailAddress from = new MailAddress(Storage.Email, "DonorPlus");
            MailAddress to = new MailAddress(email);
            MailMessage message = new MailMessage(from, to)
            {
                Subject = "Подтверждение почты",
                Body = $"Здравствуйте {Storage.temporaryData.Name} {Storage.temporaryData.SecondName}!\n" +
                    $"Подтвердите ваш аккаунт в Donor+\n" +
                    $"Код для потверждения: {Storage.ConfirmCode}.\n" +
                    $"Если это не Вы, напишите в нашу поддержку '{Storage.Email}'\n\n" +
                    $"С уважением,\nКоманда Donor+"
            };
            await smtp.SendMailAsync(message);
        }

        public async void SendCode()
        {
            GenerateCode();
            if (CrossConnectivity.Current.IsConnected)
            {
                SendEmailAsync(Storage.temporaryData.Email).GetAwaiter();
            }
            else
            {
                await DisplayAlert("Интернет отсутствует", "Подключите интернет", "ОК");
            }
        }

        private async void RegButton_Clicked(object sender, EventArgs e)
        {
            RegButton.IsEnabled = false;
            if (CheckData())
            {
                Storage.temporaryData = CreateUser();

                SendCode();
                await Navigation.PushAsync(new ConfirmPage());
            }
            RegButton.IsEnabled = true;
        }
    }
}