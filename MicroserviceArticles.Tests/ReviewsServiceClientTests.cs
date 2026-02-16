using System.Net;
using System.Net.Http;
using ArticleAPI.Services;
using ArticleAPI.Settings;
using Microsoft.Extensions.Options;

namespace MicroserviceArticles.Tests;

public class ReviewsServiceClientTests
{
    [Fact]
    public async Task HasReviewsForArticleAsync_WhenResponseIsTrue_ReturnsTrue()
    {
        var handler = new TestHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("true")
        });
        var httpClient = new HttpClient(handler);
        var settings = Options.Create(new ReviewApiSettings { BaseUrl = "http://reviews.local/" });
        var sut = new ReviewsServiceClient(httpClient, settings);

        var result = await sut.HasReviewsForArticleAsync("507f1f77bcf86cd799439021");

        Assert.True(result);
        Assert.Equal("http://reviews.local/api/Reviews/article/507f1f77bcf86cd799439021/exists", handler.LastRequest?.RequestUri?.ToString());
    }

    [Fact]
    public async Task HasReviewsForArticleAsync_WhenResponseIsFalse_ReturnsFalse()
    {
        var handler = new TestHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("false")
        });
        var httpClient = new HttpClient(handler);
        var settings = Options.Create(new ReviewApiSettings { BaseUrl = "http://reviews.local/" });
        var sut = new ReviewsServiceClient(httpClient, settings);

        var result = await sut.HasReviewsForArticleAsync("507f1f77bcf86cd799439022");

        Assert.False(result);
    }

    [Fact]
    public async Task HasReviewsForArticleAsync_WhenResponseBodyIsInvalid_ReturnsFalse()
    {
        var handler = new TestHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("not-a-boolean")
        });
        var httpClient = new HttpClient(handler);
        var settings = Options.Create(new ReviewApiSettings { BaseUrl = "http://reviews.local/" });
        var sut = new ReviewsServiceClient(httpClient, settings);

        var result = await sut.HasReviewsForArticleAsync("507f1f77bcf86cd799439023");

        Assert.False(result);
    }

    [Fact]
    public async Task HasReviewsForArticleAsync_WhenApiReturnsFailure_ThrowsHttpRequestException()
    {
        var handler = new TestHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.BadGateway));
        var httpClient = new HttpClient(handler);
        var settings = Options.Create(new ReviewApiSettings { BaseUrl = "http://reviews.local/" });
        var sut = new ReviewsServiceClient(httpClient, settings);

        await Assert.ThrowsAsync<HttpRequestException>(() => sut.HasReviewsForArticleAsync("507f1f77bcf86cd799439024"));
    }
}
