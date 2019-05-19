using Xamarin.Forms;

namespace DonorPlus.Renderers
{
    internal class NavigationPageRenderer : NavigationPage
    {
        public NavigationPageRenderer() : base()
        {
            BackgroundColor = Storage.HelpColor;
            BarBackgroundColor = Storage.HelpColor;
        }

        public NavigationPageRenderer(Page root) : base(root)
        {
            BackgroundColor = Storage.HelpColor;
            BarBackgroundColor = Storage.HelpColor;
        }
    }
}
