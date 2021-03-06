using System;
using Reyvart.OAuth2.MailRu;

namespace Owin
{
    /// <summary>
    /// Extension methods for using <see cref="MyAuthenticationMiddleware"/>
    /// </summary>
    public static class MyAuthenticationExtensions
    {
        /// <summary>
        /// Authenticate users using social network
        /// </summary>
        /// <param name="app">The <see cref="IAppBuilder"/> passed to the configuration method</param>
        /// <param name="options">Middleware configuration options</param>
        /// <returns>The updated <see cref="IAppBuilder"/></returns>
        public static IAppBuilder UseMailRuAuthentication(this IAppBuilder app, MyAuthenticationOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException("app");
            }
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            app.Use(typeof(MyAuthenticationMiddleware), app, options);
            return app;
        }

        /// <summary>
        /// Authenticate users using social network
        /// </summary>
        /// <param name="app">The <see cref="IAppBuilder"/> passed to the configuration method</param>
        /// <param name="appId">The appId assigned by social network</param>
        /// <param name="appSecret">The appSecret assigned by social network</param>
        /// <returns>The updated <see cref="IAppBuilder"/></returns>
        public static IAppBuilder UseMailRuAuthentication(
            this IAppBuilder app,
            string appId,
            string appSecret)
        {
            return UseMailRuAuthentication(
                app,
                new MyAuthenticationOptions
                {
                    AppId = appId,
                    AppSecret = appSecret
                });
        }
    }
}
