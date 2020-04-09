using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SpeechBasedGroceries.DTOs
{
    public class TokenCredentials : ServiceClientCredentials
    {

        /// <summary>
        /// The bearer token type, as serialized in an http Authentication header.
        /// </summary>
        private const string BearerTokenType = "Bearer";

        /// <summary>
        /// Gets or sets secure token used to authenticate against Microsoft Azure API. 
        /// No anonymous requests are allowed.
        /// </summary>
        protected ITokenProvider TokenProvider { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenCredentials"/>
        /// class with the given 'Bearer' token.
        /// </summary>
        /// <param name="token">Valid JSON Web Token (JWT).</param>
        public TokenCredentials(string token)
            : this(token, BearerTokenType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenCredentials"/>
        /// class with the given token and token type.
        /// </summary>
        /// <param name="token">Valid JSON Web Token (JWT).</param>
        /// <param name="tokenType">The token type of the given token.</param>
        public TokenCredentials(string token, string tokenType)
            : this(new StringTokenProvider(token, tokenType))
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException("token");
            }
            if (string.IsNullOrEmpty(tokenType))
            {
                throw new ArgumentNullException("tokenType");
            }
        }

        /// <summary>
        /// Create an access token credentials object, given an interface to a token source.
        /// </summary>
        /// <param name="tokenProvider">The source of tokens for these credentials.</param>
        public TokenCredentials(ITokenProvider tokenProvider)
        {
            if (tokenProvider == null)
            {
                throw new ArgumentNullException("tokenProvider");
            }

            this.TokenProvider = tokenProvider;
        }

        /// <summary>
        /// Apply the credentials to the HTTP request.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>
        /// Task that will complete when processing has completed.
        /// </returns>
        public async override Task ProcessHttpRequestAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (TokenProvider == null)
            {
                throw new ArgumentNullException("Token provider");
            }

            request.Headers.Authorization = await TokenProvider.GetAuthenticationHeaderAsync(cancellationToken);
            await base.ProcessHttpRequestAsync(request, cancellationToken);
        }
    }
}