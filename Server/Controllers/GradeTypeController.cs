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
    public class GradeTypeController : BaseController<GradeType>
    {
        public GradeTypeController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        [HttpDelete]
        [Route("DeleteGradeType/{pGradeTypeCode}")]
        public async Task<IActionResult> Delete(string pGradeTypeCode)
        {
            GradeType itm = await _context.GradeTypes.Where(x => x.GradeTypeCode == pGradeTypeCode).FirstOrDefaultAsync();
            _context.Remove(itm);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("GetGradeTypes")]
        public async Task<IActionResult> Get()
        {
            List<GradeType> lst = await _context.GradeTypes.OrderBy(x => x.GradeTypeCode).ToListAsync();
            return Ok(lst);
        }

        [HttpGet]
        [Route("GetGradeTypes/{pGradeTypeCode}")]
        public async Task<IActionResult> Get(string pGradeTypeCode)
        {
            GradeType itm = await _context.GradeTypes.Where(x => x.GradeTypeCode == pGradeTypeCode).FirstOrDefaultAsync();
            return Ok(itm);
        }

        [HttpPost]
        [Route("GetGradeTypes")]
        public async Task<IActionResult> Post([FromBody] GradeTypeDTO _GradeTypeDTO)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existGradeType = await _context.GradeTypes.Where(x => x.GradeTypeCode == _GradeTypeDTO.GradeTypeCode).FirstOrDefaultAsync();

                if (existGradeType == null)
                {
                    existGradeType = new GradeType();
                }

                existGradeType.SchoolId = _GradeTypeDTO.SchoolId;
                existGradeType.GradeTypeCode = _GradeTypeDTO.GradeTypeCode;
                existGradeType.Description = _GradeTypeDTO.Description;
                var updatedCourse = _context.Update(existGradeType);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(updatedCourse.Entity.GradeTypeCode);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPut]
        [Route("GetGradeTypes")]
        public async Task<IActionResult> Put([FromBody] GradeTypeDTO _GradeTypeDTO)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existGradeType = await _context.GradeTypes.Where(x => x.GradeTypeCode == _GradeTypeDTO.GradeTypeCode).FirstAsync();


                existGradeType.SchoolId = _GradeTypeDTO.SchoolId;
                existGradeType.GradeTypeCode = _GradeTypeDTO.GradeTypeCode;
                existGradeType.Description = _GradeTypeDTO.Description;
                var updatedCourse = _context.Update(existGradeType);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(updatedCourse.Entity.GradeTypeCode);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
