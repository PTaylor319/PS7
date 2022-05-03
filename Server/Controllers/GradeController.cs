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
    public class GradeController : BaseController<Grade>    
    {
        public GradeController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        [HttpDelete]
        [Route("DeleteGrade/{pSchoolId}/{pStudentId}/{pSectionId}/{pGradeTypeCode}/{pGradeCodeOcc}")]
        public async Task<IActionResult> Delete(int pSchoolId,int pStudentId, int pSectionId, int pGradeCodeOcc,string pGradeTypeCode)
        {
            Grade itm = await _context.Grades.Where(x => x.SchoolId == pSchoolId && x.StudentId == pStudentId && x.SectionId == pSectionId && x.GradeCodeOccurrence == pGradeCodeOcc && x.GradeTypeCode == pGradeTypeCode).FirstOrDefaultAsync();
            _context.Remove(itm);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("GetGrades")]
        public async Task<IActionResult> Get()
        {
            List<Grade> lst = await _context.Grades.OrderBy(x => x.StudentId).ToListAsync();
            return Ok(lst);
        }

        [HttpGet]
        [Route("GetGrades/{pSchoolId}/{pStudentId}/{pSectionId}/{pGradeTypeCode}/{pGradeCodeOcc}")]
        public async Task<IActionResult> Get(int pSchoolId, int pStudentId, int pSectionId, int pGradeCodeOcc, string pGradeTypeCode)
        {
            Grade itm = await _context.Grades.Where(x => x.SchoolId == pSchoolId && x.StudentId == pStudentId && x.SectionId == pSectionId && x.GradeCodeOccurrence == pGradeCodeOcc && x.GradeTypeCode == pGradeTypeCode).FirstOrDefaultAsync();
            return Ok(itm);
        }

        [HttpPost]
        [Route("GetGrades")]
        public async Task<IActionResult> Post([FromBody] GradeDTO _GradeDTO)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existGrade = await _context.Grades.Where(x => x.SectionId == _GradeDTO.SectionId).FirstOrDefaultAsync();

                if (existGrade == null)
                {
                    existGrade = new Grade();
                }

                existGrade.SchoolId = _GradeDTO.SchoolId;
                existGrade.StudentId = _GradeDTO.StudentId;
                existGrade.SectionId = _GradeDTO.SectionId;
                existGrade.GradeTypeCode = _GradeDTO.GradeTypeCode;
                existGrade.GradeCodeOccurrence = _GradeDTO.GradeCodeOccurrence;
                existGrade.NumericGrade = _GradeDTO.NumericGrade;
                existGrade.Comments = _GradeDTO.Comments;
                var updatedCourse = _context.Update(existGrade);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(updatedCourse.Entity.SectionId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("GetGrades")]
        public async Task<IActionResult> Put([FromBody] GradeDTO _GradeDTO)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existGrade = await _context.Grades.Where(x => x.SectionId == _GradeDTO.SectionId).FirstAsync();

                existGrade.SchoolId = _GradeDTO.SchoolId;
                existGrade.StudentId = _GradeDTO.StudentId;
                existGrade.SectionId = _GradeDTO.SectionId;
                existGrade.GradeTypeCode = _GradeDTO.GradeTypeCode;
                existGrade.GradeCodeOccurrence = _GradeDTO.GradeCodeOccurrence;
                existGrade.NumericGrade = _GradeDTO.NumericGrade;
                existGrade.Comments = _GradeDTO.Comments;
                var updatedCourse = _context.Update(existGrade);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(updatedCourse.Entity.SectionId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
