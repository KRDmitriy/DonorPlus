using System;
using System.Collections.Generic;
using DonorPlus.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DonorPlus
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class News : ContentPage
    {
        List<NewsFrameModel> news = new List<NewsFrameModel>();

        public News()
        {
            InitializeComponent();

            SetContent();

            SetColors();

            AppearenceSettings.CheckAndSetColors += SetColors;
        }

        public void SetColors()
        {
            BackgroundColor = Storage.BackColor;

            NewsLabel.TextColor = Storage.TextColor;
        }

        public void SetContent()
        {
            news.Add(new NewsFrameModel("Темная тема!", "В новой версии 1.0.1 появилась" +
                " темная тема. Настройте приложение по своему вкусу!", "Нажмите, чтобы опробовать", true));

            news.Add(new NewsFrameModel("Звонки и письма", "В новой версии 1.0.2 звонить и писать " +
                "стало ещё удобнее! Просто нажмите на телефон или эл. почту в Профиле", "Попробуйте уже сейчас", false));

            Stack0.Children.Add(news[0]);
            Stack1.Children.Add(news[1]);
        }

        private async void Frame_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Settings());
        }
    }
}