using DonorPlusLib;
using Plugin.Settings;
using Xamarin.Forms;

namespace DonorPlus
{
    public static class Storage
    {
        #region Colors
        public static Color BackColor { get; set; } = Color.White;

        public static Color HelpColor { get; set; } = Color.FromRgb(0, 150, 255);

        public static Color TextColor { get; set; } = Color.Black;

        public static Color SpecialRedColor { get; set; } = Color.FromRgb(200, 0, 0);
        #endregion

        #region Settings
        public static bool IsDarkTheme { get; set; } = false;

        public static bool IsUserNow { get; set; } = true;

        public static bool IsMailsAvaliable { get; set; }

        public static bool IsChatNow { get; set; } = false;

        public static bool IsChatBot { get; set; } = false;

        public static bool DataLoaded { get; set; } = false;

        public static bool IsEntering { get; set; } = false;
        #endregion

        public static string ConfirmCode { get; set; } = "";

        public static Client User { get; set; }

        public static Client Friend { get; set; }

        public static Client temporaryData { get; set; }

        public static string Email = "donorplus.help@gmail.com";

        public static string MailPasswd = "sDonor!12";

        public static void SetDarkTheme()
        {
            IsDarkTheme = true;
            BackColor = Color.Black;
            TextColor = Color.White;
            HelpColor = Color.FromRgb(0, 150, 255); //Color.Orange;
            CrossSettings.Current.AddOrUpdateValue("IsDarkTheme", true);
        }

        public static void SetLightTheme()
        {
            IsDarkTheme = false;
            BackColor = Color.White;
            TextColor = Color.Black;
            HelpColor = Color.FromRgb(0, 150, 255); //Color.FromRgb(0, 0, 200);
            CrossSettings.Current.AddOrUpdateValue("IsDarkTheme", false);
        }
    }
}
