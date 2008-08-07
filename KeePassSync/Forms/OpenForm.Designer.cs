namespace KeePassSync.Forms
{
    partial class OpenForm
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
            this.m_BannerImage = new System.Windows.Forms.PictureBox();
            this.m_btnOk = new System.Windows.Forms.Button();
            this.m_btnCancel = new System.Windows.Forms.Button();
            this.m_CustomPanel = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.m_grpOnlineProvider = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.m_cboProvider = new System.Windows.Forms.ComboBox();
            this.m_lnkCreateAccount = new System.Windows.Forms.LinkLabel();
            ( (System.ComponentModel.ISupportInitialize)( this.m_BannerImage ) ).BeginInit();
            this.m_CustomPanel.SuspendLayout();
            this.m_grpOnlineProvider.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_BannerImage
            // 
            this.m_BannerImage.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.m_BannerImage.Location = new System.Drawing.Point( 0, 0 );
            this.m_BannerImage.Name = "m_BannerImage";
            this.m_BannerImage.Size = new System.Drawing.Size( 390, 60 );
            this.m_BannerImage.TabIndex = 0;
            this.m_BannerImage.TabStop = false;
            // 
            // m_btnOk
            // 
            this.m_btnOk.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.m_btnOk.Location = new System.Drawing.Point( 222, 388 );
            this.m_btnOk.Name = "m_btnOk";
            this.m_btnOk.Size = new System.Drawing.Size( 75, 23 );
            this.m_btnOk.TabIndex = 5;
            this.m_btnOk.Text = "OK";
            this.m_btnOk.UseVisualStyleBackColor = true;
            this.m_btnOk.Click += new System.EventHandler( this.m_btnOk_Click );
            // 
            // m_btnCancel
            // 
            this.m_btnCancel.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.m_btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_btnCancel.Location = new System.Drawing.Point( 303, 388 );
            this.m_btnCancel.Name = "m_btnCancel";
            this.m_btnCancel.Size = new System.Drawing.Size( 75, 23 );
            this.m_btnCancel.TabIndex = 6;
            this.m_btnCancel.Text = "Cancel";
            this.m_btnCancel.UseVisualStyleBackColor = true;
            this.m_btnCancel.Click += new System.EventHandler( this.m_btnCancel_Click );
            // 
            // m_CustomPanel
            // 
            this.m_CustomPanel.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                        | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.m_CustomPanel.Controls.Add( this.panel1 );
            this.m_CustomPanel.Location = new System.Drawing.Point( 12, 178 );
            this.m_CustomPanel.Name = "m_CustomPanel";
            this.m_CustomPanel.Size = new System.Drawing.Size( 365, 204 );
            this.m_CustomPanel.TabIndex = 8;
            this.m_CustomPanel.TabStop = false;
            this.m_CustomPanel.Text = "Account Details";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point( 3, 16 );
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size( 359, 185 );
            this.panel1.TabIndex = 0;
            // 
            // m_grpOnlineProvider
            // 
            this.m_grpOnlineProvider.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.m_grpOnlineProvider.Controls.Add( this.label1 );
            this.m_grpOnlineProvider.Controls.Add( this.m_cboProvider );
            this.m_grpOnlineProvider.Controls.Add( this.m_lnkCreateAccount );
            this.m_grpOnlineProvider.Location = new System.Drawing.Point( 12, 66 );
            this.m_grpOnlineProvider.Name = "m_grpOnlineProvider";
            this.m_grpOnlineProvider.Size = new System.Drawing.Size( 365, 106 );
            this.m_grpOnlineProvider.TabIndex = 14;
            this.m_grpOnlineProvider.TabStop = false;
            this.m_grpOnlineProvider.Text = "Online Provider";
            // 
            // label1
            // 
            this.label1.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 8, 18 );
            this.label1.MaximumSize = new System.Drawing.Size( 350, 0 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 337, 26 );
            this.label1.TabIndex = 12;
            this.label1.Text = "These are online services that store your data.  Most are free, be sure you choos" +
                "e a provider from a source you trust.";
            // 
            // m_cboProvider
            // 
            this.m_cboProvider.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.m_cboProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cboProvider.FormattingEnabled = true;
            this.m_cboProvider.Location = new System.Drawing.Point( 6, 56 );
            this.m_cboProvider.Name = "m_cboProvider";
            this.m_cboProvider.Size = new System.Drawing.Size( 353, 21 );
            this.m_cboProvider.TabIndex = 11;
            this.m_cboProvider.SelectionChangeCommitted += new System.EventHandler( this.OnCboProviderSelectionChangeCommitted );
            this.m_cboProvider.SelectedIndexChanged += new System.EventHandler( this.OnCboProviderSelectionChangeCommitted );
            // 
            // m_lnkCreateAccount
            // 
            this.m_lnkCreateAccount.AutoSize = true;
            this.m_lnkCreateAccount.Enabled = false;
            this.m_lnkCreateAccount.Location = new System.Drawing.Point( 6, 80 );
            this.m_lnkCreateAccount.Name = "m_lnkCreateAccount";
            this.m_lnkCreateAccount.Size = new System.Drawing.Size( 104, 13 );
            this.m_lnkCreateAccount.TabIndex = 8;
            this.m_lnkCreateAccount.TabStop = true;
            this.m_lnkCreateAccount.Text = "Create an account...";
            this.m_lnkCreateAccount.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler( this.OnLblCreateAccountClicked );
            // 
            // OpenForm
            // 
            this.AcceptButton = this.m_btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.m_btnCancel;
            this.ClientSize = new System.Drawing.Size( 390, 423 );
            this.Controls.Add( this.m_grpOnlineProvider );
            this.Controls.Add( this.m_CustomPanel );
            this.Controls.Add( this.m_btnCancel );
            this.Controls.Add( this.m_btnOk );
            this.Controls.Add( this.m_BannerImage );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OpenForm";
            this.ShowInTaskbar = false;
            this.Text = "Open Online Database";
            this.Load += new System.EventHandler( this.OpenForm_Load );
            ( (System.ComponentModel.ISupportInitialize)( this.m_BannerImage ) ).EndInit();
            this.m_CustomPanel.ResumeLayout( false );
            this.m_grpOnlineProvider.ResumeLayout( false );
            this.m_grpOnlineProvider.PerformLayout();
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.PictureBox m_BannerImage;
        private System.Windows.Forms.Button m_btnOk;
        private System.Windows.Forms.Button m_btnCancel;
        private System.Windows.Forms.GroupBox m_CustomPanel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox m_grpOnlineProvider;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox m_cboProvider;
        private System.Windows.Forms.LinkLabel m_lnkCreateAccount;
    }
}