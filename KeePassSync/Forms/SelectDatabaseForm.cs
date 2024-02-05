/*
  KeePassSync - Online Sync Plugin for KeePass Password Safe
  Copyright (C) 2008 Shawn Casey, shawn.casey@gmail.com

  This program is free software; you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation; either version 2 of the License, or
  (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with this program; if not, write to the Free Software
  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using KeePass.Plugins;
using KeePass.Forms;
using KeePass.Resources;
using KeePassLib;
using KeePassLib.Security;

// For registry functions
using Microsoft.Win32;

namespace KeePassSync.Forms {
	public sealed partial class SelectDatabaseForm : Form {
		private string m_SelectedDatabase;
		public string SelectedDatabase { get { return m_SelectedDatabase; } }

		private KeePassSyncExt m_MainInterface = null;

		public SelectDatabaseForm(KeePassSyncExt mainInterface) {
			m_MainInterface = mainInterface;
			InitializeComponent();
		}

		private SelectDatabaseForm() {
		}

		public void SetList(string[] data) {
			for (int i = 0; i < data.Length; i++) {
				m_lbDatabases.Items.Add(data[i]);
			}
			m_lbDatabases.Refresh();
		}

		private void OnBtnOkClick(object sender, EventArgs e) {
			if (m_lbDatabases.SelectedItems.Count == 1) {
				m_SelectedDatabase = m_lbDatabases.SelectedItems[0].ToString();
				DialogResult = DialogResult.OK;
				this.Close();
			}
		}

		private void OnBtnCancelClick(object sender, EventArgs e) {
			DialogResult = DialogResult.Cancel;
			m_SelectedDatabase = null;
			this.Close();
		}


		private void OnLbDatabasesMouseDoubleClick(object sender, MouseEventArgs e) {
			if (m_lbDatabases.SelectedIndex >= 0) {
				Rectangle rect = m_lbDatabases.GetItemRectangle(m_lbDatabases.SelectedIndex);
				// if the mouse is in the range of the rect, then select it.
				if (rect.Contains(e.Location)) {
					OnBtnOkClick(this, new EventArgs());
				}
			}
		}

		private void SelectDatabaseForm_Load(object sender, EventArgs e) {
			m_BannerImage.Image = KeePass.UI.BannerFactory.CreateBanner(m_BannerImage.Width,
				m_BannerImage.Height, KeePass.UI.BannerStyle.Default, Properties.Resources.Img_48x48_Open,
				"Select Database",
				"Choose online database to open");

			this.Icon = m_MainInterface.Host.MainWindow.Icon;

			this.Left = m_MainInterface.Host.MainWindow.Left + (m_MainInterface.Host.MainWindow.Width - this.Width) / 2;
			this.Top = m_MainInterface.Host.MainWindow.Top + (m_MainInterface.Host.MainWindow.Height - this.Height) / 2;


		}
	}
}