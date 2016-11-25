using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Common {

    public class Settings {

        public string Path { get; set; } = "Configs";

        //private static Lazy<Settings> _current = new Lazy<Settings>(() => new Settings());

        //public static Settings Current => _current.Value;

        private Settings(IConfigurationRoot cfg) {
            
        }
    }
}
