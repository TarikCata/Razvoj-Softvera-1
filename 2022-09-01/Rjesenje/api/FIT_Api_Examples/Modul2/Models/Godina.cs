using FIT_Api_Examples.Modul0_Autentifikacija.Models;
using FIT_Api_Examples.Modul4_MaticnaKnjiga.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIT_Api_Examples.Modul2.Models
{
    public class Godina
    {
        [Key]
        public int Id { get; set; }
        public int GodinaStudija { get; set; }
        public float Cijena { get; set; }
        public bool Obnova { get; set; }
        public DateTime DatumUpisa { get; set; }
        public DateTime DatumOvjere { get; set; }
        public string Napomena { get; set; }
        [ForeignKey("akademskaId")]
        public int akademskaId { get; set; }
        public AkademskaGodina AkademskaGodina { get; set; }
        [ForeignKey("studentId")]
        public int studentId { get; set; }
        public Student Student { get; set; }

        [ForeignKey(nameof(evidentiraoKorisnik))]
        public int? evidentiraoKorisnikId { get; set; }
        public KorisnickiNalog? evidentiraoKorisnik { get; set; }

    }
}
