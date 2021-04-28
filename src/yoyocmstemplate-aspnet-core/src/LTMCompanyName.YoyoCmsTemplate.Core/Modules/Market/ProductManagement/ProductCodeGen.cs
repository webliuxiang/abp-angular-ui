using System;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductManagement
{
    public class ProductCodeGen
    {
        /// <summary>
        /// 生成产品编码
        /// 
        /// 规则: 产品类型(4位)-时间戳(秒级)-创建产品用户Id(4位)
        /// </summary>
        /// <param name="productType">订单来源</param>
        /// <param name="userId">创建产品用户Id+1</param>
        /// <returns></returns>
        public static string GetCode(int productType, long userId)
        {
            // 产品类型(4位)-时间戳(秒级)-创建产品用户Id(4位)

            var timestamp = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;


           


            var code = $"{EnumFormart(productType)}{timestamp}{UserIdFormart(userId)}";

            return code;
        }

        private static string UserIdFormart(long userId)
        {
            var userIdStr = userId.ToString();

            switch (userIdStr.Length)
            {
                case 1:
                    userIdStr = $"000{userIdStr}";
                    break;
                case 2:
                    userIdStr = $"00{userIdStr}";
                    break;
                case 3:
                    userIdStr = $"0{userIdStr}";
                    break;
            }

            return userIdStr;
        }

        private static string EnumFormart(int enumNum)
        {
            var enumNumStr = enumNum.ToString();
            switch (enumNumStr.Length)
            {
                case 1:
                    enumNumStr = $"000{enumNumStr}";
                    break;
                case 2:
                    enumNumStr = $"00{enumNumStr}";
                    break;
                case 3:
                    enumNumStr = $"0{enumNumStr}";
                    break;
            }

            return enumNumStr;
        }

    }
}
