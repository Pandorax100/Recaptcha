using System;
using Google.Cloud.RecaptchaEnterprise.V1;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Pandorax.RecaptchaEnterprise
{
    /// <summary>
    /// Extension methods for registering reCAPTCHA Enterprise integrations.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the reCAPTCHA Enterprise services using the supplied configuration section.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <param name="configurationSection">The configuration section containing the options.</param>
        /// <returns>The original <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddRecaptchaEnterprise(this IServiceCollection services, IConfiguration configurationSection)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(configurationSection);

            var builder = services.AddOptions<RecaptchaEnterpriseOptions>();
            builder.Bind(configurationSection);
            ConfigureValidation(builder);

            return AddCoreServices(services);
        }

        /// <summary>
        /// Adds the reCAPTCHA Enterprise services using the supplied configure action.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <param name="configureAction">The action used to configure <see cref="RecaptchaEnterpriseOptions"/>.</param>
        /// <returns>The original <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddRecaptchaEnterprise(this IServiceCollection services, Action<RecaptchaEnterpriseOptions> configureAction)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(configureAction);

            var builder = services.AddOptions<RecaptchaEnterpriseOptions>();
            builder.Configure(configureAction);
            ConfigureValidation(builder);

            return AddCoreServices(services);
        }

        private static IServiceCollection AddCoreServices(IServiceCollection services)
        {
            services.TryAddSingleton(provider =>
            {
                var options = provider.GetRequiredService<IOptions<RecaptchaEnterpriseOptions>>().Value;

                if (options.ClientFactory is not null)
                {
                    return options.ClientFactory(provider);
                }

                var builder = new RecaptchaEnterpriseServiceClientBuilder();
                options.ConfigureClientBuilder?.Invoke(builder);
                return builder.Build();
            });

            services.TryAddSingleton<IRecaptchaEnterpriseService, RecaptchaEnterpriseService>();

            return services;
        }

        private static void ConfigureValidation(OptionsBuilder<RecaptchaEnterpriseOptions> builder)
        {
            builder.Validate(options => !string.IsNullOrWhiteSpace(options.ProjectId), "Recaptcha Enterprise ProjectId must be provided.");
            builder.Validate(options => !string.IsNullOrWhiteSpace(options.SiteKey), "Recaptcha Enterprise SiteKey must be provided.");
        }
    }
}
