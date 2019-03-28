using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Domain.Entities
{
    public class CinemaInfo
    {
        [Key]
        public Guid Id { get; set; }

        //Address
        public Guid LocationId { get; set; }

        //Openinghours?
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public int NumberOfHalls { get; set; }
        //fax?
    }
}
