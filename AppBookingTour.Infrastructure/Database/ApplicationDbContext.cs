using AppBookingTour.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AppBookingTour.Infrastructure.Database;

/// <summary>
/// Inherits from IdentityDbContext for ASP.NET Core Identity integration
/// </summary>
public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    #region Identity
    // ApplicationUser already defined in IdentityDbContext base class
    // public DbSet<ApplicationUser> Users { get; set; } - inherited
    // public DbSet<IdentityRole> Roles { get; set; } - inherited
    #endregion

    #region Tours & Catalog
    public DbSet<Tour> Tours { get; set; }
    public DbSet<TourCategory> TourCategories { get; set; }
    public DbSet<TourType> TourTypes { get; set; }
    public DbSet<TourDeparture> TourDepartures { get; set; }
    public DbSet<TourItinerary> TourItineraries { get; set; }
    public DbSet<TourItineraryDestination> TourItineraryDestinations { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Destination> Destinations { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<RoomType> RoomTypes { get; set; }
    #endregion

    #region Booking
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<BookingParticipant> BookingParticipants { get; set; }
    #endregion

    #region Payments
    public DbSet<Payment> Payments { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    #endregion

    #region Content
    public DbSet<Review> Reviews { get; set; }
    public DbSet<BlogPost> BlogPosts { get; set; }
    #endregion

    #region Promotions & Combos
    public DbSet<Promotion> Promotions { get; set; }
    public DbSet<PromotionUsage> PromotionUsages { get; set; }
    public DbSet<Combo> Combos { get; set; }
    public DbSet<ComboSchedule> ComboSchedules { get; set; }
    #endregion

    #region Domain Events & Infrastructure
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Use default schema for all entities (single schema approach)
        modelBuilder.HasDefaultSchema("dbo");

        // Configure Identity tables to use default schema
        ConfigureIdentityTables(modelBuilder);
        
        // Configure domain entities
        ConfigureToursEntities(modelBuilder);
        ConfigureCatalogEntities(modelBuilder);
        ConfigureBookingEntities(modelBuilder);
        ConfigurePaymentEntities(modelBuilder);
        ConfigureContentEntities(modelBuilder);
        ConfigurePromotionEntities(modelBuilder);
        ConfigureComboEntities(modelBuilder);
        
        // Configure base entity properties
        ConfigureBaseEntityProperties(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Update audit fields
        foreach (EntityEntry<BaseEntity> entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    #region Configuration Methods

    private void ConfigureIdentityTables(ModelBuilder modelBuilder)
    {
        // Configure other Identity tables
        modelBuilder.Entity<IdentityRole>(entity =>
        {
            entity.ToTable("AspNetRoles");
        });

        modelBuilder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.ToTable("AspNetUserRoles");
        });

        modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
        {
            entity.ToTable("AspNetUserClaims");
        });

        modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
        {
            entity.ToTable("AspNetUserLogins");
        });

        modelBuilder.Entity<IdentityUserToken<string>>(entity =>
        {
            entity.ToTable("AspNetUserTokens");
        });

        modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
        {
            entity.ToTable("AspNetRoleClaims");
        });
    }

    private void ConfigureToursEntities(ModelBuilder modelBuilder)
    {
        // Tour configuration
        modelBuilder.Entity<Tour>(entity =>
        {
            entity.ToTable("Tours");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Code).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.BasePriceAdult).HasPrecision(12, 2);
            entity.Property(e => e.BasePriceChild).HasPrecision(12, 2);
            entity.Property(e => e.Rating).HasPrecision(3, 2);
            entity.Property(e => e.Status).HasConversion<int>();
            
            entity.HasIndex(e => e.Code).IsUnique();
            entity.HasIndex(e => new { e.DepartureCityId, e.TypeId, e.Status });
        });

        // TourCategory configuration
        modelBuilder.Entity<TourCategory>(entity =>
        {
            entity.ToTable("TourCategories");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            
            entity.HasIndex(e => e.Name).IsUnique();
        });

        // TourType configuration
        modelBuilder.Entity<TourType>(entity =>
        {
            entity.ToTable("TourTypes");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.PriceLevel).HasConversion<int>();
        });

        // TourDeparture configuration
        modelBuilder.Entity<TourDeparture>(entity =>
        {
            entity.ToTable("TourDepartures");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PriceAdult).HasPrecision(12, 2);
            entity.Property(e => e.PriceChildren).HasPrecision(12, 2);
            entity.Property(e => e.Status).HasConversion<int>();
            
            entity.HasIndex(e => new { e.TourId, e.DepartureDate, e.Status });
        });

        // TourItinerary configuration
        modelBuilder.Entity<TourItinerary>(entity =>
        {
            entity.ToTable("TourItineraries");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DayNumber).HasMaxLength(50).IsRequired();
            
            entity.HasIndex(e => new { e.TourId, e.DayNumber });
        });
    }

    private void ConfigureCatalogEntities(ModelBuilder modelBuilder)
    {
        // City configuration
        modelBuilder.Entity<City>(entity =>
        {
            entity.ToTable("Cities");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Code).HasMaxLength(10);
            entity.Property(e => e.Region).HasConversion<int>();
            
            entity.HasIndex(e => e.Code).IsUnique();
            entity.HasIndex(e => e.Name);
        });

        // Hotel configuration
        modelBuilder.Entity<Hotel>(entity =>
        {
            entity.ToTable("Hotels");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.Rating).HasPrecision(3, 2);
            
            entity.HasIndex(e => new { e.CityId, e.StarRating });
        });
    }

    private void ConfigureBookingEntities(ModelBuilder modelBuilder)
    {
        // Booking configuration
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.ToTable("Bookings");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.BookingCode).HasMaxLength(50).IsRequired();
            entity.Property(e => e.BookingType).HasConversion<int>();
            entity.Property(e => e.TotalAmount).HasPrecision(12, 2);
            entity.Property(e => e.DiscountAmount).HasPrecision(12, 2);
            entity.Property(e => e.FinalAmount).HasPrecision(12, 2);
            entity.Property(e => e.PaymentType).HasConversion<int>();
            entity.Property(e => e.Status).HasConversion<int>();
            entity.Property(e => e.ContactName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.ContactPhone).HasMaxLength(20).IsRequired();
            entity.Property(e => e.ContactEmail).HasMaxLength(100).IsRequired();

            entity.HasIndex(e => e.BookingCode).IsUnique();
        });

        // BookingParticipant configuration
        modelBuilder.Entity<BookingParticipant>(entity =>
        {
            entity.ToTable("BookingParticipants");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FullName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Gender).HasConversion<int>();
            entity.Property(e => e.ParticipantType).HasConversion<int>().IsRequired();
        });
    }

    private void ConfigurePaymentEntities(ModelBuilder modelBuilder)
    {
        // PaymentMethod configuration
        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.ToTable("PaymentMethods");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Code).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Type).HasConversion<int>();

            entity.HasIndex(e => e.Code).IsUnique();
        });

        // Payment configuration
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.ToTable("Payments");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PaymentNumber).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Amount).HasPrecision(12, 2);
            entity.Property(e => e.TransactionId).HasMaxLength(50);
            entity.Property(e => e.Status).HasConversion<int>();

            entity.HasIndex(e => e.PaymentNumber).IsUnique();
            entity.HasIndex(e => e.TransactionId);
        });
    }

    private void ConfigureContentEntities(ModelBuilder modelBuilder)
    {
        // BlogPost configuration
        modelBuilder.Entity<BlogPost>(entity =>
        {
            entity.ToTable("BlogPosts");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Status).HasConversion<int>();
        });

        // Review configuration
        modelBuilder.Entity<Review>(entity =>
        {
            entity.ToTable("Reviews");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.Comment).HasMaxLength(2000).IsRequired();
            entity.Property(e => e.Status).HasConversion<int>();

            entity.HasCheckConstraint("CK_Review_Rating", "Rating >= 1 AND Rating <= 5");
        });
    }

    private void ConfigurePromotionEntities(ModelBuilder modelBuilder)
    {
        // Promotion configuration
        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.ToTable("Promotions");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Code).HasMaxLength(50).IsRequired();
            entity.Property(e => e.DiscountType).HasConversion<int>();
            entity.Property(e => e.DiscountValue).HasPrecision(12, 2);
            entity.Property(e => e.MaximumDiscount).HasPrecision(12, 2);

            entity.HasIndex(e => e.Code).IsUnique();
        });
    }

    private void ConfigureComboEntities(ModelBuilder modelBuilder)
    {
        // Combo configuration
        modelBuilder.Entity<Combo>(entity =>
        {
            entity.ToTable("Combos");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.BasePriceAdult).HasPrecision(12, 2);
            entity.Property(e => e.BasePriceChildren).HasPrecision(12, 2);
            entity.Property(e => e.Vehicle).HasConversion<int>();
            entity.Property(e => e.Rating).HasPrecision(3, 2);
            
            entity.HasIndex(e => new { e.FromCityId, e.ToCityId });
        });
    }

    private void ConfigureBaseEntityProperties(ModelBuilder modelBuilder)
    {
        // Configure base entity properties for all entities
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property<DateTime>("CreatedAt")
                    .HasDefaultValueSql("GETUTCDATE()");

                modelBuilder.Entity(entityType.ClrType)
                    .Property<byte[]>("RowVersion")
                    .IsRowVersion();

                // Ignore domain events for all entities
                modelBuilder.Entity(entityType.ClrType)
                    .Ignore("DomainEvents");
            }
        }
    }

    #endregion
}