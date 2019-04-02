using System;
using System.Collections.Generic;
using System.Text;
using Data.Domain.Entities;

namespace Data.Domain.Interfaces.Repositories
{
    public interface IMovieRepository
    {
        List<Movie> getMovieList();
        /* I need to transform the ReleaseDate from string to Datetime to compare them*/
        //List<Movie> getCurrentMovieList();
        //List<Movie> getUpcomingMovieList();
    }
}
