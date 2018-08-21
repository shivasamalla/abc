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
    public class ReportController : Controller
    {

        private readonly RBTCreditControl_Context _context = null;
        //private readonly Intranet_DBContext context_Intra = null;
        private readonly ICacheManager<User> _cache;
        private static IErrLogService _ErrLogService = null;


        public ReportController(RBTCreditControl_Context context, ICacheManager<User> cache, IErrLogService LogService)
        {
            _context = context;
            _cache = cache;
            _ErrLogService = LogService;
        }

        [HttpPost]
        [Route("api/Report/UserCorprateReport")]
        public object GetUserCorporate([FromBody]UserReportSearchPara Para)
        {
            try
            {
                if (Para == null)
                    return StatusCode(400, new { status = false, Msg = "Bad Request" });

                var UserLogin = _cache.GetOrSetUserSession(Request.Query["key"], null);

                if(UserLogin.UserType.Trim().ToLower()=="supervisor")
                {
                    var userLocation = _context.UserLocation.Include(x=>x.Location).Where(x => x.FK_UserId == UserLogin.Id);
                    var Resp = _context.UserCorporateAction
                                 .Include(x => x.SubmitedCorporate).ThenInclude(y => y.CorporateMaster)
                                 .Include(x => x.CorporateStatusMaster)
                                .Where(x => x.CurrentStatus == true
                                        && (Para.userId==0 || x.UpdatedBy == Para.userId)
                                        && (x.UpdatedOn.Value.Date >= Para.dtFromDate.Date && x.UpdatedOn.Value.Date <= Para.dtToDate.Date)
                                        && (Para.ICUST == null || x.SubmitedCorporate.CorporateMaster.No_ == Para.ICUST)
                                        && (Para.statusId == 0 || x.FK_CorporateStatusMasterId == Para.statusId)
                                        && userLocation.Any(p=>p.Location.Branch == x.SubmitedCorporate.CorporateMaster.Location)
                                      )
                                .Select(x => new
                                {
                                    x.FK_SubmitedCorporateId,
                                    x.SubmitedCorporate.CorporateMaster.Name,
                                    x.SubmitedCorporate.CorporateMaster.No_,
                                    x.SubmitedCorporate.CorporateMaster.Phone,
                                    //x.SbmtAmount,
                                    //x.PromiseAmount,
                                    Amount = ((x.FK_CorporateStatusMasterId) == (int)CorporateStatus.Submition) ? x.SbmtAmount :  x.PromiseAmount,
                                    Status = x.CorporateStatusMaster.Name,
                                    x.SubmitedCorporate.CorporateMaster.Location,
                                    x.UpdatedOn
                                }).GetPaged(Para.Page, Para.PageSize);

                    Resp.Results = Resp.Results.OrderByDescending(x=>x.UpdatedOn).ToList();

                    return StatusCode(200, new { status = true, Resp });
                }
                else if(UserLogin.UserType.Trim().ToLower() == "calling")
                {
                    var Resp = _context.UserCorporateAction
                                .Include(x => x.SubmitedCorporate).ThenInclude(y => y.CorporateMaster)
                                .Include(x => x.CorporateStatusMaster)
                               .Where(x => x.CurrentStatus == true && x.UpdatedBy == UserLogin.Id
                                       && (x.UpdatedOn.Value.Date >= Para.dtFromDate.Date && x.UpdatedOn.Value.Date <= Para.dtToDate.Date)
                                       && (Para.ICUST == null || x.SubmitedCorporate.CorporateMaster.No_ == Para.ICUST)
                                       && (Para.statusId == 0 || x.FK_CorporateStatusMasterId == Para.statusId)
                                     )
                               .Select(x => new
                               {
                                   x.FK_SubmitedCorporateId,
                                   x.SubmitedCorporate.CorporateMaster.Name,
                                   x.SubmitedCorporate.CorporateMaster.No_,
                                   x.SubmitedCorporate.CorporateMaster.Phone,
                                   //x.SbmtAmount,
                                   //x.PromiseAmount,
                                   Amount=((x.FK_CorporateStatusMasterId)==(int)CorporateStatus.Submition) ? x.SbmtAmount : x.PromiseAmount,
                                   Status = x.CorporateStatusMaster.Name,
                                   x.SubmitedCorporate.CorporateMaster.Location,
                                   x.UpdatedOn
                               }).GetPaged(Para.Page, Para.PageSize);

                    Resp.Results = Resp.Results.OrderByDescending(x => x.UpdatedOn).ToList();

                    return StatusCode(200, new { status = true, Resp });
                }
                else if(UserLogin.UserType.Trim().ToLower() == "backoffice")
                {
                    var Resp = _context.UserCorporateAction
                                .Include(x => x.SubmitedCorporate).ThenInclude(y => y.CorporateMaster)
                                .Include(x => x.CorporateStatusMaster)
                               .Where(x => x.UpdatedBy == UserLogin.Id
                                       &&  (x.FK_CorporateStatusMasterId == (int)(CorporateStatus.Submition))
                                       && (x.UpdatedOn.Value.Date >= Para.dtFromDate.Date && x.UpdatedOn.Value.Date <= Para.dtToDate.Date)
                                       && (Para.ICUST == null || x.SubmitedCorporate.CorporateMaster.No_ == Para.ICUST)
                                     )
                               .Select(x => new
                               {
                                   x.FK_SubmitedCorporateId,
                                   x.SubmitedCorporate.CorporateMaster.Name,
                                   x.SubmitedCorporate.CorporateMaster.No_,
                                   x.SubmitedCorporate.CorporateMaster.Phone,
                                   //x.SbmtAmount,
                                   //x.PromiseAmount,
                                   Amount = ((x.FK_CorporateStatusMasterId) == (int)CorporateStatus.Submition) ? x.SbmtAmount : x.PromiseAmount,
                                   Status = x.CorporateStatusMaster.Name,
                                   x.SubmitedCorporate.CorporateMaster.Location,
                                   x.UpdatedOn
                               }).GetPaged(Para.Page, Para.PageSize);

                    Resp.Results = Resp.Results.OrderByDescending(x => x.UpdatedOn).ToList();

                    return StatusCode(200, new { status = true, Resp });
                }

                return StatusCode(500, new { status = false, Msg="Invalid User Type" });
            }
            catch (Exception ex)
            {
                _ErrLogService.Log(ex, HttpContext);
                return StatusCode(500, new { status = false,  Msg="Internat Server Error!", MsgDetails = ex.Message });
            }
        }


    }
}