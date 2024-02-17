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
using System.Diagnostics;
using System.IO;
using KeePassLib;
using KeePassLib.Security;

namespace KeePassSync.Providers.SFTP {
	public class FtpProvider : IOnlineProvider {
		#region -- Private data --
		private const string m_Name = "SCP/SFTP";
		private const string ONLINE_DB_PREFIX = "Keepass-";
		private string fields_username = PwDefs.UserNameField;

		private string fields_password = PwDefs.PasswordField;
		private bool memprotect_password = true;

		private string fields_host = PwDefs.UrlField;
		private string fields_port = "Port";
		private string fields_directory = "DirectoryField";
		private string fields_exec_path = "ExecPath";
		private string fields_timeout = "Timeout";
		private string fields_debug_mode = "Debug";
		private string fields_paegent = "Paegent";

		private string[] m_AcceptedNames = { "SCP/SFTP", "SCP/FTP" };
		private AccountDetails m_UserControl = null;
		#endregion

		private enum EXEC { PLINK, PSCP };
		private string str_qwt(string str) {
			return "\"" + str + "\"";
		}
		private KeePassSyncErr command_plink(String command) {
			return _run_command(EXEC.PLINK, "", command, false);
		}
		private KeePassSyncErr command_scp(String source_file, String dest_file, bool is_get) {
			if (!is_get)
				source_file = str_qwt(source_file);
			else
				dest_file = str_qwt(dest_file);

			return _run_command(EXEC.PSCP, source_file, dest_file, is_get);
		}
		private KeePassSyncErr _run_command(EXEC exec, String command_before, String command_after, bool is_get) {
			KeePassSyncErr ret = KeePassSyncErr.None;
			if (String.IsNullOrEmpty(m_UserControl.Host))
				return KeePassSyncErr.Error;

			try {
				System.Diagnostics.Process process = new Process();
				if (process == null)
					return KeePassSyncErr.Error;

				string commandStr = "-batch";
				if (m_UserControl.UsePaegentInstead)
					commandStr += " -agent";
				else
					commandStr += " -pw " + str_qwt(m_UserControl.Password);
				commandStr += " -P " + str_qwt(m_UserControl.Port);
				if (m_UserControl.DebugMode) {
					process.StartInfo.CreateNoWindow = false;
					commandStr += " -v";
					process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
				} else {
					process.StartInfo.CreateNoWindow = true;
					process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
				}
				if (!String.IsNullOrEmpty(command_before) && (exec != EXEC.PSCP || !is_get))
					commandStr += " " + command_before;

				string host_str = m_UserControl.Username + "@" + m_UserControl.Host;

				if (exec == EXEC.PSCP && is_get)
					host_str += ":" + command_before;

				if (exec == EXEC.PSCP && !is_get)
					host_str += ":" + command_after;

				commandStr += " " + str_qwt(host_str);

				if (!String.IsNullOrEmpty(command_after) && (exec != EXEC.PSCP || is_get))
					commandStr += " " + command_after;

				String exec_path = m_UserControl.ExecRoot;
				if (String.IsNullOrEmpty(exec_path))
					exec_path = KeePassSyncExt.PluginDirectory;
				switch (exec) {
					case EXEC.PSCP:
						exec_path += "\\" + "pscp.exe";
						break;
					case EXEC.PLINK:
						exec_path += "\\" + "plink.exe";
						break;
				}
				if (!System.IO.File.Exists(exec_path)) {
					m_MainInterface.SetStatus(StatusPriority.eMessageBoxInfo, "Unable to find pscp/plink.exe at the path: " + exec_path + " specified please check");
					return KeePassSyncErr.Error;
				}
				process.StartInfo.FileName = exec_path;
				process.StartInfo.Arguments = commandStr;

				process.StartInfo.WorkingDirectory = KeePassSyncExt.PluginDirectory;
				process.Start();
				bool exitedOnTime = process.WaitForExit(m_UserControl.TimeoutSeconds * 1000);

				if (exitedOnTime)
					ret = KeePassSyncErr.None;
				else {
					m_MainInterface.SetStatus(StatusPriority.eMessageBoxInfo, "A timeout occurred validating options using FTP/SCP provider.\n\nAre you sure you've authenticated your server?\nSee docs\\KeePassSync-readme.txt for details.");
					ret = KeePassSyncErr.Timeout;
				}


			} catch {
				ret = KeePassSyncErr.Error;
			}
			return ret;
		}
		private string KeepassDirFilename { get { return KeePassSyncExt.PluginDirectory + "\\KeePassSyncDir.txt"; } }
		public override KeePassSyncErr Initialize(KeePassSyncExt mainInterface) {
			KeePassSyncErr ret = base.Initialize(mainInterface);

			m_UserControl = new AccountDetails();

			m_IsInitialized = (ret == KeePassSyncErr.None);

			return ret;
		}

