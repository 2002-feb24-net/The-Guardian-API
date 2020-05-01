using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheGuardian.DataAccess;

namespace TheGuardian.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReasonsController : ControllerBase
    {
        private readonly GuardianContext _context;

        public ReasonsController(GuardianContext context)
        {
            _context = context;
        }

        // GET: api/Reasons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reason>>> GetReasons()
        {
            return await _context.Reasons.ToListAsync();
        }

        // GET: api/Reasons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reason>> GetReason(int id)
        {
            var reason = await _context.Reasons.FindAsync(id);

            if (reason == null)
            {
                return NotFound();
            }

            return reason;
        }

        // PUT: api/Reasons/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReason(int id, Reason reason)
        {
            if (id != reason.Id)
            {
                return BadRequest();
            }

            _context.Entry(reason).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReasonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Reasons
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Reason>> PostReason(Reason reason)
        {
            _context.Reasons.Add(reason);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReason", new { id = reason.Id }, reason);
        }

        // DELETE: api/Reasons/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Reason>> DeleteReason(int id)
        {
            var reason = await _context.Reasons.FindAsync(id);
            if (reason == null)
            {
                return NotFound();
            }

            _context.Reasons.Remove(reason);
            await _context.SaveChangesAsync();

            return reason;
        }

        private bool ReasonExists(int id)
        {
            return _context.Reasons.Any(e => e.Id == id);
        }
    }
}
