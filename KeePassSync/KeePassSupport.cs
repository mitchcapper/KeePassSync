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
using System.Windows.Forms;
using System.Diagnostics;

using KeePass.Plugins;
using KeePass.Forms;
using KeePass.Resources;
using KeePassLib;
using KeePassLib.Security;

namespace KeePassSync {
	public class KeePassSupport {
		public static PwEntry CreateAndEditEntry(IPluginHost host, string title) {
			PwEntry entry = null;
			if (host != null && host.Database != null) {
				entry = new PwEntry(true, true);
				host.Database.RootGroup.AddEntry(entry, false);

				if (entry != null) {
					PwGroup destGroup = host.Database.RootGroup;

					// Set some of the string fields
					entry.Strings.Set(PwDefs.TitleField, new ProtectedString(false, title));
					entry.Strings.Set(PwDefs.NotesField, new ProtectedString(false, Properties.Resources.Str_EntryNotes));
					// Finally tell the parent group that it owns this entry now

					if (destGroup != null) {
						destGroup.Entries.Add(entry);
					}

					PwEntryForm form = new PwEntryForm();
					form.InitEx(entry, PwEditMode.EditExistingEntry, host.Database, host.MainWindow.ClientIcons, false, false);
					DialogResult res = form.ShowDialog();

					if (res == DialogResult.OK) {
						host.MainWindow.UpdateUI(false, null, true, null, true, null, true);
					} else {
						DeleteEntry(host, entry);
						entry = null;
					}
				}
			}
			return entry;
		}

		public static PwEntry CreateEntry(IPluginHost host, string title) {
			PwEntry entry = null;
			if (host != null && host.Database != null) {
				entry = new PwEntry(true, true);
				if (entry != null) {
					PwGroup destGroup = host.Database.RootGroup;

					// Set some of the string fields
					entry.Strings.Set(PwDefs.TitleField, new ProtectedString(false, title));
					entry.Strings.Set(PwDefs.NotesField, new ProtectedString(false, Properties.Resources.Str_EntryNotes));
					// Finally tell the parent group that it owns this entry now

					if (destGroup != null) {
						destGroup.AddEntry(entry, true, true);
						host.MainWindow.UpdateUI(false, null, true, null, true, null, true);
					}
				}
			}
			return entry;
		}

		public static void DeleteEntry(IPluginHost host, PwEntry entry) {
			entry.ParentGroup.Entries.Remove(entry);
			host.MainWindow.UpdateUI(false, null, true, null, true, null, true);
		}

		public static void UpdateEntryName(IPluginHost host, PwEntry entry, IOnlineProvider provider) {
			if (entry != null) {
				entry.Strings.Set(PwDefs.TitleField, new ProtectedString(false, Properties.Resources.Str_PasswordEntryTemplate.Replace("%1", provider.Name)));
				host.MainWindow.UpdateUI(false, null, true, null, true, null, true);
			}
		}

		public static void RefreshGui(IPluginHost host, bool modified) {
			host.MainWindow.UpdateUI(false, null, true, null, true, null, modified);
		}

		public static bool EditEntry(IPluginHost host, PwEntry entry) {
			bool ret = false;
			if (entry != null) {
				PwEntryForm form = new PwEntryForm();
				form.InitEx(entry, PwEditMode.EditExistingEntry, host.Database, host.MainWindow.ClientIcons, false, false);
				DialogResult res = form.ShowDialog();

				if (res == DialogResult.OK) {
					host.MainWindow.UpdateUI(false, null, true, null, true, null, true);
					ret = true;
				}
			}

			return ret;
		}

		public static PwEntry FindEntry(IPluginHost host, IOnlineProvider provider) {
			Debug.Assert(host != null, "Invalid host");
			Debug.Assert(provider != null, "Invalid IOnlineProvider");
			PwEntry entry = null;

			string[] acceptedNames = provider.AcceptedNames;
			for (int i = 0; i < acceptedNames.Length; i++) {
				entry = KeePassSupport.GetEntry(host, Properties.Resources.Str_PasswordEntryTemplate.Replace("%1", acceptedNames[i]));
				if (entry != null)
					break;
			}

			return entry;
		}

		/// <summary>
		/// This finds an entry in the database.
		/// </summary>
		/// <param name="host">KeePass service handle</param>
		/// <param name="title">Entry title to find</param>
		/// <returns></returns>
		public static PwEntry GetEntry(IPluginHost host, string title) {
			Debug.Assert(host != null, "Invalid host");
			// Finally add our new group to an existing group as subgroup
			PwGroup group = host.Database.RootGroup;
			PwEntry entry = null;
			if (group != null) {
				// Find the entry
				KeePassLib.Collections.PwObjectList<PwEntry> entries = host.Database.RootGroup.Entries;
				ProtectedString ps = new ProtectedString(false, title);
				for (uint i = 0; i < entries.UCount; i++) {
					if (entries.GetAt(i).Strings.Get(PwDefs.TitleField).ReadString() == title) {
						entry = entries.GetAt(i);
						break;
					}
				}
			}

			return entry;
		}

		/// <summary>
		/// Checks to see if the entry is in the active database, if not, create a new
		/// entry with the same values.
		/// </summary>
		/// <param name="?"></param>
		/// <param name="entry"></param>
		/// <returns>True if entry was just added to the current database.</returns>
		public static bool CheckEntry(IPluginHost host, PwEntry entry) {
			bool ret = false;
			if (entry == null)
				return false;
			PwEntry newEntry = host.Database.RootGroup.FindEntry(entry.Uuid, true);
			if (newEntry == null) {
				newEntry = entry.CloneDeep();

				if (newEntry != null) {
					PwGroup destGroup = host.Database.RootGroup;
					// Finally tell the parent group that it owns this entry now
					destGroup.Entries.Add(newEntry);
					host.MainWindow.UpdateUI(false, null, true, null, true, null, true);
					ret = true;
				}
			}
			return ret;
		}
	}
}
