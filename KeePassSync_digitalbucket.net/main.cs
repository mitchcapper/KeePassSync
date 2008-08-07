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
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

using KeePassSync;

using KeePass.Plugins;
using KeePass.Forms;
using KeePass.Resources;
using KeePassLib;
using KeePassLib.Security;

using KeePassSync_digitalbucket.Forms;

namespace KeePassSync_digitalbucket
{
    // This provider stores databases in the root folder right now...
    public class CDigitalbucket : IOnlineProvider
    {
        #region -- Private data --
        private digitalbucket.net.rest.DigitalBucketConnection m_Service;
        private const string m_ServiceAddress = "https://www.digitalbucket.net/api/rest/";
        private const string m_AccountLink = "http://www.digitalbucket.net/Members/Pricing.aspx?ref=KeePassSync";
        private const string m_Name = "digitalbucket.net";
        private const string ONLINE_DB_PREFIX = "Keepass-";
        private AccountDetails m_UserControl = null;
        private string[] m_AcceptedNames = { "digitalbucket.net", "Digital Bucket" };
        #endregion

        public override KeePassSyncErr Initialize( KeePassSyncExt mainInterface )
        {
            KeePassSyncErr ret = base.Initialize( mainInterface );

            if ( ret == KeePassSyncErr.None )
            {
                m_UserControl = new KeePassSync_digitalbucket.Forms.AccountDetails();

                if ( m_OptionData != null )
                {
                    ret = InitializeService();
                }
                else
                {
                    ret = KeePassSyncErr.InvalidCredentials;
                }
            }

            m_IsInitialized = ( ret == KeePassSyncErr.None );

            return ret;
        }


        public override KeePassSyncErr ValidateOptions( OptionsData options )
        {
            m_OptionData = options;
            return InitializeService();
        }

        /// <summary>
        /// First delete the file and then save it.  This service provider automatically
        /// renames the sent file much like Windows Explorer does when there is a duplicate
        /// filename on copy.
        /// </summary>
        /// <param name="databaseName"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public override KeePassSyncErr PutFile( PwEntry entry, string databaseName, string filename )
        {
            KeePassSyncErr ret = KeePassSyncErr.Error;

            // Make the database name conform to our internal naming convention
            string realDbName = ONLINE_DB_PREFIX + databaseName;

            digitalbucket.net.rest.CustomResponse<digitalbucket.net.rest.Folder> res = m_Service.GetRootFolder();
            if ( res.Success == true )
            {
                digitalbucket.net.rest.Folder currentFolder = res.ResponseObject;

                // Delete the file first
                // Must retrieve the file
                foreach ( digitalbucket.net.rest.File file in currentFolder.ChildFiles )
                {
                    if ( file.FileName.ToLower() == realDbName.ToLower() )
                    {
                        m_Service.DeleteFile( file.FileID );
                        break;
                    }
                }

                digitalbucket.net.rest.Response rsp = m_Service.PutFile( currentFolder.FolderID, realDbName, System.IO.File.Open( filename, System.IO.FileMode.Open ) );
                if ( rsp.Success )
                {
                    ret = KeePassSyncErr.None;
                }
            }
            return ret;
        }

        public override KeePassSyncErr GetFile( PwEntry entry, string databaseName, string filename )
        {
            digitalbucket.net.rest.File onlineFile = null;
            KeePassSyncErr ret = KeePassSyncErr.Error; // default to error

            // Make the database name conform to our internal naming convention
            string realDbName = ONLINE_DB_PREFIX + databaseName;

            digitalbucket.net.rest.CustomResponse<digitalbucket.net.rest.Folder> res = m_Service.GetRootFolder();
            if ( res.Success == true )
            {
                digitalbucket.net.rest.Folder currentFolder = res.ResponseObject;
                foreach ( digitalbucket.net.rest.File file in currentFolder.ChildFiles )
                {
                    if ( file.FileName.ToLower() == realDbName.ToLower() )
                    {
                        onlineFile = file;
                        break;
                    }
                }

                if ( onlineFile != null )
                {
                    ret = GetFileOnline( onlineFile, filename );
                }
            }
            return ret;
        }

        public override string CreateAccountLink
        {
            get { return m_AccountLink; }
        }

        public override string Name
        {
            get { return m_Name; }
        }

        public override string[] AcceptedNames
        {
            get { return m_AcceptedNames; }
        }

