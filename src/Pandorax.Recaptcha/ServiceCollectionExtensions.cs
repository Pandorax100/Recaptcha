using System;
using Microsoft.Extensions.DependencyInjection;

namespace Pandorax.Recaptcha
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRecaptcha(this IServiceCollection services, Action<RecaptchaOptions> configureAction)
        {
            services.Configure(configureAction);
            services.AddHttpClient<IRecaptchaService, RecaptchaService>();

            return services;
        }
    }
}
