namespace KeePassSync.Forms
{
    partial class SelectDatabaseForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.m_lbDatabases = new System.Windows.Forms.ListBox();
            this.m_btnOk = new System.Windows.Forms.Button();
            this.m_btnCancel = new System.Windows.Forms.Button();
            this.m_BannerImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.m_BannerImage)).BeginInit();
            this.SuspendLayout();
            // 
            // m_lbDatabases
            // 
            this.m_lbDatabases.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_lbDatabases.FormattingEnabled = true;
            this.m_lbDatabases.Location = new System.Drawing.Point(12, 66);
            this.m_lbDatabases.Name = "m_lbDatabases";
            this.m_lbDatabases.Size = new System.Drawing.Size(288, 212);
            this.m_lbDatabases.TabIndex = 0;
            this.m_lbDatabases.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OnLbDatabasesMouseDoubleClick);
            // 
            // m_btnOk
            // 
            this.m_btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_btnOk.Location = new System.Drawing.Point(144, 292);
            this.m_btnOk.Name = "m_btnOk";
            this.m_btnOk.Size = new System.Drawing.Size(75, 23);
            this.m_btnOk.TabIndex = 2;
            this.m_btnOk.Text = "OK";
            this.m_btnOk.UseVisualStyleBackColor = true;
            this.m_btnOk.Click += new System.EventHandler(this.OnBtnOkClick);
            // 
            // m_btnCancel
            // 
            this.m_btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_btnCancel.Location = new System.Drawing.Point(225, 292);
            this.m_btnCancel.Name = "m_btnCancel";
            this.m_btnCancel.Size = new System.Drawing.Size(75, 23);
            this.m_btnCancel.TabIndex = 3;
            this.m_btnCancel.Text = "Cancel";
            this.m_btnCancel.UseVisualStyleBackColor = true;
            this.m_btnCancel.Click += new System.EventHandler(this.OnBtnCancelClick);
            // 
            // m_BannerImage
            // 
            this.m_BannerImage.Dock = System.Windows.Forms.DockStyle.Top;
            this.m_BannerImage.Location = new System.Drawing.Point(0, 0);
            this.m_BannerImage.Name = "m_BannerImage";
            this.m_BannerImage.Size = new System.Drawing.Size(312, 60);
            this.m_BannerImage.TabIndex = 4;
            this.m_BannerImage.TabStop = false;
            // 
            // SelectDatabaseForm
            // 
            this.AcceptButton = this.m_btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.m_btnCancel;
            this.ClientSize = new System.Drawing.Size(312, 327);
            this.Controls.Add(this.m_BannerImage);
            this.Controls.Add(this.m_btnCancel);
            this.Controls.Add(this.m_btnOk);
            this.Controls.Add(this.m_lbDatabases);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectDatabaseForm";
            this.ShowInTaskbar = false;
            this.Text = "Select Database";
            this.Load += new System.EventHandler(this.SelectDatabaseForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.m_BannerImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox m_lbDatabases;
        private System.Windows.Forms.Button m_btnOk;
        private System.Windows.Forms.Button m_btnCancel;
        private System.Windows.Forms.PictureBox m_BannerImage;
    }
}