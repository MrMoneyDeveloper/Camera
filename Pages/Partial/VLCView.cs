using LibVLCSharp.Shared;
using Microsoft.Maui.Controls;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using System;

namespace Camera.CustomControls
{
    public class VLCView : View
    {
        //private readonly LibVLC _libVLC;
        //private readonly LibVLCSharp.Shared.MediaPlayer _mediaPlayer;
        //private readonly SKCanvasView _canvasView;


        public static readonly BindableProperty StreamUrlProperty =
            BindableProperty.Create(nameof(StreamUrl), typeof(string), typeof(VLCView), default(string));

        public string StreamUrl
        {
            get => (string)GetValue(StreamUrlProperty);
            set => SetValue(StreamUrlProperty, value);
        }



        public VLCView(string rtspUrl)
        {
            Core.Initialize();
            StreamUrl = rtspUrl;

            //_libVLC = new LibVLC();
            //_mediaPlayer = new LibVLCSharp.Shared.MediaPlayer(_libVLC);

            //_canvasView = new SKCanvasView
            //{
            //    BackgroundColor = Colors.Black,
            //    HorizontalOptions = LayoutOptions.FillAndExpand,
            //    VerticalOptions = LayoutOptions.FillAndExpand
            //};

            //_canvasView.PaintSurface += OnPaintSurface;

            //Content = new Grid
            //{
            //    Children = { _canvasView },
            //    BackgroundColor = Colors.Black,
            //    HorizontalOptions = LayoutOptions.FillAndExpand,
            //    VerticalOptions = LayoutOptions.FillAndExpand
            //};

            PlayStream(rtspUrl);
        }

        //private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        //{
        //    _canvasView.InvalidateSurface();
        //}

        private void PlayStream(string rtspUrl)
        {
            LibVLC libVLC = new LibVLC();
            MediaPlayer mediaPlayer = new MediaPlayer(libVLC)
            {
                Media = new Media(libVLC, rtspUrl, FromType.FromLocation)
            };
            mediaPlayer.Play();

            //var media = new Media(_libVLC, rtspUrl, FromType.FromLocation);
            //_mediaPlayer.Media = media;
            //_mediaPlayer.Play();
        }
    }
}
