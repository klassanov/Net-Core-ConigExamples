using System.ComponentModel.DataAnnotations;

namespace ConigExamples.Configuration
{
    public class CarConfig //Validation
    {
        public string Color { get; set; }

        [Required(ErrorMessage = "Brand not specified!")]
        public string Brand { get; set; }

        public int Power { get; set; }
    }
}
