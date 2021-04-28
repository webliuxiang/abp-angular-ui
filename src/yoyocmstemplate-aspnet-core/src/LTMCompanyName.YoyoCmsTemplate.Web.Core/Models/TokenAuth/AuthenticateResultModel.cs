namespace LTMCompanyName.YoyoCmsTemplate.Models.TokenAuth
{
    public class AuthenticateResultModel
    {
        public string AccessToken { get; set; }

        public string EncryptedAccessToken { get; set; }

        public int ExpireInSeconds { get; set; }

        public long UserId { get; set; }

        public bool ShouldResetPassword { get; set; }

        public string PasswordResetCode { get; set; }

        public string ReturnUrl { get; set; }
        /// <summary>
        /// 需要进行账号绑定激活
        /// </summary>
        public bool WaitingForActivation { get; set; }
    }
}
