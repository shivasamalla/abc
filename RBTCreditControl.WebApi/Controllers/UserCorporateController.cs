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
    
    public class UserCorporateController : Controller
    {
        private readonly RBTCreditControl_Context _context;
        private readonly ICacheManager<User> _cache;
        private static IErrLogService _ErrLogService = null;
        public UserCorporateController(RBTCreditControl_Context context, ICacheManager<User> cache, IErrLogService LogService)
        {       
            _context = context;
            _cache = cache;
            _ErrLogService = LogService;
        }

        [HttpPost]
        [Route("api/UserCorporatePermission/UpdateUserCorporates")]
        public async Task<IActionResult> UpdateUserCorporates([FromQuery] int id, [FromBody]List<UserCorporate> userCorporate)
        {
            // Update User Corporate
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {

                for (int i = 0; i < userCorporate.Count; i++)
                {
                    userCorporate[i].ModifiedBy = id;
                    userCorporate[i].ModifiedOn = DateTime.Now;
                }

                var user = _context.User.Include(x => x.lstUserCorporate).SingleOrDefault(x => x.Id == id);
                user.lstUserCorporate = userCorporate;
                User existUser = await _context.Set<User>().FindAsync(id);
                if (existUser != null)
                {
                    _context.Entry(existUser).CurrentValues.SetValues(user);
                    await _context.SaveChangesAsync();
                    return StatusCode(200, new { status = true, Msg = "Record Updated!" });
                }
                else
                {
                    return StatusCode(200, new { status = false, Msg = "User Not Found!" });
                }
            }
            catch (Exception ex)
            {
                _ErrLogService.Log(ex, HttpContext);
                return StatusCode(500, new { status = true,Msg="Internal Server Error.", MsgDetails = ex.Message });
            }
        }
    }
}