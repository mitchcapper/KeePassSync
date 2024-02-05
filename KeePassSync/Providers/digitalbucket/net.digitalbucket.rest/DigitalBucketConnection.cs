// This code is provided "AS IS" without warranties 
// of any kind, bugs are probable, free for any use 
// at own risk, no responsibility accepted.
// (c) 2007 digitalbucket.net

using System;
using System.Collections.Specialized;
using System.Net;
using System.Collections.Generic;

namespace digitalbucket.net.rest {
	public class DigitalBucketConnection {
		private string username;
		private string password;
		private string address;

		public DigitalBucketConnection(string username, string password, string address) {
			this.username = username;
			this.password = password;
			this.address = address;
		}

		#region "File and Folder Operations"

		public CustomResponse<Folder> GetRootFolder() {
			HttpWebRequest req = makeRequest("GET", "getrootfolder", null);

			return new CustomResponse<Folder>(req);
		}

		public CustomResponse<Folder> GetPhotoGallery() {
			HttpWebRequest req = makeRequest("GET", "getphotogallery", null);

			return new CustomResponse<Folder>(req);
		}

		public CustomResponse<Folder> GetRecycleBin() {
			HttpWebRequest req = makeRequest("GET", "getrecyclebin", null);

			return new CustomResponse<Folder>(req);
		}

		public CustomResponse<FolderCollection> GetSharedFolders() {
			HttpWebRequest req = makeRequest("GET", "getsharedfolders", null);

			return new CustomResponse<FolderCollection>(req);
		}

		public CustomResponse<Folder> GetFolder(long folderId) {
			NameValueCollection parameters = new NameValueCollection();
			parameters.Add("folderid", folderId.ToString());

			HttpWebRequest req = makeRequest("GET", "getfolder", parameters);

			return new CustomResponse<Folder>(req);
		}

		public StreamResponse GetFile(Guid fileId) {
			NameValueCollection parameters = new NameValueCollection();
			parameters.Add("fileid", fileId.ToString());

			HttpWebRequest req = makeRequest("GET", "getfile", parameters);

			return new StreamResponse(req);
		}

		public Response PutFile(long folderId, string fileName, System.IO.Stream stream) {
			NameValueCollection parameters = new NameValueCollection();
			parameters.Add("folderid", folderId.ToString());
			parameters.Add("filename", fileName);

			HttpWebRequest req = makeRequest("PUT", "putfile", parameters);

			// cast WebRequest to a HttpWebRequest to allow for direct streaming of data
			HttpWebRequest hwr = (HttpWebRequest)req;
			hwr.AllowWriteStreamBuffering = false;
			hwr.SendChunked = false;
			hwr.ContentLength = stream.Length;

			byte[] buf = new byte[1024];
			System.IO.BufferedStream bufferedInput = new System.IO.BufferedStream(stream);
			int bytesRead = 0;
			while ((bytesRead = bufferedInput.Read(buf, 0, 1024)) > 0) {
				hwr.GetRequestStream().Write(buf, 0, bytesRead);
			}
			hwr.GetRequestStream().Close();
			stream.Close();

			return new BasicResponse(hwr);
		}

		public Response RenameFile(Guid fileId, string newName) {
			NameValueCollection parameters = new NameValueCollection();
			parameters.Add("fileid", fileId.ToString());
			parameters.Add("newname", newName);

			HttpWebRequest req = makeRequest("POST", "renamefile", parameters);

			return new BasicResponse(req);
		}

		public Response RenameFolder(long folderId, string newName) {
			NameValueCollection parameters = new NameValueCollection();
			parameters.Add("folderid", folderId.ToString());
			parameters.Add("newname", newName);

			HttpWebRequest req = makeRequest("POST", "renamefolder", parameters);

			return new BasicResponse(req);
		}

		public Response DeleteFile(Guid fileId) {
			NameValueCollection parameters = new NameValueCollection();
			parameters.Add("fileid", fileId.ToString());

			HttpWebRequest req = makeRequest("DELETE", "deletefile", parameters);

			return new BasicResponse(req);
		}

		public Response DeleteFolder(long folderId) {
			NameValueCollection parameters = new NameValueCollection();
			parameters.Add("folderid", folderId.ToString());

			HttpWebRequest req = makeRequest("DELETE", "deletefolder", parameters);

			return new BasicResponse(req);
		}

