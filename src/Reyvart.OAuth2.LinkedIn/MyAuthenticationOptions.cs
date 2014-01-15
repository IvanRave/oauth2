using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Reyvart.OAuth2.LinkedIn.Provider;

namespace Reyvart.OAuth2.LinkedIn
{
    /// <summary>
    /// Configuration options for <see cref="MyAuthenticationMiddleware"/>
    /// </summary>
    [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "LinkedIn.MyAuthenticationOptions.set_Caption(System.String)", Justification = "Not localizable.")]
    public class MyAuthenticationOptions : AuthenticationOptions
    {
        /// <summary>
        /// Initializes a new <see cref="MyAuthenticationOptions"/>
        /// </summary>
        public MyAuthenticationOptions()
            : base(Constants.Common.DefaultAuthenticationType)
        {
            // Different for different OAuth providers
            CallbackPath = new PathString(Constants.Common.CodeCallbackPath);
            Scope = string.Empty;
            Caption = Constants.Common.DefaultAuthenticationType;
            AuthenticationMode = AuthenticationMode.Passive;
            BackchannelTimeout = TimeSpan.FromSeconds(60);
        }

        /// <summary>
        /// Gets or sets the appId
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// Gets or sets the app secret
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        /// Gets or sets the a pinned certificate validator to use to validate the endpoints used
        /// in back channel communications belong to social network
        /// </summary>
        /// <value>
        /// The pinned certificate validator.
        /// </value>
        /// <remarks>If this property is null then the default certificate checks are performed,
        /// validating the subject name and if the signing chain is a trusted party.</remarks>
        public ICertificateValidator BackchannelCertificateValidator { get; set; }

        /// <summary>
        /// Gets or sets timeout value in milliseconds for back channel communications with social network
        /// </summary>
        /// <value>
        /// The back channel timeout in milliseconds.
        /// </value>
        public TimeSpan BackchannelTimeout { get; set; }

        /// <summary>
        /// The HttpMessageHandler used to communicate with social network.
        /// This cannot be set at the same time as BackchannelCertificateValidator unless the value 
        /// can be downcast to a WebRequestHandler.
        /// </summary>
        public HttpMessageHandler BackchannelHttpHandler { get; set; }

        /// <summary>
        /// Get or sets the text that the user can display on a sign in user interface.
        /// </summary>
        public string Caption
        {
            get { return Description.Caption; }
            set { Description.Caption = value; }
        }

        /// <summary>
        /// The request path within the application's base path where the user-agent will be returned.
        /// The middleware will process this request when it arrives.
        /// Default value is "/signin-external".
        /// </summary>
        public PathString CallbackPath { get; set; }

        /// <summary>
        /// Gets or sets the name of another authentication middleware which will be responsible for actually issuing a user <see cref="System.Security.Claims.ClaimsIdentity"/>.
        /// </summary>
        public string SignInAsAuthenticationType { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IMyAuthenticationProvider"/> used to handle authentication events.
        /// </summary>
        public IMyAuthenticationProvider Provider { get; set; }

        /// <summary>
        /// Gets or sets the type used to secure data handled by the middleware.
        /// </summary>
        public ISecureDataFormat<AuthenticationProperties> StateDataFormat { get; set; }

        /// <summary>
        /// Gets or sets the site redirect url after login 
        /// </summary>
        public string StoreState { get; set; }

        /// <summary>
        /// A list of permissions to request.
        /// Can be something like that "audio,video,pages" and etc.
        /// </summary>
        public string Scope { get; set; }
    }
}
