using Microsoft.AspNetCore.Authorization;

namespace JurayUniversal.Website.Middlewares
{
  public class MaxUserRequirement : IAuthorizationRequirement
{
    public int MaxUserCount { get; }

    public MaxUserRequirement(int maxUserCount)
    {
        MaxUserCount = maxUserCount;
    }
}
}
