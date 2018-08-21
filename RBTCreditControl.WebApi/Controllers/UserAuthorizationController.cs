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
    public class UserAuthorizationController : Controller
    {
        private readonly RBTCreditControl_Context _context = null;
        private readonly Intranet_DBContext context_Intra = null;
        private readonly ICacheManager<User> _CacheManager = null;
        private static IErrLogService _ErrLogService = null;

        public UserAuthorizationController(RBTCreditControl_Context context, Intranet_DBContext _context_Intra, ICacheManager<User> CacheManager, IErrLogService LogService)
        {
            _context = context;
            context_Intra = _context_Intra;
            _CacheManager = CacheManager;
            _ErrLogService = LogService;
        }
        [HttpPost]
        [Route("api/UserAuthorization/ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromQuery] int id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                // _context.Attach(user);
                User exist = await _context.Set<User>().FindAsync(id);
                exist.Password = user.Password;
                await _context.SaveChangesAsync();
                return Ok(new { status = true, Msg = "Password Changed Successfully!" });

            }
            catch (Exception ex)
            {
                _ErrLogService.Log(ex, HttpContext);
                return StatusCode(500, new { status = true, Msg = ex.Message });
            }
        }
    }
}