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
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using KeePass.Forms;
using KeePass.Plugins;
using KeePassLib;
using KeePassSync.Forms;

// You will need a password file for this solution.  See plugin development guide:
// http://keepass.info/help/v2_dev/plg_index.html


// Object lifetime
// 
// Providers aren't initialized at discovery, they are instantiated so that we can get the name of the objects to populate the combo boxes in the options form.
// After the user selects ok on the options and the online provider is different, we initialize the provider.
// On the creation of the options form, when 

namespace KeePassSync {
	public enum KeePassSyncErr {
		None = 0,
		FileNotFound,
		InvalidCredentials,
		NotConnected,
		Timeout,
		Error
	};

	/// <summary>
	/// This is the options object used between the main interface and the online and options providers.
	/// No username/password information is stored here or in/thru the option provider, however
	/// an encrypted PwEntry from KeePass is stored within the Options.
	/// </summary>
	public class OptionsData : ICloneable {
		/// <summary>
		/// KeePass entry for the appropriate username/password.
		/// </summary>
		public PwEntry PasswordEntry;

		/// <summary>
		/// Merge method to use for synchronization.
		/// </summary>
		public PwMergeMethod MergeMethod;

		/// <summary>
		/// Location of the previously saved database via the KeePassSync->Open command.
		/// </summary>
		public string PreviousDatabaseLocation;

		/// <summary>
		/// Contains the currently selected provider key.
		/// Always get the provider from the main interface since the main interface
		/// holds the dictionary for id -> provider
		/// The providers are populated once in discovery when the app starts
		/// </summary>
		public string OnlineProviderKey;

		public OptionsData() {
			MergeMethod = PwMergeMethod.Synchronize;
		}

		#region ICloneable Members

		public object Clone() {
			OptionsData options = (OptionsData)MemberwiseClone();

			return options;
		}
		#endregion
	}

	/// <summary>
	/// Used to quickly distinguish between error message output types.  Some errors
	/// are only displayed in the status bar while others display a message box with
	/// various icons.
	/// </summary>
	public enum StatusPriority {
		eStatusBar,
		eMessageBox,
		eMessageBoxInfo,
		eMessageBoxFatal
	}


	public sealed class KeePassSyncExt : Plugin {
		#region -- Private data --
		private ToolStripSeparator m_tsSeparator;
		private ToolStripMenuItem m_tsmiPopup;
		private ToolStripMenuItem m_tsmiOptions;
		private ToolStripMenuItem m_tsmiOpen;
		private ToolStripMenuItem m_tsmiSync;
		private IOptionsProvider m_OptionsProvider;
		private OptionsData m_OptionsData;
		private IPluginHost m_Host;
		private bool m_ValidDatabase;
		private bool m_ValidOnlineOptions;
		private Dictionary<string, IOnlineProvider> m_OnlineProviders;
		private static string m_PluginDirectory;
		#endregion

		#region -- Events --
		public event EventHandler OptionsChanged;
		public event EventHandler ValidDatabaseChanged;
		#endregion

		#region -- Properties --
		/// <summary>
		/// Current options
		/// </summary>
		public OptionsData Options { get { return m_OptionsData; } }

		/// <summary>
		/// KeePass plugin interface
		/// </summary>
		public IPluginHost Host { get { return m_Host; } }

		/// <summary>
		/// KeePass reports a valid database.
		/// </summary>
		public bool ValidDatabase { get { return m_ValidDatabase; } }

		/// <summary>
		/// Online provider in the options is initialized.
		/// </summary>
		public bool ValidOnlineOptions { get { return m_ValidOnlineOptions; } }

		/// <summary>
		/// Currently selected online provider from the options or open form.
		/// </summary>
		public IOnlineProvider OnlineProvider {
			get {
				IOnlineProvider ret = null;
				if (Options.OnlineProviderKey != null) {
					ret = m_OnlineProviders[Options.OnlineProviderKey];
				}
				return ret;
			}
		}

		/// <summary>
		/// Array of all of the initialized online providers.
		/// </summary>
		public IOnlineProvider[] OnlineProviders {
			get {
				IOnlineProvider[] theArray = new IOnlineProvider[m_OnlineProviders.Values.Count];
				m_OnlineProviders.Values.CopyTo(theArray, 0);
				return theArray;
			}
		}

