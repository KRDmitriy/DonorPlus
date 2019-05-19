using Android.Content;
using Android.Graphics;
using DonorPlus.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(DonorPlus.Controls.ExtendedEditorControl), typeof(ExtendedEditorControlAndroid))]
namespace DonorPlus.Droid.Renderers
{
    internal class ExtendedEditorControlAndroid : EditorRenderer
    {
        public ExtendedEditorControlAndroid(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Xamarin.Forms.Color customColor = Xamarin.Forms.Color.White;
                Control.Background.SetColorFilter(customColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
            }
        }
    }
}