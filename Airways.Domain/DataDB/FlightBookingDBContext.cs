using System;
using System.Collections.Generic;
using Airways.Domain.EntitiesDB;
using Microsoft.EntityFrameworkCore;

namespace Airways.Domain.DataDB;

public partial class FlightBookingDBContext : DbContext
{
    public FlightBookingDBContext()
    {
    }

    public FlightBookingDBContext(DbContextOptions<FlightBookingDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Connection> Connections { get; set; }

    public virtual DbSet<CustomerPref> CustomerPrefs { get; set; }

    public virtual DbSet<CustomerProfile> CustomerProfiles { get; set; }

    public virtual DbSet<Flight> Flights { get; set; }

    public virtual DbSet<Line> Lines { get; set; }

    public virtual DbSet<Meal> Meals { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=flightbookingdb.database.windows.net; Initial Catalog=FlightBookingDB; User ID=Beheerder; Password=Hazard10!; MultipleActiveResultSets=True;Encrypt=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Latin1_General_CI_AS");

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.Property(e => e.BookingId)
                .ValueGeneratedNever()
                .HasColumnName("BookingID");
            entity.Property(e => e.Class).HasMaxLength(20);
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.FlightId).HasColumnName("FlightID");
            entity.Property(e => e.MealId).HasColumnName("MealID");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(8, 2)");

            entity.HasOne(d => d.Customer).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bookings_CustomerProfiles");

            entity.HasOne(d => d.Flight).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.FlightId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bookings_Flights");

            entity.HasOne(d => d.Meal).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.MealId)
                .HasConstraintName("FK_Bookings_Meals");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.Property(e => e.CityId)
                .ValueGeneratedNever()
                .HasColumnName("CityID");
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.DestId)
                .HasColumnName("Dest_Id")
                .HasColumnType("numeric(18,0)");
        });

        modelBuilder.Entity<Connection>(entity =>
        {
            entity.Property(e => e.ConnectionId)
                .ValueGeneratedNever()
                .HasColumnName("ConnectionID");
            entity.Property(e => e.ArrivalId).HasColumnName("ArrivalID");
            entity.Property(e => e.DepartureId).HasColumnName("DepartureID");

            entity.HasOne(d => d.Arrival).WithMany(p => p.ConnectionArrivals)
                .HasForeignKey(d => d.ArrivalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Connections_ArrivalCities");

            entity.HasOne(d => d.Departure).WithMany(p => p.ConnectionDepartures)
                .HasForeignKey(d => d.DepartureId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Connections_DepartureCities");

            entity.HasOne(d => d.FlightNumber1Navigation).WithMany(p => p.ConnectionFlightNumber1Navigations)
                .HasForeignKey(d => d.FlightNumber1)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Connections_Line1");

            entity.HasOne(d => d.FlightNumber2Navigation).WithMany(p => p.ConnectionFlightNumber2Navigations)
                .HasForeignKey(d => d.FlightNumber2)
                .HasConstraintName("FK_Connections_Line2");

            entity.HasOne(d => d.FlightNumber3Navigation).WithMany(p => p.ConnectionFlightNumber3Navigations)
                .HasForeignKey(d => d.FlightNumber3)
                .HasConstraintName("FK_Connections_Line3");
        });

        modelBuilder.Entity<CustomerPref>(entity =>
        {
            entity.HasKey(e => e.PrefId);

            entity.Property(e => e.PrefId)
                .ValueGeneratedNever()
                .HasColumnName("PrefID");
            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.ProfileId).HasColumnName("ProfileID");

            entity.HasOne(d => d.City).WithMany(p => p.CustomerPrefs)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerPrefs_Cities");

            entity.HasOne(d => d.Profile).WithMany(p => p.CustomerPrefs)
                .HasForeignKey(d => d.ProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerPrefs_CustomerProfiles");
        });

        modelBuilder.Entity<CustomerProfile>(entity =>
        {
            entity.HasKey(e => e.ProfileId);

            entity.Property(e => e.ProfileId)
                .ValueGeneratedNever()
                .HasColumnName("ProfileID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.UserId).HasMaxLength(450);
        });

        modelBuilder.Entity<Flight>(entity =>
        {
            entity.HasKey(e => e.FlightId).HasName("PK__Flights__8A9E148E8B7EB9D6");

            entity.Property(e => e.FlightId).HasColumnName("FlightID");
            entity.Property(e => e.PriceBusiness)
                .HasDefaultValue(18999m)
                .HasColumnType("decimal(8, 2)");
            entity.Property(e => e.PriceEconomy)
                .HasDefaultValue(8999m)
                .HasColumnType("decimal(8, 2)");

            entity.HasOne(d => d.FlightNumberNavigation).WithMany(p => p.Flights)
                .HasForeignKey(d => d.FlightNumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Flights_Lines");
        });

        modelBuilder.Entity<Line>(entity =>
        {
            entity.HasKey(e => e.FlightNumber);

            entity.Property(e => e.FlightNumber).ValueGeneratedNever();
            entity.Property(e => e.ArrivalId).HasColumnName("ArrivalID");
            entity.Property(e => e.DepartureId).HasColumnName("DepartureID");
            entity.Property(e => e.TotalSeatsBusiness).HasDefaultValue(50);
            entity.Property(e => e.TotalSeatsEconomy).HasDefaultValue(200);

            entity.HasOne(d => d.Arrival).WithMany(p => p.LineArrivals)
                .HasForeignKey(d => d.ArrivalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lines_ArrivalCities");

            entity.HasOne(d => d.Departure).WithMany(p => p.LineDepartures)
                .HasForeignKey(d => d.DepartureId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lines_DepartureCities");
        });

        modelBuilder.Entity<Meal>(entity =>
        {
            entity.Property(e => e.MealId)
                .ValueGeneratedNever()
                .HasColumnName("MealID");
            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.City).WithMany(p => p.Meals)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK_Meals_Cities");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
