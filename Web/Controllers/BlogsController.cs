using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Entity.Models;
using Entity;
using IBiz;
using Microsoft.Extensions.Logging;

namespace Web.Controllers {
    public class BlogsController : Controller {

        private IBlogs Biz { get; }

        private ILogger<BlogsController> Log = null;

        public BlogsController(IBlogs biz, ILogger<BlogsController> log) {
            this.Biz = biz;
            this.Log = log;
            this.Log.BeginScope("{0}", DateTime.Now);
        }


        public IActionResult Index() {
            var datas = this.Biz.GetAll();
            this.Log.LogInformation(1, "here");
            return View(datas);
        }


    }
}
