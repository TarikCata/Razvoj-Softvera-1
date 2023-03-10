using FIT_Api_Examples.Data;
using FIT_Api_Examples.Helper.AutentifikacijaAutorizacija;
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
    public class GodinaController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public GodinaController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        [HttpGet("{id}")]
        public ActionResult<List<Godina>> GetAll(int id)
        {
            var data = _dbContext.Godina.Include(g => g.Student)
                                         .Include(g => g.AkademskaGodina)
                                         .Include(g => g.evidentiraoKorisnik)
                                         .Where(g => g.studentId == id)
                                         .ToList();
            return Ok(data);
        }

        [HttpPost]
        public ActionResult<Godina> Add([FromBody] AddGodinaVm vm)
        {
            var godina = new Godina();
            var student = _dbContext.Student.Find(vm.studentId);
            var akGodina = _dbContext.AkademskaGodina.Find(vm.akademskaId);
            var korisnickiNalog = HttpContext.GetLoginInfo().korisnickiNalog;
            if (student == null || akGodina == null || korisnickiNalog == null)
                return BadRequest("Greska kod dodavanja godine!");

            var istaGodina = _dbContext.Godina.
                Where(godina =>
                      godina.studentId == vm.studentId && 
                      godina.GodinaStudija == vm.GodinaStudija).FirstOrDefault();


            if (istaGodina != null && !istaGodina.Obnova && !vm.Obnova)
                return BadRequest("Ne moze upisati istu godinu a da nije obnova");

            godina.Obnova = vm.Obnova;
            godina.GodinaStudija = vm.GodinaStudija;
            godina.Cijena = vm.Cijena;
            godina.studentId = vm.studentId;
            godina.Student = student;
            godina.akademskaId = vm.akademskaId;
            godina.AkademskaGodina = akGodina;
            godina.evidentiraoKorisnik = korisnickiNalog;
            godina.DatumUpisa = (DateTime)(vm.DatumUpisa == null ? DateTime.Now : vm.DatumUpisa);
            _dbContext.Godina.Add(godina);
            _dbContext.SaveChanges();
            return Ok(godina);
        }

        [HttpPut]
        public ActionResult<Godina> Ovjera([FromBody] OvjeraVM vm)
        {
            var godina = _dbContext.Godina.Find(vm.Id);
            godina.DatumOvjere = (DateTime)(vm.DatumOvjere == null ? DateTime.Now : vm.DatumOvjere);
            godina.Napomena = vm.Napomena;
            _dbContext.Godina.Update(godina);
            _dbContext.SaveChanges();
            return Ok(godina);
        }

    }
}
