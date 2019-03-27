using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Domain.Entities
{
    public class MovieGenre
    {
        [Key]
        public Guid Id { get; set; }

        public Guid MovieId { get; set; }

        public Guid GenreId { get; set; }
    }
}
