using System;

namespace Pandorax.Recaptcha
{
    /// <summary>
    /// An options class used to configure the reCAPTCHA.
    /// </summary>
    public class RecaptchaOptions
    {
        /// <summary>
        /// Gets or sets the site key for this reCAPTCHA.
        /// </summary>
        public string SiteKey { get; set; }

        /// <summary>
        /// Gets or sets the shared key between your site and reCAPTCHA.
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// Gets the reCAPTCHA verify url.
        /// </summary>
        internal Uri RecaptchaVerifyUrl { get; } = new Uri(RecaptchaConstants.DefaultRecaptchaVerifyUrl);
    }
}
