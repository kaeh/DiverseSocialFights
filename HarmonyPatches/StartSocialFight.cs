using System;
using Harmony;
using RimWorld;
using Verse;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using DiverseSocialFights.Helpers;
using DiverseSocialFights.AlcoholContest;

namespace DiverseSocialFights.HarmonyPatches
{
    [HarmonyPatch(typeof(Pawn_InteractionsTracker))]
    [HarmonyPatch("StartSocialFight")]
    class StartSocialFight
    {
        static bool Prefix(Pawn_InteractionsTracker __instance, Pawn otherPawn)
        {
            Log.Message("Prefixing Rimworld.Pawn_InteractionsTracker.StartSocialFight");
            Pawn initiatorPawn = typeof(Pawn_InteractionsTracker).GetField("pawn", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance) as Pawn;
            Log.Message("InitiatorPawn : " + initiatorPawn);
            Log.Message("OtherPawn : " + otherPawn);
            return Dispatcher(initiatorPawn, otherPawn);
        }

        private static bool Dispatcher(Pawn initiator, Pawn otherPawn)
        {
            ThingDef alcoholUsed = null;
            // Check if alcohol contest is possible
            if (IsAlcoholContestPossible(initiator, otherPawn, ref alcoholUsed))
            {
                // TODO : Create classes handling that case
                Pawn_InteractionAlcoholContest.StartAlcoholContest(initiator, otherPawn, alcoholUsed);
                return false;
            }
            return true;
        }
        private static bool IsAlcoholContestPossible(Pawn initiator, Pawn otherPawn, ref ThingDef alcoholUsed)
        {
            // Check if both pawns are not teetotalers
            if (!(initiator.IsTeetotaler() || otherPawn.IsTeetotaler()))
            {
                Map currentMap = Find.Maps.Find(m => m.IsPlayerHome && m.mapPawns.AllPawnsSpawned.Find(p => p.Equals(initiator) || p.Equals(otherPawn)) != null);
                // Then check if there is available alcohol in colony's resources
                ResourceCounter resourceCounter = currentMap.resourceCounter;
                IList<ThingDef> availableAlcohol = resourceCounter.AllCountedAmounts.Keys.ToList().FindAll(k => ThingHelper.IsAlcohol(k) && EnoughForContest(k, resourceCounter));
                Log.Message("[DEBUG] Available alcohol type : " + availableAlcohol.Count);
                
                if (availableAlcohol.Count > 0)
                {
                    // Take alcohol with bigger count
                    alcoholUsed = availableAlcohol.ToList().OrderBy(t => resourceCounter.GetCount(t)).First();
                    Log.Message("[DEBUG] Contest will use " + availableAlcohol.First().ToString());
                    return true;
                }
            }
            return false;
        }
        private static bool EnoughForContest(ThingDef resource, ResourceCounter resourceCounter)
        {
            return resourceCounter.GetCount(resource) >= 20; // TODO : Change value with configuration ? but 20 should be minimum
        }
    }
}
