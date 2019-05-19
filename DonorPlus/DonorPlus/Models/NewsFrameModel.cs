using Xamarin.Forms;

namespace DonorPlus.Models
{
    public class NewsFrameModel : Frame
    {
        private Label HeaderLabel, FooterLabel, MainLabel;
        private StackLayout HeaderStack, FooterStack;
        private readonly StackLayout MainStack;

        public NewsFrameModel(string Header, string MainText, string Footer, bool isRef)
        {
            HasShadow = true;
            HorizontalOptions = VerticalOptions = LayoutOptions.FillAndExpand;
            CornerRadius = 20;
            Padding = 0;
            Margin = new Thickness(20, 0, 20, 0);

            HeaderLabel = new Label
            {
                Text = Header,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                HorizontalOptions = VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
            };

            FooterLabel = new Label
            {
                Text = Footer,
                FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
                HorizontalOptions = VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
            };

            MainLabel = new Label
            {
                Text = MainText,
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                HorizontalOptions = VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.Start,
                Margin = new Thickness(20, 5, 20, 5)
            };

            HeaderStack = new StackLayout
            {
                Children = { HeaderLabel },
                Padding = 0,
                Margin = 0,
                HorizontalOptions = VerticalOptions = LayoutOptions.FillAndExpand,
            };

            FooterStack = new StackLayout
            {
                Children = { FooterLabel },
                Padding = 0,
                Margin = 0,
                HorizontalOptions = VerticalOptions = LayoutOptions.FillAndExpand,
            };

            MainStack = new StackLayout
            {
                Children = { MainLabel },
                Padding = 0,
                Margin = 0,
                HorizontalOptions = VerticalOptions = LayoutOptions.FillAndExpand,
            };

            Grid grid = new Grid
            {
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition { Height = 30 },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = 15 },
                },
                RowSpacing = 0,
                HorizontalOptions = VerticalOptions = LayoutOptions.FillAndExpand
            };

            grid.Children.Add(HeaderStack, 0, 0);
            grid.Children.Add(MainStack, 0, 1);
            grid.Children.Add(FooterStack, 0, 2);

            Content = grid;

            if (isRef)
            {
                TapGestureRecognizer gesture = new TapGestureRecognizer();
                gesture.Tapped += (s, e) =>
                {
                    Navigation.PushModalAsync(new AppearenceSettings());
                };
                GestureRecognizers.Add(gesture);
            }

            SetColors();

            AppearenceSettings.CheckAndSetColors += SetColors;
        }

        public void SetColors()
        {
            BackgroundColor = BorderColor = Storage.BackColor;
            FooterLabel.TextColor = HeaderLabel.TextColor = Color.White;
            MainLabel.TextColor = Storage.TextColor;
            HeaderStack.BackgroundColor = FooterStack.BackgroundColor = Storage.HelpColor;
        }
    }
}
