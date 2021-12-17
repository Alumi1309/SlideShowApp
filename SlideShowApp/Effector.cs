using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace SlideShowApp
{
    class EffectCreator
    {
        public static System.Drawing.Image GetEffectedImage(List<Mat> imageList, int originalImageIndex, int frameLength, int framePos)
        {
            double pos = framePos;
            double len = frameLength;
            int effectImageIndex = originalImageIndex + 1;
            Mat originalMat = imageList[originalImageIndex];
            Mat effectImage = imageList[effectImageIndex];
            double alpha = (pos*pos) / (len*len) ; // (double)framePos / frameLength;
            //double whitealpha = frameLength / 2 < framePos ? 1 - (double)((double)(framePos - (frameLength/2)) / (frameLength/2)) : alpha*2;
            double whitealpha = (-1 *  ((pos - (len / 2)) * (pos - (len / 2)) / ((len / 2) * (len / 2)))) + 1;

            using (Mat outputMat = new Mat(originalMat.Size(), MatType.CV_8UC3))
            using (Mat whiteMat = new Mat(outputMat.Size(), MatType.CV_8UC3, new Scalar(255, 255, 255)))
            {
                Cv2.AddWeighted(originalMat, 1 - alpha, effectImage, alpha, 0, outputMat);
                Cv2.AddWeighted(outputMat, 1 - whitealpha, whiteMat, whitealpha, 0, outputMat);
                System.Drawing.Image outputImage = BitmapConverter.ToBitmap(outputMat);
                return outputImage;
            }
        }
    }

    internal class Effector
    {
        public double fps_{get;}
        public Mat previewImage_ { get; }
        public Mat nextImage_ { get; }

        public int width_ { get; }
        public int height_ { get; }

        public Effector(Mat previewImage, Mat nextImage, double fps)
        {
            previewImage_ = previewImage;
            nextImage_ = nextImage;
            fps_ = fps;

            if (previewImage_ is not null && nextImage_ is not null)
            {
                width_ = previewImage_.Width > nextImage_.Width ? previewImage_.Width : nextImage_.Width;
                height_ = previewImage_.Height > nextImage_.Height ? previewImage_.Height : nextImage_.Height;
            }
            else
            {
                width_ = 0;
                height_ = 0;
            }
        }


    }
}
