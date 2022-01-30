using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Maaltafels.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel()
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build().Run();
            
            //CreateHostBuilder(args).Build().Run();
        }

        // public static IHostBuilder CreateHostBuilder(string[] args) =>
        //     Host.CreateDefaultBuilder(args)
        //         .ConfigureWebHostDefaults(webBuilder =>
        //         {
        //             webBuilder.UseStartup<Startup>();
        //         });
    }
}