		/// <summary>
		/// Location of the main KeePassSync directory
		/// </summary>
		public static string PluginDirectory {
			get {
				if (m_PluginDirectory == null) {
					Assembly ass = Assembly.GetCallingAssembly();
					string path = ass.Location;
					m_PluginDirectory = path.Remove(path.LastIndexOf('\\'));
				}
				return m_PluginDirectory;
			}
		}
		#endregion

		// The Initialize function is the most important one and you probably will always 
		// override it. In this function, you get an interface to the KeePass internals: 
		// an IPluginHost interface reference. By using this interface, you can access the 
		// KeePass main menu, the currently opened database, etc. The Initialize function 
		// is called immediately after KeePass loads your plugin. All initialization should 
		// be done in this function (not in the constructor of your plugin class!). If you 
		// successfully initialized everything, you must return true. If you return false, 
		// KeePass will immediately unload your plugin.
		public override bool Initialize(IPluginHost host) {
			m_Host = host;

			// Everything uses this object for options
			m_OptionsData = new OptionsData();

			m_OptionsProvider = new OptionsProvider_Registry();
			m_OptionsProvider.Initialize(this);
			InitializeOnlineProviders();

			// Can't trust any provider
			try {
				if (!m_OptionsProvider.Read(m_OptionsData)) {
					SetStatus(StatusPriority.eStatusBar, Properties.Resources.Str_ErrOptions);
				}
			} catch {
				SetStatus(StatusPriority.eMessageBoxFatal, Properties.Resources.Str_ErrExceptionOptions);
				return false;
			}

			// Get a reference to the 'Tools' menu item container
			ToolStripItemCollection tsMenu = m_Host.MainWindow.ToolsMenu.DropDownItems;

			// Add a separator at the bottom
			m_tsSeparator = new ToolStripSeparator();
			tsMenu.Add(m_tsSeparator);

			// Add the popup menu item
			m_tsmiPopup = new ToolStripMenuItem();
			m_tsmiPopup.Text = "KeePassSync";
			m_tsmiPopup.Image = Properties.Resources.Img_16x16_Sync;
			tsMenu.Add(m_tsmiPopup);

			m_tsmiOptions = new ToolStripMenuItem();
			m_tsmiOptions.Text = "Show Options...";
			m_tsmiOptions.Click += OnMenuShowOptions;
			m_tsmiPopup.DropDownItems.Add(m_tsmiOptions);
			m_tsmiOptions.Image = Properties.Resources.Img_16x16_Options;

			m_tsmiOpen = new ToolStripMenuItem();
			m_tsmiOpen.Text = "Open...";
			m_tsmiOpen.Click += OnMenuOpen;
			m_tsmiPopup.DropDownItems.Add(m_tsmiOpen);
			m_tsmiOpen.Image = Properties.Resources.Img_16x16_Open;

			m_tsmiSync = new ToolStripMenuItem();
			m_tsmiSync.Text = "Sync";
			m_tsmiSync.Click += OnMenuSync;
			m_tsmiSync.Enabled = false;
			m_tsmiPopup.DropDownItems.Add(m_tsmiSync);
			m_tsmiSync.Image = Properties.Resources.Img_16x16_Sync;

			// Event registration
			m_Host.MainWindow.FileClosed += OnFileClosed;
			m_Host.MainWindow.FileSaved += OnFileSaved;
			m_Host.MainWindow.FileOpened += OnFileOpened;
			m_Host.MainWindow.FileCreated += OnFileCreated;

			// TODO I really need a changed event on the active database...
			//m_Host.MainWindow.ActiveDatabase.Changed += OnChangedDatabase;

			OptionsChanged += OnOptionsChanged;
			ValidDatabaseChanged += OnValidDatabaseChanged;
			m_Host.MainWindow.FileClosed += MainWindow_FileClosed;

			//OptionsChanged(this, new OptionsChangedEventArgs(m_OptionsProvider));

			return true;
		}
		private bool main_window_is_closing;
		void MainWindow_FileClosed(object sender, FileClosedEventArgs e) {
			main_window_is_closing = true;
			if (sync_thread != null)
				if (sync_thread.IsAlive) {
					sync_thread.Join(5000);
					if (sync_thread.IsAlive)
						sync_thread.IsBackground = true; //will be terminated on close now.

				}

		}

