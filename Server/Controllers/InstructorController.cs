using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWARM.EF.Data;
using SWARM.EF.Models;
using SWARM.Server.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SWARM.Shared.DTO;

namespace SWARM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : BaseController<Instructor>, IBaseController<InstructorDTO>
    {
        public InstructorController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        [HttpDelete]
        [Route("DeleteInstructor/{pInstructorId}")]
        public async Task<IActionResult> Delete(int pInstructorId)
        {
            Instructor itm = await _context.Instructors.Where(x => x.InstructorId == pInstructorId).FirstOrDefaultAsync();
            _context.Remove(itm);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("GetInstructors")]
        public async Task<IActionResult> Get()
        {
            List<Instructor> lst = await _context.Instructors.OrderBy(x => x.InstructorId).ToListAsync();
            return Ok(lst);
        }

        [HttpGet]
        [Route("GetInstructors/{pInstructorId}")]
        public async Task<IActionResult> Get(int pInstructorId)
        {
            Instructor itm = await _context.Instructors.Where(x => x.InstructorId == pInstructorId).FirstOrDefaultAsync();
            return Ok(itm);
        }
        [HttpPost]
        [Route("GetInstructors")]
        public async Task<IActionResult> Post([FromBody] InstructorDTO _InstructorDTO)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existInstructor = await _context.Instructors.Where(x => x.InstructorId == _InstructorDTO.InstructorId).FirstOrDefaultAsync();

                if (existInstructor == null)
                {
                    existInstructor = new Instructor();
                }

                existInstructor.SchoolId = _InstructorDTO.SchoolId;
                existInstructor.Salutation = _InstructorDTO.Salutation;
                existInstructor.FirstName = _InstructorDTO.FirstName;
                existInstructor.LastName = _InstructorDTO.LastName;
                existInstructor.InstructorId = _InstructorDTO.InstructorId;
                existInstructor.StreetAddress = _InstructorDTO.StreetAddress;
                existInstructor.Zip = _InstructorDTO.Zip;
                existInstructor.Phone = _InstructorDTO.Phone;
                var updatedCourse = _context.Update(existInstructor);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(updatedCourse.Entity.InstructorId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("GetInstructors")]
        public async Task<IActionResult> Put([FromBody] InstructorDTO _InstructorDTO)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existInstructor = await _context.Instructors.Where(x => x.InstructorId == _InstructorDTO.InstructorId).FirstAsync();
                existInstructor.SchoolId = _InstructorDTO.SchoolId;
                existInstructor.Salutation = _InstructorDTO.Salutation;
                existInstructor.FirstName = _InstructorDTO.FirstName;
                existInstructor.LastName = _InstructorDTO.LastName;
                existInstructor.InstructorId = _InstructorDTO.InstructorId;
                existInstructor.StreetAddress = _InstructorDTO.StreetAddress;
                existInstructor.Zip = _InstructorDTO.Zip;
                existInstructor.Phone = _InstructorDTO.Phone;
                var updatedCourse = _context.Update(existInstructor);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(updatedCourse.Entity.InstructorId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
