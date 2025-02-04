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

namespace Camera.Platforms.Windows
{
    public class VLCViewHandler : ViewHandler<VLCView, VideoView>
    {
        private LibVLC _libVLC;
        private MediaPlayer _mediaPlayer;

        public VLCViewHandler() : base(ViewMapper) { }

        protected override VideoView CreatePlatformView()
        {
            Core.Initialize(); // Initialize LibVLC
            _libVLC = new LibVLC();
            _mediaPlayer = new MediaPlayer(_libVLC);

            var videoView = new VideoView
            {
                MediaPlayer = _mediaPlayer
            };

            return videoView;
        }

        protected override void DisconnectHandler(VideoView platformView)
        {
            _mediaPlayer?.Dispose();
            _libVLC?.Dispose();
            base.DisconnectHandler(platformView);
        }
    }

}
