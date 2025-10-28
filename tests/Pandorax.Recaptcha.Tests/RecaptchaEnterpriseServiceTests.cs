using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Api.Gax.Grpc;
using Google.Cloud.RecaptchaEnterprise.V1;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Pandorax.RecaptchaEnterprise;
using Xunit;
using EnterpriseService = Pandorax.RecaptchaEnterprise.RecaptchaEnterpriseService;

namespace Pandorax.Recaptcha.Tests;

public class RecaptchaEnterpriseServiceTests
{
    [Fact]
    public async Task AssessAsync_WithValidToken_ReturnsResultFromClient()
    {
        var tokenProperties = new TokenProperties
        {
            Valid = true,
            Action = "login",
        };

        var assessment = new Assessment
        {
            TokenProperties = tokenProperties,
            RiskAnalysis = new RiskAnalysis
            {
                Score = 0.9f,
            },
        };

        var client = new FakeRecaptchaEnterpriseServiceClient(assessment);
        var options = Options.Create(new RecaptchaEnterpriseOptions
        {
            ProjectId = "demo-project",
            SiteKey = "site-key",
            ClientFactory = _ => client,
        });

        var service = new EnterpriseService(client, options);

        RecaptchaEnterpriseAssessmentResult result = await service.AssessAsync("token", expectedAction: "login");

        Assert.True(result.IsValid);
        Assert.True(result.ActionMatched);
        Assert.Equal(0.9f, result.Score);
        Assert.Same(tokenProperties, result.TokenProperties);
    }

    [Fact]
    public void AddRecaptchaEnterprise_RegistersService()
    {
        var services = new ServiceCollection();

        services.AddRecaptchaEnterprise(options =>
        {
            options.ProjectId = "demo-project";
            options.SiteKey = "site-key";
            options.ClientFactory = _ => new FakeRecaptchaEnterpriseServiceClient(new Assessment());
        });

        using ServiceProvider provider = services.BuildServiceProvider();

        var service = provider.GetService<IRecaptchaEnterpriseService>();

        Assert.NotNull(service);
    }

    private sealed class FakeRecaptchaEnterpriseServiceClient : RecaptchaEnterpriseServiceClient
    {
        private readonly Assessment _response;

        public FakeRecaptchaEnterpriseServiceClient(Assessment response) => _response = response;

        public override Task<Assessment> CreateAssessmentAsync(CreateAssessmentRequest request, CallSettings? callSettings = null)
        {
            ValidateRequest(request);
            return Task.FromResult(_response);
        }

        public override Task<Assessment> CreateAssessmentAsync(CreateAssessmentRequest request, System.Threading.CancellationToken cancellationToken)
        {
            ValidateRequest(request);
            return Task.FromResult(_response);
        }

        private static void ValidateRequest(CreateAssessmentRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.ParentAsProjectName == null)
            {
                throw new ArgumentException("Parent must be set.", nameof(request));
            }
        }
    }
}
