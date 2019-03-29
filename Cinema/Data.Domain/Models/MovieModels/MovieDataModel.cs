using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Domain.Models.MovieModels
{
    public class MovieDataModel
    {
        public string Title { get; set; }

        public string Duration { get; set; }  //ex: 1h 51mins

        public string Summary { get; set; }

        public string ReleaseDate { get; set; }
        //ex: 18+
        public string Restrictions { get; set; }


        public string UrlToIMDB { get; set; }

        public string UrlToTrailer { get; set; }

        public string Rating { get; set; }
    }
}
