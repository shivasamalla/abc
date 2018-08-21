using Microsoft.AspNetCore.Http;
using RBTCreditControl.Entity;
using RBTCreditControl.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBTCreditControl.WebApp.Models
{
    public interface IErrLogService
    {
        void Log(Exception exception,HttpContext context, string Para=null);
    }
    public class ErrLogService : IErrLogService
    {
        private RBTCreditControl_Context _context = null;
        public ErrLogService(RBTCreditControl_Context DBcontext)
        {
            _context = DBcontext;
        }
        public void Log(Exception exception, HttpContext context, string Para = null)
        {
            string message = exception.Message;
           
            StringBuilder sbExceptionMessage = new StringBuilder();
            do
            {
                sbExceptionMessage.Append("Exception Type" + Environment.NewLine);
                sbExceptionMessage.Append(exception.GetType().Name);
                sbExceptionMessage.Append(Environment.NewLine + Environment.NewLine);
                sbExceptionMessage.Append("Message" + Environment.NewLine);
                sbExceptionMessage.Append(exception.Message + Environment.NewLine + Environment.NewLine);
                sbExceptionMessage.Append("Stack Trace" + Environment.NewLine);
                sbExceptionMessage.Append(exception.StackTrace + Environment.NewLine + Environment.NewLine);
                exception = exception.InnerException;
            }
            while (exception != null);

            ErrorLog logDetails = new ErrorLog
            {
                ErrorMsg = message,
                ErrorDetails = sbExceptionMessage.ToString(),
                Para = Para,
                Uri= HttpContextExtentions.GetRequestUri(context)
            };

            _context.Add(logDetails);
            _context.SaveChanges();
        }


    }
}
