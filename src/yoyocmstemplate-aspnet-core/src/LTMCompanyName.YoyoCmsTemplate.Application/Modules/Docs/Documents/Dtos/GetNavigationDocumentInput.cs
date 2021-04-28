using System;
using System.ComponentModel.DataAnnotations;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Documents.Dtos
{
    public class GetNavigationDocumentInput
    {
        public Guid ProjectId { get; set; }

        [StringLength(ProjectConsts.MaxVersionNameLength)]
        public string Version { get; set; }
    }
}
