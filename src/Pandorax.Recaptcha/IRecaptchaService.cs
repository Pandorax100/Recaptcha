using System.Threading.Tasks;

namespace Pandorax.Recaptcha
{
    /// <summary>
    /// Used to send and receive reCAPTCHA validation responses.
    /// </summary>
    public interface IRecaptchaService
    {
        /// <summary>
        /// Validates the given reCAPTCHA form value against the server.
        /// </summary>
        /// <param name="gRecaptchaResponse">The user response token provided by the reCAPTCHA.</param>
        /// <param name="ipAddress">The user's IP address.</param>
        /// <returns>The reCAPTCHA validation response.</returns>
        Task<ValidationResponse> ValidateAsync(string gRecaptchaResponse, string ipAddress = null);
    }
}
