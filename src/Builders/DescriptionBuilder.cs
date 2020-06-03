using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace library_volunteering_enquiry_service.Builders
{
    [ExcludeFromCodeCoverage]
    public class DescriptionBuilder
    {
        private string Description { get; set; } = "";

        public DescriptionBuilder Add(string key, List<string> values, string delimiter = " ")
        {
            if(values.Count > 0)
                Description += $"{key}: {string.Join(delimiter, values.Where(_ => !string.IsNullOrEmpty(_)))} \n";

            return this;
        }

        public DescriptionBuilder Add(string key, string value)
        {
            Description += $"{key}: {value} \n";

            return this;
        }

        public DescriptionBuilder Add(string value)
        {
            Description += $"{value} \n";

            return this;
        }

        public string Build() => Description;
    }
}
