using DonorPlus.Effects;
using DonorPlus.Renderers;
using Xamarin.Forms;

namespace DonorPlus.Models
{
    public class MenuFrameModel : Frame
    {
        private Label label = new Label();
        private ImageRenderer image;

        public MenuFrameModel(string imageSource, string text, int size)
        {

            AppearenceSettings.CheckAndSetColors += SetColors;

            HasShadow = false;
            CornerRadius = 0;

            TintImageEffect effect = new TintImageEffect { TintColor = Storage.TextColor };

            image = new ImageRenderer();
            if (!string.IsNullOrEmpty(imageSource))
            {
                image.Source = "Resources/" + imageSource;
                image.HeightRequest = image.WidthRequest = 20;
                image.Aspect = Aspect.AspectFit;
                image.Effects.Add(effect);
                image.VerticalOptions = LayoutOptions.Center;
                image.HorizontalOptions = LayoutOptions.Start;
            }

            label = new Label
            {
                Text = text
            };

            label.Margin = new Thickness(5, 0, 0, 0);

            switch (size)
            {
                case 0:
                    label.FontSize = Device.GetNamedSize(
                        NamedSize.Small, typeof(Label));
                    break;

                case 1:
                    label.FontSize = Device.GetNamedSize(
                        NamedSize.Default, typeof(Label));
                    break;

                case 2:
                    {
                        label.FontSize = Device.GetNamedSize(
                            NamedSize.Medium, typeof(Label));
                        label.FontAttributes = FontAttributes.Bold;
                        label.Margin = 0;
                    }
                    break;

                case 3:
                    {
                        label.FontSize = Device.GetNamedSize(
                            NamedSize.Large, typeof(Label));
                        label.FontAttributes = FontAttributes.Bold;
                        label.Margin = 0;
                    }
                    break;
            }

            label.HorizontalTextAlignment = label.VerticalTextAlignment
                = TextAlignment.Center;
            label.VerticalOptions = LayoutOptions.FillAndExpand;
            if (!string.IsNullOrEmpty(imageSource))
            {
                label.HorizontalOptions = LayoutOptions.StartAndExpand;
            }
            else
            {
                label.HorizontalOptions = LayoutOptions.FillAndExpand;
            }


            StackLayout layout = new StackLayout() { Children = { image, label } };
            layout.Orientation = StackOrientation.Horizontal;
            layout.HorizontalOptions = layout.VerticalOptions
                = LayoutOptions.FillAndExpand;
            layout.Padding = 0;

            Content = layout;

            SetColors();
        }

        public void SetColors()
        {
            BackgroundColor = Storage.BackColor;

            if (label.Text == "Выйти")
            {
                label.TextColor = Storage.SpecialRedColor;
                TintImageEffect effect = new TintImageEffect { TintColor = Storage.SpecialRedColor };

                image.Effects.Clear();
                image.Effects.Add(effect);
            }
            else
            {
                label.TextColor = Storage.TextColor;

                TintImageEffect effect = new TintImageEffect { TintColor = Storage.TextColor };

                image.Effects.Clear();
                image.Effects.Add(effect);
            }
        }
    }
}
