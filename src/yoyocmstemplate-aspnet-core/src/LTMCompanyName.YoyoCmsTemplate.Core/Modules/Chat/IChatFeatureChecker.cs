namespace LTMCompanyName.YoyoCmsTemplate.Modules.Chat
{
    public interface IChatFeatureChecker
    {
        void CheckChatFeatures(int? sourceTenantId, int? targetTenantId);
    }
}
