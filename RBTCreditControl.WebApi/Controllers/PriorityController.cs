using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RBTCreditControl.Entity;
using RBTCreditControl.Repository;
using RBTCreditControl.WebApp.Models;

namespace RBTCreditControl.WebApp.Controllers
{
    [Produces("application/json")]
    public class PriorityController : Controller
    {
        private readonly RBTCreditControl_Context _context = null;
        //private readonly Intranet_DBContext context_Intra = null;
        private readonly ICacheManager<User> _cache=null;
        private static IErrLogService _ErrLogService = null;       
        public PriorityController(RBTCreditControl_Context context, ICacheManager<User> cache, IErrLogService LogService)
        {
            _context = context;
            _cache = cache;
            _ErrLogService = LogService;
        }

        [HttpPost]
        [Route("api/Priority/SetPriority")]
        public object  SetPriority([FromQuery] int priority, [FromQuery] int CorporateId)
        {
            try
            {
                CorporateMaster Corporate = new CorporateMaster();
                Corporate.Id = CorporateId;

                CorporateMaster CorpMaster = _context.CorporateMaster.SingleOrDefault(x=>x.Id==CorporateId);

                CorpMaster.Priority = (CorpMaster.Priority == 1) ? 0 : 1;
                CorpMaster.PriorityUpdatedBy = _cache.GetOrSetUserSession(Request.Query["key"], null).Id;
                CorpMaster.PrioriyUpdatedOn = DateTime.Now;

                _context.SaveChanges();

                return StatusCode(200, new { status = true, CorpMaster.Priority,Msg = "Priority Set Successfully!" });
            }
            catch (Exception ex)
            {
                _ErrLogService.Log(ex, HttpContext);
                return StatusCode(500, new { status = true,Msg="Internal Server Error", MsgDeatail = ex.Message });
            }
        }
    }
}