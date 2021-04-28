using System;
using System.Linq;
using Abp;
using Abp.Authorization.Users;
using LTMCompanyName.YoyoCmsTemplate.EntityFrameworkCore;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Blogs;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Seed.Tenants
{
    internal class DefaultBlogAndPostBuilder
    {
        private YoyoCmsTemplateDbContext _context;
        private readonly int _tenantId;

        public DefaultBlogAndPostBuilder(YoyoCmsTemplateDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            CreateBlogs();
            _context.SaveChanges();

        }



        private void CreateBlogs()
        {

            var adminUser = _context.Users.IgnoreQueryFilters()
                  .FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == AbpUserBase.AdminUserName);


          var blog=  _context.Blogs.IgnoreQueryFilters().FirstOrDefault(a => a.ShortName == "yoyomooc");
            if (blog==null)
            {
                blog = new Blog(SequentialGuidGenerator.Instance.Create(), "yoyomooc官方	", "yoyomooc");
                blog.Description = "yoyomooc的官方博客	";

                _context.Blogs.Add(blog);


            }




        }

    }
}
