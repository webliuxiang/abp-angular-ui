using System;
using System.Collections.Generic;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Documents.Dtos
{
    [Serializable]
    public class DocumentWithDetailsDto
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string Format { get; set; }

        public string EditLink { get; set; }

        public string RootUrl { get; set; }

        public string RawRootUrl { get; set; }

        public string Version { get; set; }

        public string LocalDirectory { get; set; }

        public string FileName { get; set; }
      

        public List<DocumentContributorDto> Contributors { get; set; }

        public bool SuccessfullyRetrieved { get; set; }

    }
}
