using GreenInspireLib.Models;
using GreenInspireLib.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GreenInspireAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private CompanyUserSqlService _sqlService;

        public CompanyController(CompanyUserSqlService sqlService)
        {
            _sqlService = sqlService;
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<CompanyUser>> GetAllCompanyUsers([FromQuery] string? searchQuery)
        {
            var companyUser = _sqlService.GetUsers(searchQuery);
            if (companyUser == null || !companyUser.Any())
            {
                return NoContent();
            }
            return Ok(companyUser);
        }

        [HttpGet("{id}")]
        public ActionResult<CompanyUser> Get(int id)
        {
            var companyUser = _sqlService.GetUserById(id);
            if(companyUser== null)
            {                   
                return NoContent();
            }
            return Ok(companyUser);
        }

        [HttpGet("idByName/{name}")]
        public ActionResult<int> GetCompanyIdByName(string name)
        {
            int companyId = _sqlService.GetIdByCompanyName(name);
            if (companyId == -1)
            {
                return NoContent();
            }
            return Ok(companyId);
        }

        // POST api/<CompanyController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CompanyController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CompanyController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
