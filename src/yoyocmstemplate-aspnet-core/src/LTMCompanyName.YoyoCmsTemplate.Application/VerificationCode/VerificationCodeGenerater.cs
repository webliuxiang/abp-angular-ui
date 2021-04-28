using OtpNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace LTMCompanyName.YoyoCmsTemplate.VerificationCode
{
    public static class VerificationCodeGenerater
    {
        public static string GenerateCode()
        {
            var totp = new Totp(Encoding.UTF8.GetBytes("52abp"), 60 * 30);
            return totp.ComputeTotp();
        }
        public static bool Verify(string code)
        {
            var totp = new Totp(Encoding.UTF8.GetBytes("52abp"), 60 * 30);
            return totp.VerifyTotp(code,out long timeStepMatched);
        }
    }
}
