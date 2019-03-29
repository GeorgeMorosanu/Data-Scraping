using Cinema.Areas.Identity.User;
using Data.Domain.Entities;
using Data.Persistence;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Cinema.Data
{
    public static class MyIdentityDataInitializer
    {
        public static void SeedData(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, DatabaseContext dbContext)
        {
          /*
            SeedRoles(roleManager);

            SeedUsers(userManager);

            SeedDatabase_CinemasAndLocations(dbContext);

            SeedDatabase_Genres(dbContext);

            SeedDatabase_TheatreHalls(dbContext);
            */

            SeedDatabase_Movies(dbContext);
        }

        public static void SeedDatabase_Movies(DatabaseContext dbContext)
        {
            //avengers, captain marvel, spiderman, once upon a time
            List<string> ListOfMovieIMDBURL = new List<string>()
            {
                "https://www.imdb.com/title/tt4154796/?ref_=cs_ov_tt",
                "https://www.imdb.com/title/tt4154664/?ref_=nv_sr_1",
                "https://www.imdb.com/title/tt4633694/?ref_=nv_sr_1",
                "https://www.imdb.com/title/tt7131622/?ref_=adv_li_tt",
                "https://www.imdb.com/title/tt3861390/?ref_=inth_ov_i"

            };
            
            foreach (var movieURL in ListOfMovieIMDBURL)
            {
                // The code below must be moved into a Service/Repository Function - getMovieDataFromIMDB(string movieURLtoIMDB)
                var teamList = new List<string>();
                var web = new HtmlWeb();
                var htmlDoc = web.Load(movieURL);
                if (htmlDoc.DocumentNode != null && htmlDoc.ParseErrors != null && !htmlDoc.ParseErrors.Any())
                {
                    String title = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"title-overview-widget\"]/div[@class=\"vital\"]/div[@class=\"title_block\"]/div[@class=\"title_bar_wrapper\"]/div[@class=\"titleBar\"]/div[@class=\"title_wrapper\"]/h1")[0].InnerText.ToString();
                    title = title.Substring(0, title.Length - 24);
                    
                    var genres = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"title-overview-widget\"]/div[@class=\"vital\"]/div[@class=\"title_block\"]/div[@class=\"title_bar_wrapper\"]/div[@class=\"titleBar\"]/div[@class=\"title_wrapper\"]/div[@class=\"subtext\"]/a");
                    List<string> ListOfGenresForGivenMovie = new List<string>();

                    foreach (var genre in genres)
                    {
                        ListOfGenresForGivenMovie.Add(genre.InnerText.ToString());
                    }

                    string releaseDate = ListOfGenresForGivenMovie[ListOfGenresForGivenMovie.Count - 1];
                    
                    string duration = "Unknown";
                    var possibleDuration = htmlDoc.DocumentNode.SelectNodes(
                        "//*[@id=\"title-overview-widget\"]/div[@class=\"vital\"]/div[@class=\"title_block\"]/div[@class=\"title_bar_wrapper\"]/div[@class=\"titleBar\"]/div[@class=\"title_wrapper\"]/div[@class=\"subtext\"]")[0].InnerHtml;
                    string regexPattern = @"<time(.*?)<\/time>";
                    Regex regex = new Regex(regexPattern, RegexOptions.Singleline);
                    MatchCollection collection = regex.Matches(possibleDuration);
                    if (collection.Count > 0)
                    {
                        duration = collection[0].Groups[1].Value;
                        duration = duration.Substring(duration.IndexOf('>')+1).Trim();
                    }
                   
                   
                    var summary = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"title-overview-widget\"]/div[@class=\"plot_summary_wrapper\"]/div[@class=\"plot_summary \"]/div[@class=\"summary_text\"]")[0].InnerText.Trim();

                    string restrictions = "12+";
                    string UrlToIMDB = movieURL;
                    var UrlToTrailer = "https://www.imdb.com"+htmlDoc.DocumentNode.SelectNodes("//*[@id=\"title-overview-widget\"]/div[@class=\"vital\"]/div[@class=\"slate_wrapper\"]/div[@class=\"slate\"]/a")[0].Attributes["href"].Value;
                    var rating = "Unknown";

                    if (htmlDoc.DocumentNode.SelectNodes( "//*[@id=\"title-overview-widget\"]/div[@class=\"vital\"]/div[@class=\"title_block\"]/div[@class=\"title_bar_wrapper\"]/div[@class=\"ratings_wrapper\"]/div[@class=\"imdbRating\"]/div[@class=\"ratingValue\"]") != null)
                    {
                        rating = rating = htmlDoc.DocumentNode.SelectNodes(
                        "//*[@id=\"title-overview-widget\"]/div[@class=\"vital\"]/div[@class=\"title_block\"]/div[@class=\"title_bar_wrapper\"]/div[@class=\"ratings_wrapper\"]/div[@class=\"imdbRating\"]/div[@class=\"ratingValue\"]")[0].InnerHtml;

                    }
                    Movie newMovie = new Movie()
                    {
                        Id = Guid.NewGuid(),
                        Title = title,
                        Duration = duration,
                        Summary = summary,
                        ReleaseDate = releaseDate,
                        Restrictions = restrictions,
                        UrlToIMDB = movieURL,
                        UrlToTrailer = UrlToTrailer,
                        Rating = rating
                    };
                    dbContext.Movies.Add(newMovie);
                    dbContext.SaveChanges();

                    ListOfGenresForGivenMovie.RemoveAt(ListOfGenresForGivenMovie.Count-1);
                    
                    foreach (var x in ListOfGenresForGivenMovie)
                    {
                        // add rows in MovieGenre
                    }
                }

            }
        }
        /*
         * MovieShowtime show = new MovieShowtime()
                            {
                                Id = Guid.NewGuid(),
                                MovieId= Guid.NewGuid(),
                                TheatreHallId=Guid.NewGuid(),
                                StartTime=DateTime.Now,
                                Subtitle = genre.InnerText.ToString(),
                                Language=""
                            };
                            dbContext.MovieShowtimes.Add(show);
                            dbContext.SaveChanges();
         */
        public static void SeedDatabase_TheatreHalls(DatabaseContext dbContext)
        {
            string nameOfTheHall = "";
            TheatreHall newTheatreHall = new TheatreHall();

            foreach (var cinema in dbContext.Cinemas.ToList())
            {
                for (int i = 1; i <= cinema.NumberOfHalls; i++)
                {
                    if (i == cinema.NumberOfHalls)
                    {
                        nameOfTheHall = "VIP";
                    }
                    else
                    {
                        nameOfTheHall = "Hall" + i;
                    }

                    newTheatreHall = new TheatreHall()
                    {
                        Id = Guid.NewGuid(),
                        CinemaId = cinema.Id,
                        Name = nameOfTheHall,
                        NumberOfAreas = 1,
                        NumberOfSeats = 500
                    };
                    
                    if (dbContext.TheatreHalls.Where(x => (x.CinemaId == newTheatreHall.CinemaId)&&(x.Name == nameOfTheHall)).ToList().Count==0)
                    {
                        dbContext.TheatreHalls.Add(newTheatreHall);
                        dbContext.SaveChanges();
                    }
                }
            }
        }

        public static void SeedDatabase_Genres(DatabaseContext dbContext)
        {
            List<string> genreList = new List<string>()
            {
                "Action",
                "Adventure",
                "Animation",
                "Biography",
                "Comedy",
                "Crime",
                "Documentary",
                "Drama",
                "Family",
                "Fantasy",
                "Film Noir",
                "History",
                "Horror",
                "Musical",
                "Mystery",
                "Romance",
                "Sci-Fi",
                "Short",
                "Sport",
                "Superhero",
                "Thriller",
                "War",
                "Western"
            };

            Genre newGenre = new Genre();
            foreach (var genre in genreList)
            {
                newGenre = new Genre()
                {
                    Id = Guid.NewGuid(),
                    Name = genre
                };
                if (dbContext.Genres.Where(x => x.Name == newGenre.Name).Count() == 0)
                {
                    dbContext.Genres.Add(newGenre);
                }
            }
            
            dbContext.SaveChanges();
        }

        public static void SeedDatabase_CinemasAndLocations(DatabaseContext dbContext)
        {
            // Cinema&Location 1
            Guid locationId = Guid.NewGuid();
            Location newLocation = new Location()
            {
                Id = locationId,
                Street = "Álfabakki 8",
                State = "Reykjavík",
                PostalCode = "109"
            };
            if (dbContext.Locations.Where(x => x.Street == newLocation.Street).ToList().Count == 0)
            {
                dbContext.Locations.Add(newLocation);
                dbContext.SaveChanges();
            }
            CinemaInfo newCinema = new CinemaInfo()
            {
                Id = Guid.NewGuid(),
                LocationId = dbContext.Locations.Where(x => x.Street == newLocation.Street).ToList()[0].Id,
                Email = "alfabakki@samfilm.is",
                PhoneNumber = "575 8900",
                NumberOfHalls = 6
            };
            if (dbContext.Cinemas.Where(x => x.Email == newCinema.Email).ToList().Count == 0)
            {
                dbContext.Cinemas.Add(newCinema);
                dbContext.SaveChanges();
            }

            //Cinema&Location 2
            locationId = Guid.NewGuid();
            newLocation = new Location()
            {
                Id = locationId,
                Street = "Fossaleyni 1",
                State = "Reykjavík",
                PostalCode = "112"
            };
            if (dbContext.Locations.Where(x => x.Street == newLocation.Street).ToList().Count == 0)
            {
                dbContext.Locations.Add(newLocation);
                dbContext.SaveChanges();
            }
            newCinema = new CinemaInfo()
            {
                Id = Guid.NewGuid(),
                LocationId = dbContext.Locations.Where(x => x.Street == newLocation.Street).ToList()[0].Id,
                Email = "egilsholl@samfilm.is",
                PhoneNumber = "575 8900",
                NumberOfHalls = 4
            };
            if (dbContext.Cinemas.Where(x => x.Email == newCinema.Email).ToList().Count == 0)
            {
                dbContext.Cinemas.Add(newCinema);
                dbContext.SaveChanges();
            }


            //Cinema&Location 3
            locationId = Guid.NewGuid();
            newLocation = new Location()
            {
                Id = locationId,
                Street = "Kringlan 4-6",
                State = "Reykjavík",
                PostalCode = "103"
            };
            if (dbContext.Locations.Where(x => x.Street == newLocation.Street).ToList().Count == 0)
            {
                dbContext.Locations.Add(newLocation);
                dbContext.SaveChanges();
            }
            newCinema = new CinemaInfo()
            {
                Id = Guid.NewGuid(),
                LocationId = dbContext.Locations.Where(x => x.Street == newLocation.Street).ToList()[0].Id,
                Email = "kringlan@samfilm.is",
                PhoneNumber = "575 8900",
                NumberOfHalls = 3
            };
            if (dbContext.Cinemas.Where(x => x.Email == newCinema.Email).ToList().Count == 0)
            {
                dbContext.Cinemas.Add(newCinema);
                dbContext.SaveChanges();
            }

            //Cinema&Location 4
            locationId = Guid.NewGuid();
            newLocation = new Location()
            {
                Id = locationId,
                Street = "Ráðhústorg 8",
                State = "Akureyri",
                PostalCode = "600"
            };
            if (dbContext.Locations.Where(x => x.Street == newLocation.Street).ToList().Count == 0)
            {
                dbContext.Locations.Add(newLocation);
                dbContext.SaveChanges();
            }
            newCinema = new CinemaInfo()
            {
                Id = Guid.NewGuid(),
                LocationId = dbContext.Locations.Where(x=>x.Street == newLocation.Street).ToList()[0].Id,
                Email = "midasala.ak@samfilm.is",
                PhoneNumber = "575 8900",
                NumberOfHalls = 2
            };
            if (dbContext.Cinemas.Where(x => x.Email == newCinema.Email).ToList().Count == 0)
            {
                dbContext.Cinemas.Add(newCinema);
                dbContext.SaveChanges();
            }

            
            //Cinema&Location 5
            locationId = Guid.NewGuid();
            newLocation = new Location()
            {
                Id = locationId,
                Street = "Hafnargata 33",
                State = "Keflavík",
                PostalCode = "235"
            };
            if (dbContext.Locations.Where(x => x.Street == newLocation.Street).ToList().Count == 0)
            {
                dbContext.Locations.Add(newLocation);
                dbContext.SaveChanges();
            }
            newCinema = new CinemaInfo()
            {
                Id = Guid.NewGuid(),
                LocationId = dbContext.Locations.Where(x => x.Street == newLocation.Street).ToList()[0].Id,
                Email = "keflavik@samfilm.is",
                PhoneNumber = "575 8900",
                NumberOfHalls = 2
            };
            if (dbContext.Cinemas.Where(x => x.Email == newCinema.Email).ToList().Count == 0)
            {
                dbContext.Cinemas.Add(newCinema);
                dbContext.SaveChanges();
            }

            dbContext.SaveChanges();
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            roleManager.CreateRole("Admin"); // Administrators/Moderators
            roleManager.CreateRole("Worker");  // ex: cashier
            roleManager.CreateRole("Customer");
        }

        public static void CreateRole(this RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (roleManager.RoleExistsAsync(roleName).Result)
            {
                return;
            }

            var role = new IdentityRole
            {
                Name = roleName
            };

            var roleResult = roleManager.CreateAsync(role).Result;

            if (!roleResult.Succeeded)
            {
                throw new Exception("Error creating role");
            }
        }

        public static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            //Admin
            string mail = "admin@gmail.com";
            if (userManager.FindByNameAsync(mail).Result == null)
            {
                var user = new ApplicationUser()
                {
                    Kennitala = "1700969999",
                    UserName = mail,
                    FirstName = "Gado",
                    LastName = "Admin",
                    BirthDate = new DateTime(1996, 09, 17),
                    Email = mail,
                    PhoneNumber = "7772181",
                    CreatedDate = DateTime.Now
                };

                IdentityResult result = userManager.CreateAsync(user, "Admin007!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }

            //Curstomers
            mail = "georgecosminmorosanu@gmail.com";
            if (userManager.FindByNameAsync(mail).Result == null)
            {
                var user = new ApplicationUser()
                {
                    Kennitala = "1700965269",
                    UserName = mail,
                    FirstName = "George",
                    LastName = "Morosanu",
                    BirthDate = new DateTime(1996, 09, 17),
                    Email = mail,
                    PhoneNumber = "0750000000",
                    CreatedDate = DateTime.Now
                };

                IdentityResult result = userManager.CreateAsync(user, "Customer007!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Customer").Wait();
                }
            }

        }
    }
}
