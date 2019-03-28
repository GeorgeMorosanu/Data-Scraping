using System;
using System.Collections.Generic;
using System.Text;
using Data.Domain.Entities;
using Data.Domain.Models.CinemaModels;

namespace Data.Domain.Interfaces.Repositories
{
    public interface ICinemaRepository
    {
        List<CinemaData> getAllCinemas();

        Location getLocationOfCinema(Guid cinemaId);

        void addCinema(string street, string state, string postalCode);
    }
}
