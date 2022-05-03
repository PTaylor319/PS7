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
    public class GradeConversionController : BaseController<GradeConversion>
    {
        public GradeConversionController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        [HttpDelete]
        [Route("DeleteGradeConverstion/{pLetterGrade}")]
        public async Task<IActionResult> Delete(String pLetterGrade)
        {
            GradeConversion itm = await _context.GradeConversions.Where(x => x.LetterGrade == pLetterGrade).FirstOrDefaultAsync();
            _context.Remove(itm);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("GetGradeConversions")]
        public async Task<IActionResult> Get()
        {
            List<GradeConversion> lst = await _context.GradeConversions.OrderBy(x => x.LetterGrade).ToListAsync();
            return Ok(lst);
        }

        [HttpGet]
        [Route("GetGradeConversions/{pLetterGrade}")]
        public async Task<IActionResult> Get(String pLetterGrade)
        {
            GradeConversion itm = await _context.GradeConversions.Where(x => x.LetterGrade == pLetterGrade).FirstOrDefaultAsync();
            return Ok(itm);
        }

        [HttpPost]
        [Route("GetGradeConversions")]
        public async Task<IActionResult> Post([FromBody] GradeConversionDTO _GradeConversionDTO)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existGradeConversion = await _context.GradeConversions.Where(x => x.LetterGrade == _GradeConversionDTO.LetterGrade).FirstOrDefaultAsync();

                if (existGradeConversion == null)
                {
                    existGradeConversion = new GradeConversion();
                }

                existGradeConversion.SchoolId = _GradeConversionDTO.SchoolId;
                existGradeConversion.LetterGrade = _GradeConversionDTO.LetterGrade;
                existGradeConversion.GradePoint = _GradeConversionDTO.GradePoint;
                existGradeConversion.MaxGrade = _GradeConversionDTO.MaxGrade;
                existGradeConversion.MinGrade = _GradeConversionDTO.MinGrade;
                var updatedCourse = _context.Update(existGradeConversion);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(updatedCourse.Entity.LetterGrade);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("GetGradeConversions")]
        public async Task<IActionResult> Put([FromBody] GradeConversionDTO _GradeConversionDTO)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existGradeConversion = await _context.GradeConversions.Where(x => x.LetterGrade == _GradeConversionDTO.LetterGrade).FirstAsync();


                existGradeConversion.SchoolId = _GradeConversionDTO.SchoolId;
                existGradeConversion.LetterGrade = _GradeConversionDTO.LetterGrade;
                existGradeConversion.GradePoint = _GradeConversionDTO.GradePoint;
                existGradeConversion.MaxGrade = _GradeConversionDTO.MaxGrade;
                existGradeConversion.MinGrade = _GradeConversionDTO.MinGrade;
                var updatedCourse = _context.Update(existGradeConversion);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(updatedCourse.Entity.LetterGrade);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
