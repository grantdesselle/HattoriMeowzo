using OpenCvSharp;
using System.Threading.Tasks;

namespace CatToyWebApp
{
    class CatFaceDetection
    {
        string PossibleCatImage { get; set; }
        readonly string PossibleCatImageDirectory = "wwwroot/CatPhotos/MaybeCat/";
        readonly string cascadeImagePath = "wwwroot/haarcascade_frontalcatface.xml";
        bool identifiedCat = false;

        public bool CheckCatPhoto()
        {
            if (System.IO.Directory.GetFiles(PossibleCatImageDirectory) != null)
            {
                PossibleCatImage = System.IO.Directory.GetFiles(PossibleCatImageDirectory)[0];
            }

            using var catsCascade = new CascadeClassifier(cascadeImagePath);

            Mat catDetectionResult = DetectFace(catsCascade, PossibleCatImage);

            /*
            Cv2.ImShow("Faces by CatDetector", catDetectionResult);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
            */

            if (this.identifiedCat == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cascade"></param>
        /// <returns></returns>
        private Mat DetectFace(CascadeClassifier cascade, string imgPath)
        {
            Mat result;

            using (var src = new Mat(imgPath, ImreadModes.Color))
            using (var gray = new Mat())
            {
                result = src.Clone();
                Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);

                // Detect faces
                Rect[] faces = cascade.DetectMultiScale(
                    gray, 1.08, 2, HaarDetectionTypes.ScaleImage, new Size(30, 30));

                if (faces.Length > 0)
                {
                    identifiedCat = true;
                }
                else
                {
                    identifiedCat = false;
                }

                /*
                foreach (Rect face in faces)
                {
                    var center = new OpenCvSharp.Point
                    {
                        X = (int)(face.X + face.Width * 0.5),
                        Y = (int)(face.Y + face.Height * 0.5)
                    };
                    var axes = new OpenCvSharp.Size
                    {
                        Width = (int)(face.Width * 0.5),
                        Height = (int)(face.Height * 0.5)
                    };
                    Cv2.Ellipse(result, center, axes, 0, 0, 360, new Scalar(255, 0, 255), 4);
                }
                */
            }
            return result;
        }
    }
}