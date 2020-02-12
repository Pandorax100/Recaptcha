using System;

namespace Pandorax.Recaptcha
{
    public class RecaptchaOptions
    {
        public string SiteKey { get; set; }

        public string SecretKey { get; set; }

        public Uri RecaptchaVerifyUrl { get; set; } = new Uri(RecaptchaConstants.DefaultRecaptchaVerifyUrl);
    }
}
