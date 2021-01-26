using ConigExamples.Configuration;
using Microsoft.Extensions.Options;

namespace ConigExamples.Infrastructure
{
    public class PersonConfigValidation : IValidateOptions<PersonConfig>
    {
        public ValidateOptionsResult Validate(string name, PersonConfig options)
        {
            switch (name)
            {
                case PersonConfig.Person1:
                    if (options.Age > 80)
                    {
                        return ValidateOptionsResult.Fail("Too old");
                    }
                    break;

                case PersonConfig.Person2:
                    if (options.Age < 20)
                    {
                        return ValidateOptionsResult.Fail("Too young");
                    }
                    break;

                default:
                    return ValidateOptionsResult.Skip;
            }

            return ValidateOptionsResult.Success;
        }
    }
}
