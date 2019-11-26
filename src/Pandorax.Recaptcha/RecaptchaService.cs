using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Pandorax.Recaptcha
{
    public class RecaptchaService : IRecaptchaService
    {
        private readonly HttpClient _client;
        private readonly string _secretKey;
        private readonly Uri _verifyUrl;

        public RecaptchaService(HttpClient client, IOptions<RecaptchaOptions> options)
        {
            _client = client;
            _secretKey = options.Value.SecretKey;
            _verifyUrl = options.Value.RecaptchaVerifyUrl;
        }

        public async Task<ValidationResponse> ValidateAsync(string gRecaptchaResponse, string ipAddress)
        {
            var parameters = new Dictionary<string, string>
            {
                ["secret"] = _secretKey,
                ["response"] = gRecaptchaResponse,
                ["remoteip"] = ipAddress
            };

            using (var content = new FormUrlEncodedContent(parameters))
            using (var response = await _client.PostAsync(_verifyUrl, content))
            {
                if (response.IsSuccessStatusCode)
                {
                    var read = await response.Content.ReadAsStringAsync();
                    var model = JsonConvert.DeserializeObject<ValidationResponse>(read);
                    return model;
                }

                return new ValidationResponse { Success = false };
            }
        }
    }
}
