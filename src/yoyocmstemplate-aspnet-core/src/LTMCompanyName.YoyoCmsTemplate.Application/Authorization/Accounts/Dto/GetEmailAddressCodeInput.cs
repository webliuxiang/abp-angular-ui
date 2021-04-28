namespace LTMCompanyName.YoyoCmsTemplate.Authorization.Accounts.Dto
{
    /// <summary>
    ///获取邮箱验证码
    /// </summary>
    public class GetEmailAddressCodeInput
    {
        public string EmailAddress { get; set; }


        public string ConfirmationCode { get; set; }
         

    }
}
