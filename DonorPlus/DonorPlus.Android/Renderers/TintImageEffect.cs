using Android.Graphics;
using Android.Renderscripts;
using Android.Widget;
using DonorPlus.Droid.Renderers;
using Java.Lang;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using FormsTintImageEffect = DonorPlus.Effects.TintImageEffect;

[assembly: ResolutionGroupName(DonorPlus.Effects.TintImageEffect.GroupName)]
[assembly: ExportEffect(typeof(TintImageEffect), DonorPlus.Effects.TintImageEffect.Name)]
namespace DonorPlus.Droid.Renderers
{

    public class TintImageEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                FormsTintImageEffect effect = (FormsTintImageEffect)Element.Effects.FirstOrDefault(e => e is FormsTintImageEffect);

                if (effect == null || !(Control is ImageView image))
                {
                    return;
                }

                PorterDuffColorFilter filter = new PorterDuffColorFilter(effect.TintColor.ToAndroid(), PorterDuff.Mode.SrcIn);
                image.SetColorFilter(filter);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"An error occurred when setting the {typeof(TintImageEffect)} effect: {ex.Message}\n{ex.StackTrace}");
            }
        }

        protected override void OnDetached() { }
    }
}