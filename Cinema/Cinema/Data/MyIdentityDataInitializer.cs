using Cinema.Areas.Identity.User;
using Data.Domain.Entities;
using Data.Persistence;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Data.Domain.Interfaces.Repositories;
using Data.Domain.Interfaces.Services;
using Data.Domain.Models.MovieModels;

namespace Cinema.Data
{
    public static class MyIdentityDataInitializer
    {
        public static void SeedData(
            RoleManager<IdentityRole> roleManager, 
            UserManager<ApplicationUser> userManager, 
            DatabaseContext dbContext, 
            IMovieService movieService, 
            IGenreRepository genreRepository,
            IGenreService genreService)
        {
          
            SeedRoles(roleManager);

            SeedUsers(userManager);

            SeedDatabase_CinemasAndLocations(dbContext);

            SeedDatabase_Genres(dbContext, genreService);

            SeedDatabase_TheatreHalls(dbContext);
            
            SeedDatabase_Movies(dbContext, movieService, genreRepository);

            /* This will be commented after one run */
            //SeedDatabase_MovieShowtimes(dbContext);
        }

        public static void SeedDatabase_MovieShowtimes(DatabaseContext dbContext)
        {
            Random rnd = new Random(); //random movie
            int numberOfMovies = dbContext.Movies.Count();
            int randomNumber = 0;

            var newMovieShowtime = new MovieShowtime();
            List<Guid> cinemaIds = dbContext.Cinemas.Select(x => x.Id).ToList();
            foreach (var cinemaId in cinemaIds)
            {
                foreach (var theatreHallId in dbContext.TheatreHalls.Where(x => x.CinemaId == cinemaId).Select(x => x.Id).ToList())
                {
                    //foreach (var movieId in dbContext.Movies.Select(x => x.Id))
                    //{
                    randomNumber = rnd.Next(numberOfMovies);

                    var movieId = dbContext.Movies.ToList()[randomNumber].Id;
                        newMovieShowtime = new MovieShowtime()
                        {
                            Id = Guid.NewGuid(),
                            TheatreHallId = theatreHallId,
                            MovieId = movieId,
                            StartTime=DateTime.Now.AddDays(1),
                            Subtitle = "Icelandic",
                            Language = "English"
                        };
                        dbContext.MovieShowtimes.Add(newMovieShowtime);
                    //}
                }
            }
            dbContext.SaveChanges();
        }

        public static void SeedDatabase_Movies(DatabaseContext dbContext, IMovieService movieService, IGenreRepository genreRepository)
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
            
            foreach (var movieUrl in ListOfMovieIMDBURL)
            {
                if (dbContext.Movies.Where(x => x.UrlToIMDB == movieUrl).Count() == 0)
                {
                    // The code below must be moved into a Service/Repository Function - getMovieDataFromIMDB(string movieURLtoIMDB)
                    MovieDataModel getDataFromIMDB = movieService.getMovieDataFromImdb(movieUrl);

                    Movie newMovie = new Movie()
                    {
                        Id = Guid.NewGuid(),
                        Title = getDataFromIMDB.Title,
                        Duration = getDataFromIMDB.Duration,
                        Summary = getDataFromIMDB.Summary,
                        ReleaseDate = getDataFromIMDB.ReleaseDate,
                        Restrictions = getDataFromIMDB.Restrictions,
                        UrlToIMDB = getDataFromIMDB.UrlToIMDB,
                        UrlToTrailer = getDataFromIMDB.UrlToTrailer,
                        Rating = getDataFromIMDB.Rating
                    };

                    // Adding the Movie
                    dbContext.Movies.Add(newMovie);
                    dbContext.SaveChanges();

                    List<string> genresForSelectedMovie = movieService.getMovieGenre(movieUrl);
                    foreach (string genre in genresForSelectedMovie)
                    {
                        MovieGenre newMovieGenre = new MovieGenre()
                        {
                            Id = Guid.NewGuid(),
                            MovieId = newMovie.Id,
                            GenreId = genreRepository.getGenreIdByGenre(genre)
                        };
                        dbContext.MovieGenres.Add(newMovieGenre);
                        dbContext.SaveChanges();
                    }
                }

            }
        }
       
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

        public static void SeedDatabase_Genres(DatabaseContext dbContext, IGenreService genreService)
        {
            List<string> genreList = genreService.getAllGenres();

            Genre newGenre = new Genre();
            foreach (var genre in genreList)
            {
                newGenre = new Genre()
                {
                    Id = Guid.NewGuid(),
                    Name = genre
                };
                if (dbContext.Genres.Where(x => x.Name == genre).Count() == 0)
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
