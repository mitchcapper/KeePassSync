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

using KeePassSync_FTP.Forms;

namespace KeePassSync_FTP
{
    public class FtpProvider : IOnlineProvider
    {
        #region -- Private data --
        private const string m_Name = "SCP/FTP";
        private const string ONLINE_DB_PREFIX = "Keepass-";
        private string[] m_AcceptedNames = { "SCP/FTP" };
        private AccountDetails m_UserControl = null;
        #endregion

        private string KeepassDirFilename { get { return KeePassSyncExt.PluginDirectory + "\\KeePassSyncDir.txt"; } }

        public override KeePassSyncErr Initialize( KeePassSyncExt mainInterface )
        {
            KeePassSyncErr ret = base.Initialize( mainInterface );

            m_UserControl = new KeePassSync_FTP.Forms.AccountDetails();
            
            m_IsInitialized = ( ret == KeePassSyncErr.None );

            return ret;
        }

        public override KeePassSyncErr ValidateOptions(OptionsData options)
        {
            KeePassSyncErr ret = KeePassSyncErr.Error;
            OptionsData oldOptions = m_OptionData;
            m_OptionData = options;

            try
            {
                System.Diagnostics.Process process = new Process();

                PwEntry entry = m_OptionData.PasswordEntry;

                // just issue command to see if timeout occurs
                string commandStr = "-pw " +
                    entry.Strings.Get( PwDefs.PasswordField ).ReadString() +
                    " " +
                    entry.Strings.Get( PwDefs.UserNameField ).ReadString() +
                    "@" +
                    entry.Strings.Get( PwDefs.UrlField ).ReadString() +
                    " ls";
                process.StartInfo.FileName = KeePassSyncExt.PluginDirectory + "\\plink.exe";
                process.StartInfo.Arguments = commandStr;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.WorkingDirectory = KeePassSyncExt.PluginDirectory;
                process.Start();
                bool exitedOnTime = process.WaitForExit( 5000 );

                if ( exitedOnTime )
                {
                    ret = KeePassSyncErr.None;
                }
                else
                {
                    m_MainInterface.SetStatus( StatusPriority.eMessageBoxInfo, "A timeout occurred validating options using FTP/SCP provider.\n\nAre you sure you've authenticated your server?\nSee docs\\KeePassSync-readme.txt for details." );
                    ret = KeePassSyncErr.Timeout;
                }


            }
            catch
            {
            }

            m_OptionData = oldOptions;
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
        public override KeePassSyncErr PutFile( PwEntry entry, string databaseName, string filename )
        {
            string commandStr;
            KeePassSyncErr ret = KeePassSyncErr.Error;
            System.Diagnostics.Process process = new Process();

            string directory = entry.Strings.Get( "DirectoryField" ).ReadString();
            if ( directory != "" && !directory.EndsWith("/") )
                directory += "/";

            string remoteDatabaseName = ONLINE_DB_PREFIX + databaseName;

            // Backup the existing file
            string backupName = remoteDatabaseName + ".bak";
            commandStr = "-pw " +
                entry.Strings.Get( PwDefs.PasswordField ).ReadString() +
                " " +
                entry.Strings.Get( PwDefs.UserNameField ).ReadString() +
                "@" +
                entry.Strings.Get( PwDefs.UrlField ).ReadString() +
                " \"mv " +
                directory + remoteDatabaseName +
                " " + directory + backupName +
                "\"";
            process.StartInfo.FileName = KeePassSyncExt.PluginDirectory + "\\plink.exe";
            process.StartInfo.Arguments = commandStr;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.WorkingDirectory = KeePassSyncExt.PluginDirectory;
            process.Start();
            bool exitedOnTime = process.WaitForExit( 7000 );


            // Send the new file
            if ( exitedOnTime )
            {
                commandStr = "-pw "
                    + entry.Strings.Get( PwDefs.PasswordField ).ReadString()
                    + " "
                    + "\"" + filename + "\""
                    + " "
                    + entry.Strings.Get( PwDefs.UserNameField ).ReadString()
                    + "@"
                    + entry.Strings.Get( PwDefs.UrlField ).ReadString()
                    + ":"
                    + directory
                    + remoteDatabaseName;

                process.StartInfo.FileName = KeePassSyncExt.PluginDirectory + "\\pscp.exe";
                process.StartInfo.Arguments = commandStr;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.WorkingDirectory = KeePassSyncExt.PluginDirectory;
                process.Start();
                exitedOnTime = process.WaitForExit( 7000 );
            }

            if ( exitedOnTime )
            {
                // Check to see if it's there
                ret = CheckRemoteFileExists( m_OptionData.PasswordEntry, remoteDatabaseName );
            }
            else
            {
                ret = KeePassSyncErr.Timeout;
            }


            return ret;
        }

        private KeePassSyncErr CheckRemoteFileExists( PwEntry entry, string remoteDatabaseName )
        {
            KeePassSyncErr ret = KeePassSyncErr.Error;

            try
            {
                ret = RemoteDatabaseListing( entry, KeepassDirFilename );
                if ( ret == KeePassSyncErr.None )
                {
                    string directory = entry.Strings.Get( "DirectoryField" ).ReadString();
                    if ( directory != "" && !directory.EndsWith( "/" ) )
                        directory += "/";

                    // read the text file for databases
                    ret = KeePassSyncErr.FileNotFound;
                    string[] lines = File.ReadAllLines( KeepassDirFilename );
                    if ( lines.Length > 0 )
                    {
                        foreach ( string line in lines )
                        {
                            if ( line == ( directory + remoteDatabaseName ) )
                            {
                                ret = KeePassSyncErr.None;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    ret = KeePassSyncErr.FileNotFound;
                }

            }
            catch
            {
            }

            if ( File.Exists( KeepassDirFilename ) )
            {
                File.Delete( KeepassDirFilename );
            }

            return ret;
        }

        public override KeePassSyncErr GetFile( PwEntry entry, string databaseName, string filename )
        {
            Debug.Assert( entry != null, "Invalid entry" );
            KeePassSyncErr ret = KeePassSyncErr.None;

            try
            {
                System.Diagnostics.Process process = new Process();
                if ( process != null )
                {
                    if ( File.Exists( filename ) )
                        File.Delete( filename );

                    string directory = entry.Strings.Get( "DirectoryField" ).ReadString();
                    if ( directory != "" && !directory.EndsWith( "/" ) )
                        directory += "/";

                    string remoteDatabaseName = ONLINE_DB_PREFIX + databaseName;

                    // Get the file
                    bool exitedOnTime = RemoteGetFile( entry, filename, directory, remoteDatabaseName );
                    if ( exitedOnTime )
                    {
                        // Check to see if it's there
                        if ( !File.Exists( filename ) )
                        {
                            ret = KeePassSyncErr.FileNotFound;
                        }
                    }
                    else
                    {
                        ret = KeePassSyncErr.Timeout;
                    }
                }
                else
                {
                    ret = KeePassSyncErr.Error;
                }
            }
            catch
            {
                ret = KeePassSyncErr.Error;
            }

            return ret;
        }

        private bool RemoteGetFile( PwEntry entry, string localFilename, string remoteDirectory, string remoteDatabaseName )
        {
            System.Diagnostics.Process process = new Process();

            string commandStr = "-pw "
                + entry.Strings.Get( PwDefs.PasswordField ).ReadString()
                + " "
                + entry.Strings.Get( PwDefs.UserNameField ).ReadString()
                + "@"
                + entry.Strings.Get( PwDefs.UrlField ).ReadString()
                + ":"
                + remoteDirectory
                + remoteDatabaseName
                + " "
                + "\"" + localFilename + "\"";

            process.StartInfo.FileName = KeePassSyncExt.PluginDirectory + "\\pscp.exe";
            process.StartInfo.Arguments = commandStr;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.WorkingDirectory = KeePassSyncExt.PluginDirectory;
            process.Start();
            return process.WaitForExit( 7000 );
        }

        public override string CreateAccountLink
        {
            get { return null; }
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
                return GetDatabases( m_OptionData.PasswordEntry );
            }
        }

        private KeePassSyncErr RemoteDatabaseListing( PwEntry entry, string localListingFileFullPath )
        {
            KeePassSyncErr ret = KeePassSyncErr.Error;

            try
            {
                System.Diagnostics.Process process = new Process();

                if ( File.Exists( localListingFileFullPath ) )
                    File.Delete( localListingFileFullPath );

                // Generate directory listing
                string remoteDirectory = entry.Strings.Get( "DirectoryField" ).ReadString();
                if ( remoteDirectory != "" && !remoteDirectory.EndsWith( "/" ) )
                    remoteDirectory += "/";

                string commandStr = "-pw " +
                    entry.Strings.Get( PwDefs.PasswordField ).ReadString() +
                    " " +
                    entry.Strings.Get( PwDefs.UserNameField ).ReadString() +
                    "@" +
                    entry.Strings.Get( PwDefs.UrlField ).ReadString() +
                    " \"ls -1 " +
                    remoteDirectory +
                    ONLINE_DB_PREFIX +
                    "* > " +
                    remoteDirectory + 
                    "KeePassSyncDir.txt\"";
                process.StartInfo.FileName = KeePassSyncExt.PluginDirectory + "\\plink.exe";
                process.StartInfo.Arguments = commandStr;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.WorkingDirectory = KeePassSyncExt.PluginDirectory;
                process.Start();
                bool exitedOnTime = process.WaitForExit( 7000 );

                if ( exitedOnTime )
                {
                    // Download the directory listing
                    commandStr = "-pw " +
                        entry.Strings.Get( PwDefs.PasswordField ).ReadString() +
                        " " +
                        entry.Strings.Get( PwDefs.UserNameField ).ReadString() +
                        "@" +
                        entry.Strings.Get( PwDefs.UrlField ).ReadString() +
                        ":" + remoteDirectory + "KeePassSyncDir.txt \"" + localListingFileFullPath + "\"";
                    process.StartInfo.FileName = KeePassSyncExt.PluginDirectory + "\\pscp.exe";
                    process.StartInfo.Arguments = commandStr;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    process.StartInfo.WorkingDirectory = KeePassSyncExt.PluginDirectory;
                    process.Start();
                    exitedOnTime = process.WaitForExit( 7000 );

                }

                if ( exitedOnTime )
                {
                    if ( File.Exists( localListingFileFullPath ) )
                        ret = KeePassSyncErr.None;
                }
                else
                {
                    ret = KeePassSyncErr.Timeout;
                }

            }
            catch
            {
            }

            return ret;
        }

        public override string[] GetDatabases( PwEntry entry )
        {
            Debug.Assert( entry != null, "Invalid entry" );

            // Use plink to issue directory command as text file
            // plink -pw password user@host "dir > out.txt"
            // use pscp to download the file
            // then plink to delete the file again
            // pscp doesn't like a full path, it likes it relative to the home folder
            KeePassSyncErr err = RemoteDatabaseListing( entry, KeepassDirFilename );
            if ( err == KeePassSyncErr.None )
            {
                // Generate directory listing
                string remoteDirectory = entry.Strings.Get( "DirectoryField" ).ReadString();
                if ( remoteDirectory != "" && !remoteDirectory.EndsWith( "/" ) )
                    remoteDirectory += "/";

                // read the text file for databases
                string[] lines = File.ReadAllLines( KeepassDirFilename );
                ArrayList databases = null;
                if ( lines.Length > 0 )
                {
                    databases = new ArrayList();
                    foreach ( string line in lines )
                    {
                        databases.Add( line.Substring( remoteDirectory.Length + ONLINE_DB_PREFIX.Length ) );
                    }
                    return (string[])databases.ToArray( typeof( string ) );
                }
            }
            else if ( err == KeePassSyncErr.Timeout )
            {
                m_MainInterface.SetStatus( StatusPriority.eMessageBoxInfo, "A timeout occurred retrieving databases.\n\nAre you sure you've authenticated your server?\nSee docs\\KeePassSync-readme.txt for details." );
            }
            else
            {
                m_MainInterface.SetStatus( StatusPriority.eMessageBoxInfo, "A general error occurred retrieving databases.\n\nAre you sure you've authenticated your server?\nSee docs\\KeePassSync-readme.txt for details." );
            }

            if ( File.Exists( KeepassDirFilename ) )
            {
                File.Delete( KeepassDirFilename );
            }


            return null;
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
            if ( entry.Strings.Get( PwDefs.UrlField ) != null )
                m_UserControl.Host = entry.Strings.Get( PwDefs.UrlField ).ReadString();
            if ( entry.Strings.Get( "DirectoryField" ) != null )
                m_UserControl.Directory = entry.Strings.Get( "DirectoryField" ).ReadString();
        }

        public override void EncodeEntry( PwEntry entry )
        {
            Debug.Assert( entry != null, "Invalid entry" );

            entry.Strings.Set( PwDefs.UserNameField, new ProtectedString( false, m_UserControl.Username ) );
            entry.Strings.Set( PwDefs.PasswordField, new ProtectedString( false, m_UserControl.Password ) );
            entry.Strings.Set( PwDefs.UrlField, new ProtectedString( false, m_UserControl.Host ) );
            entry.Strings.Set( "DirectoryField", new ProtectedString( false, m_UserControl.Directory ) );
        }

    }
}
