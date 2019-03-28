using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Domain.Entities
{
    public class TheatreHall
    {
        [Key]
        public Guid Id { get; set; }

        public Guid CinemaId { get; set; }

        //ex: Salur 1 / 2 / VIP
        public string Name { get; set; }

        //ex: 2 Areas, with one coridor in the middle
        public int NumberOfAreas { get; set; }

        public int NumberOfSeats { get; set; }
        
        //MaximumNumberOfSeatsReserved?
    }
}
