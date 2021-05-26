using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pandorax.Recaptcha
{
    /// <summary>
    /// The reCAPTCHA validation response.
    /// </summary>
    public record ValidationResponse
    {
        /// <summary>
        /// Gets a value indicating whether this request was a valid reCAPTCHA token.
        /// </summary>
        [JsonPropertyName("success")]
        public bool Success { get; init; }

        /// <summary>
        /// Gets the score for this request (0.0 - 1.0).
        /// </summary>
        [JsonPropertyName("score")]
        public double Score { get; init; }

        /// <summary>
        /// Gets the action name for this request (important to verify).
        /// </summary>
        [JsonPropertyName("action")]
        public string Action { get; init; }

        /// <summary>
        /// Gets timestamp of the challenge load (ISO format yyyy-MM-dd'T'HH:mm:ssZZ).
        /// </summary>
        [JsonPropertyName("challenge_ts")]
        public DateTimeOffset ChallengeTimeStamp { get; init; }

        /// <summary>
        /// Gets the hostname of the site where the reCAPTCHA was solved.
        /// </summary>
        [JsonPropertyName("hostname")]
        public string Hostname { get; init; }

        /// <summary>
        /// Gets the list of error codes returned from the validation request.
        /// </summary>
        [JsonPropertyName("error-codes")]
        public List<string> ErrorCodes { get; private init; }
    }
}
