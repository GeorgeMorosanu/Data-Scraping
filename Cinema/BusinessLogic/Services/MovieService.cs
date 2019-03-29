using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Data.Domain.Entities;
using Data.Domain.Interfaces.Services;
using Data.Domain.Models.MovieModels;
using HtmlAgilityPack;

namespace BusinessLogic.Services
{
    public class MovieService : IMovieService
    {
        public MovieDataModel getMovieDataFromImdb(string movieUrl)
        {
            MovieDataModel result = new MovieDataModel()
            {
                Title = "",
                Duration = "",
                Summary = "",
                ReleaseDate = "",
                Restrictions = "12+",
                Rating = "",
                UrlToIMDB = movieUrl,
                UrlToTrailer = ""
            };

            var web = new HtmlWeb();
            var htmlDoc = web.Load(movieUrl);

            if (htmlDoc.DocumentNode != null && htmlDoc.ParseErrors != null && !htmlDoc.ParseErrors.Any())
            {
                //Title
                if (htmlDoc.DocumentNode.SelectNodes(
                        "//*[@id=\"title-overview-widget\"]/div[@class=\"vital\"]/div[@class=\"title_block\"]/div[@class=\"title_bar_wrapper\"]/div[@class=\"titleBar\"]/div[@class=\"title_wrapper\"]/h1"
                        ) != null)
                {
                    String title = htmlDoc.DocumentNode.SelectNodes(
                        "//*[@id=\"title-overview-widget\"]/div[@class=\"vital\"]/div[@class=\"title_block\"]/div[@class=\"title_bar_wrapper\"]/div[@class=\"titleBar\"]/div[@class=\"title_wrapper\"]/h1"
                        )[0].InnerText.ToString();
                    title = title.Substring(0, title.Length - 24);
                    result.Title = title;
                }


                //Genres => releaseDate
                if (htmlDoc.DocumentNode.SelectNodes(
                        "//*[@id=\"title-overview-widget\"]/div[@class=\"vital\"]/div[@class=\"title_block\"]/div[@class=\"title_bar_wrapper\"]/div[@class=\"titleBar\"]/div[@class=\"title_wrapper\"]/div[@class=\"subtext\"]/a"
                        ) != null)
                {
                    var genres = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"title-overview-widget\"]/div[@class=\"vital\"]/div[@class=\"title_block\"]/div[@class=\"title_bar_wrapper\"]/div[@class=\"titleBar\"]/div[@class=\"title_wrapper\"]/div[@class=\"subtext\"]/a");
                    List<string> ListOfGenresForGivenMovie = new List<string>();

                    foreach (var genre in genres)
                    {
                        ListOfGenresForGivenMovie.Add(genre.InnerText.ToString());
                    }

                    string releaseDate = ListOfGenresForGivenMovie[ListOfGenresForGivenMovie.Count - 1];

                    result.ReleaseDate = releaseDate;
                }
                
                //Duration
                string duration = "Unknown";
                string possibleDurationHTML = htmlDoc.DocumentNode.SelectNodes(
                    "//*[@id=\"title-overview-widget\"]/div[@class=\"vital\"]/div[@class=\"title_block\"]/div[@class=\"title_bar_wrapper\"]/div[@class=\"titleBar\"]/div[@class=\"title_wrapper\"]/div[@class=\"subtext\"]"
                    )[0].InnerHtml;
                string regexPattern = @"<time(.*?)<\/time>";
                Regex regex = new Regex(regexPattern, RegexOptions.Singleline);
                MatchCollection collection = regex.Matches(possibleDurationHTML);
                if (collection.Count > 0)
                {
                    duration = collection[0].Groups[1].Value;
                    duration = duration.Substring(duration.IndexOf('>') + 1).Trim();
                }
                result.Duration = duration;
                

                //Summary
                if (htmlDoc.DocumentNode.SelectNodes(
                        "//*[@id=\"title-overview-widget\"]/div[@class=\"plot_summary_wrapper\"]/div[@class=\"plot_summary \"]/div[@class=\"summary_text\"]"
                        ) != null)
                {
                    string summary = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"title-overview-widget\"]/div[@class=\"plot_summary_wrapper\"]/div[@class=\"plot_summary \"]/div[@class=\"summary_text\"]")[0].InnerText.Trim();
                    result.Summary = summary;
                }


                //Trailer
                if (htmlDoc.DocumentNode.SelectNodes(
                        "//*[@id=\"title-overview-widget\"]/div[@class=\"vital\"]/div[@class=\"slate_wrapper\"]/div[@class=\"slate\"]/a"
                        ) !=null)
                {
                    string urlToTrailer = "https://www.imdb.com" + htmlDoc.DocumentNode.SelectNodes("//*[@id=\"title-overview-widget\"]/div[@class=\"vital\"]/div[@class=\"slate_wrapper\"]/div[@class=\"slate\"]/a")[0].Attributes["href"].Value;
                    result.UrlToTrailer = urlToTrailer;
                }


                //Rating
                var rating = "Unknown";
                if (htmlDoc.DocumentNode.SelectNodes("//*[@id=\"title-overview-widget\"]/div[@class=\"vital\"]/div[@class=\"title_block\"]/div[@class=\"title_bar_wrapper\"]/div[@class=\"ratings_wrapper\"]/div[@class=\"imdbRating\"]/div[@class=\"ratingValue\"]") != null)
                {
                    rating = htmlDoc.DocumentNode.SelectNodes(
                    "//*[@id=\"title-overview-widget\"]/div[@class=\"vital\"]/div[@class=\"title_block\"]/div[@class=\"title_bar_wrapper\"]/div[@class=\"ratings_wrapper\"]/div[@class=\"imdbRating\"]/div[@class=\"ratingValue\"]")[0].InnerText;
                    result.Rating = rating;
                }
            }

            return result;
        }

        public List<string> getMovieGenre(string movieUrl)
        {
            List<string> result = new List<string>();

            var web = new HtmlWeb();
            var htmlDoc = web.Load(movieUrl);
            
            // getting the Genres
            if (htmlDoc.DocumentNode.SelectNodes(
                    "//*[@id=\"title-overview-widget\"]/div[@class=\"vital\"]/div[@class=\"title_block\"]/div[@class=\"title_bar_wrapper\"]/div[@class=\"titleBar\"]/div[@class=\"title_wrapper\"]/div[@class=\"subtext\"]/a"
                ) != null)
            {
                var genres = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"title-overview-widget\"]/div[@class=\"vital\"]/div[@class=\"title_block\"]/div[@class=\"title_bar_wrapper\"]/div[@class=\"titleBar\"]/div[@class=\"title_wrapper\"]/div[@class=\"subtext\"]/a");

                foreach (var genre in genres)
                {
                    result.Add(genre.InnerText.ToString());
                }
                //Removing the releaseDate field
                result.RemoveAt(result.Count - 1);
            }
           
            return result;
        }
    }
}
