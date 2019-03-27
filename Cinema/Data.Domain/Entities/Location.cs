using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Domain.Entities
{
    public class Location
    {
        [Key]
        public Guid Id { get; set; }

        public string Street { get; set; }

        //or City -> Reykjavik/Akureyri
        public string State { get; set; }

        public string PostalCode { get; set; }

}
}
