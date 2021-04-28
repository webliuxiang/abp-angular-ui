using Abp.Extensions;
using Newtonsoft.Json;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Documents.Dtos
{
    public class NavigationWithDetailsDto : DocumentWithDetailsDto
    {
        [JsonProperty("items")]
        public NavigationNode RootNode { get; set; }

        public void ConvertItems()
        {
            if ( Content.IsNullOrEmpty())
            {
                RootNode = new NavigationNode();
                return;
            }

            try
            {
                RootNode = JsonConvert.DeserializeObject<NavigationNode>(Content);
            }
            catch (JsonException)
            {
                //todo: should log the exception?
                RootNode = new NavigationNode();
            }
        }
    }
}
