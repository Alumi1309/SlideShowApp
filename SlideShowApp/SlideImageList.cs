using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace SlideShowApp
{
    internal class SlideImageList : IDisposable
    {

        private bool disposed_ = false;
        public List<Mat> SlideImages = new List<Mat>();
        public List<Mat> ThumbnailImages = new List<Mat>();
        public int SlideNum { get { return SlideImages.Count(); } }

        ~SlideImageList()
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
                    foreach (var mat in SlideImages) { mat.Dispose(); }
                    foreach (var mat in ThumbnailImages) { mat.Dispose(); }
                }
                disposed_ = true;
            }
        }

        private Point CalcThumbnailSize(int pictureBoxWidth, int pictureBoxHeight, Mat originalImage)
        {
            int thumbnailWidth, thumbnailHeight;

            thumbnailWidth = pictureBoxWidth;
            thumbnailHeight = (int)((double)((double)pictureBoxWidth / originalImage.Width) * originalImage.Height);
            if (thumbnailHeight > pictureBoxHeight)
            {
                thumbnailWidth = (int)((double)((double)pictureBoxHeight / originalImage.Height) * originalImage.Width);
                thumbnailHeight = pictureBoxHeight;
            }

            return new Point(thumbnailWidth, thumbnailHeight);
        }

        public int LoadImages(int thumbnailWidth, int thumbnailHeight)
        {
            int slidenum = 0;
            try
            {
                string[] files = System.IO.Directory.GetFiles(
                    //    @"Z:\Share\_2021\2021.12\20211214_家族写真", "*", System.IO.SearchOption.AllDirectories);
                    @"\pic", "*", System.IO.SearchOption.AllDirectories);
                List<string> filelist = new List<string>();
                filelist.AddRange(files);


                foreach (var filePath in files)
                {
                    Mat image = new Mat(filePath);
                    SlideImages.Add(image);
                    var thumbnailSize = CalcThumbnailSize(thumbnailWidth, thumbnailHeight, image);
                    Mat thumbnail = new Mat(thumbnailSize.Y, thumbnailSize.X, MatType.CV_32FC3);
                    Cv2.Resize(image, thumbnail, thumbnail.Size());
                    ThumbnailImages.Add(thumbnail);
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                return -1;
            }
            slidenum = SlideImages.Count();
            return slidenum;
        }

        public System.Drawing.Image GetThumbnailImage(int index)
        {
            if (index < 0 && index > SlideNum) return null;

            System.Drawing.Image outputImage = BitmapConverter.ToBitmap(ThumbnailImages[index]);
            return outputImage;
        }

    }
}
