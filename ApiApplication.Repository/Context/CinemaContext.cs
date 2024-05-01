using ApiApplication.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiApplication.Repository.Context
{
    internal class CinemaContext : DbContext
    {
        public CinemaContext(DbContextOptions<CinemaContext> options) : base(options)
        {

        }

        public DbSet<AuditoriumEntity> Auditoriums
        {
            get; set;
        }
        public DbSet<ShowtimeEntity> Showtimes
        {
            get; set;
        }
        public DbSet<MovieEntity> Movies
        {
            get; set;
        }
        public DbSet<TicketEntity> Tickets
        {
            get; set;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _ = modelBuilder.Entity<AuditoriumEntity>(build =>
            {
                _ = build.HasKey(entry => entry.Id);
                _ = build.Property(entry => entry.Id).ValueGeneratedOnAdd();
                _ = build.HasMany(entry => entry.Showtimes).WithOne().HasForeignKey(entity => entity.AuditoriumId);
            });

            _ = modelBuilder.Entity<SeatEntity>(build =>
            {
                _ = build.HasKey(entry => new { entry.AuditoriumId, entry.Row, entry.SeatNumber });
                _ = build.HasOne(entry => entry.Auditorium).WithMany(entry => entry.Seats).HasForeignKey(entry => entry.AuditoriumId);
            });

            _ = modelBuilder.Entity<ShowtimeEntity>(build =>
            {
                _ = build.HasKey(entry => entry.Id);
                _ = build.Property(entry => entry.Id).ValueGeneratedOnAdd();
                _ = build.HasOne(entry => entry.Movie).WithMany(entry => entry.Showtimes);
                _ = build.HasMany(entry => entry.Tickets).WithOne(entry => entry.Showtime).HasForeignKey(entry => entry.ShowtimeId);
            });

            _ = modelBuilder.Entity<MovieEntity>(build =>
            {
                _ = build.HasKey(entry => entry.Id);
                _ = build.Property(entry => entry.Id).ValueGeneratedOnAdd();
            });

            _ = modelBuilder.Entity<TicketEntity>(build =>
            {
                _ = build.HasKey(entry => entry.Id);
                _ = build.Property(entry => entry.Id).ValueGeneratedOnAdd();
            });
        }
    }
}
