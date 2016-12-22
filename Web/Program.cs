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

            var dir2 = Directory.GetCurrentDirectory();
            var dir1 = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            Console.WriteLine(dir1);
            Console.WriteLine(dir2);

            var host = new WebHostBuilder()
                .UseUrls("http://+:5000")
                .UseKestrel()
#if DEBUG
                .UseContentRoot(dir2)
#else
                .UseContentRoot(dir1)
#endif                
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
