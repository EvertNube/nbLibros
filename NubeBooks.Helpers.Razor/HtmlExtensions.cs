using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace NubeBooks.Helpers.Razor
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString hidePortrait(this HtmlHelper htmlHelper, bool classOnly = true)
        {
            var hide = "hidePortrait";
            if (!classOnly) hide = "class='hidePortrait'";
            return MvcHtmlString.Create(hide);
        }

        public static MvcHtmlString hideXS(this HtmlHelper htmlHelper, bool classOnly = true)
        {
            var hide = "hidden-xs";
            if (!classOnly) hide = "class='hidden-xs'";
            return MvcHtmlString.Create(hide);
        }

        public static MvcHtmlString showAlertMessage(this HtmlHelper htmlHelper, string status_field, string message = "") { 
            if (status_field != null && !String.IsNullOrWhiteSpace(status_field))
            {
                StringBuilder sb = new StringBuilder();
                if (status_field == CONSTANTES.SUCCESS)
                {
                    sb.Append("<div class='alert alert-success'>");
                    sb.Append("<button type='button' class='close' data-dismiss='alert'>×</button>");
                    sb.AppendFormat("<i class='fa fa-ok-sign'></i> ");
                    if (message != null && !String.IsNullOrWhiteSpace(message)){ sb.Append(message); }
                    else { sb.Append(CONSTANTES.SUCCESS_MESSAGE); }
                    sb.Append("</div>");
                }
                else if (status_field == CONSTANTES.ERROR)
                { 
                    sb.Append("<div class='alert alert-danger'>");
                    sb.Append("<button type='button' class='close' data-dismiss='alert'>×</button>");
                    sb.AppendFormat("<i class='fa fa-ban-circle'></i> ");
                    if (message != null && !String.IsNullOrWhiteSpace(message)){ sb.Append(message); }
                    else { sb.Append(CONSTANTES.ERROR_MESSAGE); }
                    sb.Append("</div>");
                }
                return MvcHtmlString.Create(sb.ToString());
            }
            return MvcHtmlString.Empty;
        }

        public static string ToCapitalize(this string cadena) { 
            char[] a = cadena.ToLower().ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        public static string ToJSON(this object o)
        {
            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            return oSerializer.Serialize(o);
        }

    }
}
