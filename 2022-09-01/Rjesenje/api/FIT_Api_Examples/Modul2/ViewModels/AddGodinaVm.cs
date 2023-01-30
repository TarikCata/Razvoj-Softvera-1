using System;

namespace FIT_Api_Examples.Modul2.ViewModels
{
    public class AddGodinaVm
    {
        public int GodinaStudija { get; set; }
        public float Cijena { get; set; }
        public bool Obnova { get; set; }
        public DateTime? DatumUpisa { get; set; }
        public int akademskaId { get; set; }
        public int studentId { get; set; }
    }
}
