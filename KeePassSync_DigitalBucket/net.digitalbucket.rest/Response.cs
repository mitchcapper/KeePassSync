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
    public abstract class Response
    {
        protected HttpWebResponse response = null;
        
        public Response(HttpWebRequest request)
        {
            try
            {
                response = request.GetResponse() as HttpWebResponse;

                // this means that we got 200 - OK as reposne and we may get more detail   
                _success = true;
                _statusCode = 200;

                if (response != null && !String.IsNullOrEmpty(response.StatusDescription))
                {
                    _statusDescription = response.StatusDescription;
                }
                else
                {
                    _statusDescription = "Operation done successfully";
                }
            }
            catch (WebException wex)
            {
                _success = false;

                if (wex.Response != null)
                {
                    using (HttpWebResponse errorResponse = (HttpWebResponse)wex.Response)
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(Error));
                        Error err = (Error)serializer.Deserialize(errorResponse.GetResponseStream());

                        if (err != null)
                        {
                            _statusCode = err.Code;
                            _statusDescription = err.Message;
                        }
                        else
                        {
                            _statusDescription = wex.Message;
                        }
                    }
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                _success = false;
                _statusDescription = ex.Message;
            }
        }

        protected bool _success;
        public bool Success
        {
            get { return _success; }
            set { _success = value; }
        }

        protected int _statusCode;
        public int StatusCode
        {
            get { return _statusCode; }
            set { _statusCode = value; }
        }

        protected string _statusDescription;
        public string StatusDescription
        {
            get { return _statusDescription; }
            set { _statusDescription = value; }
        }
    }   
}
