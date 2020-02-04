using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.CAS;
using Sims3.Gameplay.Controllers;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Objects.Plumbing;
using Sims3.Gameplay.Skills;
using Sims3.Gameplay.TuningValues;
using Sims3.SimIFace;
using Sims3.SimIFace.CAS;
using System;
using System.Collections.Generic;
using System.Text;
using static ArthurGibraltarSims3Mod.Alive;
using static ArthurGibraltarSims3Mod.Interaction;
namespace ArthurGibraltarSims3Mod{
    public class ToiletRepairFix:Toilet.Repair,IPreLoad,IAddInteraction{
        static InteractionDefinition sOldSingleton;
                                              public void AddInteraction(InteractionInjectorList interactions){
                                                                                                 interactions.ReplaceNoTest<Toilet,Toilet.Repair.Definition>(Singleton);
                                              }
                                   public void OnPreLoad(){
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.Mimics.ToiletCheap            ,Toilet.Repair.Definition,Definition>(true);
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.Mimics.ToiletModerate         ,Toilet.Repair.Definition,Definition>(true);
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.Mimics.ToiletExpensive        ,Toilet.Repair.Definition,Definition>(true);
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.Mimics.ToiletDarkLux          ,Toilet.Repair.Definition,Definition>(true);
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.Mimics.ToiletDive             ,Toilet.Repair.Definition,Definition>(true);
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.Mimics.ToiletEgyptAncient     ,Toilet.Repair.Definition,Definition>(true);
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.Mimics.ToiletFuture           ,Toilet.Repair.Definition,Definition>(true);
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.Mimics.ToiletModern           ,Toilet.Repair.Definition,Definition>(true);
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.Mimics.ToiletRanch            ,Toilet.Repair.Definition,Definition>(true);
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.Mimics.ToiletRomantic         ,Toilet.Repair.Definition,Definition>(true);
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.Mimics.ToiletStall            ,Toilet.Repair.Definition,Definition>(true);
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.Toilet,Toilet.Repair.Definition,Definition>(true);
                                     sOldSingleton=Singleton;
                                                   Singleton=new Definition();
                                   }
        public new class Definition:Toilet.Repair.Definition{
            public override InteractionInstance CreateInstance(ref InteractionInstanceParameters parameters){
                            InteractionInstance na=new ToiletRepairFix();
                                                na.Init(ref parameters);
                                         return na;
            }
            public override bool Test(Sim a,Toilet target,bool isAutonomous,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
                                                if(target.Repairable==null)return false;
            return(target.Repairable.Broken);
            }
        }
    }
    public class AllInOneBathroomRepairAllInOneBathroomFix:AllInOneBathroom.RepairAllInOneBathroom,IPreLoad,IAddInteraction{
        static InteractionDefinition sOldSingleton;
                                              public void AddInteraction(InteractionInjectorList interactions){
                                                                                                 interactions.ReplaceNoTest<AllInOneBathroom,AllInOneBathroom.RepairAllInOneBathroom.Definition>(Singleton);
                                              }
                                   public void OnPreLoad(){
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.AllInOneBathroom,AllInOneBathroom.RepairAllInOneBathroom.Definition,Definition>(false);
                                     sOldSingleton=Singleton;
                                                   Singleton=new Definition();
                                   }
        public new class Definition:AllInOneBathroom.RepairAllInOneBathroom.Definition{
            public override InteractionInstance CreateInstance(ref InteractionInstanceParameters parameters){
                            InteractionInstance na=new AllInOneBathroomRepairAllInOneBathroomFix();
                                                na.Init(ref parameters);
                                         return na;
            }
            public override bool Test(Sim a,AllInOneBathroom target,bool isAutonomous,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
            return(target.Repairable.Broken);
            }
        }
    }
    public class HotTubBaseRepairHotTubFix:HotTubBase.RepairHotTub,IPreLoad,IAddInteraction{
        static InteractionDefinition sOldSingleton;
                                              public void AddInteraction(InteractionInjectorList interactions){
                                                                                                 interactions.ReplaceNoTest<HotTubBase,HotTubBase.RepairHotTub.Definition>(Singleton);
                                              }
                                   public void OnPreLoad(){
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.Mimics.HotTubGrottoLightRim       ,HotTubBase.RepairHotTub.Definition,Definition>(true);
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.Mimics.HotTubGrottoPatioModern    ,HotTubBase.RepairHotTub.Definition,Definition>(true);
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.Mimics.HotTubGrottoPatioRockGarden,HotTubBase.RepairHotTub.Definition,Definition>(true);
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.Mimics.HotTubGrottoSeasonElegant  ,HotTubBase.RepairHotTub.Definition,Definition>(true);
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.Mimics.HotTubGrottoSpanCol        ,HotTubBase.RepairHotTub.Definition,Definition>(true);
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.Mimics.HotTubGrottoTIGranite      ,HotTubBase.RepairHotTub.Definition,Definition>(true);
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.Mimics.HotTubPatioElite           ,HotTubBase.RepairHotTub.Definition,Definition>(true);
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.Mimics.HotTubSleekSopSimple       ,HotTubBase.RepairHotTub.Definition,Definition>(true);
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.HotTubGrottoCeleb                 ,HotTubBase.RepairHotTub.Definition,Definition>(true);
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.HotTubBase,HotTubBase.RepairHotTub.Definition,Definition>(true);
                                     sOldSingleton=Singleton;
                                                   Singleton=new Definition();
                                   }
        public new class Definition:HotTubBase.RepairHotTub.Definition{
            public override InteractionInstance CreateInstance(ref InteractionInstanceParameters parameters){
                            InteractionInstance na=new HotTubBaseRepairHotTubFix();
                                                na.Init(ref parameters);
                                         return na;
            }
            public override bool Test(Sim a,HotTubBase target,bool isAutonomous,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
            return(target.Repairable.Broken);
            }
        }
    }
    public class ShowerRepairShowerFix:Shower.RepairShower,IPreLoad,IAddInteraction{
        static InteractionDefinition sOldSingleton;
                                              public void AddInteraction(InteractionInjectorList interactions){
                                                                                                 interactions.Replace<Shower,Shower.RepairShower.Definition>(Singleton);
                                              }
                                   public void OnPreLoad(){
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.Mimics.ShowerBasic            ,Shower.RepairShower.Definition,Definition>(false);
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.Mimics.ShowerGen              ,Shower.RepairShower.Definition,Definition>(false);
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.Mimics.ShowerHETech           ,Shower.RepairShower.Definition,Definition>(false);
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.Mimics.ShowerLoft             ,Shower.RepairShower.Definition,Definition>(false);
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.Mimics.ShowerModern           ,Shower.RepairShower.Definition,Definition>(false);
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.Mimics.ShowerFuture           ,Shower.RepairShower.Definition,Definition>(false);
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.Mimics.ShowerRanch            ,Shower.RepairShower.Definition,Definition>(false);
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.Mimics.ShowerRomantic         ,Shower.RepairShower.Definition,Definition>(false);
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.ShowerCheap                   ,Shower.RepairShower.Definition,Definition>(false);
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.ShowerExpensive               ,Shower.RepairShower.Definition,Definition>(false);
                                     sOldSingleton=Singleton;
                                                   Singleton=new Definition();
                                   }
        public new class Definition:Shower.RepairShower.Definition{
            public override InteractionInstance CreateInstance(ref InteractionInstanceParameters parameters){
                            InteractionInstance na=new ShowerRepairShowerFix();
                                                na.Init(ref parameters);
                                         return na;
            }
            public override bool Test(Sim a,Shower target,bool isAutonomous,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
            return(target.Repairable.Broken);
            }
        }
    }
    public class TakeShowerEx:Shower.TakeShower,IPreLoad,IAddInteraction{
        static InteractionDefinition sOldSingleton;
                                              public void AddInteraction(InteractionInjectorList interactions){
                                                                                                 interactions.Replace<IShowerable,Shower.TakeShower.Definition>(Singleton);
                                              }
                                   public void OnPreLoad(){
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.Mimics.ShowerBasic            ,Shower.TakeShower.Definition,Definition>(false);
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.Mimics.ShowerGen              ,Shower.TakeShower.Definition,Definition>(false);
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.Mimics.ShowerHETech           ,Shower.TakeShower.Definition,Definition>(false);
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.Mimics.ShowerLoft             ,Shower.TakeShower.Definition,Definition>(false);
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.Mimics.ShowerModern           ,Shower.TakeShower.Definition,Definition>(false);
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.Mimics.BathtubShowerModern    ,Shower.TakeShower.Definition,Definition>(false);
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.Mimics.ShowerFuture           ,Shower.TakeShower.Definition,Definition>(false);
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.Mimics.ShowerRanch            ,Shower.TakeShower.Definition,Definition>(false);
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.Mimics.ShowerRomantic         ,Shower.TakeShower.Definition,Definition>(false);
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.Mimics.BathtubShowerSeasonChic,Shower.TakeShower.Definition,Definition>(false);
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.Mimics.BathtubShowerTILocal   ,Shower.TakeShower.Definition,Definition>(false);
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.ShowerCheap                   ,Shower.TakeShower.Definition,Definition>(false);
            Tunings.Inject<Sims3.Gameplay.Objects.Plumbing.ShowerExpensive               ,Shower.TakeShower.Definition,Definition>(false);
            Tunings.Inject<IShowerable,Shower.TakeShower.Definition,Definition>(false);
                                     sOldSingleton=Singleton;
                                                   Singleton=new Definition();
                                   }
        public static Sim.ClothesChangeReason GetOutfitReason(Sim actor){
                                                               if(actor.CareerManager!=null){
                                                              if((actor.Occupation!=null)&&
                                                                 (actor.CareerManager.CareerHoursTillWork<Sim.kClothingChangeHoursBeforeWork)){
                                                               if(actor.Occupation.HasOpenHours){
               return Sim.ClothesChangeReason.GoingOutside;
                                                               }
                                                              }
                                                               }
               return Sim.ClothesChangeReason.GettingOutOfBath;
        }
        public override bool Run(){
            try{
                if(!Target.SimLine.WaitForTurn(this,SimQueue.WaitBehavior.DefaultEvict,
                    ~(ExitReason.Replan               |
                      ExitReason.MidRoutePushRequested|
                      ExitReason.ObjectStateChanged   |
                      ExitReason.PlayIdle             |
                      ExitReason.MaxSkillPointsReached),Shower.kTimeToWaitToEvict)){
                    return(false);
                }
                try{
                    mSwitchOutfitHelper=new Sim.SwitchOutfitHelper(Actor,Sim.ClothesChangeReason.GoingToBathe);
                }catch{
                    return(false);
                }
                    mSwitchOutfitHelper.Start();
                if(Actor.HasTrait(TraitNames.Hydrophobic)){
                   Actor.PlayReaction(ReactionTypes.WhyMe,Target as GameObject,ReactionSpeed.ImmediateWithoutOverlay);
                }
                if(Actor.HasTrait(TraitNames.Daredevil  )){
                    TraitTipsManager.ShowTraitTip(0xb82d0015b9294260L,Actor,TraitTipsManager.TraitTipCounterIndex.Daredevil,TraitTipsManager.kDaredevilCountOfShowersTaken);
                }
               if(!Actor.RouteToSlotAndCheckInUse(Target,Slot.RoutingSlot_0)){
                    return(false);
               }
                //    mSituation=new Shower.ShowerPrivacySituation(this);
                //if(!mSituation.Start()){
                //        return(false);
                //}
                StandardEntry();
               if(!Actor.RouteToSlot(Target,Slot.RoutingSlot_0)){
                 if(mSituation!=null){
                    mSituation.Exit();
                 }
                StandardExit ();
                    return(false);
               }
                if(Autonomous){
                    mPriority=new InteractionPriority(InteractionPriorityLevel.UserDirected);
                }
                    mSwitchOutfitHelper.Wait(true);
bool daredevilPerforming=
                   Actor.DaredevilPerforming;
        bool flag2=Actor.GetCurrentOutfitCategoryFromOutfitInGameObject()==OutfitCategories.Singed;
                EnterStateMachine("Shower","Enter","x");
                SetActor("Shower",Target);
               if(mSituation!=null){
                  mSituation.StateMachine=mCurrentStateMachine;
               }
                SetParameter("IsShowerTub",Target.IsShowerTub);
                SetParameter("SimShouldCloseDoor",true);
                SetParameter("SimShouldClothesChange",((!daredevilPerforming&&!flag2)&&!Actor.OccultManager.DisallowClothesChange())&&!Actor.BuffManager.DisallowClothesChange());
                bool paramValue=false;
                if((Target.BoobyTrapComponent!=null)&&Target.BoobyTrapComponent.CanTriggerTrap(Actor.SimDescription)){
                     paramValue=!Actor.OccultManager.DisallowClothesChange()&&!Actor.BuffManager.DisallowClothesChange();
                }
                SimDescription description = ((Target.BoobyTrapComponent != null) && (Target.BoobyTrapComponent.TrapSetter != 0L)) ? SimDescription.Find(Target.BoobyTrapComponent.TrapSetter) : null;
                if(((description!=null)&&description.IsFairy)&&Actor.BuffManager.HasElement(BuffNames.TrickedByAFairy)){
                     paramValue=false;
                }
                SetParameter("isBoobyTrapped",paramValue);
                    mSwitchOutfitHelper.AddScriptEventHandler(this);
                AddOneShotScriptEventHandler(0x3e9,EventCallbackStartShoweringSound);
                if(Actor.HasTrait(TraitNames.Virtuoso)||RandomUtil.RandomChance((float)Target.TuningShower.ChanceOfSinging)){
                AddOneShotScriptEventHandler(0xc8 ,EventCallbackStartSinging);
                }
                PetStartleBehavior.CheckForStartle(Target as GameObject,StartleType.ShowerOn);
                AnimateSim("Loop Shower");
                   Actor.BuffManager.AddElement(BuffNames.SavingWater,Origin.FromShower,ProductVersion.EP2,TraitNames.EnvironmentallyConscious);
                    mShowerStage.ResetCompletionTime(GetShowerTime());
                StartStages();
                if(Actor.HasTrait(TraitNames.EnvironmentallyConscious)){
                BeginCommodityUpdate(CommodityKind.Hygiene,Shower.kEnvironmentallyConsciousShowerSpeedMultiplier);
                }
                if(Actor.SimDescription.IsPlantSim){
                ModifyCommodityUpdate(CommodityKind.Hygiene,Shower.kPlantSimHygieneModifier);
                }
                BeginCommodityUpdates();
                  if(paramValue){
                ApplyBoobyTrapOutfit();
                if((description!=null)&&description.IsFairy){
                   Actor.BuffManager.AddElement(BuffNames.TrickedByAFairy,Origin.FromFairy);
                }
                  }
                               bool succeeded=(false);
                try{
                    try{
Target.SimInShower=Actor;
                                    succeeded=DoLoop(~(
                                        ExitReason.Replan               |
                                        ExitReason.MidRoutePushRequested|
                                        ExitReason.ObjectStateChanged   |
                                        ExitReason.PlayIdle             |
                                        ExitReason.MaxSkillPointsReached),DuringShower,null);
  if(HavingWooHoo&&Actor.HasExitReason(ExitReason.StageComplete)){
                                    succeeded=DoLoop(~(
                                        ExitReason.Replan               | 
                                        ExitReason.MidRoutePushRequested| 
                                        ExitReason.ObjectStateChanged   | 
                                        ExitReason.PlayIdle             | 
                                        ExitReason.MaxSkillPointsReached| 
                                        ExitReason.StageComplete        ),DuringShower,null);
  }
                    }finally{
Target.SimInShower=null;
                    }
                    while(HavingWooHoo){
                        SpeedTrap.Sleep(10);
                    }
                }finally{
                EndCommodityUpdates(succeeded);
                }
                Shower.WaitToLeaveShower(Actor,Target);
                                 if(succeeded){
                Shower.ApplyPostShowerEffects(Actor,Target);
                                 }
                  if(paramValue){
                SetParameter("isBoobyTrapped",false);
                AddOneShotScriptEventHandler(0xc9 ,EventCallbackStopSinging       );
                AddOneShotScriptEventHandler(0x3ea,EventCallbackStopShoweringSound);
                    if((description!=null)&&description.IsFairy){
                AnimateSim("TriggerFairyTrap");
                    }else{
                AnimateSim("Booby Trap Reaction");
                    }
                AddOneShotScriptEventHandler(0x3e9,EventCallbackStartShoweringSound);
                        AnimateSim("Loop Shower");
                RemoveBoobyTrapOutfit();
                        SpeedTrap.Sleep(60);
                  }
                try{
                          if(flag2&&succeeded){
                    mSwitchOutfitHelper.Dispose();
                        try{
                    mSwitchOutfitHelper=new Sim.SwitchOutfitHelper(Actor,Sim.ClothesChangeReason.GoingToBathe);
                    mSwitchOutfitHelper.Start();
                    mSwitchOutfitHelper.Wait(false);
                    mSwitchOutfitHelper.ChangeOutfit();
                        }catch{}
                          }
                        bool flag5=(false);
                         if((flag2&&succeeded)||(!flag2&&!daredevilPerforming)){
                SetParameter("SimShouldClothesChange",!Actor.OccultManager.DisallowClothesChange());
                    mSwitchOutfitHelper.Dispose();
                        try{
                    mSwitchOutfitHelper=new Sim.SwitchOutfitHelper(Actor,GetOutfitReason(Actor));
                    mSwitchOutfitHelper.Start();
                    mSwitchOutfitHelper.AddScriptEventHandler(this);
                    mSwitchOutfitHelper.Wait(false);
                        }catch{}
                             flag5=( true);
                    }
                       Target.Cleanable.DirtyInc(Actor);
                AddOneShotScriptEventHandler(0xc9 ,EventCallbackStopSinging       );
                AddOneShotScriptEventHandler(0x3ea,EventCallbackStopShoweringSound);
                          if(flag5&&InventingSkill.IsBeingDetonated(Target as GameObject)){
                SetParameter("SimShouldClothesChange",false);
                    mSwitchOutfitHelper.Abort();
                    mSwitchOutfitHelper.Dispose();
                          }
                    if(Target.Repairable.UpdateBreakage(Actor)){
                       Target.StartBrokenFXInAnim(mCurrentStateMachine);
                AnimateSim("Exit Broken");
                    }else{
                AnimateSim("Exit Working");
                    }
               if((Actor.SimDescription.IsMummy||Actor.DaredevilPerforming)||(Actor.TraitManager.HasElement(TraitNames.Slob)&&RandomUtil.RandomChance01(TraitTuning.SlobTraitChanceToLeavePuddle))){
                    PuddleManager.AddPuddle(Actor.Position);
               }
                                 if(succeeded){
                   Actor.BuffManager.RemoveElement(BuffNames.GotFleasHuman);
                                 }
                }finally{
                StandardExit();
                }
                return succeeded;
            }catch(ResetException exception){
                   Alive.WriteLog(exception.Message+"\n\n"+
                                  exception.StackTrace+"\n\n"+
                                  exception.Source);
                return(false);
            }catch(     Exception exception){
                   Alive.WriteLog(exception.Message+"\n\n"+
                                  exception.StackTrace+"\n\n"+
                                  exception.Source);
                return(false);
            }
        }
        public new class Definition:Shower.TakeShower.Definition{
            public override InteractionInstance CreateInstance(ref InteractionInstanceParameters parameters){
                            InteractionInstance na=new TakeShowerEx();
                                                na.Init(ref parameters);
                                         return na;
            }
            public override string GetInteractionName(Sim actor,IShowerable target,InteractionObjectPair iop){
                       return base.GetInteractionName(actor,target,new InteractionObjectPair(sOldSingleton,target));
            }
            public override bool Test(Sim a,IShowerable target,bool isAutonomous,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
                                                     if(target.Repairable==null)return false;
                     return base.Test(a,target,isAutonomous,ref greyedOutTooltipCallback);
            }
        }
    }
}