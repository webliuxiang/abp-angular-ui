namespace LTMCompanyName.YoyoCmsTemplate.Security.PasswordComplexity
{
    public class PasswordComplexitySetting
    {
        public bool Equals(PasswordComplexitySetting other)
        {
            if (other == null)
            {
                return false;
            }

            return
                RequireDigit == other.RequireDigit &&
                RequireLowercase == other.RequireLowercase &&
                RequireNonAlphanumeric == other.RequireNonAlphanumeric &&
                RequireUppercase == other.RequireUppercase &&
                RequiredLength == other.RequiredLength;
        }
        /// <summary>
        /// 必须有数字
        /// </summary>
        public bool RequireDigit { get; set; }
        /// <summary>
        /// 必须有小写字母
        /// </summary>
        public bool RequireLowercase { get; set; }

        /// <summary>
        /// 需要非字母数字
        /// </summary>
        public bool RequireNonAlphanumeric { get; set; }

        /// <summary>
        /// 必须有大写字母
        /// </summary>
        public bool RequireUppercase { get; set; }

        /// <summary>
        /// 长度必须大于
        /// </summary>
        public int RequiredLength { get; set; }
    }
}
