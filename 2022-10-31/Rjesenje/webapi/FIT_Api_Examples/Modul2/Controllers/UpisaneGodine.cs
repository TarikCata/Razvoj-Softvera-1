using FIT_Api_Examples.Data;
using FIT_Api_Examples.Modul2.Models;
using FIT_Api_Examples.Modul2.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIT_Api_Examples.Modul2.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UpisaneGodine : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public UpisaneGodine(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        [HttpPost]
        public ActionResult<UpisanaGodina> Add([FromBody] UpisanaGodinaVM upisGodineVM)
        {
            var thisGodina = new UpisanaGodina();
            var akademskaGodina = _dbContext.AkademskaGodina.Find(upisGodineVM.AkademskaGodinaId);
            var student = _dbContext.Student.Find(upisGodineVM.StudentId);
            thisGodina.Student = student;
            thisGodina.GodinaStudija = upisGodineVM.GodinaStudija;
            thisGodina.AkademskaGodina = akademskaGodina;
            thisGodina.CijenaSkolarine = upisGodineVM.CijenaSkolarine;
            thisGodina.Obnova = upisGodineVM.Obnova;
            thisGodina.DatumUpisa = DateTime.Now;
            _dbContext.UpisanaGodina.Add(thisGodina);
            _dbContext.SaveChanges();
            return Ok(thisGodina);
        }

        [HttpGet]
        public ActionResult<List<UpisanaGodina>> GetAll(int studentId)
        {
            var godine = _dbContext.UpisanaGodina
                .Include(g => g.AkademskaGodina)
                .Include(g => g.Student)
                .Where(g => studentId == 0 || g.StudentId == studentId)
                .ToList();
            return Ok(godine);
        }
        
        [HttpPut]
        public ActionResult<List<UpisanaGodina>> Ovjeri([FromBody] OvjeriGodinuVM vm)
        {
            var thisGodina = _dbContext.UpisanaGodina.Find(vm.Id);
            if (thisGodina == null)
                return BadRequest("Can't find year!");
            thisGodina.NapomenaZaOvjeru = vm.Napomena;
            thisGodina.DatumOvjere = vm.DatumOvjere;
            _dbContext.UpisanaGodina.Update(thisGodina);
            _dbContext.SaveChanges();
            return Ok(thisGodina);
        }
    }
}
