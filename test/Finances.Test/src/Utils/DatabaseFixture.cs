using System;
using System.Linq;
using Finances.Data;
using Finances.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Finances.Test.Utils {
    public class DatabaseFixture : IDisposable {

        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions; 

        public DatabaseFixture() {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            dbContextOptionsBuilder.UseInMemoryDatabase();
            _dbContextOptions = dbContextOptionsBuilder.Options;

            using (var context = CreateContext()) {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var userManager = new UserManager<ApplicationUser>(
                    store: new UserStore<ApplicationUser>(context),
                    optionsAccessor: null,
                    passwordHasher: new PasswordHasher<ApplicationUser>(),
                    userValidators: Enumerable.Empty<IUserValidator<ApplicationUser>>(), 
                    passwordValidators: Enumerable.Empty<IPasswordValidator<ApplicationUser>>(),
                    keyNormalizer: new UpperInvariantLookupNormalizer(), 
                    errors: null,
                    services: null,
                    logger: new TestLogger<UserManager<ApplicationUser>>()
                );

                var initializer = new DatabaseInititalizer(context, userManager);
                initializer.Initialize().Wait();
            }
        }

        public ApplicationDbContext CreateContext() {
            return new ApplicationDbContext(_dbContextOptions);
        }

        public void Dispose() { }
    }
}
