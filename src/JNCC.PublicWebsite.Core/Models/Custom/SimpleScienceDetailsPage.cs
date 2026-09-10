using DocumentFormat.OpenXml.EMMA;
using Umbraco.Cms.Core.Models;

namespace JNCC.PublicWebsite.Core.Models
{
    public partial class SimpleScienceDetailsPage : IScienceCategorisablePage
    {
        public string CategoryOrderingName => string.Empty;

        public MediaWithCrops HeroImage => null;
    }
}