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
    public class ReviewsController : ControllerBase
    {
        private readonly IGuardianRepository _repository;

        public ReviewsController(IGuardianRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Reviews
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Review>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
        {
            IEnumerable<Core.Models.Review> reviews = await _repository.GetReviewsAsync();
            IEnumerable<Review> resource = reviews.Select(Mapper.MapReview);
            return Ok(resource);
        }

        // GET: api/Reviews/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Review), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Review>> GetReview(int id)
        {
            Core.Models.Review review = await _repository.GetReviewAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            return Ok(Mapper.MapReview(review));
        }

        // PUT: api/Reviews/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Review), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutReview(int id, Review review)
        {
            if (id != review.Id)
            {
                return BadRequest();
            }
            Core.Models.Review updatedReview = await _repository.PutReviewAsync(id, Mapper.MapReview(review));
            if (updatedReview == null)
            {
                return BadRequest($"Review with ID {id} was not found.");
            }

            return Ok($"Review with ID {id} was successfully updated.");
        }

        // POST: api/Reviews
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Review>> PostReview(Review review)
        {
            //return BadRequest("This method is currently non-functional");
            Core.Models.Review addedReview = await _repository.PostReviewAsync(Mapper.MapReview(review));
            if (addedReview == null)
            {
                return BadRequest($"Unable to add new review.");
            }
            return CreatedAtAction("GetReview", new { id = review.Id }, review);
        }

        // DELETE: api/Reviews/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Review>> DeleteReview(int id)
        {
            if (await _repository.RemoveReviewAsync(id))
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}
