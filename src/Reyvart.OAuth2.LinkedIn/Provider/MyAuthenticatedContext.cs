using System;
using System.Globalization;
using System.Security.Claims;
using System.Xml;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Provider;

namespace Reyvart.OAuth2.LinkedIn.Provider
{
    /// <summary>
    /// Contains information about the login session as well as the user <see cref="System.Security.Claims.ClaimsIdentity"/>.
    /// </summary>
    public class MyAuthenticatedContext : BaseContext
    {
        /// <summary>
        /// Initializes a <see cref="MyAuthenticatedContext"/>
        /// </summary>
        /// <param name="context">The OWIN environment</param>
        /// <param name="userxml">The XML document with user info</param>
        /// <param name="accessToken">Access token</param>
        /// <param name="expires">Seconds until expiration</param>
        public MyAuthenticatedContext(IOwinContext context, XmlDocument userxml, string accessToken, string expires)
            : base(context)
        {
            UserXml = userxml;
            AccessToken = accessToken;

            int expiresValue;
            if (Int32.TryParse(expires, NumberStyles.Integer, CultureInfo.InvariantCulture, out expiresValue))
            {
                ExpiresIn = TimeSpan.FromSeconds(expiresValue);
            }

            Id = TryGetValue(Constants.UserGet.Uid);
            Name = TryGetValue(Constants.UserGet.FirstName);
            LastName = TryGetValue(Constants.UserGet.LastName);
            Nickname = TryGetValue(Constants.UserGet.NickName);
            PhotoLink = TryGetValue(Constants.UserGet.PhotoLink);
        }

        /// <summary>
        /// Gets the document with user info
        /// </summary>
        public XmlDocument UserXml { get; private set; }

        /// <summary>
        /// Gets the access token
        /// </summary>
        public string AccessToken { get; private set; }

        /// <summary>
        /// Gets the access token expiration time
        /// </summary>
        public TimeSpan? ExpiresIn { get; set; }

        /// <summary>
        /// Gets the user ID
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Gets the user's name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the user's last name
        /// </summary>
        public string LastName { get; private set; }

        /// <summary>
        /// Gets the user's full name
        /// </summary>
        public string FullName
        {
            get
            {
                return Name + " " + LastName;
            }
        }


        /// <summary>
        /// Gets the user's DefaultName
        /// </summary>
        public string DefaultName
        {
            get
            {
                if (!String.IsNullOrEmpty(UserName))
                    return UserName;

                if (!String.IsNullOrEmpty(Nickname))
                    return Nickname;

                return FullName;
            }
        }

        /// <summary>
        /// Gets the user's picture link
        /// </summary>
        public string PhotoLink { get; private set; }

        /// <summary>
        /// Gets the username
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// Gets the Nickname
        /// </summary>
        public string Nickname { get; private set; }

        /// <summary>
        /// Gets the <see cref="ClaimsIdentity"/> representing the user
        /// </summary>
        public ClaimsIdentity Identity { get; set; }

        /// <summary>
        /// Gets or sets a property bag for common authentication properties
        /// </summary>
        public AuthenticationProperties Properties { get; set; }

        private string TryGetValue(string propertyName)
        {
            XmlNodeList elemList = UserXml.GetElementsByTagName(propertyName);
            if (elemList != null)
            {
                if (elemList[0] != null)
                    return elemList[0].InnerText.Trim();
            }

            return String.Empty;
        }
    }
}