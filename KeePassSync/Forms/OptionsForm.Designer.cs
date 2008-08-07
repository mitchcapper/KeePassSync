namespace KeePassSync.Forms
{
    partial class OptionsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( OptionsForm ) );
            this.m_grpMergeOptions = new System.Windows.Forms.GroupBox();
            this.m_lblMergeDesc = new System.Windows.Forms.Label();
            this.m_radioCreateNew = new System.Windows.Forms.RadioButton();
            this.m_radioKeepExisting = new System.Windows.Forms.RadioButton();
            this.m_radioOverwrite = new System.Windows.Forms.RadioButton();
            this.m_radioOverwriteNewer = new System.Windows.Forms.RadioButton();
            this.m_radioSynchronize = new System.Windows.Forms.RadioButton();
            this.m_btnOk = new System.Windows.Forms.Button();
            this.m_BannerImage = new System.Windows.Forms.PictureBox();
            this.m_lnkCreateAccount = new System.Windows.Forms.LinkLabel();
            this.m_grpAutosave = new System.Windows.Forms.GroupBox();
            this.m_cboFrequency = new System.Windows.Forms.ComboBox();
            this.m_lblFrequency = new System.Windows.Forms.Label();
            this.m_checkAutosave = new System.Windows.Forms.CheckBox();
            this.m_btnCancel = new System.Windows.Forms.Button();
            this.m_cboProvider = new System.Windows.Forms.ComboBox();
            this.m_grpOnlineProvider = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.m_MainTab = new System.Windows.Forms.TabControl();
            this.m_TabAccount = new System.Windows.Forms.TabPage();
            this.m_grpAccountInfo = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.m_btnStoreCreate = new System.Windows.Forms.Button();
            this.m_TabGeneral = new System.Windows.Forms.TabPage();
            this.m_grpMergeOptions.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.m_BannerImage ) ).BeginInit();
            this.m_grpAutosave.SuspendLayout();
            this.m_grpOnlineProvider.SuspendLayout();
            this.m_MainTab.SuspendLayout();
            this.m_TabAccount.SuspendLayout();
            this.m_grpAccountInfo.SuspendLayout();
            this.m_TabGeneral.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_grpMergeOptions
            // 
            this.m_grpMergeOptions.Controls.Add( this.m_lblMergeDesc );
            this.m_grpMergeOptions.Controls.Add( this.m_radioCreateNew );
            this.m_grpMergeOptions.Controls.Add( this.m_radioKeepExisting );
            this.m_grpMergeOptions.Controls.Add( this.m_radioOverwrite );
            this.m_grpMergeOptions.Controls.Add( this.m_radioOverwriteNewer );
            this.m_grpMergeOptions.Controls.Add( this.m_radioSynchronize );
            this.m_grpMergeOptions.Location = new System.Drawing.Point( 6, 6 );
            this.m_grpMergeOptions.Name = "m_grpMergeOptions";
            this.m_grpMergeOptions.Size = new System.Drawing.Size( 365, 178 );
            this.m_grpMergeOptions.TabIndex = 5;
            this.m_grpMergeOptions.TabStop = false;
            this.m_grpMergeOptions.Text = "Merge Options";
            // 
            // m_lblMergeDesc
            // 
            this.m_lblMergeDesc.AutoSize = true;
            this.m_lblMergeDesc.Location = new System.Drawing.Point( 6, 16 );
            this.m_lblMergeDesc.MaximumSize = new System.Drawing.Size( 360, 0 );
            this.m_lblMergeDesc.Name = "m_lblMergeDesc";
            this.m_lblMergeDesc.Size = new System.Drawing.Size( 335, 26 );
            this.m_lblMergeDesc.TabIndex = 14;
            this.m_lblMergeDesc.Text = "When the database is synchronized, this specifies how the merge will occur.";
            // 
            // m_radioCreateNew
            // 
            this.m_radioCreateNew.AutoSize = true;
            this.m_radioCreateNew.Location = new System.Drawing.Point( 6, 59 );
            this.m_radioCreateNew.Name = "m_radioCreateNew";
            this.m_radioCreateNew.Size = new System.Drawing.Size( 116, 17 );
            this.m_radioCreateNew.TabIndex = 0;
            this.m_radioCreateNew.TabStop = true;
            this.m_radioCreateNew.Text = "Create New UUIDs";
            this.m_radioCreateNew.UseVisualStyleBackColor = true;
            // 
            // m_radioKeepExisting
            // 
            this.m_radioKeepExisting.AutoSize = true;
            this.m_radioKeepExisting.Location = new System.Drawing.Point( 6, 82 );
            this.m_radioKeepExisting.Name = "m_radioKeepExisting";
            this.m_radioKeepExisting.Size = new System.Drawing.Size( 89, 17 );
            this.m_radioKeepExisting.TabIndex = 1;
            this.m_radioKeepExisting.TabStop = true;
            this.m_radioKeepExisting.Text = "Keep Existing";
            this.m_radioKeepExisting.UseVisualStyleBackColor = true;
            // 
            // m_radioOverwrite
            // 
            this.m_radioOverwrite.AutoSize = true;
            this.m_radioOverwrite.Location = new System.Drawing.Point( 6, 105 );
            this.m_radioOverwrite.Name = "m_radioOverwrite";
            this.m_radioOverwrite.Size = new System.Drawing.Size( 70, 17 );
            this.m_radioOverwrite.TabIndex = 2;
            this.m_radioOverwrite.TabStop = true;
            this.m_radioOverwrite.Text = "Overwrite";
            this.m_radioOverwrite.UseVisualStyleBackColor = true;
            // 
            // m_radioOverwriteNewer
            // 
            this.m_radioOverwriteNewer.AutoSize = true;
            this.m_radioOverwriteNewer.Location = new System.Drawing.Point( 6, 128 );
            this.m_radioOverwriteNewer.Name = "m_radioOverwriteNewer";
            this.m_radioOverwriteNewer.Size = new System.Drawing.Size( 112, 17 );
            this.m_radioOverwriteNewer.TabIndex = 3;
            this.m_radioOverwriteNewer.TabStop = true;
            this.m_radioOverwriteNewer.Text = "Overwrite if Newer";
            this.m_radioOverwriteNewer.UseVisualStyleBackColor = true;
            // 
            // m_radioSynchronize
            // 
            this.m_radioSynchronize.AutoSize = true;
            this.m_radioSynchronize.Location = new System.Drawing.Point( 6, 151 );
            this.m_radioSynchronize.Name = "m_radioSynchronize";
            this.m_radioSynchronize.Size = new System.Drawing.Size( 83, 17 );
            this.m_radioSynchronize.TabIndex = 4;
            this.m_radioSynchronize.TabStop = true;
            this.m_radioSynchronize.Text = "Synchronize";
            this.m_radioSynchronize.UseVisualStyleBackColor = true;
            // 
            // m_btnOk
            // 
            this.m_btnOk.Location = new System.Drawing.Point( 238, 340 );
            this.m_btnOk.Name = "m_btnOk";
            this.m_btnOk.Size = new System.Drawing.Size( 75, 23 );
            this.m_btnOk.TabIndex = 7;
            this.m_btnOk.Text = "OK";
            this.m_btnOk.UseVisualStyleBackColor = true;
            this.m_btnOk.Click += new System.EventHandler( this.OnBtnOkClicked );
            // 
            // m_BannerImage
            // 
            this.m_BannerImage.Dock = System.Windows.Forms.DockStyle.Top;
            this.m_BannerImage.InitialImage = null;
            this.m_BannerImage.Location = new System.Drawing.Point( 0, 0 );
            this.m_BannerImage.Name = "m_BannerImage";
            this.m_BannerImage.Size = new System.Drawing.Size( 408, 60 );
            this.m_BannerImage.TabIndex = 6;
            this.m_BannerImage.TabStop = false;
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
            // m_grpAutosave
            // 
            this.m_grpAutosave.Controls.Add( this.m_cboFrequency );
            this.m_grpAutosave.Controls.Add( this.m_lblFrequency );
            this.m_grpAutosave.Controls.Add( this.m_checkAutosave );
            this.m_grpAutosave.Location = new System.Drawing.Point( 6, 190 );
            this.m_grpAutosave.Name = "m_grpAutosave";
            this.m_grpAutosave.Size = new System.Drawing.Size( 365, 50 );
            this.m_grpAutosave.TabIndex = 10;
            this.m_grpAutosave.TabStop = false;
            this.m_grpAutosave.Text = "Autosave";
            this.m_grpAutosave.Visible = false;
            // 
            // m_cboFrequency
            // 
            this.m_cboFrequency.Enabled = false;
            this.m_cboFrequency.FormattingEnabled = true;
            this.m_cboFrequency.Items.AddRange( new object[] {
            "1 minute",
            "5 mintues",
            "15 minutes",
            "30 minutes",
            "1 hour",
            "2 hours",
            "6 hours",
            "8 hours",
            "1 day"} );
            this.m_cboFrequency.Location = new System.Drawing.Point( 250, 17 );
            this.m_cboFrequency.Name = "m_cboFrequency";
            this.m_cboFrequency.Size = new System.Drawing.Size( 109, 21 );
            this.m_cboFrequency.TabIndex = 2;
            // 
            // m_lblFrequency
            // 
            this.m_lblFrequency.AutoSize = true;
            this.m_lblFrequency.Enabled = false;
            this.m_lblFrequency.Location = new System.Drawing.Point( 187, 20 );
            this.m_lblFrequency.Name = "m_lblFrequency";
            this.m_lblFrequency.Size = new System.Drawing.Size( 57, 13 );
            this.m_lblFrequency.TabIndex = 1;
            this.m_lblFrequency.Text = "Frequency";
            // 
            // m_checkAutosave
            // 
            this.m_checkAutosave.AutoSize = true;
            this.m_checkAutosave.Enabled = false;
            this.m_checkAutosave.Location = new System.Drawing.Point( 6, 19 );
            this.m_checkAutosave.Name = "m_checkAutosave";
            this.m_checkAutosave.Size = new System.Drawing.Size( 107, 17 );
            this.m_checkAutosave.TabIndex = 0;
            this.m_checkAutosave.Text = "Enable Autosave";
            this.m_checkAutosave.UseVisualStyleBackColor = true;
            // 
            // m_btnCancel
            // 
            this.m_btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_btnCancel.Location = new System.Drawing.Point( 319, 340 );
            this.m_btnCancel.Name = "m_btnCancel";
            this.m_btnCancel.Size = new System.Drawing.Size( 75, 23 );
            this.m_btnCancel.TabIndex = 8;
            this.m_btnCancel.Text = "Cancel";
            this.m_btnCancel.UseVisualStyleBackColor = true;
            this.m_btnCancel.Click += new System.EventHandler( this.OnBtnCancelClicked );
            // 
            // m_cboProvider
            // 
            this.m_cboProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cboProvider.FormattingEnabled = true;
            this.m_cboProvider.Location = new System.Drawing.Point( 6, 56 );
            this.m_cboProvider.Name = "m_cboProvider";
            this.m_cboProvider.Size = new System.Drawing.Size( 353, 21 );
            this.m_cboProvider.TabIndex = 11;
            this.m_cboProvider.SelectedIndexChanged += new System.EventHandler( this.OnCboProviderSelectionChangeCommitted );
            // 
            // m_grpOnlineProvider
            // 
            this.m_grpOnlineProvider.Controls.Add( this.label1 );
            this.m_grpOnlineProvider.Controls.Add( this.m_cboProvider );
            this.m_grpOnlineProvider.Controls.Add( this.m_lnkCreateAccount );
            this.m_grpOnlineProvider.Location = new System.Drawing.Point( 6, 6 );
            this.m_grpOnlineProvider.Name = "m_grpOnlineProvider";
            this.m_grpOnlineProvider.Size = new System.Drawing.Size( 365, 106 );
            this.m_grpOnlineProvider.TabIndex = 13;
            this.m_grpOnlineProvider.TabStop = false;
            this.m_grpOnlineProvider.Text = "Online Provider";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 8, 18 );
            this.label1.MaximumSize = new System.Drawing.Size( 350, 0 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 337, 26 );
            this.label1.TabIndex = 12;
            this.label1.Text = "These are online services that store your data.  Most are free, be sure you choos" +
                "e a provider from a source you trust.";
            // 
            // m_MainTab
            // 
            this.m_MainTab.Controls.Add( this.m_TabAccount );
            this.m_MainTab.Controls.Add( this.m_TabGeneral );
            this.m_MainTab.Location = new System.Drawing.Point( 12, 66 );
            this.m_MainTab.Name = "m_MainTab";
            this.m_MainTab.SelectedIndex = 0;
            this.m_MainTab.Size = new System.Drawing.Size( 386, 268 );
            this.m_MainTab.TabIndex = 14;
            // 
            // m_TabAccount
            // 
            this.m_TabAccount.Controls.Add( this.m_grpOnlineProvider );
            this.m_TabAccount.Controls.Add( this.m_grpAccountInfo );
            this.m_TabAccount.Location = new System.Drawing.Point( 4, 22 );
            this.m_TabAccount.Name = "m_TabAccount";
            this.m_TabAccount.Padding = new System.Windows.Forms.Padding( 3 );
            this.m_TabAccount.Size = new System.Drawing.Size( 378, 242 );
            this.m_TabAccount.TabIndex = 0;
            this.m_TabAccount.Text = "Account";
            this.m_TabAccount.UseVisualStyleBackColor = true;
            // 
            // m_grpAccountInfo
            // 
            this.m_grpAccountInfo.Controls.Add( this.checkBox1 );
            this.m_grpAccountInfo.Controls.Add( this.label2 );
            this.m_grpAccountInfo.Controls.Add( this.m_btnStoreCreate );
            this.m_grpAccountInfo.Location = new System.Drawing.Point( 6, 118 );
            this.m_grpAccountInfo.Name = "m_grpAccountInfo";
            this.m_grpAccountInfo.Size = new System.Drawing.Size( 365, 115 );
            this.m_grpAccountInfo.TabIndex = 0;
            this.m_grpAccountInfo.TabStop = false;
            this.m_grpAccountInfo.Text = "Account Information";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point( 11, 90 );
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size( 78, 17 );
            this.checkBox1.TabIndex = 13;
            this.checkBox1.Text = "Raw Mode";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point( 8, 16 );
            this.label2.MaximumSize = new System.Drawing.Size( 350, 0 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 338, 65 );
            this.label2.TabIndex = 12;
            this.label2.Text = resources.GetString( "label2.Text" );
            // 
            // m_btnStoreCreate
            // 
            this.m_btnStoreCreate.Enabled = false;
            this.m_btnStoreCreate.Location = new System.Drawing.Point( 216, 84 );
            this.m_btnStoreCreate.Name = "m_btnStoreCreate";
            this.m_btnStoreCreate.Size = new System.Drawing.Size( 143, 23 );
            this.m_btnStoreCreate.TabIndex = 0;
            this.m_btnStoreCreate.Text = "Edit/Generate Entry";
            this.m_btnStoreCreate.UseVisualStyleBackColor = true;
            this.m_btnStoreCreate.Click += new System.EventHandler( this.m_btnStoreCreate_Click );
            // 
            // m_TabGeneral
            // 
            this.m_TabGeneral.Controls.Add( this.m_grpMergeOptions );
            this.m_TabGeneral.Controls.Add( this.m_grpAutosave );
            this.m_TabGeneral.Location = new System.Drawing.Point( 4, 22 );
            this.m_TabGeneral.Name = "m_TabGeneral";
            this.m_TabGeneral.Padding = new System.Windows.Forms.Padding( 3 );
            this.m_TabGeneral.Size = new System.Drawing.Size( 378, 242 );
            this.m_TabGeneral.TabIndex = 1;
            this.m_TabGeneral.Text = "General";
            this.m_TabGeneral.UseVisualStyleBackColor = true;
            // 
            // OptionsForm
            // 
            this.AcceptButton = this.m_btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.m_btnCancel;
            this.ClientSize = new System.Drawing.Size( 408, 379 );
            this.Controls.Add( this.m_MainTab );
            this.Controls.Add( this.m_btnOk );
            this.Controls.Add( this.m_btnCancel );
            this.Controls.Add( this.m_BannerImage );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "KeePassSync Options";
            this.Load += new System.EventHandler( this.OnFormLoad );
            this.KeyDown += new System.Windows.Forms.KeyEventHandler( this.OptionsForm_KeyDown );
            this.m_grpMergeOptions.ResumeLayout( false );
            this.m_grpMergeOptions.PerformLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.m_BannerImage ) ).EndInit();
            this.m_grpAutosave.ResumeLayout( false );
            this.m_grpAutosave.PerformLayout();
            this.m_grpOnlineProvider.ResumeLayout( false );
            this.m_grpOnlineProvider.PerformLayout();
            this.m_MainTab.ResumeLayout( false );
            this.m_TabAccount.ResumeLayout( false );
            this.m_grpAccountInfo.ResumeLayout( false );
            this.m_grpAccountInfo.PerformLayout();
            this.m_TabGeneral.ResumeLayout( false );
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.GroupBox m_grpMergeOptions;
        private System.Windows.Forms.RadioButton m_radioSynchronize;
        private System.Windows.Forms.RadioButton m_radioOverwriteNewer;
        private System.Windows.Forms.RadioButton m_radioOverwrite;
        private System.Windows.Forms.RadioButton m_radioKeepExisting;
        private System.Windows.Forms.RadioButton m_radioCreateNew;
        private System.Windows.Forms.PictureBox m_BannerImage;
        private System.Windows.Forms.LinkLabel m_lnkCreateAccount;
        private System.Windows.Forms.GroupBox m_grpAutosave;
        private System.Windows.Forms.ComboBox m_cboFrequency;
        private System.Windows.Forms.Label m_lblFrequency;
        private System.Windows.Forms.CheckBox m_checkAutosave;
        private System.Windows.Forms.Button m_btnOk;
        private System.Windows.Forms.Button m_btnCancel;
        private System.Windows.Forms.ComboBox m_cboProvider;
        private System.Windows.Forms.GroupBox m_grpOnlineProvider;
        private System.Windows.Forms.Label m_lblMergeDesc;
        private System.Windows.Forms.TabControl m_MainTab;
        private System.Windows.Forms.TabPage m_TabAccount;
        private System.Windows.Forms.TabPage m_TabGeneral;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox m_grpAccountInfo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button m_btnStoreCreate;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}