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
    public class SectionController : BaseController<Section>, IBaseController<SectionDTO>
    {
        public SectionController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        [HttpDelete]
        [Route("DeleteSection/{pSectionId}")]
        public async Task<IActionResult> Delete(int pSectionId)
        {
            Section itm = await _context.Sections.Where(x => x.SectionId == pSectionId).FirstOrDefaultAsync();
            _context.Remove(itm);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("GetSections")]
        public async Task<IActionResult> Get()
        {
            List<Section> lst = await _context.Sections.OrderBy(x => x.SectionId).ToListAsync();
            return Ok(lst);
        }

        [HttpGet]
        [Route("GetSections/{pSectionId}")]
        public async Task<IActionResult> Get(int pSectionId)
        {
            Section itm = await _context.Sections.Where(x => x.SectionId == pSectionId).FirstOrDefaultAsync();
            return Ok(itm);
        }

        [HttpPost]
        [Route("GetSections")]
        public async Task<IActionResult> Post([FromBody] SectionDTO _SectionDTO)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existSection = await _context.Sections.Where(x => x.SectionId == _SectionDTO.SectionId).FirstOrDefaultAsync();

                if (existSection == null)
                {
                    existSection = new Section();
                }

                existSection.CourseNo = _SectionDTO.CourseNo;
                existSection.SectionNo = _SectionDTO.SectionNo;
                existSection.StartDateTime = _SectionDTO.StartDateTime;
                existSection.Location = _SectionDTO.Location;
                existSection.InstructorId = _SectionDTO.InstructorId;
                existSection.Capacity = _SectionDTO.Capacity;
                existSection.SchoolId = _SectionDTO.SchoolId;
                var updatedCourse = _context.Update(existSection);
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
        [Route("GetSections")]
        public async Task<IActionResult> Put([FromBody] SectionDTO _SectionDTO)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existSection = await _context.Sections.Where(x => x.SectionId == _SectionDTO.SectionId).FirstAsync();

                existSection.CourseNo = _SectionDTO.CourseNo;
                existSection.SectionNo = _SectionDTO.SectionNo;
                existSection.StartDateTime = _SectionDTO.StartDateTime;
                existSection.Location = _SectionDTO.Location;
                existSection.InstructorId = _SectionDTO.InstructorId;
                existSection.Capacity = _SectionDTO.Capacity;
                existSection.SchoolId = _SectionDTO.SchoolId;
                var updatedCourse = _context.Update(existSection);
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
