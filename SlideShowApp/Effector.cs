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
        public Mat previewImage { get; }
        public Mat nextImage { get; }

        public int width_ { get; }
        public int height_ { get; }

        public Effector(Mat previewImage, Mat nextImage, )
    }
}
