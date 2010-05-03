using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace KeePassSync_FTP.Forms
{
    public partial class AccountDetails : UserControl
    {
        public string Username
        {
            get { return txtUsername.Text; }
            set { txtUsername.Text = value; }
        }

        public string Password
        {
            get { return txtPassword.Text; }
            set { txtPassword.Text = value; }
        }
		public bool UsePaegentInstead
		{
			get { return checkPaegent.Checked; }
			set { checkPaegent.Checked = value; }
		}
        public string Host
        {
            get { return txtHost.Text; }
            set { txtHost.Text = value; }
        }

        public string Directory
        {
            get { return txtDirectory.Text; }
            set { txtDirectory.Text = value; }
        }
		public string Port
		{
			get {
				if (String.IsNullOrEmpty(txtPort.Text))
					return "22";
				return txtPort.Text;
			}
			set { 
				if (String.IsNullOrEmpty(value))
					txtPort.Text = "22";
				else
					txtPort.Text = value;
			}
		}
		public bool DebugMode
		{
			get { return checkDebug.Checked; }
			set { checkDebug.Checked = value; }
		}
		public int TimeoutSeconds
		{
			get { return Int32.Parse(comboTimeout.SelectedItem as string); }
			set { 
					comboTimeout.SelectedItem = value.ToString();
					if (comboTimeout.SelectedIndex == -1 || comboTimeout.SelectedItem.ToString() != value.ToString())
						comboTimeout.SelectedItem = "7";

				}
		}
		public string ExecRoot
		{
			get { return checkKeepassRoot.Checked ? "" : txtExecPath.Text; }
			set {
				if (String.IsNullOrEmpty(value))
				{
					checkKeepassRoot.Checked = true;
					txtExecPath.Text = "";
				}
				else
				{
					checkKeepassRoot.Checked = false;
					txtExecPath.Text = value;
				}
			}
		}
        public AccountDetails()
        {
            InitializeComponent();
			ExecRoot = "";
			TimeoutSeconds = -1;
			Port = "";
        }

		private void AccountDetails_Load(object sender, EventArgs e)
		{
			
		}

		private void checkPaegent_CheckedChanged(object sender, EventArgs e)
		{
			txtPassword.ReadOnly = checkPaegent.Checked;
		}

		private void checkKeepassRoot_CheckedChanged(object sender, EventArgs e)
		{
			txtExecPath.ReadOnly = checkKeepassRoot.Checked;
		}

    }
}
