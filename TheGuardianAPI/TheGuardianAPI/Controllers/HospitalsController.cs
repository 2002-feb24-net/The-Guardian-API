using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheGuardian.Core.Interfaces;
using TheGuardian.DataAccess;

namespace TheGuardian.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HospitalsController : ControllerBase
    {
        private readonly IGuardianRepository guardianRepo;

        public HospitalsController(IGuardianRepository guardianRepository)
        {
            this.guardianRepo = guardianRepository;
        }

        // GET: api/Hospitals
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Hospital>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Hospital>>> GetHospitals()
        {
            IEnumerable<Core.Models.Hospital> hospitals = await guardianRepo.GetHospitalsAsync();
            return Ok(hospitals);
        }

        // GET: api/Hospitals/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Hospital), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Hospital>> GetHospital(int id)
        {
            var hospital = await guardianRepo.GetHospitalAsync(id);
            if (hospital == null)
            {
                return NotFound($"Hospital with ID {id} does not exist.");
            }
            return Ok(hospital);
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
            var hosp = await guardianRepo.PutHospitalAsync(id, Mapper.MapHospital(hospital));
            if (hosp is null)
            {
                return BadRequest($"Hospital with ID {id} does not exist.");
            }
            return Ok($"Hospital with ID {id} was successfully updated.");
        }

        // POST: api/Hospitals
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [ProducesResponseType(typeof(Hospital), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Hospital>> PostHospital(Hospital hospital)
        {
            var addedHospital = await guardianRepo.PostHospitalAsync(Mapper.MapHospital(hospital));
            if (addedHospital is null)
            {
                return BadRequest($"That hospital already exists.");
            }
            return CreatedAtAction("GetHospital", new { id = hospital.Id }, hospital);
        }

        // DELETE: api/Hospitals/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Hospital), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Hospital>> DeleteHospital(int id)
        {
            if (await guardianRepo.RemoveHospitalAsync(id))
            {
                return NotFound($"Hospital with {id} doesn't exist.");
            }
            return Ok($"Hospital with id {id} was successfully deleted.");
        }
    }
}
