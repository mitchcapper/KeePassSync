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

using KeePassSync;
using KeePass;
using KeePassLib;


namespace KeePassSync.Forms {
	public partial class OpenForm : Form {
		private KeePassSyncExt m_MainInterface;
		private string m_OnlineProviderKey;
		public string OnlineProviderKey { get { return m_OnlineProviderKey; } }

		private PwEntry m_Entry;
		public PwEntry Entry {
			get { return m_Entry; }
			set { m_Entry = (PwEntry)value.CloneDeep(); }
		}

		public OpenForm(KeePassSyncExt mainInterface) {
			m_MainInterface = mainInterface;
			InitializeComponent();
			// only do this here so i initialize the key on new()
			PopulateComboBox();
		}

		private OpenForm() {
		}

		private void RefreshBannerImage() {
			string subString = "Please enter your account information.";
			if (m_OnlineProviderKey != null) {
				subString = "Please enter your " + m_MainInterface.GetOnlineProvider(m_OnlineProviderKey).Name + " account information.";
			}

			m_BannerImage.Image = KeePass.UI.BannerFactory.CreateBanner(m_BannerImage.Width,
				m_BannerImage.Height, KeePass.UI.BannerStyle.Default, Properties.Resources.Img_48x48_Password,
				"Open Online Database",
				subString);
		}

		private void OpenForm_Load(object sender, EventArgs e) {
			RefreshBannerImage();
			ResetForm();

			this.Icon = m_MainInterface.Host.MainWindow.Icon;

			this.Left = m_MainInterface.Host.MainWindow.Left + (m_MainInterface.Host.MainWindow.Width - this.Width) / 2;
			this.Top = m_MainInterface.Host.MainWindow.Top + (m_MainInterface.Host.MainWindow.Height - this.Height) / 2;
		}

		private void m_btnCancel_Click(object sender, EventArgs e) {
			DialogResult = DialogResult.Cancel;
			m_Entry = null;
			this.Close();
		}

		private void m_btnOk_Click(object sender, EventArgs e) {
			DialogResult = DialogResult.OK;
			if (m_OnlineProviderKey != null) {
				if (m_Entry == null) {
					m_Entry = new PwEntry(true, true);
					if (m_Entry == null) {
						m_MainInterface.SetStatus(StatusPriority.eMessageBoxInfo, "Couldn't create KeePass entry.");
					}
				}
				m_MainInterface.GetOnlineProvider(m_OnlineProviderKey).EncodeEntry(m_Entry);
			} else {
				m_MainInterface.SetStatus(StatusPriority.eMessageBoxInfo, "Provider key invalid.");
			}
			this.Close();
		}

		public void DecodeEntry(PwEntry entry) {
			m_MainInterface.GetOnlineProvider(m_OnlineProviderKey).DecodeEntry(entry);
		}

		private void ResetForm() {
			PopulateComboBox();
			RefreshGuiStates();
			ResetAccountDetails();
		}

		private void ResetAccountDetails() {
			panel1.Controls.Clear();
			if (m_OnlineProviderKey != null) {
				panel1.Controls.Add(m_MainInterface.GetOnlineProvider(m_OnlineProviderKey).GetUserControl());
				ResizeDialog();
				panel1.Enabled = true;
			} else {
				panel1.Enabled = false;
			}
		}

		private void PopulateComboBox() {
			m_cboProvider.Items.Clear();

			if (m_MainInterface.OnlineProviders.Length == 0) {
				m_cboProvider.Items.Add("No providers installed...");
			} else {
				foreach (IOnlineProvider provider in m_MainInterface.OnlineProviders) {
					int index = m_cboProvider.Items.Add(provider);

					// Set the default
					if ((m_OnlineProviderKey != null) && (m_OnlineProviderKey == provider.Key)) {
						m_cboProvider.SelectedItem = m_cboProvider.Items[index];
					}
				}
			}

			if (m_cboProvider.Items.Count > 0 && m_cboProvider.SelectedItem == null) {
				m_cboProvider.SelectedItem = m_cboProvider.Items[0];
			}
		}

		private void ResizeDialog() {
			int maxWidth = 500;
			int maxHeight = 700;

			int panelHeight = panel1.Size.Height;
			int panelWidth = panel1.Size.Width;

			int diffHeight = m_MainInterface.GetOnlineProvider(m_OnlineProviderKey).GetUserControl().Size.Height - panelHeight;
			int diffWidth = m_MainInterface.GetOnlineProvider(m_OnlineProviderKey).GetUserControl().Size.Width - panelWidth;

			if (diffHeight < 0 || (diffHeight + panelHeight > maxHeight))
				diffHeight = 0;

			if (diffWidth < 0 || (diffWidth + panelWidth > maxWidth))
				diffWidth = 0;

			this.Size = this.Size + new System.Drawing.Size(diffWidth, diffHeight);

			RefreshBannerImage();
		}

		private void RefreshGuiStates() {
			if (m_OnlineProviderKey != null) {
				if (m_MainInterface.GetOnlineProvider(m_OnlineProviderKey).CreateAccountLink == null) {
					m_lnkCreateAccount.Enabled = false;
				} else {
					m_lnkCreateAccount.Enabled = true;
				}
			}
		}

		private void OnCboProviderSelectionChangeCommitted(object sender, EventArgs e) {
			// get the provider and show the entry generator
			if (m_cboProvider.SelectedItem.GetType().BaseType == typeof(IOnlineProvider)) {
				IOnlineProvider provider = (IOnlineProvider)((IOnlineProvider)m_cboProvider.SelectedItem).Clone();
				if (provider != null) {
					if (!provider.IsInitialized) {
						provider.Initialize(m_MainInterface);
					}

					m_OnlineProviderKey = provider.Key;
				}
			}
			RefreshGuiStates();
			ResetAccountDetails();
		}

		private void OnLblCreateAccountClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			if (m_cboProvider.SelectedItem != null) {
				IOnlineProvider provider = (IOnlineProvider)m_cboProvider.SelectedItem;
				if (provider != null && provider.CreateAccountLink != null) {
					if (provider.CreateAccountLink.StartsWith("http://") || provider.CreateAccountLink.StartsWith("https://")) {
						System.Diagnostics.Process.Start(provider.CreateAccountLink);
					}
				}
			}
		}
	}
}