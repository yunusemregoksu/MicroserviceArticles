using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Options;
using ReviewAPI.Services;
using ReviewAPI.Settings;

namespace MicroserviceArticles.Tests;

public class ArticlesServiceClientTests
{
    [Fact]
    public async Task ArticleExistsAsync_WhenApiReturnsSuccess_ReturnsTrue()
    {
        var handler = new TestHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK));
        var httpClient = new HttpClient(handler);
        var settings = Options.Create(new ArticleApiSettings { BaseUrl = "http://articles.local/" });
        var sut = new ArticlesServiceClient(httpClient, settings);

        var result = await sut.ArticleExistsAsync("507f1f77bcf86cd799439011");

        Assert.True(result);
        Assert.Equal("http://articles.local/api/Articles/507f1f77bcf86cd799439011", handler.LastRequest?.RequestUri?.ToString());
    }

    [Fact]
    public async Task ArticleExistsAsync_WhenApiReturnsNotFound_ReturnsFalse()
    {
        var handler = new TestHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.NotFound));
        var httpClient = new HttpClient(handler);
        var settings = Options.Create(new ArticleApiSettings { BaseUrl = "http://articles.local/" });
        var sut = new ArticlesServiceClient(httpClient, settings);

        var result = await sut.ArticleExistsAsync("507f1f77bcf86cd799439012");

        Assert.False(result);
    }

    [Fact]
    public async Task ArticleExistsAsync_WhenApiReturnsServerError_ThrowsHttpRequestException()
    {
        var handler = new TestHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.InternalServerError));
        var httpClient = new HttpClient(handler);
        var settings = Options.Create(new ArticleApiSettings { BaseUrl = "http://articles.local/" });
        var sut = new ArticlesServiceClient(httpClient, settings);

        await Assert.ThrowsAsync<HttpRequestException>(() => sut.ArticleExistsAsync("507f1f77bcf86cd799439013"));
    }
}
