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
            await EnsureCurrenciesExists();
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

        private async Task EnsureCurrenciesExists() {
            var currencies = new[] {
                "AED", "AFN", "ALL", "AMD", "ANG", "AOA", "ARS", "AUD", "AWG", "AZN",
                "BAM", "BBD", "BDT", "BGN", "BHD", "BIF", "BMD", "BND", "BOB", "BOV",
                "BRL", "BSD", "BTN", "BWP", "BYR", "BZD", "CAD", "CDF", "CHE", "CHF",
                "CHW", "CLF", "CLP", "CNY", "COP", "COU", "CRC", "CUC", "CUP", "CVE",
                "CZK", "DJF", "DKK", "DOP", "DZD", "EGP", "ERN", "ETB", "EUR", "FJD",
                "FKP", "GBP", "GEL", "GHS", "GIP", "GMD", "GNF", "GTQ", "GYD", "HKD",
                "HNL", "HRK", "HTG", "HUF", "IDR", "ILS", "INR", "IQD", "IRR", "ISK",
                "JMD", "JOD", "JPY", "KES", "KGS", "KHR", "KMF", "KPW", "KRW", "KWD",
                "KYD", "KZT", "LAK", "LBP", "LKR", "LRD", "LSL", "LTL", "LYD", "MAD",
                "MDL", "MGA", "MKD", "MMK", "MNT", "MOP", "MRO", "MUR", "MVR", "MWK",
                "MXN", "MXV", "MYR", "MZN", "NAD", "NGN", "NIO", "NOK", "NPR", "NZD",
                "OMR", "PAB", "PEN", "PGK", "PHP", "PKR", "PLN", "PYG", "QAR", "RON",
                "RSD", "RUB", "RWF", "SAR", "SBD", "SCR", "SDG", "SEK", "SGD", "SHP",
                "SLL", "SOS", "SRD", "SSP", "STD", "SVC", "SYP", "SZL", "THB", "TJS",
                "TMT", "TND", "TOP", "TRY", "TTD", "TWD", "TZS", "UAH", "UGX", "USD",
                "UYI", "UYU", "UZS", "VEF", "VND", "VUV", "WST", "XAF", "XCD", "XDR",
                "XFU", "XOF", "XPD", "XPF", "XPT", "XSU", "XTS", "XUA", "YER", "ZAR",
                "ZMW", "ZWL"
            };

            foreach (var code in currencies) {
                var currency = await _database.Currencies.GetGlobalByCode(code);
                if (currency != null) continue;
                currency = new Currency() { Code = code };
                _database.Currencies.Add(currency);
            }

            await _database.SaveChangesAsync();
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