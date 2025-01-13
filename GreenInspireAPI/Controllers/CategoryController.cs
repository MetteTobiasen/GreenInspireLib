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
        public ActionResult<IEnumerable<Category>> GetAllCategories([FromQuery] string? searchQuery)
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
        public ActionResult<Category> GetCategoryById(int id)
        {
            var category = _sqlService.GetCategoryById(id);
            if (category == null)
            {
                return NoContent();
            }
            return Ok(category);
        }

        [HttpGet("idByName/{name}")]
        public ActionResult<int> GetCategoryIdByName(string name)
        {
            int categoryId = _sqlService.GetIdByCategoryName(name);
            if (categoryId == -1)
            {
                return NoContent();
            }
            return Ok(categoryId);
        }

        // POST api/<CategoryController>
        //[HttpPost]
        //public void AddCategory([FromBody] string value)
        //{

        //}

        // PUT api/<CategoryController>/5
        //[HttpPut("{id}")]
        //public void UpdateCateogory(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<CategoryController>/5
        //[HttpDelete("{id}")]
        //public void DeleteCategory(int id)
        //{
        //}
    }
}
