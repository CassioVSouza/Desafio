using Microsoft.JSInterop;
using System.Net.Http.Headers;

namespace Frontend.Servicos.Principal
{
    public class AuthHeaderServico : DelegatingHandler
    {
        private readonly IJSRuntime _jsRuntime;

        public AuthHeaderServico(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
