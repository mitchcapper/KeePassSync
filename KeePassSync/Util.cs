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
using System.Collections;
using System.Reflection;
using System.IO;
namespace KeePassSync {
	public class Util {
		public static class HexStringConverter {
			public static byte[] ToByteArray(String HexString) {
				int NumberChars = HexString.Length;
				byte[] bytes = new byte[NumberChars / 2];
				for (int i = 0; i < NumberChars; i += 2) {
					bytes[i / 2] = Convert.ToByte(HexString.Substring(i, 2), 16);
				}
				return bytes;
			}

			public static String ToHexArray(byte[] bytes) {
				char[] c = new char[bytes.Length * 2];
				byte b;
				for (int y = 0, x = 0; y < bytes.Length; ++y, ++x) {
					b = ((byte)(bytes[y] >> 4));
					c[x] = (char)(b > 9 ? b + 0x37 : b + 0x30);
					b = ((byte)(bytes[y] & 0xF));
					c[++x] = (char)(b > 9 ? b + 0x37 : b + 0x30);
				}
				return new string(c);
			}
		}

		private static IOnlineProvider init_provider(IOnlineProvider provider, string Path) {
			provider.Path = Path;
			return provider;
		}

		public static IOnlineProvider[] DiscoverProviders() {
			ArrayList providers = new ArrayList();
			string[] ignoreDlls = { //primarily old versions 
                "KeePassSync_FTP.dll",
				"KeePassSync_S3.dll",
				"KeePassSync.dll",
				"KeePassSync_digitalBucket.net.dll"
				};

			providers.Add(init_provider(new Providers.S3.S3Provider(), "S3"));
			providers.Add(init_provider(new Providers.SFTP.FtpProvider(), "SFTP"));
			// Search through the directory for DLLs

			foreach (string filename in Directory.GetFiles(KeePassSyncExt.PluginDirectory, "*.dll")) {
				if (!filename.StartsWith("KeePassSync", StringComparison.CurrentCultureIgnoreCase))
					continue;
				bool skip = false;
				foreach (string ignore in ignoreDlls) {
					if (filename.ToLower().EndsWith(ignore.ToLower())) {
						skip = true;
						break;
					}
				}

				if (skip)
					continue;

				IOnlineProvider provider = Util.LoadOnlineProvider(filename);
				if (provider != null)
					providers.Add(provider);
			}

			return (IOnlineProvider[])providers.ToArray(typeof(IOnlineProvider));
		}

		public static IOnlineProvider LoadOnlineProvider(string providerPath) {
			IOnlineProvider provider = null;
			try {
				Assembly asm = Assembly.LoadFrom(providerPath);
				if (asm != null) {
					Type[] types = asm.GetTypes();
					if (types != null) {
						for (int i = 0; i < types.Length; i++) {
							if (types[i].IsClass && !types[i].IsAbstract && types[i].BaseType.Name == "IOnlineProvider") {
								object o = asm.CreateInstance(types[i].FullName);
								provider = o as IOnlineProvider;
								provider.Path = asm.Location;
							}
						}
					}
				}
				return provider;
			} catch {
				return null;
			}
		}
	}
}
