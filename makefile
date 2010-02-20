VERSION = 2.09
BINS=KeePassSync/bin/Release/KeePassSync.dll KeePassSync_digitalbucket.net/bin/Release/KeePassSync_digitalBucket.net.dll KeePassSync_FTP/bin/Release/KeePassSync_FTP.dll  KeePassSync_S3/bin/Release/KeePassSync_S3.dll Support/plink.exe Support/pscp.exe
BINARY_PACKAGE=KeePassSync-$(VERSION).zip
SRC_PACKAGE=KeePassSync-$(VERSION)-src.zip

package: package-bin package-src


package-bin:
	zip -j $(BINARY_PACKAGE) $(BINS)
	zip -r $(BINARY_PACKAGE) docs
	zip -d -r $(BINARY_PACKAGE) docs/Internal*

package-src:
	zip -r $(SRC_PACKAGE) KeePassSync.sln KeePassSync/* -x KeePassSync/password.pfx -x KeePassSync/KeePassSync.csproj.user -d KeePassSync/bin/* -d KeePassSync/obj/*
	zip -d -r $(SRC_PACKAGE) KeePassSync/bin KeePassSync/obj

	zip -r $(SRC_PACKAGE) KeePassSync_digitalbucket.net/*
	zip -d -r $(SRC_PACKAGE) KeePassSync_digitalbucket.net/bin KeePassSync_digitalbucket.net/obj

	zip -r $(SRC_PACKAGE) KeePassSync_FTP/*
	zip -d -r $(SRC_PACKAGE) KeePassSync_FTP/bin KeePassSync_FTP/obj

	zip -r $(SRC_PACKAGE) KeePassSync_S3/*
	zip -d -r $(SRC_PACKAGE) KeePassSync_S3/bin KeePassSync_S3/obj

	zip -r $(SRC_PACKAGE) docs
	zip -d -r $(SRC_PACKAGE) docs/Internal*

clean: 
	rm -f $(BINARY_PACKAGE) $(SRC_PACKAGE)

all: clean package