		public override KeePassSyncErr ValidateOptions(OptionsData options) {
			KeePassSyncErr ret = KeePassSyncErr.None;
			PwEntry entry = m_OptionData.PasswordEntry;
			AccountDetails old_details = m_UserControl;
			m_UserControl = new AccountDetails();
			DecodeEntry(entry);
			ret = command_plink("ls");
			m_UserControl = old_details;
			return ret;
		}


		/// <summary>
		/// First delete the file and then save it.  This service provider automatically
		/// renames the sent file much like Windows Explorer does when there is a duplicate
		/// filename on copy.
		/// </summary>
		/// <param name="databaseName"></param>
		/// <param name="filename"></param>
		/// <returns></returns>
		public override KeePassSyncErr PutFile(PwEntry entry, string databaseName, string filename) {
			DecodeEntry(entry);
			KeePassSyncErr ret = KeePassSyncErr.Error;
			System.Diagnostics.Process process = new Process();

			string directory = m_UserControl.Directory;
			if (directory != "" && !directory.EndsWith("/"))
				directory += "/";

			string remoteDatabaseName = ONLINE_DB_PREFIX + databaseName;

			// Backup the existing file
			string backupName = remoteDatabaseName + ".bak";

			ret = command_plink("mv " + str_qwt(directory + remoteDatabaseName) + " " + str_qwt(directory + backupName));



			// Send the new file
			if (ret == KeePassSyncErr.None) {
				ret = command_scp(filename, directory + remoteDatabaseName, false);
				if (ret == KeePassSyncErr.None)
					ret = CheckRemoteFileExists(remoteDatabaseName);
			}
			return ret;
		}

		private KeePassSyncErr CheckRemoteFileExists(string remoteDatabaseName) {
			KeePassSyncErr ret = KeePassSyncErr.Error;

			try {
				ret = RemoteDatabaseListing(KeepassDirFilename);
				if (ret == KeePassSyncErr.None) {
					string directory = m_UserControl.Directory;
					if (directory != "" && !directory.EndsWith("/"))
						directory += "/";

					// read the text file for databases
					ret = KeePassSyncErr.FileNotFound;
					string[] lines = File.ReadAllLines(KeepassDirFilename);
					if (lines.Length > 0) {
						foreach (string line in lines) {
							if (line == (directory + remoteDatabaseName)) {
								ret = KeePassSyncErr.None;
								break;
							}
						}
					}
				} else {
					ret = KeePassSyncErr.FileNotFound;
				}

			} catch {
				ret = KeePassSyncErr.Error;
			}

			if (File.Exists(KeepassDirFilename))
				File.Delete(KeepassDirFilename);

			return ret;
		}

		public override KeePassSyncErr GetFile(PwEntry entry, string databaseName, string filename) {
			Debug.Assert(entry != null, "Invalid entry");
			DecodeEntry(entry);
			KeePassSyncErr ret = KeePassSyncErr.None;
			if (File.Exists(filename))
				File.Delete(filename);

			string directory = m_UserControl.Directory;
			if (directory != "" && !directory.EndsWith("/"))
				directory += "/";

			string remoteDatabaseName = ONLINE_DB_PREFIX + databaseName;

			ret = command_scp(directory + remoteDatabaseName, filename, true);

			if (ret == KeePassSyncErr.None && !File.Exists(filename))
				ret = KeePassSyncErr.FileNotFound;


			return ret;
		}


		public override string CreateAccountLink {
			get { return null; }
		}

		public override string Name {
			get { return m_Name; }
		}

		public override string[] AcceptedNames {
			get { return m_AcceptedNames; }
		}

		public override string[] Databases {
			get {
				return GetDatabases(m_OptionData.PasswordEntry);
			}
		}

		private KeePassSyncErr RemoteDatabaseListing(string localListingFileFullPath) {
			KeePassSyncErr ret = KeePassSyncErr.Error;

			if (File.Exists(localListingFileFullPath))
				File.Delete(localListingFileFullPath);

			// Generate directory listing
			string remoteDirectory = m_UserControl.Directory;
			if (remoteDirectory != "" && !remoteDirectory.EndsWith("/"))
				remoteDirectory += "/";

			ret = command_plink("ls -1 " + str_qwt(remoteDirectory + ONLINE_DB_PREFIX + "*") + " > " + str_qwt(remoteDirectory + "KeePassSyncDir.txt"));


			if (ret == KeePassSyncErr.None) {
				ret = command_scp(remoteDirectory + "KeePassSyncDir.txt", localListingFileFullPath, true);
				if (ret == KeePassSyncErr.None && !File.Exists(localListingFileFullPath))
					ret = KeePassSyncErr.Error;
			}


			return ret;
		}

