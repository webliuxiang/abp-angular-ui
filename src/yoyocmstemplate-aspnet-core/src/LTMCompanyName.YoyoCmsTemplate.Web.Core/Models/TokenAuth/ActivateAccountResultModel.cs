namespace LTMCompanyName.YoyoCmsTemplate.Models.TokenAuth
{
    public class ActivateAccountResultModel
    {
        public string AccessToken { get; set; }

        public string EncryptedAccessToken { get; set; }

        public int ExpireInSeconds { get; set; }

        public long UserId { get; set; }
    }
}
