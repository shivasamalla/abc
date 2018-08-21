using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RBTCreditControl.Entity;
using RBTCreditControl.WebApp.Models;
using RBTCreditControl.Repository;

namespace RBTCreditControl.WebApp.Controllers
{

    [Produces("application/json")]
    public class CorporateController : Controller
    {
        private readonly RBTCreditControl_Context _context;
        private readonly ICacheManager<User> _cache;
        private static IErrLogService _ErrLogService = null;
        public CorporateController(RBTCreditControl_Context context, ICacheManager<User> cache, IErrLogService LogService)
        {
            _context = context;
            _cache = cache;
            _ErrLogService = LogService;
        }

        // GET: api/Corporate
        [HttpGet]
        [Route("api/Corporate/GetCorporate")]
        public Object GetCorporate(int page, int pagesize,int userId)
        {
            try
            {
                var userLocations = _context.UserLocation.Where(x => x.FK_UserId == userId).Select(x => x.FK_LocationId);
                var resp1 = _context.CorporateMaster.Where(x => userLocations.Contains(x.fk_LocationId))
                     .Select(x => new
                     {
                         x.Id,
                         x.Name,
                         x.Phone,
                         x.Location,
                         x.No_,
                         x.Email

                     }).GetPaged(page, pagesize);


                int TotalRecord = resp1.RowCount;
                return StatusCode(200, new { Results=resp1.Results.OrderBy(x=>x.Name), TotalRecord });
            }
            catch (Exception ex)
            {
                _ErrLogService.Log(ex, HttpContext);
                return StatusCode(500, new { status = true, Msg = ex.Message });
            }

        }

        [HttpGet]
        [Route("api/Corporate/GetAssignedCorporate_ByUserId")]
        public object GetAssignedCorporate_ByUserId(int userId)
        {
            // get Assigned Corporate list for User based on userId
            try
            {
                var corpList = _context.CorporateMaster.Include(x => x.lstUserCorporate).Where(U => U.lstUserCorporate.Any(x => x.fk_UserId == userId))
                     .Select(x => new
                     {
                         x.Id,
                         x.Name,
                         x.Location,
                         x.No_,
                     });

                return corpList.OrderBy(x=>x.Name);
            }
            catch (Exception ex)
            {
                _ErrLogService.Log(ex, HttpContext);
                return StatusCode(500, new { status = true, Msg = ex.Message });
            }
        }

        [HttpGet]
        [Route("api/Corporate/GetUserAssignedCorporates")]
        public object GetUserAssignedCorporates(int page, int pagesize, int userId)
        {
            // Get Assigned Corprate For User
            try
            {
               
                User user = _cache.GetOrSetUserSession(Request.Query["key"], null);
                int TotalRecords = 0;
                // bool userFlag = (user.UserType.ToLower().Trim() == "backoffice") ? false : true;
                if (user.UserType.ToLower().Trim() == "backoffice")
                {
                  var  corpList = _context.CorporateMaster.Include(x => x.lstUserCorporate).Where(U =>
                                       (U.lstUserCorporate.Any(x => x.fk_UserId == userId)
                                       && U.UpdateFlag == true && U.Balance != null && ((int)U.Balance) != 0))
                      .Select(x => new
                      {
                          x.Id,
                          x.Name,
                          x.Phone,
                          x.Location,
                          x.No_,
                          x.Email,
                          x.Balance,
                          x.Priority
                      }).GetPaged(page, pagesize);
                    corpList.Results = corpList.Results.OrderByDescending(y => y.Priority).ToList();
                    return StatusCode(200, new { corpList.Results, TotalRecords = corpList.RowCount });
                }
                if (user.UserType.ToLower().Trim() == "calling")
                {
                    
                  var  corpList = _context.SubmitedCorporate.Include(x=>x.CorporateMaster).ThenInclude(x => x.lstUserCorporate)
                                                         .Include(x=>x.lstUserCorporateAction)
                                                         .Where(U => (U.CorporateMaster.lstUserCorporate.Any(x => x.fk_UserId == userId)
                                        && (U.lstUserCorporateAction.Any(x=>x.CurrentStatus==true && x.FK_CorporateStatusMasterId==(int)CorporateStatus.Submition))
                                       ))
                        .Select(x => new
                    {
                        x.Id,
                        x.FK_CorporateMasterId,
                        x.CorporateMaster.Name,
                        x.CorporateMaster.Phone,
                        x.CorporateMaster.Location,
                        x.CorporateMaster.No_,
                        x.CorporateMaster.Email,
                        x.Balance,
                        UpdatedOn=x.lstUserCorporateAction.SingleOrDefault(r=>r.CurrentStatus==true).UpdatedOn
                    }).GetPaged(page, pagesize);
                    corpList.Results = corpList.Results.OrderBy(x => x.UpdatedOn).ToList();
                    return StatusCode(200, new { corpList.Results, TotalRecords = corpList.RowCount });
                }
                else if (user.UserType.ToLower().Trim() == "supervisor")
                {
                  var  corpList = _context.CorporateMaster.Where(U => user.lstUserLocation.Any(o => o.FK_LocationId == U.fk_LocationId) && U.Balance != null && ((int)U.Balance) != 0)
                   .Select(x => new
                   {
                       x.Id,
                       x.Name,
                       x.Phone,
                       x.Location,
                       x.No_,
                       x.Email,
                       x.Balance,
                       x.Priority
                   }).GetPaged(page, pagesize);
                    corpList.Results = corpList.Results.OrderBy(x => x.Name).ToList();
                    return StatusCode(200, new { corpList.Results, TotalRecords = corpList.RowCount });
                }
                return StatusCode(200, new { Status=false, Msg="Invalid Request" });
            }
            catch (Exception ex)
            {
                _ErrLogService.Log(ex, HttpContext);
                return StatusCode(500, new { status = true, Msg = ex.Message });
            }

        }

