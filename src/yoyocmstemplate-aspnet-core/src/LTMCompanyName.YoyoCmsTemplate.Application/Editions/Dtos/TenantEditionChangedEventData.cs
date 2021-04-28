﻿using Abp.Events.Bus;

namespace LTMCompanyName.YoyoCmsTemplate.Editions.Dtos
{
    public class TenantEditionChangedEventData : EventData
    {
        public int TenantId { get; set; }

        public int? OldEditionId { get; set; }

        public int? NewEditionId { get; set; }
    }
}