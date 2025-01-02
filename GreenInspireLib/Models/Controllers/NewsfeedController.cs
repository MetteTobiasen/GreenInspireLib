using GreenInspireLib.BusinessLogicLayer;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GreenInspireAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsfeedController : ControllerBase
    {
        private NewsfeedLogic newsfeedLogic;
        
        public NewsfeedController(NewsfeedLogic newsfeedLogic)
        {
            this.newsfeedLogic = newsfeedLogic;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<NewsfeedController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<NewsfeedController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<NewsfeedController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<NewsfeedController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
