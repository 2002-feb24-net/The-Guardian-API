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
    public class ReviewsController : ControllerBase
    {
        private readonly IGuardianRepository guardianRepo;

        public ReviewsController(IGuardianRepository guardianRepository)
        {
            guardianRepo = guardianRepository;
        }

        // GET: api/Reviews
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Review>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
        {
            IEnumerable<Core.Models.Review> reviews = await guardianRepo.GetReviewsAsync();
            return Ok(reviews);
        }

        // GET: api/Reviews/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Review), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Review>> GetReview(int id)
        {
            var review = await guardianRepo.GetReviewAsync(id);
            if (review == null)
            {
                return NotFound($"Review with ID {id} does not exist.");
            }
            return Ok(review);
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
            var updatedReview = await guardianRepo.PutReviewAsync(id, Mapper.MapReview(review));
            if (updatedReview is null)
            {
                return BadRequest($"Review with ID {id} does not exist.");
            }
            return Ok($"Review with ID {id} was successfully updated.");
        }

        // POST: api/Reviews
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [ProducesResponseType(typeof(Review), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Review>> PostReview(Review review)
        {
            var addedReview= await guardianRepo.PostReviewAsync(Mapper.MapReview(review));
            if (addedReview is null)
            {
                return BadRequest($"The user with id {review.UserId} already placed a review at hospital with id {review.HospitalId}.");
            }
            return CreatedAtAction("GetReview", new { id = review.Id }, review);
        }

        // DELETE: api/Reviews/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Review), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Review>> DeleteReview(int id)
        {
            if (await guardianRepo.RemoveReviewAsync(id))
            {
                return NotFound($"Review with {id} doesn't exist.");
            }
            return Ok($"Review with id {id} was successfully deleted.");
        }
    }
}
