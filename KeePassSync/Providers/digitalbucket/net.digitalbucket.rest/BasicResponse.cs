// This code is provided "AS IS" without warranties 
// of any kind, bugs are probable, free for any use 
// at own risk, no responsibility accepted.
// (c) 2007 digitalbucket.net

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Xml.Serialization;

namespace digitalbucket.net.rest {
	public class BasicResponse : Response {
		public BasicResponse(HttpWebRequest request)
			: base(request) {
			if (_success == false || response == null)
				return;

			// get the response text
			try {
				XmlSerializer serializer = new XmlSerializer(typeof(Success));
				Success suc = (Success)serializer.Deserialize(response.GetResponseStream());

				_statusCode = suc.Code;
				_statusDescription = suc.Message;
			} catch (Exception ex) {
				_success = false;
				_statusDescription = ex.Message;
			} finally {
				if (response != null)
					response.Close();
			}
		}
	}
}
