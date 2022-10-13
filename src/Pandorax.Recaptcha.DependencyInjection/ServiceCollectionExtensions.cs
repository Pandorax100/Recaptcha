using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Pandorax.Recaptcha
{
    /// <summary>
    /// Extension methods for adding the reCAPTCHA services to the DI container.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the <see cref="IRecaptchaService"/> and related options to the <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configurationSection">The configuration being bound.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddRecaptcha(this IServiceCollection services, IConfiguration configurationSection)
        {
            services.Configure<RecaptchaOptions>(configurationSection);
            return AddCoreServices(services);
        }

        /// <summary>
        /// Adds the <see cref="IRecaptchaService"/> and related options to the <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configureAction">The action used to configure the options.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddRecaptcha(this IServiceCollection services, Action<RecaptchaOptions> configureAction)
        {
            services.Configure(configureAction);
            return AddCoreServices(services);
        }

        private static IServiceCollection AddCoreServices(IServiceCollection services)
        {
            services.AddHttpClient<IRecaptchaService, RecaptchaService>();

            return services;
        }
    }
}
