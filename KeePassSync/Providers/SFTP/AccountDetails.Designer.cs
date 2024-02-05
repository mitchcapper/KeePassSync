namespace KeePassSync.Providers.SFTP
{
    partial class AccountDetails
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AccountDetails));
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtHost = new System.Windows.Forms.TextBox();
			this.txtUsername = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtDirectory = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.checkPaegent = new System.Windows.Forms.CheckBox();
			this.txtPort = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.txtExecPath = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.checkKeepassRoot = new System.Windows.Forms.CheckBox();
			this.label9 = new System.Windows.Forms.Label();
			this.checkDebug = new System.Windows.Forms.CheckBox();
			this.comboTimeout = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 33);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(58, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Username:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 6);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(32, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Host:";
			// 
			// txtHost
			// 
			this.txtHost.Location = new System.Drawing.Point(64, 3);
			this.txtHost.Name = "txtHost";
			this.txtHost.Size = new System.Drawing.Size(157, 20);
			this.txtHost.TabIndex = 1;
			// 
			// txtUsername
			// 
			this.txtUsername.Location = new System.Drawing.Point(64, 30);
			this.txtUsername.Name = "txtUsername";
			this.txtUsername.Size = new System.Drawing.Size(157, 20);
			this.txtUsername.TabIndex = 3;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(3, 85);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(52, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "Directory:";
			// 
			// txtDirectory
			// 
			this.txtDirectory.Location = new System.Drawing.Point(64, 82);
			this.txtDirectory.Name = "txtDirectory";
			this.txtDirectory.Size = new System.Drawing.Size(239, 20);
			this.txtDirectory.TabIndex = 6;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(3, 59);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(56, 13);
			this.label4.TabIndex = 6;
			this.label4.Text = "Password:";
			// 
			// txtPassword
			// 
			this.txtPassword.Location = new System.Drawing.Point(64, 56);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.PasswordChar = '*';
			this.txtPassword.Size = new System.Drawing.Size(157, 20);
			this.txtPassword.TabIndex = 4;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(21, 211);
			this.label6.MaximumSize = new System.Drawing.Size(350, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(341, 91);
			this.label6.TabIndex = 9;
			this.label6.Text = resources.GetString("label6.Text");
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.Location = new System.Drawing.Point(3, 196);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(44, 13);
			this.label7.TabIndex = 10;
			this.label7.Text = "Notes:";
			// 
			// checkPaegent
			// 
			this.checkPaegent.AutoSize = true;
			this.checkPaegent.Location = new System.Drawing.Point(228, 57);
			this.checkPaegent.Name = "checkPaegent";
			this.checkPaegent.Size = new System.Drawing.Size(126, 17);
			this.checkPaegent.TabIndex = 5;
			this.checkPaegent.Text = "Use Paegent Instead";
			this.checkPaegent.UseVisualStyleBackColor = true;
			this.checkPaegent.CheckedChanged += new System.EventHandler(this.checkPaegent_CheckedChanged);
			// 
			// txtPort
			// 
			this.txtPort.Location = new System.Drawing.Point(259, 3);
			this.txtPort.Name = "txtPort";
			this.txtPort.Size = new System.Drawing.Size(32, 20);
			this.txtPort.TabIndex = 2;
			this.txtPort.Text = "22";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(227, 6);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(29, 13);
			this.label5.TabIndex = 12;
			this.label5.Text = "Port:";
			// 
			// txtExecPath
			// 
			this.txtExecPath.Location = new System.Drawing.Point(114, 112);
			this.txtExecPath.Name = "txtExecPath";
			this.txtExecPath.Size = new System.Drawing.Size(164, 20);
			this.txtExecPath.TabIndex = 7;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(3, 115);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(105, 13);
			this.label8.TabIndex = 14;
			this.label8.Text = "PLINK/PSFTP Path:";
			// 
			// checkKeepassRoot
			// 
			this.checkKeepassRoot.AutoSize = true;
			this.checkKeepassRoot.Location = new System.Drawing.Point(286, 114);
			this.checkKeepassRoot.Name = "checkKeepassRoot";
			this.checkKeepassRoot.Size = new System.Drawing.Size(93, 17);
			this.checkKeepassRoot.TabIndex = 8;
			this.checkKeepassRoot.Text = "Keepass Root";
			this.checkKeepassRoot.UseVisualStyleBackColor = true;
			this.checkKeepassRoot.CheckedChanged += new System.EventHandler(this.checkKeepassRoot_CheckedChanged);
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(3, 142);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(93, 13);
			this.label9.TabIndex = 17;
			this.label9.Text = "Timeout Seconds:";
			// 
			// checkDebug
			// 
			this.checkDebug.AutoSize = true;
			this.checkDebug.Location = new System.Drawing.Point(9, 170);
			this.checkDebug.Name = "checkDebug";
			this.checkDebug.Size = new System.Drawing.Size(88, 17);
			this.checkDebug.TabIndex = 10;
			this.checkDebug.Text = "Debug Mode";
			this.checkDebug.UseVisualStyleBackColor = true;
			// 
			// comboTimeout
			// 
			this.comboTimeout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboTimeout.FormattingEnabled = true;
			this.comboTimeout.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31",
            "32",
            "33",
            "34",
            "35",
            "36",
            "37",
            "38",
            "39",
            "40"});
			this.comboTimeout.Location = new System.Drawing.Point(114, 138);
			this.comboTimeout.Name = "comboTimeout";
			this.comboTimeout.Size = new System.Drawing.Size(57, 21);
			this.comboTimeout.TabIndex = 9;
			// 
			// AccountDetails
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.comboTimeout);
			this.Controls.Add(this.checkDebug);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.checkKeepassRoot);
			this.Controls.Add(this.txtExecPath);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.txtPort);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.checkPaegent);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.txtPassword);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.txtDirectory);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtUsername);
			this.Controls.Add(this.txtHost);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "AccountDetails";
			this.Size = new System.Drawing.Size(381, 312);
			this.Load += new System.EventHandler(this.AccountDetails_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDirectory;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
		private System.Windows.Forms.CheckBox checkPaegent;
		private System.Windows.Forms.TextBox txtPort;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtExecPath;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.CheckBox checkKeepassRoot;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.CheckBox checkDebug;
		private System.Windows.Forms.ComboBox comboTimeout;
    }
}
