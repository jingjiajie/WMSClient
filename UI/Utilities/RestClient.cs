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
            string responseStr = null;
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    responseStr = reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                MessageBox.Show("加载失败：" + ex.Message,"提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return default(T);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            T result = serializer.Deserialize<T>(responseStr);
            return result;
        }
    }
}
