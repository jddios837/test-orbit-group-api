using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using orbitgroup.api.Entities;

namespace orbitgroup.api.Controllers
{
    [Route("api/[controller]")]
    public class StudentController : Controller
    {
        private readonly StudentDbContext dbContext;

        public StudentController(StudentDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        // GET ALL STUDENTS
        [HttpGet]
        public ActionResult<IEnumerable<Student>> Get()
        {
            return Ok(dbContext.Students.ToList());
        }

        // SAVE A NEW STUDENT
        [HttpPost]
        public ActionResult<IEnumerable<Student>> Post([FromBody] Student student)
        {
            if(StudentUserNameExists(student.UserName))
            {
                return BadRequest();
            }

            dbContext.Students.Add(student);
            dbContext.SaveChanges(); 
            return Ok(dbContext.Students.ToList());
        }

        // CHECK IF EXIST USERNAME
        [Route("username")]
        [Route("username/{username?}")]
        public ActionResult<Student> GetUserName(string username)
        {

            if (StudentUserNameExists(username))
            {
                return Ok();
            }

            return NotFound();
        }

        // GET A STUDENT BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> Get(int id)
        {
            var student = await dbContext.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Student student)
        {
            if (id != student.Id)
            {
                return BadRequest();
            }


            dbContext.Entry(student).State = EntityState.Modified;

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE A STUDENT BY ID
        [HttpDelete("{id}")]
        public async Task<ActionResult<Student>> Delete(int id)
        {
            var student = await dbContext.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            dbContext.Students.Remove(student);
            await dbContext.SaveChangesAsync();

            return student;
        }

        private bool StudentExists(long id) =>
         dbContext.Students.Any(e => e.Id == id);

        private bool StudentUserNameExists(string username) =>
         dbContext.Students.Any(e => e.UserName == username);
    }
}