		public override string[] GetDatabases(PwEntry entry) {
			Debug.Assert(entry != null, "Invalid entry");
			AccountDetails old_details = m_UserControl;
			m_UserControl = new AccountDetails();
			DecodeEntry(entry);


			// Use plink to issue directory command as text file
			// plink -pw password user@host "dir > out.txt"
			// use pscp to download the file
			// then plink to delete the file again
			// pscp doesn't like a full path, it likes it relative to the home folder
			KeePassSyncErr err = RemoteDatabaseListing(KeepassDirFilename);
			if (err == KeePassSyncErr.None) {
				// Generate directory listing
				string remoteDirectory = m_UserControl.Directory;
				if (remoteDirectory != "" && !remoteDirectory.EndsWith("/"))
					remoteDirectory += "/";

				// read the text file for databases
				string[] lines = File.ReadAllLines(KeepassDirFilename);
				ArrayList databases = null;
				if (lines.Length > 0) {
					databases = new ArrayList();
					foreach (string line in lines) {
						databases.Add(line.Substring(remoteDirectory.Length + ONLINE_DB_PREFIX.Length));
					}
					m_UserControl = old_details;
					return (string[])databases.ToArray(typeof(string));
				}
			} else if (err == KeePassSyncErr.Timeout)
				m_MainInterface.SetStatus(StatusPriority.eMessageBoxInfo, "A timeout occurred retrieving databases.\n\nAre you sure you've authenticated your server?\nSee docs\\KeePassSync-readme.txt for details.");
			else
				m_MainInterface.SetStatus(StatusPriority.eMessageBoxInfo, "A general error occurred retrieving databases.\n\nAre you sure you've authenticated your server?\nSee docs\\KeePassSync-readme.txt for details.");

			if (File.Exists(KeepassDirFilename))
				File.Delete(KeepassDirFilename);
			m_UserControl = old_details;
			return null;
		}

		public override System.Windows.Forms.UserControl GetUserControl() {
			return m_UserControl;
		}
		public string read_PwEntry_string(PwEntry entry, String key) {
			ProtectedString str = entry.Strings.Get(key);
			if (str == null)
				return "";
			return str.ReadString();
		}
		public bool read_PwEntry_bool(PwEntry entry, String key) {
			String val = read_PwEntry_string(entry, key);
			if (val == "true")
				return true;
			return false;
		}
		public override void DecodeEntry(PwEntry entry) {
			Debug.Assert(entry != null, "Invalid entry");
			m_UserControl.Username = read_PwEntry_string(entry, fields_username);
			m_UserControl.Password = read_PwEntry_string(entry, fields_password);
			m_UserControl.Host = read_PwEntry_string(entry, fields_host);
			m_UserControl.Port = read_PwEntry_string(entry, fields_port);
			m_UserControl.Directory = read_PwEntry_string(entry, fields_directory);
			m_UserControl.ExecRoot = read_PwEntry_string(entry, fields_exec_path);
			int timeout = 0;
			Int32.TryParse(read_PwEntry_string(entry, fields_timeout), out timeout);
			m_UserControl.TimeoutSeconds = timeout;
			m_UserControl.DebugMode = read_PwEntry_bool(entry, fields_debug_mode);
			m_UserControl.UsePaegentInstead = read_PwEntry_bool(entry, fields_paegent);

		}
		public void write_PwEntry_string(PwEntry entry, String key, String value, bool in_memory_encrypt) {
			entry.Strings.Set(key, new ProtectedString(in_memory_encrypt, value));
		}
		public void write_PwEntry_bool(PwEntry entry, String key, bool value, bool in_memory_encrypt) {
			write_PwEntry_string(entry, key, value ? "true" : "false", in_memory_encrypt);
		}
		public override void EncodeEntry(PwEntry entry) {
			Debug.Assert(entry != null, "Invalid entry");
			write_PwEntry_string(entry, fields_username, m_UserControl.Username, false);
			write_PwEntry_string(entry, fields_password, m_UserControl.Password, memprotect_password);
			write_PwEntry_string(entry, fields_host, m_UserControl.Host, false);
			write_PwEntry_string(entry, fields_port, m_UserControl.Port, false);
			write_PwEntry_string(entry, fields_directory, m_UserControl.Directory, false);
			write_PwEntry_string(entry, fields_exec_path, m_UserControl.ExecRoot, false);
			write_PwEntry_string(entry, fields_timeout, m_UserControl.TimeoutSeconds.ToString(), false);
			write_PwEntry_bool(entry, fields_debug_mode, m_UserControl.DebugMode, false);
			write_PwEntry_bool(entry, fields_paegent, m_UserControl.UsePaegentInstead, false);


		}

	}
}
