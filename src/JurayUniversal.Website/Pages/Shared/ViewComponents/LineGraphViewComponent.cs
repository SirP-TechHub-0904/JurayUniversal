
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
    public class LineGraphViewComponent : ViewComponent
    {
        private readonly DashboardDbContext _context;

        public LineGraphViewComponent(
            DashboardDbContext context)
        {
            _context = context;
        }

        public string UserInfo { get; set; }
        public async Task<IViewComponentResult> InvokeAsync(string sn, string title)
        {

            ViewBag.DataTitle = title;
             
               var ChartData = new List<DataCategory>
        {
            new DataCategory
            {
                Title = "apples",
                QueryDataList = new List<QueryDataList>
                {
                    new QueryDataList { MainData = "M", AgainstData = "12", ColorData = "rgba(153,255,51,0.6)" },
                    new QueryDataList { MainData = "T", AgainstData = "19", ColorData = "rgba(153,255,51,0.6)" },
                    new QueryDataList { MainData = "W", AgainstData = "3", ColorData = "rgba(153,255,51,0.6)" },
                    new QueryDataList { MainData = "T", AgainstData = "17", ColorData = "rgba(153,255,51,0.6)" },
                    new QueryDataList { MainData = "F", AgainstData = "6", ColorData = "rgba(153,255,51,0.6)" },
                    new QueryDataList { MainData = "S", AgainstData = "3", ColorData = "rgba(153,255,51,0.6)" },
                    new QueryDataList { MainData = "S", AgainstData = "7", ColorData = "rgba(153,255,51,0.6)" }
                }
            },
            new DataCategory
            {
                Title = "oranges",
                QueryDataList = new List<QueryDataList>
                {
                    new QueryDataList { MainData = "M", AgainstData = "2", ColorData = "rgba(255,153,0,0.6)" },
                    new QueryDataList { MainData = "T", AgainstData = "29", ColorData = "rgba(255,153,0,0.6)" },
                    new QueryDataList { MainData = "W", AgainstData = "5", ColorData = "rgba(255,153,0,0.6)" },
                    new QueryDataList { MainData = "T", AgainstData = "5", ColorData = "rgba(255,153,0,0.6)" },
                    new QueryDataList { MainData = "F", AgainstData = "2", ColorData = "rgba(255,153,0,0.6)" },
                    new QueryDataList { MainData = "S", AgainstData = "3", ColorData = "rgba(255,153,0,0.6)" },
                    new QueryDataList { MainData = "S", AgainstData = "10", ColorData = "rgba(255,153,0,0.6)" }
                }
            }
        };
             
            
            ViewBag.outcomex = ChartData;
            ViewBag.sn = sn;
            return View();
        }
    }

}
