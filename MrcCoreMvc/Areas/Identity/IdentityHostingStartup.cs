using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MrcCoreMvc.Areas.Identity.Data;
using MrcCoreMvc.Data;

[assembly: HostingStartup(typeof(MrcCoreMvc.Areas.Identity.IdentityHostingStartup))]
namespace MrcCoreMvc.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<MrcDbContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("MrcDbContextConnection")));

                services.AddDefaultIdentity<ApplicationUser>(options => {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                })
                    .AddEntityFrameworkStores<MrcDbContext>();
            });
        }
    }
}