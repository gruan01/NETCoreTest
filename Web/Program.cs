using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.Reflection;

namespace Web {
    public class Program {
        public static void Main(string[] args) {
            var host = new WebHostBuilder()
                .UseUrls("http://+:5000")
                .UseKestrel()
#if DEBUG
                .UseContentRoot(Directory.GetCurrentDirectory())
#else
                .UseContentRoot(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
#endif                
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