        public override string[] Databases
        {
            get
            {
                ArrayList databases = new ArrayList();
                digitalbucket.net.rest.CustomResponse<digitalbucket.net.rest.Folder> res = m_Service.GetRootFolder();
                if ( res.Success == true )
                {
                    digitalbucket.net.rest.Folder currentFolder = res.ResponseObject;

                    foreach ( digitalbucket.net.rest.File file in currentFolder.ChildFiles )
                    {
                        if ( file.FileName.ToLower().StartsWith( ONLINE_DB_PREFIX.ToLower() ) )
                        {
                            databases.Add( file.FileName.Substring( ONLINE_DB_PREFIX.Length ) );
                        }
                    }
                }
                else
                {
                    m_MainInterface.SetStatus( StatusPriority.eMessageBoxInfo, "Cannot retrieve online information from " + Name + " service (" + res.StatusDescription + ")." );
                }
                return (string[])databases.ToArray( typeof( string ) );
            }
        }

        private KeePassSyncErr GetFileOnline( digitalbucket.net.rest.File file, string filename )
        {
            KeePassSyncErr ret = KeePassSyncErr.FileNotFound;

            digitalbucket.net.rest.StreamResponse rsp = m_Service.GetFile( file.FileID );
            if ( rsp.Success == true )
            {
                using ( System.IO.Stream s = rsp.ResponseStream )
                {
                    using ( System.IO.Stream streamWriter = File.OpenWrite( filename ) )
                    {
                        int size = 2048;
                        byte[] data = new byte[ 2048 ];
                        while ( true )
                        {
                            size = s.Read( data, 0, data.Length );
                            if ( size > 0 )
                            {
                                streamWriter.Write( data, 0, size );
                                ret = KeePassSyncErr.None;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
            return ret;
        }

        private KeePassSyncErr InitializeService()
        {
            KeePassSyncErr ret = KeePassSyncErr.Error;

            try
            {
                m_Service = null;
                if ( m_OptionData != null )
                {
                    if ( m_OptionData.PasswordEntry != null && m_OptionData.PasswordEntry.Strings.Exists( PwDefs.UserNameField ) && m_OptionData.PasswordEntry.Strings.Exists( PwDefs.PasswordField ) )
                    {
                        ProtectedString user = m_OptionData.PasswordEntry.Strings.Get( PwDefs.UserNameField );
                        ProtectedString pw = m_OptionData.PasswordEntry.Strings.Get( PwDefs.PasswordField );

                        m_MainInterface.SetStatus( StatusPriority.eStatusBar, "Initializing " + Name + " service..." );
                        m_Service = new digitalbucket.net.rest.DigitalBucketConnection( user.ReadString(), pw.ReadString(), m_ServiceAddress );
                        digitalbucket.net.rest.CustomResponse<digitalbucket.net.rest.Folder> res = m_Service.GetRootFolder();
                        if ( res.Success == false )
                        {
                            m_Service = null;
                            m_MainInterface.SetStatus( StatusPriority.eStatusBar, "Error initializing " + Name + " service (" + res.StatusDescription + ")." );
                        }
                        else
                        {
                            m_MainInterface.SetStatus( StatusPriority.eStatusBar, Name + " service initialized." );
                        }
                    }
                }
            }
            catch
            {
                ret = KeePassSyncErr.Error;
            }

            if ( m_Service != null )
                ret = KeePassSyncErr.None;

            return ret;
        }

        // This means that it's called from the open form, there is no service or valid option data
        public override string[] GetDatabases( PwEntry entry )
        {
            Debug.Assert( entry != null, "Invalid entry" );

            string[] databases = null;

            digitalbucket.net.rest.DigitalBucketConnection prevService = m_Service;

            m_Service = new digitalbucket.net.rest.DigitalBucketConnection(
                entry.Strings.Get( PwDefs.UserNameField ).ReadString(),
                entry.Strings.Get( PwDefs.PasswordField ).ReadString(),
                m_ServiceAddress );

            databases = Databases;

            m_Service = prevService;

            return databases;
        }

        public override System.Windows.Forms.UserControl GetUserControl()
        {
            return m_UserControl;
        }

        public override void DecodeEntry( PwEntry entry )
        {
            Debug.Assert( entry != null, "Invalid entry" );

            if ( entry.Strings.Get( PwDefs.UserNameField ) != null )
                m_UserControl.Username = entry.Strings.Get( PwDefs.UserNameField ).ReadString();

            if ( entry.Strings.Get( PwDefs.PasswordField ) != null )
                m_UserControl.Password = entry.Strings.Get( PwDefs.PasswordField ).ReadString();
        }

        public override void EncodeEntry( PwEntry entry )
        {
            Debug.Assert( entry != null, "Invalid entry" );

            entry.Strings.Set( PwDefs.UserNameField, new ProtectedString( false, m_UserControl.Username ) );
            entry.Strings.Set( PwDefs.PasswordField, new ProtectedString( false, m_UserControl.Password ) );
        }
   }
}
