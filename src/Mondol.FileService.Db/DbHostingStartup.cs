using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Mondol.FileService.Db.Repositories;
using Mondol.FileService.Db.Repositories.Impls;

[assembly: HostingStartup(typeof(Mondol.FileService.Db.DbHostingStartup))]
namespace Mondol.FileService.Db
{
    /// <summary>
    /// DB层服务配置
    /// </summary>
    public class DbHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddScoped<MasterDbContext>();

                services.AddScoped<IFileRepository, FileRepository>();
                services.AddScoped<IOwnerRepository, OwnerRepository>();

                services.AddSingleton<IRepositoryAccessor, RepositoryAccessor>();
            });
        }
    }
}
