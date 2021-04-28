using System;
using System.Collections.Generic;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.yoyo.Docs.Documents
{
    public class DocumentStoreOptions
    {
        public Dictionary<string, Type> Stores { get; set; }

        public DocumentStoreOptions()
        {
            Stores = new Dictionary<string, Type>();
        }
    }
}
