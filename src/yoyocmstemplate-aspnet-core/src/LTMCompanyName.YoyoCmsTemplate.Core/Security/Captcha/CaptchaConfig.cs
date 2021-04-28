namespace LTMCompanyName.YoyoCmsTemplate.Security.Captcha
{
    public class CaptchaConfig
    {
        /// <summary>
        /// 验证码是否启用Key
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 验证码类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 验证码长度
        /// </summary>
        public int Length { get; set; }
    }
}
