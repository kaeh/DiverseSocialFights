using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace DiverseSocialFights.Helpers
{
    public static class ThingHelper
    {
        public static bool IsAlcohol(ThingDef resource)
        {
            return resource.IsPleasureDrug && resource.IsNutritionGivingIngestible && resource.LabelCap != "Ambrosia";
        }
    }
}
