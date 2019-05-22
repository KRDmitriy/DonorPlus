using DonorPlus.Effects;
using DonorPlusLib;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DonorPlus
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Notifications : ContentPage
    {
        public static List<Request> ActualRequests { get; set; } = new List<Request>();
        public ObservableCollection<Models.RequestModel> RequiredResults { get; set; }
            = new ObservableCollection<Models.RequestModel>();
        public bool IsMy { get; set; }

        public Notifications()
        {
            InitializeComponent();

            NotifList.ItemTemplate = new DataTemplate(typeof(Models.RequestCell));

            SetColors();
            App.DataAlreadyLoaded += GetData;
            Models.RequestCell.ShowAuthorPage += ShowAuthor;
            AppearenceSettings.CheckAndSetColors += SetColors;
        }

        public Notifications(bool isMy)
        {
            InitializeComponent();

            NotifList.ItemTemplate = new DataTemplate(typeof(Models.RequestCell));

            IsMy = isMy;
            NotifLabel.Text = "Мои запросы";
            NotifList_Refreshing(this, System.EventArgs.Empty);

            SetColors();
            App.DataAlreadyLoaded += GetData;
            Models.RequestCell.ShowAuthorPage += ShowAuthor;
            AppearenceSettings.CheckAndSetColors += SetColors;
        }

        public void SetColors()
        {
            BackgroundColor = Storage.BackColor;

            SearchEntry.PlaceholderColor = Color.LightGray;
            SearchEntry.TextColor = Storage.TextColor;

            TintImageEffect effect = new TintImageEffect { TintColor = Storage.TextColor };

            SearchImage.Effects.Clear();
            SearchImage.Effects.Add(effect);

            effect = new TintImageEffect { TintColor = Storage.TextColor };
            ClearImage.Effects.Clear();
            ClearImage.Effects.Add(effect);
            NotifLabel.TextColor = Storage.TextColor;
        }

        public void SetRequests()
        {
            RequiredResults.Clear();
            foreach (Request request in ActualRequests)
            {
                if (IsMy)
                {
                    if (request.AuthorID == Storage.User.Id)
                    {
                        Client client = Users.GetInfoAboutUser(request.AuthorID).User;
                        RequiredResults.Add(new Models.RequestModel()
                        {
                            AuthorID = request.AuthorID.ToString(),
                            AuthorName = client.Surname + " " + client.Name,
                            BloodGroup = request.BloodGroup,
                            RFactor = request.RFactor,
                            ExtraBloodData = request.ExtraBloodData,
                            Region = request.Region,
                            Description = request.Descripton
                        });
                    }
                }
                else
                //If we have no data about user's blood we need to show to him/her all the results
                if ((request.BloodGroup == Storage.User.BloodGroup || Storage.User.BloodGroup == "") &&
                    (request.RFactor == Storage.User.RFactor || Storage.User.RFactor == "") &&
                     request.Solved == false)
                {
                    Client client = Users.GetInfoAboutUser(request.AuthorID).User;
                    RequiredResults.Add(new Models.RequestModel()
                    {
                        AuthorID = request.AuthorID.ToString(),
                        AuthorName = client.Surname + " " + client.Name,
                        BloodGroup = request.BloodGroup,
                        RFactor = request.RFactor,
                        ExtraBloodData = request.ExtraBloodData,
                        Region = request.Region,
                        Description = request.Descripton
                    });
                }
            }

            if (RequiredResults.Count == 0)
            {
                RequiredResults.Add(new Models.RequestModel()
                {
                    AuthorID = "-1",
                    AuthorName = "Нет данных",
                    BloodGroup = "Нет данных",
                    RFactor = "Нет данных",
                    ExtraBloodData = "Нет данных",
                    Description = "Поиск не дал результатов"
                });
            }

            NotifList.ItemsSource = RequiredResults;
        }

        private void SearchRequests(object sender, System.EventArgs e)
        {
            string text = SearchEntry.Text;
            if (!string.IsNullOrEmpty(text))
            {
                RequiredResults.Clear();

                string[] data = text.Split();

                foreach (Request request in ActualRequests)
                {
                    foreach (string str in data)
                    {
                        if ((str != null) && (request.ToString().ToLower().IndexOf(str.ToLower()) != -1))
                        {
                            if (IsMy)
                            {
                                if (request.AuthorID == Storage.User.Id)
                                {
                                    Client client = Users.GetInfoAboutUser(request.AuthorID).User;
                                    RequiredResults.Add(new Models.RequestModel()
                                    {
                                        AuthorID = request.AuthorID.ToString(),
                                        AuthorName = client.Surname + " " + client.Name,
                                        BloodGroup = request.BloodGroup,
                                        RFactor = request.RFactor,
                                        ExtraBloodData = request.ExtraBloodData,
                                        Region = request.Region,
                                        Description = request.Descripton
                                    });
                                }
                            }
                            else
                            {
                                if ((request.BloodGroup == Storage.User.BloodGroup || Storage.User.BloodGroup == "") &&
                                (request.RFactor == Storage.User.RFactor || Storage.User.RFactor == "") &&
                                request.Solved == false)
                                {
                                    Client client = Users.GetInfoAboutUser(request.AuthorID).User;
                                    RequiredResults.Add(new Models.RequestModel()
                                    {
                                        AuthorID = request.AuthorID.ToString(),
                                        AuthorName = client.Surname + " " + client.Name,
                                        BloodGroup = request.BloodGroup,
                                        RFactor = request.RFactor,
                                        ExtraBloodData = request.ExtraBloodData,
                                        Region = request.Region,
                                        Description = request.Descripton
                                    });
                                }
                            }
                        }
                        break;
                    }
                }
            }
            else
            {
                RequiredResults.Clear();
                SetRequests();
            }

            if (RequiredResults.Count == 0)
            {
                RequiredResults.Add(new Models.RequestModel()
                {
                    AuthorID = "-1",
                    AuthorName = "Нет данных",
                    BloodGroup = "Нет данных",
                    RFactor = "Нет данных",
                    ExtraBloodData = "Нет данных",
                    Description = "Поиск не дал результатов"
                });
            }

            NotifList.ItemsSource = RequiredResults;
        }

        private void Clear_Tapped(object sender, System.EventArgs e)
        {
            SearchEntry.Text = string.Empty;
        }

        public async Task<bool> GetData()
        {
            NotifList.IsVisible = true;
            SearchEntry.IsEnabled = true;
            ActualRequests = await Task.Run(() => Requests.GetAll().Requests);
            SetRequests();
            NotifList.IsRefreshing = false;
            return true;
        }

        private async void NotifList_Refreshing(object sender, System.EventArgs e)
        {
            NotifList.IsVisible = true;
            SearchEntry.IsEnabled = true;
            ActualRequests = await Task.Run(() => Requests.GetAll().Requests);
            SetRequests();
            NotifList.IsRefreshing = false;
        }

        public async void ShowAuthor(int id)
        {
            try
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
                await Navigation.PushModalAsync(new Profile());
                Storage.Friend = null;
                System.GC.Collect();
            }
            catch (System.Exception ex)
            {
                await DisplayAlert("Ошибка", ex.Message, "OK");
            }
        }

        private void NotifList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
    }
}