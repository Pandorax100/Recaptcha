using System;

namespace Pandorax.Recaptcha
{
    public class RecaptchaOptions
    {
        public string SecretKey { get; set; }

        public Uri RecaptchaVerifyUrl { get; set; } = new Uri(RecaptchaConstants.DefaultRecaptchaVerifyUrl);
    }
}
