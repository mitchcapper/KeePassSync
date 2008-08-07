using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KeePassSync.Forms
{
    public partial class OpenDbForm : Form
    {
        private KeePassSyncExt m_MainInterface;

        private OpenDbForm()
        {
        }

        public OpenDbForm( KeePassSyncExt mainInterface )
        {
            m_MainInterface = mainInterface;
            InitializeComponent();
        }

        private void OpenDbForm_Load( object sender, EventArgs e )
        {
            m_BannerImage.Image = KeePass.UI.BannerFactory.CreateBanner( m_BannerImage.Width,
                m_BannerImage.Height, KeePass.UI.BannerStyle.Default, Properties.Resources.Img_48x48_Password,
                "Open Online Database",
                "Please enter your " + m_MainInterface.OnlineProvider.Name + " account information." );

            this.Icon = m_MainInterface.Host.MainWindow.Icon;

            this.Left = m_MainInterface.Host.MainWindow.Left + ( m_MainInterface.Host.MainWindow.Width - this.Width ) / 2;
            this.Top = m_MainInterface.Host.MainWindow.Top + ( m_MainInterface.Host.MainWindow.Height - this.Height ) / 2;

        }
    }
}