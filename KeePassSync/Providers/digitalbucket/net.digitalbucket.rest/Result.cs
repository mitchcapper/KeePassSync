// This code is provided "AS IS" without warranties 
// of any kind, bugs are probable, free for any use 
// at own risk, no responsibility accepted.
// (c) 2007 digitalbucket.net

using System;

namespace digitalbucket.net.rest {
	public class Error {
		public Error() { }

		public Error(int code, string message) {
			_code = code;
			_message = message;
		}

		private int _code;
		public int Code {
			get { return _code; }
			set { _code = value; }
		}

		private string _message;
		public string Message {
			get { return _message; }
			set { _message = value; }
		}
	}

	public class Success {
		public Success() { }

		public Success(int code, string message) {
			_code = code;
			_message = message;
		}

		private int _code;
		public int Code {
			get { return _code; }
			set { _code = value; }
		}

		private string _message;
		public string Message {
			get { return _message; }
			set { _message = value; }
		}
	}
}
