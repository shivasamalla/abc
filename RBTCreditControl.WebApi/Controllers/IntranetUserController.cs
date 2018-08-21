using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using RBTCreditControl.Entity;
using RBTCreditControl.WebApp.Models;
using RBTCreditControl.Repository;
using Newtonsoft.Json;


namespace RBTCreditControl.WebApp.Controllers
{
    [Produces("application/json")] 
    public class IntranetUserController : Controller
    {
        private readonly RBTCreditControl_Context _context = null;
        private readonly Intranet_DBContext context_Intra = null;
        private readonly ICacheManager<User> _CacheManager = null;
        private static IErrLogService _ErrLogService = null;
        public IntranetUserController(RBTCreditControl_Context context, Intranet_DBContext _context_Intra, ICacheManager<User> CacheManager, IErrLogService LogService)
        {
            _context = context;
            context_Intra = _context_Intra;
            _CacheManager = CacheManager;
            _ErrLogService = LogService;
        }
        [HttpGet]
        [Route("api/IntranetUser/GetEmployee")]
        public Object GetEmpData(string empCode)
        {
            try
            {
                var resp = context_Intra.Emp_Data.FirstOrDefault(x => x.emp_status == "N" && x.emp_code == empCode);
                if (resp != null)
                    return Ok(resp);
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                _ErrLogService.Log(ex, HttpContext);
                return StatusCode(500, new { status = true, Msg = ex.Message });
            }

        }
    }
}