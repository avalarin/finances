using System;
using System.Threading.Tasks;
using Finances.Exceptions;
using Xunit;

namespace Finances.Test.Utils {
    public static class AssertErrors {
        public static async Task<ApplicationError> ExpectAppError(ApplicationError expected, Func<Task> testCode) {
            var e = await Assert.ThrowsAsync<ApplicationException>(testCode);
            
            var error = e.Error;

            Assert.Equal(expected, error);

            return error;
        }
    }
}