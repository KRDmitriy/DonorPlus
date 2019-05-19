using Xamarin.Forms;

namespace DonorPlus.Renderers
{
    public class ActivityIndicatorRenderer : ActivityIndicator
    {
        public ActivityIndicatorRenderer() : base()
        {
            HorizontalOptions = VerticalOptions = LayoutOptions.FillAndExpand;

            SetColors();
            HeightRequest = WidthRequest = 40;

            AppearenceSettings.CheckAndSetColors += SetColors;
        }

        public void Start()
        {
            IsRunning = true;
            IsVisible = true;
            IsEnabled = true;
        }

        public void Stop()
        {
            IsRunning = false;
            IsVisible = false;
            IsEnabled = false;
        }

        public void SetColors()
        {
            Color = Storage.HelpColor;
            BackgroundColor = Storage.BackColor;  
        }
    }
}
