using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Common {

    public static class SettingsExtension {

        public static IApplicationBuilder UseSeetings(this IApplicationBuilder app) {
            var env = (IHostingEnvironment)app.ApplicationServices.GetService(typeof(IHostingEnvironment));
            Settings.Init(env.ContentRootPath);
            return app;
        }

    }
}
