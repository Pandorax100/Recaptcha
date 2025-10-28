using System.Threading;
using System.Threading.Tasks;

namespace Pandorax.RecaptchaEnterprise
{
    /// <summary>
    /// Sends assessment requests to Google reCAPTCHA Enterprise.
    /// </summary>
    public interface IRecaptchaEnterpriseService
    {
        /// <summary>
        /// Performs an assessment for the supplied token.
        /// </summary>
        /// <param name="token">The token obtained from the client-side integration.</param>
        /// <param name="expectedAction">The expected action that should be associated with the token.</param>
        /// <param name="siteKey">An optional site key to use instead of the configured default.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>The assessment result that includes risk score and token metadata.</returns>
        Task<RecaptchaEnterpriseAssessmentResult> AssessAsync(string token, string? expectedAction = null, string? siteKey = null, CancellationToken cancellationToken = default);
    }
}
