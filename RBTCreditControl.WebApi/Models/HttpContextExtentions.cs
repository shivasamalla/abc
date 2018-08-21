using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBTCreditControl.WebApp.Models
{
    public static class HttpContextExtentions
    {
        public static string GetRequestUri(HttpContext context)
        {
            string str = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(context.Request.Host + context.Request.Path);
            foreach (var item in context.Request.Query)
            {
                var  Fitem = context.Request.Query.First();
                if (Fitem.Key==item.Key)
                    sb.Append(string.Format("?{0}={1}", item.Key, item.Value));
                else 
                    sb.Append(string.Format("&{0}={1}", item.Key, item.Value));
            }
            return sb.ToString();
        }
    }
}
