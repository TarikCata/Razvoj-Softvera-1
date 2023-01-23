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
            thisGodina.GodinaStudija = upisGodineVM.GodinaStudija;
            thisGodina.AkademskaGodina = akademskaGodina;
            thisGodina.CijenaSkolarine = upisGodineVM.CijenaSkolarine;
            thisGodina.Obnova = upisGodineVM.Obnova;
            thisGodina.NapomenaZaOvjeru = upisGodineVM?.NapomenaZaOvjeru;
            thisGodina.DatumOvjere = (DateTime)upisGodineVM.DatumOvjere;
            thisGodina.DatumUpisa = DateTime.Now;
            _dbContext.UpisanaGodina.Add(thisGodina);
            _dbContext.SaveChanges();
            return Ok(thisGodina);
        }

        [HttpGet]
        public ActionResult<List<UpisanaGodina>> GetAll()
        {
            return Ok(_dbContext.UpisanaGodina.Include(g => g.AkademskaGodina).ToList());
        }
    }
}
