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
    public class CorporateActionController : Controller
    {

        private readonly RBTCreditControl_Context _context = null;
        //private readonly Intranet_DBContext context_Intra = null;
        private readonly ICacheManager<User> _cache;
        private static IErrLogService _ErrLogService = null;


        public CorporateActionController(RBTCreditControl_Context context ,ICacheManager<User> cache, IErrLogService LogService)
        {
            _context = context;
            _cache = cache;
            _ErrLogService = LogService;
        }

        [HttpGet]
        [Route("api/CorporateAction/GetStatusByUser")]
        public object GetStatusByUser(int userid,int IsPromise)
        {
            try
            {
                int start = 0;int end = 1;
                if (IsPromise == 0)
                {
                    start = 1;
                    end = 7;
                }
                var resp1 = _context.UserCorporateAction.Include(x => x.SubmitedCorporate).ThenInclude(x=>x.CorporateMaster)
                                                         .Include(x => x.CorporateStatusMaster).Include(x => x.UserUpdatedBy).Where(x => x.UpdatedBy == userid && x.FK_CorporateStatusMasterId> start && x.FK_CorporateStatusMasterId <= end)
                     .Select(x => new
                     {
                         x.Id,
                         x.PromiseAmount,
                         x.PromiseDate,
                         x.UpdatedOn,
                         x.CallNote,

                         Corporate = x.SubmitedCorporate.CorporateMaster.Name,
                         UserUpdatedBy = x.UserUpdatedBy.Name,
                         Status = x.CorporateStatusMaster.Name
                     });
                return StatusCode(200, new { resp1 });
            }
            catch (Exception ex)
            {
                _ErrLogService.Log(ex, HttpContext);
                return StatusCode(500, new { status = true, Msg = ex.Message });
            }
        }

        [HttpGet]
        [Route("api/CorporateAction/GetStatusByCorpId")]
        public object GetStatusByCorpId(int corpId)
        {
            try
            {
                var users = _context.User;
                var resp1 = _context.SubmitedCorporate.Include(x => x.CorporateMaster).ThenInclude(x => x.LocationMaster)
                    .Include(x => x.lstUserCorporateAction).ThenInclude(x => x.CorporateStatusMaster)
                    .Where(x => x.Id == corpId)
                     .Select(x => new
                     {
                         x.CorporateMaster.Id,
                         x.CorporateMaster.Name,
                         //x.Balance,
                         //x.Blocked,
                       //  x.Amount,
                       
                         x.CorporateMaster.Phone,
                         //x.PostingGroup,
                         x.CorporateMaster.No_,
                       //  x.toDate,
                        // x.fromDate,
                         x.CorporateMaster.Location,
                         lstUserCorporateAction = x.lstUserCorporateAction.Select(m => new
                         {
                             m.Id,
                             Status = m.CorporateStatusMaster.Name,
                             m.PromiseAmount,
                             PromiseDate = (m.PromiseDate != null) ? ((DateTime)m.PromiseDate).ToString("dd MMM yyyy") : null,
                          
                             m.CallNote,
                             SbmtFromDate = (m.SbmtFromDate != null) ? ((DateTime)m.SbmtFromDate).ToString("dd MMM yyyy") : null,
                             SbmtToDate = (m.SbmtToDate != null) ? ((DateTime)m.SbmtToDate).ToString("dd MMM yyyy") : null,
                             m.SbmtAmount,
                           
                             UpdatedOn = (m.UpdatedOn != null) ? ((DateTime)m.UpdatedOn).ToString("dd MMM yyyy hh:mm") : null,
                             UpdatedByUser =  users.SingleOrDefault(h=>h.Id== m.UpdatedBy).Name,
                            
                         }).OrderByDescending(y=>y.UpdatedOn)

                     });    
                return StatusCode(200, new { resp1 });
            }
            catch (Exception ex)
            {
                _ErrLogService.Log(ex, HttpContext);
                return StatusCode(500, new { status = true, Msg = ex.Message });
            }
        }

        // POST: api/CorporateAction
        [HttpPost]
        [Route("api/CorporateAction/PostStatus")]
        public async Task<IActionResult> PostStatus([FromQuery] long Id,[FromBody] UserCorporateAction action)
        {
            try
            {
               
                if  (action == null || Id == 0 || action.FK_CorporateStatusMasterId == null)
                    return Ok(new { status = true, Msg = "Server Validation Failed.Bad Request!" });

                User UserLogin=_cache.GetOrSetUserSession(Request.Query["key"], null);
              
                if (action.FK_CorporateStatusMasterId==(int)CorporateStatus.Submition)
                {

                    CorporateMaster CorpDetails = _context.CorporateMaster
                                                 .SingleOrDefault(x => x.Id == Id);

                    if (CorpDetails.UpdateFlag == false)
                        return Ok(new { status = false, Msg = "Corporate Already Submited!" });

                    CorpDetails.UpdateFlag = false;
                    CorpDetails.OstSubmittedOn = DateTime.Now;
                    _context.SaveChanges();

                    SubmitedCorporate SubCorp = new SubmitedCorporate();
                    SubCorp.Balance = CorpDetails.Balance;
                    SubCorp.FK_CorporateMasterId = CorpDetails.Id;
                    SubCorp.lstUserCorporateAction = new List<UserCorporateAction>();
                    action.FK_SubmitedCorporateId = Id;
                    action.UpdatedBy = UserLogin.Id;
                    action.UpdatedOn = DateTime.Now;
                    SubCorp.lstUserCorporateAction.Add(action);
                    _context.Add(SubCorp);
                    _context.SaveChanges();
                }
                else
                {
                    var corpDetails = _context.SubmitedCorporate.Include(x => x.CorporateMaster)
                                              .Include(x => x.lstUserCorporateAction)
                                              .SingleOrDefault(x => x.Id == action.FK_SubmitedCorporateId);

                    if (corpDetails.lstUserCorporateAction.Count() == 2)
                    {
                        if (corpDetails.lstUserCorporateAction[1].UpdatedBy != UserLogin.Id)
                        {
                            // dont allow other User to change status. once status changed from submited to other status
                            return Ok(new { status = false, Msg = "Corporate Status Already Changed!" });
                        }
                    }

                    corpDetails.lstUserCorporateAction.Last().CurrentStatus = false;

                    //corpDetails.lstUserCorporateAction.Where(x => x.FK_SubmitedCorporateId == action.FK_SubmitedCorporateId && x.CurrentStatus == true).ToList().ForEach(x =>
                    //{
                    //    x.CurrentStatus = false;
                    //    // OldUniqueGroupId = x.UniqueGroupId;
                    //});

                    action.UpdatedBy = UserLogin.Id;
                    action.UpdatedOn = DateTime.Now;
                    action.CurrentStatus = true;
                    //action.FK_SubmitedCorporateId = Id;
                    corpDetails.lstUserCorporateAction.Add(action);
                    _context.SaveChanges();
                }
                return Ok(new { status = true, Msg = "Updated Successfully!" });
            }
            catch (Exception ex)
            {
                _ErrLogService.Log(ex, HttpContext, JsonConvert.SerializeObject(action));
                return StatusCode(500, new { Msg="Internal Server Error", MsgDetail = ex.Message });
            }
            
        }

    }
}
