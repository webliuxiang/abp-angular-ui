using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using LTMCompanyName.YoyoCmsTemplate.PhoneBooks.Persons;

namespace LTMCompanyName.YoyoCmsTemplate.PhoneBooks.Books
{
    /// <summary>
    /// 书籍
    /// </summary>
    public class Book : Entity<long>
    {
        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        /// <summary>
        /// 书籍类型
        /// </summary>
        public BookType Type { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime PublishDate { get; set; }
        /// <summary>
        /// 是否上架
        /// </summary>
        public bool IsShangjia { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public float Price { get; set; }
        /// <summary>
        /// 库存
        /// </summary>
        public long Kucun { get; set; }
        /// <summary>
        /// 借阅人
        /// </summary>
        public ICollection<Person> People { get; set; }

    }

    public enum BookType
    {
        [Description("未分类")]
        Undefined,

        [Description("冒险")]
        Advanture,

        [Description("传记")]
        Biography,

        [Description("反乌托邦")]
        Dystopia,

        [Description("奇妙")]
        Fantastic,

        [Description("恐怖")]
        Horror,

        [Description("科学")]
        Science,

        [Description("科幻小说")]
        ScienceFiction,

        [Description("诗歌")]
        Poetry
    }
}