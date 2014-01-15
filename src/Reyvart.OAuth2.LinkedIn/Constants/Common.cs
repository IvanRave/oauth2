namespace Reyvart.OAuth2.LinkedIn.Constants
{
    static class Common
    {
        public const string DefaultAuthenticationType = "LinkedIn";

        public const string DefaultResponseType = "code";

        public const string GrantType = "authorization_code";

        public const string CodeCallbackPath = "/signin-external-linkedin";

        public const string CodeEndpoint = "https://www.linkedin.com/uas/oauth2/authorization";

        public const string TokenEndpoint = "https://www.linkedin.com/uas/oauth2/accessToken";

        public const string GraphApiEndpoint = "https://api.linkedin.com/v1/people/~";
    }
}