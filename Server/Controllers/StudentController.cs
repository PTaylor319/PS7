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
    public class StudentController : BaseController<Student>, IBaseController<StudentDTO>
    {
        public StudentController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        [HttpDelete]
        [Route("DeleteStudent/{pStudentId}")]
        public async Task<IActionResult> Delete(int pStudentId)
        {
            Student itm = await _context.Students.Where(x => x.StudentId == pStudentId).FirstOrDefaultAsync();
            _context.Remove(itm);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("GetStudents")]
        public async Task<IActionResult> Get()
        {
            List<Student> lst = await _context.Students.OrderBy(x => x.StudentId).ToListAsync();
            return Ok(lst);
        }

        [HttpGet]
        [Route("GetStudents/{pStudentId}")]
        public async Task<IActionResult> Get(int pStudentId)
        {
            Student itm = await _context.Students.Where(x => x.StudentId == pStudentId).FirstOrDefaultAsync();
            return Ok(itm);
        }

        [HttpPost]
        [Route("GetStudents")]
        public async Task<IActionResult> Post([FromBody] StudentDTO _StudentDTO)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existStudent = await _context.Students.Where(x => x.StudentId == _StudentDTO.StudentId).FirstOrDefaultAsync();

                if (existStudent == null)
                {
                    existStudent = new Student();
                }

                existStudent.StudentId = _StudentDTO.StudentId;
                existStudent.Salutation = _StudentDTO.Salutation;
                existStudent.FirstName = _StudentDTO.FirstName;
                existStudent.LastName = _StudentDTO.LastName;
                existStudent.StreetAddress = _StudentDTO.StreetAddress;
                existStudent.Zip = _StudentDTO.Zip;
                existStudent.SchoolId = _StudentDTO.SchoolId;
                existStudent.Phone = _StudentDTO.Phone;
                existStudent.Employer = _StudentDTO.Employer;
                existStudent.RegistrationDate = _StudentDTO.RegistrationDate;
                var updatedCourse = _context.Add(existStudent);
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
        [Route("GetStudents")]
        public async Task<IActionResult> Put([FromBody] StudentDTO _StudentDTO)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existStudent = await _context.Students.Where(x => x.StudentId == _StudentDTO.StudentId).FirstAsync();

                existStudent.StudentId = _StudentDTO.StudentId;
                existStudent.Salutation = _StudentDTO.Salutation;
                existStudent.FirstName = _StudentDTO.FirstName;
                existStudent.LastName = _StudentDTO.LastName;
                existStudent.StreetAddress = _StudentDTO.StreetAddress;
                existStudent.Zip = _StudentDTO.Zip;
                existStudent.SchoolId = _StudentDTO.SchoolId;
                existStudent.Phone = _StudentDTO.Phone;
                existStudent.Employer = _StudentDTO.Employer;
                existStudent.RegistrationDate = _StudentDTO.RegistrationDate;
                var updatedCourse = _context.Update(existStudent);
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
