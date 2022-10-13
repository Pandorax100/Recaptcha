using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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
        [JsonPropertyName("success")]
        public bool Success { get; internal set; }

        /// <summary>
        /// Gets the score for this request (0.0 - 1.0).
        /// </summary>
        [JsonPropertyName("score")]
        public double Score { get; private set; }

        /// <summary>
        /// Gets the action name for this request (important to verify).
        /// </summary>
        [JsonPropertyName("action")]
        public string Action { get; private set; }

        /// <summary>
        /// Gets timestamp of the challenge load (ISO format yyyy-MM-dd'T'HH:mm:ssZZ).
        /// </summary>
        [JsonPropertyName("challenge_ts")]
        public DateTimeOffset ChallengeTimeStamp { get; private set; }

        /// <summary>
        /// Gets the hostname of the site where the reCAPTCHA was solved.
        /// </summary>
        [JsonPropertyName("hostname")]
        public string Hostname { get; private set; }

        /// <summary>
        /// Gets the list of error codes returned from the validation request.
        /// </summary>
        [JsonPropertyName("error-codes")]
        public List<string> ErrorCodes { get; private set; }
    }
}
