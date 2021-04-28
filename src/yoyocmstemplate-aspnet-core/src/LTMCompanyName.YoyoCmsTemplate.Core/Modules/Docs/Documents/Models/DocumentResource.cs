namespace LTMCompanyName.YoyoCmsTemplate.Modules.yoyo.Docs.Documents.Models
{
    public class DocumentResource
    {
        public byte[] Content { get; }

        public DocumentResource(byte[] content)
        {
            Content = content;
        }
    }
}
