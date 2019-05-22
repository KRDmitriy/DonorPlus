using Xamarin.Forms;

namespace DonorPlus.Controls
{
    public class ExtendedEditorControl : Editor
    {
        public static new BindableProperty PlaceholderProperty
          = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(ExtendedEditorControl));

        public static new BindableProperty PlaceholderColorProperty
           = BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(ExtendedEditorControl), Color.LightGray);

        public static BindableProperty HasRoundedCornerProperty
        = BindableProperty.Create(nameof(HasRoundedCorner), typeof(bool), typeof(ExtendedEditorControl), false);

        public static BindableProperty IsExpandableProperty
        = BindableProperty.Create(nameof(IsExpandable), typeof(bool), typeof(ExtendedEditorControl), false);

        public bool IsExpandable
        {
            get => (bool)GetValue(IsExpandableProperty);
            set => SetValue(IsExpandableProperty, value);
        }
        public bool HasRoundedCorner
        {
            get => (bool)GetValue(HasRoundedCornerProperty);
            set => SetValue(HasRoundedCornerProperty, value);
        }

        public new string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public new Color PlaceholderColor
        {
            get => (Color)GetValue(PlaceholderColorProperty);
            set => SetValue(PlaceholderColorProperty, value);
        }

        public ExtendedEditorControl()
        {
            TextChanged += OnTextChanged;
        }

        ~ExtendedEditorControl()
        {
            TextChanged -= OnTextChanged;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsExpandable)
            {
                InvalidateMeasure();
            }
        }

    }
}