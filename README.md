What is KeePassSync?
---------------

KeePassSync is a KeePass plugin that synchronizes your database using an online storage provider of your choice, any Rclone/Rsync compat storage, or FTP/SFTP. This allows two or more computers to easily keep their data in sync.  It was originally created in 2008 by Shawn Casey, shawn.casey@gmail.com and is currently maintained in this GitHub repository.  If you are looking for a provider recommendation consider CloudFlare R2.  It is free for KeePass databases up to 10GB.

This plugin will NOT work with KeePass v1.x. Thanks to https://github.com/walterpg/plgx-build-tasks for modernizing the PLGX build process.

<!-- MarkdownTOC -->

- [Requirements:](#requirements)
- [Changes:](#changes)
- [How to install:](#how-to-install)
- [How to use:](#how-to-use)
- [Services:](#services)
- [How to compile:](#how-to-compile)
- [Service Specific Notes](#service-specific-notes)
	- [Amazon S3 \(and other compatible providers\)](#amazon-s3-and-other-compatible-providers)
	- [SFTP/FTP](#sftpftp)
- [Help!?!](#help)

<!-- /MarkdownTOC -->


Requirements:
-------------

- KeePass 2.09 or higher

Changes:
-------------
See [Change Log](ChangeLog.md) for details

How to install:
---------------

To install a plugin, follow these steps:

1. Download the plgx of the plugin from the releases page at: https://github.com/mitchcapper/KeePassSync/releases
2. Copy the plgx file to the KeePass Plugins folder
3. Restart KeePass in order to load the new plugin.

Note: If you have a very-old version of KeePassSync please delete the KeePassSync*.dll from the plugin folder.

To "uninstall" a plugin, delete the the PLGX.

How to use:
-----------

From the KeePass main menu, select Tools->KeePassSync->Show Options...

You then select your online provider you wish to use.  If you don't have an online provider account, you can create an account for the provider by clicking on the link beneath the selection box.  Your online account information is securely stored as a KeePass entry.  To create the entry, click the "Create Keepass Entry" button.  It's important that you not rename this entry after it's created.  You can move it around, but keep the same name.  The username/password in this entry will be used to connect to the selected online provider.

Once you do that, you can immediately sync with your online provider which will transmit changes from your database and apply them to the server version.  How the synchronization is resolved is configurable in the "General" tab of the KeePassSync options.  I use "Synchronize".  If this is the first time your database is synchronized, your database will be uploaded to the online provider.

Once you go to another computer, you will want to open the database from the online provider.  You don't have to create an entry, but you will have to select from the options which provider you want to use.  After that, select "Tools->KeePassSync->Open", select which provider you want to use.  Input your account details for your online provider, click ok, and a list of previously synchronized databases will appear.  Select which database you wish to open locally and declare a storage location for it.  From then on, you may open the local database normally and synchronize whenever you want.

For each services specific configuration guide please see that section below.

Services:
---------------

KeePassSync supports the following services natively (although other users can add other services):

- FTP/SFTP (through plink/psftp from PUTTY)

- Amazon S3 or S3 Compatible services, this includes:
	- [Cloudflare R2](https://www.cloudflare.com/developer-platform/r2/) - free forever basically, 10GB a month which is way more than any keepass DB
	- [Rclone Serve S3](https://rclone.org/commands/rclone_serve_s3/) - Free open source software to use as an interface to anything RClone can interface with
	- [Backblaze B2](https://www.backblaze.com/cloud-storage) - Very cheap
	- Alibaba Cloud
	- DigitalOcean
	- Dreamhost
	- [Synology C2](https://c2.synology.com/en-us)
	- Linode Object Storage
	- IDrive E2
	- Huawei OBS
	- IBM COS S3
	- [Google GCS](https://cloud.google.com/storage)
	- Any of the many other compat providers, for several more see https://rclone.org/s3/


How to compile:
---------------

This solution is compiled using Visual Studio 2022.

For more information: http://keepass.info/help/v2_dev/plg_index.html

## Service Specific Notes

### Amazon S3 (and other compatible providers)

Please see the dedicated [Amazon S3](AMAZON_S3.md) instructions.

### SFTP/FTP

The FTP provider uses SCP to transfer the files, you will have to manually establish a relationship with your FTP server. Have no fear, it's easy to do. Simply go to the directory you installed KeePassSync and type this:

plink -pw (PASSWORD) (USERNAME)@(HOST) "ls"

...of course substituting PASSWORD, USERNAME, and HOST with appropriate values.

Help!?!
-------

Create an issue on the github issues section
