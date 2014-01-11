oauth2
======

OAuth 2.0

<table>
<tr>
<td>VK</td>
<td>MailRu</td>
<td>Odnoklassniki</td>
</tr>
</table>

Get auth code

<table>
<tr>
<td>https://oauth.vk.com/authorize</td>
<td>https://connect.mail.ru/oauth/authorize</td>
<td>http://www.odnoklassniki.ru/oauth/authorize</td>
</tr>
<tr>
<td>
client_id=APP_ID
redirect_uri=REDIRECT_URI
scope=PERMISSIONS
response_type=code
</td>
<td>
client_id=APP_ID
redirect_uri=REDIRECT_URI
scope=PERMISSIONS
response_type=code
</td>
<td>
- client_id=APP_ID
- redirect_uri=REDIRECT_URI
- scope=PERMISSIONS
- response_type=code
- v=API_VERSION 
</td>
</table>
