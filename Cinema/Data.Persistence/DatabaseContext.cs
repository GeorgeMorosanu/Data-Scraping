using Data.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Persistence
{
    public class DatabaseContext : DbContext, IDatabaseContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) 
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Data.Domain.Entities.Location> Locations { get; set; }
        public DbSet<Data.Domain.Entities.CinemaInfo> Cinemas { get; set; }
        public DbSet<Data.Domain.Entities.TheatreHall> TheatreHalls { get; set; }
        public DbSet<Data.Domain.Entities.Seat> Seats { get; set; }
        public DbSet<Data.Domain.Entities.Movie> Movies { get; set; }
        public DbSet<Data.Domain.Entities.Genre> Genres { get; set; }
        public DbSet<Data.Domain.Entities.MovieGenre> MovieGenres { get; set; }
        public DbSet<Data.Domain.Entities.MovieShowtime> MovieShowtimes { get; set; }
        public DbSet<Data.Domain.Entities.Ticket> Tickets { get; set; }
        public DbSet<Data.Domain.Entities.Reservation> Reservations { get; set; }
    }
}
