﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Windows.Forms;
using KeePassLib;
using KeePassLib.Security;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;


namespace KeePassSync.Providers.S3 {

	public class S3Provider : IOnlineProvider {
		#region -- Private data --
		private const string m_Name = "S3";
		private string[] m_AcceptedNames = { "S3" };
		private AccountDetails m_UserControl;
		#endregion

		private const string access_key_id_field = PwDefs.UserNameField;
		private const string bucket_name_field = PwDefs.UrlField;
		private const string use_checksums_field = "use_checksums";
		private const string use_acls_field = "use_acls";
		private const string custom_service_url_field = "service_url";
		private const string create_backups_field = "create_backups";

		private const string secret_access_key_field = PwDefs.PasswordField;
		private bool memprotect_secret_access_key = true;


		public override KeePassSyncErr Initialize(KeePassSyncExt mainInterface) {
			KeePassSyncErr ret = base.Initialize(mainInterface);

			m_UserControl = new AccountDetails();

			m_IsInitialized = (ret == KeePassSyncErr.None);

			return ret;
		}
		public override void DecodeEntry(PwEntry entry) {
			m_UserControl.AccessKey = read_PwEntry_string(entry, access_key_id_field);
			m_UserControl.SecretAccessKey = read_PwEntry_string(entry, secret_access_key_field);
			m_UserControl.BucketName = read_PwEntry_string(entry, bucket_name_field);

			m_UserControl.CreateBackups = GetBoolField(entry, create_backups_field);
			m_UserControl.UseACLs = GetBoolField(entry, use_acls_field);
			m_UserControl.UseChecksums = GetBoolField(entry, use_checksums_field);
			m_UserControl.ServiceURL = read_PwEntry_string(entry, custom_service_url_field);
		}
		private bool GetBoolField(PwEntry entry, String fieldname) {
			var str = read_PwEntry_string(entry, fieldname);
			return str == "true";
		}
		private void SetBoolField(PwEntry entry, String fieldname, bool value) {
			write_PwEntry_string(entry, fieldname, value ? "true" : "false", false);
		}

		public override void EncodeEntry(PwEntry entry) {
			write_PwEntry_string(entry, access_key_id_field, m_UserControl.AccessKey, false);
			write_PwEntry_string(entry, secret_access_key_field, m_UserControl.SecretAccessKey, memprotect_secret_access_key);
			write_PwEntry_string(entry, bucket_name_field, m_UserControl.BucketName, false);
			write_PwEntry_string(entry, custom_service_url_field, m_UserControl.ServiceURL, false);
			SetBoolField(entry, create_backups_field, m_UserControl.CreateBackups);
			SetBoolField(entry, use_acls_field, m_UserControl.UseACLs);
			SetBoolField(entry, use_checksums_field, m_UserControl.UseChecksums);
		}
		public void write_PwEntry_string(PwEntry entry, String key, String value, bool in_memory_encrypt) {
			entry.Strings.Set(key, new ProtectedString(in_memory_encrypt, value));
		}
		public string read_PwEntry_string(PwEntry entry, String key) {
			ProtectedString str = entry.Strings.Get(key);
			if (str == null)
				return "";
			return str.ReadString();
		}
		public override KeePassSyncErr ValidateOptions(OptionsData options) {
			KeePassSyncErr ret = KeePassSyncErr.None;
			PwEntry entry = m_OptionData.PasswordEntry;
			AccountDetails old_details = m_UserControl;
			m_UserControl = new AccountDetails();
			DecodeEntry(entry);
			ret = verify_bucket_or_create(BucketName);
			m_UserControl = old_details;
			return ret;
		}

		public override string CreateAccountLink {
			get { return "http://aws.amazon.com/s3/"; }
		}
		public override string Name {
			get { return m_Name; }
		}

		public override string[] AcceptedNames {
			get { return m_AcceptedNames; }
		}

