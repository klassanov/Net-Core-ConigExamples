using Microsoft.Extensions.Configuration;

namespace ConigExamples.ConfigurationProviders
{
    public class MyCustomConfigurationSource : IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new MyCustomConfigurationProvider();
        }
    }
}
