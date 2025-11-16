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
    public DbSet<City> Cities { get; set; }
    public DbSet<Accommodation> Accommodations { get; set; }
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

    public DbSet<Image> Images { get; set; }

    public DbSet<Discount> Discounts { get; set; }

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
        ConfigureSystemParameterEntities(modelBuilder);
        ConfigureAdditionalDecimalEntities(modelBuilder);

        // Configure base entity properties
        ConfigureBaseEntityProperties(modelBuilder);

        // Mắc định restrict khi xóa cho tất cả các FK
        foreach (var foreignKey in modelBuilder.Model
            .GetEntityTypes()
            .SelectMany(e => e.GetForeignKeys()))
        {
            foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
        }
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
        modelBuilder.Entity<IdentityRole<int>>(entity =>
        {
            entity.ToTable("AspNetRoles");
        });

        modelBuilder.Entity<IdentityUserRole<int>>(entity =>
        {
            entity.ToTable("AspNetUserRoles");
        });

        modelBuilder.Entity<IdentityUserClaim<int>>(entity =>
        {
            entity.ToTable("AspNetUserClaims");
        });

        modelBuilder.Entity<IdentityUserLogin<int>>(entity =>
        {
            entity.ToTable("AspNetUserLogins");
        });

        modelBuilder.Entity<IdentityUserToken<int>>(entity =>
        {
            entity.ToTable("AspNetUserTokens");
        });

        modelBuilder.Entity<IdentityRoleClaim<int>>(entity =>
        {
            entity.ToTable("AspNetRoleClaims");
        });
    }


    private void ConfigureToursEntities(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tour>(entity =>
        {
            entity.ToTable("Tours");
            entity.Property(e => e.Code).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.BasePriceAdult).HasPrecision(12, 2);
            entity.Property(e => e.BasePriceChild).HasPrecision(12, 2);
            entity.Property(e => e.Rating).HasPrecision(3, 2);
            entity.HasIndex(e => e.Code).IsUnique();
            // Updated index: removed CategoryId
            entity.HasIndex(e => new { e.DepartureCityId, e.TypeId });

            // Relationship: TourType
            entity.HasOne(t => t.Type)
                  .WithMany(tt => tt.Tours)
                  .HasForeignKey(t => t.TypeId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(t => t.Category)
                  .WithMany(c => c.Tours)
                  .HasForeignKey(t => t.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(t => t.DepartureCity)
                  .WithMany(c => c.DepartureTours)
                  .HasForeignKey(t => t.DepartureCityId)
                  .OnDelete(DeleteBehavior.Restrict);           

            entity.HasOne(t => t.DestinationCity)
                  .WithMany(c => c.DestinationTours)
                  .HasForeignKey(t => t.DestinationCityId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<TourCategory>(e =>
        {
            e.ToTable("TourCategories");
            e.Property(x => x.Name).HasMaxLength(100).IsRequired();
            e.HasIndex(x => x.Name).IsUnique();
        });

        modelBuilder.Entity<TourType>(e =>
        {
            e.ToTable("TourTypes");
            e.Property(x => x.Name).HasMaxLength(100).IsRequired();
            e.Property(x => x.PriceLevel).HasConversion<int>();
        });

        modelBuilder.Entity<TourDeparture>(e =>
        {
            e.ToTable("TourDepartures");
            e.Property(x => x.PriceAdult).HasPrecision(12, 2);
            e.Property(x => x.PriceChildren).HasPrecision(12, 2);
            e.Property(x => x.SingleRoomSurcharge).HasPrecision(12, 2);
            e.Property(x => x.Status).HasConversion<int>();
            e.HasIndex(x => new { x.TourId, x.DepartureDate, x.Status });
        });

        modelBuilder.Entity<TourItinerary>(e =>
        {
            e.ToTable("TourItineraries");
            e.Property(x => x.DayNumber).HasMaxLength(50).IsRequired();
            e.HasIndex(x => new { x.TourId, x.DayNumber });
        });
    }

    private void ConfigureCatalogEntities(ModelBuilder modelBuilder)
    {
        // City configuration
        modelBuilder.Entity<City>(entity =>
        {
            entity.ToTable("Cities");
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Code).HasMaxLength(10);
            entity.Property(e => e.Region).HasConversion<int>();

            entity.HasIndex(e => e.Code).IsUnique();
            entity.HasIndex(e => e.Name);
        });

        // Accommodation configuration
        modelBuilder.Entity<Accommodation>(entity =>
        {
            entity.ToTable("Accommodations");
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.Rating).HasPrecision(3, 2);
        });
    }

    private void ConfigureBookingEntities(ModelBuilder modelBuilder)
    {
        // Booking configuration
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.ToTable("Bookings");
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
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Code).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Type).HasConversion<int>();

            entity.HasIndex(e => e.Code).IsUnique();
        });

        // Payment configuration
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.ToTable("Payments");
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
            entity.Property(e => e.Title).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Status).HasConversion<int>();
        });

        // Review configuration
        modelBuilder.Entity<Review>(entity =>
        {
            entity.ToTable("Reviews");
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
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Code).HasMaxLength(50).IsRequired();
            entity.Property(e => e.DiscountType).HasConversion<int>();
            entity.Property(e => e.DiscountValue).HasPrecision(12, 2);
            entity.Property(e => e.MaximumDiscount).HasPrecision(12, 2);

            entity.HasIndex(e => e.Code).IsUnique();
        });

        modelBuilder.Entity<PromotionUsage>(entity =>
        {
            entity.ToTable("PromotionUsages");
            entity.Property(e => e.DiscountAmount).HasPrecision(12, 2);
            entity.Property(e => e.UsedAt).IsRequired();

            entity.HasOne(entity => entity.Promotion)
                  .WithMany(p => p.PromotionUsages)
                  .HasForeignKey(entity => entity.PromotionId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(entity => entity.User)
                .WithMany(u => u.PromotionUsages)
                .HasForeignKey(entity => entity.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private void ConfigureComboEntities(ModelBuilder modelBuilder)
    {
        // Combo configuration
        modelBuilder.Entity<Combo>(entity =>
        {
            entity.ToTable("Combos");
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.BasePriceAdult).HasPrecision(12, 2);
            entity.Property(e => e.BasePriceChildren).HasPrecision(12, 2);
            entity.Property(e => e.Vehicle).HasConversion<int>();
            entity.Property(e => e.Rating).HasPrecision(3, 2);

            entity.HasIndex(e => new { e.FromCityId, e.ToCityId });

            // Explicit relationships for dual FK to City
            entity.HasOne(c => c.FromCity)
                  .WithMany(c => c.FromCombos)
                  .HasForeignKey(c => c.FromCityId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(c => c.ToCity)
                  .WithMany(c => c.ToCombos)
                  .HasForeignKey(c => c.ToCityId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private void ConfigureSystemParameterEntities(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SystemParameter>(entity =>
        {
            entity.ToTable("SystemParameters");
            entity.Property(e => e.Code).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.FeatureCode).HasConversion<string>();
            entity.Property(e => e.FeatureCode).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(200);
        });
    }

    private void ConfigureAdditionalDecimalEntities(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ComboSchedule>(entity =>
        {
            entity.Property(e => e.BasePriceAdult).HasPrecision(12, 2);
            entity.Property(e => e.BasePriceChildren).HasPrecision(12, 2);
            entity.Property(e => e.SingleRoomSupplement).HasPrecision(12, 2);
        });

        modelBuilder.Entity<PromotionUsage>(entity =>
        {
            entity.Property(e => e.DiscountAmount).HasPrecision(12, 2);
        });

        modelBuilder.Entity<RoomInventory>(entity =>
        {
            entity.Property(e => e.BasePriceAdult).HasPrecision(12, 2);
            entity.Property(e => e.BasePriceChildren).HasPrecision(12, 2);
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