using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Domain.Entities
{
    public class Reservation
    {
        [Key]
        public Guid Id { get; set; }

        //Phone Number / Email
        public string Contact { get; set; }

        //NoSeatsReserved -> get the list through a service
    }
}
