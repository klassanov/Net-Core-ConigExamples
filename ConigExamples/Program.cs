using ConigExamples.Extentions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;

namespace ConigExamples
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Let's get rocked");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })

                //Add an additional configuration provider
                .ConfigureAppConfiguration((ctx, builder) =>
                {
                    if (ctx.HostingEnvironment.IsProduction())
                    {

                        //Builds the current configuration, so that we can access configuration defined so far
                        //temp instance of configuration
                        var config = builder.Build();

                        //More code
                        //...
                    }
                })

                //Take full control of providers: define the order (!THE LAST PROVIDER OVVERIDES THE KEYS DEFINED PREVIOUSLY!) and the options
                .ConfigureAppConfiguration((ctx, builder) =>
                {
                    //var env = ctx.HostingEnvironment;
                    //var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));

                    ////Access all the configuration providers
                    //builder.Sources.Clear();

                    //builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    //builder.AddEnvironmentVariables();
                    //builder.AddUserSecrets(appAssembly, optional: true);

                    //Custom configuration provider
                    builder.AddMyCustomConfiguration();
                })

            ;

    }
}
