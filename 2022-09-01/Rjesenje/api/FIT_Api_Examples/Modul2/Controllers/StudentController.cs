using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FIT_Api_Examples.Data;
using FIT_Api_Examples.Helper;
using FIT_Api_Examples.Helper.AutentifikacijaAutorizacija;
using FIT_Api_Examples.Modul0_Autentifikacija.Models;
using FIT_Api_Examples.Modul2.Models;
using FIT_Api_Examples.Modul2.ViewModels;
using FIT_Api_Examples.Modul3.Models;
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

        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            if (!HttpContext.GetLoginInfo().isLogiran)
                return BadRequest("You are not logged in!");

            return Ok(_dbContext.Student.Include(s => s.opstina_rodjenja.drzava).FirstOrDefault(s => s.id == id)); ;
        }

        [HttpGet]
        public ActionResult<List<Student>> GetAll(string ime_prezime,string opstina)
        {
            var data = _dbContext.Student
                .Include(s => s.opstina_rodjenja.drzava)
                .Where(student =>(
                        ime_prezime == null || 
                        (student.ime + " " + student.prezime).ToLower().StartsWith(ime_prezime.ToLower()) ||
                        (student.prezime + " " + student.ime).ToLower().StartsWith(ime_prezime.ToLower())
                ) && (opstina == null || (student.opstina_rodjenja.description).ToLower().StartsWith(opstina.ToLower()))
                ).OrderByDescending(s => s.id)
                .AsQueryable();
            return data.Take(10).ToList();
        }

        [HttpPost]
        public ActionResult<Student> Upsert([FromBody] StudentAddVM vm)
        {
           var opstina = _dbContext.Opstina.Find(vm.opstina_rodjenja_id);
           if (vm.Id == 0)
           {
                var noviStudent = new Student();
                noviStudent.ime = vm.ime;
                noviStudent.broj_indeksa = vm.broj_indeksa;
                noviStudent.prezime = vm.prezime;
                noviStudent.opstina_rodjenja_id = vm.opstina_rodjenja_id;
                noviStudent.opstina_rodjenja = opstina;
                _dbContext.Student.Add(noviStudent);
                _dbContext.SaveChanges();
                return Ok(noviStudent);
            }
            var student = _dbContext.Student.Find(vm.Id);
            student.ime = vm.ime;
            student.broj_indeksa = vm.broj_indeksa;
            student.prezime = vm.prezime;
            student.opstina_rodjenja_id = vm.opstina_rodjenja_id;
            student.opstina_rodjenja = opstina;
           _dbContext.Student.Update(student);
            _dbContext.SaveChanges();
           return Ok(student);
        }

        [HttpDelete("{id}")]
        public ActionResult<bool> Delete(int id)
        {
            var student = _dbContext.Student.Find(id);
            if (student == null)
                return BadRequest("Can't find student!");
            _dbContext.Student.Remove(student);
            _dbContext.SaveChanges();
            return Ok(true);
        }

    }
}
                