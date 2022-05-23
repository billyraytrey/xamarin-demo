using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.IO;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinAccountSetupDemo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SuccessPage : ContentPage
    {
        public SuccessPage()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PopToRootAsync();
            return base.OnBackButtonPressed();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            svg = null;
            LoadSvg("check_circle.svg");
            xCanvasView.InvalidateSurface();
        }
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (Width > Height)
            {
                xCanvasView.WidthRequest = Height * .75;
                xCanvasView.HeightRequest = Height * .75;
            }
            else
            {
                xCanvasView.WidthRequest = Width * .75;
                xCanvasView.HeightRequest = Width * .75;
            }
        }


        private SkiaSharp.Extended.Svg.SKSvg svg;

        private static Stream GetImageStream(string svgName)
        {
            var type = typeof(App).GetTypeInfo();
            var assembly = type.Assembly;

            var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{svgName}");
            return stream;
        }


        private void LoadSvg(string svgName)
        {
            svg = new SkiaSharp.Extended.Svg.SKSvg();

            using (var stream = GetImageStream(svgName))
            {
                svg.Load(stream);
            }
        }

        private void OnPainting(object sender, SKPaintSurfaceEventArgs e)
        {
            try
            {
                var surface = e.Surface;
                var canvas = surface.Canvas;

                var width = e.Info.Width;
                var height = e.Info.Height;

                canvas.Clear(SKColors.Transparent);

                if (svg == null)
                {
                    return;
                }

                float scaleX = width / svg.Picture.CullRect.Width;
                float scaleY = height / svg.Picture.CullRect.Height;
                var matrix = SKMatrix.CreateScale(scaleX, scaleY);

                canvas.DrawPicture(svg.Picture, ref matrix);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private async void LoginClicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
    }
}