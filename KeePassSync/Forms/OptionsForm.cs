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
using System.IO;
using System.Collections.Generic;
using System.Collections;
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


// Get the data from a OnlineSyncPlugin entry inside the General section of KeePass
namespace KeePassSync.Forms {
	public sealed partial class OptionsForm : Form {
		#region -- Data & Properties --
		private KeePassSyncExt m_MainInterface = null;
		private PwEntry m_EditedEntry = null;
		private bool m_EditButtonEnabled = false;
		private bool m_CreatedEntry = false;

		private OptionsData m_OptionData = null;
		public OptionsData SavedOptions { get { return m_OptionData; } }

		private bool m_EntryChanged = false;
		public bool EntryChanged { get { return m_EntryChanged; } }

		private IOnlineProvider OnlineProvider { get { return m_MainInterface.GetOnlineProvider(m_OptionData.OnlineProviderKey); } }
		#endregion

		public OptionsForm(KeePassSyncExt mainInterface) {
			InitializeComponent();
			m_MainInterface = mainInterface;
			m_OptionData = (OptionsData)m_MainInterface.Options.Clone();
			ResetForm();
		}

		private OptionsForm() {
		}

		private void PopulateComboBox() {
			if (m_OptionData != null) {
				m_cboProvider.Items.Clear();

				if (m_MainInterface.OnlineProviders.Length == 0) {
					m_cboProvider.Items.Add("No providers installed...");
				} else {
					foreach (IOnlineProvider provider in m_MainInterface.OnlineProviders) {
						int index = m_cboProvider.Items.Add(provider);

						// Set the default
						if ((m_OptionData.OnlineProviderKey != null) && (m_OptionData.OnlineProviderKey == provider.Key)) {
							m_cboProvider.SelectedItem = m_cboProvider.Items[index];
						}
					}
				}

				if (m_cboProvider.Items.Count > 0 && m_cboProvider.SelectedItem == null) {
					m_cboProvider.SelectedItem = m_cboProvider.Items[0];
				}
			}
		}

