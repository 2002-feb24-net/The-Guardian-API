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
    public class ReasonsController : ControllerBase
    {
        private readonly IGuardianRepository guardianRepo;

        public ReasonsController(IGuardianRepository guardianRepository)
        {
            guardianRepo = guardianRepository;
        }

        // GET: api/Reasons
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Reason>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Reason>>> GetReasons()
        {
            IEnumerable<Core.Models.Reason> reasons = await guardianRepo.GetReasonsAsync();
            return Ok(reasons);
        }

        // GET: api/Reasons/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Reason), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Reason>> GetReason(int id)
        {
            var reason = await guardianRepo.GetReasonAsync(id);
            if (reason == null)
            {
                return NotFound($"Reason with ID {id} does not exist.");
            }
            return Ok(reason);
        }

        // PUT: api/Reasons/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Reason), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutReason(int id, Reason reason)
        {
            if (id != reason.Id)
            {
                return BadRequest();
            }
            var reas = await guardianRepo.PutReasonAsync(id, Mapper.MapReason(reason));
            if (reas is null)
            {
                return BadRequest($"Reason with ID {id} does not exist.");
            }
            return Ok($"Reason with ID {id} was successfully updated.");
        }

        // POST: api/Reasons
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [ProducesResponseType(typeof(Reason), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Reason>> PostReason(Reason reason)
        {
            var addedReason = await guardianRepo.PostReasonAsync(Mapper.MapReason(reason));
            if (addedReason is null)
            {
                return BadRequest($"That reason already exists.");
            }
            return CreatedAtAction("GetReason", new { id = reason.Id }, reason);
        }

        // DELETE: api/Reasons/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Reason), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Reason>> DeleteReason(int id)
        {
            if (await guardianRepo.RemoveReasonAsync(id))
            {
                return NotFound($"Reason with {id} doesn't exist.");
            }
            return Ok($"Reason with id {id} was successfully deleted.");
        }
    }
}
