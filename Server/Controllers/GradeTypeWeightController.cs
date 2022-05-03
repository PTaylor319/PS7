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
    public class GradeTypeWeightController : BaseController<GradeTypeWeight>
    {
        public GradeTypeWeightController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        [HttpDelete]
        [Route("DeleteGradeTypeWeight/{pSectionId}/{pGradeTypeCode}")]
        public async Task<IActionResult> Delete(int pSectionId, string pGradeTypeCode)
        {
            GradeTypeWeight itm = await _context.GradeTypeWeights.Where(x => x.SectionId == pSectionId && x.GradeTypeCode == pGradeTypeCode).FirstOrDefaultAsync();
            _context.Remove(itm);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("GetGradeTypeWeights")]
        public async Task<IActionResult> Get()
        {
            List<GradeTypeWeight> lst = await _context.GradeTypeWeights.OrderBy(x => x.GradeTypeCode).ToListAsync();
            return Ok(lst);
        }

        [HttpGet]
        [Route("GetGradeTypeWeights/{pSectionId}")]
        public async Task<IActionResult> Get(int pSectionId, string pGradeTypeCode)
        {
            GradeTypeWeight itm = await _context.GradeTypeWeights.Where(x => x.SectionId == pSectionId && x.GradeTypeCode == pGradeTypeCode).FirstOrDefaultAsync();
            return Ok(itm);
        }

        [HttpPost]
        [Route("GetGradeTypeWeights")]
        public async Task<IActionResult> Post([FromBody] GradeTypeWeightDTO _GradeTypeWeightDTO)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existGradeTypeWeight = await _context.GradeTypeWeights.Where(x => x.SectionId == _GradeTypeWeightDTO.SectionId && x.GradeTypeCode == _GradeTypeWeightDTO.GradeTypeCode).FirstOrDefaultAsync();

                if (existGradeTypeWeight == null)
                {
                    existGradeTypeWeight = new GradeTypeWeight();
                }

                existGradeTypeWeight.SchoolId = _GradeTypeWeightDTO.SchoolId;
                existGradeTypeWeight.SectionId = _GradeTypeWeightDTO.SectionId;
                existGradeTypeWeight.GradeTypeCode = _GradeTypeWeightDTO.GradeTypeCode;
                existGradeTypeWeight.NumberPerSection = _GradeTypeWeightDTO.NumberPerSection;
                existGradeTypeWeight.PercentOfFinalGrade = _GradeTypeWeightDTO.PercentOfFinalGrade;
                existGradeTypeWeight.DropLowest = _GradeTypeWeightDTO.DropLowest;
                var updatedCourse = _context.Update(existGradeTypeWeight);
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
        [Route("GetGradeTypeWeights")]
        public async Task<IActionResult> Put([FromBody] GradeTypeWeightDTO _GradeTypeWeightDTO)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existGradeTypeWeight = await _context.GradeTypeWeights.Where(x => x.SectionId == _GradeTypeWeightDTO.SectionId && x.GradeTypeCode == _GradeTypeWeightDTO.GradeTypeCode).FirstAsync();

                existGradeTypeWeight.SchoolId = _GradeTypeWeightDTO.SchoolId;
                existGradeTypeWeight.SectionId = _GradeTypeWeightDTO.SectionId;
                existGradeTypeWeight.GradeTypeCode = _GradeTypeWeightDTO.GradeTypeCode;
                existGradeTypeWeight.NumberPerSection = _GradeTypeWeightDTO.NumberPerSection;
                existGradeTypeWeight.PercentOfFinalGrade = _GradeTypeWeightDTO.PercentOfFinalGrade;
                existGradeTypeWeight.DropLowest = _GradeTypeWeightDTO.DropLowest;
                var updatedCourse = _context.Update(existGradeTypeWeight);
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
