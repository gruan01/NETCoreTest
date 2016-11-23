using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IBiz {
    public interface IBlogs {
        IEnumerable<Blog> GetAll();
    }
}
