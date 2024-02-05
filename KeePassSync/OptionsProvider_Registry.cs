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

using KeePassLib;

// For registry functions
using Microsoft.Win32;

namespace KeePassSync {
	public class OptionsProvider_Registry : IOptionsProvider {
		private KeePassSyncExt m_MainInterface;

		public void Initialize(KeePassSyncExt mainInterface) {
			m_MainInterface = mainInterface;
		}

		public bool Read(OptionsData mainOptions) {
			string keyName = "Software\\KeePass Plugin\\" + Properties.Resources.Str_Title;

			// Attempt to open the key
			RegistryKey key = Registry.CurrentUser.OpenSubKey(keyName);
			// If the return value is null, the key doesn't exist
			if (key == null)
				key = Registry.CurrentUser.CreateSubKey(keyName);

			if (key != null) {
				if (key.GetValue("MergeMethod") != null)
					mainOptions.MergeMethod = (PwMergeMethod)(int)key.GetValue("MergeMethod");

				if (key.GetValue("PreviousDatabaseLocation") != null)
					mainOptions.PreviousDatabaseLocation = (string)key.GetValue("PreviousDatabaseLocation");

				if (key.GetValue("OnlineProviderPath") != null) {
					string path = (string)key.GetValue("OnlineProviderPath");

					// See if the provider set in the registry is available within KeePass
					IOnlineProvider[] providers = Util.DiscoverProviders();
					foreach (IOnlineProvider provider in providers) {
						if (is_the_provider(provider, path)) {
							mainOptions.OnlineProviderKey = provider.Key;
							break;
						}
					}
				}
			}

			return (key != null);
		}
		private bool is_the_provider(IOnlineProvider provider, String path) {
			if (provider.Path == path)
				return true;
			if (path.EndsWith("KeePassSync_FTP.dll", StringComparison.CurrentCultureIgnoreCase) && provider.Path == "SFTP")
				return true;
			if (path.EndsWith("KeePassSync_S3.dll", StringComparison.CurrentCultureIgnoreCase) && provider.Path == "S3")
				return true;
			if (path.EndsWith("KeePassSync_digitalBucket.net.dll", StringComparison.CurrentCultureIgnoreCase) && provider.Path == "DigitalBucket")
				return true;

			return false;
		}
		public bool Write(OptionsData mainOptions) {
			string keyName = "Software\\KeePass Plugin\\" + Properties.Resources.Str_Title;

			// Attempt to open the key
			RegistryKey key = Registry.CurrentUser.OpenSubKey(keyName, true);

			// The key doesn't exist; create it / open it
			if (key == null)
				key = Registry.CurrentUser.CreateSubKey(keyName);

			if (key != null) {
				key.SetValue("MergeMethod", (int)mainOptions.MergeMethod);

				if (mainOptions.PreviousDatabaseLocation != null)
					key.SetValue("PreviousDatabaseLocation", mainOptions.PreviousDatabaseLocation);

				if (m_MainInterface.OnlineProvider != null && m_MainInterface.OnlineProvider.Path != null)
					key.SetValue("OnlineProviderPath", m_MainInterface.OnlineProvider.Path);
			}

			return (key != null);
		}

	}
}
