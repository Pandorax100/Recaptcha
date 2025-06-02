# Pandorax.Recaptcha

This project is a library for validating Google reCAPTCHA responses in .NET applications.

## Installation

To install the library, add the following NuGet package to your project:

```
dotnet add package Pandorax.Recaptcha
```

## Usage

### Configuration

First, configure the reCAPTCHA options in your `appsettings.json`:

```json
{
  "Recaptcha": {
    "SiteKey": "your-site-key",
    "SecretKey": "your-secret-key"
  }
}
```

Then, register the reCAPTCHA services in your `Startup.cs` or `Program.cs`:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddRecaptcha(Configuration.GetSection("Recaptcha"));
}
```

Or, if you are using `Program.cs` with top-level statements:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRecaptcha(builder.Configuration.GetSection("Recaptcha"));
```

### Validation

To validate a reCAPTCHA response, inject the `IRecaptchaService` into your controller or service and call the `ValidateAsync` method:

```csharp
public class MyController : Controller
{
    private readonly IRecaptchaService _recaptchaService;

    public MyController(IRecaptchaService recaptchaService)
    {
        _recaptchaService = recaptchaService;
    }

    [HttpPost]
    public async Task<IActionResult> SubmitForm(string recaptchaResponse)
    {
        var validationResponse = await _recaptchaService.ValidateAsync(recaptchaResponse, HttpContext.Connection.RemoteIpAddress.ToString());

        if (validationResponse.Success)
        {
            // reCAPTCHA validation succeeded
            if (validationResponse.Score >= 0.5)
            {
                // High confidence in the user's interaction
            }
            else
            {
                // Low confidence in the user's interaction
            }
        }
        else
        {
            // reCAPTCHA validation failed
        }

        return View();
    }
}
```

## License

This project is licensed under the MIT License.