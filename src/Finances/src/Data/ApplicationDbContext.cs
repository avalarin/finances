using Finances.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq;
using Npgsql.EntityFrameworkCore.PostgreSQL;

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

        public DbSet<Unit> Units { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Operation> Operations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);

            builder.HasPostgresExtension("uuid-ossp");

            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");

            builder.Entity<BookUser>().HasIndex(m => m.UserId);

            builder.Entity<Wallet>().HasIndex("BookId");

            builder.Entity<Unit>().HasIndex("Code", "BookId");

            builder.Entity<Tag>().HasIndex("BookId");

            builder.Entity<Product>().HasIndex("BookId");

            builder.Entity<Transaction>().HasIndex("BookId", "CreatedAt");

            builder.Entity<Operation>().HasIndex("TransactionId");
            builder.Entity<Operation>().HasIndex(o => o.WalletId);

            builder.Entity<TransactionTag>().HasKey(t => new {t.TagId, t.TransactionId});
            builder.Entity<TransactionTag>()
                .HasOne(tt => tt.Tag)
                .WithMany(t => t.Transactions)
                .HasForeignKey(tt => tt.TagId);
            builder.Entity<TransactionTag>()
                .HasOne(tt => tt.Transaction)
                .WithMany(t => t.Tags)
                .HasForeignKey(tt => tt.TransactionId)
                .OnDelete(DeleteBehavior.Restrict);

            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys())) {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}