using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Pandorax.Recaptcha;
using Xunit;

namespace Pandorax.Recaptcha.Tests;

public class RecaptchaServiceTests
{
    [Fact]
    public async Task ValidateAsync_WithNullIp_OmitsRemoteIpParameter()
    {
        string? capturedPayload = null;
        var handler = new StubHttpMessageHandler(async request =>
        {
            capturedPayload = await request.Content!.ReadAsStringAsync();
            return SuccessResponse();
        });

        var service = CreateService(handler);

        await service.ValidateAsync("token", null);

        Assert.NotNull(capturedPayload);
        Assert.DoesNotContain("remoteip", capturedPayload!, StringComparison.Ordinal);
    }

    [Fact]
    public async Task ValidateAsync_WithIp_IncludesRemoteIpParameter()
    {
        const string ipAddress = "1.2.3.4";
        string? capturedPayload = null;
        var handler = new StubHttpMessageHandler(async request =>
        {
            capturedPayload = await request.Content!.ReadAsStringAsync();
            return SuccessResponse();
        });

        var service = CreateService(handler);

        await service.ValidateAsync("token", ipAddress);

        Assert.NotNull(capturedPayload);
        Assert.Contains($"remoteip={ipAddress}", capturedPayload!, StringComparison.Ordinal);
    }

    [Fact]
    public async Task ValidateAsync_WhenGoogleReturnsNonSuccess_ReturnsFailureWithStatusCode()
    {
        var handler = new StubHttpMessageHandler(_ =>
        {
            var response = new HttpResponseMessage(HttpStatusCode.BadGateway);
            return Task.FromResult(response);
        });

        var service = CreateService(handler);

        ValidationResponse result = await service.ValidateAsync("token", "1.2.3.4");

        Assert.False(result.Success);
        Assert.NotNull(result.ErrorCodes);
        Assert.Contains("http_status:502", result.ErrorCodes);
    }

    [Fact]
    public async Task ValidateAsync_WhenResponseIsMalformed_ReturnsFailureWithErrorCode()
    {
        var handler = new StubHttpMessageHandler(_ =>
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("not-json", Encoding.UTF8, "application/json"),
            };

            return Task.FromResult(response);
        });

        var service = CreateService(handler);

        ValidationResponse result = await service.ValidateAsync("token", "1.2.3.4");

        Assert.False(result.Success);
        Assert.NotNull(result.ErrorCodes);
        Assert.Contains("invalid_response:malformed_json", result.ErrorCodes);
    }

    private static RecaptchaService CreateService(HttpMessageHandler handler)
    {
        var client = new HttpClient(handler, disposeHandler: false);
        var options = Options.Create(new RecaptchaOptions
        {
            SecretKey = "secret",
            SiteKey = "site",
        });

        return new RecaptchaService(client, options);
    }

    private static HttpResponseMessage SuccessResponse()
    {
        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{\"success\":true}", Encoding.UTF8, "application/json"),
        };
    }

    private sealed class StubHttpMessageHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, Task<HttpResponseMessage>> _handler;

        public StubHttpMessageHandler(Func<HttpRequestMessage, Task<HttpResponseMessage>> handler)
        {
            _handler = handler;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => _handler(request);
    }
}
