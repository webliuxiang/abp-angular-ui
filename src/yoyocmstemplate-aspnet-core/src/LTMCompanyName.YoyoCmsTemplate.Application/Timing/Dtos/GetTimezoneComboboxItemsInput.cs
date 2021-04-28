using Abp.Configuration;


namespace LTMCompanyName.YoyoCmsTemplate.Timing.Dtos
{
    /// <summary>
    /// 获取时间控件的选择器
    /// </summary>
    public class GetTimezoneComboboxItemsInput
    {
        /// <summary>
        /// 默认时区范围
        /// </summary>
        public SettingScopes DefaultTimezoneScope;

        public string SelectedTimezoneId { get; set; }
    }
}
