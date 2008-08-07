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
using System.Text;

using KeePass.Plugins;
using KeePass.Forms;
using KeePass.Resources;
using KeePassLib;
using KeePassLib.Security;

// For registry functions
using Microsoft.Win32;

namespace KeePassSync
{
    public class Options_Registry : IOptionsProvider
    {
        private KeePassSyncExt m_MainInterface;

        public void Initialize(KeePassSyncExt mainInterface)
        {
            m_MainInterface = mainInterface;
        }

        public bool Read()
        {
            string keyName = "Software\\KeePass Plugin\\" + Properties.Resources.Str_AppTitle;

            // Attempt to open the key
            RegistryKey key = Registry.CurrentUser.OpenSubKey(keyName);
            // If the return value is null, the key doesn't exist
            if (key == null)
                key = Registry.CurrentUser.CreateSubKey(keyName);

            if (key != null)
            {
                if (key.GetValue("MergeMethod") != null)
                    m_MainInterface.Options.MergeMethod = (PwMergeMethod)(int)key.GetValue("MergeMethod");

                if (key.GetValue("PreviousDatabaseLocation") != null)
                    m_MainInterface.Options.PreviousDatabaseLocation = (string)key.GetValue("PreviousDatabaseLocation");

                if (key.GetValue("OnlineProviderPath") != null)
                {
                    string path = (string)key.GetValue("OnlineProviderPath");

                    // See if the provider set in the registry is available within KeePass
                    ProviderInfo[] providers = Util.DiscoverProviders();
                    foreach (ProviderInfo provider in providers)
                    {
                        if (provider.OnlineProviderPath == path)
                        {
                            m_MainInterface.Options.ProviderInfo = provider;
                            break;
                        }
                    }
                }
            }

            return (key != null);
        }

        public bool Write()
        {
            string keyName = "Software\\KeePass Plugin\\" + Properties.Resources.Str_AppTitle;

            // Attempt to open the key
            RegistryKey key = Registry.CurrentUser.OpenSubKey(keyName, true);

            // The key doesn't exist; create it / open it
            if (key == null)
                key = Registry.CurrentUser.CreateSubKey(keyName);

            if (key != null)
            {
                key.SetValue("MergeMethod", (int)m_MainInterface.Options.MergeMethod);

                if (m_MainInterface.Options.PreviousDatabaseLocation != null)
                    key.SetValue("PreviousDatabaseLocation", m_MainInterface.Options.PreviousDatabaseLocation);

                if (m_MainInterface.Options.ProviderInfo != null && m_MainInterface.Options.ProviderInfo.OnlineProviderPath != null)
                    key.SetValue("OnlineProviderPath", m_MainInterface.Options.ProviderInfo.OnlineProviderPath);
            }

            return (key != null);
        }

    }
}
