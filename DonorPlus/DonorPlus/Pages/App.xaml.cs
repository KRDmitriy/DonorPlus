using DonorPlusLib;
using Plugin.Connectivity;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Plugin.Settings;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace DonorPlus
{
    public delegate Task<bool> DataLoader();

    public partial class App : Application
    {
        public static event DataLoader DataAlreadyLoaded;

        public App()
        {
            InitializeComponent();

            CheckPermissions();

            if (CrossSettings.Current.GetValueOrDefault("IsDarkTheme", false))
            {
                Storage.SetDarkTheme();
            }
            else
            {
                Storage.SetLightTheme();
            }

            Storage.IsMailsAvaliable = CrossSettings.Current.GetValueOrDefault("IsMailsAvaliable", false);

            if (Current.Properties.TryGetValue("user", out object id) && CrossConnectivity.Current.IsConnected)
            {
                MainPage = new NavigationPage(new MainPage());
                LogIn(false);
            }
            else
            {
                MainPage = new NavigationPage(new EnterPage());
            }
        }

        public static async void LogIn(bool IsEnter)
        {
            if (IsEnter)
            {
                DataAlreadyLoaded?.Invoke();
                Storage.DataLoaded = true;
            }
            else
            {
                if (Current.Properties.TryGetValue("user", out object id))
                {
                    try
                    {
                        ResultObj result = await Task.Run(() => Users.GetInfoAboutUser((int)id));
                        Storage.User = result.User;
                        Storage.User.AddPhoto(Photo.Get(Storage.User.Id).Image);
                        ResultObj blood = BloodData.Get(result.User.Id);
                        Storage.User.AddBloodGroup(blood.BloodGroup);
                        Storage.User.AddRFactor(blood.RFactor);
                        DataAlreadyLoaded?.Invoke();
                        Storage.DataLoaded = true;
                    }
                    catch (System.Exception ex)
                    {
                        string str = ex.Message;
                        Current.MainPage = new NavigationPage(new EnterPage());
                    }
                }
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public static async void CheckPermissions()
        {
            PermissionStatus status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            if (status != PermissionStatus.Granted)
            {
                await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Storage, });
            }

            status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            if (status != PermissionStatus.Granted)
            {
                await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, });
            }
        }
    }
}
