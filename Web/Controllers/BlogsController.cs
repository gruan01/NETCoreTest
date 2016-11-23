using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Entity.Models;
using Entity;
using IBiz;

namespace Web.Controllers {
    public class BlogsController : Controller {

        private IBlogs Biz { get; }


        public BlogsController(IBlogs biz) {
            this.Biz = biz;
        }


        public IActionResult Index() {

            var datas = this.Biz.GetAll();

            return View(datas);
        }


    }
}
