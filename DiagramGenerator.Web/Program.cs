using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace DiagramGenerator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //.ConfigureAppConfiguration(AddConfiguration)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        //private static void AddConfiguration(HostBuilderContext ctx, IConfigurationBuilder bldr)
        //{
        //    bldr.Sources.Clear();
        //    bldr.SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("config.json")
        //        .AddEnvironmentVariables();
        //}
    }
}
