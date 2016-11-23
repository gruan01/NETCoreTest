using IBiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entity.Models;
using Entity;
using Data;

namespace Biz {
    public class BlogsImpl : IBlogs {

        private BloggingContext Ctx { get; }

        public BlogsImpl(BloggingContext ctx) {
            this.Ctx = ctx;
        }

        public IEnumerable<Blog> GetAll() {
            return this.Ctx.Blog.ToList();
        }

    }
}
