using System.Globalization;

namespace LTMCompanyName.YoyoCmsTemplate.Localization
{

    /// <summary>
    /// 本地化多语言帮助类
    /// </summary>
    public static class CultureHelper
    {
        public static CultureInfo[] Cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);

        public static bool IsRtl => CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft;



        public static bool UsingLunarCalendar = CultureInfo.CurrentUICulture.DateTimeFormat.Calendar.AlgorithmType == CalendarAlgorithmType.LunarCalendar;

        public static CultureInfo GetCultureInfo(string name)
        {
            try
            {
                return CultureInfo.GetCultureInfo(name);
            }
            catch (CultureNotFoundException)
            {
                return CultureInfo.CurrentCulture;
            }
        }

    }
}
