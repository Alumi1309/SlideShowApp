using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using OpenCvSharp;

namespace SlideShowApp
{
    public partial class SlideShowApp : MaterialForm
    {
        public SlideShowApp()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        }

        private void SlideShowApp_Load(object sender, EventArgs e)
        {
            string[] files = System.IO.Directory.GetFiles(
                @"Z:\Share\_2021\2021.12\20211214_家族写真", "*", System.IO.SearchOption.AllDirectories);
            
            List<string> filelist = new List<string>();
            filelist.AddRange(files);

            using (Mat slideimg = new Mat(900, 1600, MatType.CV_32FC3))
            {
                SlideImageCreator.Create(1, filelist, slideimg);
                Cv2.ImShow("main", slideimg);
                Cv2.WaitKey(0);
            }
        }
    }
}
