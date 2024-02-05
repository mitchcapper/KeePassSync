// This code is provided "AS IS" without warranties 
// of any kind, bugs are probable, free for any use 
// at own risk, no responsibility accepted.
// (c) 2007 digitalbucket.net

using System;
using System.Xml;
using System.Xml.Serialization;

namespace digitalbucket.net.rest {
	[XmlRoot("Tags")]
	public class TagCollection {
		private Tag[] _tags;

		public TagCollection() { }

		[XmlElementAttribute("Tag")]
		public Tag[] Tags {
			get { return _tags; }
			set { _tags = value; }
		}
	}

	public struct Tag {
		public string Name;
		public int Count;

		public Tag(string name, int count) {
			Name = name;
			Count = count;
		}
	}
}
