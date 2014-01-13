namespace Reyvart.OAuth2.MailRu.Constants
{
    static class Common
    {
        public const string DefaultAuthenticationType = "MailRu";

        public const string DefaultResponseType = "code";

        public const string GrantType = "authorization_code";

        public const string CodeCallbackPath = "/signin-external-mailru";

        public const string CodeEndpoint = "https://connect.mail.ru/oauth/authorize";

        public const string TokenEndpoint = "https://connect.mail.ru/oauth/token";

        public const string GraphApiEndpoint = "https://www.appsmail.ru/platform/api";
    }
}