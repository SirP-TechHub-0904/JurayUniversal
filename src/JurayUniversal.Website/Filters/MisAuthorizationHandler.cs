using JurayUniversal.Website.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace JurayUniversal.Website.Filters
{
    public class MisFeatureRequirement : IAuthorizationRequirement
    {
        public string FeatureFolder { get; }
        public string MinimumRole { get; }

        public MisFeatureRequirement(string featureFolder, string minimumRole)
        {
            FeatureFolder = featureFolder;
            MinimumRole = minimumRole;
        }
    }

    public class MisAuthorizationHandler : AuthorizationHandler<MisFeatureRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MisAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MisFeatureRequirement requirement)
        {
            if (context.User == null)
            {
                return;
            }

            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return;
            }

            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                var authService = httpContext.RequestServices.GetRequiredService<IMisAuthorizationService>();

                bool hasAccess = false;
                if (requirement.MinimumRole == "GlobalEditor")
                {
                    hasAccess = await authService.CanEditAsync(context.User, requirement.FeatureFolder);
                }
                else
                {
                    hasAccess = await authService.HasAccessAsync(context.User, requirement.FeatureFolder);
                }
                
                if (hasAccess)
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}
