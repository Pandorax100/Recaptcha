using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
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
            ArgumentNullException.ThrowIfNull(options);
            ArgumentNullException.ThrowIfNull(client);

            _client = client;
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

            if (!response.IsSuccessStatusCode)
            {
                return new ValidationResponse
                {
                    ErrorCodes = new List<string> { $"http_status:{(int)response.StatusCode}" },
                };
            }

            try
            {
                ValidationResponse model = await response.Content.ReadFromJsonAsync<ValidationResponse>();
                return model ?? new ValidationResponse(success: false);
            }
            catch (NotSupportedException)
            {
                return new ValidationResponse
                {
                    ErrorCodes = new List<string> { "invalid_response:unsupported_content_type" },
                };
            }
            catch (JsonException)
            {
                return new ValidationResponse
                {
                    ErrorCodes = new List<string> { "invalid_response:malformed_json" },
                };
            }
        }
    }
}
