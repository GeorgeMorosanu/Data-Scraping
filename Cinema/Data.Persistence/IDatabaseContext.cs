using Data.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Persistence
{
    public interface IDatabaseContext
    {
        DbSet<Location> Locations { get; set; }

        DbSet<Cinema> Cinemas { get; set; }

        DbSet<TheatreHall> TheatreHalls { get; set; }

        DbSet<Seat> Seats { get; set; }

        DbSet<Movie> Movies { get; set; }

        DbSet<Genre> Genres { get; set; }

        DbSet<MovieGenre> MovieGenres { get; set; }

        DbSet<MovieShowtime> MovieShowtimes { get; set; }

        DbSet<Ticket> Tickets { get; set; }

        /*
         * Locations (of Cinemas)                                          V
         * Cinemas                                                         V
         * TheatreHalls (Salur) - CinemaId, Name, NoAreas                  V
         * Seats - TheatreHallId,                                          V
         * MovieShowtime - MovieId, Startingtime, TheaterHallId, Language  V
         * Movie - LinkToIMDB, ...                                         V
         * Genre                                                           V
         * MovieGenre                                                      V
         * Tickets                                                         V
         * ?Reservation?
         *
         * Offerts (Menus..)
         * Rewards (for Users)
         */

    }
}
