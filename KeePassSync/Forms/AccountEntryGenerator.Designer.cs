namespace KeePassSync.Forms {
	partial class AccountEntryGenerator {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.m_BannerImage = new System.Windows.Forms.PictureBox();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.m_CustomPanel = new System.Windows.Forms.GroupBox();
			this.panel1 = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this.m_BannerImage)).BeginInit();
			this.m_CustomPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_BannerImage
			// 
			this.m_BannerImage.Cursor = System.Windows.Forms.Cursors.Default;
			this.m_BannerImage.Dock = System.Windows.Forms.DockStyle.Top;
			this.m_BannerImage.Location = new System.Drawing.Point(0, 0);
			this.m_BannerImage.Name = "m_BannerImage";
			this.m_BannerImage.Size = new System.Drawing.Size(335, 60);
			this.m_BannerImage.TabIndex = 0;
			this.m_BannerImage.TabStop = false;
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Location = new System.Drawing.Point(167, 294);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "Save";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.Location = new System.Drawing.Point(248, 294);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 1;
			this.button2.Text = "Cancel";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// m_CustomPanel
			// 
			this.m_CustomPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_CustomPanel.Controls.Add(this.panel1);
			this.m_CustomPanel.Location = new System.Drawing.Point(12, 66);
			this.m_CustomPanel.Name = "m_CustomPanel";
			this.m_CustomPanel.Size = new System.Drawing.Size(311, 222);
			this.m_CustomPanel.TabIndex = 2;
			this.m_CustomPanel.TabStop = false;
			this.m_CustomPanel.Text = "Account Details";
			this.m_CustomPanel.Enter += new System.EventHandler(this.m_CustomPanel_Enter);
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(3, 16);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(305, 203);
			this.panel1.TabIndex = 0;
			// 
			// AccountEntryGenerator
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(335, 329);
			this.Controls.Add(this.m_CustomPanel);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.m_BannerImage);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "AccountEntryGenerator";
			this.Text = "Entry Generator";
			this.Load += new System.EventHandler(this.EntryGenerator_Load);
			((System.ComponentModel.ISupportInitialize)(this.m_BannerImage)).EndInit();
			this.m_CustomPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox m_BannerImage;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.GroupBox m_CustomPanel;
		private System.Windows.Forms.Panel panel1;
	}
}