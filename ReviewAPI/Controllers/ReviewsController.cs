using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using ReviewAPI.Entities;
using ReviewAPI.Services;

namespace ReviewAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly ReviewsService _reviewsService;
        private readonly ArticlesServiceClient _articlesServiceClient;

        public ReviewsController(ReviewsService reviewsService, ArticlesServiceClient articlesServiceClient)
        {
            _reviewsService = reviewsService;
            _articlesServiceClient = articlesServiceClient;
        }

        [HttpGet]
        public async Task<List<Review>> Get() =>
            await _reviewsService.GetAsync();


        [EnableQuery]
        [HttpGet("/odata/Reviews")]
        public IQueryable<Review> GetOData() =>
            _reviewsService.AsQueryable();

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


        [HttpGet("article/{articleId:length(24)}/exists")]
        public async Task<ActionResult<bool>> HasReviewsForArticle(string articleId)
        {
            var hasReviews = await _reviewsService.HasReviewsForArticleAsync(articleId);
            return Ok(hasReviews);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Review newReview)
        {
            try
            {
                var articleExists = await _articlesServiceClient.ArticleExistsAsync(newReview.ArticleId);

                if (!articleExists)
                {
                    return BadRequest($"Article with id '{newReview.ArticleId}' does not exist.");
                }
            }
            catch (HttpRequestException)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "Could not validate article existence.");
            }

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
