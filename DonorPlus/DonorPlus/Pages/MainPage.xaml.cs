using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace DonorPlus
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : Xamarin.Forms.TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
            On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);

            SetColors();
            AppearenceSettings.CheckAndSetColors += SetColors;
        }

        public void SetColors()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            On<Android>().SetBarItemColor(Storage.HelpColor);
#pragma warning restore CS0618 // Type or member is obsolete
#pragma warning disable CS0618 // Type or member is obsolete
            On<Android>().SetBarSelectedItemColor(Storage.SpecialRedColor);
#pragma warning restore CS0618 // Type or member is obsolete

            BarBackgroundColor = Storage.BackColor;
            BackgroundColor = Storage.BackColor;
        }

    }
}
