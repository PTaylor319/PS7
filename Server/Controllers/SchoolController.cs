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
    public class SchoolController : BaseController<School>, IBaseController<SchoolDTO>
    {
        public SchoolController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        [HttpDelete]
        [Route("DeleteSchools/{pSchoolId}")]
        public async Task<IActionResult> Delete(int pSchoolId)
        {
            School itm = await _context.Schools.Where(x => x.SchoolId == pSchoolId).FirstOrDefaultAsync();
            _context.Remove(itm);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("GetSchools")]
        public async Task<IActionResult> Get()
        {
            List<School> lst = await _context.Schools.OrderBy(x => x.SchoolId).ToListAsync();
            return Ok(lst);
        }

        [HttpGet]
        [Route("GetSchools)/{pSchoolId}")]
        public async Task<IActionResult> Get(int pSchoolId)
        {
            List<School> lst = await _context.Schools.OrderBy(x => x.SchoolId == pSchoolId).ToListAsync();
            return Ok(lst);
        }

        [HttpPost]
        [Route("GetSchools")]
        public async Task<IActionResult> Post([FromBody] SchoolDTO _SchoolDTO)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existSchool = await _context.Schools.Where(x => x.SchoolId == _SchoolDTO.SchoolId).FirstOrDefaultAsync();

                if (existSchool == null)
                {
                    existSchool = new School();
                }

                existSchool.SchoolId = _SchoolDTO.SchoolId;
                existSchool.SchoolName = _SchoolDTO.SchoolName;
                var updatedCourse = _context.Update(existSchool);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(updatedCourse.Entity.SchoolId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("GetSchools")]
        public async Task<IActionResult> Put([FromBody] SchoolDTO _SchoolDTO)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existSchool = await _context.Schools.Where(x => x.SchoolId == _SchoolDTO.SchoolId).FirstAsync();

                existSchool.SchoolId = _SchoolDTO.SchoolId;
                existSchool.SchoolName = _SchoolDTO.SchoolName;
                var updatedCourse = _context.Update(existSchool);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(updatedCourse.Entity.SchoolId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
