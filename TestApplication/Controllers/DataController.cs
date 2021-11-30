using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestApplication.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TestApplication.Data.DbModels;
using TestApplication.Data.Other;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataController : Controller
    {
        
        public readonly DataContext _dbContext;
        public readonly ILogger<DataController> _logger;
        public readonly JwtService _jwtService;

        //Will throw exception if verification unsuccessful.
        private void VerifyJwt() {
            var jwt = Request.Cookies["jwt"];
            var token = _jwtService.Verify(jwt);
        }
        
        public DataController(DataContext dbContext, JwtService jwtService)
        {
            _dbContext = dbContext;
            _jwtService = jwtService;
        }
        
        // GET: api/data
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = _jwtService.Verify(jwt);
            }
            catch
            {
                return Unauthorized();
            }

            var monitorData = (from q in _dbContext.monitorData.Include("QueueGroup")
                           select q).ToList<MonitorData>();


            return Ok(monitorData);
        }


        // GET api/data/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = _jwtService.Verify(jwt);
            }
            catch
            {
                return Unauthorized();
            }

            var monitorData = (from monitorDatum in _dbContext.monitorData.Include("QueueGroup")
                                where monitorDatum.Id==id
                               select monitorDatum).ToList<MonitorData>();
            return Ok(monitorData);
        }

        // POST api/data
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/data/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {

        }

        // DELETE api/data/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
