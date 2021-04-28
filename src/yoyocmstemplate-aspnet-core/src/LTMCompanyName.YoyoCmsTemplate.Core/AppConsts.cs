namespace LTMCompanyName.YoyoCmsTemplate
{

    /// <summary>
    /// YoyoCmsTemplate的常量字段
    /// </summary>
    public class AppConsts
    {


        #region 内置角色


        /// <summary>
        /// 教师角色
        /// </summary>
        public const string TeacherRoleName = "Teacher";
        /// <summary>
        /// 学生角色
        /// </summary>
        public const string StudentRoleName = "Student";
        /// <summary>
        /// Admin角色
        /// </summary>
        public const string AdminRoleName = "Admin";

        #endregion



        /// <summary>
        /// MVC中域的常量名称
        /// </summary>
        public const string AreasAdminName = "AreasAdminName";


        /// <summary>
        /// 门户端的管理中心常量
        /// </summary>
        public const string PortalAdminName = "Admin";

        /// <summary>
        /// 语言文件的名称
        /// </summary>
        public const string LocalizationSourceName = "YoyoCmsTemplate";

        /// <summary>
        /// 默认语言
        /// </summary>
        public const string DefaultLanguage = "zh-Hans";

        /// <summary>
        /// 多租户请求头名称
        /// </summary>
        public const string TenantIdResolveKey = "Tenant";

        /// <summary>
        /// 默认一页显示多少条数据
        /// </summary>
        public const int DefaultPageSize = 10;

        /// <summary>
        /// 一次性最多可以分多少页
        /// </summary>
        public const int MaxPageSize = 1000;
        /// <summary>
        /// 邮件地址最大长度
        /// </summary>
        public const int MaxEmailAddressLength = 250;

        /// <summary>
        /// 个人头像图片大小最多不超过1M
        /// </summary>

        public const int ResizedMaxProfilPictureBytesUserFriendlyValue = 1024;

        /// <summary>
        /// 文件最大不超过50M
        /// </summary>
        public static long MaxFileSize = 1024 * 1024 * 50;


        /// <summary>
        /// 名字最大长度
        /// </summary>
        public const int MaxNameLength = 50;


        public const int MaxAddressLength = 250;

        public const int PaymentCacheDurationInMinutes = 30;

        /// <summary>
        ///     Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public const string DefaultPassPhrase = "gsKxGZ012HLL3MI5";


        public const int MaxProfilePictureBytesUserFriendlyValue = 5;

        public const int MaxListContentLength = 150;

        public const string PageviewKey = "PageviewKey";

        /// <summary>
        /// 表的前缀名称
        /// </summary>
        public const string TablePrefix = "Ltm";


        public const string Currency = "USD";

        public const string CurrencySign = "$";


        public const string AddressLinkagePacsPath = "\\AddressLinkage\\pacs-code.json";  //联动数据省市区县

        public const string AddressLinkageProvincesPath = "\\AddressLinkage\\provinces.json";  //联动数据省

        public const string AddressLinkageCitiesPath = "\\AddressLinkage\\cities.json";  //联动数据市

        public const string AddressLinkageAreasPath = "\\AddressLinkage\\areas.json";  //联动数据县

        public const string AddressLinkageStreetsPath = "\\AddressLinkage\\streets.json";  //联动数据镇



        /// <summary>
        /// 下载信息Session
        /// </summary>
        public const string DownloadFileCode = "DownloadFileCode";

    }

    /// <summary>
    /// 缓存常量
    /// </summary>
    public static class CacheConsts
    {
        public const string HostApi = "HostApi";

        public const string HostApi_TokenAuth_Authenticate = "HostApi.TokenAuth.Authenticate";

        public const string Portal = "Portal";

        public const string Portal_About = "Portal.About";

        public const string Portal_Account_Portal = "Portal.Account.ResetPassword";


        public const string Portal_Wiki = "Portal.Wiki";

    }

}
