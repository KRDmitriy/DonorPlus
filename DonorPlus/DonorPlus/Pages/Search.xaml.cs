using DonorPlus.Effects;
using DonorPlus.Models;
using DonorPlusLib;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DonorPlus
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Search : ContentPage
    {
        public static List<Client> clients;
        public ObservableCollection<UserModel> UsersCollection { get; set; } = new ObservableCollection<UserModel>();

        public Search()
        {
            InitializeComponent();

            SetColors();

            ResultList.ItemTemplate = new DataTemplate(typeof(UserCell));

            App.DataAlreadyLoaded += GetData;
            AppearenceSettings.CheckAndSetColors += SetColors;
        }

        public void SetUsers()
        {
            UsersCollection.Clear();
            foreach (Client client in clients)
            {
                if (string.IsNullOrEmpty(client.Name))
                {
                    UsersCollection.Add(new UserModel
                    {
                        ModeID = client.Id,
                        ImageSource = "Resources/hospital.png",
                        Surname = client.Surname,
                        Name = "",
                        BloodGroup = "",
                        RFactor = ""
                    });
                }
                else
                {
                    UsersCollection.Add(new UserModel
                    {
                        ModeID = client.Id,
                        ImageSource = "Resources/user.png",
                        Surname = client.Surname,
                        Name = client.Name,
                        BloodGroup = client.BloodGroup,
                        RFactor = client.RFactor
                    });
                }
            }
            ResultList.ItemsSource = UsersCollection;
        }

        public void SetColors()
        {
            BackgroundColor = Storage.BackColor;

            SearchLabel.TextColor = Storage.TextColor;
            SearchEntry.PlaceholderColor = Color.LightGray;
            SearchEntry.TextColor = Storage.TextColor;

            TintImageEffect effect = new TintImageEffect { TintColor = Storage.TextColor };

            SearchImage.Effects.Clear();
            SearchImage.Effects.Add(effect);

            effect = new TintImageEffect { TintColor = Storage.TextColor };
            ClearImage.Effects.Clear();
            ClearImage.Effects.Add(effect);
        }

        private void SearchUsers(object sender, System.EventArgs e)
        {
            string text = SearchEntry.Text.Trim();
            if (!string.IsNullOrEmpty(text))
            {
                UsersCollection.Clear();

                string[] data = text.Split();


                foreach (Client client in clients)
                {
                    foreach (string str in data)
                    {
                        if ((str != null) && (client.ToString().ToLower().IndexOf(str.ToLower()) != -1))
                        {
                            if (string.IsNullOrEmpty(client.Name))
                            {
                                UsersCollection.Add(new UserModel
                                {
                                    ModeID = client.Id,
                                    ImageSource = "Resources/hospital.png",
                                    Surname = client.Surname,
                                    Name = client.Name
                                });
                            }
                            else
                            {
                                UsersCollection.Add(new UserModel
                                {
                                    ModeID = client.Id,
                                    ImageSource = "Resources/user.png",
                                    Surname = client.Surname,
                                    Name = client.Name
                                });
                            }

                            break;
                        }
                    }
                }

                if (UsersCollection.Count == 0)
                {
                    UsersCollection.Add(new UserModel
                    {
                        ModeID = -1,
                        Surname = "Нет результата",
                        Name = "Попробуйте ещё раз"
                    });
                }
            }
            else
            {
                UsersCollection.Clear();
                SetUsers();
            }
            ResultList.ItemsSource = UsersCollection;
        }

        private void Clear_Tapped(object sender, System.EventArgs e)
        {
            SearchEntry.Text = string.Empty;
        }

        private async void ResultList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                ResultList.IsEnabled = false;
                if (e.Item != null)
                {
                    UserModel userView = (UserModel)e.Item;
                    int id = userView.ModeID;
                    if (id != -1)
                    {
                        if (id != Storage.User.Id)
                        {
                            Storage.Friend = Users.GetInfoAboutUser(id).User;
                            Storage.Friend.AddPhoto(Photo.Get(id).Image);
                            ResultObj result = BloodData.Get(id);
                            Storage.Friend.AddBloodGroup(result.BloodGroup);
                            Storage.Friend.AddRFactor(result.RFactor);
                            Storage.IsUserNow = false;
                        }
                        else
                        {
                            Storage.IsUserNow = true;
                        }
                        await Navigation.PushAsync(new Profile());
                    }
                }
                ((ListView)sender).SelectedItem = null;
                ResultList.IsEnabled = true;
                Storage.Friend = null;
                System.GC.Collect();
            }
            catch (System.Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        public async Task<bool> GetData()
        {
            ResultList.IsVisible = true;
            SearchEntry.IsEnabled = true;
            clients = await Task.Run(() => Users.GetAllUsers());
            SetUsers();
            ResultList.IsRefreshing = false;
            return true;
        }

        private async void ResultList_Refreshing(object sender, System.EventArgs e)
        {
            ResultList.IsVisible = true;
            SearchEntry.IsEnabled = true;
            clients = await Task.Run(() => Users.GetAllUsers());
            SetUsers();
            ResultList.IsRefreshing = false;
        }
    }
}