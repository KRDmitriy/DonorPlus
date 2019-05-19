using CoreAnimation;
using CoreGraphics;
using DonorPlus.iOS;
using Foundation;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(DonorPlus.Renderers.ImageRenderer), typeof(ImageCircleRenderer))]
namespace DonorPlus.iOS
{
    /// <summary>
    /// ImageCircle Implementation
    /// </summary>
    [Preserve(AllMembers = true)]
    public class ImageCircleRenderer : Xamarin.Forms.Platform.iOS.ImageRenderer
    {
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        /// <summary>
        /// Used for registration with dependency service
        /// </summary>
        public static async void Init()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
        {
            DateTime temp = DateTime.Now;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);
            if (Element == null)
            {
                return;
            }

            CreateCircle();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == VisualElement.HeightProperty.PropertyName ||
                e.PropertyName == VisualElement.WidthProperty.PropertyName ||
              e.PropertyName == DonorPlus.Renderers.ImageRenderer.BorderColorProperty.PropertyName ||
              e.PropertyName == DonorPlus.Renderers.ImageRenderer.BorderThicknessProperty.PropertyName ||
              e.PropertyName == DonorPlus.Renderers.ImageRenderer.FillColorProperty.PropertyName)
            {
                CreateCircle();
            }
        }

        private void CreateCircle()
        {
            try
            {
                double min = Math.Min(Element.Width, Element.Height);
                Control.Layer.CornerRadius = (nfloat)(min / 2.0);
                Control.Layer.MasksToBounds = false;
                Control.BackgroundColor = ((DonorPlus.Renderers.ImageRenderer)Element).FillColor.ToUIColor();
                Control.ClipsToBounds = true;

                float borderThickness = ((DonorPlus.Renderers.ImageRenderer)Element).BorderThickness;

                //Remove previously added layers
                CALayer tempLayer = Control.Layer.Sublayers?
                                       .Where(p => p.Name == borderName)
                                       .FirstOrDefault();
                tempLayer?.RemoveFromSuperLayer();

                CALayer externalBorder = new CALayer
                {
                    Name = borderName,
                    CornerRadius = Control.Layer.CornerRadius,
                    Frame = new CGRect(-.5, -.5, min + 1, min + 1),
                    BorderColor = ((DonorPlus.Renderers.ImageRenderer)Element).BorderColor.ToCGColor(),
                    BorderWidth = ((DonorPlus.Renderers.ImageRenderer)Element).BorderThickness
                };

                Control.Layer.AddSublayer(externalBorder);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to create circle image: " + ex);
            }
        }

        private const string borderName = "borderLayerName";
    }
}