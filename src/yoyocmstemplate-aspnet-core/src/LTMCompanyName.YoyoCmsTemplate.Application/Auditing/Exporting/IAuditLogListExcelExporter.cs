using System.Collections.Generic;
using L._52ABP.Application.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Auditing.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Auditing.Dtos.EntityChange;

namespace LTMCompanyName.YoyoCmsTemplate.Auditing.Exporting
{

    public interface IAuditLogListExcelExporter
    {

        FileDto ExportAuditLogToFile(List<AuditLogListDto> auditLogListDtos);

        FileDto ExportEntityChangeToFile(List<EntityChangeListDto> entityChangeListDtos);
    }
}
