using DonorPlus.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DonorPlus.Models
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserCell : ViewCell
    {
        public UserCell()
        {
            InitializeComponent();

            SetColors();

            AppearenceSettings.CheckAndSetColors += SetColors;
        }

        public static readonly BindableProperty SelectedItemBackgroundColorProperty = BindableProperty.Create("SelectedItemBackgroundColor", typeof(Color), typeof(UserCell));
        public Color SelectedItemBackgroundColor
        {
            get => (Color)GetValue(SelectedItemBackgroundColorProperty);
            set => SetValue(SelectedItemBackgroundColorProperty, value);
        }

        public void SetColors()
        {
            SurnameStack.BackgroundColor = NameStack.BackgroundColor =
                ImageStack.BackgroundColor =
                MainFrame.BorderColor = Storage.BackColor;

            SurnameLabel.TextColor = Storage.HelpColor;
            NameLabel.TextColor = Storage.TextColor;

            TintImageEffect effect = new TintImageEffect { TintColor = Storage.TextColor };
            Photo.Effects.Clear();
            Photo.Effects.Add(effect);
            SelectedItemBackgroundColor = Storage.BackColor;
            MainFrame.HasShadow = !Storage.IsDarkTheme;
        }
    }
}