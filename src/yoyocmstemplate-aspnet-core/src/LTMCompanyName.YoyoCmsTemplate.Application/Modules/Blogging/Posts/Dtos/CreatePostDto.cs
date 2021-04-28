using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Posts;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Posts.Enums;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Posts.Dtos
{
    public class CreatePostDto
    { /// <summary>
      /// 文章的列表DTO
      /// <see cref="Post"/>
      /// </summary>


        /// <summary>
        /// Id
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// 博客Id
        /// </summary>
        public Guid BlogId { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [MaxLength(500, ErrorMessage = "地址超出最大长度")]
        public string Url { get; set; }


        /// <summary>
        /// 封面
        /// </summary>
        [MaxLength(500, ErrorMessage = "封面超出最大长度")]
        public string CoverImage { get; set; }

        /// <summary>
        /// 历史内容
        /// </summary>
        public   string HistoryContent { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [MaxLength(250, ErrorMessage = "标题超出最大长度")]
        [Required(ErrorMessage = "标题不能为空")]
        public string Title { get; set; }



        /// <summary>
        /// 内容
        /// </summary>
        [Required(ErrorMessage = "内容不能为空")]
        public string Content { get; set; }




        /// <summary>
        /// 文章类型
        /// </summary>
        [Required(ErrorMessage = "文章类型不能为空")]
        public PostType PostType { get; set; }






        /// <summary>
        /// 文章标签
        /// </summary>
        public List<string> NewTags { get; set; }





        //// custom codes



        //// custom codes end

    }
}
