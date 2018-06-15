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
        public static T Get<T>(string url,string failHint = "加载失败")
        {
            return Request<T>(url,"GET",null,failHint);
        }

        public static T Post<T>(string url,string bodyStr, string failHint = "加载失败")
        {
            return Request<T>(url, "POST", bodyStr,failHint);
        }

        public static T Put<T>(string url,string bodyStr, string failHint = "加载失败")
        {
            return Request<T>(url, "PUT", bodyStr,failHint);
        }

        public static T Delete<T> (string url,string bodyStr, string failHint = "加载失败")
        {
            return Request<T>(url, "DELETE", bodyStr,failHint);
        }

        public static T Request<T>(string url, string method = "GET", string bodyStr = null, string failHint = "加载失败")
        {
            string responseStr = null;
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            try
            {
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

            }
            catch (WebException ex)
            {
                string message = ex.Message;
                if (ex.Response != null)
                {
                    message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                MessageBox.Show(failHint + "：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return default(T);
                throw new Exception(ex.ToString());
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            T result = serializer.Deserialize<T>(responseStr);
            return result;
        }

        public static T RequestPost<T>(string url,string bodyStr = null,string method = "POST")
        {
            string responseStr = null;
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
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
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            T result = serializer.Deserialize<T>(responseStr);
            return result;
        }
    }
}
