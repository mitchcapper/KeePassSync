using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using KeePassLib;
using KeePassLib.Security;
using KeePassSync.AmazonS3;

namespace KeePassSync.Providers.S3 {

	public class S3Provider : IOnlineProvider {
		#region -- Private data --
		private const string m_Name = "S3";
		private string[] m_AcceptedNames = { "S3" };
		private AccountDetails m_UserControl;
		#endregion

		private const string access_key_field = PwDefs.UserNameField;
		private const string bucket_name_field = PwDefs.UrlField;
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
			m_UserControl.AccessKey = read_PwEntry_string(entry, access_key_field);
			m_UserControl.SecretAccessKey = read_PwEntry_string(entry, secret_access_key_field);
			m_UserControl.BucketName = read_PwEntry_string(entry, bucket_name_field);
			String backup_str = read_PwEntry_string(entry, create_backups_field);

			m_UserControl.CreateBackups = false;
			if (backup_str == "true")
				m_UserControl.CreateBackups = true;
		}

		public override void EncodeEntry(PwEntry entry) {
			write_PwEntry_string(entry, access_key_field, m_UserControl.AccessKey, false);
			write_PwEntry_string(entry, secret_access_key_field, m_UserControl.SecretAccessKey, memprotect_secret_access_key);
			write_PwEntry_string(entry, bucket_name_field, m_UserControl.BucketName, false);
			write_PwEntry_string(entry, create_backups_field, m_UserControl.CreateBackups ? "true" : "false", false);
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
			ret = verify_bucket_or_create(m_UserControl.BucketName);
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
		public override KeePassSyncErr PutFile(PwEntry entry, string remoteFilename, string localFilename) {
			try {
				DecodeEntry(entry);
				BinaryReader reader = new BinaryReader(File.OpenRead(localFilename));
				byte[] data = new byte[reader.BaseStream.Length];
				reader.Read(data, 0, data.Length);
				reader.Close();
				KeePassSyncErr err = verify_bucket_or_create(m_UserControl.BucketName);
				if (err != KeePassSyncErr.None)
					return err;
				DateTime stamp;
				String sig;
				if ((m_UserControl.CreateBackups) && fileExists(remoteFilename)){
					string backupFilename = remoteFilename + ".bkup_day" + DateTime.Today.Day;

					GenerateSigElements("CopyObject", out stamp, out sig);
					client.CopyObject(m_UserControl.BucketName, remoteFilename, m_UserControl.BucketName, backupFilename,
					                  MetadataDirective.COPY, true, null, null, System.DateTime.MinValue, false,
					                  System.DateTime.MinValue, false, null, null, StorageClass.STANDARD, false,
					                  m_UserControl.AccessKey, stamp, true, sig, null);
				}
				Grant[] acl = null;
				if (fileExists(remoteFilename)){
					GenerateSigElements("GetObjectAccessControlPolicy", out stamp, out sig);
					AccessControlPolicy policy = client.GetObjectAccessControlPolicy(m_UserControl.BucketName, remoteFilename,
					                                                                 m_UserControl.AccessKey, stamp, true, sig, null);
					acl = policy.AccessControlList;
				}


				GenerateSigElements("PutObjectInline", out stamp, out sig);
				PutObjectResult result = client.PutObjectInline(m_UserControl.BucketName, remoteFilename, null, data, data.Length, acl, StorageClass.STANDARD, true, m_UserControl.AccessKey, stamp, true, sig, null);
				if (result == null)
					throw new Exception("Put File Failed");

				string hash = get_md5(data);
				if (hash != result.ETag)
					throw new Exception("File uploaded but our hash of: " + hash + " does not match server hash of: " + result.ETag);
			}
			catch (Exception e) {
				return convert_exception(e);
			}
			return KeePassSyncErr.None;
		}
		public override UserControl GetUserControl() {
			return m_UserControl;
		}
		public override string[] GetDatabases(PwEntry entry) {
			throw new NotImplementedException();
		}
		private string get_md5(byte[] data) {
			MD5 md5 = new MD5CryptoServiceProvider();

			return "\"" + BitConverter.ToString(md5.ComputeHash(data)).Replace("-", "").ToLower() + "\"";
		}
		private bool fileExists(string key) {
			DateTime stamp;
			String sig;
			bool result = false;
			GenerateSigElements("GetObject", out stamp, out sig);
			try {
				GetObjectResult cur_item = client.GetObject(m_UserControl.BucketName, key, false, false, false, m_UserControl.AccessKey, stamp, true, sig, null);
				if (cur_item != null)
					result = true;
			} catch (System.Web.Services.Protocols.SoapException e) {
				if (e.Message != "The specified key does not exist.")
					throw e;
			}
			return result;
		}

		public override KeePassSyncErr GetFile(PwEntry entry, string remoteFilename, string localFilename) {
			DecodeEntry(entry);
			try {
				verify_bucket_or_create(m_UserControl.BucketName);

				DateTime stamp;
				String sig;
				GenerateSigElements("GetObject", out stamp, out sig);
				GetObjectResult result = client.GetObject(m_UserControl.BucketName, remoteFilename, false, true, true, m_UserControl.AccessKey, stamp, true, sig, null);
				if (result == null || result.Data == null)
					return KeePassSyncErr.FileNotFound;
				byte[] data = result.Data;
				string hash = get_md5(data);
				if (hash != result.ETag)
					throw new Exception("File downloaded but our hash of: " + hash + " does not match server hash of: " + result.ETag);
				BinaryWriter writer = new BinaryWriter(File.OpenWrite(localFilename));
				writer.Write(data);
				writer.Close();
			}
			catch (Exception e) {
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
		private DateTime LocalNow() {
			DateTime now = DateTime.Now;
			return new DateTime(
			now.Year, now.Month, now.Day,
			now.Hour, now.Minute, now.Second,
			now.Millisecond, DateTimeKind.Local);
		}

		private string SignRequest(string secret, string operation, DateTime timestamp) {
			HMACSHA1 hmac = new HMACSHA1(Encoding.UTF8.GetBytes(secret));
			string isoTimeStamp = timestamp.ToUniversalTime().ToString(
			"yyyy-MM-ddTHH:mm:ss.fffZ",
			CultureInfo.InvariantCulture);
			string signMe = "AmazonS3" + operation + isoTimeStamp;
			string signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(signMe)));
			return signature;
		}
		private void GenerateSigElements(String command, out DateTime stamp, out string sig) {
			stamp = LocalNow();
			sig = SignRequest(m_UserControl.SecretAccessKey, command, stamp);

		}
		private AmazonS3.AmazonS3 client = new AmazonS3.AmazonS3();
		private KeePassSyncErr verify_bucket_or_create(String bucket_name) {
			try {
				DateTime stamp;
				String sig;
				GenerateSigElements("ListAllMyBuckets", out stamp, out sig);
				ListAllMyBucketsResult result = client.ListAllMyBuckets(m_UserControl.AccessKey, stamp, true, sig);
				foreach (ListAllMyBucketsEntry bucket in result.Buckets) {
					if (bucket.Name == bucket_name)
						return KeePassSyncErr.None;
				}

				GenerateSigElements("CreateBucket", out stamp, out sig);
				client.CreateBucket(bucket_name, null, m_UserControl.AccessKey, stamp, true, sig);

				GenerateSigElements("ListAllMyBuckets", out stamp, out sig);
				result = client.ListAllMyBuckets(m_UserControl.AccessKey, stamp, true, sig);
				foreach (ListAllMyBucketsEntry bucket in result.Buckets) {
					if (bucket.Name == bucket_name)
						return KeePassSyncErr.None;
				}
				throw new Exception("Unable to find bucket, even after creating");
			}
			catch (Exception e) {
				return convert_exception(e);
			}
		}

	}
}
