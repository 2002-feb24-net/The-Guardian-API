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
    public class UsersController : ControllerBase
    {
        private readonly IGuardianRepository _repository;

        public UsersController(IGuardianRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Users
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            IEnumerable<Core.Models.User> users = await _repository.GetUsersAsync();
            IEnumerable<User> resource = users.Select(Mapper.MapUser);
            return Ok(resource);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            Core.Models.User user = await _repository.GetUserAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(Mapper.MapUser(user));
        }

        // GET: api/Users/email
        [HttpGet("{email}/{password}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<User>> GetUser(string email, string password)
        {
            if (await _repository.GetUserLoginAsync(email, password) is Core.Models.User user)
            {
                return Ok(Mapper.MapUser(user));
            }
            else
            {
                return NotFound();
            }
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
            Core.Models.User updatedUser = await _repository.PutUserAsync(id, Mapper.MapUser(user));
            if (updatedUser == null)
            {
                return BadRequest($"User with ID {id} was not found.");
            }

            return Ok($"User with ID {id} was successfully updated.");
        }

        // POST: api/Users
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            Core.Models.User addedUser = await _repository.PostUserAsync(Mapper.MapUser(user));
            if (addedUser == null)
            {
                return BadRequest($"Unable to add new user.");
            }
            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            if (await _repository.RemoveUserAsync(id))
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}
