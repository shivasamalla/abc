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
    //[BasicAuthorizeAttribute("my-example-realm.com")]
    [Produces("application/json")]
    public class UserController : Controller
    {
        private readonly RBTCreditControl_Context _context = null;
        private readonly Intranet_DBContext context_Intra = null;
        private readonly ICacheManager<User> _CacheManager = null;
        private static IErrLogService _ErrLogService = null;

        public UserController(RBTCreditControl_Context context, Intranet_DBContext _context_Intra, ICacheManager<User> CacheManager, IErrLogService LogService)
        {
            _context = context;
            context_Intra = _context_Intra;
            _CacheManager = CacheManager;
            _ErrLogService = LogService;
        }

       
        

        // GET: api/User
        [HttpGet]
        [Route("api/User/GetALL_P")]
        public Object GetUser(int page, int pagesize, int userId)
        {
            try
            {
                User user = _CacheManager.GetOrSetUserSession(Request.Query["key"], null);

                var resp = _context.User.Include(r => r.lstUserCorporate)
                                       .Include(x => x.lstUserLocation)
                                       .Include(x => x.Supervisor)
                                       .Where(x => x.UserType != "Admin" && x.UserType != "Supervisor" && x.IsActive &&
                                                  user.lstUserLocation.Any(M => x.lstUserCorporate.Any(y => y.CorporateMaster.fk_LocationId == M.FK_LocationId))
                                              )
                    .Select(x => new
                    {
                        x.Id,
                        x.UserType,
                        x.IsActive,
                        x.Name,
                        x.EmpCode,
                        Supervisor = x.Supervisor.Name,
                    }).GetPaged(page, pagesize);

                var TotalRecords = resp.RowCount;
                return StatusCode(200, new { TotalRecords, resp });
            }
            catch (Exception ex)
            {
                _ErrLogService.Log(ex, HttpContext);
                return StatusCode(500, new { status = true, Msg = ex.Message });
            }
         
        }

        [HttpGet]
        [Route("api/User/GetUsersForSupervisor_ByLocation")]
        public Object GetUsersForSupervisor_ByLocation(int userId)
        {
            try
            {
                User user = _CacheManager.GetOrSetUserSession(Request.Query["key"], null);

                var resp = _context.User.Include(r => r.lstUserCorporate)
                                       .Include(x => x.lstUserLocation)
                                       .Include(x => x.Supervisor)
                                       .Where(x => x.UserType != "Admin" && x.UserType != "Supervisor" && x.IsActive &&
                                                  user.lstUserLocation.Any(M => x.lstUserCorporate.Any(y => y.CorporateMaster.fk_LocationId == M.FK_LocationId))
                                              )
                    .Select(x => new
                    {
                        x.Id, 
                        x.Name,
                        x.EmpCode
                    });
                               
                return StatusCode(200, new { resp });
            }
            catch (Exception ex)
            {
                _ErrLogService.Log(ex, HttpContext);
                return StatusCode(500, new { status = true, Msg = ex.Message });
            }

        }

        [HttpGet]
        [Route("api/User/Login")]
        public Object UserLogin(string empCode, string password)
        {
            try
            {
                var ss = _context.User.ToList();
                var resp = _context.User.Include(x => x.lstUserLocation).SingleOrDefault(x => x.EmpCode == empCode && x.Password == password && x.IsActive == true);

                if (resp != null)
                {
                    Guid obj = Guid.NewGuid();
                    string key = obj.ToString();
                   
                    if (resp.Password !="reset@123")
                    {
                        _CacheManager.GetOrSetUserSession(key, resp);
                        return Ok(new { CPwdF=false,Token = key, resp.UserType, id = resp.Id,resp.Name });
                    }
                    return Ok(new { CPwdF = true, Token = key, resp.UserType, id = resp.Id, resp.Name });
                }
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                _ErrLogService.Log(ex, HttpContext);
                return StatusCode(500, new { status = true, Msg = ex.Message });
            }

        }

        [HttpGet]
        [Route("api/User/LogOut")]
        public Object UserLogOut(string key)
        {
            try
            {
                _CacheManager.RemoveCacheObject(key);
                return Ok(new { status = true, Msg = "Logout Successfully!" });
            }
            catch (Exception ex)
            {
                _ErrLogService.Log(ex, HttpContext);
                return StatusCode(500, new { status = true, Msg = ex.Message });
            }
           
        }

        [HttpGet]
        [Route("api/User/CheckUserSession")]
        public Object UserLogin(string Token)
        {
            try
            {
                if (_CacheManager.CheckSessionExists(Token))
                    return Ok(new { status = true, Msg = "Session Exists." });
                else
                    return Ok(new { status = false, Msg = "Session Not Exists." });
            }
            catch (Exception ex)
            {
                _ErrLogService.Log(ex, HttpContext);
                return StatusCode(500, new { status = true, Msg = ex.Message });
            }
          
        }

        // GET: api/User/5
        [HttpGet]
        [Route("api/User/GetUserById")]
        public Object GetUserById([FromQuery] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                
                var UserDetails = _context.User.Include(y => y.lstUserLocation).Where(x => x.Id == id)
                   .Select(x => new
                   {
                       x.Id,
                       x.UserType,
                       x.IsActive,
                       x.Name,
                       x.Email,
                       x.EmpCode,
                       x.lstUserLocation,
                     //  SuppervisorName = x.Supervisor.Name,
                       //SuppervisorId = x.Supervisor.Id
                   });

                //   var user = await _context.User.SingleOrDefaultAsync(m => m.Id == id);

                if (UserDetails == null)
                {
                    return NotFound();
                }
                return Ok(new { UserDetails });
            }
            catch (Exception ex)
            {
                _ErrLogService.Log(ex, HttpContext);
                return StatusCode(500, new { status = true, Msg = ex.Message });
            }


        }

       
        // PUT: api/User/5
        [HttpPost]
        [Route("api/User/UpdateUserLocation")]
        public async Task<IActionResult> UpdateUserLocation([FromQuery] int id, [FromBody]User UserPara)
        {
            // Update User Corporate
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                User user = new User();
                user = await _context.User.Include(x => x.lstUserLocation).FirstOrDefaultAsync(x => x.Id == id);

              //  _context.User.Attach(user);

                user.lstUserLocation = UserPara.lstUserLocation;
                user.Name = UserPara.Name;
                user.Email= UserPara.Email;
                user.IsActive = UserPara.IsActive;
                user.ModifiedOn = DateTime.Now;
                user.ModifiedBy = _CacheManager.GetOrSetUserSession(Request.Query["key"], null).Id;

                _context.SaveChanges();
                return StatusCode(200, new { status = true, Msg = "Record Updated!" });
            }
            catch (Exception ex)
            {
                _ErrLogService.Log(ex, HttpContext);
                return StatusCode(500, new { status = true, Msg = ex.Message });
            }

        }


      

        [HttpPut]
        [Route("api/User/ActiveInActiveUser")]
        public async Task<IActionResult> PutActiveInActiveUser([FromQuery] int id, bool Flag)
        {
            try
            {
                User user = new User();
                user.Id = id;
                _context.User.Attach(user);
                // Now the entity is being tracked by EF, update required properties.
                user.IsActive = Flag;
                // EF knows only to update the propeties specified above.
                await _context.SaveChangesAsync();

                return StatusCode(200, new { Msg = "Status Updated Successfully.." });
            }
            catch (Exception ex)
            {
                _ErrLogService.Log(ex, HttpContext);
                return StatusCode(500, new { status = true, Msg = ex.Message });
            }
           
        }
        // POST: api/User
        [HttpPost]
        [Route("api/User/CreateUser")]
        public async Task<IActionResult> PostUser([FromBody] User user)
        {
            try
            {
              
                // trim All string property for User
                if (User == null || string.IsNullOrEmpty(user.EmpCode) || string.IsNullOrEmpty(user.Name))
                    return Ok(new { status = true, Msg = "Server Validation Failed.Bad Request!" });

                if (_context.User.Any(x => x.EmpCode == user.EmpCode && x.IsActive == true))
                    return Ok(new { status = true, Msg = "Already Exists!" });

                StringManipulation<User>.Trim(user);
                user.Password = "reset@123";
                user.CreatedBy = _CacheManager.GetOrSetUserSession(Request.Query["key"], null).Id;
                user.CreatedOn = DateTime.Now;

                if(user.UserType!="Supervisor")
                {
                    foreach (var item in user.lstUserCorporate)
                    {
                        item.CreatedBy = user.CreatedBy;
                        item.CreatedOn = DateTime.Now;
                    }
                }
               

                _context.User.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("Cannot insert duplicate key"))
                    return Ok(new { status = true, Msg = "Already Exists!" });
                
                _ErrLogService.Log(ex,HttpContext, JsonConvert.SerializeObject(user));
                return StatusCode(500,new { status = true, Msg = ex.Message });
            }
            return Ok(new { status = true, Msg = "Inserted Successfully!" });
        }

        // DELETE: api/User/5
        [HttpDelete]
        [Route("api/User/DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromQuery] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var user = await _context.User.SingleOrDefaultAsync(m => m.Id == id);
                if (user == null)
                {
                    return NotFound();
                }
                _context.User.Remove(user);
                await _context.SaveChangesAsync();
                return Ok(user);
            }
            catch (Exception ex)
            {
                _ErrLogService.Log(ex, HttpContext);
                return StatusCode(500, new { status = true, Msg = ex.Message });
            }
           
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}