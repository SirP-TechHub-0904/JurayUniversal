using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using JurayUniversal.Persistence.EF.SQL;

var optionsBuilder = new DbContextOptionsBuilder<DashboardDbContext>();
optionsBuilder.UseSqlServer(""Server=(localdb)\\mssqllocaldb;Database=JurayUniversalDb;Trusted_Connection=True;MultipleActiveResultSets=true"");
using (var context = new DashboardDbContext(optionsBuilder.Options))
{
    var settings = context.MisSettings.FirstOrDefault();
    if (settings != null)
    {
        Console.WriteLine($""EnableZeptoMail: {settings.EnableZeptoMail}"");
        Console.WriteLine($""ZeptoMailApiKey: {settings.ZeptoMailApiKey}"");
        Console.WriteLine($""ZeptoMailSenderEmail: {settings.ZeptoMailSenderEmail}"");
    }
    else
    {
        Console.WriteLine(""No MisSettings found in DB."");
    }
}
