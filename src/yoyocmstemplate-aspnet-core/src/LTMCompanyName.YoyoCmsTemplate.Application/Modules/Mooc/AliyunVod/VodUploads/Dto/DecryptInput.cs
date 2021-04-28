namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.AliyunVod.VodUploads.Dto
{
    public class DecryptInput
    {
        /// <summary>
        /// 密文
        /// </summary>
        public string Ciphertext { get; set; }

        /// <summary>
        /// 自定义验证tocken
        /// </summary>
        public string MtsHlsUriToken { get; set; }

    }
}
