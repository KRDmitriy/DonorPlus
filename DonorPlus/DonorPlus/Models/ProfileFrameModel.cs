using Xamarin.Forms;

namespace DonorPlus.Models
{
    public class ProfileFrameModel : Frame
    {
        public ProfileFrameModel(string key, string value)
        {
            HasShadow = true;
            BackgroundColor = Storage.BackColor;
            BorderColor = Storage.HelpColor;
            CornerRadius = 20;
            Padding = 0;
            Margin = new Thickness(20, 10, 20, 5);
            HorizontalOptions = VerticalOptions = LayoutOptions.FillAndExpand;
            HeightRequest = 50;


            Label keyLabel = new Label
            {
                Text = key,
                TextColor = Color.White,// Storage.TextColor,
                Margin = new Thickness(15, 0, 0, 0),
                FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            Label valueLabel = new Label
            {
                Text = value,
                TextColor = Storage.TextColor,
                Margin = new Thickness(45, 0, 0, 0),
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
            };

            Grid keyGrid = new Grid()
            {
                Children = { keyLabel },
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = 0,
                Margin = 0,
                BackgroundColor = Storage.HelpColor
            };

            Grid emptyGrid = new Grid()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = 0,
                Margin = 0,
                BackgroundColor = Color.Black
            };

            Grid valueGrid = new Grid()
            {
                Children = { valueLabel },
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = 0,
                Margin = 0,
                BackgroundColor = Storage.BackColor
            };

            Grid grid = new Grid
            {
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = 1 },
                    new RowDefinition { Height = GridLength.Star }
                }
            };

            grid.VerticalOptions = grid.HorizontalOptions = LayoutOptions.FillAndExpand;
            grid.Children.Add(keyGrid, 0, 0);
            grid.Children.Add(emptyGrid, 0, 1);
            grid.Children.Add(valueGrid, 0, 2);
            grid.RowSpacing = 0;

            Content = grid;
        }
    }
}
