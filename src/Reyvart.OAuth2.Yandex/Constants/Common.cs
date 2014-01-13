namespace Reyvart.OAuth2.Yandex.Constants
{
    static class Common
    {
        public const string DefaultAuthenticationType = "Yandex";

        public const string DefaultResponseType = "code";

        public const string GrantType = "authorization_code";

        public const string CodeCallbackPath = "/signin-external-yandex";

        public const string CodeEndpoint = "https://oauth.yandex.ru/authorize";

        public const string TokenEndpoint = "https://oauth.yandex.ru/token";

        public const string GraphApiEndpoint = "https://login.yandex.ru/info";
    }
}