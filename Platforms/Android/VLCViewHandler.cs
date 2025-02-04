using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Handlers;
using LibVLCSharp.Shared;
using LibVLCSharp.Platforms.Windows;
using Microsoft.UI.Xaml.Controls;
using Camera.CustomControls;

namespace Camera.Platforms.Android
{
    public class VLCViewHandler : ViewHandler<VLCView, FrameLayout>
    {
        private LibVLC _libVLC;
        private MediaPlayer _mediaPlayer;
        private VideoView _videoView;

        public VLCViewHandler() : base(ViewMapper) { }

        protected override FrameLayout CreatePlatformView()
        {
            var context = MauiApplication.Current.ApplicationContext;
            var layout = new FrameLayout(context);

            _libVLC = new LibVLC(context);
            _videoView = new VideoView(context);
            _mediaPlayer = new MediaPlayer(_libVLC);
            _videoView.SetMediaPlayer(_mediaPlayer);

            layout.AddView(_videoView);
            return layout;
        }

        protected override void ConnectHandler(FrameLayout platformView)
        {
            base.ConnectHandler(platformView);
            if (VirtualView?.StreamUrl != null)
            {
                PlayStream(VirtualView.StreamUrl);
            }
        }

        private void PlayStream(string rtspUrl)
        {
            var media = new Media(_libVLC, rtspUrl, FromType.FromLocation);
            _mediaPlayer.Media = media;
            _mediaPlayer.Play();
        }
    }
}
