using Data.Domain.Interfaces.Repositories;
using Data.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly DatabaseContext _dbContext;

        public GenreRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Guid getGenreIdByGenre(string genreName)
        {
            return _dbContext.Genres.Where(x => x.Name == genreName).Select(x => x.Id).FirstOrDefault();
        }
    }
}
