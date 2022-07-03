using Microsoft.AspNetCore.Mvc;
using ReviewAPI.Entities;
using ReviewAPI.Services;

namespace ReviewAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly ReviewsService _reviewsService;

        public ReviewsController(ReviewsService reviewsService) =>
            _reviewsService = reviewsService;

        [HttpGet]
        public async Task<List<Review>> Get() =>
            await _reviewsService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Review>> Get(string id)
        {
            var review = await _reviewsService.GetAsync(id);

            if (review is null)
            {
                return NotFound();
            }

            return review;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Review newReview)
        {
            await _reviewsService.CreateAsync(newReview);

            return CreatedAtAction(nameof(Get), new { id = newReview.Id }, newReview);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Review updatedReview)
        {
            var review = await _reviewsService.GetAsync(id);

            if (review is null)
            {
                return NotFound();
            }

            updatedReview.Id = review.Id;

            await _reviewsService.UpdateAsync(id, updatedReview);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var review = await _reviewsService.GetAsync(id);

            if (review is null)
            {
                return NotFound();
            }

            await _reviewsService.RemoveAsync(id);

            return NoContent();
        }
    }
}
