using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace KeePassSync.Providers.DigitalBucket {
	public partial class AccountDetails : UserControl {
		public string Username {
			get { return txtUsername.Text; }
			set { txtUsername.Text = value; }
		}

		public string Password {
			get { return txtPassword.Text; }
			set { txtPassword.Text = value; }
		}

		public AccountDetails() {
			InitializeComponent();
		}

	}
}
