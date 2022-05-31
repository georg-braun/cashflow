# Test with real Authentication

API routes are protected. Therefore it's necessary to get an access token.

https://auth0.com/docs/get-started/authentication-and-authorization-flow/call-your-api-using-resource-owner-password-flow

The Auth0 sdk is used to get a valid access token. To connect the Auth0 service it's necessary to add the correct
settings:

- change values in `appsettings.json` (don't commit secrets!)
- `dotnet user-secrets set "x:y" "value"`
- environment variable `x__y = value`