		private void ResetForm() {
			if (m_OptionData != null) {
				PopulateComboBox();

				// Setup default merge
				// Very important that the tab order for the radio buttons must be the same as the MergeMethod enum
				int control = 0;
				for (int i = 0; i < m_grpMergeOptions.Controls.Count; i++) {
					RadioButton btn = m_grpMergeOptions.Controls[i] as RadioButton;
					if (btn != null) {
						btn.Checked = ((int)m_OptionData.MergeMethod == control);
						control++;
					}
				}
			}

			RefreshGuiStates();
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

		private void CopyData() {
			int control = 0;
			for (int i = 0; i < m_grpMergeOptions.Controls.Count; i++) {
				RadioButton btn = m_grpMergeOptions.Controls[i] as RadioButton;
				if (btn != null) {
					if (btn.Checked) {
						m_OptionData.MergeMethod = (PwMergeMethod)control;
					}
					control++;
				}
			}

			if (m_cboProvider.SelectedItem != null && m_cboProvider.SelectedItem.GetType().BaseType == typeof(IOnlineProvider)) {
				IOnlineProvider pi = m_cboProvider.SelectedItem as IOnlineProvider;
				m_OptionData.OnlineProviderKey = pi.Key;
			} else {
				m_OptionData.OnlineProviderKey = null;
			}

			if (m_EditedEntry != null) {
				if (m_OptionData.PasswordEntry != null) {
					m_OptionData.PasswordEntry.AssignProperties(m_EditedEntry, false, true, false);
					m_OptionData.PasswordEntry.Touch(true);
				} else {
					m_OptionData.PasswordEntry = m_EditedEntry;
					m_EditedEntry = m_OptionData.PasswordEntry.CloneDeep();
					m_OptionData.PasswordEntry.Touch(true);
				}
			}
		}
		private void OnBtnOkClicked(object sender, EventArgs e) {
			// Don't do this now, too many issues with it such as what happens
			// when they don't validate, someone opens the entry directly and 
			// inputs correct information.  There is no sense in validating now
			// because you'll always have to return an error if incorrect.
			//
			// Once threading and all the events are implemented, this may be
			// viable to give the user a heads up

			//// Test the password to validate the options
			//m_OptionData.OnlineProvider.ValidateOptions();

			DialogResult = DialogResult.OK;
			CopyData();

			this.Close();
		}

		private void RefreshGuiStates() {
			RefreshButtonText();
			if (OnlineProvider != null) {
				if (OnlineProvider.CreateAccountLink == null) {
					m_lnkCreateAccount.Enabled = false;
				} else {
					m_lnkCreateAccount.Enabled = true;
				}
			}
		}

		private void OnBtnCancelClicked(object sender, EventArgs e) {
			if (m_CreatedEntry) {
				KeePassSupport.DeleteEntry(m_MainInterface.Host, m_EditedEntry);
				m_EditedEntry = null;
			}
			DialogResult = DialogResult.Cancel;
			m_EntryChanged = false;
			this.Close();
		}

		private void OnFormLoad(object sender, EventArgs e) {
			m_BannerImage.Image = KeePass.UI.BannerFactory.CreateBanner(m_BannerImage.Width,
				m_BannerImage.Height, KeePass.UI.BannerStyle.Default, Properties.Resources.Img_48x48_Sync,
				"KeePassSync Options",
				"Here you setup your online account and sync options");

			RetrieveKeepassEntry(OnlineProvider);

			this.Icon = m_MainInterface.Host.MainWindow.Icon;

			this.Left = m_MainInterface.Host.MainWindow.Left + (m_MainInterface.Host.MainWindow.Width - this.Width) / 2;
			this.Top = m_MainInterface.Host.MainWindow.Top + (m_MainInterface.Host.MainWindow.Height - this.Height) / 2;
		}

		private void OnCboProviderSelectionChangeCommitted(object sender, EventArgs e) {
			if (m_cboProvider.SelectedItem.GetType().BaseType == typeof(IOnlineProvider)) {
				m_OptionData.OnlineProviderKey = ((IOnlineProvider)m_cboProvider.SelectedItem).Key;
				if (RetrieveKeepassEntry(OnlineProvider)) {
					OnlineProvider.DecodeEntry(m_EditedEntry);
				}
			}

			RefreshGuiStates();
		}

		private bool RetrieveKeepassEntry(IOnlineProvider provider) {
			bool ret = false;
			PwEntry entry = null;
			if (OnlineProvider != null)
				m_OptionData.PasswordEntry = KeePassSupport.FindEntry(m_MainInterface.Host, OnlineProvider);
			else
				m_OptionData.PasswordEntry = null;
			if (provider != null) {
				entry = KeePassSupport.FindEntry(m_MainInterface.Host, provider);
			}

			if (entry != null) {
				m_EditedEntry = entry.CloneDeep();
				ret = true;
			}
			m_EditButtonEnabled = ret;

			RefreshButtonText();

			return ret;
		}

		private void RefreshButtonText() {
			string[] buttonText = { "Create KeePass Entry", "Edit KeePass Entry" };

			if (m_MainInterface.ValidDatabase) {
				m_btnStoreCreate.Enabled = true;

				if (m_EditButtonEnabled) {
					m_btnStoreCreate.Text = buttonText[1];
				} else {
					m_btnStoreCreate.Text = buttonText[0];
				}
			} else {
				m_btnStoreCreate.Enabled = false;
			}

		}


		private void m_btnStoreCreate_Click(object sender, EventArgs e) {
			// if editing the entry, the entry must be non-null, it's just a matter of using raw-editing or user-editing
			if (m_EditButtonEnabled) {
				if (m_EditedEntry == null)
					m_EditedEntry = m_OptionData.PasswordEntry.CloneDeep();

				// Pass the entry to the provider for it to parse and populate it's account details
				// It will then show the entry in the form of it's own control
				// or if "Raw Mode" is enabled, show the KeePass entry directly.
				if (checkBox1.Checked) {
					m_EntryChanged = KeePassSupport.EditEntry(m_MainInterface.Host, m_EditedEntry);
				} else {
					m_EntryChanged = m_MainInterface.GetOnlineProvider(m_OptionData.OnlineProviderKey).EditEntry(m_EditedEntry);
				}
			} else {
				m_EditedEntry = KeePassSupport.CreateEntry(m_MainInterface.Host, Properties.Resources.Str_PasswordEntryTemplate.Replace("%1", m_MainInterface.GetOnlineProvider(m_OptionData.OnlineProviderKey).Name));
				m_CreatedEntry = false;
				m_EntryChanged = true;

				if (checkBox1.Checked) {
					m_CreatedEntry = KeePassSupport.EditEntry(m_MainInterface.Host, m_EditedEntry);
				} else {
					m_CreatedEntry = m_MainInterface.GetOnlineProvider(m_OptionData.OnlineProviderKey).EditEntry(m_EditedEntry);
				}

				if (!m_CreatedEntry) {
					KeePassSupport.DeleteEntry(m_MainInterface.Host, m_EditedEntry);
					m_EditedEntry = null;
					m_EntryChanged = false;
				}
			}

			// This updates the provider's name based on the new provider's name and all of the accepted names.  This allows 
			// providers to rename their providers and still have the old KeePass entries work.
			if (m_EntryChanged) {
				m_EditButtonEnabled = true;
				RefreshButtonText();
				KeePassSupport.UpdateEntryName(m_MainInterface.Host, m_EditedEntry, m_MainInterface.GetOnlineProvider(m_OptionData.OnlineProviderKey));
			}
		}

	}
}