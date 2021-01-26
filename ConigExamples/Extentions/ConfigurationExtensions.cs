using ConigExamples.ConfigurationProviders;
using Microsoft.Extensions.Configuration;

namespace ConigExamples.Extentions
{
    public static class ConfigurationExtensions
    {
        public static IConfigurationBuilder AddMyCustomConfiguration(this IConfigurationBuilder builder)
        {
            return builder.Add(new MyCustomConfigurationSource());
        }
    }
}
