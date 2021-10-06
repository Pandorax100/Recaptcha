using System;
using System.Collections.Generic;
#if NET5_0
using System.Text.Json.Serialization;
#elif NET461
using Newtonsoft.Json;
#endif

namespace Pandorax.Recaptcha
{
    /// <summary>
    /// The reCAPTCHA validation response.
    /// </summary>
    public class ValidationResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResponse"/> class.
        /// </summary>
        public ValidationResponse()
        {
        }

        internal ValidationResponse(bool success) => Success = success;

        /// <summary>
        /// Gets a value indicating whether this request was a valid reCAPTCHA token.
        /// </summary>
#if NET5_0
        [JsonPropertyName("success")]
        [JsonInclude]
#elif NET461
        [JsonProperty("success")]
#endif
        public bool Success { get; internal set; }

        /// <summary>
        /// Gets the score for this request (0.0 - 1.0).
        /// </summary>
#if NET5_0
        [JsonPropertyName("score")]
        [JsonInclude]
#elif NET461
        [JsonProperty("score")]
#endif
        public double Score { get; private set; }

        /// <summary>
        /// Gets the action name for this request (important to verify).
        /// </summary>
#if NET5_0
        [JsonPropertyName("action")]
        [JsonInclude]
#elif NET461
        [JsonProperty("action")]
#endif
        public string Action { get; private set; }

        /// <summary>
        /// Gets timestamp of the challenge load (ISO format yyyy-MM-dd'T'HH:mm:ssZZ).
        /// </summary>
#if NET5_0
        [JsonPropertyName("challenge_ts")]
        [JsonInclude]
#elif NET461
        [JsonProperty("challenge_ts")]
#endif
        public DateTimeOffset ChallengeTimeStamp { get; private set; }

        /// <summary>
        /// Gets the hostname of the site where the reCAPTCHA was solved.
        /// </summary>
#if NET5_0
        [JsonPropertyName("hostname")]
        [JsonInclude]
#elif NET461
        [JsonProperty("hostname")]
#endif
        public string Hostname { get; private set; }

        /// <summary>
        /// Gets the list of error codes returned from the validation request.
        /// </summary>
#if NET5_0
        [JsonPropertyName("error-codes")]
        [JsonInclude]
#elif NET461
        [JsonProperty("error-codes")]
#endif
        public List<string> ErrorCodes { get; private set; }
    }
}
