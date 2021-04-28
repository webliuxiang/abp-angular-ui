namespace LTMCompanyName.YoyoCmsTemplate.HostManagement.Settings.Dtos
{
    public class GeneralSettingsEditDto
    {
        /// <summary>
        /// 时区
        /// </summary>
        public string Timezone { get; set; }

        /// <summary>
        /// 这个字段只用于比较用户的时区与默认时区
        /// </summary>
        public string TimezoneForComparison { get; set; }
    }
}
