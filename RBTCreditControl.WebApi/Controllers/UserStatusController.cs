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
  
    public class UserStatusController : Controller
    {
        private readonly RBTCreditControl_Context _context = null;
        //private readonly Intranet_DBContext context_Intra = null;
        private readonly ICacheManager<User> _cache;
        private static IErrLogService _ErrLogService = null;


        public UserStatusController(RBTCreditControl_Context context, ICacheManager<User> cache, IErrLogService LogService)
        {
            _context = context;
            _cache = cache;
            _ErrLogService = LogService;
        }
        [HttpGet]
        [Route("api/UserStatus/GetStatusByAllUser")]
        public object GetStatusByAllUser(int supervisorId)
        {
            try
            {
                var corpLocation = _context.CorporateMaster.Include(x => x.LocationMaster).Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.No_,


                    Location = x.LocationMaster.BranchLocation,
                    LocationId= x.LocationMaster.Id

                }); 

                var resp1 = _context.User
                       .Include(x => x.lstUserCorporateAction)
                       .ThenInclude(x => x.CorporateStatusMaster).Where(x => x.fk_SupervisorId == supervisorId).
                    Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.EmpCode,
                        x.Email,
                        lstUserCorporateAction = x.lstUserCorporateAction.Select(m => new
                        {
                            m.Id,
                            m.PromiseAmount,
                            m.PromiseDate,
                            m.UpdatedOn,
                            m.CallNote,
                            LocationName = corpLocation.SingleOrDefault(h => h.Id == m.FK_SubmitedCorporateId).Location,
                            corporateName = corpLocation.SingleOrDefault(h => h.Id == m.FK_SubmitedCorporateId).Name,
                            Status = m.CorporateStatusMaster.Name
                        })
                    });

                  
                   
                return StatusCode(200, new { resp1 });
            }
            catch (Exception ex)
            {
                _ErrLogService.Log(ex, HttpContext);
                return StatusCode(500, new { status = true, Msg = ex.Message });
            }
        }

        //// GET: api/UserStatus/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}
        
        //// POST: api/UserStatus
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}
        
        //// PUT: api/UserStatus/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}
        
        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
