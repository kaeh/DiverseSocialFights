using DiverseSocialFights.Defs;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace DiverseSocialFights.AlcoholContest
{
    public static class Pawn_InteractionAlcoholContest
    {
        public static void StartAlcoholContest(Pawn initiator, Pawn otherPawn, ThingDef usedAlcohol)
        {
            if (PawnUtility.ShouldSendNotificationAbout(initiator) || PawnUtility.ShouldSendNotificationAbout(otherPawn))
            {
                Thought thought;
                StringBuilder sb = new StringBuilder();
                if (!InteractionUtility.TryGetRandomSocialFightProvokingThought(initiator, otherPawn, out thought))
                {
                    sb.Append("Pawn ").Append(initiator).Append(" started an alcohol contest with ");
                    sb.Append(otherPawn).Append(", but he has no negative opinion thoughts towards ");
                    sb.Append(otherPawn).Append(".");
                    Log.Warning(sb.ToString());
                }
                else
                {

                    Messages.Message("MessageAlcoholContest".Translate(new object[]
                    {
                        initiator.LabelShort,
                        otherPawn.LabelShort,
                        thought.LabelCapSocial
                    }), initiator, MessageTypeDefOf.ThreatSmall);
                }
            }

            MentalStateHandler initiatorMentalStateHandler = initiator.mindState.mentalStateHandler;
            MentalStateDef alcoholContest = DSF_MentalStateDefOf.AlcoholContest;
            initiatorMentalStateHandler.TryStartMentalState(alcoholContest, null, false, false, otherPawn);

            MentalStateHandler otherPawnMentalStateHandler = otherPawn.mindState.mentalStateHandler;
            alcoholContest = DSF_MentalStateDefOf.AlcoholContest;

            Pawn otherPawn2 = initiator;
            otherPawnMentalStateHandler.TryStartMentalState(alcoholContest, null, false, false, otherPawn2);
            TaleRecorder.RecordTale(TaleDefOf.SocialFight, new object[]{ initiator, otherPawn });
        }
    }
}
