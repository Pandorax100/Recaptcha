using System;
using System.ComponentModel.DataAnnotations;
using Google.Cloud.RecaptchaEnterprise.V1;

namespace Pandorax.RecaptchaEnterprise
{
    /// <summary>
    /// Options used to configure access to Google reCAPTCHA Enterprise.
    /// </summary>
    public class RecaptchaEnterpriseOptions
    {
        /// <summary>
        /// Gets or sets the Google Cloud project identifier that owns the reCAPTCHA Enterprise keys.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string ProjectId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the default site key to send with assessment requests.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string SiteKey { get; set; } = null!;

        /// <summary>
        /// Gets or sets the default expected action that tokens should be associated with.
        /// </summary>
        public string? DefaultExpectedAction { get; set; }

        /// <summary>
        /// Gets or sets an optional callback to customize the <see cref="RecaptchaEnterpriseServiceClientBuilder"/> used to construct the underlying client.
        /// </summary>
        public Action<RecaptchaEnterpriseServiceClientBuilder>? ConfigureClientBuilder { get; set; }

        /// <summary>
        /// Gets or sets an optional factory used to create the <see cref="RecaptchaEnterpriseServiceClient"/>.
        /// When supplied, <see cref="ConfigureClientBuilder"/> is ignored.
        /// </summary>
        public Func<IServiceProvider, RecaptchaEnterpriseServiceClient>? ClientFactory { get; set; }
    }
}
