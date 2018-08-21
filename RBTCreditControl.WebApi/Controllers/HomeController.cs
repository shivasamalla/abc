using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RBTCreditControl.Entity;
using RBTCreditControl.Repository;
using RBTCreditControl.WebApp.Models;

namespace RBTCreditControl.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly RBTCreditControl_Context _context = null;
        private readonly Intranet_DBContext context_Intra = null;
        private readonly ICacheManager<User> _CacheManager = null;
        private static IErrLogService _ErrLogService = null;

        public HomeController(RBTCreditControl_Context context, Intranet_DBContext _context_Intra, ICacheManager<User> CacheManager, IErrLogService LogService)
        {
            _context = context;
            context_Intra = _context_Intra;
            _CacheManager = CacheManager;
            _ErrLogService = LogService;
        }

  
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult ChangePassword()
        {
            return View();
        }
        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }

        public IActionResult AuthenticateUser([FromQuery]string empCode, [FromQuery]string password)
        {
            try
            {
                var ss = _context.User.ToList();
                var resp = _context.User.Include(x => x.lstUserLocation).SingleOrDefault(x => x.EmpCode == empCode && x.Password == password && x.IsActive == true);

                if (resp != null)
                {
                    Guid obj = Guid.NewGuid();
                    string key = obj.ToString();

                    if (resp.Password != "reset@123")
                    {
                        _CacheManager.GetOrSetUserSession(key, resp);
                        SetCookie("UserData",JsonConvert.SerializeObject(new { resp.Id, resp.UserType, resp.Name }), null);
                        return Ok(new { CPwdF = false, Status = true, Token = key, resp.UserType, id = resp.Id, resp.Name });
                    }
                    else
                    {
                        return StatusCode(200, new { Status = true,id=resp.Id, CPwdF = true,Msg = "Pass" });
                        // RedirectToAction("ChangePassword");
                        // HttpContext.Response.Redirect("Home/ChangePassword");
                    }
                }
                else
                    return StatusCode(200, new { Status = true,  Msg = "Invalid UserName or Password" });
            }
            catch (Exception ex)
            {
                _ErrLogService.Log(ex, HttpContext);
                return StatusCode(500, new { Status = false, MsgDetails = ex.Message, Msg="Internal Server Error" });
            }
            return NoContent();
            //return View("Index");
        }

        public IActionResult OneTimeChangepassword([FromQuery] int id, [FromQuery]string password)
        {
            try
            {
                // _context.Attach(user);
                User exist = _context.Set<User>().Find(id);
                exist.Password = password;
                _context.SaveChanges();
                // RedirectToAction("Login");
                return Ok(new { status = true, Msg = "Password Changed Successfully!" });

            }
            catch (Exception ex)
            {
                _ErrLogService.Log(ex, HttpContext);
                return StatusCode(500, new { status = true, Msg = ex.Message });
            }
        }
        public void SetCookie(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();
            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);
            Response.Cookies.Append(key, value, option);
        }
    }
}
