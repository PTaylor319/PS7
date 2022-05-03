using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWARM.EF.Data;
using SWARM.EF.Models;
using SWARM.Server.Controllers.Base;
using SWARM.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWARM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZipcodeController : BaseController<Zipcode>
    {
        public ZipcodeController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        [HttpDelete]
        [Route("DeleteZipcode/{pZip}")]
        public async Task<IActionResult> Delete(string pZip)
        {
            Zipcode itm = await _context.Zipcodes.Where(x => x.Zip == pZip).FirstOrDefaultAsync();
            _context.Remove(itm);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("GetZipcodes")]
        public async Task<IActionResult> Get()
        {
            List<Zipcode> lst = await _context.Zipcodes.OrderBy(x => x.Zip).ToListAsync();
            return Ok(lst);
        }

        [HttpGet]
        [Route("GetZipcodes/{pZipcodeId}")]
        public async Task<IActionResult> Get(string pZip)
        {
            Zipcode itm = await _context.Zipcodes.Where(x => x.Zip == pZip).FirstOrDefaultAsync();
            return Ok(itm);
        }

        [HttpPost]
        [Route("GetZipcodes")]
        public async Task<IActionResult> Post([FromBody] ZipcodeDTO _ZipcodeDTO)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existZipcode = await _context.Zipcodes.Where(x => x.Zip == _ZipcodeDTO.Zip).FirstOrDefaultAsync();

                if (existZipcode == null)
                {
                    existZipcode = new Zipcode();
                }

                existZipcode.Zip = _ZipcodeDTO.Zip;
                existZipcode.City = _ZipcodeDTO.City;
                existZipcode.State = _ZipcodeDTO.State;
                var updatedCourse = _context.Update(existZipcode);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(updatedCourse.Entity.Zip);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("GetZipcodes")]
        public async Task<IActionResult> Put([FromBody] ZipcodeDTO _ZipcodeDTO)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existZipcode = await _context.Zipcodes.Where(x => x.Zip == _ZipcodeDTO.Zip).FirstAsync();

                existZipcode.Zip = _ZipcodeDTO.Zip;
                existZipcode.City = _ZipcodeDTO.City;
                existZipcode.State = _ZipcodeDTO.State;
                var updatedCourse = _context.Update(existZipcode);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(updatedCourse.Entity.Zip);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
