using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Cheeseria.API
{
    /// <summary>
    /// Application Entry Point
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Entry Point
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Standard ASP.NET Create Host Builder.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
