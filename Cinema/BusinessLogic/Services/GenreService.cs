using Data.Domain.Interfaces.Services;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class GenreService : IGenreService
    {
        public List<string> getAllGenres()
        {
            List<string> result = new List<string>();


            string genreUrl = "https://www.imdb.com/feature/genre";

            var web = new HtmlWeb();
            var htmlDoc = web.Load(genreUrl);

            // getting the Genres
            foreach (var section in htmlDoc.DocumentNode.SelectNodes(
                "//*[@id=\"main\"]/div[6]/span/div[@class=\"ab_links\"]/div[@class=\"widget_content no_inline_blurb\"]/div[@class=\"widget_nested\"]/div[@class=\"full-table\"]/div[@class=\"table-cell\"]")
            )
            {
               
                var genres = section.ChildNodes.Nodes().ToList();

                foreach (var genre in genres)
                {
                   var selectedGenre = genre.InnerText.ToString().Trim();
                    if(selectedGenre != "" && selectedGenre != null)
                    result.Add(selectedGenre);
                }
            }

            return result;
        }
    }
}
