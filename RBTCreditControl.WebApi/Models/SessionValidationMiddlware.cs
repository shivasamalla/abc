using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RBTCreditControl.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBTCreditControl.WebApp.Models
{
    public class SessionValidationMiddlware
    {
        private readonly RequestDelegate next;
        private  ICacheManager<User> _CacheManager = null;
        private readonly IHostingEnvironment _hostingEnvironment;
        public SessionValidationMiddlware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, IHostingEnvironment hostingEnvironment, ICacheManager<User> CacheManager /* other dependencies */)
        {
            //await next(context); // disable session validation

            _CacheManager = CacheManager;
            string Token = context.Request.Headers["Token"];
            Token = context.Request.Query["key"];

            string l = context.Request.Path;

            string webRootPath = hostingEnvironment.WebRootPath;
            string contentRootPath = hostingEnvironment.ContentRootPath;
            await next(context);
            //if (context.Request.Path == "/" || context.Request.Path.ToString().Contains("Home/AuthenticateUser") || context.Request.Path.ToString().Contains("api/User/Login") ||  context.Request.Path.ToString().Contains("Home/Login") || context.Request.Path.ToString().Contains("home/ChangePassword")|| context.Request.Path.ToString().Contains("UserAuthorization/ChangePassword"))
            //    await next(context);

            //else if (!string.IsNullOrEmpty(Token))
            //{
            //    if (!CheckUserSession(Token))
            //    {
            //       // context.Response.StatusCode = 401;
            //        //context.Response.ContentType = "application/json";
            //        // string jsonString = JsonConvert.SerializeObject(new { Msg = "Invalid Token!" });
            //        context.Response.Redirect("/");
            //        // await context.Response.WriteAsync(jsonString, Encoding.UTF8);
            //        // to stop futher pipeline execution 
            //        return;
            //    }
            //    else
            //    {

            //        await next(context);
            //    }
            //}
            //else
            //{
            //    //context.Response.StatusCode = 401;
            //    //context.Response.ContentType = "application/json";
            //    //string jsonString = JsonConvert.SerializeObject(new { Msg = "Invalid Token!" });
            //    //await context.Response.WriteAsync("<script>alert('s')</script>", Encoding.UTF8);
            //    // to stop futher pipeline execution 
            //    context.Response.Redirect("/");
            //   return;
            //}
        }

        private bool CheckUserSession(string Key)
        {
            return _CacheManager.CheckSessionExists(Key);
        }
    }
}
