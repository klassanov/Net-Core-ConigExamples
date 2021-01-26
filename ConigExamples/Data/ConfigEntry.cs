using System.ComponentModel.DataAnnotations;

namespace ConigExamples.Data
{
    public class ConfigEntry
    {
        [Key]
        public string Key { get; set; }

        public string Value { get; set; }
    }
}
