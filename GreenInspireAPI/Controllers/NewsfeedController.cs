using GreenInspireLib.BusinessLogicLayer;
using GreenInspireLib.Models;
using GreenInspireLib.DTO;
using GreenInspireLib.Services;
using Microsoft.AspNetCore.Mvc;
using Azure.Core.Serialization;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using System;
using Azure.Core;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GreenInspireAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsfeedController : ControllerBase
    {
        private NewsfeedLogic _newsfeedLogic;
        private NewsfeedSqlService _newsfeedSqlService;

        

        public NewsfeedController(NewsfeedLogic newsfeedLogic, NewsfeedSqlService newsfeedSqlService)
        {
            _newsfeedLogic = newsfeedLogic;
            _newsfeedSqlService = newsfeedSqlService;
        }

        public class AddNewsfeedRequest()
        {
            public Newsfeed newsfeed { get; set; }
            public Category category { get; set; }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<IEnumerable<NewsfeedWithCategoryDTO>> GetAllNewsfeeds([FromQuery] string? searchQuery)
        {
            var newsfeeds = _newsfeedLogic.GetAllNewsfeedsWithCategory(searchQuery);
            if (newsfeeds == null || !newsfeeds.Any())
            {
                return NoContent();
            }
            return Ok(newsfeeds);

        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<Newsfeed> GetNewsfeedById(int id)
        {
            var newsfeed = _newsfeedSqlService.GetNewsfeedById(id);
            if (newsfeed == null)
            {
                return NoContent();
            }
            return Ok(newsfeed);
        }

        //[HttpPost]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public ActionResult<NewsfeedWithCategoryDTO> AddNewsfeed([FromBody] AddNewsfeedRequest request)
        //{
        //    try
        //    {
        //        var newsfeedToAdd = _newsfeedLogic.AddNewsfeed(request.category, request.newsfeed);
        //        return Created("/" + newsfeedToAdd.newsfeedId, newsfeedToAdd);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
            
            
        //}

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Newsfeed> AddNewsfeed([FromBody] Newsfeed newsfeed, int categoryId, string? imageFile = null)
        {
            try
            {
                var newsfeedToAdd = _newsfeedSqlService.AddNewsfeed(newsfeed, categoryId, imageFile);
                return Created("/" + newsfeedToAdd.NewsfeedId, newsfeedToAdd);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpPost]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public ActionResult<Newsfeed> Post([FromBody] Newsfeed newsfeed, string newsfeedImagePath, string categoryName)
        //{
        //    try
        //    {
        //        //var category = newsfeed.Categories.FirstOrDefault();
        //        //if (category == null) throw new ArgumentException("Category not found");
        //        _newsfeedLogic.AddNewsfeedWithTransaction(categoryName, newsfeed, newsfeedImagePath, newsfeed.CompanyUserId);
        //        return Created("/" + newsfeed.NewsfeedId, newsfeed);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}




        [HttpPost("AddNewsfeedToCategory")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Category> AddNewsfeedToCategory([FromBody] string categoryName, int newsfeedId )
        {
            try
            {
                var category = _newsfeedLogic.AddNewsfeedToCategoryNewsfeedList(categoryName, newsfeedId);

                return Created( "/" + category.CategoryId, category);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddCategoryToNewsfeed")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Newsfeed> AddCategoryToNewsfeed([FromBody] string categoryName, int newsfeedId)
        {
            try
            {
                var newsfeed = _newsfeedLogic.AddCategoryToNewsfeedCategoryList(categoryName, newsfeedId);

                return Created("/" + newsfeed.NewsfeedId, newsfeed);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
