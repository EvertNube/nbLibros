using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace NubeBooks.Helpers
{
    public static class ShortUrl
    {
        public static string Shorten(string urlToEncode)
        { 
            using (var client = new WebClient()){
                var url = CONSTANTES.URL_BITLY_API;
                NameValueCollection data = new NameValueCollection();
                data.Add("access_token", ConfigurationManager.AppSettings["BitlyToken"].ToString());
                data.Add("longUrl", urlToEncode);
                byte[] result = client.UploadValues(url, data);
                String json = Encoding.ASCII.GetString(result);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                dynamic item = serializer.Deserialize<object>(json);
                try
                {
                    if (item != null && item["status_code"] == 200 && item["data"] != null && item["data"]["url"] != null)
                    {
                        return (string)item["data"]["url"];
                    }
                    else { return ""; }
                }
                catch (Exception e)
                {
                    //throw e;
                }
                return "";
            }
        }
    }
}
