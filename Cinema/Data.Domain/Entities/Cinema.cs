using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Domain.Entities
{
    public class Cinema
    {
        [Key]
        public Guid Id { get; set; }

        //Address
        public Guid LocationId { get; set; }

        //Openinghours?
    }
}
