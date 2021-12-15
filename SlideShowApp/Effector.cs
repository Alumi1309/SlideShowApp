using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace SlideShowApp.ImageMaker
{
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
