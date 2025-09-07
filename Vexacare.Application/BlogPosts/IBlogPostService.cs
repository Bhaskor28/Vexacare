using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vexacare.Application.Locations;

namespace Vexacare.Application.BlogPosts
{
    public interface IBlogPostService
    {
        Task<IEnumerable<BlogPostVM>> GetAllBlogPostAsync(); // Changed to plural
        Task<BlogPostVM> GeBlogPostByIdAsync(int id);
        Task<BlogPostVM> CreateBlogPostAsync(BlogPostVM blogPostVM);
        Task<BlogPostVM> UpdateBlogPostAsync(BlogPostVM blogPostVM);
        Task<bool> DeleteBlogPostAsync(int id);
    }
}
