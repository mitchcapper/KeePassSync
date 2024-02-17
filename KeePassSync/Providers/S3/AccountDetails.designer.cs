namespace KeePassSync.Providers.S3 {
	partial class AccountDetails {
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.label1 = new System.Windows.Forms.Label();
			this.txtAccessKey = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtBucketName = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtSecretAccessKey = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.cbxDailyBackups = new System.Windows.Forms.CheckBox();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 25);
			this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(157, 25);
			this.label1.TabIndex = 0;
			this.label1.Text = "Access Key ID:";
			// 
			// txtAccessKey
			// 
			this.txtAccessKey.Location = new System.Drawing.Point(212, 19);
			this.txtAccessKey.Margin = new System.Windows.Forms.Padding(26, 6, 6, 6);
			this.txtAccessKey.Name = "txtAccessKey";
			this.txtAccessKey.Size = new System.Drawing.Size(390, 31);
			this.txtAccessKey.TabIndex = 2;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 125);
			this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(146, 25);
			this.label3.TabIndex = 4;
			this.label3.Text = "Bucket Name:";
			// 
			// txtBucketName
			// 
			this.txtBucketName.Location = new System.Drawing.Point(162, 119);
			this.txtBucketName.Margin = new System.Windows.Forms.Padding(6);
			this.txtBucketName.Name = "txtBucketName";
			this.txtBucketName.Size = new System.Drawing.Size(440, 31);
			this.txtBucketName.TabIndex = 4;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(6, 75);
			this.label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(199, 25);
			this.label4.TabIndex = 6;
			this.label4.Text = "Access Key Secret:";
			// 
			// txtSecretAccessKey
			// 
			this.txtSecretAccessKey.Location = new System.Drawing.Point(212, 69);
			this.txtSecretAccessKey.Margin = new System.Windows.Forms.Padding(6);
			this.txtSecretAccessKey.Name = "txtSecretAccessKey";
			this.txtSecretAccessKey.PasswordChar = '*';
			this.txtSecretAccessKey.Size = new System.Drawing.Size(390, 31);
			this.txtSecretAccessKey.TabIndex = 3;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(16, 232);
			this.label6.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.label6.MaximumSize = new System.Drawing.Size(600, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(569, 50);
			this.label6.TabIndex = 9;
			this.label6.Text = "If the bucket does not exist it will be greated automatically (assuming this key " +
    "has create rights). ";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 170);
			this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(520, 25);
			this.label2.TabIndex = 11;
			this.label2.Text = "Create Daily Backups (last 30 days, 1 backup a day):";
			// 
			// cbxDailyBackups
			// 
			this.cbxDailyBackups.AutoSize = true;
			this.cbxDailyBackups.Location = new System.Drawing.Point(574, 169);
			this.cbxDailyBackups.Margin = new System.Windows.Forms.Padding(6);
			this.cbxDailyBackups.Name = "cbxDailyBackups";
			this.cbxDailyBackups.Size = new System.Drawing.Size(28, 27);
			this.cbxDailyBackups.TabIndex = 12;
			this.cbxDailyBackups.UseVisualStyleBackColor = true;
			// 
			// linkLabel1
			// 
			this.linkLabel1.AutoSize = true;
			this.linkLabel1.Location = new System.Drawing.Point(6, 199);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(184, 25);
			this.linkLabel1.TabIndex = 13;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "Setup Instructions";
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// AccountDetails
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.linkLabel1);
			this.Controls.Add(this.cbxDailyBackups);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.txtSecretAccessKey);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.txtBucketName);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtAccessKey);
			this.Controls.Add(this.label1);
			this.Margin = new System.Windows.Forms.Padding(6);
			this.Name = "AccountDetails";
			this.Size = new System.Drawing.Size(624, 296);
			this.Load += new System.EventHandler(this.AccountDetails_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtAccessKey;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtBucketName;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtSecretAccessKey;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox cbxDailyBackups;
		private System.Windows.Forms.LinkLabel linkLabel1;
	}
}
