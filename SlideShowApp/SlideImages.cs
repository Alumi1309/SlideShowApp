using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

#nullable enable

namespace SlideShowApp
{
    class SlideImageCreator
    {
        IReadOnlyCollection<string> originalImagePathList_;
        List<Mat> slideImages_;
        List<Mat> photos_;
        private bool disposed_ = false;

        public SlideImageCreator(List<string> imagePathList)
        {
            originalImagePathList_ = imagePathList;
            slideImages_ = new List<Mat>();
            photos_ = new List<Mat>();
        }

        Mat? GetSlideImage(int index)
        {
            return (slideImages_.Count < index && index > 0) ? slideImages_[index] : null;
        }

        void AddSlideImage(Mat source)
        {
            if (source is not null)
                slideImages_.Add(source);
        }

        ~SlideImageCreator()
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
                    foreach (var mat in slideImages_) { mat.Dispose(); }
                    foreach (var mat in photos_) { mat.Dispose(); }
                }
                disposed_ = true;
            }
        }

        public int LoadPhoto(int videoWidth, int videoHeight)
        {
            try
            {
                foreach (var photoPath in originalImagePathList_)
                {
                    using (Mat photo = new Mat(photoPath))
                    {
                        var scale = GetPhotoScale(photo, videoWidth, videoHeight);
                        Mat memoryImage = new Mat((int)(photo.Height * scale), (int)(photo.Width * scale), MatType.CV_32FC3);
                        Cv2.Resize(photo, memoryImage, memoryImage.Size());
                        photos_.Add(memoryImage);
                    }
                }

            }catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                return -1;
            }
            return 0;
        }
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

        public static double GetPhotoScale(Mat mainImage, int videoWidth, int videoHeight)
        {
            return mainImage.Width > mainImage.Height ?
                (videoWidth * mainImageScale_widthLong) / mainImage.Width :
                (videoHeight * mainImageScale_heightLong) / mainImage.Height;
        }

        public void PutSubImage(int photoIndex, Mat backGroundImage, int angleMax)
        {
            Mat subImage = new Mat();
            try
            {
                subImage = photos_[photoIndex].Clone();
                if (subImage.Width == 0 || subImage.Height == 0) return;
                TransToGray(subImage);
                createBoxedImage(subImage);

                var random = new Random();
                var scale = GetSubImageScale(subImage, backGroundImage);
                var angle = random.Next(0 - angleMax, angleMax);
                var pos = new Point(random.Next(0 - backGroundImage.Width / 6, (int)(backGroundImage.Width)),       //貼り付ける画像の中心位置
                                random.Next(0 - backGroundImage.Height / 6, (int)(backGroundImage.Height)));      //

                PutRotateImage(subImage, backGroundImage, scale, angle, pos);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            finally
            {
                subImage.Dispose();
            }
        }

        public void PutMainImage(int photoIndex, Mat backGroundImage, int angleMax)
        {
            Mat mainImage = new Mat();
            try
            {
                mainImage = photos_[photoIndex].Clone();
                createBoxedImage(mainImage);

                var random = new Random();
                var angle = random.Next(0 - angleMax, angleMax);
                var pos = new Point(mainImage.Width / 2, mainImage.Height / 2);

                Cv2.CvtColor(backGroundImage, backGroundImage, ColorConversionCodes.GRAY2BGR);

                PutRotateImage(mainImage, backGroundImage, 1, angle, pos);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            finally
            {
                mainImage.Dispose();
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
        const double mainImageScale_widthLong = 0.7;
        const double mainImageScale_heightLong = 1.0;
        public void Create(int mainImageIndex, List<string> ImagePathList, Mat SlideImage)
        {
            var random = new Random();
            for (int i = 0; i < 200; i++)
            {
                PutSubImage(random.Next(0, ImagePathList.Count()),
                    SlideImage, subImageAngleMax);
            }

            PutMainImage(mainImageIndex, SlideImage, mainImageAngleMax);
        }



    }
}
