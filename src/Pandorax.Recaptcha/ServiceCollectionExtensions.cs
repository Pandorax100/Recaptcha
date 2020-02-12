using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Pandorax.Recaptcha
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRecaptcha(this IServiceCollection services, IConfiguration configurationSection)
        {
            return AddRecaptcha(services, options => configurationSection.Bind(options));
        }

        public static IServiceCollection AddRecaptcha(this IServiceCollection services, Action<RecaptchaOptions> configureAction)
        {
            services.Configure(configureAction);
            services.AddHttpClient<IRecaptchaService, RecaptchaService>();

            return services;
        }
    }
}
