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

namespace RBTCreditControl.WebApp.Controllers.RBTUser
{
    [Produces("application/json")]  
    public class RBTAdminController : Controller
    {
        private readonly RBTCreditControl_Context _context = null;
        private readonly Intranet_DBContext context_Intra = null;
        private readonly ICacheManager<User> _CacheManager = null;
        private static IErrLogService _ErrLogService = null;
        public RBTAdminController(RBTCreditControl_Context context, Intranet_DBContext _context_Intra, ICacheManager<User> CacheManager, IErrLogService LogService)
        {
            _context = context;
            context_Intra = _context_Intra;
            _CacheManager = CacheManager;
            _ErrLogService = LogService;
        }

        // GET: api/User
        [HttpGet]
        [Route("api/RBTAdmin/GetAllUsers")]
        public Object GetUser(int page, int pagesize)
        {
            try
            {
                var resp = _context.User.Include(x => x.Supervisor).Where(x => x.UserType != "Admin")
               .Select(x => new
               {
                   x.Id,
                   x.UserType,
                   x.IsActive,
                   x.Email,
                   x.Name,
                   x.EmpCode,
                   x.Password,
                   Supervisor = x.Supervisor.Name
               }).GetPaged(page, pagesize);
                var TotalRecords = resp.RowCount;
                resp.Results = resp.Results.OrderBy(x => x.Name).ToList();
                return StatusCode(200, new { TotalRecords, resp });

            }
            catch (Exception ex)
            {
                _ErrLogService.Log(ex, HttpContext);
                return StatusCode(500, new { status = true, Msg = ex.Message });
            }

        }
    }
}