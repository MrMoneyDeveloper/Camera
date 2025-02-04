using AVFoundation;
using Camera.CustomControls;
using LibVLCSharp.Shared;
using Microsoft.Maui.Handlers;
using UIKit;

namespace Camera.Platforms.iOS
{
    public class VLCViewHandler : ViewHandler<VLCView, UIView>
    {
        private LibVLC _libVLC;
        private MediaPlayer _mediaPlayer;
        private UIView _videoView;

        public VLCViewHandler() : base(ViewMapper) { }

        protected override UIView CreatePlatformView()
        {
            _videoView = new UIView();
            _libVLC = new LibVLC();
            _mediaPlayer = new MediaPlayer(_libVLC);

            return _videoView;
        }

        protected override void ConnectHandler(UIView platformView)
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
