using System.Collections.Generic;
using System.Linq;
using FIT_Api_Examples.Data;
using FIT_Api_Examples.Modul2.ViewModels;
using FIT_Api_Examples.Modul3_MaticnaKnjiga.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FIT_Api_Examples.Modul2.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class StudentController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public StudentController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }


        [HttpGet]
        public ActionResult<List<Student>> GetAll(string ime_prezime,string opstina)
        {
            var data = _dbContext.Student
                .Include(s => s.opstina_rodjenja.drzava)
                .Where(student =>
                (
                    ime_prezime == null || 
                    (student.ime + " " + student.prezime).ToLower().StartsWith(ime_prezime.ToLower()) ||
                    (student.prezime + " " + student.ime).ToLower().StartsWith(ime_prezime.ToLower())
                ) &&
                (
                    opstina == null || 
                    student.opstina_rodjenja.description.ToLower().StartsWith(opstina.ToLower()
                )
                ))
                .OrderByDescending(s => s.id)
                .AsQueryable();
            return data.Take(10).ToList();
        }

        [HttpPost]
        public ActionResult<Student> Upsert([FromBody] StudentAddVM studentVM)
        {
            if (studentVM.id == 0)
            {
                var newStudent = new Student();
                newStudent.ime = studentVM.ime;
                newStudent.prezime = studentVM.prezime;
                newStudent.opstina_rodjenja_id = studentVM.opstina_rodjenja_id;
                newStudent.lozinka = "test";
                _dbContext.Student.Add(newStudent);
                _dbContext.SaveChanges();
                return Ok(newStudent);
            }

            var studentToUpdate = _dbContext.Student.Find(studentVM.id);
            if (studentToUpdate == null)
                return BadRequest("Can't find user!");
            studentToUpdate.ime = studentVM.ime;
            studentToUpdate.prezime = studentVM.prezime;
            studentToUpdate.opstina_rodjenja_id = studentVM.opstina_rodjenja_id;
            _dbContext.Student.Update(studentToUpdate);
            _dbContext.SaveChanges();
            return Ok(studentToUpdate);
        }

        [HttpGet]
        public ActionResult<Student> GetById(int id)
        {
            if(id ==0)
                return BadRequest("Can't find user!");
            var student = _dbContext.Student.Find(id);
            return Ok(student);
        }

    }
}
