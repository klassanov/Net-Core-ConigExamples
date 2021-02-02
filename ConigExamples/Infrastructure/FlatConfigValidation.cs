using ConigExamples.Configuration;
using Microsoft.Extensions.Options;

namespace ConigExamples.Infrastructure
{
    public class FlatConfigValidation : IValidateOptions<FlatConfig>
    {
        //Sections cross validation example
        //This validation approach supports DI and also other properties of the config file can be used to validate
        private readonly BeerConfig _beerConfig;

        public FlatConfigValidation(IOptions<BeerConfig> beerConfig)
        {
            _beerConfig = beerConfig.Value;
        }

        public ValidateOptionsResult Validate(string name, FlatConfig options)
        {
            if (options.Price > 100000)
            {
                return ValidateOptionsResult.Fail("Flat is too expensive");
            }

            //Depend on another configuration
            else if (_beerConfig.Volume < 330)
            {
                return ValidateOptionsResult.Fail("I need more beer to decide");
            }

            return ValidateOptionsResult.Success;
        }
    }
}
