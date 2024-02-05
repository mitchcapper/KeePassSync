// This code is provided "AS IS" without warranties 
// of any kind, bugs are probable, free for any use 
// at own risk, no responsibility accepted.
// (c) 2007 digitalbucket.net

using System;
using System.Net;

namespace digitalbucket.net.rest {
	public class StreamResponse : Response {
		public StreamResponse(HttpWebRequest request)
			: base(request) {
			if (_success && response != null)
				_responseStream = response.GetResponseStream();
		}

		private System.IO.Stream _responseStream;
		public System.IO.Stream ResponseStream {
			get { return _responseStream; }
			set { _responseStream = value; }
		}
	}
}
