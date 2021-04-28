namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Models.Account
{

    /// <summary>
    /// 注册成功后的返回值内容
    /// </summary>
    public class RegisterResultViewModel
    {
        public string TenancyName { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public string NameAndSurname { get; set; }

        public bool IsActive { get; set; }

        public bool IsEmailConfirmationRequiredForLogin { get; set; }

        public bool IsEmailConfirmed { get; set; }
    }
}