		public IOnlineProvider GetOnlineProvider(string key) {
			if (m_OnlineProviders.Count > 0)
				return m_OnlineProviders[key];
			return null;
		}

		private void InitializeOnlineProviders() {
			m_OnlineProviders = new Dictionary<string, IOnlineProvider>();

			IOnlineProvider[] providers = Util.DiscoverProviders();
			foreach (IOnlineProvider provider in providers) {
				try {
					provider.Initialize(this);
					m_OnlineProviders.Add(provider.Key, provider);
				} catch {
					SetStatus(StatusPriority.eStatusBar, Properties.Resources.Str_ErrProviderInitialization.Replace("%1", provider.Name));
				}
			}
		}

		void OnOptionsChanged(object sender, EventArgs e) {
			m_ValidOnlineOptions = false;

			try {
				if (OnlineProvider != null) {
					try {
						if (!OnlineProvider.IsInitialized)
							OnlineProvider.Initialize(this);
					} catch {
						SetStatus(StatusPriority.eMessageBoxInfo, Properties.Resources.Str_ErrProviderInitialization.Replace("%1", OnlineProvider.Name));
						SetStatus(StatusPriority.eStatusBar, Properties.Resources.Str_ErrProviderInitialization.Replace("%1", OnlineProvider.Name));
						return;
					}

					while (m_ValidDatabase) {
						KeePassSyncErr err = OnlineProvider.ValidateOptions(m_OptionsData);
						string msg = "";
						bool tryEdit = false;

						if (err == KeePassSyncErr.Error) {
							tryEdit = true;
							msg = Properties.Resources.Str_ErrValidatingOptions_Error;
						} else if (err == KeePassSyncErr.Timeout) {
							msg = Properties.Resources.Str_ErrValidatingOptions_Timeout;
						} else if (err == KeePassSyncErr.NotConnected) {
							msg = Properties.Resources.Str_ErrValidatingOptions_NotConnected;
						} else if (err == KeePassSyncErr.None) {
							m_ValidOnlineOptions = true;
							break;
						}

						SetStatus(StatusPriority.eStatusBar, msg);

						if (m_ValidDatabase) {
							// Ask user to edit/create settings
							if (tryEdit)
								msg += "\n\n" + Properties.Resources.Str_EditCreateQuestion.Replace("%1", OnlineProvider.Name);
							else
								msg += "\n\n" + Properties.Resources.Str_TryAgain;

							DialogResult res = MessageBox.Show(msg, Properties.Resources.Str_Title, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
							if (res == DialogResult.Yes) {
								if (tryEdit)
									OnlineProvider.EditEntry(m_OptionsData.PasswordEntry);
							} else {
								m_ValidOnlineOptions = false;
								break;
							}
						}
					}

				} else {
					SetStatus(StatusPriority.eStatusBar, Properties.Resources.Str_ErrNoProviderSelected);
				}
			} catch {
				SetStatus(StatusPriority.eMessageBoxInfo, Properties.Resources.Str_ErrOptions);
				m_ValidOnlineOptions = false;
			}

			m_tsmiSync.Enabled = m_ValidOnlineOptions && m_ValidDatabase;
		}
		void find_online_provider() {
			m_OptionsData.PasswordEntry = OnlineProvider != null ? KeePassSupport.FindEntry(Host, OnlineProvider) : null;
		}

		void OnValidDatabaseChanged(object sender, EventArgs e) {
			if (m_Host.MainWindow.ActiveDatabase != null && m_OptionsProvider != null) {
				find_online_provider();

				OptionsChanged(this, new EventArgs());
			}
		}

		// You cannot abort this process 
		// (it's just a notification and your last chance to clean up all used resources, etc.). 
		// Immediately after you return from this function, KeePass can unload your plugin
		public override void Terminate() {
		}

		public void SetStatus(StatusPriority priority, string msg) {
			switch (priority) {
				case StatusPriority.eMessageBoxFatal:
					MessageBox.Show(msg, Properties.Resources.Str_Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					break;
				case StatusPriority.eMessageBoxInfo:
					MessageBox.Show(msg, Properties.Resources.Str_Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
					break;
				case StatusPriority.eMessageBox:
					MessageBox.Show(msg, Properties.Resources.Str_Title, MessageBoxButtons.OK);
					break;
				case StatusPriority.eStatusBar:
					thread_MainWindow_SetStatusEx(msg);
					break;
			}
		}


		private void OnMenuShowOptions(object sender, EventArgs e) {
			// Set the status bar to the KeePass default
			SetStatus(StatusPriority.eStatusBar, "");
			Forms.OptionsForm optionsForm = new Forms.OptionsForm(this);

			DialogResult res = optionsForm.ShowDialog();
			if (res == DialogResult.OK) {
				KeePassSupport.RefreshGui(Host, optionsForm.EntryChanged);
				if (optionsForm.SavedOptions != m_OptionsData) {
					// Options have changed.  Need to save them and notify a change occurred.
					m_OptionsData = optionsForm.SavedOptions;
					m_OptionsProvider.Write(optionsForm.SavedOptions);
					OptionsChanged(this, new EventArgs());
				}
			}
		}

		/// <summary>
		/// Trys to sync from merging a file from the remote store to the local store.
		/// </summary>
		/// <returns>
		/// 0 - Successful
		/// 1 - Cannot find the file, shouldn't fail the sync though.
		/// 2 - Invalid Options
		/// 3 - Error processing the request (not online, etc.)
		/// </returns>
		private KeePassSyncErr MergeIn() {
			KeePassSyncErr ret = KeePassSyncErr.None;

			try {
				if (m_ValidOnlineOptions) {
					// This could be replaced with a handler for ActiveDatabase.Changed event

					string dbName = Path.GetTempFileName();
					ret = OnlineProvider.GetFile(m_OptionsData.PasswordEntry, GetDatabaseName(m_Host.Database), dbName);

					if (ret == KeePassSyncErr.None) {
						KeePassLib.Serialization.IOConnectionInfo serialinfo = new KeePassLib.Serialization.IOConnectionInfo();
						serialinfo.Path = dbName;

						KeePassLib.Interfaces.IStatusLogger logger = new Log();
						PwDatabase db = new PwDatabase();
						try {
							// if so, try to load the database from google (as a saved temp file)
							db.Open(serialinfo, m_Host.Database.MasterKey, logger);

							// Just in case it was dirty...
							m_Host.Database.Save(logger);

							m_Host.Database.MergeIn(db, PwMergeMethod.Synchronize);
							db.Close();

							// TODO - delete more securely
							File.Delete(serialinfo.Path);
							thread_MainWindow_UpdateUI(false, null, true, true, true, null, db.Modified);
						} catch (Exception e) {
							SetStatus(StatusPriority.eMessageBox, e.Message);
							ret = KeePassSyncErr.Error;
						}
					} else {
						SetStatus(StatusPriority.eStatusBar, "Database not online, stored current database.");
					}
				} else {
					SetStatus(StatusPriority.eStatusBar, "Invalid online provider options");
					ret = KeePassSyncErr.InvalidCredentials;
				}
			} catch {
				SetStatus(StatusPriority.eStatusBar, "Unable to sync (export) database.");
				ret = KeePassSyncErr.Error;
			}

			return ret;
		}
		private delegate void MainWindow_StringArg(String arg);
		private delegate PwGroup MainWindow_VoidArg();
		private MainWindow_StringArg MainWindow_SetStatusEx_del;
		private MainWindow_VoidArg MainWindow_GetSelectedGroup_del;
		public delegate void MainWindow_UpdateUI(bool bRecreateTabBar, KeePass.UI.PwDocument dsSelect, bool bUpdateGroupList, PwGroup pgSelect, bool bUpdateEntryList, PwGroup pgEntrySource, bool bSetModified);
		private MainWindow_UpdateUI MainWindow_UpdateUI_del;
		private void thread_MainWindow_SetStatusEx(String arg) {
			if (main_window_is_closing)
				return;
			if (sync_thread != Thread.CurrentThread) //we are in the original thread
			{
				m_Host.MainWindow.SetStatusEx(arg);
				return;
			}

			if (MainWindow_SetStatusEx_del == null)
				MainWindow_SetStatusEx_del = new MainWindow_StringArg(m_Host.MainWindow.SetStatusEx);
			if (main_window_is_closing)
				return;
			m_Host.MainWindow.Invoke(MainWindow_SetStatusEx_del, new object[] { arg });
		}
		private void thread_MainWindow_UpdateUI(bool bRecreateTabBar, KeePass.UI.PwDocument dsSelect, bool bUpdateGroupList, bool CurGroupNotRootGroup, bool bUpdateEntryList, PwGroup pgEntrySource, bool bSetModified) {
			if (main_window_is_closing)
				return;
			if (MainWindow_GetSelectedGroup_del == null)
				MainWindow_GetSelectedGroup_del = new MainWindow_VoidArg(m_Host.MainWindow.GetSelectedGroup);

			if (MainWindow_UpdateUI_del == null)
				MainWindow_UpdateUI_del = new MainWindow_UpdateUI(m_Host.MainWindow.UpdateUI);
			PwGroup pgSelect;
			if (CurGroupNotRootGroup)
				pgSelect = (PwGroup)m_Host.MainWindow.Invoke(MainWindow_GetSelectedGroup_del);
			else
				pgSelect = m_Host.Database.RootGroup;
			if (main_window_is_closing)
				return;
			m_Host.MainWindow.Invoke(MainWindow_UpdateUI_del, new object[] { bRecreateTabBar, dsSelect, bUpdateGroupList, pgSelect, bUpdateEntryList, pgEntrySource, bSetModified });

		}

		private Thread sync_thread;
		private void thread_sync() {
			try {
				find_online_provider();
				if (!m_Host.Database.IsOpen || OnlineProvider == null)
					return;
				String provider_name = OnlineProvider.Name ?? "Syncer";

				SetStatus(StatusPriority.eStatusBar, Properties.Resources.Str_Syncing.Replace("%1", provider_name));
				// This should read online database, merge and save it back out
				KeePassSyncErr status = MergeIn();
				if (status == KeePassSyncErr.None || status == KeePassSyncErr.FileNotFound) {
					status = MergeOut();
					if (status != KeePassSyncErr.None)
						SetStatus(StatusPriority.eStatusBar, Properties.Resources.Str_ErrSync);
					else
						SetStatus(StatusPriority.eStatusBar, Properties.Resources.Str_Synced.Replace("%1", provider_name));
				}
			} catch (Exception e) {
				MessageBox.Show(e.Message + "\r\n" + e.StackTrace);
				Debugger.Launch();
			}
		}
		private void Sync(bool in_background) {
			if (sync_thread != null) {
				if (sync_thread.IsAlive) {
					MessageBox.Show("Already syncing refusing to sync again...");
					return;
				}
				sync_thread.Join();
				sync_thread = null;
			}
			if (m_ValidOnlineOptions) {
				bool added = KeePassSupport.CheckEntry(m_Host, m_OptionsData.PasswordEntry);
				if (added)
					ValidDatabaseChanged(this, new EventArgs());
			}
			if (in_background) {
				sync_thread = new Thread(thread_sync);
				sync_thread.Start();
				return;
			}
			thread_sync();
		}

		private void OnMenuSync(object sender, EventArgs e) {
			// Set the status bar to the KeePass default
			SetStatus(StatusPriority.eStatusBar, "");
			Sync(false);
		}

		/// <summary>
		/// Loads the database from an online source and asks user for a location.
		/// </summary>
		/// <param name="onlineFilename"></param>
		/// <param name="newLocation"></param>
		/// <returns>Local filename of database</returns>
		private bool StoreOnlineDb(string onlineFilename, out string newLocation) {
			bool ret = false;
			newLocation = null;

			try {
				// Select the folder to store it into
				newLocation = GetNewLocation();
				if (newLocation != null) {
					if (!newLocation.EndsWith("\\")) {
						newLocation += "\\";
					}
					newLocation += onlineFilename;

					// Load file from online provider and save locally
					KeePassSyncErr status = OnlineProvider.GetFile(m_OptionsData.PasswordEntry, onlineFilename, newLocation);
					if (status == KeePassSyncErr.None) {
						ret = true;
					} else {
						newLocation = null;
					}
				}
			} catch {
				SetStatus(StatusPriority.eStatusBar, Properties.Resources.Str_ErrSync);
			}

			return ret;
		}

		/// <summary>
		/// Download the database from its online source and opens it
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnMenuOpen(object sender, EventArgs e) {
			string[] databases = null;
			PwEntry oldEntry = m_OptionsData.PasswordEntry;
			OpenForm openForm;
			bool success = false;

			// Set the status bar to the KeePass default
			SetStatus(StatusPriority.eStatusBar, "");

			try {

				openForm = new OpenForm(this);
				if (m_OptionsData.PasswordEntry != null) {
					openForm.DecodeEntry(m_OptionsData.PasswordEntry);
				}
				DialogResult res = openForm.ShowDialog();
				if (DialogResult.OK == res) {
					if (openForm.Entry != null) {
						try {
							m_OptionsData.PasswordEntry = openForm.Entry.CloneDeep();

							m_OptionsData.OnlineProviderKey = openForm.OnlineProviderKey;
							databases = OnlineProvider.GetDatabases(m_OptionsData.PasswordEntry);

							if (databases == null) {
								SetStatus(StatusPriority.eMessageBoxInfo, "No databases found.");
							}
						} catch {
							SetStatus(StatusPriority.eMessageBoxInfo, Properties.Resources.Str_ErrRetrieveDatabase);
						}
					} else {
						SetStatus(StatusPriority.eMessageBoxInfo, "Temp entry couldn't be created.");
					}
				}
			} catch {
				SetStatus(StatusPriority.eMessageBoxInfo, Properties.Resources.Str_ErrOpenDatabase);
			}

			try {
				if (databases != null) {
					SelectDatabaseForm openDlg = new SelectDatabaseForm(this);
					openDlg.SetList(databases);
					DialogResult resDb = openDlg.ShowDialog();
					switch (resDb) {
						case DialogResult.OK:
							string databaseName = openDlg.SelectedDatabase;
							string newLocation;
							if (StoreOnlineDb(databaseName, out newLocation)) {
								if (OpenDb(newLocation))
									success = true;
							}
							break;
					}
				}
			} catch {
				SetStatus(StatusPriority.eMessageBoxInfo, Properties.Resources.Str_ErrOpenDatabase);
			}

			if (!success) {
				m_OptionsData.PasswordEntry = oldEntry;
			}
		}

		private bool OpenDb(string newLocation) {
			bool success = false;

			if (newLocation != null) {
				// open the database
				KeePassLib.Serialization.IOConnectionInfo serialinfo = new KeePassLib.Serialization.IOConnectionInfo();
				serialinfo.Path = newLocation;
				KeePassLib.Interfaces.IStatusLogger logger = new Log();
				new PwDatabase();

				while (!success) {
					try {
						KeePassLib.Keys.CompositeKey key = GetPassword();
						if (key == null)
							break;
						m_Host.Database.Open(serialinfo, key, logger);
						m_Host.MainWindow.UpdateUI(false, null, true, m_Host.MainWindow.GetSelectedGroup(), true, null, false);

						success = true;

						// For some reason, we don't get a KeePass notification of file opened here
						m_ValidDatabase = true;
						ValidDatabaseChanged(this, new EventArgs());
					} catch (KeePassLib.Keys.InvalidCompositeKeyException e) {
						SetStatus(StatusPriority.eMessageBox, e.Message);
					} catch (Exception e) {
						SetStatus(StatusPriority.eMessageBox, e.Message);
						break;
					}
				}
			}
			return success;
		}

		private string GetNewLocation() {
			string ret = null;

			FolderBrowserDialog dlg = new FolderBrowserDialog();
			// Set the help text description for the FolderBrowserDialog.
			dlg.Description = "Select destination directory for downloaded database.";
			dlg.ShowNewFolderButton = true;

			// Default to the My Documents folder.
			dlg.RootFolder = Environment.SpecialFolder.Desktop;
			if (m_OptionsData != null && m_OptionsData.PreviousDatabaseLocation != null)
				dlg.SelectedPath = m_OptionsData.PreviousDatabaseLocation;
			else
				dlg.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);


			// We use this loop to make sure that we have access to the folder in question.
			bool tryAgain = true;
			while (tryAgain) {
				DialogResult res = dlg.ShowDialog();
				switch (res) {
					case DialogResult.OK:
						// Make sure we have access to the selected folder
						string tempfile = dlg.SelectedPath + "\\del.tmp";
						try {
							FileStream fs = File.OpenWrite(tempfile);
							if (fs == null) {
								SetStatus(StatusPriority.eMessageBox, "Unable to create database in " + dlg.SelectedPath + ".  \rPlease select another folder.");
								break;
							}
							fs.Close();
							File.Delete(tempfile);
							m_OptionsData.PreviousDatabaseLocation = dlg.SelectedPath;
							ret = dlg.SelectedPath;
							tryAgain = false;

							// We just changed the options, time to save them
							m_OptionsProvider.Write(m_OptionsData);

						} catch (UnauthorizedAccessException) {
							SetStatus(StatusPriority.eMessageBoxFatal, "You cannot create a database in " + dlg.SelectedPath);
						} catch (Exception e) {
							SetStatus(StatusPriority.eMessageBox, e.Message);
						}
						break;
					case DialogResult.Cancel:
						tryAgain = false;
						break;
					default:
						break;
				}
			}
			return ret;
		}

		private KeePassLib.Keys.CompositeKey GetPassword() {
			KeyPromptForm dlg = new KeyPromptForm();
			dlg.ShowDialog();
			if (dlg.HasClosedWithExit)
				return null;
			return dlg.CompositeKey;
		}

		private void OnFileClosed(object sender, EventArgs e) {
			m_ValidDatabase = false;
			m_OptionsData.PasswordEntry = null;
			ValidDatabaseChanged(this, new EventArgs());
		}

		private void OnFileOpened(object sender, EventArgs e) {
			m_ValidDatabase = true;
			ValidDatabaseChanged(this, new EventArgs());
		}

		private void OnFileCreated(object sender, EventArgs e) {
			m_ValidDatabase = true;
			ValidDatabaseChanged(this, new EventArgs());
		}

		private string GetDatabaseName(PwDatabase db) {
			int index = db.IOConnectionInfo.Path.LastIndexOf('\\');
			string name = db.IOConnectionInfo.Path.Substring(index + 1);
			return name;
		}

		private void OnFileSaved(object sender, FileSavedEventArgs e) {
			Sync(true);
		}

		private KeePassSyncErr MergeOut() {
			KeePassSyncErr ret = KeePassSyncErr.Error;

			try {
				if (m_ValidOnlineOptions) {
					KeePassLib.Interfaces.IStatusLogger logger = new Log();

					// Just in case it was dirty...
					m_Host.Database.Save(logger);

					// Backup the database online
					PwDatabase db = m_Host.Database;
					KeePassLib.Serialization.IOConnectionInfo serialinfo = new KeePassLib.Serialization.IOConnectionInfo();

					serialinfo.Path = Path.GetTempFileName();
					db.SaveAs(serialinfo, false, logger);

					// Save the database from the temp database just made with the name of the main database
					ret = OnlineProvider.PutFile(m_OptionsData.PasswordEntry, GetDatabaseName(m_Host.Database), serialinfo.Path);

					// TODO - delete more securely
					File.Delete(serialinfo.Path);

					if (ret != KeePassSyncErr.None) {
						SetStatus(StatusPriority.eStatusBar, "Unable to save the database.");
					}
				} else {
					SetStatus(StatusPriority.eStatusBar, "Invalid online provider options.");
					ret = KeePassSyncErr.InvalidCredentials;
				}
			} catch {
				SetStatus(StatusPriority.eStatusBar, "Unable to sync (export) database.");
				ret = KeePassSyncErr.Error;
			}

			return ret;
		}

	}



	public class Log : KeePassLib.Interfaces.IStatusLogger {
		#region IStatusLogger Members

		public bool ContinueWork() {
			return true;
		}

		public void EndLogging() {
			return;
		}

		public bool SetProgress(uint uPercent) {
			return true;
		}

		public bool SetText(string strNewText, KeePassLib.Interfaces.LogStatusType lsType) {
			return true;
		}

		public void StartLogging(string strOperation, bool bWriteOperationToLog) {
			throw new NotImplementedException();
		}

		#endregion
	}
}