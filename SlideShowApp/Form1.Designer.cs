namespace SlideShowApp
{
    partial class SlideShowApp
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.buCreateSlideImage = new MaterialSkin.Controls.MaterialFlatButton();
            this.pbThumbnail = new System.Windows.Forms.PictureBox();
            this.buCreateEffect = new MaterialSkin.Controls.MaterialFlatButton();
            ((System.ComponentModel.ISupportInitialize)(this.pbThumbnail)).BeginInit();
            this.SuspendLayout();
            // 
            // buCreateSlideImage
            // 
            this.buCreateSlideImage.AutoSize = true;
            this.buCreateSlideImage.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buCreateSlideImage.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.buCreateSlideImage.Depth = 0;
            this.buCreateSlideImage.Location = new System.Drawing.Point(22, 390);
            this.buCreateSlideImage.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buCreateSlideImage.MouseState = MaterialSkin.MouseState.HOVER;
            this.buCreateSlideImage.Name = "buCreateSlideImage";
            this.buCreateSlideImage.Primary = false;
            this.buCreateSlideImage.Size = new System.Drawing.Size(143, 36);
            this.buCreateSlideImage.TabIndex = 0;
            this.buCreateSlideImage.Text = "スライド画像生成";
            this.buCreateSlideImage.UseVisualStyleBackColor = false;
            this.buCreateSlideImage.Click += new System.EventHandler(this.buCreateSlideImage_Click);
            // 
            // pbThumbnail
            // 
            this.pbThumbnail.Location = new System.Drawing.Point(161, 109);
            this.pbThumbnail.Name = "pbThumbnail";
            this.pbThumbnail.Size = new System.Drawing.Size(434, 243);
            this.pbThumbnail.TabIndex = 1;
            this.pbThumbnail.TabStop = false;
            // 
            // buCreateEffect
            // 
            this.buCreateEffect.AutoSize = true;
            this.buCreateEffect.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buCreateEffect.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.buCreateEffect.Depth = 0;
            this.buCreateEffect.Location = new System.Drawing.Point(203, 390);
            this.buCreateEffect.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buCreateEffect.MouseState = MaterialSkin.MouseState.HOVER;
            this.buCreateEffect.Name = "buCreateEffect";
            this.buCreateEffect.Primary = false;
            this.buCreateEffect.Size = new System.Drawing.Size(144, 45);
            this.buCreateEffect.TabIndex = 2;
            this.buCreateEffect.Text = "エフェクト生成";
            this.buCreateEffect.UseVisualStyleBackColor = false;
            this.buCreateEffect.Click += new System.EventHandler(this.buCreateEffect_Click);
            // 
            // SlideShowApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buCreateEffect);
            this.Controls.Add(this.pbThumbnail);
            this.Controls.Add(this.buCreateSlideImage);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "SlideShowApp";
            this.Text = "SlideShow app";
            this.Load += new System.EventHandler(this.SlideShowApp_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbThumbnail)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MaterialSkin.Controls.MaterialFlatButton buCreateSlideImage;
        private System.Windows.Forms.PictureBox pbThumbnail;
        private MaterialSkin.Controls.MaterialFlatButton buCreateEffect;
    }
}

