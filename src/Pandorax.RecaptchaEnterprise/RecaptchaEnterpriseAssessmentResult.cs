using System.Collections.Generic;
using Google.Cloud.RecaptchaEnterprise.V1;

namespace Pandorax.RecaptchaEnterprise
{
    /// <summary>
    /// Represents the outcome of a reCAPTCHA Enterprise assessment.
    /// </summary>
    public record RecaptchaEnterpriseAssessmentResult(
        bool IsValid,
        bool ActionMatched,
        float Score,
        TokenProperties TokenProperties,
        IReadOnlyList<RiskAnalysis.Types.ClassificationReason> Reasons);
}
