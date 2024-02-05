using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using KeePassLib;

namespace KeePassSync.Forms {
	public partial class AccountEntryGenerator : Form {
		private KeePassSyncExt m_MainInterface = null;
		private IOnlineProvider m_OnlineProvider = null;

		public AccountEntryGenerator(KeePassSyncExt mainInterface, IOnlineProvider onlineProvider) {
			InitializeComponent();
			m_MainInterface = mainInterface;
			m_OnlineProvider = onlineProvider;

			panel1.Controls.Add(m_OnlineProvider.GetUserControl());
		}

		private void ResizeDialog() {
			int maxWidth = 500;
			int maxHeight = 700;

			int panelHeight = panel1.Size.Height;
			int panelWidth = panel1.Size.Width;

			int diffHeight = m_OnlineProvider.GetUserControl().Size.Height - panelHeight;
			int diffWidth = m_OnlineProvider.GetUserControl().Size.Width - panelWidth;

			if (diffHeight < 0 || (diffHeight + panelHeight > maxHeight))
				diffHeight = 0;

			if (diffWidth < 0 || (diffWidth + panelWidth > maxWidth))
				diffWidth = 0;

			this.Size = this.Size + new System.Drawing.Size(diffWidth, diffHeight);

		}

		private void EntryGenerator_Load(object sender, EventArgs e) {
			ResizeDialog();

			m_BannerImage.Image = KeePass.UI.BannerFactory.CreateBanner(m_BannerImage.Width,
				m_BannerImage.Height, KeePass.UI.BannerStyle.Default, Properties.Resources.Img_48x48_Sync,
				"KeePassSync Entry Editor",
				"This will create/edit an entry for the provider");

			this.Icon = m_MainInterface.Host.MainWindow.Icon;
			this.Left = m_MainInterface.Host.MainWindow.Left + (m_MainInterface.Host.MainWindow.Width - this.Width) / 2;
			this.Top = m_MainInterface.Host.MainWindow.Top + (m_MainInterface.Host.MainWindow.Height - this.Height) / 2;
		}

		// @todo internal?
		public void EncodeEntry(PwEntry entry) {
			m_OnlineProvider.EncodeEntry(entry);
		}

		// @todo internal?
		public void DecodeEntry(PwEntry entry) {
			m_OnlineProvider.DecodeEntry(entry);
		}

		private void button2_Click(object sender, EventArgs e) {
			DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void button1_Click(object sender, EventArgs e) {
			DialogResult = DialogResult.OK;
			this.Close();
		}

		private void m_CustomPanel_Enter(object sender, EventArgs e) {

		}

	}
}
