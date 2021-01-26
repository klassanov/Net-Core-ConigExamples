using ConigExamples.Configuration;
using Microsoft.Extensions.Options;

namespace ConigExamples.Infrastructure
{
    public class Car
    {
        private IOptions<CarConfig> _options;

        public Car(IOptions<CarConfig> options)
        {
            _options = options;
        }

        public string PresentYourself()
        {
            return $"Hi, I am a {_options.Value.Color} car!";
        }
    }
}