		public Response CreateFolder(long parentId, string folderName) {
			NameValueCollection parameters = new NameValueCollection();
			parameters.Add("parentid", parentId.ToString());
			parameters.Add("foldername", folderName);

			HttpWebRequest req = makeRequest("POST", "createfolder", parameters);

			return new BasicResponse(req);
		}

		public Response MoveFile(Guid fileId, long targetFolderId) {
			NameValueCollection parameters = new NameValueCollection();
			parameters.Add("fileid", fileId.ToString());
			parameters.Add("targetfolderid", targetFolderId.ToString());

			HttpWebRequest req = makeRequest("POST", "movefile", parameters);

			return new BasicResponse(req);
		}

		public Response MoveFolder(long folderId, long targetFolderId) {
			NameValueCollection parameters = new NameValueCollection();
			parameters.Add("folderid", folderId.ToString());
			parameters.Add("targetfolderid", targetFolderId.ToString());

			HttpWebRequest req = makeRequest("POST", "movefolder", parameters);

			return new BasicResponse(req);
		}

		public Response CopyFile(Guid fileId, long targetFolderId) {
			NameValueCollection parameters = new NameValueCollection();
			parameters.Add("fileid", fileId.ToString());
			parameters.Add("targetfolderid", targetFolderId.ToString());

			HttpWebRequest req = makeRequest("POST", "copyfile", parameters);

			return new BasicResponse(req);
		}

		public Response CopyFolder(long folderId, long targetFolderId) {
			NameValueCollection parameters = new NameValueCollection();
			parameters.Add("folderid", folderId.ToString());
			parameters.Add("targetfolderid", targetFolderId.ToString());

			HttpWebRequest req = makeRequest("POST", "copyfolder", parameters);

			return new BasicResponse(req);
		}

		#endregion

		#region "Publish Operations"

		public CustomResponse<FolderCollection> GetPublishedFolders() {
			HttpWebRequest req = makeRequest("GET", "getpublishedfolders", null);

			return new CustomResponse<FolderCollection>(req);
		}

		public CustomResponse<FileCollection> GetPublishedFiles() {
			HttpWebRequest req = makeRequest("GET", "getpublishedfiles", null);

			return new CustomResponse<FileCollection>(req);
		}

		public Response PublishFile(Guid fileId) {
			NameValueCollection parameters = new NameValueCollection();
			parameters.Add("fileid", fileId.ToString());

			HttpWebRequest req = makeRequest("POST", "publishfile", parameters);

			return new BasicResponse(req);
		}

		public Response PublishFolder(long folderId) {
			NameValueCollection parameters = new NameValueCollection();
			parameters.Add("folderid", folderId.ToString());

			HttpWebRequest req = makeRequest("POST", "publishfolder", parameters);

			return new BasicResponse(req);
		}

		public Response UnPublishFile(Guid fileId) {
			NameValueCollection parameters = new NameValueCollection();
			parameters.Add("fileid", fileId.ToString());

			HttpWebRequest req = makeRequest("POST", "unpublishfile", parameters);

			return new BasicResponse(req);
		}

		public Response UnPublishFolder(long folderId) {
			NameValueCollection parameters = new NameValueCollection();
			parameters.Add("folderid", folderId.ToString());

			HttpWebRequest req = makeRequest("POST", "unpublishfolder", parameters);

			return new BasicResponse(req);
		}

		#endregion

		#region "Tagging Operations"

		public Response TagFile(Guid fileId, string tag) {
			NameValueCollection parameters = new NameValueCollection();
			parameters.Add("fileid", fileId.ToString());
			parameters.Add("tag", tag);

			HttpWebRequest req = makeRequest("POST", "tagfile", parameters);

			return new BasicResponse(req);
		}

		public Response TagFolder(long folderId, string tag) {
			NameValueCollection parameters = new NameValueCollection();
			parameters.Add("folderid", folderId.ToString());
			parameters.Add("tag", tag);

			HttpWebRequest req = makeRequest("POST", "tagfolder", parameters);

			return new BasicResponse(req);
		}

		public Response RemoveFileTags(Guid fileId) {
			NameValueCollection parameters = new NameValueCollection();
			parameters.Add("fileid", fileId.ToString());

			HttpWebRequest req = makeRequest("DELETE", "removefiletags", parameters);

			return new BasicResponse(req);
		}

		public Response RemoveFolderTags(long folderId) {
			NameValueCollection parameters = new NameValueCollection();
			parameters.Add("folderid", folderId.ToString());

			HttpWebRequest req = makeRequest("DELETE", "removefoldertags", parameters);

			return new BasicResponse(req);
		}

