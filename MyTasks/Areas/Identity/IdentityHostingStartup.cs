using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(MyTasks.Areas.Identity.IdentityHostingStartup))]
namespace MyTasks.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder) => builder.ConfigureServices((context, services) => {
            });
    }
}