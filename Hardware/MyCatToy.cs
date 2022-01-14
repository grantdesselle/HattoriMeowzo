using System;
using System.Threading.Tasks;
using System.Diagnostics;


namespace CatToyWebApp.Hardware
{
    public class MyCatToy
    {
        public Camera CatCamera { get; set; }
        private MotionDetector CatDetector { get; set; }
        private Servo ToyServo { get; set; }
        public static bool CatToyIsPlaying { get; set; }

        public MyCatToy()
        {
            CatCamera = new Camera();
            CatDetector = new MotionDetector();
            ToyServo = new Servo();
            CatToyIsPlaying = false;

            StartAutoMode();
        }

        private void StartAutoMode()
        {
            CatDetector.MotionDetected += this.OnMotionDetected;
            CatDetector.DetectMotion();
        }

        public async void OnMotionDetected(object source, EventArgs e)
        {
            Console.WriteLine("Motion Detector just detected motion. Need to make camera check if its the cat");
            await CatCamera.TakePictureTaskNoString(Camera.MaybeCatPath); //should return this to check for cat method, & maybe not await it

            // Call Camera to take picture and once pic is taken, let me know if it sees a cat
            // and if it sees a cat, start a routine. 
            if (CheckForCat() == true)
            {
                Console.WriteLine("I see a cat, now I'll start playing");
                await ShortRoutineAsync();
            }
            else
            {
                Console.WriteLine("a cat was not found");
            }
            GoIntoStandby();
        }
        public bool CheckForCat()
        {
            var catDetection = new CatFaceDetection();
            //CatCamera.TakePictureTaskNoString(Camera.MaybeCatPath);           

            return catDetection.CheckCatPhoto();
        }
        public async Task ShortRoutineAsync()
        {
            MyCatToy.CatToyIsPlaying = true;

            this.CatCamera.TakePicture(Camera.CatPhotosPath);
            await ToyServo.MotorTestAsync();
            await this.CatCamera.FFmpegRawVideoConvert();
        }

        public void GoIntoStandby()
        {
            Console.WriteLine("Going into standby");
            MyCatToy.CatToyIsPlaying = false;

            if (System.IO.Directory.GetFiles(Camera.MaybeCatPath) != null)
            {
                foreach (var file in System.IO.Directory.GetFiles(Camera.MaybeCatPath))
                {
                    System.IO.File.Delete(file);
                }
            };

            CatDetector.DetectMotion();
        }
    }
}
