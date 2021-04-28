using Abp.IdentityServer4;
using Abp.Zero.EntityFrameworkCore;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Roles;
using LTMCompanyName.YoyoCmsTemplate.DataFileObjects;
using LTMCompanyName.YoyoCmsTemplate.Dropdown;
using LTMCompanyName.YoyoCmsTemplate.Editions;
using LTMCompanyName.YoyoCmsTemplate.EntityMapper.DataFileObjects;
using LTMCompanyName.YoyoCmsTemplate.EntityMapper.Mooc;
using LTMCompanyName.YoyoCmsTemplate.EntityMapper.MultiTenancy;
using LTMCompanyName.YoyoCmsTemplate.EntityMapper.Products;
using LTMCompanyName.YoyoCmsTemplate.EntityMapper.ProductSecretKeys;
using LTMCompanyName.YoyoCmsTemplate.EntityMapper.UserManagerment.Users;
using LTMCompanyName.YoyoCmsTemplate.Friendships;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Blogs;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Comments;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Posts;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Tagging;
using LTMCompanyName.YoyoCmsTemplate.Modules.Chat;
using LTMCompanyName.YoyoCmsTemplate.Modules.ContentManagement.Members;
using LTMCompanyName.YoyoCmsTemplate.Modules.ContentManagement.PortalWebsiteSetting;
using LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Projects;
using LTMCompanyName.YoyoCmsTemplate.Modules.FileManager;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.DownloadLogs;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.OrderManagement;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductManagement;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductSecretKeyManagement;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseCategoryInfo;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseClassHours;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseSections;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.Relationships;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.VideoResources;
using LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.NeteaseOrders;
using LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.TencentOrders;
using LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.BannerAds;
using LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.Blogrolls;
using LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.Notices;
using LTMCompanyName.YoyoCmsTemplate.Modules.WeChat.WechatAppConfigs;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy.Payments;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;
using Microsoft.EntityFrameworkCore;
using YoYo.ABPCommunity.EntityMapper.Orders;
using YoYo.ABPCommunity.EntityMapper.OtherOrders.TencentOrderInfos;
using LTMCompanyName.YoyoCmsTemplate.EntityMapper.MessageHistorys;
using LTMCompanyName.YoyoCmsTemplate.Message;

namespace LTMCompanyName.YoyoCmsTemplate.EntityFrameworkCore
{
    public class YoyoCmsTemplateDbContext : AbpZeroDbContext<Tenant, Role, User, YoyoCmsTemplateDbContext>, IAbpPersistedGrantDbContext
    {
        /* Define a DbSet for each entity of the application */

        #region 测试

        //public DbSet<Book> Books { get; set; }
        // public DbSet<Person> People { get; set; }

        #endregion 测试

        #region 数据文件存储

        public virtual DbSet<DataFileObject> DataFileObjects { get; set; }

        public DbSet<SysFile> SysFiles { get; set; }

        #endregion 数据文件存储

        #region 订阅

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        #endregion 订阅

        #region IdentityServer4

        public virtual DbSet<PersistedGrantEntity> PersistedGrants { get; set; }

        #endregion IdentityServer4

        #region 微信

        public virtual DbSet<WechatAppConfig> WechatAppConfigs { get; set; }

        #endregion 微信

        #region 博客模块

        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public DbSet<BlogTag> BlogTags { get; set; }


        #endregion 博客模块

        #region 聊天模块

        public virtual DbSet<Friendship> Friendships { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        #endregion 聊天模块

        #region 网站设置模块

        /**
         *轮播图
         * 友情链接
         * 公告
         *
         */

        public DbSet<Blogroll> Blogrolls { get; set; }
        public DbSet<BlogrollType> BlogrollTypes { get; set; }
        public DbSet<WebSiteNotice> WebSiteNotices { get; set; }
        public DbSet<BannerAd> BannerAds { get; set; }

        #endregion 网站设置模块

        #region Old模块

        public DbSet<PortalWebsiteSetting> PortalWebsiteSettings { get; set; }

        public DbSet<Member> Members { get; set; }

        #endregion Old模块



        #region Mooc慕课模块

        
        /// <summary>
        /// 课程分类
        /// </summary>
        public DbSet<CourseCategory> CourseCategories { get; set; }
        /// <summary>
        /// 课程分类和课程的中间表
        /// </summary>
        public DbSet<CourseToCourseCategory>  CourseToCourseCategories { get; set; }
        /// <summary>
        /// 课程表
        /// </summary>
        public DbSet<Course> Courses { get; set; }
        /// <summary>
        /// 课程章节
        /// </summary>
        public DbSet<CourseSection> CourseSections { get; set; }
        /// <summary>
        /// 课程课时
        /// </summary>
        public DbSet<CourseClassHour> CourseClassHours { get; set; }
        /// <summary>
        /// 视频资源
        /// </summary>
        public DbSet<VideoResource> VideoResources { get; set; }
        

        #endregion


        #region 其他订单

        /// <summary>
        /// 订单信息
        /// </summary>
        public DbSet<NeteaseOrderInfo> NeteaseOrderInfos { get; set; }

        public DbSet<TencentOrderInfo> TencentOrderInfos { get; set; }

        #endregion 课程

        #region Dropdown
        //public DbSet<DropdownType> DropdownType { get; set; }

        //public DbSet<DropdownList> DropdownList { get; set; }

        #endregion

        public DbSet<Project> Projects { get; set; }

        public DbSet<DownloadLog> DownloadLogs { get; set; }

        public DbSet<UserDownloadConfig> UserDownloadConfigs { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductSecretKey> ProductSecretKeys { get; set; }

        public DbSet<Order> Orders { get; set; }


        #region 消息发送历史
        public virtual DbSet<MessageHistory> MessageHistorie { get; set; }
        #endregion 消息发送历史

        public YoyoCmsTemplateDbContext(DbContextOptions<YoyoCmsTemplateDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //修改ABP的默认表信息
            //modelBuilder.ChangeAbpTablePrefix<Tenant, Role, User>(string.Empty, AbpProEFCoreConsts.SchemaNames.ABP);
            base.OnModelCreating(modelBuilder);

            // ============  其它对表信息的修改 =============

            modelBuilder.ApplyConfiguration(new UserCfg());

            modelBuilder.ApplyConfiguration(new TenantCfg());

            modelBuilder.ApplyConfiguration(new DataFileObjectCfg());



            // ==================== Market ================================
            modelBuilder.ApplyConfiguration(new ProductSecretKeyCfg());
            modelBuilder.ApplyConfiguration(new ProductCfg());
            modelBuilder.ApplyConfiguration(new OrderCfg());

            modelBuilder.ApplyConfiguration(new CourseCfg());
            modelBuilder.ApplyConfiguration(new VideoResourceCfg());

            modelBuilder.ApplyConfiguration(new TencentOrderInfoCfg());











            // ==================== identityServer4 ================================
            modelBuilder.ConfigurePersistedGrantEntity();

            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
            });

            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.FriendUserId });
                b.HasIndex(e => new { e.FriendTenantId, e.UserId });
                b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
            });


            // ==================== MessageHistory ================================
            modelBuilder.ApplyConfiguration(new MessageHistoryCfg());



        }
    }
}
