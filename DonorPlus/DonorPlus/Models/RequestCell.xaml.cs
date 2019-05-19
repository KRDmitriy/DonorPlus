using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DonorPlus.Models
{
    public delegate void MyAction(int x);

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RequestCell : ViewCell
    {
        public static MyAction ShowAuthorPage;
        public RequestCell()
        {
            InitializeComponent();

            SetColors();

            AppearenceSettings.CheckAndSetColors += SetColors;
        }

        public void SetColors()
        {
            MainFrame.BackgroundColor = Storage.BackColor;
            MainFrame.BorderColor = Storage.HelpColor;
            Header.BackgroundColor = Storage.HelpColor;
            BloodGroupLabel.TextColor = BloodGroupDataLabel.TextColor =
                RFactorLabel.TextColor = RFactorDataLabel.TextColor =
                RegionLabel.TextColor = RegionDataLabel.TextColor =
                AuthorLabel.TextColor = AuthorDataLabel.TextColor =
                ExtraBloodDataLabel.TextColor = ExtraBloodDataValueLabel.TextColor = Color.White;

            Description.TextColor = Storage.TextColor;
        }

        private void ShowAuthor(object sender, EventArgs e)
        {
            Action.Tapped -= ShowAuthor;
            try
            {
                ShowAuthorPage?.Invoke(int.Parse(AuthorID.Text));
            }
            finally { }
            Action.Tapped += ShowAuthor;
        }
    }
}