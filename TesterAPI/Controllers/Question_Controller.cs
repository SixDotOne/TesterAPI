using System;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TesterAPI.Models;
using System.Data.Entity;
using Microsoft.AspNetCore.Authorization;

namespace TesterAPI.Controllers
{
    [Authorize]
    [Route("api/questions")]
    [ApiController]
    public class Question_Controller : Controller
    {
        private readonly ApplicationContext _context;
        public List<Question> Questions { get; set; }
        public Question_Controller(ApplicationContext db)
        {
            _context = db;
        }
        public void OnGet()
        {
            Questions = _context.Questions.AsNoTracking().ToList();
        }
        //
        [HttpGet()]
        public IActionResult GetQuestions(int Test_ID)
        {
            var QuestionsToReturn = from b in _context.Questions
                                    where b.Test_ID.Equals(Test_ID)
                                    select b;
            if (QuestionsToReturn == null)
            {
                return NotFound();
            }
            return Ok(QuestionsToReturn);
        }

        [HttpGet("{id}")]
        public IActionResult GetQuestion(int id)
        {
            var QuestionToReturn = Questions.FirstOrDefault(c => c.ID == id);
            if (QuestionToReturn == null)
            {
                return NotFound();
            }
            return Ok(QuestionToReturn);
        }
        [HttpPost()]
        public IActionResult PostQuestion([FromBody] Question QuestionToPost)
        {

            _context.Questions.Add(QuestionToPost);
            _context.SaveChanges();
            return new CreatedAtRouteResult("GetQuestion", new { id = QuestionToPost.ID });
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteQuestion(int id)
        {

            var QuestionFormDB = Questions.FirstOrDefault(p => p.ID == id);
            if (QuestionFormDB == null)
            {
                return NotFound();
            }
            Questions.Remove(QuestionFormDB);
            return NoContent();
        }
        [HttpPatch("{id}")]
        public IActionResult PatchQuestion(int id,
    [FromBody] JsonPatchDocument<Question> patchDoc)
        {
            if (patchDoc != null)
            {
                var QuestionFromDB = Questions.FirstOrDefault(x => x.ID == id);

                if (QuestionFromDB == null)
                {
                    return NotFound();
                }

                patchDoc.ApplyTo(QuestionFromDB, (Microsoft.AspNetCore.JsonPatch.Adapters.IObjectAdapter)ModelState);


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
        [HttpPut("{id}", Name = "UpdateQuestion")]
        public IActionResult PutQuestion(int id,
    [FromBody] Question QuestionToPut)
        {
            if (QuestionToPut.ID != id)
            {
                return BadRequest();
            }

            _context.Entry(QuestionToPut).State = (Microsoft.EntityFrameworkCore.EntityState)EntityState.Modified;
            _context.SaveChanges();
            return NoContent();
        }
        //
    }
}
