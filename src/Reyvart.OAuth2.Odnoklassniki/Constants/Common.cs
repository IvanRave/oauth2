namespace Reyvart.OAuth2.Odnoklassniki.Constants
{
    static class Common
    {
        public const string DefaultAuthenticationType = "Odnoklassniki";

        public const string DefaultResponseType = "code";

        public const string GrantType = "authorization_code";

        public const string CodeCallbackPath = "/signin-external-odnoklassniki";

        public const string CodeEndpoint = "https://www.odnoklassniki.ru/oauth/authorize";

        public const string TokenEndpoint = "https://api.odnoklassniki.ru/oauth/token.do";

        public const string GraphApiEndpoint = "http://api.odnoklassniki.ru/fb.do";
    }
}