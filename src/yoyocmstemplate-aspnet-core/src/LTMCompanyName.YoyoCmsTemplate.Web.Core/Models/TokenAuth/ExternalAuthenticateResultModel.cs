namespace LTMCompanyName.YoyoCmsTemplate.Models.TokenAuth
{
    public class ExternalAuthenticateResultModel
    {
        public string AccessToken { get; set; }

        public string EncryptedAccessToken { get; set; }

        public int ExpireInSeconds { get; set; }

         /// <summary>
        /// 等待激活
        /// </summary>
        public bool WaitingForActivation { get; set; }

        /// <summary>
        /// 需要进行绑定账号
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 扩展登录的key
        /// </summary>
        public string ProviderKey { get; set; }

    }
}
