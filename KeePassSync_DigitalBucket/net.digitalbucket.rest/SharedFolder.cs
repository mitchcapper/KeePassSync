// This code is provided "AS IS" without warranties 
// of any kind, bugs are probable, free for any use 
// at own risk, no responsibility accepted.
// (c) 2007 digitalbucket.net

using System;
using System.Collections.Generic;
using System.Text;

namespace digitalbucket.net.rest
{
    public class SharedFolder
    {
        private long _folderId;
        private bool _enabled;
        private string _comment;
        private List<Permission> _permissions;

        public SharedFolder()
        { }

        public long FolderID
        {
            get { return _folderId; }
            set { _folderId = value; }
        }

        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        public string Comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        public List<Permission> Permissions
        {
            get
            {
                if (_permissions == null)
                    _permissions = new List<Permission>();
                return _permissions;
            }
        }
    }

    public class Permission
    {
        private string _userName;
        private AccessType _userAccessType;

        public Permission()
        { }

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        public AccessType UserAccessType
        {
            get { return _userAccessType; }
            set { _userAccessType = value; }
        }
    }

    public enum AccessType
    {
        View,
        FullControl
    }
}
