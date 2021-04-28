using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Newtonsoft.Json;

namespace LTMCompanyName.YoyoCmsTemplate.Notifications.SmsMessage
{
    public class SmsMessage : YoyoCmsTemplateDomainServiceBase, ISmsMessage
    {
        public Task SendMessage(string phoneNumber, string templateCode, IDictionary<string, string> smsParams)
        {
            SendAcs(phoneNumber, templateCode, smsParams);
            return Task.CompletedTask;
        }
        private static void SendAcs(string phoneNumber, string templateCode, IDictionary<string, string> smsParams)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                throw new Exception("phoneNumber不能为空");
            }

            //string product = "Dysmsapi";//短信API产品名称（短信产品名固定，无需修改）
            string domain = "dysmsapi.aliyuncs.com";//短信API产品域名（接口地址固定，无需修改）
            string accessKeyId = " ";//你的accessKeyId
            string accessKeySecret = " ";//你的accessKeySecret

            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", accessKeyId, accessKeySecret);
            //IAcsClient client = new DefaultAcsClient(profile);
            // SingleSendSmsRequest request = new SingleSendSmsRequest();
            //初始化ascClient,暂时不支持多region（请勿修改）
            //DefaultProfile.AddEndpoint("cn-hangzhou", "cn-hangzhou", product, domain);

            IAcsClient acsClient = new DefaultAcsClient(profile);
            CommonRequest request = new CommonRequest();

            try
            {
                request.Method = Aliyun.Acs.Core.Http.MethodType.POST;
                request.Domain = domain;
                request.Version = "2017-05-25";
                request.Action = "SendSms";
                request.AddQueryParameters("RegionId", "cn-hangzhou");
                request.AddQueryParameters("PhoneNumbers", phoneNumber);  //必填:待发送手机号。支持以逗号分隔的形式进行批量调用，批量上限为1000个手机号码,批量调用相对于单条调用及时性稍有延迟,验证码类型的短信推荐使用单条调用的方式，发送国际/港澳台消息时，接收号码格式为00+国际区号+号码，如“0085200000000”
                request.AddQueryParameters("SignName", "52abp");  //必填:短信签名-可在短信控制台中找到
                request.AddQueryParameters("TemplateCode", templateCode); //必填:短信模板-可在短信控制台中找到，发送国际/港澳台消息时，请使用国际/港澳台短信模版
                request.AddQueryParameters("TemplateParam", JsonConvert.SerializeObject(smsParams)); //可选:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为

                var response = acsClient.GetCommonResponse(request);
            }
            catch (ServerException ex)
            {
                throw new ApplicationException(ex.Message);
            }
            catch (ClientException ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }
    }
}
