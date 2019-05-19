using DonorPlus.iOS.Renderers;
using System;
using System.Linq;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using FormsTintImageEffect = DonorPlus.Effects.TintImageEffect;


[assembly: ResolutionGroupName(DonorPlus.Effects.TintImageEffect.GroupName)]
[assembly: ExportEffect(typeof(TintImageEffect), DonorPlus.Effects.TintImageEffect.Name)]
namespace DonorPlus.iOS.Renderers
{
    public class TintImageEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                FormsTintImageEffect effect = (FormsTintImageEffect)Element.Effects.FirstOrDefault(e => e is FormsTintImageEffect);

                if (effect == null || !(Control is UIImageView image))
                {
                    return;
                }

                image.Image = image.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                image.TintColor = effect.TintColor.ToUIColor();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"An error occurred when setting the {typeof(TintImageEffect)} effect: {ex.Message}\n{ex.StackTrace}");
            }
        }

        protected override void OnDetached() { }
    }
}