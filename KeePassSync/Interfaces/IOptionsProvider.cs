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

namespace KeePassSync {
	/// <summary>
	/// Interface for the Options provider.  This is an interface so that it's easily
	/// able to be switched from one implementation to another.  Currently the only
	/// implementation is using the registry to store the options.
	/// </summary>
	public interface IOptionsProvider {
		/// <summary>
		/// This method stores the main plugin interface so that subsequent reads/writes
		/// have access to the one and only options object.
		/// </summary>
		/// <param name="mainInterface">Main plugin interface.</param>
		void Initialize(KeePassSyncExt mainInterface);

		/// <summary>
		/// Reads option information from local store and puts into main interface's options object.
		/// </summary>
		/// <returns></returns>
		bool Read(OptionsData mainOptions);

		/// <summary>
		/// Writes the options from the main interface to local store.
		/// </summary>
		/// <returns></returns>
		bool Write(OptionsData mainOptions);
	}
}
