namespace Reyvart.OAuth2.LinkedIn.Constants
{
    /// <summary>
    /// User get values
    /// http://developer.linkedin.com/documents/profile-api
    /// </summary>
    static class UserGet
    {
        public const string Uid = "id";

        public const string FirstName = "first-name";

        public const string LastName = "last-name";

        /// <summary>
        /// No nick name in linkedIn: use FirstName + LastName
        /// </summary>
        public const string NickName = "";

        public const string PhotoLink = "picture-url";
    }
}