		public override string[] Databases {
			get {
				return GetDatabases(m_OptionData.PasswordEntry);
			}
		}
		private bool UsePayloadSigning { get { return String.IsNullOrWhiteSpace(m_UserControl.ServiceURL); } }
		private AmazonS3Client GetClient() {
			var isS3Official = String.IsNullOrWhiteSpace(m_UserControl.ServiceURL);
			if (!isS3Official && m_UserControl.ServiceURL.StartsWith("http", StringComparison.CurrentCultureIgnoreCase) == false)
				m_UserControl.ServiceURL = "https://" + m_UserControl.ServiceURL;

			var url = isS3Official ? "https://s3.amazonaws.com" : m_UserControl.ServiceURL;
			var cfg = new AmazonS3Config { ServiceURL = url };
			//cfg.ProxyHost = "127.0.0.1";cfg.ProxyPort = 1234;
			 List<string> headerRemove= null;
			if (!isS3Official) {
				headerRemove = new List<string> {"x-amz-tagging-directive"};
			}


			var client = new OurAmazonS3Client(m_UserControl.AccessKey, m_UserControl.SecretAccessKey, cfg,headerRemove);

			return client;

		}
		public override KeePassSyncErr PutFile(PwEntry entry, string remoteFilename, string localFilename) {
			try {
				DecodeEntry(entry);
				using (var fs = File.OpenRead(localFilename)) {
					KeePassSyncErr err = verify_bucket_or_create(BucketName);
					if (err != KeePassSyncErr.None)
						return err;

					var client = GetClient();
					S3AccessControlList acl = null;
					if (FileExists(remoteFilename)) {
						string backupFilename = remoteFilename + ".bkup_day" + DateTime.Today.Day;
						if (m_UserControl.UseACLs)
							acl = client.GetACL(new GetACLRequest() { BucketName = BucketName, Key = remoteFilename }).AccessControlList;
						if (m_UserControl.CreateBackups) {
							var copy = new CopyObjectRequest { DestinationBucket = BucketName, SourceBucket = BucketName, SourceKey = remoteFilename, DestinationKey = backupFilename };

							
							client.CopyObject(copy);
						}


					}
					var hash = GetSHA1(fs); //s3 will auto seek to start on stream after
					var req = new PutObjectRequest { BucketName = BucketName, Key = remoteFilename, InputStream = fs, AutoCloseStream = false, DisablePayloadSigning = !UsePayloadSigning };
					if (m_UserControl.UseChecksums)
						req.ChecksumSHA1 = hash;
					client.PutObject(req);
					if (acl != null) {
						try {
							client.PutACL(new PutACLRequest { BucketName = BucketName, Key = remoteFilename, AccessControlList = acl });
						} catch (AmazonS3Exception s3e) {
							if (s3e.ErrorCode != "AccessControlListNotSupported")
								throw s3e;
						}
					}


				}
			} catch (Exception e) {
				return convert_exception(e);
			}
			return KeePassSyncErr.None;
		}
		private string GetSHA1(Stream stream) {
			using (var sha1 = SHA1.Create()) {
				return Convert.ToBase64String(sha1.ComputeHash(stream));
			}

		}
		public override UserControl GetUserControl() {
			return m_UserControl;
		}
		public override string[] GetDatabases(PwEntry entry) {
			var client = GetClient();
			DecodeEntry(entry);
			List<string> databases = new List<string>();
			var files = client.ListObjectsV2(new ListObjectsV2Request { BucketName = BucketName });
			foreach (var file in files.S3Objects) {
				if (file.Key.EndsWith(".kdbx"))
					databases.Add(file.Key);
			}
			return databases.ToArray();
		}

