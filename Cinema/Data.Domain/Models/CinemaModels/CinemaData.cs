using Data.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Domain.Models.CinemaModels
{
    public class CinemaData
    {
        public CinemaInfo Cinema { get; set; }
        public Location CinemaLocation { get; set; }
    }
}
