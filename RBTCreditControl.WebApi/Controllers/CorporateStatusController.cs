using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RBTCreditControl.Entity;
using RBTCreditControl.Repository;
using RBTCreditControl.WebApp.Models;

namespace RBTCreditControl.WebApp.Controllers
{
    [Produces("application/json")]
    public class CorporateStatusController : Controller
    {
        private readonly RBTCreditControl_Context _context = null;
        //private readonly Intranet_DBContext context_Intra = null;
        private readonly ICacheManager<User> _cache;
        private static IErrLogService _ErrLogService = null;


        public CorporateStatusController(RBTCreditControl_Context context, ICacheManager<User> cache, IErrLogService LogService)
        {
            _context = context;
            _cache = cache;
            _ErrLogService = LogService;
        }

        // GET: api/CorporateAction
        [HttpGet]
        [Route("api/CorporateStatus/GetStatus")]
        public Object Get()
        {
           try
            {
                var statusList = _context.StatusMaster.Where(x=>x.Id!=(int)CorporateStatus.Submition).ToList();
                return Ok(new { status = true, statusList, Msg = "Done!" });
            }
            catch (Exception ex)
            {
                _ErrLogService.Log(ex, HttpContext, JsonConvert.SerializeObject(ex));
                return StatusCode(500, new { status = false, Msg = "Internal Server Error!",MsgDetails = ex.Message });
            }
        }
        [HttpGet]
        [Route("api/CorporateStatus/GetStausForReports")]
        public Object GetStausForReports()
        {
            try
            {
                var statusList = _context.StatusMaster.ToList();
                return Ok(new { status = true, statusList, Msg = "Done!" });
            }
            catch (Exception ex)            
            {
                _ErrLogService.Log(ex, HttpContext, JsonConvert.SerializeObject(ex));
                return StatusCode(500, new { status = false, Msg = "Internal Server Error!", MsgDetails = ex.Message });

            }
        }

    }
}