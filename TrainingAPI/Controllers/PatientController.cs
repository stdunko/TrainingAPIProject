using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingAPI.DTO;
using TrainingAPI.Enums;
using TrainingAPI.Models;

namespace TrainingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly TrainingAPIDBContext _context;

        public PatientController(TrainingAPIDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientResponseDto>>> GetAll()
        {
            try
            {
                var patients = await _context.Patients.Include(p => p.Name).ToListAsync();

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
                        Given = p.Name.Given
                    }
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error when reading patients: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PatientResponseDto>> GetById(Guid id)
        {
            try
            {
                var patient = await _context.Patients.Include(p => p.Name)
                    .FirstOrDefaultAsync(p => p.ID == id);
                if (patient == null)
                {
                    return NotFound($"Patient with ID {id} not found.");
                }

                var result = new PatientResponseDto
                {
                    Id = patient.ID,
                    Gender = patient.Gender.ToString(),
                    BirthDate = patient.BirthDate,
                    Active = patient.Active.ToString(),
                    Name = new NameDto
                    {
                        Use = patient.Name.Use,
                        Family = patient.Name.Family,
                        Given = patient.Name.Given
                    }
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error when reading patient: " + ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<PatientResponseDto>> Create([FromBody] PatientCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var newName = new Name
                {
                    ID = Guid.NewGuid(),
                    Use = dto.Name.Use,
                    Family = dto.Name.Family,
                    Given = dto.Name.Given
                };

                var newPatient = new Patient
                {
                    ID = Guid.NewGuid(),
                    Gender = Enum.Parse<GenderEnum>(dto.Gender.ToString(), true),
                    BirthDate = dto.BirthDate,
                    Active = Enum.Parse<ActiveEnum>(dto.Active.ToString(), true),
                    Name = newName,
                    NameId = newName.ID
                };

                _context.Patients.Add(newPatient);
                await _context.SaveChangesAsync();

                return Ok("Successfully added");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error when creating patient: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] PatientUpdateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingPatient = await _context.Patients.Include(p => p.Name)
                    .FirstOrDefaultAsync(p => p.ID == id);
                if (existingPatient == null)
                {
                    return NotFound($"Patient with ID {id} not found.");
                }

                existingPatient.Gender = Enum.Parse<GenderEnum>(dto.Gender.ToString(), true);
                existingPatient.BirthDate = dto.BirthDate;
                existingPatient.Active = Enum.Parse<ActiveEnum>(dto.Active.ToString());

                if (existingPatient.Name != null && dto.Name != null)
                {
                    existingPatient.Name.Use = dto.Name.Use;
                    existingPatient.Name.Family = dto.Name.Family;
                    existingPatient.Name.Given = dto.Name.Given;
                }

                await _context.SaveChangesAsync();

                return Ok("Successfully updated");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error when updating patient: " + ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                var patient = await _context.Patients.Include(p => p.Name)
                    .FirstOrDefaultAsync(p => p.ID == id);
                if (patient == null)
                {
                    return NotFound($"Patient with ID {id} not found.");
                }

                if (patient.Name != null)
                {
                    _context.Remove(patient.Name);
                }

                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();

                return Ok("Successfully deleted");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error when deleting patient: " + ex.Message);
            }
        }
    }
}