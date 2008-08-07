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

using System.Windows.Forms;

//using KeePass.Plugins;
//using KeePass.Forms;
//using KeePass.Resources;
using KeePassLib;
using System;
//using KeePassLib.Security; 

namespace KeePassSync
{
    public abstract class IOnlineProvider : ICloneable
    {
        protected KeePassSyncExt m_MainInterface;
        protected OptionsData m_OptionData;
        protected bool m_HandlerAdded = false;
        protected bool m_IsInitialized = false;

        /// <summary>
        /// Has the online provider been initialized for its internal services yet?
        /// </summary>
        public bool IsInitialized { get { return m_IsInitialized; } }
        
        /// <summary>
        /// Link for the user to create an account.  This must start with either "http://" or "https://" to work with the options dialog linkage.
        /// </summary>
        public abstract string CreateAccountLink { get; }

        /// <summary>
        /// Preferred friendly-name of the provider.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Full path of the online provider's assembly.
        /// </summary>
        public string Path;

        /// <summary>
        /// Available friendly-names of the provider.  Useful when a developer needs to rename his plugin or find a plugin that doesn't match the new name.
        /// </summary>
        public abstract string[] AcceptedNames { get; }

        /// <summary>
        /// Retrieves an array of the available databases from the online provider.  Provider must have valid options.
        /// </summary>
        public abstract string[] Databases { get; }

        /// <summary>
        /// Key used in the options and as a hash to retrieve the OnlineProvider from the main interface.
        /// </summary>
        public string Key { get { return Path; } }

        public void OptionsChangedHandler( object sender, EventArgs e )
        {
            m_OptionData = m_MainInterface.Options;
        }


        /// <summary>
        /// This initializes the plugin and the plugin can then use the options provider to retrieve the 
        /// latest options before every save and load.  In the future, this could be optimized to receive
        /// an event whenever the options have been changed.  (detected through watching the keys via win32 api)
        /// 
        /// This MUST be called from a client provider, ala base.Initialize( mainInterface )
        /// </summary>
        /// <param name="options"></param>
        public virtual KeePassSyncErr Initialize( KeePassSyncExt mainInterface )
        {
            m_MainInterface = mainInterface;
            m_IsInitialized = true;

            // Don't add the handler twice
            if ( !m_HandlerAdded )
            {
                m_MainInterface.OptionsChanged += this.OptionsChangedHandler;
                m_HandlerAdded = true;
            }

            m_OptionData = m_MainInterface.Options;

            return KeePassSyncErr.None;
        }

        ~IOnlineProvider()
        {
            if ( m_MainInterface != null && m_HandlerAdded )
            {
                m_MainInterface.OptionsChanged -= OptionsChangedHandler;
            }
        }

        /// <summary>
        /// Stores a local file online.
        /// </summary>
        /// <param name="remoteFilename">Remote filename</param>
        /// <param name="localFilename">Local filename</param>
        /// <returns>True if successful, false otherwise.</returns>
        public abstract KeePassSyncErr PutFile( PwEntry entry, string remoteFilename, string localFilename );
        
        /// <summary>
        /// Retrieves an online file into a local file.
        /// </summary>
        /// <param name="remoteFilename">Remote filename</param>
        /// <param name="localFilename">Local filename</param>
        /// <returns>
        /// 0 - Successful
        /// 1 - File not found
        /// 2 - Error
        /// </returns>
        public abstract KeePassSyncErr GetFile( PwEntry entry, string remoteFilename, string localFilename );

        /// <summary>
        /// Checks to see if options are valid for the online provider.  Tries to connect to the service and validate the username/password information.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public abstract KeePassSyncErr ValidateOptions( OptionsData options );

        /// <summary>
        /// Retrieves an array of the available databases from the online provider.  Provider doesn't need to have valid options.
        /// </summary>
        /// <param name="user">User for connection.</param>
        /// <param name="password">Password for connection.</param>
        /// <param name="optionalData">Optional data for the provider.  For FTP, this is a combination of FTP address and directory.</param>
        /// <returns></returns>
        /// @todo user/password encrypted
        public abstract string[] GetDatabases( PwEntry entry );

        /// <summary>
        /// Returns the UserControl to display within option dialogs.
        /// </summary>
        /// <returns>UserControl to be used within option dialogs</returns>
        public abstract UserControl GetUserControl();

        /// <summary>
        /// This takes an existing entry and populates the internal provider's options control with
        /// information parsed from the entry.  A dialog is displayed and the results of the dialog 
        /// will then be recombined into a new entry.  If the dialog is cancelled, the returned
        /// entry will be null.
        /// </summary>
        /// <param name="entry">Starting entry to populate the internal account details control.</param>
        /// <returns>A regenerated KeePass entry based on the user's modifications of the account details.  If cancel is hit on the dialog, a null entry is returned.</returns>
        public bool EditEntry( PwEntry entry )
        {
            bool ret = false;

            KeePassSync.Forms.AccountEntryGenerator dialog = new KeePassSync.Forms.AccountEntryGenerator( m_MainInterface, this );

            if ( dialog != null )
            {
                if ( entry != null )
                    dialog.DecodeEntry( entry );

                DialogResult res = dialog.ShowDialog();
                if ( res == DialogResult.OK )
                {
                    dialog.EncodeEntry( entry );
                    ret = true;
                }
            }

            return ret;
        }

        /// <summary>
        /// Populates control with entry data.  This uses the providers internal UserControl for data.
        /// </summary>
        /// <param name="entry"></param>
        abstract public void DecodeEntry( PwEntry entry );

        /// <summary>
        /// Populates entry with control data.  This uses the providers internal UserControl for data.
        /// </summary>
        /// <param name="entry"></param>
        abstract public void EncodeEntry( PwEntry entry );

        public override string ToString()
        {
            return Name;
        }

        #region ICloneable Members

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
