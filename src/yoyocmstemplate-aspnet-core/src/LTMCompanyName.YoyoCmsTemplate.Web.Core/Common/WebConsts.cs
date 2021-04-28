namespace LTMCompanyName.YoyoCmsTemplate.Common
{
    public static class WebConsts
    {
        public const string SwaggerUiEndPoint = "/swagger";
        public const string HangfireDashboardEndPoint = "/hangfire";
        public const string HealthCheckEndPoint = "/healthz";
        public const string HealthCheckUIEndPoint = "/healthchecks-ui";

        public static bool SwaggerUiEnabled = true;

        public static bool HealthCheckEnabled = true;
        public static bool HealthCheckUIEnabled = true;

        /// <summary>
        /// 是否启用hangfir面板
        /// </summary>
        public static bool HangfireDashboardEnabled = true;

        public const string DefaultCorsPolicyName = "YoyoCmsTemplate";

        public static class GraphQL
        {
            public const string PlaygroundEndPoint = "/ui/playground";
            public const string EndPoint = "/graphql";

            public static bool PlaygroundEnabled = false;
            public static bool Enabled = false;
        }
    }
}