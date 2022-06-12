using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StoreSystemApi.Models.Auth;
using StoreSystemApi.Models.Entities;
using System.Linq.Expressions;

namespace StoreSystemApi.Models
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<BonusCard> BonusCards { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Purchase> Purchases { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Query filter to automatically remove deleted Entities from the result
            // Problem: Product has a global query filter defined and is the required end of a relationship
            //          with the entity 'PurchaseProduct'. This may lead to unexpected results when
            //          the required entity is filtered out
            /*foreach (var entityType in builder.Model.GetEntityTypes())
            {
                //If the actual entity is an Entity type. 
                if (typeof(Entity).IsAssignableFrom(entityType.ClrType))
                {
                    //That always excludes deleted items. You can opt out by using dbSet.IgnoreQueryFilters()
                    var parameter = Expression.Parameter(entityType.ClrType, "p");

                    var deletedCheck = Expression.Lambda(
                        Expression.Equal(Expression.Property(parameter, "DeletedAt"), Expression.Constant(null)),
                        parameter);

                    builder.Entity(entityType.ClrType).HasQueryFilter(deletedCheck);
                }
            }*/

            base.OnModelCreating(builder);

            builder.Entity<BonusCard>()
                .HasOne(bc => bc.Owner)
                .WithOne(u => u.BonusCard)
                .HasForeignKey<BonusCard>(bc => bc.OwnerId);

            ConfigurePurchases(builder);
            ConfigureProducts(builder);

            // many-to-many table
            builder.Entity<PurchaseProduct>()
                .HasKey(pp => new { pp.PurchaseId, pp.ProductId });

            builder.Entity<PurchaseProduct>()
                .HasOne(pp => pp.Product)
                .WithMany(p => p.PurchaseProducts)
                .HasForeignKey(pp => pp.ProductId);

            builder.Entity<PurchaseProduct>()
                .HasOne(pp => pp.Purchase)
                .WithMany(p => p.PurchaseProducts)
                .HasForeignKey(pp => pp.PurchaseId);

            PopulateDb(builder);
        }

        private void PopulateDb(ModelBuilder builder)
        {
            //Seeding a  'Administrator' role to AspNetRoles table
            builder.Entity<IdentityRole<Guid>>()
                .HasData(new IdentityRole<Guid> {
                    Id = new Guid("2c5e174e-3b0e-446f-86af-483d56fd7210"),
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR",
                    ConcurrencyStamp = "46477ba0-29d2-4ba5-b10c-471a6aa88869"
                });


            //a hasher to hash the password before seeding the user to the db
            var hasher = new PasswordHasher<IdentityUser>();


            //Seeding the User to AspNetUsers table
            builder.Entity<User>().HasData(
                new User
                {
                    Id = new Guid("8e445865-a24d-4543-a6c6-9443d048cdb9"), // primary key
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    FirstName = "Admin",
                    MiddleName = "Admin",
                    LastName = "Admin",
                    Email = "admin@mail.com",
                    NormalizedEmail = "ADMIN@MAIL.COM",
                    PhoneNumber = "+111111111111",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    PasswordHash = "AQAAAAEAACcQAAAAEFvpwHR0kH1sy6DQWpIndLCdmZsahrddpi9XzA5DkTSadoKfzl+amp9ya+lWuMpIWQ==",
                    ConcurrencyStamp = "598b1fc2-783d-4c22-82e3-0fcc51bc4f16"
                }
            );


            //Seeding the relation between our user and role to AspNetUserRoles table
            builder.Entity<IdentityUserRole<Guid>>().HasData(
                new IdentityUserRole<Guid>
                {
                    RoleId = new Guid("2c5e174e-3b0e-446f-86af-483d56fd7210"),
                    UserId = new Guid("8e445865-a24d-4543-a6c6-9443d048cdb9")
                }
            );
        }

        public void ConfigurePurchases(ModelBuilder builder)
        {
            builder.Entity<Purchase>()
                .Property(p => p.Total)
                .HasColumnType("decimal(19, 4)");

            builder.Entity<Purchase>()
                .Property(p => p.TaxRate)
                .HasColumnType("decimal(19, 4)");

            builder.Entity<Purchase>()
                .Property(p => p.PurchaseDate)
                .HasDefaultValueSql("getutcdate()");

            builder.Entity<Purchase>()
                .HasOne(p => p.BonusCard)
                .WithMany(bc => bc.Purchases)
                .HasForeignKey(p => p.BonusCardId);

            builder.Entity<Purchase>()
                .HasOne(p => p.Cashier)
                .WithMany(c => c.Purchases)
                .HasForeignKey(p => p.CashierId);
        }

        public void ConfigureProducts(ModelBuilder builder)
        {
            builder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(19, 4)");

            builder.Entity<Product>()
                .HasIndex(p => p.Barcode)
                .IsUnique();

            builder.Entity<Product>()
                .Property(p => p.DiscountRate)
                .HasColumnType("decimal(5, 4)");

            builder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            builder.Entity<Product>()
                .HasOne(p => p.Supplier)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.SupplierId);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            HandleEntityProperties();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            HandleEntityProperties();
            return base.SaveChanges();
        }

        private void HandleEntityProperties()
        {
            var insertedEntries = ChangeTracker.Entries()
                               .Where(x => x.State == EntityState.Added)
                               .Select(x => x.Entity);
            foreach (var insertedEntry in insertedEntries)
            {
                var newEntity = insertedEntry as Entity;
                //If the inserted object is an Entity. 
                if (newEntity != null)
                {
                    newEntity.CreatedAt = DateTimeOffset.UtcNow;
                }
            }

            var modifiedEntries = ChangeTracker.Entries()
                       .Where(x => x.State == EntityState.Modified)
                       .Select(x => x.Entity);
            foreach (var modifiedEntry in modifiedEntries)
            {
                //If the inserted object is an Entity. 
                var modifiedEntity = modifiedEntry as Entity;
                if (modifiedEntity != null)
                {
                    modifiedEntity.ModifiedAt = DateTimeOffset.UtcNow;
                }
            }

            var deletedEntries = ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Deleted);

            foreach (var deletedEntry in deletedEntries)
            {
                //If the deleted object is an Entity. 
                var deletedEntity = deletedEntry.Entity as Entity;
                if (deletedEntity != null)
                {
                    deletedEntry.State = EntityState.Modified;
                    deletedEntity.DeletedAt = DateTimeOffset.UtcNow;
                }
            }
        }

        public DbSet<StoreSystemApi.Models.Entities.PurchaseProduct>? PurchaseProduct { get; set; }
    }
}
