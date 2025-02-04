//using Android.Widget;
using Camera.CustomControls;
using LibVLCSharp.MAUI;
using LibVLCSharp.Shared;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;

namespace Camera.Pages
{
    public partial class RTSPStreamPage : ContentPage
    {
        private LibVLC _libVLC;
        private Grid _streamGrid;
        private readonly List<string> rtspUrls = new()
        {
            "rtsp://admin:Admin12345@e7e90eaa3a0c.sn.mynetname.net:4244/Streaming/Channels/102/",
            "rtsp://admin:Admin12345@e7e90eaa3a0c.sn.mynetname.net:4244/Streaming/Channels/102/",
            "rtsp://admin:Admin12345@e7e90eaa3a0c.sn.mynetname.net:4244/Streaming/Channels/102/",
            "rtsp://admin:Admin12345@e7e90eaa3a0c.sn.mynetname.net:4244/Streaming/Channels/102/"
        };

        public RTSPStreamPage()
        {
            //InitializeComponent();
            //Core.Initialize();
            SetupGridLayout();
            //AddVideoPlayers();
        }

        private void SetupGridLayout()
        {
            // Initialize VLC
            Core.Initialize();
            _libVLC = new LibVLC();

            // Create the grid layout
            _streamGrid = new Grid
            {
                RowSpacing = 5,
                ColumnSpacing = 5
            };

            int numColumns = rtspUrls.Count; // Adjust as needed
            int numRows = (int)Math.Ceiling(rtspUrls.Count / (double)numColumns);

            for (int r = 0; r < numRows; r++)
            {
                _streamGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
            }
            for (int c = 0; c < numColumns; c++)
            {
                _streamGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            }

            // Add video streams to the grid
            for (int i = 0; i < rtspUrls.Count; i++)
            {
                int row = i / numColumns;
                int col = i % numColumns;

                var videoView = CreateVideoView(rtspUrls[i]);
                _streamGrid.Children.Add(videoView);
                Grid.SetRow(videoView, row);
                Grid.SetColumn(videoView, col);
            }

            Content = new ScrollView { Content = _streamGrid };
        }

        private View CreateVideoView(string rtspUrl)
        {
            var mediaPlayer = new MediaPlayer(_libVLC)
            {
                EnableHardwareDecoding = true
            };

            var videoView = new VideoView
            {
                MediaPlayer = mediaPlayer,
                HeightRequest = 300,
                WidthRequest = 300,
                BackgroundColor = Colors.Black
            };

            var media = new Media(_libVLC, rtspUrl, FromType.FromLocation);
            mediaPlayer.Play(media);

            return videoView;
        }

    }
}
        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    Device.BeginInvokeOnMainThread(() =>
        //    {
        //        var testLabel = new Label
        //        {
        //            Text = "TEST",
        //            BackgroundColor = Color.FromRgb(255,0,0),
        //            TextColor = Color.FromRgb(0, 0, 0)
        //        };

        //        StreamGrid.Children.Add(testLabel);
        //    });

        //}


    

