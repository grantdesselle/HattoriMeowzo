using System;
using System.Threading;
using System.Threading.Tasks;
using MMALSharp;
using MMALSharp.Common;
using MMALSharp.Components;
using MMALSharp.Handlers;
using MMALSharp.Ports;

namespace CatToyWebApp.Hardware
{
    public class Camera
    {
        MMALCamera cam = MMALCamera.Instance;

        // Magic Strings
        public static readonly string CatVideosPath = "/home/pi/Desktop/CatToyWebApp/wwwroot/CatVideos/";
        public static readonly string CatPhotosPath = "/home/pi/Desktop/CatToyWebApp/wwwroot/CatPhotos/";
        public static readonly string MaybeCatPath = "/home/pi/Desktop/CatToyWebApp/wwwroot/CatPhotos/MaybeCat/";

        public async void TakePicture(string mediaFolderPath)
        {
            using (var imgCaptureHandler = new ImageStreamCaptureHandler(mediaFolderPath, "jpg"))
            {
                await cam.TakePicture(imgCaptureHandler, MMALEncoding.JPEG, MMALEncoding.I420);
            }
        }

        public async Task TakePictureTaskNoString(string mediaFolderPath)
        {
            using (var imgCaptureHandler = new ImageStreamCaptureHandler(mediaFolderPath + DateTime.Now.ToString("MM-dd-yyyy-HH-mm-ss") + ".jpg"))
            {
                await cam.TakePicture(imgCaptureHandler, MMALEncoding.JPEG, MMALEncoding.I420);
            }
        }
        public async Task FFmpegRawVideoConvert()
        {
            using (var ffCaptureHandler = FFmpegCaptureHandler.RawVideoToMP4(CatVideosPath, DateTime.Now.ToString("MM-dd-yyyy-HH-mm-ss")))
            using (var vidEncoder = new MMALVideoEncoder())
            using (var renderer = new MMALVideoRenderer())
            {
                cam.ConfigureCameraSettings();

                var portConfig = new MMALPortConfig(MMALEncoding.H264, MMALEncoding.I420, 10, MMALVideoEncoder.MaxBitrateLevel4, null);

                // Create our component pipeline. Here we are using the H.264 standard with a YUV420 pixel format. The video will be taken at 25Mb/s.
                vidEncoder.ConfigureOutputPort(portConfig, ffCaptureHandler);

                cam.Camera.VideoPort.ConnectTo(vidEncoder);
                cam.Camera.PreviewPort.ConnectTo(renderer);

                // Camera warm up time
                await Task.Delay(2000);

                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));

                // Take video for whatever seconds listed above
                await cam.ProcessAsync(cam.Camera.VideoPort, cts.Token);
            }
        }
        
        // Probably Should Call this on app shutdown
        public void CleanUpCamera()
        {
            cam.Cleanup();
        }
        
        public Camera()
        {
            MMALCameraConfig.Resolution = new MMALSharp.Common.Utility.Resolution(1280, 720);
        }
    }
}
