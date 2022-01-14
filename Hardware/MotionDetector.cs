using System;
using System.Threading;
using System.Device.Gpio;
using Iot.Device.Hcsr501;
using System.Threading.Tasks;

namespace CatToyWebApp.Hardware
{
    public class MotionDetector
    {
        public delegate void MotionDetectedEventHandler(object sender, EventArgs args);
        public event MotionDetectedEventHandler MotionDetected;

        const int Pin = 17;

        public async void DetectMotion()
        {
            await StartDetectingMotion();
        }

        private async Task StartDetectingMotion()
        {
            if (MyCatToy.CatToyIsPlaying)
                return;

            using Hcsr501 sensor = new(Pin);

            while (!MyCatToy.CatToyIsPlaying)
            {
                if (sensor.IsMotionDetected)
                {
                    OnMotionDetected();
                    await Task.Delay(1000);
                    return;
                }
            }
        }

        protected virtual void OnMotionDetected()
        {
            if (MotionDetected != null)
            {
                MotionDetected(this, EventArgs.Empty);
            }
        }
    }
}
