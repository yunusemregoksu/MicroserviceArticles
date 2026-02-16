using System.Net;
using System.Net.Http;

namespace MicroserviceArticles.Tests;

internal sealed class TestHttpMessageHandler : HttpMessageHandler
{
    private readonly Func<HttpRequestMessage, HttpResponseMessage> _responseFactory;

    public HttpRequestMessage? LastRequest { get; private set; }

    public TestHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> responseFactory)
    {
        _responseFactory = responseFactory;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        LastRequest = request;
        var response = _responseFactory(request);
        response.RequestMessage = request;
        return Task.FromResult(response);
    }
}
