namespace Reyvart.OAuth2.Vkontakte.Constants
{
    static class Common
    {
        public const string DefaultAuthenticationType = "Vkontakte";

        public const string ApiVersion = "5.5";

        public const string DefaultResponseType = "code";

        public const string CodeEndpoint = "https://oauth.vk.com/authorize";

        public const string CodeCallbackPath = "/signin-external-vkontakte";

        public const string TokenEndpoint = "https://oauth.vk.com/access_token";

        public const string GraphApiEndpoint = "https://api.vk.com/method";
    }
}
