using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CarDealer.Resources
{
    public class LocService
    {
        private IStringLocalizer localizer;

        public LocService(IStringLocalizerFactory stringLocalizerFactory)
        {
            var type = typeof(SharedResource);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            localizer = stringLocalizerFactory.Create("SharedResource", assemblyName.Name);
        }
        public LocalizedString GetLocalizedHtmlString(string key)
        {
            return localizer[key];
        }
    }
}
