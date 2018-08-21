using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Internal;
using System.Text;
using System.IO;
using RBTCreditControl.Repository;
using RBTCreditControl.Entity;

namespace RBTCreditControl.WebApp.Models
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private static IErrLogService _LogService = null;
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, IErrLogService LogService /* other dependencies */)
        {
            try
            {
                _LogService = LogService;
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task<Task> HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected
            if (exception is UnauthorizedAccessException)
            {
                code = HttpStatusCode.Unauthorized;
            }
            else
            {
                string reqPara = await FormatRequest(context.Request);
               // _LogService.Log(exception, reqPara, "RBT Credit Control");
            }
            //if (exception is NotFiniteNumberException) code = HttpStatusCode.NotFound;
            //else if (exception is UnauthorizedAccessException) code = HttpStatusCode.Unauthorized;
            //else if (exception is ) code = HttpStatusCode.BadRequest;

            var result = JsonConvert.SerializeObject(new { error = exception.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }

        private static async Task<string> FormatRequest(HttpRequest request)
        {
            var body = request.Body;
            request.EnableRewind();
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            request.Body = body;
            request.Body.Seek(0, SeekOrigin.Begin);
            var messageObjToLog = new { scheme = request.Scheme, host = request.Host, path = request.Path, queryString = request.Query, requestBody = bodyAsText };
            return JsonConvert.SerializeObject(messageObjToLog);
        }


       
    }
}
