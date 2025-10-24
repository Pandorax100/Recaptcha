using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Pandorax.Recaptcha
{
    /// <inheritdoc cref="IRecaptchaService" />
    public class RecaptchaService : IRecaptchaService
    {
        private readonly HttpClient _client;
        private readonly string _secretKey;
        private readonly Uri _verifyUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecaptchaService" /> class.
        /// </summary>
        /// <param name="client">The <see cref="HttpClient"/> associated.</param>
        /// <param name="options">The <see cref="IOptions{RecaptchaOptions}"/> used to configure the reCAPTCHA.</param>
        public RecaptchaService(HttpClient client, IOptions<RecaptchaOptions> options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _client = client ?? throw new ArgumentNullException(nameof(client));
            _secretKey = options.Value.SecretKey;
            _verifyUrl = options.Value.RecaptchaVerifyUrl;
        }

        /// <inheritdoc />
        public async Task<ValidationResponse> ValidateAsync(string recaptchaResponse, string ipAddress)
        {
            var parameters = new Dictionary<string, string>
            {
                ["secret"] = _secretKey,
                ["response"] = recaptchaResponse,
            };

            if (!string.IsNullOrWhiteSpace(ipAddress))
            {
                parameters["remoteip"] = ipAddress;
            }

            using var content = new FormUrlEncodedContent(parameters);
            using var response = await _client.PostAsync(_verifyUrl, content);

            if (response.IsSuccessStatusCode)
            {
                ValidationResponse model = await response.Content.ReadFromJsonAsync<ValidationResponse>();

                return model;
            }

            return new ValidationResponse(success: false);
        }
    }
}
