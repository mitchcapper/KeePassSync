// This code is provided "AS IS" without warranties 
// of any kind, bugs are probable, free for any use 
// at own risk, no responsibility accepted.
// (c) 2007 digitalbucket.net

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Xml.Serialization;

namespace digitalbucket.net.rest
{
    public class CustomResponse<T> : Response
    {
        public CustomResponse(HttpWebRequest request)
            : base(request)
        {
            if (_success == false || response == null)
                return;

            // get the response text
            try
            {
                System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream());
                string responseText = reader.ReadToEnd();

                // serialize response
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                if (_success && !String.IsNullOrEmpty(responseText))
                {
                    _responseObject = (T)serializer.Deserialize(new System.IO.StringReader(responseText));
                }
            }
            catch (Exception ex)
            {
                _success = false;
                _statusDescription = ex.Message;
            }
            finally
            {
                if (response != null)
                    response.Close();
            }
        }

        private T _responseObject;
        public T ResponseObject
        {
            get { return _responseObject; }
            set { _responseObject = value; }
        }
    } 
}
