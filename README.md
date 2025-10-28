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

When validating a reCAPTCHA response, you should check the following properties on the validation response:

- `Success`: Indicates whether the validation was successful.
- `Score`: Represents the confidence score (for reCAPTCHA v3). You may want to set a threshold (e.g., 0.5).
- `Action`: Should match the expected action you specified when rendering the reCAPTCHA widget.

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
        var validationResponse = await _recaptchaService.ValidateAsync(recaptchaResponse, HttpContext.Connection.RemoteIpAddress);

        if (validationResponse.Success)
        {
            // reCAPTCHA validation succeeded
            if (validationResponse.Score >= 0.5)
            {
                // High confidence in the user's interaction
                if (validationResponse.Action == "submit_form")
                {
                    // Action matches expected value
                }
                else
                {
                    // Action does not match expected value
                }
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

## reCAPTCHA Enterprise

For Google reCAPTCHA Enterprise assessments, install the companion package:

```
dotnet add package Pandorax.RecaptchaEnterprise
```

Configure the Enterprise options (project ID, default site key, and optional expected action) and register the service:

```csharp
builder.Services.AddRecaptchaEnterprise(options =>
{
    options.ProjectId = builder.Configuration["RecaptchaEnterprise:ProjectId"];
    options.SiteKey = builder.Configuration["RecaptchaEnterprise:SiteKey"];
});
```

The Enterprise service creates `CreateAssessment` requests using Google's `Google.Cloud.RecaptchaEnterprise.V1` client. Supply Google Cloud credentials (environment variable or workload identity) so the client can authenticate. The resulting `IRecaptchaEnterpriseService` returns both the risk score and token metadata, allowing you to enforce action matches and score thresholds.
