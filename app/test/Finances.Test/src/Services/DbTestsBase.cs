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
            Db = DbHelper.CreateDbContext(new LoggerFactory(_outputHelper));
        }

        protected ILogger<T> CreateLogger<T>() {
            return new TestLogger<T>(_outputHelper);
        } 

        public virtual void Dispose() {
            Db.Dispose();
        }
        
        public class LoggerFactory : ILoggerFactory {
            private readonly ITestOutputHelper _outputHelper;

            public LoggerFactory(ITestOutputHelper outputHelper) {
                _outputHelper = outputHelper;
            }

            public void Dispose() {
                throw new NotImplementedException();
            }

            public ILogger CreateLogger(string categoryName) {
                return new TestLogger(_outputHelper);
            }

            public void AddProvider(ILoggerProvider provider) {
                throw new NotImplementedException();
            }
        }
    }
}