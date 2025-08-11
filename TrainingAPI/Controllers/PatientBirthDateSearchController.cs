using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingAPI.DTO;
using TrainingAPI.Models;

namespace TrainingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientBirthDateSearchController : ControllerBase
    {
        private readonly TrainingAPIDBContext _context;
        public PatientBirthDateSearchController(TrainingAPIDBContext context)
        {
            _context = context;
        }

        [HttpPost("search")]
        public async Task<ActionResult<IEnumerable<PatientResponseDto>>> SearchPatients([FromBody] PatientBirthDateSearchDto searchDto)
        {
            var query = _context.Patients.Include(p => p.Name).AsQueryable();

            if (searchDto.ExactDate.HasValue)
            {
                query = query.Where(p => p.BirthDate.Date == searchDto.ExactDate.Value.Date);
            }
            else
            {
                if (searchDto.FromDate.HasValue)
                {
                    query = query.Where(p => p.BirthDate.Date >= searchDto.FromDate.Value.Date);
                }

                if (searchDto.ToDate.HasValue)
                {
                    query = query.Where(p => p.BirthDate.Date <= searchDto.ToDate.Value.Date);
                }

                if (searchDto.BeforeDate.HasValue)
                {
                    query = query.Where(p => p.BirthDate.Date < searchDto.BeforeDate.Value.Date);
                }

                if (searchDto.AfterDate.HasValue)
                {
                    query = query.Where(p => p.BirthDate.Date > searchDto.AfterDate.Value.Date);
                }
            }

            var patients = await query.ToListAsync();

            var result = patients.Select(p => new PatientResponseDto
            {
                Id = p.ID,
                Gender = p.Gender.ToString(),
                BirthDate = p.BirthDate,
                Active = p.Active.ToString(),
                Name = new NameDto
                {
                    Use = p.Name.Use,
                    Family = p.Name.Family,
                    Given = p.Name.Given,
                }
            });

            return Ok(result);
        }
    }
}
