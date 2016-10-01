using System;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Finances.Test.Utils {
    public class TestLogger<T> : ILogger<T> {
        private readonly ITestOutputHelper _outputHelper;

        public TestLogger(ITestOutputHelper outputHelper) {
            _outputHelper = outputHelper;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) {
            if (formatter == null) throw new ArgumentNullException(nameof(formatter));
            var message = formatter(state, exception);
            _outputHelper.WriteLine(message);
        }

        public bool IsEnabled(LogLevel logLevel) {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state) {
            return null;
        }
    }
}