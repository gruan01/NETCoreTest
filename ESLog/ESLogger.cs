using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESLog {
    public class ESLogger : ILogger {

        public string Name { get; }

        public Func<string, LogLevel, bool> Filter { get; }

        public IDisposable BeginScope<TState>(TState state) {
            if (state == null) {
                throw new ArgumentNullException("state");
            }
            return LogScope.Push(this.Name, state);
        }

        public ESLogger(string name, Func<string, LogLevel, bool> filter) {
            this.Name = name;
            this.Filter = filter;
        }

        public bool IsEnabled(LogLevel logLevel) {
            return this.Filter.Invoke(this.Name, logLevel);
        }

        public async void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) {
            if (!this.IsEnabled(logLevel))
                return;

            if (formatter == null) {
                throw new ArgumentNullException("formatter");
            }
            var text = formatter.Invoke(state, exception);
            if (!string.IsNullOrEmpty(text) || exception != null) {
                var entry = new ESLogEntry() {
                    Title = $"{text};{exception?.Message}",
                    StackTrace = exception?.StackTrace,
                    CreatedOn = DateTime.Now,
                    EventID = eventId.Id,
                    EventName = eventId.Name
                };

                await ES.Write(entry);
            }
        }
    }
}
