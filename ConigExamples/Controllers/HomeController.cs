using ConigExamples.Configuration;
using ConigExamples.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace ConigExamples.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IConfiguration configuration;
        private readonly Features features;

        public HomeController(
            ILogger<HomeController> logger,
            IConfiguration configuration,
            IOptions<Features> features, //Internally registered as singleton, so changes in appsettings at runtime are not reflected

            IOptionsSnapshot<Features> featuresSnapshot, //Internally registered as scoped => per request, so changes in appsettings at runtime are reflected

            IOptionsMonitor<Features> featuresMonitor, /*Internally registered as singleton, but changes in appsettings at runtime are reflected
                                                      * Scoped services cannot be used as a dependency of singleton serices. The IOptionsMonitor solves
                                                      * this problem, being able at the same time to load the latest changes at runtime
                                                      */

            IOptionsMonitor<PersonConfig> personConfig, //Named options

            IOptions<CarConfig> carConfig,

            IOptions<FlatConfig> flatConfig,

            IGimmyConfig gimmyConfig //forwarding (abstracting) to options via an interface. Consumer abstraction

            )
        {
            this.logger = logger;
            this.configuration = configuration;
            this.features = features.Value;

            var x = featuresSnapshot.Value;
            var y = featuresMonitor.CurrentValue;

            //Subscribe for changes in the configuration through the monitor
            featuresMonitor.OnChange(f =>
            {
                x = f; //Re-assign the lates changes without running the whole constructor, but only the callback
                System.Diagnostics.Debug.WriteLine("Configuration changed");
            });

            var a = personConfig.Get("Person1");
            var b = personConfig.Get("Person2");

            //Consume validated data. If you do not consume the data, exception will not be thrown, it is kind of lazy, even if it passed in the constructor
            var c = carConfig.Value;
            string color = c.Color;

            _ = flatConfig.Value;

            _ = gimmyConfig.Age;
        }

        public IActionResult Index()
        {
            //Way 1 : Flatten the ful key name every time
            bool greetingEnabled = this.configuration.GetValue<bool>("Features:HomePage:EnableGreeting");
            decimal paymentAmount = this.configuration.GetValue<decimal>("Features:HomePage:PaymentAmount");

            //Way 2: Use section and type only once the key within the section
            IConfigurationSection homePageFeatures = this.configuration.GetSection("Features:HomePage");
            bool greetingEnabled2 = homePageFeatures.GetValue<bool>("EnableGreeting");
            decimal paymentAmount2 = homePageFeatures.GetValue<decimal>("PaymentAmount");

            //Read connection strings
            var connectionString = this.configuration.GetConnectionString("DefaultConnection");

            //Binding
            Features f = new Features();
            this.configuration.Bind("Features:HomePage", f);

            //Configurations per environment ???
            string message = this.configuration.GetValue<string>("environmentTest");

            //Consume the injected IOptions<Features>
            var t = this.features.EnableGreeting;


            //Setting colors by reading configuration
            ViewBag.HeaderColor = this.configuration.GetValue<string>("ColorSettings:HeaderColor");
            ViewBag.TextColor = this.configuration.GetValue<string>("ColorSettings:TextColor");

            //Reading secrets via user secrets
            //Not saved in source-controlled files, not directly in configuration...but it is good only for development
            //For production (at least 2 options):
            //1. Azure Key Vault
            //2. AWS Parameter store
            //Loads the configuration at run time through a web service?
            var apiKey = this.configuration.GetValue<string>("ApiKey");

            //Custom configuration provider which loads data from a database table
            var bla = this.configuration.GetValue<string>("bla");

            //!!! in options we have everything configured from every provider...

            return View(new PageViewModel() { Message = message });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
