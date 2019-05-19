using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Views;
using DonorPlus.Droid.Renderers;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(DonorPlus.Renderers.ImageRenderer), typeof(ImageRendererAndroid))]
namespace DonorPlus.Droid.Renderers
{
    /// <summary>
    /// ImageCircle Implementation
    /// </summary>
    [Preserve(AllMembers = true)]
    public class ImageRendererAndroid : ImageRenderer
    {
#pragma warning disable CS0618 // Type or member is obsolete
        public ImageRendererAndroid() : base()
#pragma warning restore CS0618 // Type or member is obsolete
        {

        }

        public ImageRendererAndroid(Context context) : base(context)
        {

        }
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        /// <summary>
        /// Used for registration with dependency service
        /// </summary>
        public static async void Init()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
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

            if (e.OldElement == null)
            {
                //Only enable hardware accelleration on lollipop
                if ((int)Android.OS.Build.VERSION.SdkInt < 21)
                {
                    SetLayerType(LayerType.Software, null);
                }

            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);


            if (e.PropertyName == DonorPlus.Renderers.ImageRenderer.BorderColorProperty.PropertyName ||
              e.PropertyName == DonorPlus.Renderers.ImageRenderer.BorderThicknessProperty.PropertyName ||
              e.PropertyName == DonorPlus.Renderers.ImageRenderer.FillColorProperty.PropertyName)
            {
                Invalidate();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="child"></param>
        /// <param name="drawingTime"></param>
        /// <returns></returns>
        protected override bool DrawChild(Canvas canvas, Android.Views.View child, long drawingTime)
        {
            try
            {

                float radius = Math.Min(Width, Height) / 2f;

                float borderThickness = ((DonorPlus.Renderers.ImageRenderer)Element).BorderThickness;

                float strokeWidth = 0f;

                if (borderThickness > 0)
                {
                    float logicalDensity = Android.App.Application.Context.Resources.DisplayMetrics.Density;
                    strokeWidth = (float)Math.Ceiling(borderThickness * logicalDensity + .5f);
                }

                radius -= strokeWidth / 2f;




                Path path = new Path();
                path.AddCircle(Width / 2.0f, Height / 2.0f, radius, Path.Direction.Ccw);


                canvas.Save();
                canvas.ClipPath(path);



                Paint paint = new Paint
                {
                    AntiAlias = true
                };
                paint.SetStyle(Paint.Style.Fill);
                paint.Color = ((DonorPlus.Renderers.ImageRenderer)Element).FillColor.ToAndroid();
                canvas.DrawPath(path, paint);
                paint.Dispose();


                bool result = base.DrawChild(canvas, child, drawingTime);

                path.Dispose();
                canvas.Restore();

                path = new Path();
                path.AddCircle(Width / 2f, Height / 2f, radius, Path.Direction.Ccw);


                if (strokeWidth > 0.0f)
                {
                    paint = new Paint
                    {
                        AntiAlias = true,
                        StrokeWidth = strokeWidth
                    };
                    paint.SetStyle(Paint.Style.Stroke);
                    paint.Color = ((DonorPlus.Renderers.ImageRenderer)Element).BorderColor.ToAndroid();
                    canvas.DrawPath(path, paint);
                    paint.Dispose();
                }

                path.Dispose();
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Unable to create circle image: " + ex);
            }

            return base.DrawChild(canvas, child, drawingTime);
        }
    }
}