﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data {
    public static class BlogDBInitilizer {

        public static void Init(BloggingContext ctx) {
            ctx.Database.EnsureCreated();
        }

    }
}
