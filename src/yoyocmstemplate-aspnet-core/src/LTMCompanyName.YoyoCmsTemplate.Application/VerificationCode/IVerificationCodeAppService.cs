using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LTMCompanyName.YoyoCmsTemplate.VerificationCode
{
    public interface IVerificationCodeAppService : IApplicationService
    {
        Task SendSmsVerificationCode(string phoneNumber);
        Task SendMailVerificationCode(string mailAddress);

        Task<bool> Verify(string code);
    }
}
