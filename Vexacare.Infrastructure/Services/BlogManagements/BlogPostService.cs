using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vexacare.Application.BlogPosts;
using Vexacare.Application.Interfaces;
using Vexacare.Domain.Entities.BlogManagement;
using Vexacare.Infrastructure.Data;

namespace Vexacare.Infrastructure.Services.BlogManagements
{
    public class BlogPostService : IBlogPostService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;
        private const string ContainerName = "Blogs";

        public BlogPostService(ApplicationDbContext context,
            IMapper mapper,
            IFileStorageService fileStorageService
            )
        {
            _context = context;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
        }

        public async Task<BlogPostVM> CreateBlogPostAsync(BlogPostVM blogPostVM)
        {
            string imageUrl = null;
            if (blogPostVM.FeaturedImage != null)
            {
                imageUrl = await _fileStorageService.SaveFileAsync(blogPostVM.FeaturedImage, ContainerName);
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    await _fileStorageService.DeleteFileAsync(imageUrl, ContainerName);
                }
                imageUrl = await _fileStorageService.SaveFileAsync(blogPostVM.FeaturedImage, ContainerName);
                blogPostVM.FeaturedImagePath = imageUrl;
            }
            
            var blogPost = _mapper.Map<BlogPost>(blogPostVM);
            blogPost.CreatedAt = DateTime.UtcNow;

            // Handle image upload if provided

            _context.BlogPosts.Add(blogPost);
            await _context.SaveChangesAsync();

            return _mapper.Map<BlogPostVM>(blogPost);
        }

        public Task<bool> DeleteBlogPostAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<BlogPostVM> GeBlogPostByIdAsync(int id)
        {
            var blog = await _context.BlogPosts.FirstOrDefaultAsync(a=>a.Id==id);
            
            return _mapper.Map<BlogPostVM>(blog);
        }

        public async Task<IEnumerable<BlogPostVM>> GetAllBlogPostAsync()
        {
            var blogPosts = await _context.BlogPosts
                .Include(b => b.Category)
                .ToListAsync();
            

            return _mapper.Map<IEnumerable<BlogPostVM>>(blogPosts);
        }

        public Task<BlogPostVM> UpdateBlogPostAsync(BlogPostVM blogPostVM)
        {
            throw new NotImplementedException();
        }
    }
}
