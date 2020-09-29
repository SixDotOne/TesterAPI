using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TesterAPI.Models;

namespace TesterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Users_Controller : Controller
    {
        private readonly ApplicationContext _context;
        public List<User> Users { get; set; }
        public Users_Controller(ApplicationContext db)
        {
            _context = db;
        }
        public void OnGet()
        {
            Users = _context.Users.AsNoTracking().ToList();
        }
        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var UserToReturn = Users.FirstOrDefault(c => c.ID == id);
            if (UserToReturn == null)
            {
                return NotFound();
            }
            return Ok(UserToReturn);
        }
        [HttpPost()]
        public IActionResult PostQuestion([FromBody] User UserToPost)
        {

            _context.Users.Add(UserToPost);
            _context.SaveChanges();
            return Ok();
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {

            var UserFormDB = Users.FirstOrDefault(p => p.ID == id);
            if (UserFormDB == null)
            {
                return NotFound();
            }
            Users.Remove(UserFormDB);
            return NoContent();
        }
    }
}
