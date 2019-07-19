using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorClientApp.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class BuildRenderTreeAttribute : Attribute
    {
        public BuildRenderTreeAttribute()
        {

        }

        public bool ShouldCallBaseClassBefore { get; set; }

        public bool ShouldCallBaseClassAfter { get; set; }
    }
}
