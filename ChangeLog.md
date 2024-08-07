2024-07-10: v8.2
- For SFTP check the application  directory for keepass for psftp binaries
- 

2024-02-17: v8.1
- Don't auto-lowercase S3 bucket names, legacy allows different cases
- Don't compare sha1 hashes when they don't exist (legacy kpsync uploads)

2024-02-17: v8.0
- Amazon S3 updated to use SDK
- Project style changed and updated
- DigitalBucket removed (adios)
- Added update checks
- Project migrated to GitHub and git
- Use amazing PlgxBuildTasks to modernize plgx building


2016-04-01: v6.10
- Unreleased, get databases in S3, minor other S3 updates, migration to .net 4.5

2012-01-16: v6.0
- Compat with keepass 2.18 and higher
- S3 permission preservation

2009-03-10: v2.11
- Plugin distributed as PLGX rather than dll
- Moved stock providers to be inside of main project
- Now compatible with old versions from 2.09 or higher


2009-05-03: v2.10.1
- DigitalBucket: Fixed New Database Initialization
- SFTP: Added Key Authentication through option to use Paegent
- SFTP: Command line arguments are now quoted so support spacing
- SFTP: Added configurable timeout
- SFTP: Added ability to change path for executables
- SFTP: Added debug mode
- SFTP: Added ability to set port for host
- SFTP: Code Cleanup


2009-03-10: v2.10
- Compatible with KeePass v2.10

2009-02-20: v2.09
- Compatible with KeePass v2.09
- Fixes to Double Creation Entries and Editing Problems
- Fixes to Setup
- Amazon S3 Provider Added
- Background Sync Support


2009-03-14: v2.07
- Compatible with KeePass v2.07 Beta

2008-11-02: v2.06
- Compatible with KeePass v2.06 Beta

2008-08-06: v2.05.2
- On open of DB, temp entry no longer hangs around
- On first creation of syncEntry in database, sync doesn't fail because mergeIn() can't find file
- On save, we sync, not just export.
- Default button on openForm correct
- On options form, switching providers now changes the entry in the edit entry form
- Detects timeout with FTP/SCP provider
- When plugins are in a subdirectory, KeePassSync still works
- Completely removed ProviderInfo references
- Providers are only initialized once at startup
- Providers are only stored within the main interface, all other references just use a key and go through the main interface
- Ripped out redundant ProviderInfo struct
- Visual Studio solution now in VS2008 format
- Unified options GUI for multiple online providers.
- New FTP/SCP provider using Putty's plink.exe and pscp.exe
- Fixed many stability bugs for options dialog
- Changed the name of DigitalBucket to digitalbucket.net
- Fixed crash when no KeePassSync providers are present
- Updated docs
- Added documents directory to the tree


2008-05-27: v2.05.1
- Added documents to the tree and release packages
- Removed reliance upon registry, it's still used but not required
- Fixed issue of not being able to find login info after opening file from online provider
- Cleaned up options interface.
- Better code documentation
- Post build events
- Copyright info

2008-05-12: v2.05
- Project started