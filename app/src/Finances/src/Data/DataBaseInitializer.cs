using System;
using System.Threading.Tasks;
using Finances.Models;
using Microsoft.AspNetCore.Identity;

namespace Finances.Data {
    public class DatabaseInititalizer {
        private readonly ApplicationDbContext _database;

        private readonly UserManager<ApplicationUser> _userManager; 

        public DatabaseInititalizer(ApplicationDbContext database,
                                    UserManager<ApplicationUser> userManager) {

            _database = database;
            _userManager = userManager;
        }

        public async Task Initialize() {
            await _database.Database.EnsureCreatedAsync();

            await EnsureAdminExists();
            await EnsureUnitsExists();
        }

        private async Task EnsureAdminExists() {
            var adminUser = await _userManager.FindByNameAsync("Admin");
            if (adminUser == null) {
                adminUser = new ApplicationUser { UserName = "Admin", Email = "admin@sample.net" };
                var result = await _userManager.CreateAsync(adminUser, "Adm1n1str@tor");
                if (result != IdentityResult.Success) {
                    throw new InvalidOperationException("Cannot create admin user");
                }
            }
        }

        private async Task EnsureUnitsExists() {
            await EnsureUnitExists("metre", 2);
            await EnsureUnitExists("kilogram", 2);
            await EnsureUnitExists("gram", 2);
            await EnsureUnitExists("piece", 0);
            await _database.SaveChangesAsync();
        }

        private async Task EnsureUnitExists(string code, int decimals) {
            var unit = await _database.Units.GetGlobalByCode(code);
            if (unit != null) return;
            unit = new Unit() { Code = code, Decimals = decimals };
            _database.Units.Add(unit);
        }
    }
}