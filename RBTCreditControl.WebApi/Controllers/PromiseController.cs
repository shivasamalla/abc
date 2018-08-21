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
    public class PromiseController : Controller
    {
        private readonly RBTCreditControl_Context _context = null;
        //private readonly Intranet_DBContext context_Intra = null;
        private readonly ICacheManager<User> _cache;
        private static IErrLogService _ErrLogService = null;


        public PromiseController(RBTCreditControl_Context context, ICacheManager<User> cache, IErrLogService LogService)
        {
            _context = context;
            _cache = cache;
            _ErrLogService = LogService;
        }

        [HttpGet]
        [Route("api/Promise/GetPromisedCorporate")]
        public Object GetPromisedCorporate(int page, int pagesize,bool IsPromise)
        {
            //int dop = (int)CorporateStatus.Promise;
            try
            {
                int userId = _cache.GetOrSetUserSession(Request.Query["key"], null).Id;
                
                int TotalRecords = 0;
                if (IsPromise)
                {
                  var  statusList = _context.UserCorporateAction.Include(x => x.SubmitedCorporate).ThenInclude(x=>x.CorporateMaster)
                                   .Include(x => x.CorporateStatusMaster)
                                   .Include(x => x.UserUpdatedBy)
                                   .Where(x => x.UpdatedBy == userId && x.CurrentStatus == true && x.FK_CorporateStatusMasterId == (int)CorporateStatus.Promise)
                        .Select(x => new
                        {
                            x.Id,
                            x.PromiseAmount,
                            PromiseDate = (x.PromiseDate != null) ? ((DateTime)x.PromiseDate).ToString("dd MMM yyyy") : null,
                            UpdatedOn = (x.UpdatedOn != null) ? ((DateTime)x.UpdatedOn).ToString("dd MMM yyyy hh:mm:ss") : null,
                            x.CallNote,
                            x.SubmitedCorporate.Balance,
                            x.FK_SubmitedCorporateId,
                            CorporateName = x.SubmitedCorporate.CorporateMaster.Name,
                            CorporateLocation = x.SubmitedCorporate.CorporateMaster.Location,
                            CorporateIcust = x.SubmitedCorporate.CorporateMaster.No_,
                            CorporatePhone = x.SubmitedCorporate.CorporateMaster.Phone,
                            CorporateEmail = x.SubmitedCorporate.CorporateMaster.Email,
                            UserUpdatedBy = x.UserUpdatedBy.Name,
                            Status = x.CorporateStatusMaster.Name,
                        }).GetPaged(page, pagesize);

                    TotalRecords = statusList.RowCount;
                    statusList.Results = statusList.Results.OrderByDescending(x => x.UpdatedOn).ToList();
                    return StatusCode(200, new { statusList.Results, TotalRecords });
                }
                else
                {
                  var  statusList = _context.UserCorporateAction.Include(x => x.SubmitedCorporate).ThenInclude(x => x.CorporateMaster)
                                 .Include(x => x.CorporateStatusMaster)
                                 .Include(x => x.UserUpdatedBy)
                                 .Where(x => x.UpdatedBy == userId && x.CurrentStatus == true && x.FK_CorporateStatusMasterId != (int)CorporateStatus.Promise)
                      .Select(x => new
                      {
                          x.Id,
                         // x.PromiseAmount,
                         // PromiseDate = (x.PromiseDate != null) ? ((DateTime)x.PromiseDate).ToString("dd MMM yyyy") : null,
                          UpdatedOn = (x.UpdatedOn != null) ? ((DateTime)x.UpdatedOn).ToString("dd MMM yyyy hh:mm:ss") : null,
                          //x.CallNote,
                          //x.Corporate.Balance,
                          x.FK_SubmitedCorporateId,
                          CorporateName = x.SubmitedCorporate.CorporateMaster.Name,
                          CorporateLocation = x.SubmitedCorporate.CorporateMaster.Location,
                          CorporateIcust = x.SubmitedCorporate.CorporateMaster.No_,
                          //CorporatePhone = x.Corporate.Phone,
                          CorporateEmail = x.SubmitedCorporate.CorporateMaster.Email,
                         // UserUpdatedBy = x.UserUpdatedBy.Name,
                          Status = x.CorporateStatusMaster.Name
                      }).GetPaged(page, pagesize);

                    TotalRecords = statusList.RowCount;
                    statusList.Results = statusList.Results.OrderByDescending(x => x.UpdatedOn).ToList();
                    return StatusCode(200, new { statusList.Results, TotalRecords });
                }

            }
            catch (Exception ex)
            {
                _ErrLogService.Log(ex, HttpContext);
                return StatusCode(500, new { status = false, Msg = ex.Message });
            }
        }

        
    }
}