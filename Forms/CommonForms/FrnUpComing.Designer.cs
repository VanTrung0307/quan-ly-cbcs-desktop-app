namespace QuanLyCBCS.Forms.CommonForms
{
    partial class FrnUpComing
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.PictureBox picUpcoming;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (picUpcoming != null)
                {
                    System.Drawing.Image image = picUpcoming.Image;
                    picUpcoming.Image = null;
                    image?.Dispose();
                }

                if (components != null)
                    components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.picUpcoming = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picUpcoming)).BeginInit();
            this.SuspendLayout();
            //
            // picUpcoming
            //
            this.picUpcoming.BackColor = System.Drawing.Color.FromArgb(239, 247, 255);
            this.picUpcoming.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picUpcoming.Margin = System.Windows.Forms.Padding.Empty;
            this.picUpcoming.Location = new System.Drawing.Point(0, 0);
            this.picUpcoming.Name = "picUpcoming";
            this.picUpcoming.Size = new System.Drawing.Size(1200, 675);
            this.picUpcoming.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picUpcoming.TabIndex = 0;
            this.picUpcoming.TabStop = false;
            //
            // FrnUpComing
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(239, 247, 255);
            this.Padding = System.Windows.Forms.Padding.Empty;
            this.Margin = System.Windows.Forms.Padding.Empty;
            this.ClientSize = new System.Drawing.Size(1200, 675);
            this.Controls.Add(this.picUpcoming);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrnUpComing";
            this.ShowInTaskbar = false;
            this.Text = "Sẽ được cập nhật sớm";
            ((System.ComponentModel.ISupportInitialize)(this.picUpcoming)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion
    }
}
