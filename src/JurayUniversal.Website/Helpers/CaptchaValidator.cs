using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using JurayUniversal.Domain.Models;

namespace JurayUniversal.Website.Helpers
{
    public static class CaptchaValidator
    {
        public static async Task<bool> ValidateCaptchaAsync(
            MisSetting setting, 
            int? firstNumber, 
            int? secondNumber, 
            int? captchaAnswer, 
            string recaptchaResponse)
        {
            if (setting == null || setting.ActiveCaptcha == Domain.Enum.Enum.CaptchaType.None)
            {
                return true; // Captcha is disabled
            }

            if (setting.ActiveCaptcha == Domain.Enum.Enum.CaptchaType.Arithmetic)
            {
                if (firstNumber == null || secondNumber == null || captchaAnswer == null)
                    return false;
                    
                return (firstNumber + secondNumber) == captchaAnswer;
            }

            if (setting.ActiveCaptcha == Domain.Enum.Enum.CaptchaType.GoogleRecaptcha)
            {
                if (string.IsNullOrEmpty(recaptchaResponse) || string.IsNullOrEmpty(setting.GoogleRecaptchaSecretKey))
                    return false;

                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify",
                        new FormUrlEncodedContent(new[]
                        {
                            new KeyValuePair<string, string>("secret", setting.GoogleRecaptchaSecretKey),
                            new KeyValuePair<string, string>("response", recaptchaResponse)
                        }));

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        using (var doc = JsonDocument.Parse(json))
                        {
                            if (doc.RootElement.TryGetProperty("success", out var successElement))
                            {
                                return successElement.GetBoolean();
                            }
                        }
                    }
                }
                return false;
            }

            return true;
        }
    }
}
