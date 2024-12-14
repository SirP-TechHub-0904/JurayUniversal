using JurayUniversal.Application.Dtos;
using JurayUniversal.Application.Repository.Services;
using JurayUniversal.Domain.Models.NIPSS;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Pages.Shared.ViewComponents
{

    public class UserEvaluationStatusViewComponent : ViewComponent
    {
        private readonly DashboardDbContext _context;
        private readonly ISettingsService _setting;
        public UserEvaluationStatusViewComponent(
            DashboardDbContext context, ISettingsService setting)
        {
            _context = context;
            _setting = setting;
        }


        public async Task<IViewComponentResult> InvokeAsync(long pid, long tid, long mid)
        {
            //check
            var check = await _context.AccountModeratorEvaluations.FirstOrDefaultAsync(x => x.ParticipantListId == pid && x.TimeTableId == tid && x.ModeratorId == mid);
            //var check = await _context.AccountModeratorEvaluations.FirstOrDefaultAsync(x => x.ParticipantListId == ParticipantListId && x.TimeTableId == TimetableId && x.ModeratorId == ModeratorId);


            //var check = await _context.AccountModeratorEvaluations
            //    .AsNoTracking()
            //    .FirstOrDefaultAsync(x => x.ParticipantListId == pid && x.TimeTableId == tid && x.ModeratorId == mid);
            if (check != null)
            {
                TempData["checkbutton"] = "evaluated";
            }

            ViewBag.Pid = pid;
            ViewBag.Mid = mid;
            ViewBag.Tid = tid;

            return View(check);
        }
    }
}
