using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesterAPI.Models;


namespace TesterAPI.Controllers
{
    [ApiController]
    [Route("api/tests")]
    public class Test_Controller : Controller
    {
        private readonly ApplicationContext _context;
        public List<Test> Tests { get; set; }
        public Test_Controller(ApplicationContext db)
        {
            _context = db;
        }
        public void OnGet()
        {
            Tests = _context.Tests.AsNoTracking().ToList();
        }
        [HttpGet()]
        public IActionResult GetTests()
        {
            return Ok(Tests);
        }

        [HttpGet("{id}")]
        public IActionResult GetTest(int id)
        {
            var TestToReturn = Tests.FirstOrDefault(c => c.ID == id);
            if (TestToReturn == null)
            {
                return NotFound();
            }
            return Ok(TestToReturn);
        }
        [HttpPost()]
        public IActionResult PostTest([FromBody] Test TestToPost)
        {

            _context.Tests.Add(TestToPost);
            _context.SaveChanges();
            return new CreatedAtRouteResult("GetTest", new { id = TestToPost.ID });
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteTest(int id)
        {
            
            var TestFormDB = Tests.FirstOrDefault(p => p.ID == id);
            if (TestFormDB == null)
            {
                return NotFound();
            }
            Tests.Remove(TestFormDB);
            return NoContent();
        }
        [HttpPatch("{id}")]
        public IActionResult PatchTest(int id,
    [FromBody] JsonPatchDocument<Test> patchDoc)
        {
            if (patchDoc != null)
            {
                var TestFromDB = Tests.FirstOrDefault(x => x.ID == id);

                if (TestFromDB == null)
                {
                    return NotFound();
                }

                patchDoc.ApplyTo(TestFromDB, (Microsoft.AspNetCore.JsonPatch.Adapters.IObjectAdapter)ModelState);


                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _context.SaveChanges();
                return NoContent();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpPut("{id}", Name = "UpdateTest")]
        public IActionResult PutTest(int id,
    [FromBody] Test TestToPut)
        {
            if (TestToPut.ID != id)
            {
                return BadRequest();
            }

            _context.Entry(TestToPut).State = EntityState.Modified;
            _context.SaveChanges();
            return NoContent();
        }
    }
}

