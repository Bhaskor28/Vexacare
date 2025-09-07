using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vexacare.Domain.Entities.BlogManagement;

namespace Vexacare.Application.BlogPosts
{
    public class BlogCategoryVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<BlogPost> BlogPosts { get; set; }
    }
}
