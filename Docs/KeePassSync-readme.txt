IMPORTANT NOTE:
---------------
IMPORTANT NOTE FOR FTP/SCP USERS!  The FTP provider uses SCP to transfer the files, you will have to manually establish a relationship with your FTP server.  Have no fear, it's easy to do.  Simply go to the directory you installed KeePassSync and type this:

plink -pw (PASSWORD) (USERNAME)@(HOST) "ls"

...of course substituting PASSWORD, USERNAME, and HOST with appropriate values.


What is KeePassSync?
--------------------
KeePassSync is a KeePass plugin that synchronizes your database using various online storage providers. This allows two or more computers to easily keep their data in sync.  Each version of KeePassSync requires the corresponding version of KeePass.  For example, if you download KeePassSync 2.05, it requires KeePass 2.05.

This plugin will NOT work with KeePass v1.x.


Requirements:
-------------
- KeePass 2.07 Beta


How to install:
---------------
To install a plugin, follow these steps:

   1. Download the plugin from the page above and unpack the ZIP file to a directory of your choice.
   2. Copy the unpacked plugin files into the KeePass directory (where the KeePass.exe is) or a subdirectory of it.
   3. Restart KeePass in order to load the new plugin.

In other words, to "install" a plugin you simply need to copy it somewhere into the KeePass directory, that's all.

To "uninstall" a plugin, delete the plugin files.



How to use:
-----------
From the KeePass main menu, select Tools->KeePassSync->Show Options...

You then select your online provider you wish to use.  If you don't have an online provider account, you can create an account for the provider by clicking on the link beneath the selection box.  Your online account information is securely stored as a KeePass entry.  To create the entry, click the "Create Keepass Entry" button.  It's important that you not rename this entry after it's created.  You can move it around, but keep the same name.  The username/password in this entry will be used to connect to the selected online provider.

Once you do that, you can immediately sync with your online provider which will transmit changes from your database and apply them to the server version.  How the synchronization is resolved is configurable in the "General" tab of the KeePassSync options.  I use "Synchronize".  If this is the first time your database is synchronized, your database will be uploaded to the online provider.

Once you go to another computer, you will want to open the database from the online provider.  You don't have to create an entry, but you will have to select from the options which provider you want to use.  After that, select "Tools->KeePassSync->Open", select which provider you want to use.  Input your account details for your online provider, click ok, and a list of previously synchronized databases will appear.  Select which database you wish to open locally and declare a storage location for it.  From then on, you may open the local database normally and synchronize whenever you want.


How to compile:
---------------
This solution is compiled using Visual Studio 2008.

Because KeePass is a strong-named assembly, your plugin must have a strong name and sign this assembly, too. Go to the project options of your plugin and open the Signing tab. Enable assembly signing and generate a new key pair.

For more information: http://keepass.info/help/v2_dev/plg_index.html


Help!?!
-------
If you have any questions, please leave a message in the KeePassSync forum:
http://sourceforge.net/forum/forum.php?forum_id=810364
