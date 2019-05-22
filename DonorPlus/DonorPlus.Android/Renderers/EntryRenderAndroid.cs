using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using DonorPlus.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;


[assembly: ExportRenderer(typeof(DonorPlus.Renderers.EntryRenderer), typeof(EntryRenderAndroid))]
namespace DonorPlus.Droid.Renderers
{
    public class EntryRenderAndroid : EntryRenderer
    {
        public EntryRenderAndroid(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SetPadding(15, 0, 0, 15);
                if (Control == null || e.NewElement == null)
                {
                    return;
                }

                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                {
                    Control.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.White);
                }
                else
                {
                    Control.Background.SetColorFilter(Android.Graphics.Color.White, PorterDuff.Mode.SrcAtop);
                }
            }
        }
    }
}