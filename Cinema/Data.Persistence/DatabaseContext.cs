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

        public DbSet<Location> Locations { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<TheatreHall> TheatreHalls { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<MovieGenre> MovieGenres { get; set; }
        public DbSet<MovieShowtime> MovieShowtimes { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
    }
}
