using System.Threading.Tasks;

namespace JurayUniversal.Website.Services
{
    public interface IMisSmartAssistService
    {
        Task<string> GenerateChatResponseAsync(string prompt, string? systemMessage = null);
    }
}
