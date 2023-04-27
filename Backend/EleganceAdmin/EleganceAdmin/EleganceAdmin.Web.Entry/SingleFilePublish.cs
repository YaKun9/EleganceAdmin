using Furion;
using System.Reflection;

namespace EleganceAdmin.Web.Entry
{
    public class SingleFilePublish : ISingleFilePublish
    {
        public Assembly[] IncludeAssemblies()
        {
            return Array.Empty<Assembly>();
        }

        public string[] IncludeAssemblyNames()
        {
            return new[]
            {
            "EleganceAdmin.Application",
            "EleganceAdmin.Core",
            "EleganceAdmin.EntityFramework.Core",
            "EleganceAdmin.Web.Core"
            };
        }
    }
}