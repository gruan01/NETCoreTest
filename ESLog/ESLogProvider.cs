using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ESLog {
    public class ESLogProvider : ILoggerProvider {

        private Func<string, LogLevel, bool> Filter = null;

        public ESLogProvider(Func<string, LogLevel, bool> filter = null) {
            if (filter != null)
                this.Filter = filter;
            else
                this.Filter = new Func<string, LogLevel, bool>((name, level) => level >= LogLevel.Information);
        }

        public ILogger CreateLogger(string categoryName) {
            return new ESLogger(categoryName, this.Filter);
        }

        public void Dispose() {

        }
    }
}
