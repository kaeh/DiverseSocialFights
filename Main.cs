using Harmony;
using Verse;
using System.Reflection;

namespace DiverseSocialFights
{
    [StaticConstructorOnStartup]
    class Main
    {
        static Main()
        {
            var harmony = HarmonyInstance.Create("kaeh.rimworld.diverseSocialFights");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
