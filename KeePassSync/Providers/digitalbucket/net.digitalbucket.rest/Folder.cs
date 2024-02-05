// This code is provided "AS IS" without warranties 
// of any kind, bugs are probable, free for any use 
// at own risk, no responsibility accepted.
// (c) 2007 digitalbucket.net

using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace digitalbucket.net.rest {
	[XmlRoot("Folder")]
	public class Folder {
		private long _id;
		private long _parentId;
		private string _name;
		private string _description;
		private DateTime _createDate;
		private bool _shared;
		private string _tags;
		private bool _published;
		private List<Folder> _childFolders;
		private List<File> _childFiles;

		public Folder() { }

		public long FolderID {
			get { return _id; }
			set { _id = value; }
		}

		public long ParentFolderID {
			get { return _parentId; }
			set { _parentId = value; }
		}

		public string FolderName {
			get { return _name; }
			set { _name = value; }
		}

		public string Comment {
			get { return _description; }
			set { _description = value; }
		}

		public DateTime CreateDate {
			get { return _createDate; }
			set { _createDate = value; }
		}

		public bool Shared {
			get { return _shared; }
			set { _shared = value; }
		}

		public bool Published {
			get { return _published; }
			set { _published = value; }
		}

		public string Tags {
			get { return _tags; }
			set { _tags = value; }
		}

		[XmlArrayItem("Folder")]
		public List<Folder> ChildFolders {
			get { return _childFolders; }
			set { _childFolders = value; }
		}

		[XmlArrayItem("File")]
		public List<File> ChildFiles {
			get { return _childFiles; }
			set { _childFiles = value; }
		}
	}

	[XmlRoot("Folders")]
	public class FolderCollection {
		private List<Folder> _folders;

		public FolderCollection() { }

		[XmlElementAttribute("Folder")]
		public List<Folder> Folders {
			get { return _folders; }
			set { _folders = value; }
		}
	}

}
