oauth2
======

Auth modules for ASP.NET MVC5 OWIN projects

Use next methods:

- app.UseVkontakteAuthentication(appId, secretKey);

- app.UseMailRuAuthentication(appId, secretKey);

- app.UseOdnoklassnikiAuthentication(appId, secretKey, publicKey); 

- app.UseYandexAuthentication(appId, appPassword);

- app.UseLinkedInAuthentication(appId, appSecret);
 

##App registration links:

- http://vk.com/editapp?act=create
- http://api.mail.ru/sites/my/add/
- http://www.odnoklassniki.ru/devaccess
- https://oauth.yandex.ru/client/new
- https://www.linkedin.com/secure/developer

##Steps

###Get auth code (GET)

<table>
<tr>
  <td>VK</td>
  <td>MailRu</td>
  <td>Odnoklassniki</td>
</tr>
<tr>
<td>https://oauth.vk.com/authorize</td>
<td>https://connect.mail.ru/oauth/authorize</td>
<td>http://www.odnoklassniki.ru/oauth/authorize</td>
</tr>
<tr>
<td>
<ul>
  <li>client_id=APP_ID</li>
  <li>redirect_uri=REDIRECT_URI</li>
  <li>scope=PERMISSIONS</li>
  <li>response_type=code</li>
  <li>v=API_VERSION</li>
</ul>
</td>
<td>
<ul>
  <li>client_id=APP_ID</li>
  <li>redirect_uri=REDIRECT_URI</li>
  <li>scope=PERMISSIONS</li>
  <li>response_type=code</li>
</ul>
</td>
<td>
<ul>
  <li>client_id=APP_ID</li>
  <li>redirect_uri=REDIRECT_URI</li>
  <li>scope=PERMISSIONS</li>
  <li>response_type=code</li>
</ul>
</td>
</table>

### Change code to access token

<table>
<tr>
  <td>VK</td>
  <td>MailRu</td>
  <td>Odnoklassniki</td>
</tr>
<tr>
  <td>GET: https://oauth.vk.com/access_token</td>
  <td>POST: https://connect.mail.ru/oauth/token</td>
  <td>POST: https://api.odnoklassniki.ru/oauth/token.do</td>
</tr>
<tr>
  <td>
    <ul>
      <li>client_id=APP_ID</li>
      <li>client_secret=CLIENT_SECRET</li>
      <li>code=CODE</li>
      <li>redirect_uri=REDIRECT_URI</li>
    </ul>
  </td>
  <td>
    <ul>
      <li>client_id=APP_ID</li>
      <li>client_secret=CLIENT_SECRET</li>
      <li>code=CODE</li>
      <li>redirect_uri=REDIRECT_URI</li>
      <li>grant_type=authorization_code</li>
    </ul>
  </td>
  <td>
    <ul>
      <li>client_id=APP_ID</li>
      <li>client_secret=CLIENT_SECRET</li>
      <li>code=CODE</li>
      <li>redirect_uri=REDIRECT_URI</li>
      <li>grant_type=authorization_code</li>
    </ul>
  </td>
</tr>
<tr>
  <td>
    <ul>
      <li>access_token=ACCESS_TOKEN</li>
      <li>expires_in=EXPIRES_IN</li>
      <li>user_id=USER_ID</li>
    </ul>
  </td>
  <td>
    <ul>
      <li>access_token=ACCESS_TOKEN</li>
      <li>expires_in=EXPIRES_IN</li>
      <li>x_mailru_vid=USER_ID</li>
      <li>refresh_token=REFRESH_TOKEN</li>
    </ul>
  </td>
  <td>
    <ul>
      <li>access_token=ACCESS_TOKEN</li>
      <li>token_type=session</li>
      <li>refresh_token=REFRESH_TOKEN</li>
    </ul>
  </td>
</tr>
</table>

### Get user data using access token (GET or POST)

<table>
<tr>
  <td>VK</td>
  <td>MailRu</td>
  <td>Odnoklassniki</td>
</tr>
<tr>
  <td>https://api.vk.com/method/users.get</td>
  <td>https://www.appsmail.ru/platform/api</td>
  <td>http://api.odnoklassniki.ru/fb.do</td>
</tr>
<tr>
  <td>
    <ul>
      <li>user_ids=USER_IDS</li>
      <li>fields=FIEDS</li>
    </ul>
  </td>
  <td>
    <ul>
      <li>method=users.getInfo</li>
      <li>app_id=APP_ID</li>
      <li>sig=SIG</li>
      <li>secure=1</li>
      <li>uid=CURRENT_UID</li>
      <li>uids=UIDS_FOR_INFO</li>
    </ul>
  </td>
  <td>
    <ul>
      <li>method=users.getCurrentUser</li>
      <li>application_key=PUBLIC_KEY</li>
      <li>fields=FIELDS</li>
      <li>access_token=ACCESS_TOKEN</li>
      <li>sig=SIG</li>
    </ul>
  </td>
</tr>
<tr>
  <td>http://vk.com/dev/users.get</td>
  <td>http://api.mail.ru/docs/reference/rest/users.getInfo</td>
  <td>http://apiok.ru/wiki/display/api/users.getInfo</td>
</tr>
</table>