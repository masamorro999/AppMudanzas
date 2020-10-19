using System;
using System.ComponentModel.DataAnnotations;

namespace TNSEjecutor.Common.Entities
{
    public class Ejecutor
    {
        public int Id { get; set; }

        //[MaxLength(15)]
        [Required]
        public int Document { get; set; }
        public string TransacDate { get; set; }
        public string NWorkTrips { get; set; }
    }
}
