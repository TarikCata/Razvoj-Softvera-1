using FIT_Api_Examples.Modul3_MaticnaKnjiga.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIT_Api_Examples.Modul2.Models
{
    [Table("UpisanaGodina")]
    public class UpisanaGodina
    {
        [Key]
        public int Id { get; set; }
        public DateTime DatumUpisa { get; set; }
        public int GodinaStudija { get; set; }
        [ForeignKey("StudentId")]
        public int? StudentId { get; set; }
        public Student Student { get; set; }
        [ForeignKey("AkademskaId")]
        public int? AkademskaId { get; set; }
        public AkademskaGodina AkademskaGodina { get; set; }
        public float CijenaSkolarine { get; set; }
        public bool Obnova { get; set; }
        public DateTime DatumOvjere { get; set; }
        public string NapomenaZaOvjeru { get; set; }

    }
}
