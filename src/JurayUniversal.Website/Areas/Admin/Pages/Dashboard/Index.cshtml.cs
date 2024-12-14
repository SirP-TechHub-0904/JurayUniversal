using JurayUniversal.Application.Dtos;
using JurayUniversal.Application.Dtos.Dashboarrd;
using JurayUniversal.Application.Services;
using JurayUniversal.Domain.Models;
using JurayUniversal.Website.Areas.Admin.Pages.UserManagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace JurayUniversal.Website.Areas.Admin.Pages.Dashboard
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Admin,Manager,Editor,Administrator")]

    public class IndexModel : PageModel
    {
        private readonly UserManager<Profile> _userManager;

        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public IndexModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<DataListDto> DataListDto { get; set; } = new List<DataListDto>();
        public IList<DataListDto> DataListMain { get; set; } = new List<DataListDto>();
        public IList<QueryDataList> Query1 { get; set; } = new List<QueryDataList>();
        public IList<QueryDataList> Query2 { get; set; } = new List<QueryDataList>();

        public bool Custom { get; set; }
        public IList<DashboardData> DashboardDatas { get; set; }
        public IList<UserCategory> UserCategories { get; set; }

        public IList<XProjectTask> XProjectTask { get; set; }
        public IList<DataListDto> UserDashboard { get; set; } = new List<DataListDto>();
        public IList<DataListDto> UserAccountDashboard { get; set; } = new List<DataListDto>();
        public async Task OnGetAsync()
        {
            var dashboardsettings = await _context.DashboardSettings.FirstOrDefaultAsync();

            DashboardDatas = await _context.DashboardDatas
                .Include(x => x.DashboardDataLists)
                .ToListAsync();
            if(dashboardsettings != null) { 
            Custom = dashboardsettings.UserCustomDashboard;
                //
            }
            //

            var useraccount = await _context.UserCategories.Include(x=>x.Profiles).Where(x=>x.Publish == true && x.ShowInDashboard == true).ToListAsync();
            
            foreach(var item in useraccount)
            {
                UserAccountDashboard.Add(new DataListDto
                {
                    DataCount = item.Profiles.Count().ToString(),
                    DataTitle = item.Title,
                    DataPath = "/CH/Index",
                    DataArea = item.Id.ToString(),
                    DataColor = MainServices.SelectColor()
                });
            }
            var report = _context.Report.AsQueryable();

            DataListDto.Add(new DataListDto
            {
                DataCount = report.Count().ToString(),
                DataTitle = "REPORT",
                DataPath = "/IReport/Index",
                DataColor = MainServices.SelectColor()
            });

            var request = _context.ClientRequest.AsQueryable();

            DataListDto.Add(new DataListDto
            {
                DataCount = request.Count().ToString(),
                DataTitle = "CLIENT REQUEST",
                DataPath = "/IReport/Index",
                DataColor = MainServices.SelectColor()
            });

            var jobs = _context.Jobs.AsQueryable();

            DataListDto.Add(new DataListDto
            {
                DataCount = jobs.Count().ToString(),
                DataTitle = "JOBS",
                DataPath = "/IReport/Index",
                DataColor = MainServices.SelectColor()
            });

            var training = _context.Trainings.AsQueryable();

            DataListDto.Add(new DataListDto
            {
                DataCount = training.Count().ToString(),
                DataTitle = "TRAININGS",
                DataPath = "/IReport/Index",
                DataColor = MainServices.SelectColor()
            });

            var expenses = _context.CompayExpenses.AsQueryable();
            DataListDto.Add(new DataListDto
            {
                DataCount = expenses.Sum(x => x.Amount).ToString("C", CultureInfo.GetCultureInfo("en-NG")),
                DataTitle = "EXPENSES",
                DataPath = "/IReport/Index",
                DataColor = MainServices.SelectColor()
            });


            var blog = _context.Blogs.AsQueryable();
            DataListDto.Add(new DataListDto
            {
                DataCount = blog.Count().ToString(),
                DataTitle = "NEWS POST",
                DataPath = "/IReport/Index",
                DataColor = MainServices.SelectColor()
            });

            var testimony = _context.Testimonies.AsQueryable();
            DataListDto.Add(new DataListDto
            {
                DataCount = testimony.Count().ToString(),
                DataTitle = "TESTIMONIES",
                DataPath = "/IReport/Index",
                DataColor = MainServices.SelectColor()
            });


            var career = _context.CareerFiles.AsQueryable();
            DataListDto.Add(new DataListDto
            {
                DataCount = career.Count().ToString(),
                DataTitle = "CAREER",
                DataPath = "/IReport/Index",
                DataColor = MainServices.SelectColor()
            });



            DataListDto.Add(new DataListDto
            {
                DataCount = "0",
                DataTitle = "SMS SENT",
                DataPath = "/IReport/Index",
                DataColor = MainServices.SelectColor()
            });

            DataListDto.Add(new DataListDto
            {
                DataCount = "0",
                DataTitle = "EMAILS SENT",
                DataPath = "/IReport/Index",
                DataColor = MainServices.SelectColor()
            });
            DataListDto.Add(new DataListDto
            {
                DataCount = "0",
                DataTitle = "NEWSLETTER SENT",
                DataPath = "/IReport/Index",
                DataColor = MainServices.SelectColor()
            });

            DataListDto.Add(new DataListDto
            {
                DataCount = "0",
                DataTitle = "PROPOSALS SENT",
                DataPath = "/IReport/Index",
                DataColor = MainServices.SelectColor()
            });

            var UserDatasList = new List<ProfileDto>();
            var UserDatas = await _userManager.Users.ToListAsync();

            var excludedRoles = new List<string> { "mSuperAdmin", "SubAdmin" };

            foreach (var user in UserDatas)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                if (userRoles.All(roleName => !excludedRoles.Contains(roleName)))
                {
                    var profile = new ProfileDto
                    {
                        Fullname = user.FirstName + " " + user.MiddleName + " " + user.LastName,
                        Phone = user.PhoneNumber,
                        Email = user.Email,
                        Status = user.UserStatus,
                        Id = user.Id,
                        Roles = string.Join(", ", userRoles) // Combine roles into a comma-separated string
                    };

                    UserDatasList.Add(profile);
                }
            }

            var users = UserDatasList.AsQueryable();

            DataListMain.Add(new DataListDto
            {
                DataCount = users.Count().ToString(),
                DataTitle = "USERS",
                DataPath = "/UserManagement/Index",
                DataColor = MainServices.SelectColor()
            });
            DataListMain.Add(new DataListDto
            {
                DataCount = users.Where(x=>x.Gender == Domain.Enum.Enum.GenderStatus.Male).Count().ToString(),
                DataTitle = "MALES",
                DataPath = "/UserManagement/Index",
                DataColor = MainServices.SelectColor()
            });
            DataListMain.Add(new DataListDto
            {
                DataCount = users.Where(x => x.Gender == Domain.Enum.Enum.GenderStatus.Male).Count().ToString(),
                DataTitle = "FEMALES",
                DataPath = "/UserManagement/Index",
                DataColor = MainServices.SelectColor()
            });

            DataListMain.Add(new DataListDto
            {
                DataCount = "0",
                DataTitle = "CLIENTS",
                DataPath = "/IReport/Index",
                DataColor = MainServices.SelectColor()
            });

            var products = _context.Products.AsQueryable();

            DataListMain.Add(new DataListDto
            {
                DataCount = products.Count().ToString(),
                DataTitle = "PRODUCTS",
                DataPath = "/IProduct/Main/Index",
                DataArea = "Content",
                DataColor = MainServices.SelectColor()
            });

            var proposal = _context.Proposals.AsQueryable();

            DataListMain.Add(new DataListDto
            {
                DataCount = proposal.Count().ToString(),
                DataTitle = "PROPOSALS",
                DataPath = "/IProposalPage/Index",
                DataColor = MainServices.SelectColor()
            });

            try
            {



                Query1 = new List<QueryDataList>();
                Query1.Add(new QueryDataList { MainData = "State 1", AgainstData = "777", ColorData = "#FFA07A" });
                Query1.Add(new QueryDataList { MainData = "State 2", AgainstData = "343", ColorData = "#ffce56" });
                Query1.Add(new QueryDataList { MainData = "State 3", AgainstData = "6654", ColorData = "#D3D3D3" });
                Query1.Add(new QueryDataList { MainData = "State 4", AgainstData = "6643", ColorData = "#000000" });



            }
            catch (Exception c)
            {

            }
            try
            {



                Query2 = new List<QueryDataList>();
                Query2.Add(new QueryDataList { MainData = "State 1", AgainstData = "777", ColorData = "#FFA07A" });
                Query2.Add(new QueryDataList { MainData = "State 2", AgainstData = "343", ColorData = "#ffce56" });
                Query2.Add(new QueryDataList { MainData = "State 3", AgainstData = "6654", ColorData = "#D3D3D3" });
                Query2.Add(new QueryDataList { MainData = "State 4", AgainstData = "6643", ColorData = "#000000" });



            }
            catch (Exception c)
            {

            }
            //
            XProjectTask = await _context.XProjectTasks
                .Include(x => x.XProjectSection).ThenInclude(x => x.XProject)
                .Include(x => x.ApprovedBy)
                .Include(x => x.TestedBy)
                .Include(x => x.User)
                 
                .ToListAsync();
        }
    }

}
