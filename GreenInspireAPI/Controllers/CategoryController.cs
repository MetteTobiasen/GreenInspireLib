using GreenInspireLib.Models;
using GreenInspireLib.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GreenInspireAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private CategorySqlService _sqlService;

        public CategoryController(CategorySqlService sqlService)
        {
            _sqlService = sqlService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Category>> Get([FromQuery] string? searchQuery)
        {
            var category = _sqlService.GetCategories(searchQuery);
            if (category == null  || !category.Any())
            {
                return NoContent();
            }
            return Ok(category);
        }

        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CategoryController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CategoryController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
