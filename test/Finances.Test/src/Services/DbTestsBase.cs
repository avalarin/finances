using System;
using Finances.Data;
using Finances.Models;
using Finances.Test.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Finances.Test.Services {
    public class DbTestsBase : IDisposable {
        private readonly ITestOutputHelper _outputHelper;
        protected ApplicationDbContext Db { get; }

        public DbTestsBase(ITestOutputHelper outputHelper) {
            _outputHelper = outputHelper;
            Db = DbHelper.CreateDbContext(CreateLogger<UserManager<ApplicationUser>>());
        }

        protected ILogger<T> CreateLogger<T>() {
            return new TestLogger<T>(_outputHelper);
        } 

        public virtual void Dispose() {
            Db.Dispose();
        }
    }
}