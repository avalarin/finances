using Finances.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Finances.Data {
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
        public ApplicationDbContext() {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) {
        }

        public DbSet<Session> Sessions { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<BookUser> BooksUsers { get; set; }

        public DbSet<Wallet> Wallets { get; set; }
        
        public DbSet<Currency> Currencies { get; set; }

        public DbSet<Unit> Units { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");

            builder.Entity<BookUser>().HasIndex(m => m.UserId);

            builder.Entity<Wallet>().HasIndex("BookId");

            builder.Entity<Currency>().HasIndex("Code", "BookId");

            builder.Entity<Unit>().HasIndex("Code", "BookId");

            builder.Entity<Tag>().HasIndex("BookId");

            builder.Entity<Product>().HasIndex("BookId");
        }
    }
}