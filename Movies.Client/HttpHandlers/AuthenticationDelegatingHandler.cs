using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Movies.Client.HttpHandlers
{
    public class AuthenticationDelegatingHandler : DelegatingHandler
    {
        //private readonly IHttpClientFactory _httpClientFactory;
        //private readonly ClientCredentialsTokenRequest _tokenRequest;

        //// Constructor: Initializes the required dependencies for the handler.
        //public AuthenticationDelegatingHandler(IHttpClientFactory httpClientFactory, ClientCredentialsTokenRequest tokenRequest)
        //{
        //    _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        //    _tokenRequest = tokenRequest ?? throw new ArgumentNullException(nameof(tokenRequest));

        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationDelegatingHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
         

        // Overrides the base `SendAsync` method to add an access token to the request before forwarding it.
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //if (request == null)
            //{
            //    throw new ArgumentNullException(nameof(request)); // Ensures the request is not null.
            //}

            //// Create a client for communication with the Identity Provider (IDP).
            //var httpClient = _httpClientFactory.CreateClient("IDPClient");

            //// Request an access token using client credentials flow.
            //var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(_tokenRequest);

            //// Check if there was an error in obtaining the token.
            //if (tokenResponse.IsError)
            //{
            //    throw new HttpRequestException("Something went wrong while requesting the access token");
            //}

            //// Set the bearer token for authorization in the outgoing request.
            //request.SetBearerToken(tokenResponse.AccessToken ?? throw new InvalidOperationException("Access token is null"));

            // Forward the request to the next handler in the pipeline.

            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            if(!string.IsNullOrWhiteSpace(accessToken))
            {
                request.SetBearerToken(accessToken);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
