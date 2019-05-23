using DonorPlus.Models;
using DonorPlus.Views;
using DonorPlusLib;
using Plugin.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DonorPlus
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Profile : ContentPage
    {
        private List<ProfileFrameModel> frames = new List<ProfileFrameModel>();

        public delegate void FriendAdded();
        public static event FriendAdded FriendAddedEvent;

        private Client client;

        public Profile()
        {
            InitializeComponent();

            MainGrid.IsVisible = false;

            SetColors();

            Task.Run(() => Waiter());
        }

        public void ChangeProfile(object sender, EventArgs e)
        {
            EditImage.IsEnabled = false;
            Navigation.PushModalAsync(new RegistrationPage(true));
            EditImage.IsEnabled = true;
        }

        public async void AddToFriends(object sender, EventArgs e)
        {
            try
            {
                Contacts.Add(Storage.User.Id, Storage.Friend.Id);
                PersonalInfo.IsVisible = true;
                FriendAddedEvent?.Invoke();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", ex.Message, "OK");
            }
        }

        private async void Call_Tapped(object sender, EventArgs e)
        {
            try
            {
                IPhoneCallTask phoneDialer = CrossMessaging.Current.PhoneDialer;
                if (phoneDialer.CanMakePhoneCall)
                {
                    phoneDialer.MakePhoneCall(client.Phone);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", ex.Message, "OK");
            }
        }

        private async void SendEmail_Tapped(object sender, EventArgs e)
        {
            try
            {
                IEmailTask emailMessenger = CrossMessaging.Current.EmailMessenger;
                if (emailMessenger.CanSendEmail)
                {
                    emailMessenger.SendEmail(client.Email, "", "");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", ex.Message, "OK");
            }
        }

        public async void GoToDialog(object sender, EventArgs e)
        {
            DialogButton.IsEnabled = false;
            if (!Contacts.CheckContact(Storage.User.Id, Storage.Friend.Id))
            {
                await DisplayAlert("Отказано",
                   "Вы не можете отправлять письма этому пользователю. " +
                   "Возможно, следует добавить его в друзья", "ОК");
            }
            else
            {
                await Navigation.PushModalAsync(new ChatPage());
            }
            DialogButton.IsEnabled = true;
        }

        public void SetColors()
        {
            BackgroundColor = Storage.BackColor;
            PhotoEffect.TintColor = Storage.TextColor;
            SpecialPhotoEffect.TintColor = Storage.TextColor;
            EditImageEffect.TintColor = Storage.TextColor;
            SettingsImageEffect.TintColor = Storage.TextColor;
            DialogButton.TextColor = Storage.TextColor;
            DialogButton.BackgroundColor = Storage.BackColor;
            AddToFriendsButton.TextColor = Storage.TextColor;
            AddToFriendsButton.BackgroundColor = Storage.BackColor;
            MyRequestsButton.TextColor = Storage.TextColor;
            MyRequestsButton.BackgroundColor = Storage.BackColor;
            MyHistoryButton.TextColor = Storage.TextColor;
            MyHistoryButton.BackgroundColor = Storage.BackColor;

            NameLabel.TextColor = Storage.TextColor;
            BloodGroupLabel.TextColor = Storage.HelpColor;
            BloodGroupDataLabel.TextColor = Storage.TextColor;
            RFactorLabel.TextColor = Storage.HelpColor;
            RFactorDataLabel.TextColor = Storage.TextColor;
            EmailLabel.TextColor = Storage.HelpColor;
            EmailDataLabel.TextColor = Storage.TextColor;
            PhoneLabel.TextColor = Storage.HelpColor;
            PhoneDataLabel.TextColor = Storage.TextColor;
        }

        private void SettingsImage_Tapped(object sender, EventArgs e)
        {
            SettingsImage.IsEnabled = false;
            Navigation.PushModalAsync(new SelfSettings());
            SettingsImage.IsEnabled = true;
        }

        public async void Waiter()
        {
            while (true)
            {
                if (Storage.DataLoaded)
                {
                    break;
                }
            }
            await SetData();
        }

        public async Task<bool> SetData()
        {
            IsBusy = true;
            if (Storage.IsUserNow)
            {
                client = Storage.User;
            }
            else
            {
                client = Storage.Friend;
            }

            NameLabel.Text = $"{client.Surname} {client.Name}";
            BloodGroupDataLabel.Text = string.IsNullOrWhiteSpace(client.BloodGroup) ?
                "Нет данных" : $"{client.BloodGroup}";
            RFactorDataLabel.Text = string.IsNullOrWhiteSpace(client.RFactor) ?
                "Нет данных" : $"{client.RFactor}";
            EmailDataLabel.Text = $"{client.Email}";
            PhoneDataLabel.Text = $"{client.Phone}";

            if (client.Photo != null)
            {
                Photo.Source = ImageSource.FromStream(() => new MemoryStream(client.Photo));
                Photo.Effects.Clear();
            }

            if (client.Id == 1)
            {
                SpecialImage.Source = "Resources/checked.png";
            }
            else
            {
                SpecialImage.Source = "Resources/round.png";
            }


            if (Storage.IsUserNow)
            {
                OwnPage.IsVisible = true;
                MyPageExtra.IsVisible = true;
                PersonalInfo.IsVisible = true;
                OtherPage.IsVisible = false;
            }
            else
            {
                OwnPage.IsVisible = false;
                MyPageExtra.IsVisible = false;
                PersonalInfo.IsVisible = await Task.Run(() => Contacts.CheckContact(Storage.User.Id, Storage.Friend.Id));
                OtherPage.IsVisible = true;
            }

            IsBusy = false;
            BloodInfo.IsVisible = true;
            MainGrid.IsVisible = true;
            return true;
        }

        private async void MyRequestsButton_Clicked(object sender, EventArgs e)
        {
            MyRequestsButton.IsEnabled = false;
            await Navigation.PushModalAsync(new Notifications(true));
            MyRequestsButton.IsEnabled = true;
        }

        private async void MyHistoryButton_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Внимание!", "В данный момент журнал не доступен!", "OK");
        }
    }
}