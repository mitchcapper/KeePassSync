namespace KeePassSync.Forms {
	partial class OpenDbForm {
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
			this.label1 = new System.Windows.Forms.Label();
			this.m_cboProvider = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.m_btnStoreCreate = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.m_BannerImage)).BeginInit();
			this.SuspendLayout();
			// 
			// m_BannerImage
			// 
			this.m_BannerImage.Dock = System.Windows.Forms.DockStyle.Top;
			this.m_BannerImage.Location = new System.Drawing.Point(0, 0);
			this.m_BannerImage.Name = "m_BannerImage";
			this.m_BannerImage.Size = new System.Drawing.Size(378, 60);
			this.m_BannerImage.TabIndex = 0;
			this.m_BannerImage.TabStop = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 63);
			this.label1.MaximumSize = new System.Drawing.Size(350, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(81, 13);
			this.label1.TabIndex = 12;
			this.label1.Text = "Select provider:";
			// 
			// m_cboProvider
			// 
			this.m_cboProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_cboProvider.FormattingEnabled = true;
			this.m_cboProvider.Location = new System.Drawing.Point(12, 79);
			this.m_cboProvider.Name = "m_cboProvider";
			this.m_cboProvider.Size = new System.Drawing.Size(354, 21);
			this.m_cboProvider.TabIndex = 11;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 138);
			this.label2.MaximumSize = new System.Drawing.Size(350, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(349, 26);
			this.label2.TabIndex = 12;
			this.label2.Text = "Note: To connect to the provider, a temporary KeePass entry is needed.  This will" +
				" not be stored in any database.";
			// 
			// m_btnStoreCreate
			// 
			this.m_btnStoreCreate.Location = new System.Drawing.Point(12, 106);
			this.m_btnStoreCreate.Name = "m_btnStoreCreate";
			this.m_btnStoreCreate.Size = new System.Drawing.Size(101, 23);
			this.m_btnStoreCreate.TabIndex = 0;
			this.m_btnStoreCreate.Text = "Create Entry...";
			this.m_btnStoreCreate.UseVisualStyleBackColor = true;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(210, 177);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 13;
			this.button1.Text = "OK";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(291, 177);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 14;
			this.button2.Text = "Cancel";
			this.button2.UseVisualStyleBackColor = true;
			// 
			// OpenDbForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(378, 212);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.m_btnStoreCreate);
			this.Controls.Add(this.m_cboProvider);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.m_BannerImage);
			this.Name = "OpenDbForm";
			this.Text = "OpenDbForm";
			this.Load += new System.EventHandler(this.OpenDbForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.m_BannerImage)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox m_BannerImage;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox m_cboProvider;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button m_btnStoreCreate;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
	}
}