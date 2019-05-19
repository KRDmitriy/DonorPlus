using DonorPlus.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(DonorPlus.Renderers.EntryRenderer), typeof(EntryRenderIOS))]
namespace DonorPlus.iOS.Renderers
{
    public class EntryRenderIOS : EntryRenderer
    {
        public EntryRenderIOS() { }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.BorderStyle = UIKit.UITextBorderStyle.None;
            }
        }
    }
}