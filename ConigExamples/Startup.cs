using ConigExamples.Configuration;
using ConigExamples.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ConigExamples
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            //Options pattern
            services.Configure<Features>(Configuration.GetSection("Features:HomePage"));

            //Named Options: Use when you have 2 different sections with the same properties
            services.Configure<PersonConfig>("Person1", Configuration.GetSection("Person1"));
            services.Configure<PersonConfig>("Person2", Configuration.GetSection("Person2"));

            //Options Validation - 1
            services.AddOptions<CarConfig>()
                .Bind(Configuration.GetSection("Car"))
                .ValidateDataAnnotations();

            //Options Validation - 2 (also in chaining)
            services.AddOptions<BeerConfig>()
                .Bind(Configuration.GetSection("Beer"))
                .Validate(b =>
                {
                    if (b.AlcoholPercentage < 5)
                    {
                        return false;
                    }
                    return true;
                }, "The beer should be stronger")

                .Validate(b =>
                {
                    if (b.Volume < 500)
                    {
                        return false;
                    }
                    return true;
                }, "The beer should be bigger");


            //Add hosted service that runs before the web application starts
            services.AddHostedService<ValidateOptionsService>();

            services.Configure<FlatConfig>(Configuration.GetSection("Flat"));

            services.TryAddEnumerable(ServiceDescriptor.Singleton<IValidateOptions<FlatConfig>, FlatConfigValidation>());
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IValidateOptions<PersonConfig>, PersonConfigValidation>());

            //Consumer abstractions
            services.Configure<GimmyConfig>(Configuration.GetSection("Gimmy"));
            services.AddSingleton<IGimmyConfig>(sp => sp.GetRequiredService<IOptions<GimmyConfig>>().Value);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });


            var connStr = Configuration.GetConnectionString("DefaultConnection");
        }
    }
}
