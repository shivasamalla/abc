using System;
using System.Collections.Generic;
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
    [Produces("application/json")]

    public class BackOfficeController : Controller
    {
        private readonly RBTCreditControl_Context _context = null;

        private readonly ICacheManager<User> _CacheManager = null;
        private static IErrLogService _ErrLogService = null;

        public BackOfficeController(RBTCreditControl_Context context, ICacheManager<User> CacheManager, IErrLogService LogService)
        {
            _context = context;

            _CacheManager = CacheManager;
            _ErrLogService = LogService;
        }

        // GET: api/BackOffice/5
        [Route("api/BackOffice/GetDetails")]
        public Object GetUser(int page, int pagesize, int userid)
        {
            try
            {
                // var resp = _context.Corporate.Where(x => x.OstSubmittedBy == userid)
                //.Select(x => new
                //{
                //    x.Id,
                //    OstSubmittedOn = (x.OstSubmittedOn != null) ? ((DateTime)x.OstSubmittedOn).ToString("dd MMM yyyy hh:mm") : null,
                //    x.Name,
                //    x.No_,
                //    //toDate = (x.toDate != null) ? ((DateTime)x.toDate).ToString("dd MMM yyyy") : null,
                //    //fromDate = (x.fromDate != null) ? ((DateTime)x.fromDate).ToString("dd MMM yyyy") : null,
                //    //x.Amount,
                //}).GetPaged(page, pagesize);
                // var TotalRecords = _context.User.Include(x => x.Supervisor).Where(x => x.UserType != "Admin").Count();
                // return StatusCode(200, new { TotalRecords, resp });
                return null;
            }
            catch (Exception ex)
            {
                _ErrLogService.Log(ex, HttpContext);
                return StatusCode(500, new { status = true, Msg = ex.Message });
            }

        }

        [HttpPost]
        [Route("api/BackOffice/SubmitOst")]
        public async Task<IActionResult> PostStatus([FromQuery] long CorporateId, [FromBody] UserCorporateAction action)
        {
            try
            {

                if (action == null || CorporateId == 0 || action.FK_CorporateStatusMasterId == null)
                    return Ok(new { status = true, Msg = "Server Validation Failed.Bad Request!" });

                User UserLogin = _CacheManager.GetOrSetUserSession(Request.Query["key"], null);

                
                    CorporateMaster CorpDetails = _context.CorporateMaster
                                                 .SingleOrDefault(x => x.Id == CorporateId);

                    CorpDetails.UpdateFlag = false;
                    CorpDetails.OstSubmittedOn = DateTime.Now;
                    _context.SaveChanges();

                    SubmitedCorporate SubCorp = new SubmitedCorporate();
                    SubCorp.Balance = CorpDetails.Balance;
                    SubCorp.FK_CorporateMasterId = CorpDetails.Id;
                    SubCorp.lstUserCorporateAction = new List<UserCorporateAction>();
                    action.UpdatedBy = UserLogin.Id;
                    action.UpdatedOn = DateTime.Now;
                    SubCorp.lstUserCorporateAction.Add(action);

                    _context.Add(SubCorp);
                    _context.SaveChanges();
                    //_context.CorporateMaster.Add(CorpDetails);
            
                
               
                return Ok(new { status = true, Msg = "Updated Successfully!" });
            }
            catch (Exception ex)
            {
                _ErrLogService.Log(ex, HttpContext, JsonConvert.SerializeObject(action));
                return StatusCode(500, new { Msg = "Internal Server Error", MsgDetail = ex.Message });
            }

        }

        [Route("api/BackOffice/GetCorporateList")]
        public Object GetCorporateList(int page, int pagesize)
        {
            try
            {
                dynamic corpList = null;
                User LoginUser = _CacheManager.GetOrSetUserSession(Request.Query["key"], null);
                int TotalRecords = 0;

                    //corpList = _context.CorporateMaster.Include(x => x.lstUserCorporate).Include(x => x.lstUserCorporateAction)
                    //                     .Where(U => U.UpdateFlag == false
                    //                               && (U.lstUserCorporateAction.Any(x=>x.CurrentStatus==true && x.FK_CorporateStatusMasterId!=(int)(CorporateStatus.Submition)))
                    //                               && (U.lstUserCorporate.Any(x => x.fk_UserId == LoginUser.Id)
                    //                               && U.Balance != null && ((int)U.Balance) != 0))
                    //    .Select(x => new
                    //    {
                    //        x.Id,
                    //        x.Name,
                    //        x.Phone,
                    //        x.Location,
                    //        x.No_,
                    //        x.Email,
                    //        x.Balance
                    //    }).GetPaged(page, pagesize);
                    //TotalRecords = _context.Corporate.Include(x => x.lstUserCorporate).Where(U => (U.lstUserCorporate.Any(x => x.fk_UserId == LoginUser.Id) && U.Balance != null && ((int)U.Balance) != 0)).Count();
                
                return StatusCode(200, new { corpList.Results, TotalRecords });
            }
            catch (Exception ex)
            {
                _ErrLogService.Log(ex, HttpContext);
                return StatusCode(500, new { status = true, Msg = ex.Message });
            }

        }

    }
}
