using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Google.Api.Gax.ResourceNames;
using Google.Cloud.RecaptchaEnterprise.V1;
using Microsoft.Extensions.Options;

namespace Pandorax.RecaptchaEnterprise
{
    /// <inheritdoc cref="IRecaptchaEnterpriseService" />
    public class RecaptchaEnterpriseService : IRecaptchaEnterpriseService
    {
        private readonly RecaptchaEnterpriseServiceClient _client;
        private readonly RecaptchaEnterpriseOptions _options;
        private readonly ProjectName _projectName;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecaptchaEnterpriseService"/> class.
        /// </summary>
        /// <param name="client">The <see cref="RecaptchaEnterpriseServiceClient"/> used to communicate with Google.</param>
        /// <param name="optionsAccessor">The configured <see cref="RecaptchaEnterpriseOptions"/>.</param>
        public RecaptchaEnterpriseService(RecaptchaEnterpriseServiceClient client, IOptions<RecaptchaEnterpriseOptions> optionsAccessor)
        {
            ArgumentNullException.ThrowIfNull(client);
            ArgumentNullException.ThrowIfNull(optionsAccessor);

            _client = client;
            _options = optionsAccessor.Value;
            _projectName = ProjectName.FromProject(_options.ProjectId);
        }

        /// <inheritdoc />
        public async Task<RecaptchaEnterpriseAssessmentResult> AssessAsync(string token, string? expectedAction = null, string? siteKey = null, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("Token must be provided.", nameof(token));
            }

            string finalSiteKey = string.IsNullOrWhiteSpace(siteKey) ? _options.SiteKey : siteKey!;
            if (string.IsNullOrWhiteSpace(finalSiteKey))
            {
                throw new InvalidOperationException("A site key must be configured before invoking reCAPTCHA Enterprise assessments.");
            }

            string? actionToEvaluate = !string.IsNullOrWhiteSpace(expectedAction) ? expectedAction : _options.DefaultExpectedAction;

            var assessment = new Assessment
            {
                Event = new Event
                {
                    Token = token,
                    SiteKey = finalSiteKey,
                },
            };

            if (!string.IsNullOrWhiteSpace(actionToEvaluate))
            {
                assessment.Event.ExpectedAction = actionToEvaluate;
            }

            var request = new CreateAssessmentRequest
            {
                ParentAsProjectName = _projectName,
                Assessment = assessment,
            };

            Assessment response = await _client.CreateAssessmentAsync(request, cancellationToken: cancellationToken).ConfigureAwait(false);
            TokenProperties tokenProperties = response.TokenProperties ?? new TokenProperties();

            var reasons = response.RiskAnalysis?.Reasons != null
                ? new List<RiskAnalysis.Types.ClassificationReason>(response.RiskAnalysis.Reasons)
                : new List<RiskAnalysis.Types.ClassificationReason>();

            float score = response.RiskAnalysis?.Score ?? 0f;
            bool actionMatched = string.IsNullOrWhiteSpace(actionToEvaluate) || string.Equals(tokenProperties.Action, actionToEvaluate, StringComparison.Ordinal);
            bool isValid = tokenProperties.Valid && actionMatched;

            return new RecaptchaEnterpriseAssessmentResult(isValid, actionMatched, score, tokenProperties, reasons);
        }
    }
}
