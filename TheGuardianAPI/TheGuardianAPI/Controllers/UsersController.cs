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
    public class UsersController : ControllerBase
    {
        private readonly IGuardianRepository guardianRepo;

        public UsersController(IGuardianRepository guardianRepository)
        {
            guardianRepo = guardianRepository;
        }

        // GET: api/Users
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            IEnumerable<Core.Models.User> users = await guardianRepo.GetUsersAsync();
            return Ok(users);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await guardianRepo.GetUserAsync(id);
            if (user == null)
            {
                return NotFound($"User with ID {id} does not exist.");
            }
            return Ok(user);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }
            var updatedUser = await guardianRepo.PutUserAsync(id, Mapper.MapUser(user));
            if (updatedUser is null)
            {
                return BadRequest($"User with ID {id} does not exist.");
            }
            return Ok($"User with ID {id} was successfully updated.");
        }

        // POST: api/Users
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            var addedUser = await guardianRepo.PostUserAsync(Mapper.MapUser(user));
            if (addedUser is null)
            {
                return BadRequest($"A user with email {user.Email} already exists.");
            }
            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            if (await guardianRepo.RemoveUserAsync(id))
            {
                return NotFound($"User with {id} doesn't exist.");
            }
            return Ok($"User with id {id} was successfully deleted.");
        }
    }
}
