using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleCrudApi.Data;
using SimpleCrudApi.Models;

namespace SimpleCrudApi.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {

        private readonly AppDbContext _context;

        public StudentsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Students.ToList());
        }

        [HttpPost]
        public IActionResult Post(Student student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
            return Ok(student);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Student student)
        {
            var s = _context.Students.Find(id);
            if (s == null) return NotFound();

            s.Name = student.Name;
            _context.SaveChanges();
            return Ok(s);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var s = _context.Students.Find(id);
            if (s == null) return NotFound();

            _context.Students.Remove(s);
            _context.SaveChanges();
            return Ok();
        }
    }
}