        [HttpGet]
        [Route("api/Corporate/GetCorporateByUser")]
        public Object GetCorporateByUser(int userId)
        {
            try
            {
                // Get Corporate List Based on Suppervisor Location
                var userLocations = _context.UserLocation.Where(x => x.FK_UserId == userId).Select(x => x.FK_LocationId);
                var resp1 = _context.LocationMaster.Include(x => x.lstCorporateMaster).Where(x => userLocations.Contains(x.Id))
                     .Select(x => new
                     {
                         x.Id,
                         x.Branch,
                         x.BranchLocation,
                         x.lstCorporateMaster,
                         x.CityName
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
        [Route("api/Corporate/GetCorporateByPkId")]
        public Object GetCorporateByPkId(int corpId)
        {
            try
            {
                // Get Corporate List Based on Suppervisor Location
                // var userLocations = _context.UserLocation.Where(x => x.FK_UserId == userId).Select(x => x.FK_LocationId);
                var resp1 = _context.SubmitedCorporate.Include(x=>x.CorporateMaster).ThenInclude(x=>x.LocationMaster)
                           .Where(x => x.Id == corpId)
                     .Select(x => new
                     {
                         x.Id,
                         x.CorporateMaster.Balance,
                         x.CorporateMaster.CustomerGroup,
                         x.CorporateMaster.CustomerType,
                         x.CorporateMaster.Email,
                         x.CorporateMaster.Phone,
                         x.CorporateMaster.PostingGroup,
                         x.CorporateMaster.No_,
                         LocationMaster= x.CorporateMaster.LocationMaster.BranchLocation
                     });
            
                return StatusCode(200, new { resp1 = resp1.OrderByDescending(x=>x.Id) });
            }
            catch (Exception ex)
            {
                _ErrLogService.Log(ex, HttpContext);
                return StatusCode(500, new { status = true, Msg = ex.Message });
            }
        }

        // GET: api/Corporate/5
        [Route("api/ERP/GetCorporate")]
        public Object GetCorporate(string Icust, string Location)
        {
            try
            {
                if (string.IsNullOrEmpty(Icust) || string.IsNullOrEmpty(Location))
                {
                    return BadRequest(ModelState);
                }

                List<CorporateMaster> lstCorp = new ERPService().getData(Location, Icust);
                if (lstCorp != null)
                {
                    return Ok(lstCorp.Select(x =>
                                        new
                                        {
                                            Phone = x.Phone.Replace("\\", "/"),
                                            Name = x.Name,
                                            Location = x.Location,
                                            Email = x.Email,
                                            No_ = x.No_,
                                            
                                        }));
                }
                else
                {
                    return StatusCode(500, new { Msg = "Result Not Found!" });
                }
            }
            catch (Exception ex)
            {
                _ErrLogService.Log(ex, HttpContext);
                return StatusCode(500, new { status = true, Msg = ex.Message });
            }

        }

        // PUT: api/Corporate/5
        [HttpPut]
        [Route("api/Corporate/UpdateCorporate")]
        public async Task<IActionResult> PutCorporate([FromRoute] int id, [FromBody] CorporateMaster Corporate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Corporate.Id)
            {
                return BadRequest();
            }

            try
            {
                _context.Entry(Corporate).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return StatusCode(200, new { status = true, Msg = "UPDATED" });  
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("Cannot insert duplicate key"))
                {
                    return Ok(new { status = true, Msg = "Already Exists!" });
                }
                _ErrLogService.Log(ex, HttpContext);
                return StatusCode(500, new { status = true, Msg = ex.Message });
            }
        }

       
        // POST: api/Corporate
        [HttpPost]
        [Route("api/Corporate/InsertCorporate")]
        public async Task<IActionResult> PostCorporate([FromBody]CorporateMaster Corporate)
        {
            try
            {
                //string key = Request.Query["key"];
                //User user = _cache.GetOrSetUserSession(key, null);

                //Corporate.CreatedBy =(int)user.Id;

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                bool exists = _context.CorporateMaster.Any(x => x.No_ == Corporate.No_);
                if (!exists)
                {
                    _context.CorporateMaster.Add(Corporate);
                    await _context.SaveChangesAsync();
                    return Ok(new { status = true, Msg = "Inserted successfully!" });
                }
                else
                {
                    return StatusCode(500,new { status = false, Msg = "Corporate Already Exists!" });
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("Cannot insert duplicate key"))
                {
                    return StatusCode(500,new { status = true, Msg = "Corporate Already Exists!" });
                }
                _ErrLogService.Log(ex, HttpContext);
                return StatusCode(500, new { status = true, Msg = ex.Message });
            }

        }

       
  
        // DELETE: api/Corporate/5
        [HttpDelete]
        [Route("api/Corporate/DeleteCorporate")]
        public async Task<IActionResult> DeleteCorporate([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var Corporate = await _context.CorporateMaster.SingleOrDefaultAsync(m => m.Id == id);
                if (Corporate == null)
                {
                    return NotFound();
                }

                _context.CorporateMaster.Remove(Corporate);
                await _context.SaveChangesAsync();
                return Ok(Corporate);
            }
            catch (Exception ex)
            {
                _ErrLogService.Log(ex, HttpContext);
                return StatusCode(500, new { status = true, Msg = ex.Message });
            }

            
        }

        private bool CorporateExists(int id)
        {
            return _context.CorporateMaster.Any(e => e.Id == id);
        }
    }

  
 
}