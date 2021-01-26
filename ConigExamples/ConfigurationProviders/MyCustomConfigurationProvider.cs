using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace ConigExamples.ConfigurationProviders
{
    public class MyCustomConfigurationProvider : ConfigurationProvider
    {
        //can be for ex. a provider that loads the data from the database
        public MyCustomConfigurationProvider() : base()
        {

        }

        public override void Load()
        {
            Data = new Dictionary<string, string>() {
                { "bla", "bla" }
            };
        }
    }
}
