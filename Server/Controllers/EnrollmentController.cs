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
    public class EnrollmentController : BaseController<Enrollment>
    {
        public EnrollmentController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        [HttpDelete]
        [Route("DeleteEnrollment/{pStudentId}/{pSectionId}/{pSchoolId}")]
        public async Task<IActionResult> Delete(int pStudentId, int pSectionId, int pSchoolId)
        {
            Enrollment itm = await _context.Enrollments.Where(x => x.StudentId == pStudentId && x.SectionId == pSectionId && x.SchoolId == pSchoolId).FirstOrDefaultAsync();
            _context.Remove(itm);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("GetEnrollments")]
        public async Task<IActionResult> Get()
        {
            List<Enrollment> lst = await _context.Enrollments.OrderBy(x => x.StudentId).ToListAsync();
            return Ok(lst);
        }

        [HttpGet]
        [Route("GetEnrollments/{pStudentId}/{pSectionId}/{pSchoolId}")]
        public async Task<IActionResult> Get(int pStudentId, int pSectionId, int pSchoolId)
        {
            Enrollment itm = await _context.Enrollments.Where(x => x.StudentId == pStudentId && x.SectionId == pSectionId && x.SchoolId == pSchoolId).FirstOrDefaultAsync();
            return Ok(itm);
        }

        [HttpPost]
        [Route("GetEnrollments")]
        public async Task<IActionResult> Post([FromBody] EnrollmentDTO _EnrollmentDTO)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existEnrollment = await _context.Enrollments.Where(x => x.StudentId == _EnrollmentDTO.StudentId && x.SectionId == _EnrollmentDTO.SectionId && x.SchoolId == _EnrollmentDTO.SchoolId).FirstOrDefaultAsync();

                if (existEnrollment == null)
                {
                    existEnrollment = new Enrollment();
                }

                existEnrollment.StudentId = existEnrollment.StudentId;
                existEnrollment.SectionId = _EnrollmentDTO.SectionId;
                existEnrollment.EnrollDate = _EnrollmentDTO.EnrollDate;
                existEnrollment.FinalGrade = _EnrollmentDTO.FinalGrade;
                existEnrollment.SchoolId = _EnrollmentDTO.SchoolId;
                var updatedCourse = _context.Update(existEnrollment);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(updatedCourse.Entity.StudentId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("GetEnrollments")]
        public async Task<IActionResult> Put([FromBody] EnrollmentDTO _EnrollmentDTO)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existEnrollment = await _context.Enrollments.Where(x => x.StudentId == _EnrollmentDTO.StudentId && x.SectionId == _EnrollmentDTO.SectionId && x.SchoolId == _EnrollmentDTO.SchoolId).FirstAsync();


                existEnrollment.StudentId = existEnrollment.StudentId;
                existEnrollment.SectionId = _EnrollmentDTO.SectionId;
                existEnrollment.EnrollDate = _EnrollmentDTO.EnrollDate;
                existEnrollment.FinalGrade = _EnrollmentDTO.FinalGrade;
                existEnrollment.SchoolId = _EnrollmentDTO.SchoolId;
                var updatedCourse = _context.Update(existEnrollment);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(updatedCourse.Entity.StudentId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
