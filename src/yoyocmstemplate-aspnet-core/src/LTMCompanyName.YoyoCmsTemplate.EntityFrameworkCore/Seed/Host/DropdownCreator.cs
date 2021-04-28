using System.Linq;
using LTMCompanyName.YoyoCmsTemplate.Dropdown;
using LTMCompanyName.YoyoCmsTemplate.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Seed.Host
{
    /// <summary>
    /// 下拉列表的种子数据
    /// </summary>
    public class DropdownCreator
    {
        private readonly YoyoCmsTemplateDbContext _context;

        public DropdownCreator(YoyoCmsTemplateDbContext context)
        {
            _context = context;
        }

        //public void Create()
        //{
        //    CreateDDType();
        //    CreateDDList();
        //}

        ///// <summary>
        ///// 创建下拉分类
        ///// </summary>
        //private void CreateDDType()
        //{
     
        //    var allddType =_context.DropdownType.IgnoreQueryFilters().ToList();

        //    // 用户来源
        //    var userSource = allddType.FirstOrDefault(r => r.Id == "UserSource");
        //    if (userSource == null)
        //    {
        //        userSource = _context.DropdownType
        //            .Add(new Dropdown.DropdownType()
        //            {
        //                Id = "UserSource",
        //                Name_TX = "用户来源",
        //                IsActive_YN = true
        //            }).Entity;
        //    }
        //}

        ///// <summary>
        ///// 创建下拉内容
        ///// </summary>
        //private void CreateDDList()
        //{
        //    var allddList = _context.DropdownList.IgnoreQueryFilters().ToList();

        //    var qq = allddList.FirstOrDefault(r => r.Id == "UserSource_QQ");
        //    if (qq == null)
        //    {
        //        qq = AddDropdownList("UserSource", "QQ", "UserSource");
        //    }

        //    var wx = allddList.FirstOrDefault(r => r.Id == "UserSource_WX");
        //    if (wx == null)
        //    {
        //        wx = AddDropdownList("UserSource_WX", "微信", "UserSource");
        //    }
        //}

        //private DropdownList AddDropdownList(string id,string name,string ddTypeId)
        //{
        //    return _context.DropdownList
        //        .Add(new Dropdown.DropdownList()
        //        {
        //            Id = id,
        //            Name_TX = name,
        //            DDType_Id = ddTypeId,
        //            IsActive_YN = true
        //        }).Entity;
        //}
    }
}
