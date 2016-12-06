using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ESLog {
    public class LogScope {

        private class DisposableScope : IDisposable {

            public void Dispose() {
                LogScope.Current = LogScope.Current.Parent;
            }

        }

        private readonly string _name;

        private readonly object _state;

        private static AsyncLocal<LogScope> _value = new AsyncLocal<LogScope>();

        public LogScope Parent {
            get;
            private set;
        }

        public static LogScope Current {
            get {
                return LogScope._value.Value;
            }
            set {
                LogScope._value.Value = value;
            }
        }

        internal LogScope(string name, object state) {
            this._name = name;
            this._state = state;
        }

        public static IDisposable Push(string name, object state) {
            LogScope current = LogScope.Current;
            LogScope.Current = new LogScope(name, state);
            LogScope.Current.Parent = current;
            return new LogScope.DisposableScope();
        }

        public override string ToString() {
            var state = _state;
            if (state == null) {
                return null;
            }
            return state.ToString();
        }
    }
}
