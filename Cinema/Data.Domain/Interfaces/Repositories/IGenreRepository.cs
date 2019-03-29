using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Domain.Interfaces.Repositories
{
    public interface IGenreRepository
    {
        Guid getGenreIdByGenre(string genreName);
    }
}
