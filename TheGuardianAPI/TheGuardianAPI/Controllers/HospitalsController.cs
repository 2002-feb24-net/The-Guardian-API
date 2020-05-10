using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheGuardian.Core.Interfaces;
using TheGuardian.DataAccess;

namespace TheGuardian.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HospitalsController : ControllerBase
    {
        private readonly IGuardianRepository _repository;

        public HospitalsController(IGuardianRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Hospitals
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Hospital>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Hospital>>> GetHospitals()
        {
            IEnumerable<Core.Models.Hospital> hospitals = await _repository.GetHospitalsAsync();
            IEnumerable<Hospital> resource = hospitals.Select(Mapper.MapHospital);
            return Ok(resource);
        }

        // GET: api/Hospitals/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Hospital), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Hospital>> GetHospital(int id)
        {
            Core.Models.Hospital hospital = await _repository.GetHospitalAsync(id);
            if (hospital == null)
            {
                return NotFound();
            }
            return Ok(Mapper.MapHospital(hospital));
        }

        // PUT: api/Hospitals/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Hospital), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutHospital(int id, Hospital hospital)
        {
            if (id != hospital.Id)
            {
                return BadRequest();
            }
            Core.Models.Hospital updatedHospital = await _repository.PutHospitalAsync(id, Mapper.MapHospital(hospital));
            if (updatedHospital == null)
            {
                return BadRequest($"Hospital with ID {id} was not found.");
            }

            return Ok($"Hospital with ID {id} was successfully updated.");
        }

        // POST: api/Hospitals
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [ProducesResponseType(typeof(Hospital), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Hospital>> PostHospital(Hospital hospital)
        {
            Core.Models.Hospital addedHospital = await _repository.PostHospitalAsync(Mapper.MapHospital(hospital));
            if (addedHospital == null)
            {
                return BadRequest($"Unable to add new hospital.");
            }
            return CreatedAtAction("GetHospital", new { id = hospital.Id }, hospital);
        }

        // DELETE: api/Hospitals/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteHospital(int id)
        {
            if (await _repository.RemoveHospitalAsync(id))
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}
