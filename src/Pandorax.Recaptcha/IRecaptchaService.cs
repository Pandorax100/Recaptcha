using System.Threading.Tasks;

namespace Pandorax.Recaptcha
{
    public interface IRecaptchaService
    {
        Task<ValidationResponse> ValidateAsync(string gRecaptchaResponse, string ipAddress = null);
    }
}
