using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Domain.Entities
{
    public class Seat
    {
        [Key]
        public Guid Id { get; set; }

        public Guid TheatreHallId { get; set; }

        //ex: Area A/B - if there are more then 1 areas
        public string Area { get; set; }

        //ex: Row 2
        public string Row { get; set; }

        //ex: Seat Number 4
        public string SeatNumber { get; set; }

        //status? -> Hot Spot -> higher price

        /*
         * When displayed for cashiers:
         *   - gray - available
         *   - green - selected
         *   - red - unavailable (only 1 seat left unoccupied)
         *   - ?blue? - already bought
         */
    }
}
