using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using System.Xml;
using Microsoft.Owin;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Logging;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using Newtonsoft.Json;
using Reyvart.OAuth2.Odnoklassniki.Provider;
using System.Text;

namespace Reyvart.OAuth2.Odnoklassniki
{
    public class MyAuthenticationHandler : AuthenticationHandler<MyAuthenticationOptions>
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;

        public MyAuthenticationHandler(HttpClient httpClient, ILogger logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        //<summary>step 1
        //called at the end of server request after site controllers
        //if client not autorized 401 - redirect to social network - It is start point of the authorization process
        //Redirect user to social network where he need loging and allow access to your app
        //after that redirect back to {host}/signin-external
        //</summary
        protected override Task ApplyResponseChallengeAsync()
        {
            if (Response.StatusCode != 401)
            {
                return Task.FromResult<object>(null);
            }

            //Helper checking if that module called for login
            AuthenticationResponseChallenge challenge = Helper.LookupChallenge(Options.AuthenticationType, Options.AuthenticationMode);

            if (challenge != null)
            {
                string baseUri =
                    Request.Scheme +
                    Uri.SchemeDelimiter +
                    Request.Host +
                    Request.PathBase;

                string currentUri =
                    baseUri +
                    Request.Path +
                    Request.QueryString;

                string redirectUri =
                    baseUri +
                    Options.CallbackPath;

                AuthenticationProperties properties = challenge.Properties;
                if (string.IsNullOrEmpty(properties.RedirectUri))
                {
                    properties.RedirectUri = currentUri;
                }

                // OAuth2 10.12 CSRF
                GenerateCorrelationId(properties);

                string state = Options.StateDataFormat.Protect(properties);

                Options.StoreState = state;

                string authorizationEndpoint =
                    Constants.Common.CodeEndpoint +
                        "?client_id=" + Uri.EscapeDataString(Options.AppId) +
                        "&redirect_uri=" + Uri.EscapeDataString(redirectUri) +
                        "&scope=" + Uri.EscapeDataString(Options.Scope) +
                        "&response_type=" + Constants.Common.DefaultResponseType;

                Response.Redirect(authorizationEndpoint);
            }

            return Task.FromResult<object>(null);
        }

        //<summary>step 2.0
        //Called at start of page request, before site controllers
        //</summary>
        public override async Task<bool> InvokeAsync()
        {
            return await InvokeReplyPathAsync();
        }

        //step 2.1
        //called at start of page request - checking if request match with "{host}/signin-external" url {?code=*******************}
        //if matched - making AuthenticationTicket 
        private async Task<bool> InvokeReplyPathAsync()
        {
            if (Options.CallbackPath.HasValue && Options.CallbackPath == Request.Path)
            {
                AuthenticationTicket ticket = await AuthenticateAsync(); //call Task<AuthenticationTicket> AuthenticateCoreAsync() step 2.3
                if (ticket == null)
                {
                    _logger.WriteWarning("Invalid return state, unable to redirect.");
                    Response.StatusCode = 500;
                    return true;
                }

                var context = new MyReturnEndpointContext(Context, ticket);
                context.SignInAsAuthenticationType = Options.SignInAsAuthenticationType;
                context.RedirectUri = ticket.Properties.RedirectUri;

                await Options.Provider.ReturnEndpoint(context);


                if (context.SignInAsAuthenticationType != null &&
                    context.Identity != null)
                {
                    ClaimsIdentity grantIdentity = context.Identity;
                    if (!string.Equals(grantIdentity.AuthenticationType, context.SignInAsAuthenticationType, StringComparison.Ordinal))
                    {
                        grantIdentity = new ClaimsIdentity(grantIdentity.Claims, context.SignInAsAuthenticationType, grantIdentity.NameClaimType, grantIdentity.RoleClaimType);
                    }
                    Context.Authentication.SignIn(context.Properties, grantIdentity);
                }

                if (!context.IsRequestCompleted && context.RedirectUri != null)
                {
                    string redirectUri = context.RedirectUri;
                    if (context.Identity == null)
                    {
                        // add a redirect hint that sign-in failed in some way
                        redirectUri = WebUtilities.AddQueryString(redirectUri, "error", "access_denied");
                    }
                    Response.Redirect(redirectUri);
                    context.RequestCompleted();
                }

                return context.IsRequestCompleted;
            }

            return false;
        }

        //step 2.3
        //making AuthenticationTicket after client return from social netwrork
        //here we make actually autorization work
        protected override async Task<AuthenticationTicket> AuthenticateCoreAsync()
        {
            AuthenticationProperties properties = null;
            try
            {
                string code = "";

                IReadableStringCollection query = Request.Query;
                IList<string> values = query.GetValues("code");

                if (values != null && values.Count == 1)
                {
                    code = values[0];
                }

                properties = Options.StateDataFormat.Unprotect(Options.StoreState);
                if (properties == null)
                {
                    return null;
                }

                // OAuth2 10.12 CSRF
                if (!ValidateCorrelationId(properties, _logger))
                {
                    return new AuthenticationTicket(null, properties);
                }

                string requestPrefix = Request.Scheme + Uri.SchemeDelimiter + Request.Host;
                string redirectUri = requestPrefix + Request.PathBase + Options.CallbackPath;

                string tokenRequestUrl = Constants.Common.TokenEndpoint;


                string tokenBody =
                    "code=" + Uri.EscapeDataString(code) +
                    "&redirect_uri=" + Uri.EscapeDataString(redirectUri) +
                    "&grant_type=" + Constants.Common.GrantType +
                    "&client_id=" + Uri.EscapeDataString(Options.AppId) +
                    "&client_secret=" + Uri.EscapeDataString(Options.AppSecret);

                HttpContent tokenContent = new StringContent(tokenBody);
                tokenContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");

                HttpResponseMessage tokenResponse = await _httpClient.PostAsync(tokenRequestUrl, tokenContent);
                //await _httpClient.GetAsync(tokenRequestUrl, Request.CallCancelled);
                tokenResponse.EnsureSuccessStatusCode();
                string text = await tokenResponse.Content.ReadAsStringAsync();
                //IFormCollection form = WebHelpers.ParseForm(text);
                var JsonResponse = JsonConvert.DeserializeObject<dynamic>(text);
                //JObject TokenResponse = JObject.Parse(text);

                string accessToken = JsonResponse["access_token"];

                // Params
                SortedDictionary<string, string> nvc = new SortedDictionary<string, string>();

                nvc.Add("method", Constants.UserGet.MethodName);
                nvc.Add("application_key", Options.AppPublicKey);
                nvc.Add("fields", string.Join(",", new string[] {
                    Constants.UserGet.FirstName, 
                    Constants.UserGet.LastName, 
                    Constants.UserGet.NickName,
                    Constants.UserGet.PhotoLink,
                    Constants.UserGet.Uid}));
                nvc.Add("format", "xml");

                // sig = md5( request_params_composed_string+ md5(access_token + application_secret_key))
                // http://apiok.ru/wiki/display/api/Authentication+and+Authorization (make it lower case)
                //// sig = md5( request_params_composed_string+ md5(access_token + application_secret_key))

                StringBuilder paramString = new StringBuilder();
                foreach (var item in nvc)
                {
                    paramString.Append(item.Key + "=" + item.Value);
                }

                string sig = Helpers.CryptoHelper.GetMd5Hash(paramString.ToString() + Helpers.CryptoHelper.GetMd5Hash(accessToken + Options.AppSecret));

                _logger.WriteInformation("sig: " + sig);

                // Add access_token and sig after calculate
                nvc.Add("access_token", Uri.EscapeDataString(accessToken));
                nvc.Add("sig", sig);

                string resultQueryStr = string.Join("&", nvc.Select(c => c.Key + "=" + c.Value).ToArray());

                // public method which dont require token
                string userInfoLink = Constants.Common.GraphApiEndpoint + "?" + resultQueryStr;

                HttpResponseMessage graphResponse = await _httpClient.GetAsync(userInfoLink, Request.CallCancelled);
                graphResponse.EnsureSuccessStatusCode();
                text = await graphResponse.Content.ReadAsStringAsync();
                XmlDocument UserInfoResponseXml = new XmlDocument();
                UserInfoResponseXml.LoadXml(text);

                // Access token has limited lifetime, about 30 minutes: http://apiok.ru/wiki/display/api/OAuth+2.0
                var context = new MyAuthenticatedContext(Context, UserInfoResponseXml, accessToken, "1800");
                context.Identity = new ClaimsIdentity(
                    Options.AuthenticationType,
                    ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);

                const string XmlSchemaString = "http://www.w3.org/2001/XMLSchema#string";

                if (!string.IsNullOrEmpty(context.Id))
                {
                    context.Identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, context.Id, XmlSchemaString, Options.AuthenticationType));
                }
                if (!string.IsNullOrEmpty(context.DefaultName))
                {
                    context.Identity.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, context.DefaultName, XmlSchemaString, Options.AuthenticationType));
                }
                if (!string.IsNullOrEmpty(context.FullName))
                {
                    context.Identity.AddClaim(new Claim("urn:odnoklassniki:name", context.FullName, XmlSchemaString, Options.AuthenticationType));
                }
                if (!string.IsNullOrEmpty(context.PhotoLink))
                {
                    context.Identity.AddClaim(new Claim("urn:odnoklassniki:link", context.PhotoLink, XmlSchemaString, Options.AuthenticationType));
                }
                context.Properties = properties;

                await Options.Provider.Authenticated(context);

                return new AuthenticationTicket(context.Identity, context.Properties);

            }
            catch (Exception ex)
            {
                _logger.WriteError(ex.Message);
                return new AuthenticationTicket(null, properties);
            }
        }
    }
}
