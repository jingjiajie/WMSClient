using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace WMS.UI
{
    class RestClient
    {
        public static T Get<T>(string url) 
        {
            return Request<T>(url,"GET");
        }

        public static T Post<T>(string url,string bodyStr)
        {
            return Request<T>(url, "POST", bodyStr);
        }

        public static T Put<T>(string url,string bodyStr)
        {
            return Request<T>(url, "PUT", bodyStr);
        }

        public static T Delete<T> (string url,string bodyStr)
        {
            return Request<T>(url, "DELETE", bodyStr);
        }

        public static T Request<T>(string url, string method = "GET", string bodyStr = null)
        {
            string responseStr = null;
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;          
            //try
            //{
                request.Method = method;
                if (!string.IsNullOrWhiteSpace(bodyStr))
                {
                    request.ContentType = "application/json";
                    byte[] bytes = Encoding.UTF8.GetBytes(bodyStr);
                    request.GetRequestStream().Write(bytes, 0, bytes.Length);     
                }
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    responseStr = reader.ReadToEnd();
                }
            /*
            }
            catch (WebException ex)
            {
               MessageBox.Show("加载失败：" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return default(T);
                throw new Exception(ex.ToString());              
            }
            */
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            T result = serializer.Deserialize<T>(responseStr);
            return result;
        }
    }
}
