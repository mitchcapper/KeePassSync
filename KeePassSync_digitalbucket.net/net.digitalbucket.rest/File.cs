// This code is provided "AS IS" without warranties 
// of any kind, bugs are probable, free for any use 
// at own risk, no responsibility accepted.
// (c) 2007 digitalbucket.net

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace digitalbucket.net.rest
{
    [XmlRoot("File")]
    public class File
    {
        private Guid _id;
        private long _folderId;
        private string _name;
        private long _size;
        private DateTime _createDate;
        private DateTime _lastModified;
        private string _comment;
        private bool _published;
        private string _tags;

        public File()
        { }

        public Guid FileID
        {
            get { return _id; }
            set { _id = value; }
        }

        public long FolderID
        {
            get { return _folderId; }
            set { _folderId = value; }
        }

        public string FileName
        {
            get { return _name; }
            set { _name = value; }
        }

        public long FileSize
        {
            get { return _size; }
            set { _size = value; }
        }

        public DateTime CreateDate
        {
            get { return _createDate; }
            set { _createDate = value; }
        }

        public DateTime LastModified
        {
            get { return _lastModified; }
            set { _lastModified = value; }
        }

        public string Comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        public bool Published
        {
            get { return _published; }
            set { _published = true; }
        }

        public string Tags
        {
            get { return _tags; }
            set { _tags = value; }
        }
    }

    [XmlRoot("Files")]
    public class FileCollection
    {
        private List<File> _files;

        public FileCollection()
        { }

        [XmlElementAttribute("File")]
        public List<File> Files
        {
            get { return _files; }
            set { _files = value; }
        }
    }
}
