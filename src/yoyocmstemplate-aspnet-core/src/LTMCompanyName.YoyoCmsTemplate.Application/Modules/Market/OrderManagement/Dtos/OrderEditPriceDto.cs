using System;
using System.Collections.Generic;
using System.Text;

using Abp.Application.Services.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.OrderManagement.Dtos
{
    public class OrderEditPriceDto : EntityDto<Guid>
    {
        /// <summary>
        /// 实付金额
        /// </summary>
        public decimal ActualPayment { get; set; }
    }
}
