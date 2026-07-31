using JurayUniversal.Application.Dtos.MIS.UserManagement;
using JurayUniversal.Application.Repository.MIS.UserManagement;
using JurayUniversal.Domain.Models;
using JurayUniversal.Website.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisUserManagement.Pages.Dashboard
{
    [Authorize]
    [RequiresNewMISDashboard]
    public class KpiSettingsModel : PageModel
    {
        private readonly IMisUserManagementRepository _userManagementRepo;
        private readonly UserManager<Profile> _userManager;

        public KpiSettingsModel(IMisUserManagementRepository userManagementRepo, UserManager<Profile> userManager)
        {
            _userManagementRepo = userManagementRepo;
            _userManager = userManager;
        }

        public List<MisKpiIndicator> Indicators { get; set; } = new();
        public List<MisKpiEvaluationPeriod> EvaluationPeriods { get; set; } = new();

        [BindProperty]
        public MisKpiIndicatorDto IndicatorInput { get; set; } = new();

        [BindProperty]
        public MisKpiEvaluationPeriod PeriodInput { get; set; } = new();

        public bool IsReadOnly { get; set; }

        private async Task<bool> CheckAccessAndSetReadOnlyAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return false;

            var roles = await _userManager.GetRolesAsync(currentUser);
            bool hasAccess = roles.Contains("mSuperAdmin") || roles.Contains("Admin") || roles.Contains("UserManagement_GlobalEditor") || roles.Contains("UserManagement_FeatureEditor") || roles.Contains("UserManagement_Readonly");
            
            if (!hasAccess) return false;

            IsReadOnly = !roles.Contains("mSuperAdmin") && !roles.Contains("Admin") && !roles.Contains("UserManagement_GlobalEditor") && !roles.Contains("UserManagement_FeatureEditor");
            return true;
        }

        public async Task<IActionResult> OnGetAsync(long? editId, long? editPeriodId)
        {
            if (!await CheckAccessAndSetReadOnlyAsync()) return RedirectToPage("/Platform/Index", new { area = "MisUserManagement" });

            Indicators = await _userManagementRepo.GetAllKpiIndicatorsAsync();
            EvaluationPeriods = await _userManagementRepo.GetAllEvaluationPeriodsAsync();

            if (editId.HasValue)
            {
                var ind = await _userManagementRepo.GetKpiIndicatorByIdAsync(editId.Value);
                if (ind != null)
                {
                    IndicatorInput = new MisKpiIndicatorDto
                    {
                        Id = ind.Id,
                        Title = ind.Title,
                        Description = ind.Description,
                        MaxScore = ind.MaxScore,
                        SortOrder = ind.SortOrder,
                        IsActive = ind.IsActive
                    };
                }
            }

            if (editPeriodId.HasValue)
            {
                var p = await _userManagementRepo.GetEvaluationPeriodByIdAsync(editPeriodId.Value);
                if (p != null)
                {
                    PeriodInput = p;
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!await CheckAccessAndSetReadOnlyAsync()) return RedirectToPage("/Platform/Index", new { area = "MisUserManagement" });
            if (IsReadOnly)
            {
                TempData["error"] = "You do not have permission to modify KPIs.";
                return RedirectToPage();
            }

            if (!ModelState.IsValid)
            {
                Indicators = await _userManagementRepo.GetAllKpiIndicatorsAsync();
                EvaluationPeriods = await _userManagementRepo.GetAllEvaluationPeriodsAsync();
                return Page();
            }

            if (IndicatorInput.Id > 0)
            {
                await _userManagementRepo.UpdateKpiIndicatorAsync(IndicatorInput);
                TempData["success"] = "KPI indicator updated successfully!";
            }
            else
            {
                await _userManagementRepo.CreateKpiIndicatorAsync(IndicatorInput);
                TempData["success"] = "KPI indicator created successfully!";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(long id)
        {
            if (!await CheckAccessAndSetReadOnlyAsync()) return RedirectToPage("/Platform/Index", new { area = "MisUserManagement" });
            if (IsReadOnly)
            {
                TempData["error"] = "You do not have permission to delete KPIs.";
                return RedirectToPage();
            }

            if (id > 0)
            {
                await _userManagementRepo.DeleteKpiIndicatorAsync(id);
                TempData["success"] = "KPI indicator deleted successfully!";
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSavePeriodAsync()
        {
            if (!await CheckAccessAndSetReadOnlyAsync()) return RedirectToPage("/Platform/Index", new { area = "MisUserManagement" });
            if (IsReadOnly)
            {
                TempData["error"] = "You do not have permission to modify Evaluation Periods.";
                return RedirectToPage();
            }

            if (string.IsNullOrWhiteSpace(PeriodInput.Title))
            {
                TempData["error"] = "Please provide a valid Evaluation Period Title.";
                return RedirectToPage();
            }

            if (PeriodInput.Id > 0)
            {
                await _userManagementRepo.UpdateEvaluationPeriodAsync(PeriodInput);
                TempData["success"] = "Evaluation Period updated successfully!";
            }
            else
            {
                await _userManagementRepo.CreateEvaluationPeriodAsync(PeriodInput);
                TempData["success"] = "Evaluation Period created successfully!";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeletePeriodAsync(long id)
        {
            if (!await CheckAccessAndSetReadOnlyAsync()) return RedirectToPage("/Platform/Index", new { area = "MisUserManagement" });
            if (IsReadOnly)
            {
                TempData["error"] = "You do not have permission to delete Evaluation Periods.";
                return RedirectToPage();
            }

            if (id > 0)
            {
                await _userManagementRepo.DeleteEvaluationPeriodAsync(id);
                TempData["success"] = "Evaluation Period deleted successfully!";
            }
            return RedirectToPage();
        }
    }
}
