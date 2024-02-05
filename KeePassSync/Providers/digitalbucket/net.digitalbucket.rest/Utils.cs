// This code is provided "AS IS" without warranties 
// of any kind, bugs are probable, free for any use 
// at own risk, no responsibility accepted.
// (c) 2007 digitalbucket.net

using System;

namespace digitalbucket.net.rest {
	class Utils {
		public static string GetStramText(System.IO.Stream stream) {
			string result = null;
			using (System.IO.StreamReader sr = new System.IO.StreamReader(stream)) {
				result = sr.ReadToEnd();
			}
			return result;
		}

		public static string EncodeTo64(string toEncode) {
			byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
			string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
			return returnValue;
		}
	}
}
