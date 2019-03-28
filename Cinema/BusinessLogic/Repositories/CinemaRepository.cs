using Data.Domain.Entities;
using Data.Domain.Interfaces.Repositories;
using Data.Domain.Models;
using Data.Domain.Models.CinemaModels;
using Data.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic.Repositories
{
    public class CinemaRepository : ICinemaRepository
    {
        private readonly DatabaseContext _dbContext;

        public CinemaRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void addCinema(string street, string state, string postalCode)
        {
            Location newCinemaLocation = new Location()
            {
                Id= new Guid(),
                Street=street,
                State=state,
                PostalCode = postalCode
            };

            _dbContext.Locations.Add(newCinemaLocation);

            _dbContext.Cinemas.Add(
                new CinemaInfo()
                {
                    Id = new Guid(),
                    LocationId = newCinemaLocation.Id
                }
                );

            _dbContext.SaveChanges();
        }

        public List<CinemaData> getAllCinemas()
        {
            List<CinemaData> rez = (from cinema in _dbContext.Cinemas
                                    join location in _dbContext.Locations on cinema.LocationId equals location.Id
                                    select new CinemaData(){Cinema=cinema,CinemaLocation =location}).ToList();
            return rez;
            
        }

    public Location getLocationOfCinema(Guid cinemaId)
        {
            throw new NotImplementedException();
        }
    }
}
