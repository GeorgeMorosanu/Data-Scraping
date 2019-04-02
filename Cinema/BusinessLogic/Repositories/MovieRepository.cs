using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Domain.Entities;
using Data.Domain.Interfaces.Repositories;
using Data.Persistence;

namespace BusinessLogic.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly DatabaseContext _dbContext;

        public MovieRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

     /*   public List<Movie> getCurrentMovieList()
        {
            return _dbContext.Movies.Where(x => x.ReleaseDate <= DateTime.Now).OrderBy(x=>x.Title).ToString;
        }
        */
        public List<Movie> getMovieList()
        {
            return _dbContext.Movies.OrderBy(x => x.Title).ToList();
        }

      /*  public List<Movie> getUpcomingMovieList()
        {
            return _dbContext.Movies.Where(x => x.ReleaseDate >= DateTime.Now).OrderBy(x => x.Title).ToString;
        }*/
    }
}
