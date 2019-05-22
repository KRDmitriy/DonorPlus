using DonorPlus.Models;
using DonorPlus.Views;
using DonorPlusLib;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DonorPlus
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dialogs : ContentPage
    {
        public ObservableCollection<UserModel> DialogsCollection { get; set; } = new ObservableCollection<UserModel>();

        private List<Client> users = new List<Client>();
        public List<int> contacts;

        public Dialogs()
        {
            InitializeComponent();

            DialogList.ItemTemplate = new DataTemplate(typeof(UserCell));

            SetColors();
            DialogList_Refreshing(this, new System.EventArgs());
            App.DataAlreadyLoaded += GetData;
            Profile.FriendAddedEvent += SetDialogs;
            AppearenceSettings.CheckAndSetColors += SetColors;
        }

        public void SetColors()
        {
            BackgroundColor = Storage.BackColor;

            DialogLabel.TextColor = Storage.TextColor;
        }

        public void SetDialogs()
        {
            DialogsCollection.Clear();
            users.Clear();
            if (contacts != null)
            {
                foreach (int contact in contacts)
                {
                    users.Add(Users.GetInfoAboutUser(contact).User);
                }

                int k = 0;

                foreach (Client user in users)
                {
                    DialogsCollection.Add(
                        new UserModel
                        {
                            ModeID = k,
                            ImageSource = "Resources/message.png",
                            Surname = user.Surname,
                            Name = user.Name
                        });
                    ++k;
                }
            }
            DialogsCollection.Add(
                new UserModel
                {
                    ModeID = -1,
                    ImageSource = "Resources/message.png",
                    Surname = "Бот",
                    Name = "для активных диалогов"
                });
            DialogList.ItemsSource = DialogsCollection;
        }

        private async void DialogList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            DialogList.IsEnabled = false;
            if (e.Item != null)
            {
                UserModel userView = (UserModel)e.Item;
                int id = userView.ModeID;
                if (id != -1)
                {
                    Storage.Friend = users[id];
                    await Navigation.PushModalAsync(new ChatPage());
                }
                else
                {
                    Storage.IsChatBot = true;
                    await Navigation.PushModalAsync(new ChatPage());
                }
            }
            ((ListView)sender).SelectedItem = null;
            DialogList.IsEnabled = true;
        }

        public async Task<bool> GetData()
        {
            ResultObj result = await Task.Run(() => Contacts.GetContacts(Storage.User.Id));
            contacts = result.Contacts;
            SetDialogs();
            DialogList.IsVisible = true;
            return true;
        }

        private async void DialogList_Refreshing(object sender, System.EventArgs e)
        {
            if (Storage.DataLoaded)
            {
                ResultObj result = await Task.Run(() => Contacts.GetContacts(Storage.User.Id));
                contacts = result.Contacts;
                SetDialogs();
                DialogList.IsVisible = true;
            }
            DialogList.IsRefreshing = false;
        }
    }
}