using DonorPlusLib;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DonorPlus
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RequestPage : ContentPage
    {
        public RequestPage()
        {
            InitializeComponent();

            SetColors();

            BloodGroupEntry.SelectedItem = "Выберите ...";
            RFactorEntry.SelectedItem = "Выберите ...";

            AppearenceSettings.CheckAndSetColors += SetColors;
        }

        public void SetColors()
        {
            BackgroundColor = Storage.BackColor;
            MainLabel.TextColor = Storage.TextColor;

            DescriptionEntry.BackgroundColor = BloodGroupEntry.BackgroundColor =
                RFactorEntry.BackgroundColor = ExtraBloodDataEntry.BackgroundColor = Storage.BackColor;

            DescriptionEntry.PlaceholderColor = ExtraBloodDataEntry.PlaceholderColor = Color.LightGray;


            DescriptionLabelStack.BackgroundColor = BloodGroupLabelStack.BackgroundColor =
                RFactorLabelStack.BackgroundColor = ExtraBloodDataLabelStack.BackgroundColor = Storage.HelpColor;

            DescriptionEntryStack.BackgroundColor = BloodGroupEntryStack.BackgroundColor =
                RFactorEntryStack.BackgroundColor = ExtraBloodDataEntryStack.BackgroundColor = Storage.BackColor;

            DescriptionEntry.TextColor = BloodGroupEntry.TextColor =
                RFactorEntry.TextColor = ExtraBloodDataEntry.TextColor = Storage.TextColor;

            DescriptionLabel.TextColor = BloodGroupLabel.TextColor =
                RFactorLabel.TextColor = ExtraBloodDataLabel.TextColor =
                SendButton.TextColor = Color.White;

            SendButton.BackgroundColor = Storage.HelpColor;
        }

        private bool CheckData()
        {
            if (BloodGroupEntry.SelectedItem.ToString() == "Выберите ..." || 
                RFactorEntry.SelectedItem.ToString() == "Выберите ...")
                return false;
            return !(string.IsNullOrWhiteSpace(BloodGroupEntry.SelectedItem.ToString()) ||
                string.IsNullOrWhiteSpace(RFactorEntry.SelectedItem.ToString()));
        }

        private async void SendButton_Clicked(object sender, System.EventArgs e)
        {
            if (CheckData())
            {
                Request request = new Request()
                {
                    AuthorID = Storage.User.Id,
                    BloodGroup = BloodGroupEntry.SelectedItem.ToString(),
                    RFactor = RFactorEntry.SelectedItem.ToString(),
                    ExtraBloodData = ExtraBloodDataEntry.Text.Trim(),
                    Descripton = DescriptionEntry.Text.Trim(),
                    Region = "Москва",
                    Solved = false
                };
                string res = await Task.Run(() => Requests.Push(request));
                if (res != "OK")
                {
                    await DisplayAlert("Ошибка", "Возможно отсутствует подключение к интернету", "ОК");
                }
                else
                {
                    OnBackButtonPressed();
                }
            }
            else
            {
                await DisplayAlert("Внимание", "Выберите группу крови и резус-фактор", "OK");
            }
        }
    }
}