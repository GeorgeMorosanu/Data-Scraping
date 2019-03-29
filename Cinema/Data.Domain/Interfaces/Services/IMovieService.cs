using Data.Domain.Entities;
using Data.Domain.Models.MovieModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Domain.Interfaces.Services
{
    public interface IMovieService
    {
        MovieDataModel getMovieDataFromImdb(string movieUrl);
        List<string> getMovieGenre(string movieUrl);
    }
}
