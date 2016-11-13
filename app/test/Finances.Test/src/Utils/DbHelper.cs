using System;
using System.Linq;
using Finances.Data;
using Finances.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Finances.Test.Utils {
    public static class DbHelper {

        public static ApplicationDbContext CreateDbContext(ILoggerFactory loggerFactory) {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            dbContextOptionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString())
                                   .ConfigureWarnings((bldr) => {
                                       bldr.Ignore(InMemoryEventId.TransactionIgnoredWarning);
                                   });

            var dbContextOptions = dbContextOptionsBuilder.Options;


            using (var context = new ApplicationDbContext(dbContextOptions)) {
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
                    logger: loggerFactory.CreateLogger<UserManager<ApplicationUser>>()
                    );

                new DatabaseInititalizer(context, userManager).Initialize().Wait();

                userManager.CreateAsync(new ApplicationUser { UserName = "Admin", Email = "admin@sample.net" }, "Adm1n1str@tor").Wait();
                userManager.CreateAsync(new ApplicationUser { UserName = "Member", Email = "Member@sample.net" }, "Adm1n1str@tor").Wait();
                userManager.CreateAsync(new ApplicationUser { UserName = "Guest", Email = "Guest@sample.net" }, "Adm1n1str@tor").Wait();
            }

            return new ApplicationDbContext(dbContextOptions);
        }

    }
}