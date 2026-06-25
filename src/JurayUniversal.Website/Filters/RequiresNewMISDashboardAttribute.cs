using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Filters
{
    public class RequiresNewMISDashboardAttribute : Attribute, IAsyncPageFilter
    {
        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            var dbContext = context.HttpContext.RequestServices.GetService(typeof(DashboardDbContext)) as DashboardDbContext;
            
            if (dbContext != null)
            {
                var superSetting = await dbContext.SuperSettings.FirstOrDefaultAsync();
                
                if (superSetting == null || !superSetting.UseNewMISDashboard)
                {
                    context.Result = new NotFoundResult();
                    return;
                }
            }

            await next();
        }

        public async Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            await Task.CompletedTask;
        }
    }
}
