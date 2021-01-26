using ConigExamples.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace ConigExamples.Infrastructure
{
    //runs when application first starts
    //Generally IHostedService is used for long running background tasks
    //Here it is used for eager options validation
    //Should be registered in the DI container

    //Eager validation for everything, always needed
    public class ValidateOptionsService : IHostedService
    {
        private readonly ILogger<ValidateOptionsService> _logger;
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly IOptions<CarConfig> _carConfig;
        private readonly IOptions<BeerConfig> _beerConfig;
        IOptionsMonitor<PersonConfig> _personConfigNamedOptions;

        public ValidateOptionsService(
            ILogger<ValidateOptionsService> logger,
            IHostApplicationLifetime appLifetime,
            IOptions<CarConfig> carConfig,
            IOptions<BeerConfig> beerConfig,
            IOptionsMonitor<PersonConfig> personConfigNamedOptions)
        {
            _logger = logger;
            _appLifetime = appLifetime;
            _carConfig = carConfig;
            _beerConfig = beerConfig;
            _personConfigNamedOptions = personConfigNamedOptions;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _ = _carConfig.Value; //accessing triggers validation
                _ = _beerConfig.Value; //accessing triggers validation

                _ = _personConfigNamedOptions.Get(PersonConfig.Person1);
                _ = _personConfigNamedOptions.Get(PersonConfig.Person2);

            }
            catch (OptionsValidationException ex)
            {
                _logger.LogError("Options validation check failed");

                foreach (var failure in ex.Failures)
                {
                    _logger.LogError(failure);
                }

                //if the configuration cannot be validated, it is better not to run the application 
                _appLifetime.StopApplication();

            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
