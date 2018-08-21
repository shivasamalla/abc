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
    public class FollowUpController : Controller
    {
        private readonly RBTCreditControl_Context _context = null;
        //private readonly Intranet_DBContext context_Intra = null;
        private readonly ICacheManager<User> _cache;
        private static IErrLogService _ErrLogService = null;


        public FollowUpController(RBTCreditControl_Context context, ICacheManager<User> cache, IErrLogService LogService)
        {
            _context = context;
            _cache = cache;
            _ErrLogService = LogService;
        }

        [HttpGet]
        [Route("api/FollowUp/GetFollowupFlag")]
        public object GetFollowupFlag()
        {

            try
            {
                int userId = _cache.GetOrSetUserSession(Request.Query["key"], null).Id;
                var cnt = _context.UserCorporateAction.Include(x => x.SubmitedCorporate)
                            .Include(x => x.CorporateStatusMaster)
                            .Include(x => x.UserUpdatedBy)
                            .Where(x => x.UpdatedBy == userId && x.CurrentStatus == true && Convert.ToDateTime(x.PromiseDate).Date == DateTime.Now.Date && x.FK_CorporateStatusMasterId == (int)CorporateStatus.Promise).Count();
                return StatusCode(200, new { status = (cnt > 0) ? true : false });
            }
            catch (Exception ex)
            {
                _ErrLogService.Log(ex, HttpContext);
                return StatusCode(500, new { status = false, Msg = ex.Message });
            }
        }
        [HttpGet]
        [Route("api/FollowUp/GetFollowupList_ByUserId")]
        public object GetFollowupList_ByUserId(int page, int pagesize)
        {
            try
            {
               
                int userId = _cache.GetOrSetUserSession(Request.Query["key"],null).Id;
                var resp1 = _context.UserCorporateAction.Include(x => x.SubmitedCorporate).ThenInclude(x=>x.CorporateMaster)
                            .Include(x => x.CorporateStatusMaster)
                            .Include(x => x.UserUpdatedBy)
                            .Where(x => x.UpdatedBy == userId && x.CurrentStatus == true && Convert.ToDateTime(x.PromiseDate).Date == DateTime.Now.Date && x.FK_CorporateStatusMasterId == 1)
                     .Select(x => new
                     {
                         x.Id,
                         x.PromiseAmount,
                         PromiseDate =(x.PromiseDate != null) ? ((DateTime)x.PromiseDate).ToString("dd MMM yyyy") : null, 
                         UpdatedOn = (x.UpdatedOn!=null) ? ((DateTime)x.UpdatedOn).ToString("dd MMM yyyy hh:mm:ss") : null,
                         x.CallNote,
                         x.FK_SubmitedCorporateId,
                         
                         CorporateName = x.SubmitedCorporate.CorporateMaster.Name,
                         CorporateLocation = x.SubmitedCorporate.CorporateMaster.Location,
                         CorporateIcust = x.SubmitedCorporate.CorporateMaster.No_,
                         CorporatePhone = x.SubmitedCorporate.CorporateMaster.Phone,
                         CorporateEmail = x.SubmitedCorporate.CorporateMaster.Email,

                         UserUpdatedBy = x.UserUpdatedBy.Name,
                         Status = x.CorporateStatusMaster.Name,
                         x.SubmitedCorporate.CorporateMaster.Balance
                     }).GetPaged(page,pagesize);
                int TotalRecords = resp1.RowCount;
                resp1.Results = resp1.Results.OrderByDescending(x => x.UpdatedOn).ToList();
                //_context.UserCorporateAction.Include(x => x.Corporate)
                //            .Include(x => x.CorporateStatusMaster)
                //            .Include(x => x.UserUpdatedBy)
                //            .Where(x => x.UpdatedBy == userId && x.CurrentStatus == true && Convert.ToDateTime(x.PromiseDate).Date == DateTime.Now.Date && x.FK_CorporateStatusMasterId == 1).Count();
                return StatusCode(200, new { resp1 , TotalRecords });
            }
            catch (Exception ex)
            {
                _ErrLogService.Log(ex, HttpContext);
                return StatusCode(500, new { status = false, Msg = ex.Message });
            }
        }
        //[HttpGet]
        //[Route("api/CorporateAction/GetFollowupList")]
        //public object GetFollowupList()
        //{
        //    try
        //    {

        //        var resp1 = _context.UserCorporateAction.Include(x => x.Corporate).Include(x => x.CorporateStatusMaster).Include(x => x.UserUpdatedBy).Where(x => Convert.ToDateTime(x.PromiseDate).Date == DateTime.Now.Date && x.FK_CorporateStatusMasterId == 1)
        //             .Select(x => new
        //             {
        //                 x.Id,
        //                 x.PromiseAmount,
        //                 x.PromiseDate,
        //                 x.UpdatedOn,
        //                 x.CallNote,

        //                 Corporate = x.Corporate.Name,
        //                 UserUpdatedBy = x.UserUpdatedBy.Name,
        //                 Status = x.CorporateStatusMaster.Name
        //             });
        //        return StatusCode(200, new { resp1 });
        //    }
        //    catch (Exception ex)
        //    {
        //        _ErrLogService.Log(ex, HttpContext);
        //        return StatusCode(500, new { status = true, Msg = ex.Message });
        //    }
        //}
    }
}