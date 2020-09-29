using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TesterAPI.Models;

namespace TesterAPI.Controllers
{
    [Authorize]
    [Route("api/answers")]
    [ApiController]
    public class Answers_Controller : Controller
    {
        private readonly ApplicationContext _context;
        public List<Answer> Answers { get; set; }
        public Answers_Controller(ApplicationContext db)
        {
            _context = db;
        }
        public void OnGet()
        {
            Answers = _context.Answers.AsNoTracking().ToList();
        }
        //
        [HttpGet()]
        public IActionResult GetAnswers(int Question_ID)
        {
            var AnswersToReturn = from b in _context.Answers
                                  where b.Question_ID.Equals(Question_ID)
                                    select b;
            if (AnswersToReturn == null)
            {
                return NotFound();
            }
            return Ok(AnswersToReturn);
        }

        
        [HttpPost()]
        public IActionResult PostAnswer([FromBody] Answer AnswerToPost)
        {

            _context.Answers.Add(AnswerToPost);
            _context.SaveChanges();
            return new CreatedAtRouteResult("GetQuestion", new { id = AnswerToPost.ID });
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteAnswer(int id)
        {

            var AnswerFormDB = Answers.FirstOrDefault(p => p.ID == id);
            if (AnswerFormDB == null)
            {
                return NotFound();
            }
            Answers.Remove(AnswerFormDB);
            return NoContent();
        }
        //
    }
}
