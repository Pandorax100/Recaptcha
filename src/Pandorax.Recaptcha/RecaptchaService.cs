using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
#if NET461
using Newtonsoft.Json;
#endif

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
                ["remoteip"] = ipAddress,
            };

            using var content = new FormUrlEncodedContent(parameters);
            using var response = await _client.PostAsync(_verifyUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
#if NET5_0
                var model = System.Text.Json.JsonSerializer.Deserialize<ValidationResponse>(responseString);
#elif NET461
                var model = JsonConvert.DeserializeObject<ValidationResponse>(responseString);
#endif
                return model;
            }

            return new ValidationResponse(success: false);
        }
    }
}
