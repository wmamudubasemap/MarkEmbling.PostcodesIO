using Newtonsoft.Json.Serialization;
using System.Text.RegularExpressions;

namespace MarkEmbling.PostcodesIO.Internals
{
    public class LowercaseWithUnderscoresContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            string name = Regex.Replace(propertyName, "([A-Z])", "_$1").ToLowerInvariant();
            if (name.StartsWith("_"))
            {
                name = name.Substring(1);
            }

            return name;
        }
    }
}