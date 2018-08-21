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
   
    public class LocationMasterController : Controller
    {
        private readonly RBTCreditControl_Context _context;
        private static IErrLogService _ErrLogService = null;
        public LocationMasterController(RBTCreditControl_Context context, IErrLogService LogService)
        {
            _context = context;
            _ErrLogService = LogService;
        }

        // GET: api/LocationMaster
        [HttpGet]
        [Route("api/LocationMaster/GetALL")]
        public Object GetLocationMaster()
        {

            try
            {
                return _context.LocationMaster.Select(x => new { x.Id, x.BranchLocation, x.Branch });
            }
            catch (Exception ex)
            {
                _ErrLogService.Log(ex, HttpContext);
                return StatusCode(500, new { status = true, Msg = ex.Message });
            }
        }

        [HttpGet]
        [Route("api/LocationMaster/GetALL_P")]
        public Object GetLocationMaster(int page, int pagesize)
        {
            try
            {
                var resp = _context.LocationMaster.GetPaged(page, pagesize);
                return new { TotalRecords = resp.RowCount, resp = resp.Results.OrderBy(x => x.Branch) };
            }
            catch (Exception ex)
            {
                _ErrLogService.Log(ex, HttpContext);
                return StatusCode(500, new { status = true, Msg = ex.Message });
            }
            // return resp;
        }

        // GET: api/LocationMaster/5
        [HttpGet]
        [Route("api/LocationMaster/GetByUserId")]
        public async Task<IActionResult> GetLocationByUserId([FromQuery] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
               
                var locationMaster = _context.UserLocation.Include(x=>x.Location).Where(x => x.FK_UserId == id)
                                     .Select(x=> new {x.Location.Id,x.Location.Branch,x.Location.BranchLocation });

                if (locationMaster == null)
                {
                    return NotFound();
                }

               return StatusCode(200, new { status = true, locationMaster });
            }
            catch (Exception ex)
            {
                _ErrLogService.Log(ex, HttpContext);
                return StatusCode(500, new { status = true, Msg = ex.Message });
            }
        }

        // GET: api/LocationMaster/5
        [HttpGet]
        [Route("api/LocationMaster/GetById")]
        public async Task<IActionResult> GetLocationMaster([FromQuery] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var locationMaster = await _context.LocationMaster.SingleOrDefaultAsync(m => m.Id == id);

                if (locationMaster == null)
                {
                    return NotFound();
                }

                return Ok(locationMaster);
            }
            catch (Exception ex)
            {
                _ErrLogService.Log(ex, HttpContext);
                return StatusCode(500, new { status = true, Msg = ex.Message });
            }
        }

        // PUT: api/LocationMaster/5
        [HttpPut]
        [Route("api/LocationMaster/UpdateLocationMaster")]
        public async Task<IActionResult> PutLocationMaster([FromQuery] int id, [FromBody] LocationMaster locationMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != locationMaster.Id)
            {
                return BadRequest();
            }
            locationMaster.ModifiedOn = DateTime.Now;
            _context.Entry(locationMaster).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return StatusCode(200, new { Msg = "Inserted!" });
            }
            catch (Exception ex)
            {
                _ErrLogService.Log(ex, HttpContext);
                return StatusCode(500, new { status = true, Msg = ex.Message });
            }

        }

        // POST: api/LocationMaster
        [HttpPost]
        [Route("api/LocationMaster/PostLocationMaster")]
        public async Task<IActionResult> PostLocationMaster([FromBody] LocationMaster locationMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                bool exists = _context.LocationMaster.Any(x => x.Name.ToUpper() == locationMaster.Name.ToUpper());
                if (!exists)
                {
                    _context.LocationMaster.Add(locationMaster);
                    await _context.SaveChangesAsync();
                    return Ok(new { status = true, Msg = "Inserted" });
                }
                else
                {
                    return Ok(new { status = false, Msg = "Already Exists!" });
                }
            }
            catch (Exception ex)
            {
                _ErrLogService.Log(ex, HttpContext);
                return StatusCode(500, new { status = true, Msg = ex.Message });
            }

        }

        // DELETE: api/LocationMaster/5
        [HttpDelete]
        [Route("api/LocationMaster/DeleteLocationMaster")]
        public async Task<IActionResult> DeleteLocationMaster([FromQuery] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var locationMaster = await _context.LocationMaster.SingleOrDefaultAsync(m => m.Id == id);
                if (locationMaster == null)
                {
                    return NotFound();
                }

                _context.LocationMaster.Remove(locationMaster);
                await _context.SaveChangesAsync();

                return Ok(locationMaster);
            }
            catch (Exception ex)
            {
                _ErrLogService.Log(ex, HttpContext);
                return StatusCode(500, new { status = true, Msg = ex.Message });
            }
        }

        private bool LocationMasterExists(int id)
        {
            return _context.LocationMaster.Any(e => e.Id == id);
        }
    }
}