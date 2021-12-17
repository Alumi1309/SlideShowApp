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
        int slideNum_ = 0;
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
        }

        private void buCreateSlideImage_Click(object sender, EventArgs e)
        {
            string[] files = System.IO.Directory.GetFiles(
            //    @"Z:\Share\_2021\2021.12\20211214_家族写真", "*", System.IO.SearchOption.AllDirectories);
                @"C:\Users\S137092\Pictures\pic", "*", System.IO.SearchOption.AllDirectories);

            List<string> filelist = new List<string>();
            filelist.AddRange(files);

            var slideImageCreator = new SlideImageCreator(filelist);
            slideImageCreator.LoadPhoto(1600, 900);

            using (Mat slideimg = new Mat(900, 1600, MatType.CV_32FC3))
            {
                for (int i = 0; i < filelist.Count; i++)
                {
                    slideImageCreator.Create(i, filelist, slideimg);
                    Cv2.ImShow("main", slideimg);
                    if(System.IO.Directory.Exists(@"/pic/") == false)
                    {
                        System.IO.Directory.CreateDirectory(@"/pic/");
                    }
                    Cv2.ImWrite("/pic/" + i.ToString() + ".jpg", slideimg);
                    Cv2.WaitKey(1000);
                }
            }
        }

        private async Task DispEffect(SlideImageList imageList, int index, int effectSecond, int fps = 30)
        {
            int frameLength = fps * effectSecond;
            await Task.Run(async () =>
            {
                if (slideNum_ == -1) return;
                else if (index + 1 > imageList.SlideNum) return;


                // 次に処理するフレームの時刻（初回は即処理するので初期値は現在時刻をセット）
                double nextFrame = (double)System.Environment.TickCount;
                // フレームを処理する周期（1/60秒）
                float period = 1000f / 30f;

                for (int frame = 0; frame < frameLength;)
                {
                    // 現在の時刻を取得
                    double tickCount = (double)System.Environment.TickCount;

                    // 次に処理するフレームの時刻まで間がある場合は、処理をスキップする
                    if (tickCount < nextFrame)
                    {
                        // 1ms以上の間があるか？
                        if (nextFrame - tickCount > 1)
                        {
                            // Sleepする
                            await Task.Delay((int)(nextFrame - tickCount));
                        }
                        // Windowsメッセージを処理させる
                        Application.DoEvents();
                        // 残りの処理をスキップする
                        continue;
                    }


                    //
                    // ここで描画処理を行う。
                    //
                    var image = EffectCreator.GetEffectedImage(imageList.ThumbnailImages, index , frameLength, frame);
                    this.Invoke((Action)(() => { pbThumbnail.Image = image; }));

                    // 次のフレームの時刻を計算する
                    nextFrame += period;
                    frame++;
                }

            });
        }

        private async void buCreateEffect_Click(object sender, EventArgs e)
        {
            SlideImageList imageList = new SlideImageList();
            imageList.LoadImages(pbThumbnail.Width, pbThumbnail.Height);

            for(int i=0; i<imageList.SlideNum-1; i++)
            {
                var image = imageList.GetThumbnailImage(i);
                pbThumbnail.Image = image;
                await Task.Delay(4000);
                await DispEffect(imageList, i, 3);
            }


        }
    }
}
