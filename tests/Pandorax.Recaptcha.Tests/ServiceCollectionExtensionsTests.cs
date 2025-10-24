using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Pandorax.Recaptcha;
using Xunit;

namespace Pandorax.Recaptcha.Tests;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddRecaptcha_WithNullServices_Throws()
    {
        var configuration = new ConfigurationBuilder().Build().GetSection("Recaptcha");

        Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddRecaptcha(null!, configuration));
    }

    [Fact]
    public void AddRecaptcha_WithNullConfigurationSection_Throws()
    {
        var services = new ServiceCollection();

        Assert.Throws<ArgumentNullException>(() => services.AddRecaptcha(configurationSection: null!));
    }

    [Fact]
    public void AddRecaptcha_WithNullConfigureDelegate_Throws()
    {
        var services = new ServiceCollection();

        Assert.Throws<ArgumentNullException>(() => services.AddRecaptcha(configureAction: null!));
    }

    [Fact]
    public void AddRecaptcha_WithEmptySecret_ThrowsOptionsValidationException()
    {
        var services = new ServiceCollection();

        services.AddRecaptcha(options =>
        {
            options.SiteKey = "site";
            options.SecretKey = string.Empty;
        });

        using var provider = services.BuildServiceProvider();

        var exception = Assert.Throws<OptionsValidationException>(() => provider.GetRequiredService<IOptions<RecaptchaOptions>>().Value);
        Assert.Contains("Recaptcha SecretKey must be provided.", exception.Failures);
    }

    [Fact]
    public void AddRecaptcha_WithEmptySiteKey_ThrowsOptionsValidationException()
    {
        var services = new ServiceCollection();

        services.AddRecaptcha(options =>
        {
            options.SiteKey = string.Empty;
            options.SecretKey = "secret";
        });

        using var provider = services.BuildServiceProvider();

        var exception = Assert.Throws<OptionsValidationException>(() => provider.GetRequiredService<IOptions<RecaptchaOptions>>().Value);
        Assert.Contains("Recaptcha SiteKey must be provided.", exception.Failures);
    }

    [Fact]
    public void AddRecaptcha_WithValidOptions_ResolvesRecaptchaService()
    {
        var configurationValues = new Dictionary<string, string?>
        {
            ["Recaptcha:SiteKey"] = "site",
            ["Recaptcha:SecretKey"] = "secret",
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configurationValues)
            .Build()
            .GetSection("Recaptcha");

        var services = new ServiceCollection();
        services.AddRecaptcha(configuration);

        using var provider = services.BuildServiceProvider();

        var service = provider.GetRequiredService<IRecaptchaService>();

        Assert.NotNull(service);
    }
}
