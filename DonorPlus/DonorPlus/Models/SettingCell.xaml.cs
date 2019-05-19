using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DonorPlus.Models
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingCell : ViewCell
	{
		public SettingCell ()
		{
			InitializeComponent ();

            SetColors();
            AppearenceSettings.CheckAndSetColors += SetColors;
		}

        public void SetColors()
        {
            TextLabel.TextColor = Storage.TextColor;
            PhotoEffect.TintColor = Storage.TextColor;
        }
	}
}