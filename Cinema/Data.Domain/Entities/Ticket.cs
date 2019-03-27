using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Domain.Entities
{
    public class Ticket
    {
        [Key]
        public Guid Id { get; set; }

        public Guid SeatId { get; set; }

        public Guid MovieId { get; set; }
        
        public bool Paid { get; set; }

        public bool Reserved { get; set; }

        //Which is null if the ticket is not reserved
        public Guid ReservationId { get; set; }

    }
}
