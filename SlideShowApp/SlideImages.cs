using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

#nullable enable

namespace SlideShowApp
{
    internal class SlideImages : IDisposable
    {
        IReadOnlyCollection<string> originalImagePathList_;
        List<Mat> slideImages_;
        private bool disposed_ = false;

        SlideImages(List<string> imagePathList)
        {
            originalImagePathList_ = imagePathList;
            slideImages_ = new List<Mat>();

        }

        Mat? GetSlideImage(int index)
        {
            return (slideImages_.Count < index && index > 0)? slideImages_[index] : null;
        }

        void AddSlideImage(Mat source)
        {
            if(source is not null)
                slideImages_.Add(source);
        }

        ~SlideImages()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed_)
            {
                if (disposing)
                {
                    foreach(var mat in slideImages_){mat.Dispose();}
                }
                disposed_ = true;
            }
        }

    }

    class SlideImageCreator
    {
        public static int GetImamgeArea(Mat image)
        {
            return image?.Width * image?.Height ?? 0;
        }

        public static string GetMatValStr(Mat mat)
        {
            string log = "";
            for (int j = 0; j < 2; j++)
            {
                log += "{ ";
                for (int i = 0; i < 3; i++)
                {
                    log += mat.At<double>(j, i).ToString() + " ";
                }
                log += "}";
            }
            return log;
        }

        public static void createBoxedImage(Mat image)
        {
            var rect = new Rect(0,0,image.Width,image.Height);
            Cv2.Rectangle(image, rect, Scalar.White, (image.Width + image.Height) / 60);
        }

        public static void TransToGray(Mat image)
        {
            Cv2.CvtColor(image, image, ColorConversionCodes.BGR2GRAY);
        }

        public static double GetSubImageScale(Mat subImage, Mat slideImage)
        {
            var slideImageArea = (double)GetImamgeArea(slideImage);
            var subImageArea = (double)GetImamgeArea(subImage);
            var scale = (slideImageArea / subImageArea) / subImageScale;
            scale =  Math.Sqrt(scale);
            return scale;
        }

        public static double GetMainImageScale(Mat mainImage, Mat slideImage)
        {
            return mainImage.Width > mainImage.Height ?
                (slideImage.Width * mainImageScale) / mainImage.Width :
                (slideImage.Height * mainImageScale) / mainImage.Height;
        }

        public static void PutSubImage(string subImagePath, Mat backGroundImage, int angleMax)
        {
            using (Mat subImage = new Mat(subImagePath))
            {
                if (subImage.Width == 0 || subImage.Height == 0) return;
                TransToGray(subImage);
                createBoxedImage(subImage);

                var random = new Random();
                var scale = GetSubImageScale(subImage, backGroundImage);
                var angle = random.Next(0 - angleMax, angleMax);
                var pos = new Point(random.Next(0 - backGroundImage.Width / 3, (int)(backGroundImage.Width * 1.3)),       //貼り付ける画像の中心位置
                                random.Next(0 - backGroundImage.Height / 3, (int)(backGroundImage.Height * 1.3)));      //

                PutRotateImage(subImage, backGroundImage, scale, angle, pos);
            }
        }

        public static void PutMainImage(string mainImagePath, Mat backGroundImage, int angleMax)
        {
            using (Mat mainImage = new Mat(mainImagePath))
            {
                createBoxedImage(mainImage);

                var random = new Random();
                var scale = GetMainImageScale(mainImage, backGroundImage);
                var angle = random.Next(0 - angleMax, angleMax);
                var pos = new Point(mainImage.Width/2, mainImage.Height/2);

                Cv2.CvtColor(backGroundImage, backGroundImage, ColorConversionCodes.GRAY2BGR);

                PutRotateImage(mainImage, backGroundImage, scale, angle, pos);
            }
        }

        public static void PutRotateImage(Mat rotateImage, Mat backGroundImage, double scale, int angle, Point pos)
        {
            Point imageCenter = new Point(rotateImage.Width / 2, rotateImage.Height / 2);
            Mat rotationMatrix = Cv2.GetRotationMatrix2D(imageCenter, angle, scale);

            rotationMatrix.At<double>(0, 2) -= pos.X - (backGroundImage.Width/2);
            rotationMatrix.At<double>(1, 2) -= pos.Y - (backGroundImage.Height/2);

            Cv2.WarpAffine(rotateImage, backGroundImage, rotationMatrix, backGroundImage.Size(),
                borderMode: BorderTypes.Transparent);

        }

        const int subImageAngleMax = 60;
        const int mainImageAngleMax = 10;
        const double subImageScale = 5;
        const double mainImageScale = 0.8;
        public static void Create(int mainImageIndex, List<string> ImagePathList, Mat SlideImage)
        {
            var random = new Random();
            for (int i = 0; i < 50; i++)
            {
                PutSubImage(ImagePathList[random.Next(0, ImagePathList.Count())],
                    SlideImage, subImageAngleMax);
            }

            PutMainImage(ImagePathList[mainImageIndex], SlideImage, mainImageAngleMax);
            Cv2.ImShow("main", SlideImage);
            Cv2.WaitKey(0);
        }



    }
}
