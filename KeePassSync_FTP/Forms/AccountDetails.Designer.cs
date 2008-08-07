namespace KeePassSync_FTP.Forms
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( AccountDetails ) );
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
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 3, 32 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 58, 13 );
            this.label1.TabIndex = 0;
            this.label1.Text = "Username:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point( 3, 6 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 32, 13 );
            this.label2.TabIndex = 1;
            this.label2.Text = "Host:";
            // 
            // txtHost
            // 
            this.txtHost.Location = new System.Drawing.Point( 64, 3 );
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size( 157, 20 );
            this.txtHost.TabIndex = 1;
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point( 64, 29 );
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size( 157, 20 );
            this.txtUsername.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point( 3, 84 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 52, 13 );
            this.label3.TabIndex = 4;
            this.label3.Text = "Directory:";
            // 
            // txtDirectory
            // 
            this.txtDirectory.Location = new System.Drawing.Point( 64, 81 );
            this.txtDirectory.Name = "txtDirectory";
            this.txtDirectory.Size = new System.Drawing.Size( 239, 20 );
            this.txtDirectory.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point( 3, 58 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 56, 13 );
            this.label4.TabIndex = 6;
            this.label4.Text = "Password:";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point( 64, 55 );
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size( 157, 20 );
            this.txtPassword.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point( 21, 127 );
            this.label6.MaximumSize = new System.Drawing.Size( 350, 0 );
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size( 332, 65 );
            this.label6.TabIndex = 9;
            this.label6.Text = resources.GetString( "label6.Text" );
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.label7.Location = new System.Drawing.Point( 3, 114 );
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size( 44, 13 );
            this.label7.TabIndex = 10;
            this.label7.Text = "Notes:";
            // 
            // AccountDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.label7 );
            this.Controls.Add( this.label6 );
            this.Controls.Add( this.txtPassword );
            this.Controls.Add( this.label4 );
            this.Controls.Add( this.txtDirectory );
            this.Controls.Add( this.label3 );
            this.Controls.Add( this.txtUsername );
            this.Controls.Add( this.txtHost );
            this.Controls.Add( this.label2 );
            this.Controls.Add( this.label1 );
            this.Name = "AccountDetails";
            this.Size = new System.Drawing.Size( 372, 206 );
            this.ResumeLayout( false );
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
    }
}