		private bool FileExists(string filename) {

			var client = GetClient();
			try {
				var meta = client.GetObjectMetadata(BucketName, filename);
				return true;
			} catch (AmazonS3Exception s3) {
				if (s3.StatusCode != HttpStatusCode.NotFound)
					throw s3;
				return false;
			}
		}
		//public string BucketName => m_UserControl.BucketName?.ToLower();
		//public string BucketName { get { return m_UserControl.BucketName == null ? null : m_UserControl.BucketName.ToLower(); } }
		public string BucketName { get { return m_UserControl.BucketName == null ? null : m_UserControl.BucketName; } }
		public override KeePassSyncErr GetFile(PwEntry entry, string remoteFilename, string localFilename) {
			DecodeEntry(entry);
			var client = GetClient();
			try {
				verify_bucket_or_create(BucketName);
				var req = new GetObjectRequest { BucketName = BucketName, Key = remoteFilename };
				if (m_UserControl.UseChecksums)
					req.ChecksumMode = ChecksumMode.ENABLED;

				var obj = client.GetObject(req);
				using (var memStream = new MemoryStream()) {
					using (var resp = obj.ResponseStream) {
						resp.CopyTo(memStream);
					}
					memStream.Seek(0, SeekOrigin.Begin);
					var hash = GetSHA1(memStream);

					//the alg must have been set to sha1 during upload for this to work, prior to 2024 we didn't use sha, in addition if a user manually copied it might lose the SHA checksum
					if (m_UserControl.UseChecksums && obj.ResponseMetadata.ChecksumAlgorithm == Amazon.Runtime.CoreChecksumAlgorithm.SHA1 && !obj.ChecksumSHA1.Equals(hash, StringComparison.OrdinalIgnoreCase))
						//throw new Exception($"File downloaded but our hash of: {hash} does not match server hash of: {obj.ChecksumSHA1}");
						throw new Exception("File downloaded but our hash of: " + hash + " does not match server hash of: " + obj.ChecksumSHA1);

					using (var fs = File.OpenWrite(localFilename)) {
						memStream.Seek(0, SeekOrigin.Begin);
						memStream.CopyTo(fs);
					}
				}
			} catch (Exception e) {
				var ae = e as AmazonS3Exception;
				if (e.Message == "The specified key does not exist." || (ae != null && ae.ErrorCode=="NoSuchKey") )//errorcode check for 3rd parties that may not conform
					return KeePassSyncErr.FileNotFound;
				return convert_exception(e);
			}
			return KeePassSyncErr.None;

		}
		private KeePassSyncErr convert_exception(Exception e) {
			if (e.GetType() == typeof(WebException)) {
				WebException w_exp = (WebException)e;
				if (w_exp.Status == WebExceptionStatus.ConnectFailure || w_exp.Status == WebExceptionStatus.NameResolutionFailure)
					return KeePassSyncErr.NotConnected;

			}
			KeePassSyncErr ret;
			string msg;
			StatusPriority priority = StatusPriority.eMessageBoxFatal;
			switch (e.Message) {
				case "The specified bucket is not valid.":
					ret = KeePassSyncErr.Error;
					msg = "Unable to access or create the bucket, make sure bucketname is only lowercase characters and numbers and that you own it (if it exists) min 3 chars max 63";
					break;
				case "Access Denied":
					ret = KeePassSyncErr.Error;
					msg = "If the bucket exists, does the access key you are using have read/write access to it? if it doesn't exist do you have permissions with this access key to create? If not create yourself";
					break;
				case "The specified key does not exist.":
					ret = KeePassSyncErr.FileNotFound;
					msg = "Tried to get file we could not find";
					priority = StatusPriority.eStatusBar;
					break;
				case "The request signature we calculated does not match the signature you provided. Check your key and signing method.":
				case "The AWS Access Key Id you provided does not exist in our records.":
					ret = KeePassSyncErr.InvalidCredentials;
					msg = "Invalid Credentials";
					break;
				default:
					msg = e.Message + "\r\n" + e.StackTrace;
					ret = KeePassSyncErr.Error;
					break;
			}
			m_MainInterface.SetStatus(priority, "KeyPassSync_S3: " + msg);
			return ret;
		}


		private KeePassSyncErr verify_bucket_or_create(String bucket_name) {
			try {
				var client = GetClient();
				if (!AmazonS3Util.DoesS3BucketExistV2(client, bucket_name))
					client.PutBucket(bucket_name);
				return KeePassSyncErr.None;

			} catch (Exception e) {
				return convert_exception(e);
			}
		}

	}
}
