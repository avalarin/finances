using Finances.Test.Utils;
using Xunit;

namespace Finances.Test.Services {
    public class TestsBase : IClassFixture<DatabaseFixture> {
        public TestsBase(DatabaseFixture fixture) {
            DbFixture = fixture;
        }

        protected DatabaseFixture DbFixture { get; }
    }
}