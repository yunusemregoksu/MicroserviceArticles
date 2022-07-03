using ArticleAPI.Entities;
using ArticleAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ArticleAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly ArticlesService _articlesService;

        public ArticlesController(ArticlesService articlesService) =>
            _articlesService = articlesService;

        [HttpGet]
        public async Task<List<Article>> Get() =>
            await _articlesService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Article>> Get(string id)
        {
            var article = await _articlesService.GetAsync(id);

            if (article is null)
            {
                return NotFound();
            }

            return article;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Article newArticle)
        {
            await _articlesService.CreateAsync(newArticle);

            return CreatedAtAction(nameof(Get), new { id = newArticle.Id }, newArticle);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Article updatedArticle)
        {
            var article = await _articlesService.GetAsync(id);

            if (article is null)
            {
                return NotFound();
            }

            updatedArticle.Id = article.Id;

            await _articlesService.UpdateAsync(id, updatedArticle);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var article = await _articlesService.GetAsync(id);

            if (article is null)
            {
                return NotFound();
            }

            await _articlesService.RemoveAsync(id);

            return NoContent();
        }
    }
}
