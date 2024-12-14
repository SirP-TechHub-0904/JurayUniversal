
using JurayUniversal.Application.Dtos;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Pages.Shared.ViewComponents
{
    public class MapViewComponent : ViewComponent
    {
        private readonly DashboardDbContext _context;

        public MapViewComponent(
            DashboardDbContext context)
        {
            _context = context;
        }

        public string UserInfo { get; set; }
        public async Task<IViewComponentResult> InvokeAsync(string sn, string title)
        {

            ViewBag.DataTitle = title;
            var list = new List<QueryDataList>();

            // Populate the list with data
            if (sn == "sssa")
            {
                list.Add(new QueryDataList { MainData = "State 1", AgainstData = "45", ColorData = "#00FF00" });
                list.Add(new QueryDataList { MainData = "State 2", AgainstData = "65", ColorData = "#C0C0C0" });
                list.Add(new QueryDataList { MainData = "State 3", AgainstData = "76", ColorData = "#D2691E" });
                list.Add(new QueryDataList { MainData = "State 4", AgainstData = "23", ColorData = "#FFF8DC" });

            }
            if (sn == "dfv")
            {
                list.Add(new QueryDataList { MainData = "State 1", AgainstData = "1000", ColorData = "#ff6384" });
                list.Add(new QueryDataList { MainData = "State 2", AgainstData = "2000", ColorData = "#ffce56" });
                list.Add(new QueryDataList { MainData = "State 3", AgainstData = "3000", ColorData = "#9400D3" });
                list.Add(new QueryDataList { MainData = "State 4", AgainstData = "7000", ColorData = "#000000" });
                list.Add(new QueryDataList { MainData = "State 1", AgainstData = "1000", ColorData = "#696969" });
                list.Add(new QueryDataList { MainData = "State 2", AgainstData = "2000", ColorData = "#9400D3" });

            }
            if (sn == "mmn")
            {
                list.Add(new QueryDataList { MainData = "State 1", AgainstData = "777", ColorData = "#FFA07A" });
                list.Add(new QueryDataList { MainData = "State 2", AgainstData = "343", ColorData = "#ffce56" });
                list.Add(new QueryDataList { MainData = "State 3", AgainstData = "6654", ColorData = "#D3D3D3" });
                list.Add(new QueryDataList { MainData = "State 4", AgainstData = "6643", ColorData = "#000000" });
                list.Add(new QueryDataList { MainData = "State 1", AgainstData = "1222", ColorData = "#ff6384" });
                list.Add(new QueryDataList { MainData = "State 2", AgainstData = "3", ColorData = "#E0FFFF" });
                list.Add(new QueryDataList { MainData = "State 2", AgainstData = "77", ColorData = "#ffce56" });
                list.Add(new QueryDataList { MainData = "State 3", AgainstData = "3000", ColorData = "#87CEFA" });
                list.Add(new QueryDataList { MainData = "State 4", AgainstData = "7000", ColorData = "#000000" });

            }
            ViewBag.outcomex = list;
            ViewBag.sn = sn;
            return View();
        }
    }

}
