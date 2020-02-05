using Sims3.Gameplay.ActiveCareer.ActiveCareers;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.ActorSystems.Children;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Careers;
using Sims3.Gameplay.Controllers;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Interfaces;
using Sims3.Gameplay.Skills;
using Sims3.Gameplay.Socializing;
using Sims3.SimIFace;
using Sims3.SimIFace.CAS;
using System;
using System.Collections.Generic;
using System.Text;
using static ArthurGibraltarSims3Mod.Alive;
using static ArthurGibraltarSims3Mod.Interaction;
namespace ArthurGibraltarSims3Mod{
    public class GiveBottleFix:GiveBottle,IPreLoad,IAddInteraction{
        static InteractionDefinition sOldSingleton;
                                              public void AddInteraction(InteractionInjectorList interactions){
                                                                                                 interactions.Replace<Sim,GiveBottle.Definition>(Singleton);
                                              }
                                   public void OnPreLoad(){
            Tunings.Inject<Sim,GiveBottle.Definition,Definition>(false);
                                     sOldSingleton=Singleton;
                                                   Singleton=new Definition();
                                   }
        public override bool Run(){
            if(!ChildUtils.StartInteractionWithCarriedChild((SocialInteraction)this,"BeGivenBottle")){
                    if(this.Actor.SimDescription!=null&&this.Actor.SimDescription.IsBonehilda){
                        Alive.WriteLog("GiveBottle:Bonehilda:Run:FAIL[0]");
                    }
        return(false);
            }
this.BeginCommodityUpdates();
          this.Actor.CarryingChildPosture.AnimateInteractionWithCarriedChild(nameof(GiveBottle));
          this.Target.Motives.SetMax(CommodityKind.Hunger);
this.EndCommodityUpdates(true);
            if(!ChildUtils.FinishInteractionWithCarriedChild((SocialInteraction)this)){
                    if(this.Actor.SimDescription!=null&&this.Actor.SimDescription.IsBonehilda){
                        Alive.WriteLog("GiveBottle:Bonehilda:Run:FAIL[1]");
                    }
        return(false);
            }else{
        return( true);
            }
        }
        public new class Definition:GiveBottle.Definition{
            public override InteractionInstance CreateInstance(ref InteractionInstanceParameters parameters){
                            InteractionInstance na=new GiveBottleFix();
                                                na.Init(ref parameters);
                                         return na;
            }
            public override bool Test(Sim a,Sim target,bool isAutonomous,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
                if(target.SimDescription.Age!=CASAgeGenderFlags.Baby){
                    if(a.SimDescription!=null&&a.SimDescription.IsBonehilda){
                        Alive.WriteLog("GiveBottle:Bonehilda:Test:FAIL[0]");
                    }
            return(false);
                }
            return( true);}
        }
    }
    public class PickUpChildFix:PickUpChild,IPreLoad,IAddInteraction{
        static InteractionDefinition sOldSingleton;
                                              public void AddInteraction(InteractionInjectorList interactions){
                                                                                                 interactions.Replace<Sim,PickUpChild.Definition>(Singleton);
                                              }
                                   public void OnPreLoad(){
            Tunings.Inject<Sim,PickUpChild.Definition,Definition>(false);
                                     sOldSingleton=Singleton;
                                                   Singleton=new Definition();
                                   }
        public new class Definition:PickUpChild.Definition{
            public override InteractionInstance CreateInstance(ref InteractionInstanceParameters parameters){
                            InteractionInstance na=new PickUpChildFix();
                                                na.Init(ref parameters);
                                         return na;
            }
            public override bool Test(Sim actor,Sim target,bool isAutonomous,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
                if(!(target.SimDescription.ToddlerOrBelow)){
                    //if(actor.SimDescription!=null&&actor.SimDescription.IsBonehilda){
                        //Alive.WriteLog("PickUpChild:Bonehilda:Test:FAIL[4]");
                    //}
            return(false);
                }
                if(!(!actor.SimDescription.ChildOrBelow&&target.Posture.Container==target)){
                    //if(actor.SimDescription!=null&&actor.SimDescription.IsBonehilda){
                        //Alive.WriteLog("PickUpChild:Bonehilda:Test:FAIL[0]:"        +target.Name+":"+target.Posture+":"+target.Posture.Container+":"+actor.SimDescription.ChildOrBelow);
                    //}
            return(false);
                }else{
                        //Alive.WriteLog("PickUpChild:"+actor.Name+":Test:SUCCESS[0]:"+target.Name+":"+target.Posture+":"+target.Posture.Container+":"+actor.SimDescription.ChildOrBelow);
                }
                if(!(actor.CarryingChildPosture==null)){
                    //if(actor.SimDescription!=null&&actor.SimDescription.IsBonehilda){
                        //Alive.WriteLog("PickUpChild:Bonehilda:Test:FAIL[1]");
                    //}
            return(false);
                }
                if(!(target!=actor.Posture.Container)){
                    //if(actor.SimDescription!=null&&actor.SimDescription.IsBonehilda){
                        //Alive.WriteLog("PickUpChild:Bonehilda:Test:FAIL[2]:"        +target.Name+":"+actor.Posture+":"+actor.Posture.Container);
                    //}
            return(false);
                }else{
                        //Alive.WriteLog("PickUpChild:"+actor.Name+":Test:SUCCESS[2]:"+target.Name+":"+actor.Posture+":"+actor.Posture.Container);
                }
                if(!(!target.Posture.Satisfies(CommodityKind.InFairyHouse,(IGameObject)null))){
                    //if(actor.SimDescription!=null&&actor.SimDescription.IsBonehilda){
                        //Alive.WriteLog("PickUpChild:Bonehilda:Test:FAIL[3]");
                    //}
            return(false);
                }else{
                        //Alive.WriteLog("PickUpChild:"+actor.Name+":Test:SUCCESS[3]");
                }
            return( true);}
            //----------------------------------------
            public override InteractionTestResult Test(ref InteractionInstanceParameters parameters,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
                if(parameters.EffectivelyAutonomous&&parameters.Priority.Level<InteractionPriorityLevel.ESRB&&(parameters.Actor is Sim actor&&actor.CarryingChildPosture!=null)){
                    if(actor.SimDescription!=null&&actor.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:InteractionTest:FAIL[0]");
                    }
            return(InteractionTestResult.GenericFail);
                }
                                    InteractionTestResult result=TestFix(ref parameters,ref greyedOutTooltipCallback);
                if(!InteractionDefinitionUtilities.IsPass(result)){
                    if(parameters.Actor is Sim actor1&&actor1.SimDescription!=null&&actor1.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:InteractionTest:FAIL[1]:"+result);
                    }
            return(result);
                }
                if(PickUpChild.Definition.CanPickUpBabyOrToddler(ref parameters)){
            return(InteractionTestResult.Pass);
                }else{
                    if(parameters.Actor is Sim actor2&&actor2.SimDescription!=null&&actor2.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:InteractionTest:FAIL[2]");
                    }
                }
                greyedOutTooltipCallback=!((Sim)parameters.Actor).IsFemale?new GreyedOutTooltipCallback(PickUpChild.Definition.CantPickUpGreyedOutTooltipMale):
                                                                           new GreyedOutTooltipCallback(PickUpChild.Definition.CantPickUpGreyedOutTooltipFemale);
            return(InteractionTestResult.GenericFail);
            }
            public virtual InteractionTestResult TestFix(ref InteractionInstanceParameters parameters,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
                Sim  actor1=parameters. Actor as Sim;
                Sim target1=parameters.Target as Sim;
                if((object) actor1==null){
            return(InteractionTestResult.Root_Null_Actor );
                }
                if((object)target1==null){
            return(InteractionTestResult.Root_Null_Target);
                }
                Sim  actor2=(object)actor1 as Sim;
        IGameObject target2=(IGameObject)target1;
                for(IGameObject gameObject=(IGameObject)target1;gameObject!=null;gameObject=gameObject.Parent){
                             if(gameObject==Sims3.Gameplay.UI.LiveDragHelperModel.CachedTopDraggedObject)
            return(InteractionTestResult.Root_TargetOnHandTool);
                }
                bool flag=( true);
                InteractionTuning tuning=parameters.InteractionObjectPair.Tuning;
                Tradeoff mTradeoff=tuning?.mTradeoff;
                if(tuning!=null){
                    CommodityKind workMotive=actor1.WorkMotive;
                               if(workMotive!=CommodityKind.None&&mTradeoff.SatisfiesCommodity(workMotive)){
                     flag=(false);
                               }
                    if(parameters.Autonomous){
                        if(tuning.HasFlags(InteractionTuning.FlagField.DisallowAutonomous)&&flag){
            return(InteractionTestResult.Tuning_DisallowAutonomous);
                        }
                        if(mTradeoff.FunExit&&actor1.Motives.FunInteractionTest(false))
            return(InteractionTestResult.Tuning_FunInteractionTest);
                    }else 
                    if(tuning.HasFlags(InteractionTuning.FlagField.DisallowUserDirected)){
            return(InteractionTestResult.Tuning_DisallowUserDirected);
                    }
                    if(actor1.IsSelectable&&tuning.HasFlags(InteractionTuning.FlagField.DisallowPlayerSim)){
            return(InteractionTestResult.Tuning_DisallowPlayerSim);
                    }
                    if(flag){
                                     AutonomyFix fix=actor1.Autonomy as AutonomyFix;
     InteractionTestResult interactionTestResult=fix.CheckAvailability(parameters.Autonomous,tuning.Availability,parameters.InteractionObjectPair);
                        if(interactionTestResult!=InteractionTestResult.Pass){
                            if(actor1.SimDescription!=null&&actor1.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:TestFix:FAIL[0]:"+interactionTestResult);
                            }
            return(interactionTestResult);
                        }
                    }
                }
                actor1.Autonomy.UpdateCacheIfNeeded((IGameObject)target1);
               var interactionTestResult1=InteractionDefinitionUtilities.CommonTests((InteractionDefinition)this,actor2,target2,parameters);
                if(interactionTestResult1!=InteractionTestResult.Pass){
                            if(actor1.SimDescription!=null&&actor1.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:TestFix:FAIL[1]:"+interactionTestResult1);
                            }
            return(interactionTestResult1);
                }
               var interactionTestResult2=!(this is IMetaInteractionDefinition)?    InteractionDefinitionUtilities.SpecialCaseTests((InteractionDefinition)this,actor2,target2,parameters)
                                                                               :MetaInteractionDefinitionUtilities.SpecialCaseTests(actor2,target2,parameters);
                if(interactionTestResult2!=InteractionTestResult.Pass){
                            if(actor1.SimDescription!=null&&actor1.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:TestFix:FAIL[2]:"+interactionTestResult2+":"+(this is IMetaInteractionDefinition));
                            }
            return(interactionTestResult2);
                }
                if(!this.Test(actor1,target1,parameters.Autonomous,ref greyedOutTooltipCallback)){
                            //if(actor1.SimDescription!=null&&actor1.SimDescription.IsBonehilda){
                        //Alive.WriteLog("PickUpChild:Bonehilda:TestFix:FAIL[5]");
                            //}
            return(InteractionTestResult.Def_TestFailed);
                }
                if(tuning!=null){
 InteractionTestResult interactionTestResult3=actor1.Autonomy.CheckAvailabilityTooltip((InteractionDefinition)this,(IGameObject)target1,tuning.Availability,parameters,mTradeoff,ref greyedOutTooltipCallback);
                    if(interactionTestResult3!=InteractionTestResult.Pass){
                            if(actor1.SimDescription!=null&&actor1.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:TestFix:FAIL[3]:"+interactionTestResult3);
                            }
            return(interactionTestResult3);
                    }
                }
               var interactionTestResult4=InteractionDefinitionUtilities.SpecialCaseTooltipTests((InteractionDefinition)this,actor2,target2,parameters,ref greyedOutTooltipCallback);
                if(interactionTestResult4!=InteractionTestResult.Pass){
                            if(actor1.SimDescription!=null&&actor1.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:TestFix:FAIL[4]:"+interactionTestResult4);
                            }
            return(interactionTestResult4);
                }
            return(InteractionTestResult.Pass);
            }
            //----------------------------------------
        }
    }
    public class AutonomyFix:Autonomy{
        public new InteractionTestResult CheckAvailability(bool autonomous,Availability availability,InteractionObjectPair iop){
            CASAGSAvailabilityFlags availabilityFlags=CASUtils.CASAGSAvailabilityFlagsFromCASAgeGenderFlags(this.mActor.SimDescription.Age|this.mActor.SimDescription.Species);
                                if((availabilityFlags&availability.AgeSpeciesAvailabilityFlags)!=availabilityFlags)
        return(InteractionTestResult.Tuning_Age);
            if(availability.WorldRestrictionType==WorldRestrictionType.Allow){
                if(!availability.WorldRestrictionWorldTypes.Contains(GameUtils.GetCurrentWorldType())&&!availability.WorldRestrictionWorldNames.Contains(GameUtils.GetCurrentWorld()))
        return(InteractionTestResult.Tuning_World);
            }else 
            if(availability.WorldRestrictionType==WorldRestrictionType.Disallow&&(availability.WorldRestrictionWorldTypes.Contains(GameUtils.GetCurrentWorldType())||availability.WorldRestrictionWorldNames.Contains(GameUtils.GetCurrentWorld())))
        return(InteractionTestResult.Tuning_World);
            if(availability.MotiveThresholdType!=CommodityKind.None){
                float num=this.Motives.GetValue(availability.MotiveThresholdType);
                if(availability.HasFlags(Availability.FlagField.MotiveBelowCheck)){
                    if((double)num>(double)availability.MotiveThresholdValue)
        return(InteractionTestResult.Tuning_MotiveTooHigh);
                }else 
                if((double)num<(double)availability.MotiveThresholdValue)
        return(InteractionTestResult.Tuning_MotiveTooLow);
            }
            if((autonomous||this.mActor.SimDescription.IsEP11Bot)&&availability.ExcludingTraits!=null){
                foreach(TraitNames excludingTrait in availability.ExcludingTraits){
                    if(this.mActor.TraitManager.HasElement(excludingTrait))
        return(InteractionTestResult.Tuning_HasTrait);
                }
            }
            if(availability.RequiredTraits!=null){
                bool flag1=this.mActor.HasTrait(TraitNames.RobotHiddenTrait);
                bool flag2=false;
                bool flag3=false;
                foreach(TraitNames requiredTrait in availability.RequiredTraits){
                bool flag4=ActionData.IsBotSpecificTrait(requiredTrait);
                    if(flag4&&flag1)
                        flag3|=this.mActor.TraitManager.HasElement(requiredTrait);
                    else
                    if(!flag4&&!flag1){
                        flag2=true;
                        flag3|=this.mActor.TraitManager.HasElement(requiredTrait);
                    }
                }
                if((flag2||flag1)&&!flag3)
        return(InteractionTestResult.Tuning_MissingTrait);
            }
            if(availability.SkillThresholdType!=SkillNames.None&&this.mActor.SkillManager.GetSkillLevel(availability.SkillThresholdType)<availability.SkillThresholdValue)
        return(InteractionTestResult.Tuning_SkillTooLow);
          if (availability.CareerThresholdType != OccupationNames.Undefined)
          {
            bool flag = true;
            if (availability.HasFlags(Availability.FlagField.IncludePastCareers) && this.mActor.CareerManager.PreviouslySatisfied(availability.CareerThresholdType, availability.CareerThresholdValue))
              flag = false;
            if (flag)
            {
              if (this.mActor.Occupation == null)
                return InteractionTestResult.Tuning_NoCareer;
              if (this.mActor.Occupation.Guid != availability.CareerThresholdType)
                return InteractionTestResult.Tuning_WrongCareer;
              if (this.mActor.Occupation.HighestLevelAchieved < availability.CareerThresholdValue)
                return InteractionTestResult.Tuning_CareerLevelTooLow;
            }
          }
          if (availability.ExcludingBuffs != null)
          {
            foreach (BuffNames excludingBuff in availability.ExcludingBuffs)
            {
              if (this.mActor.BuffManager.HasElement(excludingBuff))
                return InteractionTestResult.Tuning_HasBuff;
            }
          }
          if (availability.RequiredBuffs != null)
          {
            bool flag = false;
            foreach (BuffNames requiredBuff in availability.RequiredBuffs)
            {
              if (this.mActor.BuffManager.HasElement(requiredBuff))
              {
                flag = true;
                break;
              }
            }
            if (!flag)
              return InteractionTestResult.Tuning_MissingBuff;
          }
          Lot lot = iop.Target.GetOwnerLot();
          if (iop.InteractionDefinition is ISoloInteractionDefinition)
            lot = this.mActor.LotCurrent;
          DaycareSituation daycareSituationForSim = DaycareSituation.GetDaycareSituationForSim(this.mActor);
          if ((lot == null || !lot.IsResidentialOwnedBy(this.Actor)) && (daycareSituationForSim == null || daycareSituationForSim.Lot != lot) && !this.CheckAvailabilityOnLot(iop, availability, iop.Target.LotCurrent, autonomous))
            return InteractionTestResult.Tuning_LotAvailability;
          if (availability.HasFlags(Availability.FlagField.DisallowedFromInventory) && this.mActor.Inventory.Contains(iop.Target))
            return InteractionTestResult.Tuning_InInventory;
          if (this.CurrentSearchType != AutonomySearchType.PostureTransition && autonomous && !availability.HasFlags(Availability.FlagField.AllowInTombRoomAutonomous) && (this.IsActorInTombRoom || TombRoomManager.IsObjectInATombRoom(iop.Target)))
            return InteractionTestResult.Tuning_AutonomousDisableInTombRoom;
          switch (availability.MoodThresholdType)
          {
            case MoodThresholdType.TrueOnlyIfMoodBelowBad:
              if ((double) this.mActor.MoodManager.MoodValue > (double) MoodManager.MoodStrongNegativeValue)
                return InteractionTestResult.Tuning_MoodIsNotBad;
              break;
            case MoodThresholdType.TrueOnlyIfMoodAboveBad:
              if ((double) this.mActor.MoodManager.MoodValue < (double) MoodManager.MoodStrongNegativeValue)
                return InteractionTestResult.Tuning_MoodIsBad;
              break;
            case MoodThresholdType.TrueOnlyIfMoodBelowThreshold:
              if ((double) this.mActor.MoodManager.MoodValue > (double) availability.MoodThresholdValue)
                return InteractionTestResult.Tuning_MoodTooHigh;
              break;
            case MoodThresholdType.TrueOnlyIfMoodAboveThreshold:
              if ((double) this.mActor.MoodManager.MoodValue < (double) availability.MoodThresholdValue)
                return InteractionTestResult.Tuning_MoodTooLow;
              break;
          }
          return availability.OccultRestrictionType != OccultRestrictionType.Ignore && (availability.HasFlags(Availability.FlagField.OccultRestrictionsHumanDisallowed) && this.mActor.CurrentOccultType == Sims3.UI.Hud.OccultTypes.None && availability.OccultRestrictionType == OccultRestrictionType.Inclusive || !availability.HasFlags(Availability.FlagField.OccultRestrictionsHumanDisallowed) && this.mActor.CurrentOccultType == Sims3.UI.Hud.OccultTypes.None && availability.OccultRestrictionType == OccultRestrictionType.Exclusive || ((availability.OccultRestrictions ^ (availability.OccultRestrictions | this.mActor.OccultManager.CurrentOccultTypes)) != Sims3.UI.Hud.OccultTypes.None && availability.OccultRestrictionType == OccultRestrictionType.Inclusive || (availability.OccultRestrictions & this.mActor.OccultManager.CurrentOccultTypes) != Sims3.UI.Hud.OccultTypes.None && availability.OccultRestrictionType == OccultRestrictionType.Exclusive)) ? InteractionTestResult.Tuning_OccultTypeNotAllowed : InteractionTestResult.Pass;
        }
    }
}