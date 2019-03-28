using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Domain.Entities
{
    public class MovieShowtime
    {
        [Key]
        public Guid Id { get; set; }

        // What movie?
        public Guid MovieId { get; set; }

        // Where?
        public Guid TheatreHallId { get; set; }

        // When?
        public DateTime StartTime { get; set; }

        public string Subtitle { get; set; }
        
        public string Language { get; set; }
    }
}