		public Response GetFileTags(Guid fileId) {
			NameValueCollection parameters = new NameValueCollection();
			parameters.Add("fileid", fileId.ToString());

			HttpWebRequest req = makeRequest("GET", "getfiletags", parameters);

			return new BasicResponse(req);
		}

		public Response GetFolderTags(long folderId) {
			NameValueCollection parameters = new NameValueCollection();
			parameters.Add("folderid", folderId.ToString());

			HttpWebRequest req = makeRequest("GET", "getfoldertags", parameters);

			return new BasicResponse(req);
		}

		public Response RemoveTag(string tag) {
			NameValueCollection parameters = new NameValueCollection();
			parameters.Add("tag", tag);

			HttpWebRequest req = makeRequest("DELETE", "removetag", parameters);

			return new BasicResponse(req);
		}

		public CustomResponse<FileCollection> GetFilesByTag(string tag) {
			NameValueCollection parameters = new NameValueCollection();
			parameters.Add("tag", tag);

			HttpWebRequest req = makeRequest("GET", "getfilesbytag", parameters);

			return new CustomResponse<FileCollection>(req);
		}

		public CustomResponse<FolderCollection> GetFoldersByTag(string tag) {
			NameValueCollection parameters = new NameValueCollection();
			parameters.Add("tag", tag);

			HttpWebRequest req = makeRequest("GET", "getfoldersbytag", parameters);

			return new CustomResponse<FolderCollection>(req);
		}

		public CustomResponse<TagCollection> GetAllTags() {
			HttpWebRequest req = makeRequest("GET", "getalltags", null);

			return new CustomResponse<TagCollection>(req);
		}

		#endregion

		#region "Sharing Operations"

		public CustomResponse<SharedFolder> GetSharedFolder(long folderId) {
			NameValueCollection parameters = new NameValueCollection();
			parameters.Add("folderid", folderId.ToString());

			HttpWebRequest req = makeRequest("GET", "getsharedfolder", parameters);

			return new CustomResponse<SharedFolder>(req);
		}

		public Response ShareFolder(long folderId, bool enabled, string comment) {
			NameValueCollection parameters = new NameValueCollection();
			parameters.Add("folderid", folderId.ToString());
			parameters.Add("enabled", enabled.ToString());
			parameters.Add("comment", comment);

			HttpWebRequest req = makeRequest("POST", "sharefolder", null);

			return new BasicResponse(req);
		}

		public Response RemoveFolderSharing(long folderId) {
			NameValueCollection parameters = new NameValueCollection();
			parameters.Add("folderid", folderId.ToString());

			HttpWebRequest req = makeRequest("DELETE", "removefoldersharing", null);

			return new BasicResponse(req);
		}

		public Response AddPermission(long folderId, string userName, AccessType accessType) {
			NameValueCollection parameters = new NameValueCollection();
			parameters.Add("folderid", folderId.ToString());
			parameters.Add("username", username);
			parameters.Add("accesstype", accessType.ToString());

			HttpWebRequest req = makeRequest("POST", "addpermission", null);

			return new BasicResponse(req);
		}

		public Response RemovePermission(long folderId, string userName) {
			NameValueCollection parameters = new NameValueCollection();
			parameters.Add("folderid", folderId.ToString());
			parameters.Add("username", username);

			HttpWebRequest req = makeRequest("DELETE", "removepermission", null);

			return new BasicResponse(req);
		}

		#endregion

		#region "Private Helper Methods"

		private HttpWebRequest makeRequest(string method, string resource, NameValueCollection parameters) {
			return makeRequest(method, resource, parameters, 3600000);
		}

		private HttpWebRequest makeRequest(string method, string resource, NameValueCollection parameters, int timeout) {
			string url = address + resource + ".axd";
			if (parameters != null && parameters.Count > 0) {
				for (int i = 0; i < parameters.Count; ++i) {
					url += (i == 0) ? "?" : "&";
					url += parameters.Keys[i] + "=" + parameters[i];
				}
			}

			HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
			req.Timeout = timeout;
			req.Method = method;
			req.ContentLength = 0;

			AddAuthenticationHeader(req);

			return req;
		}

		private void AddAuthenticationHeader(WebRequest request) {
			request.Headers.Add("User-Authentication", Utils.EncodeTo64(username + ":" + password));
		}

		#endregion
	}
}
