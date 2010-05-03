﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace KeePassSync_S3
{
    public partial class AccountDetails : UserControl
    {
        public string AccessKey
        {
            get { return txtAccessKey.Text; }
            set { txtAccessKey.Text = value; }
        }

        public string SecretAccessKey
        {
            get { return txtSecretAccessKey.Text; }
            set { txtSecretAccessKey.Text = value; }
        }

        public string BucketName
        {
            get { return txtBucketName.Text; }
            set { txtBucketName.Text = value; }
        }
		public bool CreateBackups
		{
			get { return cbxDailyBackups.Checked; }
			set { cbxDailyBackups.Checked = value; }

		}

        public AccountDetails()
        {
            InitializeComponent();
        }

		private void AccountDetails_Load(object sender, EventArgs e)
		{

		}

    }
}