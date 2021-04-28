using System.ComponentModel;

namespace LTMCompanyName.YoyoCmsTemplate.WechatManagement
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MsgTypeEnum
    {
        [Description("文本消息")]
        Text = 1,
        [Description("图文消息")]
        ImageAndText = 2,
        [Description("图片消息")]
        Image = 3
    }
    /// <summary>
    /// 匹配模式
    /// </summary>
    public enum MatchModeEnum
    {
        [Description("精准匹配")]
        ExactMatch = 1,
        [Description("模糊匹配")]
        FuzzyMatch = 2,
    }
    /// <summary>
    /// 微信应用类型
    /// </summary>
    public enum WechatAppTypeEnum
    {
        [Description("订阅号")]
        订阅号 = 1,
        [Description("认证订阅号")]
        认证订阅号 = 2,
        [Description("服务号")]
        服务号 = 3,
        [Description("认证服务号")]
        认证服务号 = 4
    }

    /// <summary>
    /// 活动状态
    /// </summary>
    public enum ActivityStatusEnum
    {
        [Description("草稿")]
        Draft = 1,
        [Description("已发布")]
        Published = 2,
        [Description("已下架")]
        Unshelve = 3
    }

    /// <summary>
    /// 文章发布状态
    /// </summary>
    public enum ArticlePushStatusEnum
    {
        [Description("草稿")]
        Draft = 1,
        [Description("已发布")]
        Published = 2,
    }

    /// <summary>
    /// 微信个性化菜单 性别枚举
    /// </summary>
    public enum WechatMenuSexEnum
    {
        /// <summary>
        /// 男
        /// </summary>
        [Description("男")]
        M = 1,
        /// <summary>
        /// 女
        /// </summary>
        [Description("女")]
        F = 2,
    }

    /// <summary>
    /// 微信个性化菜单 客户端版本枚举
    /// </summary>
    public enum WechatMenuClientPlatformTypeEnum
    {
        [Description("苹果iOS")]
        IOS = 1,
        [Description("安卓")]
        Android = 2,
        [Description("其它")]
        Others = 3
    }

    /// <summary>
    /// 菜单按钮类型
    /// </summary>
    public enum WechatMenuTypeEnum
    {
        /// <summary>
        /// click - 点击事件(传回服务器)
        /// </summary>
        [Description("点击事件(传回服务器)")]
        click,
        /// <summary>
        /// view - 访问网页(直接跳转)
        /// </summary>
        [Description("访问网页(直接跳转)")]
        view,
        /// <summary>
        /// miniprogram - 小程序(直接跳转)
        /// </summary>
        [Description("小程序(直接跳转)")]
        miniprogram,
        /// <summary>
        ///  location_select - 弹出地理位置选择器
        /// </summary>
        [Description("弹出地理位置选择器")]
        location_select,
        /// <summary>
        ///pic_photo_or_album - 弹出拍照或者相册发图
        /// </summary>
        [Description("弹出拍照或者相册发图")]
        pic_photo_or_album,
        /// <summary>
        /// pic_sysphoto - 弹出系统拍照发图
        /// </summary>
        [Description("弹出系统拍照发图")]
        pic_sysphoto,
        /// <summary>
        /// pic_weixin - 弹出微信相册发图器
        /// </summary>
        [Description("弹出微信相册发图器")]
        pic_weixin,
        /// <summary>
        /// scancode_push - 扫码推事件
        /// </summary>
        [Description("扫码推事件")]
        scancode_push,
        /// <summary>
        /// scancode_waitmsg - 扫码推事件且弹出“消息接收中”提示框
        /// </summary>
        [Description("扫码推事件且弹出\"消息接收中\"提示框")]
        scancode_waitmsg,
        /// <summary>
        /// media_id - 下发消息(除文本消息)
        /// </summary>
        [Description("下发消息(除文本消息)")]
        media_id,
        /// <summary>
        /// view_limited - 跳转图文消息URL
        /// </summary>
        [Description("跳转图文消息URL")]
        view_limited,
    }
}
