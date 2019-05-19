using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DonorPlus.Models
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DialogCell : ViewCell
    {
        public DialogCell()
        {
            InitializeComponent();

            SetColors();

            AppearenceSettings.CheckAndSetColors += SetColors;
        }

        public void SetColors()
        {
            InfoStack.BackgroundColor = SurnameStack.BackgroundColor = Storage.HelpColor;
            IDStack.BackgroundColor = NameStack.BackgroundColor = MainFrame.BorderColor = Storage.BackColor;
            InfoLabel.TextColor = SurnameLabel.TextColor =
                IDLabel.TextColor = NameLabel.TextColor = Storage.TextColor;
            MainFrame.HasShadow = !Storage.IsDarkTheme;
        }
    }
}