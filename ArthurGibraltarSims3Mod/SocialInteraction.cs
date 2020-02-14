using Sims3.Gameplay;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.ActiveCareer.ActiveCareers;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.ActorSystems.Children;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Careers;
using Sims3.Gameplay.CAS;
using Sims3.Gameplay.CelebritySystem;
using Sims3.Gameplay.Controllers;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.EventSystem;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Interfaces;
using Sims3.Gameplay.Objects;
using Sims3.Gameplay.Objects.Beds;
using Sims3.Gameplay.Objects.Miscellaneous;
using Sims3.Gameplay.Pools;
using Sims3.Gameplay.Scuba;
using Sims3.Gameplay.Seasons;
using Sims3.Gameplay.Services;
using Sims3.Gameplay.Skills;
using Sims3.Gameplay.Socializing;
using Sims3.Gameplay.UI;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.SimIFace.CAS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using static ArthurGibraltarSims3Mod.Alive;
using static ArthurGibraltarSims3Mod.Interaction;
namespace ArthurGibraltarSims3Mod{
    //  TO DO: LetChildOut GetSelfPickedUp FeedOnFloor TeachToTalk TeachToWalk Tickle FeedToddlerInHighChair (InteractionDefinition) ToddlerHug (InteractionDefinition) PlayPeekAboo Crib.PutChildOnLotInCrib (InteractionDefinition) HighChairBase.PutCarriedChildInChair HighChairBase.PutChildOnLotInChair HighChairBase.ServeFood HighChairBase.GiveBabyFood HighChairBase.GiveBottle HighChairBase.Hold Crib_ReadToSleep
    public class SnuggleFix:Snuggle,IPreLoad,IAddInteraction{//  ToddlerOrBelow
        static InteractionDefinition sOldSingleton;
                                              public void AddInteraction(InteractionInjectorList interactions){
                                                                                                 interactions.ReplaceNoTest<Sim,Snuggle.Definition>(Singleton);
                                              }
                                   public void OnPreLoad(){
            Tunings.Inject<Sim,Snuggle.Definition,Definition>(false);
                                     sOldSingleton=Singleton;
                                                   Singleton=new Definition();
                                   }
        public override bool Run(){
            if(!CarriedChildInteractionBFix.StartInteractionWithCarriedChild1((SocialInteraction)this,"BeSnuggled"))
        return false;
this.BeginCommodityUpdates();
            this.StartSocial("Baby Toddler Snuggle");
            this.Actor.CarryingChildPosture.AnimateInteractionWithCarriedChild(nameof(Snuggle));
            EventTracker.SendEvent(EventTypeId.kBabyToddlerSnuggle,(IActor)this.Actor,(IGameObject)this.Target);
            this.FinishSocialAndLeaveConversation("Baby Toddler Snuggle");
this.EndCommodityUpdates(true);
        return ChildUtils.FinishInteractionWithCarriedChild((SocialInteraction)this);
        }
        public new class Definition:Snuggle.Definition{
            public override InteractionInstance CreateInstance(ref InteractionInstanceParameters parameters){
                            InteractionInstance na=new SnuggleFix();
                                                na.Init(ref parameters);
                                         return na;
            }
            public override InteractionTestResult Test(ref InteractionInstanceParameters parameters,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
                Sim  actor1=parameters. Actor as Sim;
                Sim target1=parameters.Target as Sim;
                if((object) actor1==null)
            return InteractionTestResult.Root_Null_Actor;
                if((object)target1==null)
            return InteractionTestResult.Root_Null_Target;
                Sim actor2=(object)actor1 as Sim;
                IGameObject target2=(IGameObject)target1;
                for(IGameObject gameObject=(IGameObject)target1;gameObject!=null;gameObject=gameObject.Parent){
                    if(gameObject==LiveDragHelperModel.CachedTopDraggedObject)
            return InteractionTestResult.Root_TargetOnHandTool;
                }
                bool flag=true;
                InteractionTuning tuning=parameters.InteractionObjectPair.Tuning;
                Tradeoff mTradeoff = tuning?.mTradeoff;
                if(tuning!=null){
                    CommodityKind workMotive=actor1.WorkMotive;
                    if(workMotive!=CommodityKind.None&&mTradeoff.SatisfiesCommodity(workMotive))
                     flag=false;
                    if(parameters.Autonomous){
                        if(tuning.HasFlags(InteractionTuning.FlagField.DisallowAutonomous)&&flag)
            return InteractionTestResult.Tuning_DisallowAutonomous;
                        if(mTradeoff.FunExit&&actor1.Motives.FunInteractionTest(false))
            return InteractionTestResult.Tuning_FunInteractionTest;
                    }else if(tuning.HasFlags(InteractionTuning.FlagField.DisallowUserDirected))
            return InteractionTestResult.Tuning_DisallowUserDirected;
                    if(actor1.IsSelectable&&tuning.HasFlags(InteractionTuning.FlagField.DisallowPlayerSim))
            return InteractionTestResult.Tuning_DisallowPlayerSim;
                  if(flag){
                                                    AutonomyFix fix=new AutonomyFix(actor1.Autonomy.mActor,actor1.Autonomy.Motives,actor1.Autonomy.CurrentSearchType,actor1.Autonomy.IsActorInTombRoom);
                    InteractionTestResult interactionTestResult=fix.CheckAvailability1(parameters.Autonomous,tuning.Availability,parameters.InteractionObjectPair);
                    if(interactionTestResult!=InteractionTestResult.Pass)
            return interactionTestResult;
                  }
                }
                actor1.Autonomy.UpdateCacheIfNeeded((IGameObject)target1);
               var interactionTestResult1=AutonomyFix.CommonTests1((InteractionDefinition)this,actor2,target2,parameters);
                if(interactionTestResult1!=InteractionTestResult.Pass)
            return interactionTestResult1;
               var interactionTestResult2=!(this is IMetaInteractionDefinition)?    InteractionDefinitionUtilities.SpecialCaseTests((InteractionDefinition)this,actor2,target2,parameters) 
                                                                               :MetaInteractionDefinitionUtilities.SpecialCaseTests(actor2,target2,parameters);
                if(interactionTestResult2!=InteractionTestResult.Pass)
            return interactionTestResult2;
                if(!this.Test(actor1,target1,parameters.Autonomous,ref greyedOutTooltipCallback))
            return InteractionTestResult.Def_TestFailed;
                if(tuning!=null){
 InteractionTestResult interactionTestResult3=actor1.Autonomy.CheckAvailabilityTooltip((InteractionDefinition)this,(IGameObject)target1,tuning.Availability,parameters,mTradeoff,ref greyedOutTooltipCallback);
                    if(interactionTestResult3!=InteractionTestResult.Pass)
                return interactionTestResult3;
                }
 InteractionTestResult interactionTestResult4=InteractionDefinitionUtilities.SpecialCaseTooltipTests((InteractionDefinition)this,actor2,target2,parameters,ref greyedOutTooltipCallback);
                return interactionTestResult4!=InteractionTestResult.Pass?interactionTestResult4:InteractionTestResult.Pass;
            }
        }
    }
    public class TossInAirFix:TossInAir,IPreLoad,IAddInteraction{//  Toddler only
        static InteractionDefinition sOldSingleton;
                                              public void AddInteraction(InteractionInjectorList interactions){
                                                                                                 interactions.ReplaceNoTest<Sim,TossInAir.Definition>(Singleton);
                                              }
                                   public void OnPreLoad(){
            Tunings.Inject<Sim,TossInAir.Definition,Definition>(false);
                                     sOldSingleton=Singleton;
                                                   Singleton=new Definition();
                                   }
        public override bool Run(){
            if(!CarriedChildInteractionBFix.StartInteractionWithCarriedChild1((SocialInteraction)this,"BeTossedInAir"))
        return false;
this.BeginCommodityUpdates();
            this.StartSocial("Toddler Toss In Air");
            this.Actor.CarryingChildPosture.AnimateInteractionWithCarriedChild(nameof(TossInAir));
            EventTracker.SendEvent(EventTypeId.kToddlerTossInAir,(IActor)this.Actor,(IGameObject)this.Target);
            this.FinishSocialAndLeaveConversation("Toddler Toss In Air");
            this.Target.Motives.ChangeValue(CommodityKind.Fun,TossInAir.ToddlerFunGain);
this.EndCommodityUpdates(true);
        return ChildUtils.FinishInteractionWithCarriedChild((SocialInteraction)this);
        }
        public new class Definition:TossInAir.Definition{
            public override InteractionInstance CreateInstance(ref InteractionInstanceParameters parameters){
                            InteractionInstance na=new TossInAirFix();
                                                na.Init(ref parameters);
                                         return na;
            }
            public override InteractionTestResult Test(ref InteractionInstanceParameters parameters,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
                Sim  actor1=parameters. Actor as Sim;
                Sim target1=parameters.Target as Sim;
                if((object) actor1==null)
            return InteractionTestResult.Root_Null_Actor;
                if((object)target1==null)
            return InteractionTestResult.Root_Null_Target;
                Sim actor2=(object)actor1 as Sim;
                IGameObject target2=(IGameObject)target1;
                for(IGameObject gameObject=(IGameObject)target1;gameObject!=null;gameObject=gameObject.Parent){
                    if(gameObject==LiveDragHelperModel.CachedTopDraggedObject)
            return InteractionTestResult.Root_TargetOnHandTool;
                }
                bool flag=true;
                InteractionTuning tuning=parameters.InteractionObjectPair.Tuning;
                Tradeoff mTradeoff = tuning?.mTradeoff;
                if(tuning!=null){
                    CommodityKind workMotive=actor1.WorkMotive;
                    if(workMotive!=CommodityKind.None&&mTradeoff.SatisfiesCommodity(workMotive))
                     flag=false;
                    if(parameters.Autonomous){
                        if(tuning.HasFlags(InteractionTuning.FlagField.DisallowAutonomous)&&flag)
            return InteractionTestResult.Tuning_DisallowAutonomous;
                        if(mTradeoff.FunExit&&actor1.Motives.FunInteractionTest(false))
            return InteractionTestResult.Tuning_FunInteractionTest;
                    }else if(tuning.HasFlags(InteractionTuning.FlagField.DisallowUserDirected))
            return InteractionTestResult.Tuning_DisallowUserDirected;
                    if(actor1.IsSelectable&&tuning.HasFlags(InteractionTuning.FlagField.DisallowPlayerSim))
            return InteractionTestResult.Tuning_DisallowPlayerSim;
                  if(flag){
                                                    AutonomyFix fix=new AutonomyFix(actor1.Autonomy.mActor,actor1.Autonomy.Motives,actor1.Autonomy.CurrentSearchType,actor1.Autonomy.IsActorInTombRoom);
                    InteractionTestResult interactionTestResult=fix.CheckAvailability1(parameters.Autonomous,tuning.Availability,parameters.InteractionObjectPair);
                    if(interactionTestResult!=InteractionTestResult.Pass)
            return interactionTestResult;
                  }
                }
                actor1.Autonomy.UpdateCacheIfNeeded((IGameObject)target1);
               var interactionTestResult1=AutonomyFix.CommonTests1((InteractionDefinition)this,actor2,target2,parameters);
                if(interactionTestResult1!=InteractionTestResult.Pass)
            return interactionTestResult1;
               var interactionTestResult2=!(this is IMetaInteractionDefinition)?    InteractionDefinitionUtilities.SpecialCaseTests((InteractionDefinition)this,actor2,target2,parameters) 
                                                                               :MetaInteractionDefinitionUtilities.SpecialCaseTests(actor2,target2,parameters);
                if(interactionTestResult2!=InteractionTestResult.Pass)
            return interactionTestResult2;
                if(!this.Test(actor1,target1,parameters.Autonomous,ref greyedOutTooltipCallback))
            return InteractionTestResult.Def_TestFailed;
                if(tuning!=null){
 InteractionTestResult interactionTestResult3=actor1.Autonomy.CheckAvailabilityTooltip((InteractionDefinition)this,(IGameObject)target1,tuning.Availability,parameters,mTradeoff,ref greyedOutTooltipCallback);
                    if(interactionTestResult3!=InteractionTestResult.Pass)
                return interactionTestResult3;
                }
 InteractionTestResult interactionTestResult4=InteractionDefinitionUtilities.SpecialCaseTooltipTests((InteractionDefinition)this,actor2,target2,parameters,ref greyedOutTooltipCallback);
                return interactionTestResult4!=InteractionTestResult.Pass?interactionTestResult4:InteractionTestResult.Pass;
            }
        }
    }
    public class PlayWithFix:PlayWith,IPreLoad,IAddInteraction{//  Baby only
        static InteractionDefinition sOldSingleton;
                                              public void AddInteraction(InteractionInjectorList interactions){
                                                                                                 interactions.ReplaceNoTest<Sim,PlayWith.Definition>(Singleton);
                                              }
                                   public void OnPreLoad(){
            Tunings.Inject<Sim,PlayWith.Definition,Definition>(false);
                                     sOldSingleton=Singleton;
                                                   Singleton=new Definition();
                                   }
        public override bool Run(){
            if(!CarriedChildInteractionBFix.StartInteractionWithCarriedChild1((SocialInteraction)this,"BeTossedInAir"))
        return false;
this.BeginCommodityUpdates();
            this.StartSocial("Baby Play With");
            this.Actor.CarryingChildPosture.AnimateInteractionWithCarriedChild(nameof(PlayWith));
            this.FinishSocialAndLeaveConversation("Baby Play With");
            this.Target.Motives.ChangeValue(CommodityKind.Fun,PlayWith.ToddlerFunGain);
this.EndCommodityUpdates(true);
        return ChildUtils.FinishInteractionWithCarriedChild((SocialInteraction)this);
        }
        public new class Definition:PlayWith.Definition{
            public override InteractionInstance CreateInstance(ref InteractionInstanceParameters parameters){
                            InteractionInstance na=new PlayWithFix();
                                                na.Init(ref parameters);
                                         return na;
            }
            public override InteractionTestResult Test(ref InteractionInstanceParameters parameters,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
            return !PickUpChild.Definition.CanPickUpBabyOrToddler(ref parameters)?InteractionTestResult.GenericFail:TestFix(ref parameters,ref greyedOutTooltipCallback);
            }
            public InteractionTestResult TestFix(ref InteractionInstanceParameters parameters,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
                Sim  actor1=parameters. Actor as Sim;
                Sim target1=parameters.Target as Sim;
                if((object) actor1==null)
            return InteractionTestResult.Root_Null_Actor;
                if((object)target1==null)
            return InteractionTestResult.Root_Null_Target;
                Sim actor2=(object)actor1 as Sim;
                IGameObject target2=(IGameObject)target1;
                for(IGameObject gameObject=(IGameObject)target1;gameObject!=null;gameObject=gameObject.Parent){
                    if(gameObject==LiveDragHelperModel.CachedTopDraggedObject)
            return InteractionTestResult.Root_TargetOnHandTool;
                }
                bool flag=true;
                InteractionTuning tuning=parameters.InteractionObjectPair.Tuning;
                Tradeoff mTradeoff = tuning?.mTradeoff;
                if(tuning!=null){
                    CommodityKind workMotive=actor1.WorkMotive;
                    if(workMotive!=CommodityKind.None&&mTradeoff.SatisfiesCommodity(workMotive))
                     flag=false;
                    if(parameters.Autonomous){
                        if(tuning.HasFlags(InteractionTuning.FlagField.DisallowAutonomous)&&flag)
            return InteractionTestResult.Tuning_DisallowAutonomous;
                        if(mTradeoff.FunExit&&actor1.Motives.FunInteractionTest(false))
            return InteractionTestResult.Tuning_FunInteractionTest;
                    }else if(tuning.HasFlags(InteractionTuning.FlagField.DisallowUserDirected))
            return InteractionTestResult.Tuning_DisallowUserDirected;
                    if(actor1.IsSelectable&&tuning.HasFlags(InteractionTuning.FlagField.DisallowPlayerSim))
            return InteractionTestResult.Tuning_DisallowPlayerSim;
                  if(flag){
                                                    AutonomyFix fix=new AutonomyFix(actor1.Autonomy.mActor,actor1.Autonomy.Motives,actor1.Autonomy.CurrentSearchType,actor1.Autonomy.IsActorInTombRoom);
                    InteractionTestResult interactionTestResult=fix.CheckAvailability1(parameters.Autonomous,tuning.Availability,parameters.InteractionObjectPair);
                    if(interactionTestResult!=InteractionTestResult.Pass)
            return interactionTestResult;
                  }
                }
                actor1.Autonomy.UpdateCacheIfNeeded((IGameObject)target1);
               var interactionTestResult1=AutonomyFix.CommonTests1((InteractionDefinition)this,actor2,target2,parameters);
                if(interactionTestResult1!=InteractionTestResult.Pass)
            return interactionTestResult1;
               var interactionTestResult2=!(this is IMetaInteractionDefinition)?    InteractionDefinitionUtilities.SpecialCaseTests((InteractionDefinition)this,actor2,target2,parameters) 
                                                                               :MetaInteractionDefinitionUtilities.SpecialCaseTests(actor2,target2,parameters);
                if(interactionTestResult2!=InteractionTestResult.Pass)
            return interactionTestResult2;
                if(!this.Test(actor1,target1,parameters.Autonomous,ref greyedOutTooltipCallback))
            return InteractionTestResult.Def_TestFailed;
                if(tuning!=null){
 InteractionTestResult interactionTestResult3=actor1.Autonomy.CheckAvailabilityTooltip((InteractionDefinition)this,(IGameObject)target1,tuning.Availability,parameters,mTradeoff,ref greyedOutTooltipCallback);
                    if(interactionTestResult3!=InteractionTestResult.Pass)
                return interactionTestResult3;
                }
 InteractionTestResult interactionTestResult4=InteractionDefinitionUtilities.SpecialCaseTooltipTests((InteractionDefinition)this,actor2,target2,parameters,ref greyedOutTooltipCallback);
                return interactionTestResult4!=InteractionTestResult.Pass?interactionTestResult4:InteractionTestResult.Pass;
            }
        }
    }
    public class FeedOnFloorFix:FeedOnFloor,IPreLoad,IAddInteraction{
        static InteractionDefinition sOldSingleton;
                                              public void AddInteraction(InteractionInjectorList interactions){
                                                                                                 interactions.Replace<Sim,FeedOnFloor.Definition>(Singleton);
                                              }
                                   public void OnPreLoad(){
            Tunings.Inject<Sim,FeedOnFloor.Definition,Definition>(false);
                                     sOldSingleton=Singleton;
                                                   Singleton=new Definition();
                                   }
        public override bool Run(){
            switch(StartInteractionWithChildOnFloor1((SocialInteraction)this,"TakeBottle")){
                case ChildUtils.Return.False:
        return false;
                case ChildUtils.Return.True:
        return true;
                default:
                    this.mCurrentStateMachine=StateMachineClient.Acquire((IHasScriptProxy)this.Target,"GiveBottle");
                    this.SetActorAndEnter("x",(IHasScriptProxy)this.Actor,"Enter");
                    this.SetActorAndEnter("y",(IHasScriptProxy)this.Target,"Enter");
this.BeginCommodityUpdates();
                    this.AnimateJoinSims("GiveBottle");
                    InteractionPriority priority=this.GetPriority();
                    if(priority.Level<InteractionPriorityLevel.NonCriticalNPCBehavior){
                        MotiveTuning motiveTuning=this.Target.GetMotiveTuning(CommodityKind.Hunger);
                        priority=new InteractionPriority(InteractionPriorityLevel.NonCriticalNPCBehavior,ScoringFunctionsForBabiesAndToddlers.HungerIntensity*(motiveTuning!=null?motiveTuning.Max:1f));
                    }
                    this.Actor.CheckAndAddDislikesChildrenBuff(this.Target);
                    DrinkBottle instance=DrinkBottle.Singleton.CreateInstance((IGameObject)this.Target,(IActor)this.Target,priority,false,true)as DrinkBottle;
                                instance.mCurrentStateMachine=this.mCurrentStateMachine;
                    this.Target.InteractionQueue.PushAsContinuation((InteractionInstance)instance,true);
                    this.mCurrentStateMachine.RemoveActor("x");
this.EndCommodityUpdates(true);
                    if(!(this.Actor.SimDescription!=null&&this.Actor.SimDescription.IsBonehilda))
                        ChildUtils.UpdateRelationshipWithChild(this.Actor,this.Target,FeedOnFloor.kLTRLikingDelta);
            return ChildUtils.FinishInteractionWithChildOnFloor((SocialInteraction)this);
            }
        }
        public new class Definition:FeedOnFloor.Definition{
            public override InteractionInstance CreateInstance(ref InteractionInstanceParameters parameters){
                            InteractionInstance na=new FeedOnFloorFix();
                                                na.Init(ref parameters);
                                         return na;
            }
            public override bool Test(Sim a,Sim target,bool isAutonomous,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
            return ChildUtils.TestInteractionWithChildOnFloor(a,target,isAutonomous)&&FeedOnFloor.Definition.Test(target,isAutonomous,true);
            }
            public override InteractionTestResult Test(ref InteractionInstanceParameters parameters,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
                Sim  actor1=parameters. Actor as Sim;
                Sim target1=parameters.Target as Sim;
                if((object) actor1==null)
            return InteractionTestResult.Root_Null_Actor;
                if((object)target1==null)
            return InteractionTestResult.Root_Null_Target;
                Sim actor2=(object)actor1 as Sim;
                IGameObject target2=(IGameObject)target1;
                for(IGameObject gameObject=(IGameObject)target1;gameObject!=null;gameObject=gameObject.Parent){
                    if(gameObject==LiveDragHelperModel.CachedTopDraggedObject)
            return InteractionTestResult.Root_TargetOnHandTool;
                }
                bool flag=true;
                InteractionTuning tuning=parameters.InteractionObjectPair.Tuning;
                Tradeoff mTradeoff = tuning?.mTradeoff;
                if(tuning!=null){
                    CommodityKind workMotive=actor1.WorkMotive;
                    if(workMotive!=CommodityKind.None&&mTradeoff.SatisfiesCommodity(workMotive))
                     flag=false;
                    if(parameters.Autonomous){
                        if(tuning.HasFlags(InteractionTuning.FlagField.DisallowAutonomous)&&flag)
            return InteractionTestResult.Tuning_DisallowAutonomous;
                        if(mTradeoff.FunExit&&actor1.Motives.FunInteractionTest(false))
            return InteractionTestResult.Tuning_FunInteractionTest;
                    }else if(tuning.HasFlags(InteractionTuning.FlagField.DisallowUserDirected))
            return InteractionTestResult.Tuning_DisallowUserDirected;
                    if(actor1.IsSelectable&&tuning.HasFlags(InteractionTuning.FlagField.DisallowPlayerSim))
            return InteractionTestResult.Tuning_DisallowPlayerSim;
                  if(flag){
                                                    AutonomyFix fix=new AutonomyFix(actor1.Autonomy.mActor,actor1.Autonomy.Motives,actor1.Autonomy.CurrentSearchType,actor1.Autonomy.IsActorInTombRoom);
                    InteractionTestResult interactionTestResult=fix.CheckAvailability1(parameters.Autonomous,tuning.Availability,parameters.InteractionObjectPair);
                    if(interactionTestResult!=InteractionTestResult.Pass)
            return interactionTestResult;
                  }
                }
                actor1.Autonomy.UpdateCacheIfNeeded((IGameObject)target1);
               var interactionTestResult1=AutonomyFix.CommonTests1((InteractionDefinition)this,actor2,target2,parameters);
                if(interactionTestResult1!=InteractionTestResult.Pass)
            return interactionTestResult1;
               var interactionTestResult2=!(this is IMetaInteractionDefinition)?    InteractionDefinitionUtilities.SpecialCaseTests((InteractionDefinition)this,actor2,target2,parameters) 
                                                                               :MetaInteractionDefinitionUtilities.SpecialCaseTests(actor2,target2,parameters);
                if(interactionTestResult2!=InteractionTestResult.Pass)
            return interactionTestResult2;
                if(!this.Test(actor1,target1,parameters.Autonomous,ref greyedOutTooltipCallback))
            return InteractionTestResult.Def_TestFailed;
                if(tuning!=null){
 InteractionTestResult interactionTestResult3=actor1.Autonomy.CheckAvailabilityTooltip((InteractionDefinition)this,(IGameObject)target1,tuning.Availability,parameters,mTradeoff,ref greyedOutTooltipCallback);
                    if(interactionTestResult3!=InteractionTestResult.Pass)
                return interactionTestResult3;
                }
 InteractionTestResult interactionTestResult4=InteractionDefinitionUtilities.SpecialCaseTooltipTests((InteractionDefinition)this,actor2,target2,parameters,ref greyedOutTooltipCallback);
                return interactionTestResult4!=InteractionTestResult.Pass?interactionTestResult4:InteractionTestResult.Pass;
            }
        }
        public static ChildUtils.Return StartInteractionWithChildOnFloor1(SocialInteraction interactionA,string receptiveInteractionNameKey){
            if(interactionA.Target.Posture.Container==interactionA.Actor){
                if(!CarriedChildInteractionBFix.StartInteractionWithCarriedChild1(interactionA,receptiveInteractionNameKey))
        return ChildUtils.Return.False;
                if(interactionA.SocialJig==null)
                   interactionA.SocialJig=(SocialJig)(GlobalFunctions.CreateObjectOutOfWorld("SocialJigBabyToddler")as SocialJigTwoPerson);
                                   ChildUtils.PutDownClass putDownClass=new ChildUtils.PutDownClass(interactionA.Actor,interactionA.Target,ChildUtils.Where.ClosestSpot,Vector3.Zero,(InteractionDefinition)null,interactionA.SocialJig as SocialJigTwoPerson);
               var fix=new PutDownChildFix.PutDownClassFix(putDownClass);
                if(fix.PutDownChild1())
        return ChildUtils.Return.Continue;
                   interactionA.FinishLinkedInteraction();
                   interactionA.WaitForSyncComplete();
        return ChildUtils.Return.False;
            }
            if(!interactionA.Target.Posture.Satisfies(CommodityKind.Standing,(IGameObject)null)){
                if(!interactionA.Actor.SimDescription.TeenOrAbove){
        return ChildUtils.Return.False;
                }
                ChildUtils.ReenqueueWithCarryingChildPrecondition((InteractionInstance)interactionA);
        return ChildUtils.Return.True;
            }
            var fix1=new SocialInteractionFix(interactionA);
        return !fix1.BeginSocialInteraction1((InteractionDefinition)new SocialInteractionBFix.NoAgeCheckDefinition(ChildUtils.Localize(interactionA.Target.IsFemale,receptiveInteractionNameKey),false),true,false)?ChildUtils.Return.False:ChildUtils.Return.Continue;
        }
        public class SocialInteractionFix{
              public SocialInteractionFix(SocialInteraction interaction){
                                           this.interaction=interaction;
              }
                       public SocialInteraction interaction;
            public bool BeginSocialInteraction1(InteractionDefinition socialToPushToTarget,bool pairedSocial,bool doCallOver){
            return this.BeginSocialInteraction1(socialToPushToTarget,pairedSocial,interaction.Target.GetSocialRadiusWith(interaction.Actor),doCallOver);
            }
            public bool BeginSocialInteraction1(InteractionDefinition socialToPushToTarget,bool pairedSocial,float separationDist,bool doCallOver){
                interaction.mSocialDist=separationDist;
                if(doCallOver&&interaction.Actor.IsActiveSim&&(!interaction.Autonomous&&interaction.Target.IsNPC)&&(interaction.Target.IsRouting&&!(interaction.Actor.Posture is SwimmingInPool)&&(!(interaction.Actor.Posture is ScubaDiving)&&LotManager.RoomIdIsOutside(interaction.Target.RoomId)))&&(interaction.Target.CurrentInteraction==null||interaction.Target.CurrentInteraction.Autonomous)){
                    Vector3 vector3=interaction.Target.Position-interaction.Actor.Position;
                    float num1=vector3.LengthSqr();
                    vector3=vector3.Normalize();
                    float num2=100f;
                    if((double)num1<=(double)num2*(double)num2){
                        float num3=1.570796f;
                        if((double)(vector3*interaction.Target.ForwardVector)>Math.Cos((double)num3))
                            interaction.CallOver();
                    }
                }
                interaction.mResultCode=SocialInteraction.SocialResultCode.None;
                Sim  actor=interaction. Actor;
                Sim target=interaction.Target;
                SocialInteraction.SocialResultCode resultCode;
                bool flag1;
                if((actor.Conversation==null||actor.Conversation!=target.Conversation)&&!actor.IsRiding(interaction.Target)||pairedSocial){
                    if(target.SimRoutingComponent.IsInCarSequence()||target.IsSleeping&&!SocialInteraction.CanInteractWithSleepingSim(actor,target)){
                        interaction.SetResultCodeIfNoneIsSet(SocialInteraction.SocialResultCode.SocialNotAvailableOnTarget);
            return false;
                    }
                    interaction.mSleepingListener=EventTracker.AddListener(EventTypeId.kSimFellAsleep,new ProcessEventDelegate(interaction.CancelSocialOnEventTrackerEvent),target,(GameObject)null);
                     flag1=interaction.SocialRouteA(socialToPushToTarget,pairedSocial,separationDist,out resultCode);
                }else{
                    resultCode=interaction.LockSimB(socialToPushToTarget,pairedSocial,separationDist);
                     flag1=resultCode==SocialInteraction.SocialResultCode.Succeeded;
                }
                interaction.SetInitialRouteComplete();
                bool flag2=false;
                  if(flag1){
                    actor.LoopIdle();
                    if(interaction is FeedOnFloorFix feedOnFloor&&feedOnFloor!=null&&feedOnFloor.StartSync1(true,false,(SyncLoopCallbackFunction)null,0.0f,true)){
                        SocialInteraction interactionInstance=interaction.LinkedInteractionInstance as SocialInteraction;
                        if(interaction.mbIsTurnToFace){
                            DateAndTime previousDateAndTime=SimClock.CurrentTime();
                            while(!interactionInstance.mbBRoutePlanned){
                                if((double)SimClock.ElapsedTime(TimeUnit.Minutes,previousDateAndTime)>=(double)SocialInteraction.kSocialTimeoutTime||actor.HasExitReason(ExitReason.Default)||(target.HasExitReason(ExitReason.Default)||interactionInstance.Cancelled)){
                                    interaction.mResultCode=SocialInteraction.SocialResultCode.PathPlanToToSocialTargetFailed;
            return false;
                                }
Simulator.Sleep(0U);
                            }
                        }
                        if(interaction.mbIsTurnToFace&&!interactionInstance.mbIsTurnToFace)
                            interaction.SetApproachRouteComplete();
                        actor.SynchronizationLevel=Sim.SyncLevel.Started;
                    SimFix fix=new SimFix(actor);
                        if(fix.WaitForSynchronizationLevelWithSim1(target,Sim.SyncLevel.Routed,SocialInteraction.kSocialSyncGiveupTime)){
                            if(interaction.mRoute!=null){
                                if(interaction.mbIsTurnToFace&&interactionInstance.mbIsTurnToFace&&interaction.mRoute.PlanResult.Succeeded()){
                                    interaction.mRoute.CreateSegmentAtDistanceBeforeEndOfRoute(SocialInteraction.kApproachGreetDistance);
                                    interaction.mRoute.RegisterCallback(new RouteCallback(interaction.ReleaseSimBApproach),RouteCallbackType.TriggerOnce,RouteCallbackConditions.LessThan(SocialInteraction.kApproachGreetDistance+0.5f));
                                }
                     flag1=actor.DoRoute(interaction.mRoute);
                                interaction.mRoute=(Sims3.SimIFace.Route)null;
                                if(interaction.Actor.Posture is ScubaDiving&&flag1)
                                    actor.LoopIdle();
                            }
                        }else{
                     flag2=true;
                            resultCode=SocialInteraction.SocialResultCode.SyncToSocialTargetTimedOut;
                     flag1=false;
                        }
                        actor.SynchronizationLevel=Sim.SyncLevel.Committed;
                     SimFix fix1=new SimFix(actor);
                        if(!fix1.WaitForSynchronizationLevelWithSim1(target,Sim.SyncLevel.Committed,SocialInteraction.kSocialSyncGiveupTime)){
                            resultCode=SocialInteraction.SocialResultCode.SyncToSocialTargetTimedOut;
                     flag1=false;
                        }
                    }else
                     flag1=false;
                }
                  if(flag2&&!actor.HasExitReason(ExitReason.UserCanceled))
                    actor.PlayRouteFailure(target.GetThoughtBalloonThumbnailKey());
                if(actor.HasExitReason()){
                     flag1=false;
                    resultCode=SocialInteraction.SocialResultCode.CanceledByExitReason;
                }
                  if(flag1&&interaction.mbValidateFacing&&!interaction.SimsAreFacing(actor,target))
                    actor.RoutingComponent.RouteTurnToFace(target.PositionOnFloor);
                if(!interaction.OnBeginSocialInteractionDone())
                     flag1=false;
                  if(flag1){
                    actor.PopCanePostureIfNecessary();
                    if(!(actor.CurrentInteraction is IUmbrellaSupportedSocial))
                        Umbrella.PopUmbrellaPostureIfNecessary(actor,false);
                    actor.PopBackpackPostureIfNecessary();
                    if(!(actor.CurrentInteraction is IJetpackInteraction))
                        actor.PopJetpackPostureIfNecessary();
                  }
                interaction.SetResultCodeIfNoneIsSet(resultCode);
                  if(flag1){
                    interaction.CheckForPlayTimeBuffs(interaction.Actor,interaction.Target);
                    interaction.CheckForPlayTimeBuffs(interaction.Target,interaction.Actor);
                  }
            return flag1;
            }
        }
        public bool StartSync1(bool shouldBeMaster,bool ignoreExitReasons,SyncLoopCallbackFunction loopCallback,float notifySimMinutes,bool performSocializeWithTest){
            if(!this.SafeToSync()){
        return false;
            }
            Sim syncTarget=this.GetSyncTarget();
            if(syncTarget==null){
        return false;
            }
            ExitReason exitReason=ignoreExitReasons?ExitReason.None:ExitReason.Default;
            DateAndTime previousDateAndTime1=SimClock.CurrentTime();
            DateAndTime previousDateAndTime2=previousDateAndTime1;
            while((double)SimClock.ElapsedTime(TimeUnit.Minutes,previousDateAndTime1)<(double)InteractionInstance.kNumMinToWaitOnPreSync&&(this.GetTargetCurrentInteraction()==null||this.GetTargetCurrentInteraction().LinkedInteractionInstance!=this)){
                if(this.InstanceActor.HasExitReason(exitReason)){
        return false;
                }
                bool flag=false;
                if(syncTarget.InteractionQueue.IsRunning(this.LinkedInteractionInstance,true)){
                     flag=true;
                }else{
                    foreach(InteractionInstance interaction in syncTarget.InteractionQueue.InteractionList){
                        if(interaction.LinkedInteractionInstance==this){
                     flag=true;
                            break;
                        }
                    }
                }
                if(syncTarget.InteractionQueue.GetHeadInteraction() is IPreventSocialization headInteraction&&headInteraction!=null&&!headInteraction.SocializationAllowed(this.InstanceActor,syncTarget)||!flag){
        return false;
                }
                if(loopCallback!=null&&(double)SimClock.ElapsedTime(TimeUnit.Minutes,previousDateAndTime2)>=(double)notifySimMinutes){
                    if(!loopCallback()){
        return false;
                    }
                    previousDateAndTime2=SimClock.CurrentTime();
                }
Simulator.Sleep(0U);
            }
            this.InstanceActor.SynchronizationRole=shouldBeMaster?Sim.SyncRole.Initiator:Sim.SyncRole.Receiver;
            this.InstanceActor.SynchronizationTarget=syncTarget;
            this.InstanceActor.SynchronizationLevel=Sim.SyncLevel.Started;
         SimFix fix=new SimFix(this.InstanceActor);
            if(!fix.WaitForSynchronizationLevelWithSim1(syncTarget,Sim.SyncLevel.Started,exitReason,(float)InteractionInstance.kNumMinToWaitOnSyncStart,loopCallback,notifySimMinutes,performSocializeWithTest)){
        return false;
            }
        return true;}
    }
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
            if(!CarriedChildInteractionBFix.StartInteractionWithCarriedChild1((SocialInteraction)this,"BeGivenBottle")){
        return(false);
            }
this.BeginCommodityUpdates();
          this.Actor.CarryingChildPosture.AnimateInteractionWithCarriedChild(nameof(GiveBottle));
          this.Target.Motives.SetMax(CommodityKind.Hunger);
this.EndCommodityUpdates(true);
            if(!ChildUtils.FinishInteractionWithCarriedChild((SocialInteraction)this)){
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
            return(false);
                }
            return( true);}
            public override InteractionTestResult Test(ref InteractionInstanceParameters parameters,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
                Sim  actor1=parameters. Actor as Sim;
                Sim target1=parameters.Target as Sim;
                if((object) actor1==null)
            return InteractionTestResult.Root_Null_Actor;
                if((object)target1==null)
            return InteractionTestResult.Root_Null_Target;
                Sim actor2=(object)actor1 as Sim;
                IGameObject target2=(IGameObject)target1;
                for(IGameObject gameObject=(IGameObject)target1;gameObject!=null;gameObject=gameObject.Parent){
                    if(gameObject==LiveDragHelperModel.CachedTopDraggedObject)
            return InteractionTestResult.Root_TargetOnHandTool;
                }
                bool flag=true;
                InteractionTuning tuning=parameters.InteractionObjectPair.Tuning;
                Tradeoff mTradeoff = tuning?.mTradeoff;
                if(tuning!=null){
                    CommodityKind workMotive=actor1.WorkMotive;
                    if(workMotive!=CommodityKind.None&&mTradeoff.SatisfiesCommodity(workMotive))
                     flag=false;
                    if(parameters.Autonomous){
                        if(tuning.HasFlags(InteractionTuning.FlagField.DisallowAutonomous)&&flag)
            return InteractionTestResult.Tuning_DisallowAutonomous;
                        if(mTradeoff.FunExit&&actor1.Motives.FunInteractionTest(false))
            return InteractionTestResult.Tuning_FunInteractionTest;
                    }else if(tuning.HasFlags(InteractionTuning.FlagField.DisallowUserDirected))
            return InteractionTestResult.Tuning_DisallowUserDirected;
                    if(actor1.IsSelectable&&tuning.HasFlags(InteractionTuning.FlagField.DisallowPlayerSim))
            return InteractionTestResult.Tuning_DisallowPlayerSim;
                  if(flag){
                                                    AutonomyFix fix=new AutonomyFix(actor1.Autonomy.mActor,actor1.Autonomy.Motives,actor1.Autonomy.CurrentSearchType,actor1.Autonomy.IsActorInTombRoom);
                    InteractionTestResult interactionTestResult=fix.CheckAvailability1(parameters.Autonomous,tuning.Availability,parameters.InteractionObjectPair);
                    if(interactionTestResult!=InteractionTestResult.Pass)
            return interactionTestResult;
                  }
                }
                actor1.Autonomy.UpdateCacheIfNeeded((IGameObject)target1);
               var interactionTestResult1=AutonomyFix.CommonTests1((InteractionDefinition)this,actor2,target2,parameters);
                if(interactionTestResult1!=InteractionTestResult.Pass)
            return interactionTestResult1;
               var interactionTestResult2=!(this is IMetaInteractionDefinition)?    InteractionDefinitionUtilities.SpecialCaseTests((InteractionDefinition)this,actor2,target2,parameters) 
                                                                               :MetaInteractionDefinitionUtilities.SpecialCaseTests(actor2,target2,parameters);
                if(interactionTestResult2!=InteractionTestResult.Pass)
            return interactionTestResult2;
                if(!this.Test(actor1,target1,parameters.Autonomous,ref greyedOutTooltipCallback))
            return InteractionTestResult.Def_TestFailed;
                if(tuning!=null){
 InteractionTestResult interactionTestResult3=actor1.Autonomy.CheckAvailabilityTooltip((InteractionDefinition)this,(IGameObject)target1,tuning.Availability,parameters,mTradeoff,ref greyedOutTooltipCallback);
                    if(interactionTestResult3!=InteractionTestResult.Pass)
                return interactionTestResult3;
                }
 InteractionTestResult interactionTestResult4=InteractionDefinitionUtilities.SpecialCaseTooltipTests((InteractionDefinition)this,actor2,target2,parameters,ref greyedOutTooltipCallback);
                return interactionTestResult4!=InteractionTestResult.Pass?interactionTestResult4:InteractionTestResult.Pass;
            }
        }
    }
    public class ChangeToddlerClothesFix:ChangeToddlerClothes,IPreLoad,IAddInteraction{
        static InteractionDefinition sOldSingleton;
                                              public void AddInteraction(InteractionInjectorList interactions){
                                                                                                 interactions.ReplaceNoTest<Sim,ChangeToddlerClothes.ChangeToddlerClothesDefinition>(Singleton);
                                              }
                                   public void OnPreLoad(){
            Tunings.Inject<Sim,ChangeToddlerClothes.ChangeToddlerClothesDefinition,ChangeToddlerClothesDefinition>(false);
                                     sOldSingleton=Singleton;
                                                   Singleton=new ChangeToddlerClothesDefinition();
                                   }
        public override bool Run(){
            if(this.Autonomous)
                this.Reason=Sim.ClothesChangeReason.TemperatureTooCold;
            switch(this.StartBuildingOutfit()){
                case ChildUtils.Return.False:
        return false;
                case ChildUtils.Return.True:
        return true;
                default:
                    if(!CarriedChildInteractionBFix.StartInteractionWithCarriedChild1((SocialInteraction)this,"ChangeClothes"))
        return false;
this.BeginCommodityUpdates();
                    this.mSwitchOutfitHelper.Wait(true);
                    this.mSwitchOutfitHelper.AddScriptEventHandler(this.Actor.Posture.CurrentStateMachine);
                    this.Actor.CarryingChildPosture.AnimateInteractionWithCarriedChild("ChangeDiaper");
                    if(this.Reason==Sim.ClothesChangeReason.Force)
                        this.Target.MarkUserDirectedClothingChange();
this.EndCommodityUpdates(true);
                    this.NotifyHudSimChanged();
        return ChildUtils.FinishInteractionWithCarriedChild((SocialInteraction)this);
            }
        }
        public new class ChangeToddlerClothesDefinition:ChangeToddlerClothes.ChangeToddlerClothesDefinition{
            public override InteractionInstance CreateInstance(ref InteractionInstanceParameters parameters){
                            InteractionInstance na=new ChangeToddlerClothesFix();
                                                na.Init(ref parameters);
                                         return na;
            }
            public override InteractionTestResult Test(ref InteractionInstanceParameters parameters,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
                Sim  actor1=parameters. Actor as Sim;
                Sim target1=parameters.Target as Sim;
                if((object) actor1==null)
            return InteractionTestResult.Root_Null_Actor;
                if((object)target1==null)
            return InteractionTestResult.Root_Null_Target;
                Sim actor2=(object)actor1 as Sim;
                IGameObject target2=(IGameObject)target1;
                for(IGameObject gameObject=(IGameObject)target1;gameObject!=null;gameObject=gameObject.Parent){
                    if(gameObject==LiveDragHelperModel.CachedTopDraggedObject)
            return InteractionTestResult.Root_TargetOnHandTool;
                }
                bool flag=true;
                InteractionTuning tuning=parameters.InteractionObjectPair.Tuning;
                Tradeoff mTradeoff = tuning?.mTradeoff;
                if(tuning!=null){
                    CommodityKind workMotive=actor1.WorkMotive;
                    if(workMotive!=CommodityKind.None&&mTradeoff.SatisfiesCommodity(workMotive))
                     flag=false;
                    if(parameters.Autonomous){
                        if(tuning.HasFlags(InteractionTuning.FlagField.DisallowAutonomous)&&flag)
            return InteractionTestResult.Tuning_DisallowAutonomous;
                        if(mTradeoff.FunExit&&actor1.Motives.FunInteractionTest(false))
            return InteractionTestResult.Tuning_FunInteractionTest;
                    }else if(tuning.HasFlags(InteractionTuning.FlagField.DisallowUserDirected))
            return InteractionTestResult.Tuning_DisallowUserDirected;
                    if(actor1.IsSelectable&&tuning.HasFlags(InteractionTuning.FlagField.DisallowPlayerSim))
            return InteractionTestResult.Tuning_DisallowPlayerSim;
                  if(flag){
                                                    AutonomyFix fix=new AutonomyFix(actor1.Autonomy.mActor,actor1.Autonomy.Motives,actor1.Autonomy.CurrentSearchType,actor1.Autonomy.IsActorInTombRoom);
                    InteractionTestResult interactionTestResult=fix.CheckAvailability1(parameters.Autonomous,tuning.Availability,parameters.InteractionObjectPair);
                    if(interactionTestResult!=InteractionTestResult.Pass)
            return interactionTestResult;
                  }
                }
                actor1.Autonomy.UpdateCacheIfNeeded((IGameObject)target1);
               var interactionTestResult1=AutonomyFix.CommonTests1((InteractionDefinition)this,actor2,target2,parameters);
                if(interactionTestResult1!=InteractionTestResult.Pass)
            return interactionTestResult1;
               var interactionTestResult2=!(this is IMetaInteractionDefinition)?    InteractionDefinitionUtilities.SpecialCaseTests((InteractionDefinition)this,actor2,target2,parameters) 
                                                                               :MetaInteractionDefinitionUtilities.SpecialCaseTests(actor2,target2,parameters);
                if(interactionTestResult2!=InteractionTestResult.Pass)
            return interactionTestResult2;
                if(!this.Test(actor1,target1,parameters.Autonomous,ref greyedOutTooltipCallback))
            return InteractionTestResult.Def_TestFailed;
                if(tuning!=null){
 InteractionTestResult interactionTestResult3=actor1.Autonomy.CheckAvailabilityTooltip((InteractionDefinition)this,(IGameObject)target1,tuning.Availability,parameters,mTradeoff,ref greyedOutTooltipCallback);
                    if(interactionTestResult3!=InteractionTestResult.Pass)
                return interactionTestResult3;
                }
 InteractionTestResult interactionTestResult4=InteractionDefinitionUtilities.SpecialCaseTooltipTests((InteractionDefinition)this,actor2,target2,parameters,ref greyedOutTooltipCallback);
                return interactionTestResult4!=InteractionTestResult.Pass?interactionTestResult4:InteractionTestResult.Pass;
            }
        }
    }
    public class ChangeDiaperFix:ChangeDiaper,IPreLoad,IAddInteraction{
        static InteractionDefinition sOldSingleton;
                                              public void AddInteraction(InteractionInjectorList interactions){
                                                                                                 interactions.Replace<Sim,ChangeDiaper.Definition>(Singleton);
                                              }
                                   public void OnPreLoad(){
            Tunings.Inject<Sim,ChangeDiaper.Definition,Definition>(false);
                                     sOldSingleton=Singleton;
                                                   Singleton=new Definition();
                                   }
        public override bool Run(){
            if(!CarriedChildInteractionBFix.StartInteractionWithCarriedChild1((SocialInteraction)this,"HaveDiaperChanged"))
        return false;
this.BeginCommodityUpdates();
            this.Actor.CarryingChildPosture.AnimateInteractionWithCarriedChild(nameof(ChangeDiaper));
            this.Target.Motives.SetMax(CommodityKind.Hygiene);
this.EndCommodityUpdates(true);
        return ChildUtils.FinishInteractionWithCarriedChild((SocialInteraction)this);
        }
        public new class Definition:ChangeDiaper.Definition{
            public override InteractionInstance CreateInstance(ref InteractionInstanceParameters parameters){
                            InteractionInstance na=new ChangeDiaperFix();
                                                na.Init(ref parameters);
                                         return na;
            }
            public override bool Test(Sim a,Sim target,bool isAutonomous,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
            return target.SimDescription.ToddlerOrBelow;
            }
            public override InteractionTestResult Test(ref InteractionInstanceParameters parameters,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
                Sim  actor1=parameters. Actor as Sim;
                Sim target1=parameters.Target as Sim;
                if((object) actor1==null)
            return InteractionTestResult.Root_Null_Actor;
                if((object)target1==null)
            return InteractionTestResult.Root_Null_Target;
                Sim actor2=(object)actor1 as Sim;
                IGameObject target2=(IGameObject)target1;
                for(IGameObject gameObject=(IGameObject)target1;gameObject!=null;gameObject=gameObject.Parent){
                    if(gameObject==LiveDragHelperModel.CachedTopDraggedObject)
            return InteractionTestResult.Root_TargetOnHandTool;
                }
                bool flag=true;
                InteractionTuning tuning=parameters.InteractionObjectPair.Tuning;
                Tradeoff mTradeoff = tuning?.mTradeoff;
                if(tuning!=null){
                    CommodityKind workMotive=actor1.WorkMotive;
                    if(workMotive!=CommodityKind.None&&mTradeoff.SatisfiesCommodity(workMotive))
                     flag=false;
                    if(parameters.Autonomous){
                        if(tuning.HasFlags(InteractionTuning.FlagField.DisallowAutonomous)&&flag)
            return InteractionTestResult.Tuning_DisallowAutonomous;
                        if(mTradeoff.FunExit&&actor1.Motives.FunInteractionTest(false))
            return InteractionTestResult.Tuning_FunInteractionTest;
                    }else if(tuning.HasFlags(InteractionTuning.FlagField.DisallowUserDirected))
            return InteractionTestResult.Tuning_DisallowUserDirected;
                    if(actor1.IsSelectable&&tuning.HasFlags(InteractionTuning.FlagField.DisallowPlayerSim))
            return InteractionTestResult.Tuning_DisallowPlayerSim;
                  if(flag){
                                                    AutonomyFix fix=new AutonomyFix(actor1.Autonomy.mActor,actor1.Autonomy.Motives,actor1.Autonomy.CurrentSearchType,actor1.Autonomy.IsActorInTombRoom);
                    InteractionTestResult interactionTestResult=fix.CheckAvailability1(parameters.Autonomous,tuning.Availability,parameters.InteractionObjectPair);
                    if(interactionTestResult!=InteractionTestResult.Pass)
            return interactionTestResult;
                  }
                }
                actor1.Autonomy.UpdateCacheIfNeeded((IGameObject)target1);
               var interactionTestResult1=AutonomyFix.CommonTests1((InteractionDefinition)this,actor2,target2,parameters);
                if(interactionTestResult1!=InteractionTestResult.Pass)
            return interactionTestResult1;
               var interactionTestResult2=!(this is IMetaInteractionDefinition)?    InteractionDefinitionUtilities.SpecialCaseTests((InteractionDefinition)this,actor2,target2,parameters) 
                                                                               :MetaInteractionDefinitionUtilities.SpecialCaseTests(actor2,target2,parameters);
                if(interactionTestResult2!=InteractionTestResult.Pass)
            return interactionTestResult2;
                if(!this.Test(actor1,target1,parameters.Autonomous,ref greyedOutTooltipCallback))
            return InteractionTestResult.Def_TestFailed;
                if(tuning!=null){
 InteractionTestResult interactionTestResult3=actor1.Autonomy.CheckAvailabilityTooltip((InteractionDefinition)this,(IGameObject)target1,tuning.Availability,parameters,mTradeoff,ref greyedOutTooltipCallback);
                    if(interactionTestResult3!=InteractionTestResult.Pass)
                return interactionTestResult3;
                }
 InteractionTestResult interactionTestResult4=InteractionDefinitionUtilities.SpecialCaseTooltipTests((InteractionDefinition)this,actor2,target2,parameters,ref greyedOutTooltipCallback);
                return interactionTestResult4!=InteractionTestResult.Pass?interactionTestResult4:InteractionTestResult.Pass;
            }
        }
    }
    //  TO DO
    public class CarriedChildInteractionBFix:CarriedChildInteractionB,IPreLoad,IAddInteraction{
                                              public void AddInteraction(InteractionInjectorList interactions){
                                              }
                                   public void OnPreLoad(){
                                   }
        public override bool Test(){
            if(this.LinkedInteractionInstance==null||this.LinkedInteractionInstance.Test()){
        return Test1();
            }
            InteractionInstance currentInteraction=this.LinkedInteractionInstance.InstanceActor.CurrentInteraction;
            InteractionInstance interactionInstance=this.LinkedInteractionInstance;
        return false;
        }
        public          bool Test1(){
            InteractionInstanceParameters interactionParameters=this.GetInteractionParameters();
            GreyedOutTooltipCallback greyedOutTooltipCallback=(GreyedOutTooltipCallback)null;
        return IUtil.IsPass(this.InteractionDefinition.Test(ref interactionParameters,ref greyedOutTooltipCallback));
        }
        public override bool Run(){
            bool succeeded=false;
            if(!this.StartSlaveInteraction()){
        return false;
            }
this.Actor.OnSocializedWith();
            if(this.LinkedInteractionInstance==null||!this.Target.InteractionQueue.HasInteraction(this.LinkedInteractionInstance)&&this.Target.InteractionQueue.TransitionInteraction!=this.LinkedInteractionInstance){
        return false;
            }
          bool flag=!this.Target.IsRiding(this.Actor)||!this.ActionData.SocialCanBeDoneToMounedHorse//  "Mounted"
                    ?this.ReceiveSocialInteraction1()//  Baby: Receive Interaction
                    :this.StartSync3(false,false,(SyncLoopCallbackFunction)null,0.0f,true);//  Riding Horse 
            this.AddMotiveArrow(CommodityKind.Social,true);
this.BeginCommodityUpdates();
            if(flag){
                if(this.LinkedInteractionInstance is SocialInteractionA)this.Actor.EnterSocializingPosture(this.Target);
                 succeeded=this.DoLoop(ExitReason.Default);
            }else{
                this.Actor.AddExitReason(ExitReason.RouteFailed);
            }
            this.CopyExitReasonToLinkedInteraction();
            this.WaitForMasterInteractionToFinish();
this.EndCommodityUpdates(succeeded);
            this.WaitForSyncComplete();
        return succeeded;
        }
        public bool ReceiveSocialInteraction1(){
            this.mResultCode=SocialInteraction.SocialResultCode.None;
            Sim target=this.Target;
            Sim actor=this.Actor;
                actor.SynchronizationLevel=Sim.SyncLevel.Started;
                actor.SynchronizationTarget=target;
                actor.SynchronizationRole=Sim.SyncRole.Receiver;
            this.mbIsTurnToFace=false;
            bool flag1=false;
              SocialInteraction interactionInstance1=this.LinkedInteractionInstance as SocialInteraction;
            InteractionInstance interactionInstance2=target.InteractionQueue.TransitionInteraction??target.CurrentInteraction;
            if(interactionInstance1!=null&&interactionInstance1!=interactionInstance2){
                if(interactionInstance2 is ISyncViaGroupId){
                    if((long)interactionInstance1.GroupId!=(long)interactionInstance2.GroupId){
                        this.mResultCode=SocialInteraction.SocialResultCode.PathPlanToToSocialTargetFailed;
        return false;
                    }
                }else if(!interactionInstance1.PartOfGoHereSituation&&!this.PartOfGoHereSituation){
                    this.mResultCode=SocialInteraction.SocialResultCode.PathPlanToToSocialTargetFailed;
        return false;
                }
            }
                actor.LoopIdle();
            bool flag2=false;
            bool flag3=false;
            DateAndTime previousDateAndTime1=SimClock.CurrentTime();
            while(!interactionInstance1.mbInitialRoute){
                if((double)SimClock.ElapsedTime(TimeUnit.Minutes,previousDateAndTime1)>=(double)SocialInteraction.kSocialTimeoutTime){
                    this.mResultCode=SocialInteraction.SocialResultCode.PathPlanToToSocialTargetFailed;
        return false;
                }
                if(target.HasExitReason(ExitReason.Default)){
                    this.mResultCode=SocialInteraction.SocialResultCode.PathPlanToToSocialTargetFailed;
        return false;
                }
                if(actor.HasExitReason(ExitReason.Default)){
                    this.mResultCode=SocialInteraction.SocialResultCode.PathPlanToToSocialTargetFailed;
        return false;
                }
                if(interactionInstance1.Cancelled){
                    this.mResultCode=SocialInteraction.SocialResultCode.PathPlanToToSocialTargetFailed;
        return false;
                }
Simulator.Sleep(0U);
            }
            bool flag4=true;
            if(this.SocialJig!=null){
                if(Sims3.SimIFace.Objects.IsValid(this.SocialJig.ObjectId)&&this.SocialJig.EnsurePlacement(target,actor)){
                    Vector3 routeFromPoint=Vector3.Invalid;
                    if(target.IsRouting&&target.RoutingComponent.GetCurrentRoute()!=null)
                            routeFromPoint=target.RoutingComponent.GetCurrentRoute().GetDestPoint();
                        Sims3.SimIFace.Route jigA=this.SocialJig.RouteToJigA(target,actor.ObjectId,routeFromPoint);
                        Sims3.SimIFace.Route jigB=this.SocialJig.RouteToJigB(actor,target.ObjectId);
                        if(jigA.PlanResult.Succeeded()&&jigB.PlanResult.Succeeded()){
                            interactionInstance1.mRoute=jigA;
                            this.mRoute=jigB;
                 flag1=true;
                        }else{
                            this.mRoute=interactionInstance1.mRoute=(Sims3.SimIFace.Route)null;
                 flag4=false;
                        }
                }else{
                    this.mRoute=interactionInstance1.mRoute=(Sims3.SimIFace.Route)null;
                 flag4=false;
                }
            }else if(actor.Posture is RidingPosture posture){
                if(posture.Container is Sim container){
                    Sims3.SimIFace.Route routeTurnToFace=container.CreateRouteTurnToFace(target.Position);
                                         routeTurnToFace.ExecutionFromNonSimTaskIsSafe=true;
                       container.DoRoute(routeTurnToFace);
                }
            }else if(actor.SimDescription.Baby){
                if(!actor.Posture.Satisfies(CommodityKind.BeingCarried,(IGameObject)null))
                 flag4=this.SetupTwoPersonSocialWithBaby(target,actor,interactionInstance1);
            }else if(actor.Posture is SwimmingInPool||actor.Posture.AllowsRouting()){
                Conversation conversation=actor.Conversation;
                             conversation?.DiscourageRegion.Disable();
                List<Sim>simList=new List<Sim>();
                if(actor.Conversation!=null)
                         simList.AddRange((IEnumerable<Sim>)actor.Conversation.Members);
                     if(!simList.Contains(target))
                         simList.Add(     target);
                     if(!simList.Contains(actor))
                         simList.Add(     actor);
                if(this.mbPairedSocial||!SocialInteraction.sGroupConversationRoutingEnabled||simList.Count<=2){
                 flag2=this.SimsNeedToRoute(target,actor);
                 flag3=!flag2;
                }
              if(flag2)
                 flag4=this.SetupTwoPersonSocial(target,actor,interactionInstance1);
              else if(actor.Conversation!=null&&!flag3)
                 flag4=this.SetupGroupSocial(target,actor,interactionInstance1);
                             conversation?.DiscourageRegion.Enable();
            }
            if(this.mRoute!=null){
                if(actor.SimDescription.ToddlerOrAbove){
                    if(this.mbIsTurnToFace&&interactionInstance1.mbIsTurnToFace){
                        actor.SynchronizationLevel=Sim.SyncLevel.Routed;
                        this.mbBRoutePlanned=true;
                        DateAndTime previousDateAndTime2=SimClock.CurrentTime();
                        while(!interactionInstance1.mbApproachRoute){
                            if((double)SimClock.ElapsedTime(TimeUnit.Minutes,previousDateAndTime2)>=(double)SocialInteraction.kSocialTimeoutTime){
                                this.mResultCode=SocialInteraction.SocialResultCode.PathPlanToToSocialTargetFailed;
        return false;
                            }
                            if(target.HasExitReason(ExitReason.Default)){
                                this.mResultCode=SocialInteraction.SocialResultCode.PathPlanToToSocialTargetFailed;
        return false;
                            }
                            if(actor.HasExitReason(ExitReason.Default)){
                                this.mResultCode=SocialInteraction.SocialResultCode.PathPlanToToSocialTargetFailed;
        return false;
                            }
                            if(interactionInstance1.Cancelled){
                                this.mResultCode=SocialInteraction.SocialResultCode.PathPlanToToSocialTargetFailed;
        return false;
                            }
Simulator.Sleep(0U);
                        }
                    }else 
              if(flag1){
                        actor.SynchronizationLevel=Sim.SyncLevel.Routed;
                        this.mbBRoutePlanned=true;
Simulator.Sleep(0U);
              }
                 flag4=actor.DoRoute(this.mRoute);
                actor.LoopIdle();
                }
                this.mRoute=(Sims3.SimIFace.Route)null;
            }
            this.mbIsTurnToFace = false;
              if(flag4){
                actor.SynchronizationLevel=Sim.SyncLevel.Routed;
                this.mbBRoutePlanned=true;
            SimFix fix=new SimFix(actor);
                if(fix.WaitForSynchronizationLevelWithSim1(target,Sim.SyncLevel.Routed,SocialInteraction.kSocialSyncGiveupTime)&&flag2){
                    interactionInstance1.mbValidateFacing=true;
                    if(!this.SimsAreFacing(target,actor)){
                 flag4=actor.RoutingComponent.RouteTurnToFace(target.PositionOnFloor);
                        actor.LoopIdle();
                    }
                }
              }
              if(flag4){
                actor.PopCanePostureIfNecessary();
                if(!(interactionInstance1 is IUmbrellaSupportedSocial))
                    Umbrella.PopUmbrellaPostureIfNecessary(actor,false);
                actor.PopBackpackPostureIfNecessary();
                if(!(interactionInstance1 is IJetpackInteraction))
                actor.PopJetpackPostureIfNecessary();
                actor.SynchronizationLevel=Sim.SyncLevel.Committed;
                SimFix fix=new SimFix(actor);
            bool flag5=fix.WaitForSynchronizationLevelWithSim1(target,Sim.SyncLevel.Committed,SocialInteraction.kSocialSyncGiveupTime);
                this.mResultCode=!flag5?(!actor.HasExitReason()?SocialInteraction.SocialResultCode.SyncToSocialTargetTimedOut:SocialInteraction.SocialResultCode.CanceledByExitReason):SocialInteraction.SocialResultCode.Succeeded;
          return flag5;
              }
            this.mResultCode=SocialInteraction.SocialResultCode.PathPlanToToSocialTargetFailed;
        return false;
        }
        public bool StartSync3(bool shouldBeMaster,bool ignoreExitReasons,SyncLoopCallbackFunction loopCallback,float notifySimMinutes,bool performSocializeWithTest){
            if(!this.SafeToSync()){
        return false;
            }
            Sim syncTarget=this.GetSyncTarget();
            if(syncTarget==null){
        return false;
            }
            ExitReason exitReason=ignoreExitReasons?ExitReason.None:ExitReason.Default;
            DateAndTime previousDateAndTime1=SimClock.CurrentTime();
            DateAndTime previousDateAndTime2=previousDateAndTime1;
            while((double)SimClock.ElapsedTime(TimeUnit.Minutes,previousDateAndTime1)<(double)InteractionInstance.kNumMinToWaitOnPreSync&&(this.GetTargetCurrentInteraction()==null||this.GetTargetCurrentInteraction().LinkedInteractionInstance!=this)){
                if(this.InstanceActor.HasExitReason(exitReason)){
        return false;
                }
                bool flag=false;
                if(syncTarget.InteractionQueue.IsRunning(this.LinkedInteractionInstance,true)){
                     flag=true;
                }else{
                    foreach(InteractionInstance interaction in syncTarget.InteractionQueue.InteractionList){
                        if(interaction.LinkedInteractionInstance==this){
                     flag=true;
                            break;
                        }
                    }
                }
                if(syncTarget.InteractionQueue.GetHeadInteraction() is IPreventSocialization headInteraction&&headInteraction!=null&&!headInteraction.SocializationAllowed(this.InstanceActor,syncTarget)||!flag){
        return false;
                }
                if(loopCallback!=null&&(double)SimClock.ElapsedTime(TimeUnit.Minutes,previousDateAndTime2)>=(double)notifySimMinutes){
                    if(!loopCallback()){
        return false;
                    }
                    previousDateAndTime2=SimClock.CurrentTime();
                }
Simulator.Sleep(0U);
            }
            this.InstanceActor.SynchronizationRole=shouldBeMaster?Sim.SyncRole.Initiator:Sim.SyncRole.Receiver;
            this.InstanceActor.SynchronizationTarget=syncTarget;
            this.InstanceActor.SynchronizationLevel=Sim.SyncLevel.Started;
         SimFix fix=new SimFix(this.InstanceActor);
            if(!fix.WaitForSynchronizationLevelWithSim1(syncTarget,Sim.SyncLevel.Started,exitReason,(float)InteractionInstance.kNumMinToWaitOnSyncStart,loopCallback,notifySimMinutes,performSocializeWithTest)){
        return false;
            }
        return true;}
        public new class Definition:CarriedChildInteractionB.Definition{
            public          InteractionInstance CreateInstance0(ref InteractionInstanceParameters parameters){
                            InteractionInstance interaction=new CarriedChildInteractionBFix();
                                                interaction.Init(ref parameters);
                     return(InteractionInstance)interaction;
            }
            public          InteractionInstance CreateInstance1(ref InteractionInstanceParameters parameters){
                            InteractionInstance instance=CreateInstance0(ref parameters);
                if(this.AllowCarryChild){
                    PosturePreconditionSetData set1=new PosturePreconditionSetData(CommodityKind.CarryingChild  ,0.0f);
                    PosturePreconditionOptionsData data=new PosturePreconditionOptionsData();
                                                   data.AddOption(set1);
                    PosturePreconditionSetData set2=new PosturePreconditionSetData(CommodityKind.CarryingPet    ,0.0f);
                                                   data.AddOption(set2);
                    PosturePreconditionSetData set3=new PosturePreconditionSetData(CommodityKind.CarryingHoloPet,0.0f);
                                                   data.AddOption(set3);
                    if(instance.PosturePreconditions==null){
                    PosturePreconditionSetData set4=new PosturePreconditionSetData(CommodityKind.Standing       ,1f  );
                                                   data.AddOption(set4);
                    }else{
                       foreach(PreconditionSet set4 in instance.PosturePreconditions.Sets){
                    PosturePreconditionSetData set5=new PosturePreconditionSetData(set4.Commodity,set4.Value);
                            foreach(CommodityKind condition in set4.Conditions)
                                               set5.AddCheck(condition);
                                            if(set5.Commodity==CommodityKind.PostureSocializing)
                                               set5.AddCheck(CommodityKind.MayCarryChild);
                                data.AddOption(set5);
                       }
                    }
                                                instance.PosturePreconditions=new PreconditionOptions(data);
                }
                                         return instance;
            }
            public override InteractionInstance CreateInstance(ref InteractionInstanceParameters parameters){
                            InteractionInstance instance=CreateInstance1(ref parameters);
                PosturePreconditionSetData set=new PosturePreconditionSetData(CommodityKind.BeingCarried,1f);
                                           set.AddCheck(CommodityKind.IsTarget);
                PosturePreconditionOptionsData data=new PosturePreconditionOptionsData();
                                               data.AddOption(set);
                                                instance.PosturePreconditions=new PreconditionOptions(data);
                                         return instance;
            }
            public Definition(string name):base(name){}
            public Definition(){}
            public override bool Test(Sim actor,Sim target,bool isAutonomous,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
            return actor!=null&&target!=null&&actor!=target;
            }
            public override InteractionTestResult Test(ref InteractionInstanceParameters parameters,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
                Sim  actor1=parameters. Actor as Sim;
                Sim target1=parameters.Target as Sim;
                if((object) actor1==null)
            return InteractionTestResult.Root_Null_Actor;
                if((object)target1==null)
            return InteractionTestResult.Root_Null_Target;
                Sim actor2=(object)actor1 as Sim;
                IGameObject target2=(IGameObject)target1;
                for(IGameObject gameObject=(IGameObject)target1;gameObject!=null;gameObject=gameObject.Parent){
                    if(gameObject==LiveDragHelperModel.CachedTopDraggedObject)
            return InteractionTestResult.Root_TargetOnHandTool;
                }
                bool flag=true;
                InteractionTuning tuning=parameters.InteractionObjectPair.Tuning;
                Tradeoff mTradeoff = tuning?.mTradeoff;
                if(tuning!=null){
                    CommodityKind workMotive=actor1.WorkMotive;
                    if(workMotive!=CommodityKind.None&&mTradeoff.SatisfiesCommodity(workMotive))
                     flag=false;
                    if(parameters.Autonomous){
                        if(tuning.HasFlags(InteractionTuning.FlagField.DisallowAutonomous)&&flag)
            return InteractionTestResult.Tuning_DisallowAutonomous;
                        if(mTradeoff.FunExit&&actor1.Motives.FunInteractionTest(false))
            return InteractionTestResult.Tuning_FunInteractionTest;
                    }else if(tuning.HasFlags(InteractionTuning.FlagField.DisallowUserDirected))
            return InteractionTestResult.Tuning_DisallowUserDirected;
                    if(actor1.IsSelectable&&tuning.HasFlags(InteractionTuning.FlagField.DisallowPlayerSim))
            return InteractionTestResult.Tuning_DisallowPlayerSim;
                  if(flag){
                                                    AutonomyFix fix=new AutonomyFix(actor1.Autonomy.mActor,actor1.Autonomy.Motives,actor1.Autonomy.CurrentSearchType,actor1.Autonomy.IsActorInTombRoom);
                    InteractionTestResult interactionTestResult=fix.CheckAvailability1(parameters.Autonomous,tuning.Availability,parameters.InteractionObjectPair);
                    if(interactionTestResult!=InteractionTestResult.Pass)
            return interactionTestResult;
                  }
                }
                actor1.Autonomy.UpdateCacheIfNeeded((IGameObject)target1);
               var interactionTestResult1=AutonomyFix.CommonTests1((InteractionDefinition)this,actor2,target2,parameters);
                if(interactionTestResult1!=InteractionTestResult.Pass)
            return interactionTestResult1;
               var interactionTestResult2=!(this is IMetaInteractionDefinition)?    InteractionDefinitionUtilities.SpecialCaseTests((InteractionDefinition)this,actor2,target2,parameters) 
                                                                               :MetaInteractionDefinitionUtilities.SpecialCaseTests(actor2,target2,parameters);
                if(interactionTestResult2!=InteractionTestResult.Pass)
            return interactionTestResult2;
                if(!this.Test(actor1,target1,parameters.Autonomous,ref greyedOutTooltipCallback))
            return InteractionTestResult.Def_TestFailed;
                if(tuning!=null){
 InteractionTestResult interactionTestResult3=actor1.Autonomy.CheckAvailabilityTooltip((InteractionDefinition)this,(IGameObject)target1,tuning.Availability,parameters,mTradeoff,ref greyedOutTooltipCallback);
                    if(interactionTestResult3!=InteractionTestResult.Pass)
                return interactionTestResult3;
                }
 InteractionTestResult interactionTestResult4=InteractionDefinitionUtilities.SpecialCaseTooltipTests((InteractionDefinition)this,actor2,target2,parameters,ref greyedOutTooltipCallback);
                return interactionTestResult4!=InteractionTestResult.Pass?interactionTestResult4:InteractionTestResult.Pass;
            }
        }
        public static bool StartInteractionWithCarriedChild1(SocialInteraction interactionA,string receptiveInteractionNameKey){
            SocialInteractionB instance=new CarriedChildInteractionBFix.Definition(ChildUtils.Localize(interactionA.Target.IsFemale,receptiveInteractionNameKey)).CreateInstance((IGameObject)interactionA.Actor,(IActor)interactionA.Target,interactionA.GetPriority(),interactionA.EffectivelyAutonomous,interactionA.CancellableByPlayer) as SocialInteractionB;
                    return StartInteractionWithCarriedChild1(interactionA,instance);
        }
        public static bool StartInteractionWithCarriedChild1(SocialInteraction interactionA,SocialInteractionB interactionB){
            interactionB.LinkedInteractionInstance=(InteractionInstance)interactionA;
            interactionA.Target.InteractionQueue.Add((InteractionInstance)interactionB);
            if(!interactionB.CancelNonBSocialInteractionsFromQueue(interactionB.Actor)){
        return false;
            }
            interactionA.SetInitialRouteComplete();
            while(interactionA.Target.CurrentInteraction!=interactionB){
                if(interactionA.InstanceActor.HasExitReason()){
        return false;
                }
                if(!interactionA.Target.InteractionQueue.HasInteraction((InteractionInstance)interactionB)){
        return false;
                }
                if(!interactionA.Test()){
        return false;
                }
Simulator.Sleep(0U);
            }
            interactionA.Actor.SynchronizationRole=Sim.SyncRole.Initiator;
            interactionA.Actor.SynchronizationTarget=interactionA.Target;
            interactionA.Actor.SynchronizationLevel=Sim.SyncLevel.Started;
         SimFix fix=new SimFix(interactionA.Actor);
            if(!fix.WaitForSynchronizationLevelWithSim1(interactionA.Target,Sim.SyncLevel.Started,10f)){
            interactionA.Actor.ClearSynchronizationData();
        return false;
            }
            interactionA.Actor.SynchronizationLevel=Sim.SyncLevel.Committed;
        SimFix fix2=new SimFix(interactionA.Actor);
            if(fix2.WaitForSynchronizationLevelWithSim1(interactionA.Target,Sim.SyncLevel.Committed,10f)){
        return true;
            }
            interactionA.Actor.ClearSynchronizationData();
        return false;
        }
    }
    public class SocialInteractionBFix:SocialInteractionB,IPreLoad,IAddInteraction{
                                              public void AddInteraction(InteractionInjectorList interactions){
                                              }
                                   public void OnPreLoad(){
                                   }
        public override bool Test(){
            if(this.LinkedInteractionInstance==null||this.LinkedInteractionInstance.Test()){
        return Test1();
            }
            InteractionInstance currentInteraction=this.LinkedInteractionInstance.InstanceActor.CurrentInteraction;
            InteractionInstance interactionInstance=this.LinkedInteractionInstance;
        return false;
        }
        public          bool Test1(){
            InteractionInstanceParameters interactionParameters=this.GetInteractionParameters();
            GreyedOutTooltipCallback greyedOutTooltipCallback=(GreyedOutTooltipCallback)null;
        return IUtil.IsPass(this.InteractionDefinition.Test(ref interactionParameters,ref greyedOutTooltipCallback));
        }
        public override bool Run(){
            bool succeeded=false;
            if(!this.StartSlaveInteraction()){
        return false;
            }
this.Actor.OnSocializedWith();
            if(this.LinkedInteractionInstance==null||!this.Target.InteractionQueue.HasInteraction(this.LinkedInteractionInstance)&&this.Target.InteractionQueue.TransitionInteraction!=this.LinkedInteractionInstance){
        return false;
            }
          bool flag=!this.Target.IsRiding(this.Actor)||!this.ActionData.SocialCanBeDoneToMounedHorse//  "Mounted"
                    ?this.ReceiveSocialInteraction1()//  Toddler: Receive Interaction
                    :this.StartSync3(false,false,(SyncLoopCallbackFunction)null,0.0f,true);//  Riding Horse 
            this.AddMotiveArrow(CommodityKind.Social,true);
this.BeginCommodityUpdates();
            if(flag){
                if(this.LinkedInteractionInstance is SocialInteractionA)this.Actor.EnterSocializingPosture(this.Target);
                 succeeded=this.DoLoop(ExitReason.Default);
            }else{
                this.Actor.AddExitReason(ExitReason.RouteFailed);
            }
            this.CopyExitReasonToLinkedInteraction();
            this.WaitForMasterInteractionToFinish();
this.EndCommodityUpdates(succeeded);
            this.WaitForSyncComplete();
        return succeeded;
        }
        public bool ReceiveSocialInteraction1(){
            this.mResultCode=SocialInteraction.SocialResultCode.None;
            Sim target=this.Target;
            Sim actor=this.Actor;
                actor.SynchronizationLevel=Sim.SyncLevel.Started;
                actor.SynchronizationTarget=target;
                actor.SynchronizationRole=Sim.SyncRole.Receiver;
            this.mbIsTurnToFace=false;
            bool flag1=false;
              SocialInteraction interactionInstance1=this.LinkedInteractionInstance as SocialInteraction;
            InteractionInstance interactionInstance2=target.InteractionQueue.TransitionInteraction??target.CurrentInteraction;
            if(interactionInstance1!=null&&interactionInstance1!=interactionInstance2){
                if(interactionInstance2 is ISyncViaGroupId){
                    if((long)interactionInstance1.GroupId!=(long)interactionInstance2.GroupId){
                        this.mResultCode=SocialInteraction.SocialResultCode.PathPlanToToSocialTargetFailed;
        return false;
                    }
                }else if(!interactionInstance1.PartOfGoHereSituation&&!this.PartOfGoHereSituation){
                    this.mResultCode=SocialInteraction.SocialResultCode.PathPlanToToSocialTargetFailed;
        return false;
                }
            }
                actor.LoopIdle();
            bool flag2=false;
            bool flag3=false;
            DateAndTime previousDateAndTime1=SimClock.CurrentTime();
            while(!interactionInstance1.mbInitialRoute){
                if((double)SimClock.ElapsedTime(TimeUnit.Minutes,previousDateAndTime1)>=(double)SocialInteraction.kSocialTimeoutTime){
                    this.mResultCode=SocialInteraction.SocialResultCode.PathPlanToToSocialTargetFailed;
        return false;
                }
                if(target.HasExitReason(ExitReason.Default)){
                    this.mResultCode=SocialInteraction.SocialResultCode.PathPlanToToSocialTargetFailed;
        return false;
                }
                if(actor.HasExitReason(ExitReason.Default)){
                    this.mResultCode=SocialInteraction.SocialResultCode.PathPlanToToSocialTargetFailed;
        return false;
                }
                if(interactionInstance1.Cancelled){
                    this.mResultCode=SocialInteraction.SocialResultCode.PathPlanToToSocialTargetFailed;
        return false;
                }
Simulator.Sleep(0U);
            }
            bool flag4=true;
            if(this.SocialJig!=null){
                if(Sims3.SimIFace.Objects.IsValid(this.SocialJig.ObjectId)&&this.SocialJig.EnsurePlacement(target,actor)){
                    Vector3 routeFromPoint=Vector3.Invalid;
                    if(target.IsRouting&&target.RoutingComponent.GetCurrentRoute()!=null)
                            routeFromPoint=target.RoutingComponent.GetCurrentRoute().GetDestPoint();
                        Sims3.SimIFace.Route jigA=this.SocialJig.RouteToJigA(target,actor.ObjectId,routeFromPoint);
                        Sims3.SimIFace.Route jigB=this.SocialJig.RouteToJigB(actor,target.ObjectId);
                        if(jigA.PlanResult.Succeeded()&&jigB.PlanResult.Succeeded()){
                            interactionInstance1.mRoute=jigA;
                            this.mRoute=jigB;
                 flag1=true;
                        }else{
                            this.mRoute=interactionInstance1.mRoute=(Sims3.SimIFace.Route)null;
                 flag4=false;
                        }
                }else{
                    this.mRoute=interactionInstance1.mRoute=(Sims3.SimIFace.Route)null;
                 flag4=false;
                }
            }else if(actor.Posture is RidingPosture posture){
                if(posture.Container is Sim container){
                    Sims3.SimIFace.Route routeTurnToFace=container.CreateRouteTurnToFace(target.Position);
                                         routeTurnToFace.ExecutionFromNonSimTaskIsSafe=true;
                       container.DoRoute(routeTurnToFace);
                }
            }else if(actor.SimDescription.Baby){
                if(!actor.Posture.Satisfies(CommodityKind.BeingCarried,(IGameObject)null))
                 flag4=this.SetupTwoPersonSocialWithBaby(target,actor,interactionInstance1);
            }else if(actor.Posture is SwimmingInPool||actor.Posture.AllowsRouting()){
                Conversation conversation=actor.Conversation;
                             conversation?.DiscourageRegion.Disable();
                List<Sim>simList=new List<Sim>();
                if(actor.Conversation!=null)
                         simList.AddRange((IEnumerable<Sim>)actor.Conversation.Members);
                     if(!simList.Contains(target))
                         simList.Add(     target);
                     if(!simList.Contains(actor))
                         simList.Add(     actor);
                if(this.mbPairedSocial||!SocialInteraction.sGroupConversationRoutingEnabled||simList.Count<=2){
                 flag2=this.SimsNeedToRoute(target,actor);
                 flag3=!flag2;
                }
              if(flag2)
                 flag4=this.SetupTwoPersonSocial(target,actor,interactionInstance1);
              else if(actor.Conversation!=null&&!flag3)
                 flag4=this.SetupGroupSocial(target,actor,interactionInstance1);
                             conversation?.DiscourageRegion.Enable();
            }
            if(this.mRoute!=null){
                if(actor.SimDescription.ToddlerOrAbove){
                    if(this.mbIsTurnToFace&&interactionInstance1.mbIsTurnToFace){
                        actor.SynchronizationLevel=Sim.SyncLevel.Routed;
                        this.mbBRoutePlanned=true;
                        DateAndTime previousDateAndTime2=SimClock.CurrentTime();
                        while(!interactionInstance1.mbApproachRoute){
                            if((double)SimClock.ElapsedTime(TimeUnit.Minutes,previousDateAndTime2)>=(double)SocialInteraction.kSocialTimeoutTime){
                                this.mResultCode=SocialInteraction.SocialResultCode.PathPlanToToSocialTargetFailed;
        return false;
                            }
                            if(target.HasExitReason(ExitReason.Default)){
                                this.mResultCode=SocialInteraction.SocialResultCode.PathPlanToToSocialTargetFailed;
        return false;
                            }
                            if(actor.HasExitReason(ExitReason.Default)){
                                this.mResultCode=SocialInteraction.SocialResultCode.PathPlanToToSocialTargetFailed;
        return false;
                            }
                            if(interactionInstance1.Cancelled){
                                this.mResultCode=SocialInteraction.SocialResultCode.PathPlanToToSocialTargetFailed;
        return false;
                            }
Simulator.Sleep(0U);
                        }
                    }else 
              if(flag1){
                        actor.SynchronizationLevel=Sim.SyncLevel.Routed;
                        this.mbBRoutePlanned=true;
Simulator.Sleep(0U);
              }
                 flag4=actor.DoRoute(this.mRoute);
                actor.LoopIdle();
                }
                this.mRoute=(Sims3.SimIFace.Route)null;
            }
            this.mbIsTurnToFace = false;
              if(flag4){
                actor.SynchronizationLevel=Sim.SyncLevel.Routed;
                this.mbBRoutePlanned=true;
            SimFix fix=new SimFix(actor);
                if(fix.WaitForSynchronizationLevelWithSim1(target,Sim.SyncLevel.Routed,SocialInteraction.kSocialSyncGiveupTime)&&flag2){
                    interactionInstance1.mbValidateFacing=true;
                    if(!this.SimsAreFacing(target,actor)){
                 flag4=actor.RoutingComponent.RouteTurnToFace(target.PositionOnFloor);
                        actor.LoopIdle();
                    }
                }
              }
              if(flag4){
                actor.PopCanePostureIfNecessary();
                if(!(interactionInstance1 is IUmbrellaSupportedSocial))
                    Umbrella.PopUmbrellaPostureIfNecessary(actor,false);
                actor.PopBackpackPostureIfNecessary();
                if(!(interactionInstance1 is IJetpackInteraction))
                actor.PopJetpackPostureIfNecessary();
                actor.SynchronizationLevel=Sim.SyncLevel.Committed;
                SimFix fix=new SimFix(actor);
            bool flag5=fix.WaitForSynchronizationLevelWithSim1(target,Sim.SyncLevel.Committed,SocialInteraction.kSocialSyncGiveupTime);
                this.mResultCode=!flag5?(!actor.HasExitReason()?SocialInteraction.SocialResultCode.SyncToSocialTargetTimedOut:SocialInteraction.SocialResultCode.CanceledByExitReason):SocialInteraction.SocialResultCode.Succeeded;
          return flag5;
              }
            this.mResultCode=SocialInteraction.SocialResultCode.PathPlanToToSocialTargetFailed;
        return false;
        }
        public bool StartSync3(bool shouldBeMaster,bool ignoreExitReasons,SyncLoopCallbackFunction loopCallback,float notifySimMinutes,bool performSocializeWithTest){
            if(!this.SafeToSync()){
        return false;
            }
            Sim syncTarget=this.GetSyncTarget();
            if(syncTarget==null){
        return false;
            }
            ExitReason exitReason=ignoreExitReasons?ExitReason.None:ExitReason.Default;
            DateAndTime previousDateAndTime1=SimClock.CurrentTime();
            DateAndTime previousDateAndTime2=previousDateAndTime1;
            while((double)SimClock.ElapsedTime(TimeUnit.Minutes,previousDateAndTime1)<(double)InteractionInstance.kNumMinToWaitOnPreSync&&(this.GetTargetCurrentInteraction()==null||this.GetTargetCurrentInteraction().LinkedInteractionInstance!=this)){
                if(this.InstanceActor.HasExitReason(exitReason)){
        return false;
                }
                bool flag=false;
                if(syncTarget.InteractionQueue.IsRunning(this.LinkedInteractionInstance,true)){
                     flag=true;
                }else{
                    foreach(InteractionInstance interaction in syncTarget.InteractionQueue.InteractionList){
                        if(interaction.LinkedInteractionInstance==this){
                     flag=true;
                            break;
                        }
                    }
                }
                if(syncTarget.InteractionQueue.GetHeadInteraction() is IPreventSocialization headInteraction&&headInteraction!=null&&!headInteraction.SocializationAllowed(this.InstanceActor,syncTarget)||!flag){
        return false;
                }
                if(loopCallback!=null&&(double)SimClock.ElapsedTime(TimeUnit.Minutes,previousDateAndTime2)>=(double)notifySimMinutes){
                    if(!loopCallback()){
        return false;
                    }
                    previousDateAndTime2=SimClock.CurrentTime();
                }
Simulator.Sleep(0U);
            }
            this.InstanceActor.SynchronizationRole=shouldBeMaster?Sim.SyncRole.Initiator:Sim.SyncRole.Receiver;
            this.InstanceActor.SynchronizationTarget=syncTarget;
            this.InstanceActor.SynchronizationLevel=Sim.SyncLevel.Started;
         SimFix fix=new SimFix(this.InstanceActor);
            if(!fix.WaitForSynchronizationLevelWithSim1(syncTarget,Sim.SyncLevel.Started,exitReason,(float)InteractionInstance.kNumMinToWaitOnSyncStart,loopCallback,notifySimMinutes,performSocializeWithTest)){
        return false;
            }
        return true;}
        public new class NoAgeOrClosedVenueCheckDefinition:SocialInteractionBFix.NoAgeCheckDefinition,IAllowedOnClosedVenues{
                  public NoAgeOrClosedVenueCheckDefinition(string name,bool allowCarryChild):base(name,allowCarryChild){}
                  public NoAgeOrClosedVenueCheckDefinition(){}
        }
        public new class NoAgeCheckDefinition:SocialInteractionBFix.Definition,IOverridesAgeTests{
                  public NoAgeCheckDefinition(string name,bool allowCarryChild):base((string)null,name,allowCarryChild){}
                  public NoAgeCheckDefinition(){ }
            public SpecialCaseAgeTests GetSpecialCaseAgeTests(){
            return SpecialCaseAgeTests.None;
            }
        }
        public new class Definition:SocialInteractionB.Definition{
                  public Definition(string key,string name,bool allowCarryChild){
                                  this.Key=key;
                                            this.Name=name;
                                           this.AllowCarryChild=allowCarryChild;
                  }
                  public Definition(){}
            public          InteractionInstance CreateInstance0(ref InteractionInstanceParameters parameters){
                            InteractionInstance interaction=new SocialInteractionBFix();
                                                interaction.Init(ref parameters);
                     return(InteractionInstance)interaction;
            }
            public override InteractionInstance CreateInstance(ref InteractionInstanceParameters parameters){
                            InteractionInstance instance=CreateInstance0(ref parameters);
                if(this.AllowCarryChild){
                    PosturePreconditionSetData set1=new PosturePreconditionSetData(CommodityKind.CarryingChild  ,0.0f);
                    PosturePreconditionOptionsData data=new PosturePreconditionOptionsData();
                                                   data.AddOption(set1);
                    PosturePreconditionSetData set2=new PosturePreconditionSetData(CommodityKind.CarryingPet    ,0.0f);
                                                   data.AddOption(set2);
                    PosturePreconditionSetData set3=new PosturePreconditionSetData(CommodityKind.CarryingHoloPet,0.0f);
                                                   data.AddOption(set3);
                    if(instance.PosturePreconditions==null){
                    PosturePreconditionSetData set4=new PosturePreconditionSetData(CommodityKind.Standing       ,1f  );
                                                   data.AddOption(set4);
                    }else{
                       foreach(PreconditionSet set4 in instance.PosturePreconditions.Sets){
                    PosturePreconditionSetData set5=new PosturePreconditionSetData(set4.Commodity,set4.Value);
                            foreach(CommodityKind condition in set4.Conditions)
                                               set5.AddCheck(condition);
                                            if(set5.Commodity==CommodityKind.PostureSocializing)
                                               set5.AddCheck(CommodityKind.MayCarryChild);
                                data.AddOption(set5);
                       }
                    }
                                                instance.PosturePreconditions=new PreconditionOptions(data);
                }
                                         return instance;
            }
        }
    }
    public class PutChildInCribFix:PutChildInCrib,IPreLoad,IAddInteraction{
        static InteractionDefinition sOldSingleton;
                                              public void AddInteraction(InteractionInjectorList interactions){
                                                                                                 interactions.ReplaceNoTest<Sim,PutChildInCrib.Definition>(Singleton);
                                              }
                                   public void OnPreLoad(){
            Tunings.Inject<Sim,PutChildInCrib.Definition,Definition>(false);
                                     sOldSingleton=Singleton;
                                                   Singleton=new Definition();
                                   }
        public override bool Run(){
            ICrib[]objects=Sims3.Gameplay.Queries.GetObjects<ICrib>(this.Target.LotCurrent);
            float num1=float.MaxValue;
            ICrib crib1=(ICrib)null;
            if(objects.Length==1){
                if(objects[0].IsEmpty())
                    crib1=objects[0];
            }else{
                foreach(ICrib crib2 in objects){
                    if(crib2.IsEmpty()){
                        float num2=(crib2.Position-this.Target.Position).LengthSqr();
                        if((double)num2<(double)num1){
                            num1=num2;
                            crib1=crib2;
                        }
                    }
                }
            }
            if(crib1==null)
        return false;
this.BeginCommodityUpdates();
            this.Actor.InteractionQueue.PushAsContinuation(crib1.PutInCrib(this.Actor),true);
this.EndCommodityUpdates(true);
        return true;
        }
        public new class Definition:PutChildInCrib.Definition{
            public override InteractionInstance CreateInstance(ref InteractionInstanceParameters parameters){
                            InteractionInstance na=new PutChildInCribFix();
                                                na.Init(ref parameters);
                                         return na;
            }
            public override InteractionTestResult Test(ref InteractionInstanceParameters parameters,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
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
                    try{
                                     AutonomyFix fix=new AutonomyFix(actor1.Autonomy.mActor,actor1.Autonomy.Motives,actor1.Autonomy.CurrentSearchType,actor1.Autonomy.IsActorInTombRoom);
     InteractionTestResult interactionTestResult=fix.CheckAvailability1(parameters.Autonomous,tuning.Availability,parameters.InteractionObjectPair);
                        if(interactionTestResult!=InteractionTestResult.Pass){
            return(interactionTestResult);
                        }
                    }catch(Exception exception){
         //  Get stack trace for the exception. with source file information
               var st=new StackTrace(exception,true);
         //  Get the top stack frame
         var frame=st.GetFrame(0);
         //  Get the line number from the stack frame
    var line=frame.GetFileLineNumber();
                      Alive.WriteLog(exception.Message+"\n\n"+
                                     exception.StackTrace+"\n\n"+
                                     exception.Source+"\n\n"+
                                     line);
            return(InteractionTestResult.GenericUnknown);
                    }finally{
                    }
                    }
                }
                actor1.Autonomy.UpdateCacheIfNeeded((IGameObject)target1);
                    try{
               var interactionTestResult1=AutonomyFix.CommonTests1((InteractionDefinition)this,actor2,target2,parameters);
                if(interactionTestResult1!=InteractionTestResult.Pass){
            return(interactionTestResult1);
                }
                    }catch(Exception exception){
         //  Get stack trace for the exception. with source file information
               var st=new StackTrace(exception,true);
         //  Get the top stack frame
         var frame=st.GetFrame(0);
         //  Get the line number from the stack frame
    var line=frame.GetFileLineNumber();
                      Alive.WriteLog(exception.Message+"\n\n"+
                                     exception.StackTrace+"\n\n"+
                                     exception.Source+"\n\n"+
                                     line);
            return(InteractionTestResult.GenericUnknown);
                    }finally{
                    }
               var interactionTestResult2=!(this is IMetaInteractionDefinition)?    InteractionDefinitionUtilities.SpecialCaseTests((InteractionDefinition)this,actor2,target2,parameters)
                                                                               :MetaInteractionDefinitionUtilities.SpecialCaseTests(actor2,target2,parameters);
                if(interactionTestResult2!=InteractionTestResult.Pass){
            return(interactionTestResult2);
                }
                if(!this.Test(actor1,target1,parameters.Autonomous,ref greyedOutTooltipCallback)){
            return(InteractionTestResult.Def_TestFailed);
                }
                if(tuning!=null){
               var interactionTestResult3=actor1.Autonomy.CheckAvailabilityTooltip((InteractionDefinition)this,(IGameObject)target1,tuning.Availability,parameters,mTradeoff,ref greyedOutTooltipCallback);
                if(interactionTestResult3!=InteractionTestResult.Pass){
            return(interactionTestResult3);
                }
                }
               var interactionTestResult4=InteractionDefinitionUtilities.SpecialCaseTooltipTests((InteractionDefinition)this,actor2,target2,parameters,ref greyedOutTooltipCallback);
                if(interactionTestResult4!=InteractionTestResult.Pass){
            return(interactionTestResult4);
                }
            return(InteractionTestResult.Pass);
            }
        }
    }
    public class PutCarriedChildInCribFix:Crib.PutCarriedChildInCrib,IPreLoad,IAddInteraction{
        static InteractionDefinition sOldSingleton;
                                              public void AddInteraction(InteractionInjectorList interactions){
                                                                                                 interactions.ReplaceNoTest<Crib,Crib.PutCarriedChildInCrib.Definition>(Singleton);
                                              }
                                   public void OnPreLoad(){
            Tunings.Inject<Sims3.Gameplay.Objects.Beds.Mimics.CribCheap    ,Crib.PutCarriedChildInCrib.Definition,Definition>(false);
            Tunings.Inject<Sims3.Gameplay.Objects.Beds.Mimics.CribExpensive,Crib.PutCarriedChildInCrib.Definition,Definition>(false);
            Tunings.Inject<Crib,Crib.PutCarriedChildInCrib.Definition,Definition>(false);
                                     sOldSingleton=Singleton;
                                                   Singleton=new Definition();
                                   }
        public override bool Run(){
            if(this.Actor.CarryingChildPosture==null)
        return false;
            Sim child=this.Actor.CarryingChildPosture.Child;
            if(!this.mTriedChangingClothes&&child.SimDescription.Toddler&&child.CurrentOutfitCategory!=OutfitCategories.Sleepwear){
                this.mTriedChangingClothes=true;
                this.mChangeToddlerClothes=ChangeToddlerClothes.Singleton.CreateInstance((IGameObject)child,(IActor)this.Actor,this.GetPriority(),false,this.CancellableByPlayer)as ChangeToddlerClothes;
                this.mChangeToddlerClothes.Reason=Sim.ClothesChangeReason.GoingToBed;
                int num=(int)this.mChangeToddlerClothes.StartBuildingOutfit();
            }
            if(!this.Actor.RouteToSlotAndCheckInUse((IGameObject)this.Target,this.Target.GetRoutingSlot(child)))
        return false;
            if(this.mChangeToddlerClothes!=null&&this.Actor.InteractionQueue.PushAsContinuation((InteractionInstance)this.mChangeToddlerClothes,true)){
                this.Actor.InteractionQueue.PushAsContinuation((InteractionInstance)this,true);
                this.mChangeToddlerClothes=(ChangeToddlerClothes)null;
        return true;
            }
            if(!StartObjectInteractionWithChild1((InteractionInstance)this,child,CommodityKind.BeingCarried,"BePutInCrib"))
        return false;
this.StandardEntry();
            this.Target.AddToUseList(child);
            StateMachineClient cribStateMachine=StateMachineClient.Acquire((IHasScriptProxy)child,nameof(Crib));
                               cribStateMachine.SetActor("x",(IHasScriptProxy)this.Actor);
                               cribStateMachine.SetActor("y",(IHasScriptProxy)child);
                               cribStateMachine.SetActor("crib",(IHasScriptProxy) this.Target);
                               cribStateMachine.SetParameter("ExitFacingCrib",this.Actor.InteractionQueue.GetHeadInteraction()is Crib.Crib_ReadToSleep);
                               cribStateMachine.EnterState("x","EnterCarrying");
                               cribStateMachine.EnterState("y","EnterCarrying");
this.BeginCommodityUpdates();
            if(child.SimDescription.Age==CASAgeGenderFlags.Toddler){
                child.UnParent();
                child.SetPosition(this.Target.GetPositionOfSlot(Slot.ContainmentSlot_1));
                child.SetForward(this.Target.GetForwardOfSlot(Slot.ContainmentSlot_1));
            }
                               cribStateMachine.RequestState(false,"x","PutDown");
                               cribStateMachine.RequestState(true,"y","PutDown");
                               cribStateMachine.RequestState(false,"x","ExitStanding");
                               cribStateMachine.RequestState(true,"y","InCrib");
            if(child.SimDescription.Age==CASAgeGenderFlags.Baby&&child.Parent!=this.Target)
                child.ParentToSlot((IGameObject)this.Target,Slot.ContainmentSlot_1);
            StuffedAnimal stuffedAnimal=child.Inventory.Find<StuffedAnimal>();
            if(stuffedAnimal!=null&&!stuffedAnimal.InUse&&this.Target.StuffedAnimal==null)
                this.Target.ParentStuffedAnimal((IStuffedAnimal)stuffedAnimal,true);
this.EndCommodityUpdates(true);
            Crib.InCribPosture inCribPosture=new Crib.InCribPosture(child,this.Target,cribStateMachine);
            child.Posture=(Posture)inCribPosture;
            this.Actor.Posture=this.Actor.Standing;
            child.BridgeOrigin=child.Posture.Idle();
            InteractionInstance headInteraction=this.Actor.InteractionQueue.GetHeadInteraction();
            if(headInteraction==null||headInteraction.Target!=child){
                if(child.BuffManager.HasAnyElement(BuffNames.Sleepy,BuffNames.Tired,BuffNames.Exhausted)&&!child.InteractionQueue.HasInteractionOfType(Sims3.Gameplay.ActorSystems.Children.Sleep.Singleton)){
                           InteractionInstance instance=Sims3.Gameplay.ActorSystems.Children.Sleep.Singleton.CreateInstance((IGameObject)child,(IActor)child,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,false);
                    child.InteractionQueue.Add(instance);
                }
            }
                               cribStateMachine.RemoveActor("x");
this.StandardExit();
        return ChildUtils.FinishObjectInteractionWithChild((InteractionInstance)this,child);
        }
        public static bool StartObjectInteractionWithChild1(InteractionInstance interactionA,Sim child,CommodityKind childPosture,string receptiveInteractionNameKey){
            if(child==null||child.HasBeenDestroyed||!interactionA.SafeToSync()){
                Sim instanceActor0=interactionA.InstanceActor;
        return false;
            }
            Sim instanceActor=interactionA.InstanceActor;
            InteractionInstance instance=new ChildPlaceholderInteractionFix.Definition(ChildUtils.Localize(child.IsFemale,receptiveInteractionNameKey)).CreateInstance((IGameObject)instanceActor,(IActor)child,interactionA.GetPriority(),interactionA.Autonomous,interactionA.CancellableByPlayer);
                                instance.LinkedInteractionInstance=interactionA;
            ChildUtils.SetPosturePrecondition(instance,childPosture);
            if(!child.InteractionQueue.Add(instance)){
        return false;
            }
            if(!(interactionA is PutCarriedChildInCribFix putCarriedChildInCrib)||putCarriedChildInCrib==null||!putCarriedChildInCrib.StartSync1(true,false,(SyncLoopCallbackFunction)null,0.0f,true)){
        return false;
            }
            instanceActor.SynchronizationLevel=Sim.SyncLevel.Committed;
        SimFix fix=new SimFix(instanceActor);
            if(fix.WaitForSynchronizationLevelWithSim1(child,Sim.SyncLevel.Committed,SocialInteraction.kSocialSyncGiveupTime))
        return true;
            instanceActor.ClearSynchronizationData();
        return false;}
        public bool StartSync1(bool shouldBeMaster,bool ignoreExitReasons,SyncLoopCallbackFunction loopCallback,float notifySimMinutes,bool performSocializeWithTest){
            if(!this.SafeToSync()){
        return false;
            }
            Sim syncTarget=this.GetSyncTarget();
            if(syncTarget==null){
        return false;
            }
            ExitReason exitReason=ignoreExitReasons?ExitReason.None:ExitReason.Default;
            DateAndTime previousDateAndTime1=SimClock.CurrentTime();
            DateAndTime previousDateAndTime2=previousDateAndTime1;
            while((double)SimClock.ElapsedTime(TimeUnit.Minutes,previousDateAndTime1)<(double)InteractionInstance.kNumMinToWaitOnPreSync&&(this.GetTargetCurrentInteraction()==null||this.GetTargetCurrentInteraction().LinkedInteractionInstance!=this)){
                if(this.InstanceActor.HasExitReason(exitReason)){
        return false;
                }
                bool flag=false;
                if(syncTarget.InteractionQueue.IsRunning(this.LinkedInteractionInstance,true)){
                     flag=true;
                }else{
                    foreach(InteractionInstance interaction in syncTarget.InteractionQueue.InteractionList){
                        if(interaction.LinkedInteractionInstance==this){
                     flag=true;
                            break;
                        }
                    }
                }
                if(syncTarget.InteractionQueue.GetHeadInteraction() is IPreventSocialization headInteraction&&headInteraction!=null&&!headInteraction.SocializationAllowed(this.InstanceActor,syncTarget)||!flag){
        return false;
                }
                if(loopCallback!=null&&(double)SimClock.ElapsedTime(TimeUnit.Minutes,previousDateAndTime2)>=(double)notifySimMinutes){
                    if(!loopCallback()){
        return false;
                    }
                    previousDateAndTime2=SimClock.CurrentTime();
                }
Simulator.Sleep(0U);
            }
            this.InstanceActor.SynchronizationRole=shouldBeMaster?Sim.SyncRole.Initiator:Sim.SyncRole.Receiver;
            this.InstanceActor.SynchronizationTarget=syncTarget;
            this.InstanceActor.SynchronizationLevel=Sim.SyncLevel.Started;
         SimFix fix=new SimFix(this.InstanceActor);
            if(!fix.WaitForSynchronizationLevelWithSim1(syncTarget,Sim.SyncLevel.Started,exitReason,(float)InteractionInstance.kNumMinToWaitOnSyncStart,loopCallback,notifySimMinutes,performSocializeWithTest)){
        return false;
            }
        return true;}
        public new class Definition:Crib.PutCarriedChildInCrib.Definition{
            public override InteractionInstance CreateInstance(ref InteractionInstanceParameters parameters){
                            InteractionInstance na=new PutCarriedChildInCribFix();
                                                na.Init(ref parameters);
                                         return na;
            }
            public override InteractionTestResult Test(ref InteractionInstanceParameters parameters,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
                Sim   actor1=parameters. Actor as Sim;
                Crib target1=parameters.Target as Crib;
                if((object) actor1==null)
            return InteractionTestResult.Root_Null_Actor;
                if((object)target1==null)
            return InteractionTestResult.Root_Null_Target;
                Sim actor2=(object)actor1 as Sim;
                IGameObject target2=(IGameObject)target1;
                for(IGameObject gameObject=(IGameObject)target1;gameObject!=null;gameObject=gameObject.Parent){
                    if(gameObject==LiveDragHelperModel.CachedTopDraggedObject)
            return InteractionTestResult.Root_TargetOnHandTool;
                }
                bool flag=true;
                InteractionTuning tuning=parameters.InteractionObjectPair.Tuning;
                Tradeoff mTradeoff=tuning?.mTradeoff;
                if(tuning!=null){
                    CommodityKind workMotive=actor1.WorkMotive;
                    if(workMotive!=CommodityKind.None&&mTradeoff.SatisfiesCommodity(workMotive))
                     flag=false;
                    if(parameters.Autonomous){
                        if(tuning.HasFlags(InteractionTuning.FlagField.DisallowAutonomous)&&flag)
            return InteractionTestResult.Tuning_DisallowAutonomous;
                        if(mTradeoff.FunExit&&actor1.Motives.FunInteractionTest(false))
            return InteractionTestResult.Tuning_FunInteractionTest;
                    }else if(tuning.HasFlags(InteractionTuning.FlagField.DisallowUserDirected))
            return InteractionTestResult.Tuning_DisallowUserDirected;
                    if(actor1.IsSelectable&&tuning.HasFlags(InteractionTuning.FlagField.DisallowPlayerSim))
            return InteractionTestResult.Tuning_DisallowPlayerSim;
                  if(flag){
                                     AutonomyFix fix=new AutonomyFix(actor1.Autonomy.mActor,actor1.Autonomy.Motives,actor1.Autonomy.CurrentSearchType,actor1.Autonomy.IsActorInTombRoom);
     InteractionTestResult interactionTestResult=fix.CheckAvailability1(parameters.Autonomous,tuning.Availability,parameters.InteractionObjectPair);
                        if(interactionTestResult!=InteractionTestResult.Pass)
            return interactionTestResult;
                  }
                }
                actor1.Autonomy.UpdateCacheIfNeeded((IGameObject) target1);
               var interactionTestResult1=AutonomyFix.CommonTests1((InteractionDefinition)this,actor2,target2,parameters);
                if(interactionTestResult1!=InteractionTestResult.Pass)
            return interactionTestResult1;
               var interactionTestResult2=!(this is IMetaInteractionDefinition)?    InteractionDefinitionUtilities.SpecialCaseTests((InteractionDefinition)this,actor2,target2,parameters)
                                                                               :MetaInteractionDefinitionUtilities.SpecialCaseTests(actor2,target2,parameters);
                if(interactionTestResult2!=InteractionTestResult.Pass)
            return interactionTestResult2;
                if(!this.Test(actor1,target1,parameters.Autonomous,ref greyedOutTooltipCallback))
            return InteractionTestResult.Def_TestFailed;
                if(tuning!=null){
 InteractionTestResult interactionTestResult3=actor1.Autonomy.CheckAvailabilityTooltip((InteractionDefinition)this,(IGameObject)target1,tuning.Availability,parameters,mTradeoff,ref greyedOutTooltipCallback);
                    if(interactionTestResult3!=InteractionTestResult.Pass)
            return interactionTestResult3;
                }
               var interactionTestResult4=InteractionDefinitionUtilities.SpecialCaseTooltipTests((InteractionDefinition)this,actor2,target2,parameters,ref greyedOutTooltipCallback);
            return interactionTestResult4!=InteractionTestResult.Pass?interactionTestResult4:InteractionTestResult.Pass;
            }
        }
    }
    public class PutDownChildFix:PutDownChild,IPreLoad,IAddInteraction{
        static InteractionDefinition sOldSingleton;
                                              public void AddInteraction(InteractionInjectorList interactions){
                                                                                                 interactions.ReplaceNoTest<Sim,PutDownChild.Definition>(Singleton);
                                              }
                                   public void OnPreLoad(){
            Tunings.Inject<Sim,PutDownChild.Definition,Definition>(false);
                                     sOldSingleton=Singleton;
                                                   Singleton=new Definition();
                                   }
        public override bool Run(){
            BeingCarriedPosture.PushPutDownOnParent pushPutDownOnParent1=(BeingCarriedPosture.PushPutDownOnParent)null;
            if(!(this.Target.InteractionQueue.TransitionInteraction is BeingCarriedPosture.PushPutDownOnParent pushPutDownOnParent2))
                pushPutDownOnParent2=this.Target.InteractionQueue.GetHeadInteraction() as BeingCarriedPosture.PushPutDownOnParent;
            BeingCarriedPosture.PushPutDownOnParent pushPutDownOnParent3=pushPutDownOnParent2;
            bool flag1=pushPutDownOnParent3!=null&&pushPutDownOnParent3.Target==this.Target&&this.Target.Posture.Container==this.Actor;
              if(flag1){
                if(this.LinkedInteractionInstance!=pushPutDownOnParent3){
                    pushPutDownOnParent3.LinkedInteractionInstance=(InteractionInstance)null;
                    this.LinkedInteractionInstance=(InteractionInstance)pushPutDownOnParent3;
                }
              }else{
                    pushPutDownOnParent1=this.LinkedInteractionInstance as BeingCarriedPosture.PushPutDownOnParent;
                    this.LinkedInteractionInstance=(InteractionInstance)null;
                if(!CarriedChildInteractionBFix.StartInteractionWithCarriedChild1((SocialInteraction)this,"BePutDown")){
                    if(pushPutDownOnParent1!=null){
                        if(this.LinkedInteractionInstance!=null){
                            this.CopyExitReasonToLinkedInteraction();
                            this.LinkedInteractionInstance.InstanceActor.InteractionQueue.CancelInteraction(this.LinkedInteractionInstance,false);
                            this.LinkedInteractionInstance=(InteractionInstance)null;
                        }
                            this.LinkedInteractionInstance=(InteractionInstance)pushPutDownOnParent1;
                    }
                    this.CopyExitReasonToLinkedInteraction();
        return false;
                }
              }
            TerrainType terrainType=World.GetTerrainType(this.Actor.Position);
            for(int index=0;index<PutDownChildHere.kProhibitedTerrainTypes.Length;++index){
                if(terrainType==PutDownChildHere.kProhibitedTerrainTypes[index]){
                    Vector3 invalid=Vector3.Invalid;
                    Quaternion identity=Quaternion.Identity;
                    if(!World.FindPlaceOnRoad(this.Actor.Proxy,this.Actor.Position,2U,ref invalid,ref identity)||!this.Actor.RouteToPoint(invalid)){
        return false;
                    }
                    break;
                }
            }
            this.CreateJig();
            ChildUtils.PutDownClass putDownClass=new ChildUtils.PutDownClass(this.Actor,this.Target,this.PutDownType,this.PutDownTargetLocation,this.mSocialToPushToTarget,this.SocialJig as SocialJigTwoPerson,this.FindGoodLocationStrategies);
       PutDownClassFix fix=new PutDownClassFix(putDownClass);
            if(this.Actor.IsActiveFirefighter&&Occupation.SimIsWorkingAtJobOnLot(this.Actor,this.Target.LotCurrent))
                this.RequestWalkStyle(Sim.WalkStyle.FastJog);
            bool flag2=fix.PutDownChild1();
             if(!flag1&&(this.mSocialToPushToTarget==null||this.LinkedInteractionInstance==null||this.LinkedInteractionInstance.InteractionDefinition!=this.mSocialToPushToTarget)){
                 flag2=ChildUtils.FinishInteractionWithCarriedChild((SocialInteraction)this)&&flag2;
                this.LinkedInteractionInstance=(InteractionInstance)pushPutDownOnParent1;
                this.CopyExitReasonToLinkedInteraction();
             }
          return flag2;
        }
        public new class Definition:PutDownChild.Definition{
            public override InteractionInstance CreateInstance(ref InteractionInstanceParameters parameters){
                            InteractionInstance na=new PutDownChildFix();
                                                na.Init(ref parameters);
                                         return na;
            }
            public override bool Test(Sim a,Sim target,bool isAutonomous,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
            return a!=target&&a.CarryingChildPosture!=null&&a.CarryingChildPosture.Child==target;
            }
            public override InteractionTestResult Test(ref InteractionInstanceParameters parameters,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
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
                    try{
                                     AutonomyFix fix=new AutonomyFix(actor1.Autonomy.mActor,actor1.Autonomy.Motives,actor1.Autonomy.CurrentSearchType,actor1.Autonomy.IsActorInTombRoom);
     InteractionTestResult interactionTestResult=fix.CheckAvailability1(parameters.Autonomous,tuning.Availability,parameters.InteractionObjectPair);
                        if(interactionTestResult!=InteractionTestResult.Pass){
            return(interactionTestResult);
                        }
                    }catch(Exception exception){
         //  Get stack trace for the exception. with source file information
               var st=new StackTrace(exception,true);
         //  Get the top stack frame
         var frame=st.GetFrame(0);
         //  Get the line number from the stack frame
    var line=frame.GetFileLineNumber();
                      Alive.WriteLog(exception.Message+"\n\n"+
                                     exception.StackTrace+"\n\n"+
                                     exception.Source+"\n\n"+
                                     line);
            return(InteractionTestResult.GenericUnknown);
                    }finally{
                    }
                    }
                }
                actor1.Autonomy.UpdateCacheIfNeeded((IGameObject)target1);
                    try{
               var interactionTestResult1=AutonomyFix.CommonTests1((InteractionDefinition)this,actor2,target2,parameters);
                if(interactionTestResult1!=InteractionTestResult.Pass){
            return(interactionTestResult1);
                }
                    }catch(Exception exception){
         //  Get stack trace for the exception. with source file information
               var st=new StackTrace(exception,true);
         //  Get the top stack frame
         var frame=st.GetFrame(0);
         //  Get the line number from the stack frame
    var line=frame.GetFileLineNumber();
                      Alive.WriteLog(exception.Message+"\n\n"+
                                     exception.StackTrace+"\n\n"+
                                     exception.Source+"\n\n"+
                                     line);
            return(InteractionTestResult.GenericUnknown);
                    }finally{
                    }
               var interactionTestResult2=!(this is IMetaInteractionDefinition)?    InteractionDefinitionUtilities.SpecialCaseTests((InteractionDefinition)this,actor2,target2,parameters)
                                                                               :MetaInteractionDefinitionUtilities.SpecialCaseTests(actor2,target2,parameters);
                if(interactionTestResult2!=InteractionTestResult.Pass){
            return(interactionTestResult2);
                }
                if(!this.Test(actor1,target1,parameters.Autonomous,ref greyedOutTooltipCallback)){
            return(InteractionTestResult.Def_TestFailed);
                }
                if(tuning!=null){
               var interactionTestResult3=actor1.Autonomy.CheckAvailabilityTooltip((InteractionDefinition)this,(IGameObject)target1,tuning.Availability,parameters,mTradeoff,ref greyedOutTooltipCallback);
                if(interactionTestResult3!=InteractionTestResult.Pass){
            return(interactionTestResult3);
                }
                }
               var interactionTestResult4=InteractionDefinitionUtilities.SpecialCaseTooltipTests((InteractionDefinition)this,actor2,target2,parameters,ref greyedOutTooltipCallback);
                if(interactionTestResult4!=InteractionTestResult.Pass){
            return(interactionTestResult4);
                }
            return(InteractionTestResult.Pass);
            }
        }
        public class PutDownClassFix{
              public PutDownClassFix(ChildUtils.PutDownClass putDownClass){
                                           this.putDownClass=putDownClass;
              }
                 public ChildUtils.PutDownClass putDownClass;
            public bool PutDownChild1(){
                    try{
                CarryingChildPosture carryingChildPosture=putDownClass.Actor.CarryingChildPosture;
                if(carryingChildPosture==null){
            return false;
                }
                ChildUtils.PutDownClass.Phase phase=ChildUtils.PutDownClass.Phase.Nearby;
                bool flag1=false;
                putDownClass.mPutDownActualLocation=Vector3.Zero;
                putDownClass.mPutDownActualForward=Vector3.UnitZ;
                for(;phase<=ChildUtils.PutDownClass.Phase.Corner3&&!flag1;++phase){
                    putDownClass.Actor.RemoveExitReason(ExitReason.RouteFailed);
                    if(!putDownClass.Actor.HasExitReason(ExitReason.Default)){
                        switch(putDownClass.mPutDownType){
                            case ChildUtils.Where.BestSpot:
                            case ChildUtils.Where.ClosestSpot:
                                putDownClass.mPutDownActualLocation=putDownClass.Actor.Position;
                                putDownClass.mPutDownActualForward=putDownClass.Actor.ForwardVector;
                                break;
                            case ChildUtils.Where.SpecificSpot:
                                putDownClass.mPutDownActualLocation=World.SnapToFloor(putDownClass.mPutDownTargetLocation);
                                putDownClass.mPutDownActualForward=putDownClass.mPutDownActualLocation-putDownClass.Actor.Position;
                                putDownClass.mPutDownActualForward.y=0.0f;
                                putDownClass.mPutDownActualForward.Normalize();
                                break;
                        }
                        if(putDownClass.Actor.LotCurrent.IsFireOnLot()&&!putDownClass.Actor.IsActiveFirefighter){
                            Route route=putDownClass.Actor.CreateRoute();
                                  route.PlanToPointRadialRange(putDownClass.Actor.LotCurrent.FireManager.FireCentroid,FireManager.kMinBabyPutDownDistance,FireManager.kMaxBabyPutDownDistance,RouteDistancePreference.PreferFurthestFromRouteDestination,RouteOrientationPreference.NoPreference,putDownClass.Actor.LotCurrent.LotId,(int[])null);
                               if(route.PlanResult.Succeeded())
                                putDownClass.mPutDownActualLocation=route.PlanResult.mDestination;
                        }
                        bool flag2=true;
                        bool tempPlacement=true;
                        switch(phase){
                            case ChildUtils.PutDownClass.Phase.Nearby:
                                flag2=false;
                                if(putDownClass.mPutDownType==ChildUtils.Where.SpecificSpot){
                                    phase=ChildUtils.PutDownClass.Phase.Corner3;
                                    break;
                                }
                                break;
                            case ChildUtils.PutDownClass.Phase.FrontDoor:
                                Door frontDoor=putDownClass.Actor.LotCurrent.FindFrontDoor();
                                if(frontDoor!=null){
                                    flag2=false;
                                    tempPlacement=false;
                                    putDownClass.mPutDownActualLocation=frontDoor.Position;
                                    putDownClass.mPutDownActualForward=frontDoor.ForwardVector;
                                    break;
                                }
                                break;
                            case ChildUtils.PutDownClass.Phase.Mailbox:
                                Mailbox mailbox=putDownClass.Actor.LotCurrent.FindMailbox();
                                if(mailbox!=null){
                                    flag2=false;
                                    putDownClass.mPutDownActualLocation=mailbox.Position;
                                    putDownClass.mPutDownActualForward=mailbox.ForwardVector;
                                  break;
                                }
                                break;
                            case ChildUtils.PutDownClass.Phase.Corner0:
                            case ChildUtils.PutDownClass.Phase.Corner1:
                            case ChildUtils.PutDownClass.Phase.Corner2:
                            case ChildUtils.PutDownClass.Phase.Corner3:
                                flag2=false;
                                putDownClass.mPutDownActualLocation=putDownClass.Actor.LotCurrent.Corners[(int)(phase-3)];
                                putDownClass.mPutDownActualForward=Vector3.UnitZ;
                                break;
                        }
                        if((double)putDownClass.mPutDownActualForward.LengthSqr()<0.00999999977648258)
                            putDownClass.mPutDownActualForward=putDownClass.Actor.ForwardVector;
                        if(flag2)
                            putDownClass.mPutDownActualLocation=Vector3.Zero;
                        else if(!GlobalFunctions.FindGoodLocationNearby((IGameObject)putDownClass.SocialJig,ref putDownClass.mPutDownActualLocation,ref putDownClass.mPutDownActualForward,putDownClass.mStrategies,tempPlacement)){
Simulator.Sleep(0U);
                            putDownClass.mPutDownActualLocation=Vector3.Zero;
                        }
                        if(putDownClass.mPutDownActualLocation!=Vector3.Zero){
                            putDownClass.SocialJig.SetPosition(putDownClass.mPutDownActualLocation);
                            putDownClass.SocialJig.SetForward(putDownClass.mPutDownActualForward);
                            putDownClass.SocialJig.SetOpacity(0.0f,0.0f);
                            putDownClass.SocialJig.AddToWorld();
                            if((double)(putDownClass.Actor.Position-putDownClass.mPutDownActualLocation).Length()<0.100000001490116){
                                Route routeTurnToFace=putDownClass.Actor.RoutingComponent.CreateRouteTurnToFace(putDownClass.SocialJig.GetSlotPosition(SocialJigTwoPerson.RoutingSlots.SimB));
                                routeTurnToFace.AddObjectToIgnoreForRoute(putDownClass.SocialJig.ObjectId);
                                flag1=putDownClass.Actor.DoRoute(routeTurnToFace);
                            }else{
                                Route jigA=putDownClass.SocialJig.RouteToJigA(putDownClass.Actor);
                                jigA.DoRouteFail=phase==ChildUtils.PutDownClass.Phase.Corner3;
                                flag1=putDownClass.Actor.DoRoute(jigA);
                            }
                        }
                    }else
                        break;
                }
                if(!flag1){
            return false;
                }
                carryingChildPosture.RemoveSocialBoost();
                if(putDownClass.Target.SimDescription.Toddler){
                    Slots.DetachFromSlot(putDownClass.Target.ObjectId);
                    putDownClass.Target.SetPosition(putDownClass.SocialJig.GetPositionOfSlot(SocialJigTwoPerson.RoutingSlots.SimB));
                    putDownClass.Target.SetForward(putDownClass.SocialJig.GetForwardOfSlot(SocialJigTwoPerson.RoutingSlots.SimB));
                }
                carryingChildPosture.CurrentStateMachine.SetActor("socialTemplate",(IHasScriptProxy)putDownClass.SocialJig);
                carryingChildPosture.CurrentStateMachine.RequestState((string)null,"PutDown");
                carryingChildPosture.CurrentStateMachine.RequestState((string)null,"Exit");
                carryingChildPosture.CurrentStateMachine.RemoveActor("socialTemplate");
                if(putDownClass.Target.SimDescription.Baby){
                    putDownClass.Target.UnParent();
                    putDownClass.Target.SetPosition(putDownClass.SocialJig.GetPositionOfSlot(SocialJigTwoPerson.RoutingSlots.SimB));
                    putDownClass.Target.SetForward(putDownClass.SocialJig.GetForwardOfSlot(SocialJigTwoPerson.RoutingSlots.SimB));
                    if(DeepSnowEffectManager.ShouldPlayDeepSnowEffectsNow(putDownClass.Target))
                    putDownClass.Target.DeepSnowEffectManager.PlayDeepSnowEffect(false,true);
                }
                putDownClass.Target.Posture=putDownClass.Target.Standing;
                putDownClass.Actor.Posture=putDownClass.Actor.Standing;
                putDownClass.Actor.SimRoutingComponent.EnableDynamicFootprint();
                putDownClass.Actor.SimRoutingComponent.ForceUpdateDynamicFootprint();
                putDownClass.Target.SimRoutingComponent.EnableDynamicFootprint();
                putDownClass.Target.SimRoutingComponent.ForceUpdateDynamicFootprint();
                if(putDownClass.mSocialToPushToTarget!=null){
                    SocialInteraction socialInteraction=(putDownClass.Actor.InteractionQueue.TransitionInteraction??putDownClass.Actor.CurrentInteraction)as SocialInteraction;
                                      socialInteraction.FinishLinkedInteraction();
                                      socialInteraction.WaitForSyncComplete();
                    putDownClass.Actor.ClearExitReasons();
                    putDownClass.PushSocialB(socialInteraction);
                    putDownClass.Actor.SynchronizationRole=Sim.SyncRole.Initiator;
                    putDownClass.Actor.SynchronizationLevel=Sim.SyncLevel.Started;
                    putDownClass.Actor.SynchronizationTarget=putDownClass.Target;
                 SimFix fix=new SimFix(putDownClass.Actor);
                    if(!fix.WaitForSynchronizationLevelWithSim1(putDownClass.Target,Sim.SyncLevel.Started   ,15f)){
            return false;
                    }
                        putDownClass.Actor.SynchronizationLevel=Sim.SyncLevel.Routed;
                 SimFix fix1=new SimFix(putDownClass.Actor);
                    if(!fix1.WaitForSynchronizationLevelWithSim1(putDownClass.Target,Sim.SyncLevel.Routed   ,15f)){
            return false;
                    }
                        putDownClass.Actor.SynchronizationLevel=Sim.SyncLevel.Committed;
                 SimFix fix2=new SimFix(putDownClass.Actor);
                    if(!fix2.WaitForSynchronizationLevelWithSim1(putDownClass.Target,Sim.SyncLevel.Committed,15f)){
            return false;
                    }
                }
            return true;
                    }catch(Exception exception){
         //  Get stack trace for the exception. with source file information
               var st=new StackTrace(exception,true);
         //  Get the top stack frame
         var frame=st.GetFrame(0);
         //  Get the line number from the stack frame
    var line=frame.GetFileLineNumber();
                      Alive.WriteLog(exception.Message+"\n\n"+
                                     exception.StackTrace+"\n\n"+
                                     exception.Source+"\n\n"+
                                     line);
            return false;
                    }finally{
                    }
            }
        }
    }
    public class CribHoldFix:Crib.Hold,IPreLoad,IAddInteraction{
        static InteractionDefinition sOldSingleton;
                                              public void AddInteraction(InteractionInjectorList interactions){
                                                                                                 interactions.ReplaceNoTest<Crib,Crib.Hold.Definition>(Singleton);
                                              }
                                   public void OnPreLoad(){
            Tunings.Inject<Sims3.Gameplay.Objects.Beds.Mimics.CribCheap    ,Crib.Hold.Definition,Definition>(false);
            Tunings.Inject<Sims3.Gameplay.Objects.Beds.Mimics.CribExpensive,Crib.Hold.Definition,Definition>(false);
            Tunings.Inject<Crib,Crib.Hold.Definition,Definition>(false);
                                     sOldSingleton=Singleton;
                                                   Singleton=new Definition();
                                   }
        public override bool Run(){
            if(!this.Actor.RouteToSlot((IGameObject)this.Target,this.Target.GetRoutingSlotForContainedSim()))
        return false;
this.StandardEntry(false);
            Sim child=this.Target.Child;
            if(child==null){
this.StandardExit(false,false);
        return false;
            }
            if(child.SimDescription.ChildOrAbove){
this.StandardExit(false,false);
        return false;
            }
            if(!StartObjectInteractionWithChild1((InteractionInstance)this,child,CommodityKind.InCrib,"BeTakenFromCrib")){
this.StandardExit(false,false);
        return false;
            }
            StateMachineClient currentStateMachine=(child.Posture as Crib.InCribPosture).CurrentStateMachine;
            StuffedAnimal stuffedAnimal=this.Target.StuffedAnimal;
            if(stuffedAnimal!=null&&child.Inventory.Contains((IGameObject)stuffedAnimal)){
                this.mTeddyBear=stuffedAnimal;
                this.mChild=child;
                stuffedAnimal.FadeOut();
            }
this.BeginCommodityUpdates();
                               currentStateMachine.SetActor("x",(IHasScriptProxy)this.Actor);
                               currentStateMachine.EnterState("x","EnterStanding");
                               currentStateMachine.RequestState(false,"x","ExitCarrying");
                               currentStateMachine.RequestState(true,"y","ExitCarrying");
this.EndCommodityUpdates(true);
            ChildUtils.CarryChild(this.Actor,child,false);
            this.Target.RemoveFromUseList(child);
this.StandardExit(false,false);
        return ChildUtils.FinishObjectInteractionWithChild((InteractionInstance)this,child);
        }
        public static bool StartObjectInteractionWithChild1(InteractionInstance interactionA,Sim child,CommodityKind childPosture,string receptiveInteractionNameKey){
            if(child==null||child.HasBeenDestroyed||!interactionA.SafeToSync()){
                Sim instanceActor0=interactionA.InstanceActor;
        return false;
            }
            Sim instanceActor=interactionA.InstanceActor;
            InteractionInstance instance=new ChildPlaceholderInteractionFix.Definition(ChildUtils.Localize(child.IsFemale,receptiveInteractionNameKey)).CreateInstance((IGameObject)instanceActor,(IActor)child,interactionA.GetPriority(),interactionA.Autonomous,interactionA.CancellableByPlayer);
                                instance.LinkedInteractionInstance=interactionA;
            ChildUtils.SetPosturePrecondition(instance,childPosture);
            if(!child.InteractionQueue.Add(instance)){
        return false;
            }
            if(!(interactionA is CribHoldFix cribHoldFix)||cribHoldFix==null||!cribHoldFix.StartSync1(true,false,(SyncLoopCallbackFunction)null,0.0f,true)){
        return false;
            }
            instanceActor.SynchronizationLevel=Sim.SyncLevel.Committed;
        SimFix fix=new SimFix(instanceActor);
            if(fix.WaitForSynchronizationLevelWithSim1(child,Sim.SyncLevel.Committed,SocialInteraction.kSocialSyncGiveupTime))
        return true;
            instanceActor.ClearSynchronizationData();
        return false;}
        public bool StartSync1(bool shouldBeMaster,bool ignoreExitReasons,SyncLoopCallbackFunction loopCallback,float notifySimMinutes,bool performSocializeWithTest){
            if(!this.SafeToSync()){
        return false;
            }
            Sim syncTarget=this.GetSyncTarget();
            if(syncTarget==null){
        return false;
            }
            ExitReason exitReason=ignoreExitReasons?ExitReason.None:ExitReason.Default;
            DateAndTime previousDateAndTime1=SimClock.CurrentTime();
            DateAndTime previousDateAndTime2=previousDateAndTime1;
            while((double)SimClock.ElapsedTime(TimeUnit.Minutes,previousDateAndTime1)<(double)InteractionInstance.kNumMinToWaitOnPreSync&&(this.GetTargetCurrentInteraction()==null||this.GetTargetCurrentInteraction().LinkedInteractionInstance!=this)){
                if(this.InstanceActor.HasExitReason(exitReason)){
        return false;
                }
                bool flag=false;
                if(syncTarget.InteractionQueue.IsRunning(this.LinkedInteractionInstance,true)){
                     flag=true;
                }else{
                    foreach(InteractionInstance interaction in syncTarget.InteractionQueue.InteractionList){
                        if(interaction.LinkedInteractionInstance==this){
                     flag=true;
                            break;
                        }
                    }
                }
                if(syncTarget.InteractionQueue.GetHeadInteraction() is IPreventSocialization headInteraction&&headInteraction!=null&&!headInteraction.SocializationAllowed(this.InstanceActor,syncTarget)||!flag){
        return false;
                }
                if(loopCallback!=null&&(double)SimClock.ElapsedTime(TimeUnit.Minutes,previousDateAndTime2)>=(double)notifySimMinutes){
                    if(!loopCallback()){
        return false;
                    }
                    previousDateAndTime2=SimClock.CurrentTime();
                }
Simulator.Sleep(0U);
            }
            this.InstanceActor.SynchronizationRole=shouldBeMaster?Sim.SyncRole.Initiator:Sim.SyncRole.Receiver;
            this.InstanceActor.SynchronizationTarget=syncTarget;
            this.InstanceActor.SynchronizationLevel=Sim.SyncLevel.Started;
         SimFix fix=new SimFix(this.InstanceActor);
            if(!fix.WaitForSynchronizationLevelWithSim1(syncTarget,Sim.SyncLevel.Started,exitReason,(float)InteractionInstance.kNumMinToWaitOnSyncStart,loopCallback,notifySimMinutes,performSocializeWithTest)){
        return false;
            }
        return true;}
        public new class Definition:Crib.Hold.Definition{
            public override InteractionInstance CreateInstance(ref InteractionInstanceParameters parameters){
                            InteractionInstance na=new CribHoldFix();
                                                na.Init(ref parameters);
                                         return na;
            }
            public override InteractionTestResult Test(ref InteractionInstanceParameters parameters,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
                if(parameters.Target is Crib target){
                    if(target.Child==null)
            return InteractionTestResult.GenericFail;
                    InteractionTestResult result=InteractionDefinitionUtilities.RunSpecialCaseAgeTests(ref parameters,parameters.Actor as Sim,target.Child,SpecialCaseAgeTests.Standard);
                    if(!parameters.Autonomous&&!InteractionDefinitionUtilities.IsPass(result)){
                        greyedOutTooltipCallback=!((Sim)parameters.Actor).IsFemale?new GreyedOutTooltipCallback(Crib.Hold.Definition.CantPickUpGreyedOutTooltipMale)
                                                                                  :new GreyedOutTooltipCallback(Crib.Hold.Definition.CantPickUpGreyedOutTooltipFemale);
                    if(parameters.Actor.SimDescription!=null&&parameters.Actor.SimDescription.IsBonehilda){
                        Alive.WriteLog("Bonehilda:CribHoldFix:Test:FAIL[0]:"+result+":"+parameters.Actor.ExitReason);
                    }
            return result;
                    }
                }
            return TestFix(ref parameters,ref greyedOutTooltipCallback);
            }
            public InteractionTestResult TestFix(ref InteractionInstanceParameters parameters,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
                Sim   actor1=parameters. Actor as Sim;
                Crib target1=parameters.Target as Crib;
                if((object) actor1==null)
            return InteractionTestResult.Root_Null_Actor;
                if((object)target1==null)
            return InteractionTestResult.Root_Null_Target;
                Sim actor2=(object)actor1 as Sim;
                IGameObject target2=(IGameObject)target1;
                for(IGameObject gameObject=(IGameObject)target1;gameObject!=null;gameObject=gameObject.Parent){
                    if(gameObject==LiveDragHelperModel.CachedTopDraggedObject)
            return InteractionTestResult.Root_TargetOnHandTool;
                }
                bool flag=true;
                InteractionTuning tuning=parameters.InteractionObjectPair.Tuning;
                Tradeoff mTradeoff=tuning?.mTradeoff;
                if(tuning!=null){
                    CommodityKind workMotive=actor1.WorkMotive;
                    if(workMotive!=CommodityKind.None&&mTradeoff.SatisfiesCommodity(workMotive))
                     flag=false;
                    if(parameters.Autonomous){
                        if(tuning.HasFlags(InteractionTuning.FlagField.DisallowAutonomous)&&flag)
            return InteractionTestResult.Tuning_DisallowAutonomous;
                        if(mTradeoff.FunExit&&actor1.Motives.FunInteractionTest(false))
            return InteractionTestResult.Tuning_FunInteractionTest;
                    }else if(tuning.HasFlags(InteractionTuning.FlagField.DisallowUserDirected))
            return InteractionTestResult.Tuning_DisallowUserDirected;
                    if(actor1.IsSelectable&&tuning.HasFlags(InteractionTuning.FlagField.DisallowPlayerSim))
            return InteractionTestResult.Tuning_DisallowPlayerSim;
                  if(flag){
                                     AutonomyFix fix=new AutonomyFix(actor1.Autonomy.mActor,actor1.Autonomy.Motives,actor1.Autonomy.CurrentSearchType,actor1.Autonomy.IsActorInTombRoom);
     InteractionTestResult interactionTestResult=fix.CheckAvailability1(parameters.Autonomous,tuning.Availability,parameters.InteractionObjectPair);
                        if(interactionTestResult!=InteractionTestResult.Pass)
            return interactionTestResult;
                  }
                }
                actor1.Autonomy.UpdateCacheIfNeeded((IGameObject) target1);
               var interactionTestResult1=AutonomyFix.CommonTests1((InteractionDefinition)this,actor2,target2,parameters);
                if(interactionTestResult1!=InteractionTestResult.Pass)
            return interactionTestResult1;
               var interactionTestResult2=!(this is IMetaInteractionDefinition)?    InteractionDefinitionUtilities.SpecialCaseTests((InteractionDefinition)this,actor2,target2,parameters)
                                                                               :MetaInteractionDefinitionUtilities.SpecialCaseTests(actor2,target2,parameters);
                if(interactionTestResult2!=InteractionTestResult.Pass)
            return interactionTestResult2;
                if(!this.Test(actor1,target1,parameters.Autonomous,ref greyedOutTooltipCallback))
            return InteractionTestResult.Def_TestFailed;
                if(tuning!=null){
 InteractionTestResult interactionTestResult3=actor1.Autonomy.CheckAvailabilityTooltip((InteractionDefinition)this,(IGameObject)target1,tuning.Availability,parameters,mTradeoff,ref greyedOutTooltipCallback);
                    if(interactionTestResult3!=InteractionTestResult.Pass)
            return interactionTestResult3;
                }
               var interactionTestResult4=InteractionDefinitionUtilities.SpecialCaseTooltipTests((InteractionDefinition)this,actor2,target2,parameters,ref greyedOutTooltipCallback);
            return interactionTestResult4!=InteractionTestResult.Pass?interactionTestResult4:InteractionTestResult.Pass;
            }
        }
    }
    public class PickUpChildFix:PickUpChild,IPreLoad,IAddInteraction{
        static InteractionDefinition sOldSingleton;
                                              public void AddInteraction(InteractionInjectorList interactions){
                                                                                                 interactions.ReplaceNoTest<Sim,PickUpChild.Definition>(Singleton);
                                              }
                                   public void OnPreLoad(){
            Tunings.Inject<Sim,PickUpChild.Definition,Definition>(false);
                                     sOldSingleton=Singleton;
                                                   Singleton=new Definition();
                                   }
        public override bool Run(){
            this.Actor.LookAtManager.DisableLookAts();
            string angle="";
            if(this.Actor.IsActiveFirefighter&&Occupation.SimIsWorkingAtJobOnLot(this.Actor,this.Target.LotCurrent)){
                this.SetPriority(new InteractionPriority(InteractionPriorityLevel.Fire,float.MinValue));
                this.RequestWalkStyle(Sim.WalkStyle.FastJog);
            }
            SocialJigTwoPerson objectOutOfWorld;
            if(this.Target.SimDescription.Baby){
                Route route=this.Actor.CreateRoute();
                      route.PlanToPointRadialRange((IHasScriptProxy)this.Target,this.Target.Posture.Container.Position,0.0f,SocialInteraction.kSocialRouteMinDist,Vector3.UnitZ,360f,RouteDistancePreference.PreferNearestToRouteOrigin,RouteOrientationPreference.TowardsObject,this.Target.LotCurrent.LotId,new int[1]{
                        this.Target.RoomId
                      });
                if(!this.Actor.DoRoute(route)){
        return false;
                }
                if(!StartObjectInteractionWithChild1((InteractionInstance)this,this.Target,CommodityKind.Standing,"BePickedUp")){
        return false;
                }
                if(!this.Actor.RouteToObjectRadius((IGameObject)this.Target,0.7f)){
        return false;
                }
                this.SocialJig=(SocialJig)(GlobalFunctions.CreateObjectOutOfWorld("SocialJigBabyToddler") as SocialJigTwoPerson);
                this.SocialJig.SetOpacity(0.0f,0.0f);
                Vector3 position=this.Actor.Position;
                Vector3 forwardVector1=this.Actor.ForwardVector;
                this.SocialJig.SetPosition(position);
                this.SocialJig.SetForward(forwardVector1);
                this.SocialJig.RegisterParticipants(this.Actor,this.Target);
                this.SocialJig.AddToWorld();
                Vector3 forwardOfSlot=this.SocialJig.GetForwardOfSlot(SocialJigTwoPerson.RoutingSlots.SimB);
                Vector3 forwardVector2=this.Target.ForwardVector;
                double x1=(double)forwardOfSlot.x;
                double z1=(double)forwardOfSlot.z;
                double x2=(double)forwardVector2.x;
                double z2=(double)forwardVector2.z;
                double x3=x1*x2+z1*z2;
                double y=x1*z2-z1*x2;
                float num1=(float)Math.Atan2(y,x3)-0.7853982f;
                while((double)num1<0.0)
                              num1+=6.283185f;
                float num2=0.0f;
                float num3=(float)(2.0*(double)num1/3.14159274101257);
           if((double)num3<1.0){
                    angle="_270";
                      num2=1.570796f;
           }else 
           if((double)num3<2.0){
                    angle="_180";
                      num2=3.141593f;
           }else
           if((double)num3<3.0){
                    angle="_90";
                      num2=-1.570796f;
           }else
                    angle="";
                this.BabyJig=objectOutOfWorld=GlobalFunctions.CreateObjectOutOfWorld("SocialJigBabyToddler") as SocialJigTwoPerson;
                objectOutOfWorld.SetOpacity(0.0f,0.0f);
                Vector3 forwardVector3=this.SocialJig.ForwardVector;
                Vector3 forward=Quaternion.MakeFromEulerAngles(0.0f,(float)Math.Atan2(y,x3)-num2,0.0f).ToMatrix().TransformVector(forwardVector3);
                objectOutOfWorld.SetPosition(this.Target.Position-forward*0.7f);
                objectOutOfWorld.SetForward(forward);
                objectOutOfWorld.AddToWorld();
            }else{
                if(this.Actor.IsAtHome&&this.Actor.LotCurrent.HasVirtualResidentialSlots&&(!this.Actor.IsInPublicResidentialRoom||this.Actor.Level!=this.Target.Level)&&!this.Actor.RouteToObjectRadius((IGameObject) this.Target, 0.7f))
        return false;
                this.SocialJig=(SocialJig)(objectOutOfWorld=GlobalFunctions.CreateObjectOutOfWorld("SocialJigBabyToddler") as SocialJigTwoPerson);
                this.SocialJig.SetOpacity(0.0f,0.0f);
                var fix=new SocialInteractionFix(this);
                if(!fix.BeginSocialInteraction1((InteractionDefinition)new SocialInteractionBFix.NoAgeOrClosedVenueCheckDefinition(ChildUtils.Localize(this.Target.SimDescription.IsFemale,"BePickedUp"),false),true,false))
        return false;
            }
            if(this.LinkedInteractionInstance.InstanceActor.CurrentInteraction!=this.LinkedInteractionInstance)
        return false;
this.BeginCommodityUpdates();
            ChildUtils.PickUpChild(this.Actor,this.Target,(SocialJig)objectOutOfWorld,angle);
            if(GameUtils.IsInstalled(ProductVersion.EP8)&&this.Target.SimDescription.Toddler&&(this.Target.CurrentOutfitCategory!=OutfitCategories.Outerwear&&this.Target.IsOutside)&&(double)SeasonsManager.Temperature<=(double)ChangeToddlerClothes.kTemperatureToSwitchToddlerToOuterwear){
                ChangeToddlerClothes instance=ChangeToddlerClothes.Singleton.CreateInstance((IGameObject)this.Target,(IActor)this.Actor,this.GetPriority(),this.Autonomous,this.CancellableByPlayer) as ChangeToddlerClothes;
                                     instance.Reason=Sim.ClothesChangeReason.TemperatureTooCold;
                int num=(int)instance.StartBuildingOutfit();
                this.TryPushAsContinuation((InteractionInstance)instance);
            }
            this.FinishLinkedInteraction();
this.EndCommodityUpdates(true);
            this.WaitForSyncComplete();
        return true;
        }
        public static bool StartObjectInteractionWithChild1(InteractionInstance interactionA,Sim child,CommodityKind childPosture,string receptiveInteractionNameKey){
            if(child==null||child.HasBeenDestroyed||!interactionA.SafeToSync()){
                Sim instanceActor0=interactionA.InstanceActor;
        return false;
            }
            Sim instanceActor=interactionA.InstanceActor;
            InteractionInstance instance=new ChildPlaceholderInteractionFix.Definition(ChildUtils.Localize(child.IsFemale,receptiveInteractionNameKey)).CreateInstance((IGameObject)instanceActor,(IActor)child,interactionA.GetPriority(),interactionA.Autonomous,interactionA.CancellableByPlayer);
                                instance.LinkedInteractionInstance=interactionA;
            ChildUtils.SetPosturePrecondition(instance,childPosture);
            if(!child.InteractionQueue.Add(instance)){
        return false;
            }
            if(!(interactionA is PickUpChildFix pickUp)||pickUp==null||!pickUp.StartSync1(true,false,(SyncLoopCallbackFunction)null,0.0f,true)){
        return false;
            }
            instanceActor.SynchronizationLevel=Sim.SyncLevel.Committed;
        SimFix fix=new SimFix(instanceActor);
            if(fix.WaitForSynchronizationLevelWithSim1(child,Sim.SyncLevel.Committed,SocialInteraction.kSocialSyncGiveupTime))
        return true;
            instanceActor.ClearSynchronizationData();
        return false;}
        public class SocialInteractionFix{
              public SocialInteractionFix(SocialInteraction interaction){
                                           this.interaction=interaction;
              }
                       public SocialInteraction interaction;
            public bool BeginSocialInteraction1(InteractionDefinition socialToPushToTarget,bool pairedSocial,bool doCallOver){
            return this.BeginSocialInteraction1(socialToPushToTarget,pairedSocial,interaction.Target.GetSocialRadiusWith(interaction.Actor),doCallOver);
            }
            public bool BeginSocialInteraction1(InteractionDefinition socialToPushToTarget,bool pairedSocial,float separationDist,bool doCallOver){
                interaction.mSocialDist=separationDist;
                if(doCallOver&&interaction.Actor.IsActiveSim&&(!interaction.Autonomous&&interaction.Target.IsNPC)&&(interaction.Target.IsRouting&&!(interaction.Actor.Posture is SwimmingInPool)&&(!(interaction.Actor.Posture is ScubaDiving)&&LotManager.RoomIdIsOutside(interaction.Target.RoomId)))&&(interaction.Target.CurrentInteraction==null||interaction.Target.CurrentInteraction.Autonomous)){
                    Vector3 vector3=interaction.Target.Position-interaction.Actor.Position;
                    float num1=vector3.LengthSqr();
                    vector3=vector3.Normalize();
                    float num2=100f;
                    if((double)num1<=(double)num2*(double)num2){
                        float num3=1.570796f;
                        if((double)(vector3*interaction.Target.ForwardVector)>Math.Cos((double)num3))
                            interaction.CallOver();
                    }
                }
                interaction.mResultCode=SocialInteraction.SocialResultCode.None;
                Sim  actor=interaction. Actor;
                Sim target=interaction.Target;
                SocialInteraction.SocialResultCode resultCode;
                bool flag1;
                if((actor.Conversation==null||actor.Conversation!=target.Conversation)&&!actor.IsRiding(interaction.Target)||pairedSocial){
                    if(target.SimRoutingComponent.IsInCarSequence()||target.IsSleeping&&!SocialInteraction.CanInteractWithSleepingSim(actor,target)){
                        interaction.SetResultCodeIfNoneIsSet(SocialInteraction.SocialResultCode.SocialNotAvailableOnTarget);
            return false;
                    }
                    interaction.mSleepingListener=EventTracker.AddListener(EventTypeId.kSimFellAsleep,new ProcessEventDelegate(interaction.CancelSocialOnEventTrackerEvent),target,(GameObject)null);
                     flag1=interaction.SocialRouteA(socialToPushToTarget,pairedSocial,separationDist,out resultCode);
                }else{
                    resultCode=interaction.LockSimB(socialToPushToTarget,pairedSocial,separationDist);
                     flag1=resultCode==SocialInteraction.SocialResultCode.Succeeded;
                }
                interaction.SetInitialRouteComplete();
                bool flag2=false;
                  if(flag1){
                    actor.LoopIdle();
                    if(interaction is PickUpChildFix pickUp&&pickUp!=null&&pickUp.StartSync1(true,false,(SyncLoopCallbackFunction)null,0.0f,true)){
                        SocialInteraction interactionInstance=interaction.LinkedInteractionInstance as SocialInteraction;
                        if(interaction.mbIsTurnToFace){
                            DateAndTime previousDateAndTime=SimClock.CurrentTime();
                            while(!interactionInstance.mbBRoutePlanned){
                                if((double)SimClock.ElapsedTime(TimeUnit.Minutes,previousDateAndTime)>=(double)SocialInteraction.kSocialTimeoutTime||actor.HasExitReason(ExitReason.Default)||(target.HasExitReason(ExitReason.Default)||interactionInstance.Cancelled)){
                                    interaction.mResultCode=SocialInteraction.SocialResultCode.PathPlanToToSocialTargetFailed;
            return false;
                                }
Simulator.Sleep(0U);
                            }
                        }
                        if(interaction.mbIsTurnToFace&&!interactionInstance.mbIsTurnToFace)
                            interaction.SetApproachRouteComplete();
                        actor.SynchronizationLevel=Sim.SyncLevel.Started;
                    SimFix fix=new SimFix(actor);
                        if(fix.WaitForSynchronizationLevelWithSim1(target,Sim.SyncLevel.Routed,SocialInteraction.kSocialSyncGiveupTime)){
                            if(interaction.mRoute!=null){
                                if(interaction.mbIsTurnToFace&&interactionInstance.mbIsTurnToFace&&interaction.mRoute.PlanResult.Succeeded()){
                                    interaction.mRoute.CreateSegmentAtDistanceBeforeEndOfRoute(SocialInteraction.kApproachGreetDistance);
                                    interaction.mRoute.RegisterCallback(new RouteCallback(interaction.ReleaseSimBApproach),RouteCallbackType.TriggerOnce,RouteCallbackConditions.LessThan(SocialInteraction.kApproachGreetDistance+0.5f));
                                }
                     flag1=actor.DoRoute(interaction.mRoute);
                                interaction.mRoute=(Sims3.SimIFace.Route)null;
                                if(interaction.Actor.Posture is ScubaDiving&&flag1)
                                    actor.LoopIdle();
                            }
                        }else{
                     flag2=true;
                            resultCode=SocialInteraction.SocialResultCode.SyncToSocialTargetTimedOut;
                     flag1=false;
                        }
                        actor.SynchronizationLevel=Sim.SyncLevel.Committed;
                     SimFix fix1=new SimFix(actor);
                        if(!fix1.WaitForSynchronizationLevelWithSim1(target,Sim.SyncLevel.Committed,SocialInteraction.kSocialSyncGiveupTime)){
                            resultCode=SocialInteraction.SocialResultCode.SyncToSocialTargetTimedOut;
                     flag1=false;
                        }
                    }else
                     flag1=false;
                }
                  if(flag2&&!actor.HasExitReason(ExitReason.UserCanceled))
                    actor.PlayRouteFailure(target.GetThoughtBalloonThumbnailKey());
                if(actor.HasExitReason()){
                     flag1=false;
                    resultCode=SocialInteraction.SocialResultCode.CanceledByExitReason;
                }
                  if(flag1&&interaction.mbValidateFacing&&!interaction.SimsAreFacing(actor,target))
                    actor.RoutingComponent.RouteTurnToFace(target.PositionOnFloor);
                if(!interaction.OnBeginSocialInteractionDone())
                     flag1=false;
                  if(flag1){
                    actor.PopCanePostureIfNecessary();
                    if(!(actor.CurrentInteraction is IUmbrellaSupportedSocial))
                        Umbrella.PopUmbrellaPostureIfNecessary(actor,false);
                    actor.PopBackpackPostureIfNecessary();
                    if(!(actor.CurrentInteraction is IJetpackInteraction))
                        actor.PopJetpackPostureIfNecessary();
                  }
                interaction.SetResultCodeIfNoneIsSet(resultCode);
                  if(flag1){
                    interaction.CheckForPlayTimeBuffs(interaction.Actor,interaction.Target);
                    interaction.CheckForPlayTimeBuffs(interaction.Target,interaction.Actor);
                  }
            return flag1;
            }
        }
        public bool StartSync1(bool shouldBeMaster,bool ignoreExitReasons,SyncLoopCallbackFunction loopCallback,float notifySimMinutes,bool performSocializeWithTest){
            if(!this.SafeToSync()){
        return false;
            }
            Sim syncTarget=this.GetSyncTarget();
            if(syncTarget==null){
        return false;
            }
            ExitReason exitReason=ignoreExitReasons?ExitReason.None:ExitReason.Default;
            DateAndTime previousDateAndTime1=SimClock.CurrentTime();
            DateAndTime previousDateAndTime2=previousDateAndTime1;
            while((double)SimClock.ElapsedTime(TimeUnit.Minutes,previousDateAndTime1)<(double)InteractionInstance.kNumMinToWaitOnPreSync&&(this.GetTargetCurrentInteraction()==null||this.GetTargetCurrentInteraction().LinkedInteractionInstance!=this)){
                if(this.InstanceActor.HasExitReason(exitReason)){
        return false;
                }
                bool flag=false;
                if(syncTarget.InteractionQueue.IsRunning(this.LinkedInteractionInstance,true)){
                     flag=true;
                }else{
                    foreach(InteractionInstance interaction in syncTarget.InteractionQueue.InteractionList){
                        if(interaction.LinkedInteractionInstance==this){
                     flag=true;
                            break;
                        }
                    }
                }
                if(syncTarget.InteractionQueue.GetHeadInteraction() is IPreventSocialization headInteraction&&headInteraction!=null&&!headInteraction.SocializationAllowed(this.InstanceActor,syncTarget)||!flag){
        return false;
                }
                if(loopCallback!=null&&(double)SimClock.ElapsedTime(TimeUnit.Minutes,previousDateAndTime2)>=(double)notifySimMinutes){
                    if(!loopCallback()){
        return false;
                    }
                    previousDateAndTime2=SimClock.CurrentTime();
                }
Simulator.Sleep(0U);
            }
            this.InstanceActor.SynchronizationRole=shouldBeMaster?Sim.SyncRole.Initiator:Sim.SyncRole.Receiver;
            this.InstanceActor.SynchronizationTarget=syncTarget;
            this.InstanceActor.SynchronizationLevel=Sim.SyncLevel.Started;
         SimFix fix=new SimFix(this.InstanceActor);
            if(!fix.WaitForSynchronizationLevelWithSim1(syncTarget,Sim.SyncLevel.Started,exitReason,(float)InteractionInstance.kNumMinToWaitOnSyncStart,loopCallback,notifySimMinutes,performSocializeWithTest)){
        return false;
            }
        return true;}
        public new class Definition:PickUpChild.Definition{
            public override InteractionInstance CreateInstance(ref InteractionInstanceParameters parameters){
                            InteractionInstance na=new PickUpChildFix();
                                                na.Init(ref parameters);
                                         return na;
            }
            public override bool Test(Sim actor,Sim target,bool isAutonomous,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
                if(!(target.SimDescription.ToddlerOrBelow)){
            return(false);
                }
                if(!(!actor.SimDescription.ChildOrBelow&&target.Posture.Container==target)){
            return(false);
                }else{
                }
                if(!(actor.CarryingChildPosture==null)){
            return(false);
                }
                if(!(target!=actor.Posture.Container)){
            return(false);
                }else{
                }
                if(!(!target.Posture.Satisfies(CommodityKind.InFairyHouse,(IGameObject)null))){
            return(false);
                }else{
                }
            return( true);}
            //----------------------------------------
            public override InteractionTestResult Test(ref InteractionInstanceParameters parameters,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
                if(parameters.EffectivelyAutonomous&&parameters.Priority.Level<InteractionPriorityLevel.ESRB&&(parameters.Actor is Sim actor&&actor.CarryingChildPosture!=null)){
            return(InteractionTestResult.GenericFail);
                }
                                    InteractionTestResult result=TestFix(ref parameters,ref greyedOutTooltipCallback);
                if(!InteractionDefinitionUtilities.IsPass(result)){
            return(result);
                }
                if(PickUpChild.Definition.CanPickUpBabyOrToddler(ref parameters)){
            return(InteractionTestResult.Pass);
                }else{
                }
                greyedOutTooltipCallback=!((Sim)parameters.Actor).IsFemale?new GreyedOutTooltipCallback(PickUpChild.Definition.CantPickUpGreyedOutTooltipMale):
                                                                           new GreyedOutTooltipCallback(PickUpChild.Definition.CantPickUpGreyedOutTooltipFemale);
            return(InteractionTestResult.GenericFail);
            }
            public InteractionTestResult TestFix(ref InteractionInstanceParameters parameters,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
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
                    try{
                                     AutonomyFix fix=new AutonomyFix(actor1.Autonomy.mActor,actor1.Autonomy.Motives,actor1.Autonomy.CurrentSearchType,actor1.Autonomy.IsActorInTombRoom);
     InteractionTestResult interactionTestResult=fix.CheckAvailability1(parameters.Autonomous,tuning.Availability,parameters.InteractionObjectPair);
                        if(interactionTestResult!=InteractionTestResult.Pass){
            return(interactionTestResult);
                        }
                    }catch(Exception exception){
         //  Get stack trace for the exception. with source file information
               var st=new StackTrace(exception,true);
         //  Get the top stack frame
         var frame=st.GetFrame(0);
         //  Get the line number from the stack frame
    var line=frame.GetFileLineNumber();
                      Alive.WriteLog(exception.Message+"\n\n"+
                                     exception.StackTrace+"\n\n"+
                                     exception.Source+"\n\n"+
                                     line);
            return(InteractionTestResult.GenericUnknown);
                    }finally{
                    }
                    }
                }
                actor1.Autonomy.UpdateCacheIfNeeded((IGameObject)target1);
                    try{
               var interactionTestResult1=AutonomyFix.CommonTests1((InteractionDefinition)this,actor2,target2,parameters);
                if(interactionTestResult1!=InteractionTestResult.Pass){
            return(interactionTestResult1);
                }
                    }catch(Exception exception){
         //  Get stack trace for the exception. with source file information
               var st=new StackTrace(exception,true);
         //  Get the top stack frame
         var frame=st.GetFrame(0);
         //  Get the line number from the stack frame
    var line=frame.GetFileLineNumber();
                      Alive.WriteLog(exception.Message+"\n\n"+
                                     exception.StackTrace+"\n\n"+
                                     exception.Source+"\n\n"+
                                     line);
            return(InteractionTestResult.GenericUnknown);
                    }finally{
                    }
               var interactionTestResult2=!(this is IMetaInteractionDefinition)?    InteractionDefinitionUtilities.SpecialCaseTests((InteractionDefinition)this,actor2,target2,parameters)
                                                                               :MetaInteractionDefinitionUtilities.SpecialCaseTests(actor2,target2,parameters);
                if(interactionTestResult2!=InteractionTestResult.Pass){
            return(interactionTestResult2);
                }
                if(!this.Test(actor1,target1,parameters.Autonomous,ref greyedOutTooltipCallback)){
            return(InteractionTestResult.Def_TestFailed);
                }
                if(tuning!=null){
               var interactionTestResult3=actor1.Autonomy.CheckAvailabilityTooltip((InteractionDefinition)this,(IGameObject)target1,tuning.Availability,parameters,mTradeoff,ref greyedOutTooltipCallback);
                if(interactionTestResult3!=InteractionTestResult.Pass){
            return(interactionTestResult3);
                }
                }
               var interactionTestResult4=InteractionDefinitionUtilities.SpecialCaseTooltipTests((InteractionDefinition)this,actor2,target2,parameters,ref greyedOutTooltipCallback);
                if(interactionTestResult4!=InteractionTestResult.Pass){
            return(interactionTestResult4);
                }
            return(InteractionTestResult.Pass);
            }
            //----------------------------------------
        }
    }
    public class ChildPlaceholderInteractionFix:ChildUtils.ChildPlaceholderInteraction,IPreLoad,IAddInteraction{
                                              public void AddInteraction(InteractionInjectorList interactions){
                                              }
                                   public void OnPreLoad(){
                                   }
        public override bool Run(){
            if(!this.StartSync2(false,false,(SyncLoopCallbackFunction)null,0.0f,true)){
        return false;
            }
            this.Actor.SynchronizationLevel=Sim.SyncLevel.Committed;
         SimFix fix=new SimFix(this.Actor);
            if(!fix.WaitForSynchronizationLevelWithSim1(this.Target,Sim.SyncLevel.Committed,SocialInteraction.kSocialSyncGiveupTime)){
        return false;
            }
        bool flag=this.DoLoop(ExitReason.Default);
        this.CopyExitReasonToLinkedInteraction();
        this.WaitForMasterInteractionToFinish();
        return this.WaitForSyncComplete()&&flag;
        }
        public bool StartSync2(bool shouldBeMaster,bool ignoreExitReasons,SyncLoopCallbackFunction loopCallback,float notifySimMinutes,bool performSocializeWithTest){
            if(!this.SafeToSync()){
        return false;
            }
            Sim syncTarget=this.GetSyncTarget();
            if(syncTarget==null){
        return false;
            }
            ExitReason exitReason=ignoreExitReasons?ExitReason.None:ExitReason.Default;
            DateAndTime previousDateAndTime1=SimClock.CurrentTime();
            DateAndTime previousDateAndTime2=previousDateAndTime1;
            while((double)SimClock.ElapsedTime(TimeUnit.Minutes,previousDateAndTime1)<(double)InteractionInstance.kNumMinToWaitOnPreSync&&(this.GetTargetCurrentInteraction()==null||this.GetTargetCurrentInteraction().LinkedInteractionInstance!=this)){
                if(this.InstanceActor.HasExitReason(exitReason)){
        return false;
                }
                bool flag=false;
                if(syncTarget.InteractionQueue.IsRunning(this.LinkedInteractionInstance,true)){
                     flag=true;
                }else{
                    foreach(InteractionInstance interaction in syncTarget.InteractionQueue.InteractionList){
                        if(interaction.LinkedInteractionInstance==this){
                     flag=true;
                            break;
                        }
                    }
                }
                if(syncTarget.InteractionQueue.GetHeadInteraction() is IPreventSocialization headInteraction&&!headInteraction.SocializationAllowed(this.InstanceActor,syncTarget)||!flag){
        return false;
                }
                if(loopCallback!=null&&(double)SimClock.ElapsedTime(TimeUnit.Minutes,previousDateAndTime2)>=(double)notifySimMinutes){
                    if(!loopCallback()){
        return false;
                    }
              previousDateAndTime2 = SimClock.CurrentTime();
                }
Simulator.Sleep(0U);
            }
            this.InstanceActor.SynchronizationRole=shouldBeMaster?Sim.SyncRole.Initiator:Sim.SyncRole.Receiver;
            this.InstanceActor.SynchronizationTarget=syncTarget;
            this.InstanceActor.SynchronizationLevel=Sim.SyncLevel.Started;
         SimFix fix=new SimFix(this.InstanceActor);
            if(!fix.WaitForSynchronizationLevelWithSim1(syncTarget,Sim.SyncLevel.Started,exitReason,(float)InteractionInstance.kNumMinToWaitOnSyncStart,loopCallback,notifySimMinutes,performSocializeWithTest)){
        return false;
            }
        return true;}
        [DoesntRequireTuning]
        public new class Definition:ChildUtils.ChildPlaceholderInteraction.Definition{
            public override InteractionInstance CreateInstance(ref InteractionInstanceParameters parameters){
                            InteractionInstance na=new ChildPlaceholderInteractionFix();
                                                na.Init(ref parameters);
                                         return na;
            }
            public Definition(){}
            public Definition(string localizedName){
                           this.Name=localizedName;
            }
            public override bool Test(Sim a,Sim target,bool isAutonomous,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
            return true;
            }
            public override string GetInteractionName(Sim actor,Sim target,InteractionObjectPair iop){
            return this.Name;
            }
        }
    }
    public class SimFix{
          public SimFix(Sim sim){
                   this.sim=sim;
          }
          public Sim sim;
        public bool WaitForSynchronizationLevelWithSim1(Sim targetSim,Sim.SyncLevel desiredSynchLevel,float giveupTime){
        return this.WaitForSynchronizationLevelWithSim1(targetSim,desiredSynchLevel,ExitReason.Default,giveupTime,(SyncLoopCallbackFunction)null,0.0f,true);
        }
        public bool WaitForSynchronizationLevelWithSim1(Sim targetSim,Sim.SyncLevel desiredSynchLevel,ExitReason exitReasonInterrupt,float giveupTime,SyncLoopCallbackFunction loopCallback,float notifySimMinutes,bool performSocializeWithTest){
            DateAndTime previousDateAndTime1=SimClock.CurrentTime();
            DateAndTime previousDateAndTime2=previousDateAndTime1;
            GreyedOutTooltipCallback greyedOutTooltipCallback=(GreyedOutTooltipCallback)null;
            sim.SynchronizationSleeping=sim.SynchronizationLevel>=desiredSynchLevel;
            while(!sim.IsAtSynchronizationLevelWith(targetSim,desiredSynchLevel)){
                if((double)SimClock.ElapsedTime(TimeUnit.Minutes,previousDateAndTime1)>=(double)giveupTime){
                    sim.OnWaitForSynchronizationLevelWithSimFailed(targetSim);
        return false;
                }
                if(sim.HasExitReason(exitReasonInterrupt)){
                    sim.OnWaitForSynchronizationLevelWithSimFailed(targetSim);
        return false;
                }
                InteractionTestResult result;
                if(performSocializeWithTest&&!IUtil.IsPass(result=CanSocializeWithSyncCheck1((string)null,sim,targetSim,sim.IsInAutonomousInteraction(),ref greyedOutTooltipCallback,true,true))){
                    sim.OnWaitForSynchronizationLevelWithSimFailed(targetSim);
        return false;
                }
                if(loopCallback!=null&&(double)SimClock.ElapsedTime(TimeUnit.Minutes,previousDateAndTime2)>=(double)notifySimMinutes){
                    if(loopCallback()){
                        previousDateAndTime2=SimClock.CurrentTime();
                    }else{
                        sim.OnWaitForSynchronizationLevelWithSimFailed(targetSim);
        return false;
                    }
                }
Simulator.Sleep(0U);
            }
            sim.SynchronizationSleeping=false;
            ExitReason reason=exitReasonInterrupt;
            if(sim.SynchronizationRole==Sim.SyncRole.Receiver)
                reason&=ExitReason.SynchronizationFailed;
            if(sim.HasExitReason(reason)){
                sim.OnWaitForSynchronizationLevelWithSimFailed(targetSim);
        return false;
            }
            if(sim.IsAtSynchronizationLevelWith(targetSim,desiredSynchLevel))
        return true;
            sim.OnWaitForSynchronizationLevelWithSimFailed(targetSim);
        return false;}
        public static InteractionTestResult CanSocializeWithSyncCheck1(string social,Sim actor,Sim target,bool isAutonomous,ref GreyedOutTooltipCallback greyedOutTooltipCallback,bool bAllowOnToddlers,bool interactionHasAlreadyStarted){
        return target!=null&&target.HasBeenDestroyed?InteractionTestResult.Root_Null_Target
               :CanSocializeWith1(social,actor,target,isAutonomous,ref greyedOutTooltipCallback,bAllowOnToddlers,interactionHasAlreadyStarted,true);
        }
        public static InteractionTestResult CanSocializeWith1(string social,Sim actor,Sim target,bool isAutonomous,ref GreyedOutTooltipCallback greyedOutTooltipCallback,bool bAllowOnToddlers,bool interactionHasAlreadyStarted,bool isSyncCheck){
            if(target==null||target.InteractionQueue==null){
        return InteractionTestResult.Root_Null_Target;
            }
            if(target==actor){
        return InteractionTestResult.Social_ActorIsTarget;
            }
            //bool flag1=(target.CurrentInteraction is IAllowedWithSocializationDisabled||target.CurrentInteraction!=null&&target.CurrentInteraction.InteractionDefinition is IAllowedWithSocializationDisabled)&&interactionHasAlreadyStarted;
            //bool flag2=target.SimDescription.IsBonehilda&&actor.SimDescription.ChildOrAbove&&!isAutonomous&&interactionHasAlreadyStarted;
            //if(!target.CanBeSocializedWith&&!flag1&&!flag2){
        //  return InteractionTestResult.Social_TargetCannotBeSocializedWith;
            //}
            if(!target.InWorld||!actor.InWorld){
        return InteractionTestResult.Social_TargetOutOfWorld;
            }
            if(!bAllowOnToddlers&&(actor.SimDescription.ToddlerOrBelow||target.SimDescription.ToddlerOrBelow)){
        return InteractionTestResult.Social_ActorOrTargetIsToddler;
            }
            if(!isSyncCheck){
                string onlyAllowedSocial=target.SocialComponent.OnlyAllowedSocial;
                if(!string.IsNullOrEmpty(onlyAllowedSocial)&&onlyAllowedSocial!=social){
        return InteractionTestResult.Social_TargetCannotBeSocializedWith;
                }
            }
            if(target.IsSleeping&&!SocialInteraction.CanInteractWithSleepingSim(actor,target))
        return InteractionTestResult.Social_TargetSleeping;
            if(target.SimRoutingComponent.IsInCarSequence())
        return InteractionTestResult.Social_TargetInCar;
            InteractionInstance headInteraction1=target.InteractionQueue.GetHeadInteraction();
            if(!(headInteraction1 is IIgnoreLonerRestrictionsInteraction)&&isAutonomous&&(actor.SimDescription.ChildOrAbove&&target.SimDescription.ChildOrAbove)&&(actor.HasTrait(TraitNames.Loner)&&actor.Conversation==null)){
                Relationship relationship=Relationship.Get(actor,target,false);
                          if(relationship==null||!relationship.AreFriends())
        return InteractionTestResult.Social_LonersMustBeFriends;
            }
            if(!interactionHasAlreadyStarted){
                if(!actor.IsSelectable&&headInteraction1!=null&&headInteraction1.Target is Sims3.Gameplay.Abstracts.RabbitHole)
        return InteractionTestResult.Social_TargetIsGoingToRabbitHole;
                if(isAutonomous&&headInteraction1!=null&&(headInteraction1.SatisfiesCommodity(CommodityKind.Hunger)||headInteraction1.SatisfiesCommodity(CommodityKind.Bladder)))
        return InteractionTestResult.Social_TargetIsEatingOrPeeing;
                if(isAutonomous&&headInteraction1!=null&&headInteraction1.GetPriority().Level>=InteractionPriorityLevel.UserDirected)
        return InteractionTestResult.Social_TargetRunningUserDirectedInteraction;
            }
            ActionData actionData=(ActionData)null;
            if(social!=null)
                       actionData=ActionData.Get(social);
            if(isAutonomous){
                if(actor.IsPet&&target.IsPet){
                    CASAgeGenderFlags age1= actor.SimDescription.Age;
                    CASAgeGenderFlags age2=target.SimDescription.Age;
                    if(actor.HasTrait(TraitNames.ProudPet)&&age1>age2||target.HasTrait(TraitNames.ProudPet)&&age2>age1)
        return InteractionTestResult.Social_ProhibitedByProudPetTraitAgeConsiderations;
                }
                bool isOutside1=target.IsOutside;
                bool isOutside2= actor.IsOutside;
                Lot lotCurrent1=target.LotCurrent;
                Lot lotCurrent2= actor.LotCurrent;
                bool flag3=lotCurrent1==target.LotHome;
                bool flag4=lotCurrent2== actor.LotHome;
                if(lotCurrent2.IsResidentialLot){
                    if(!isOutside2&&isOutside1&&!target.IsGreetedOnLot(lotCurrent2))
        return InteractionTestResult.Social_TargetIsUngreetedOnCurrentLot;
                    if(!isOutside1&&!flag4&&(isOutside2&&!actor.IsGreetedOnLot(lotCurrent2)))
        return InteractionTestResult.Social_ActorIsUngreetedOnCurrentLot;
                }
                if(lotCurrent1.IsResidentialLot){
                    if(isOutside2&&!isOutside1&&!actor.IsGreetedOnLot(lotCurrent1))
        return InteractionTestResult.Social_ActorIsUngreetedOnTargetLot;
                    if(flag4&&!isOutside2&&(!flag3 && isOutside1)&&!target.IsGreetedOnLot(lotCurrent1)&&(social==null||actionData!=null&&!actionData.CanUngreetedSimsReceiveThisSocial))
        return InteractionTestResult.Social_TargetIsUngreetedOnTargetLot;
                }
                if(!(headInteraction1 is IIgnoreCelebrityRestrictions)&&(actor.SimDescription.IsCelebrity||target.SimDescription.IsCelebrity)&&((long)Math.Abs(actor.CelebrityManager.GetCelebrityLevelDelta(target.SimDescription))>(long)CelebrityManager.kCelebrityLevelsApartForAutonomousSocialization&&actor.Household!=target.Household&&!target.CelebrityManager.HasBeenImpressedBy(actor.SimDescription))){
                    Relationship relationship=Relationship.Get(actor,target,false);
                              if(relationship==null||!relationship.AreFriendsOrRomantic())
        return InteractionTestResult.Social_CelebrityLevelDifferenceTooGreat;
                }
                if(actionData!=null){
                    if(!actionData.IsAllowedWhileHoldingADrink&&(actor.GetObjectInRightHand()is IGlass||target.GetObjectInRightHand() is IGlass))
        return InteractionTestResult.Social_ActorOrTargetIsHoldingADrink;
                    if(actionData.DisallowWhileCarryingMinorPet&&(actor.Posture is MinorPetCarryPosture||target.Posture is MinorPetCarryPosture))
        return InteractionTestResult.Social_ActorOrTargetIsCarryingMinorPet;
                    if(!actionData.IsAllowedWhileCarryingUmbrella&&(actor.Posture.Satisfies(CommodityKind.HoldingUmbrella,(IGameObject)null)||target.Posture.Satisfies(CommodityKind.HoldingUmbrella,(IGameObject)null)))
        return InteractionTestResult.Social_ActorOrTargetIsCarryingUmbrella;
                }
                if(target.IsInBeingRiddenPosture&&actor.IsHuman&&(!target.IsBeingRiddenBy(actor)&&actionData!=null))
        return InteractionTestResult.Social_MountedTargetProhibitedSocialization;
                if(target.Posture is TiedToPost&&!(headInteraction1 is IHitchingPostBeUntied))
        return InteractionTestResult.Social_TargetTiedToHitchingPost;
                if(actionData!=null&&actionData.DisallowAutonomousWhileSunburnt&&actor.BuffManager.HasElement(BuffNames.Sunburnt))
        return InteractionTestResult.Social_DisallowAutonomousWhileActorIsSunburnt;
            }
                if(actionData!=null&&!actionData.IsAllowedWhileScubaDiving&&(actor.Posture is ScubaDiving||target.Posture is ScubaDiving))
        return InteractionTestResult.Special_ScubaTestsFailed;
                if(!isAutonomous&&actionData!=null&&(actionData.DoCelebrityImpressCheck&&GameUtils.IsInstalled(ProductVersion.EP3))&&!CelebrityManager.CanSocialize(actor,target))
        return InteractionTestResult.Social_CannotSocializeWithCelebrity;
                if(target.Posture is InRabbitHolePosture)
        return InteractionTestResult.Social_TargetInRabbitHole;
                if(GameUtils.IsInstalled(ProductVersion.EP4)){
                    if(!OccultImaginaryFriend.CanSimSocializeWithSim(actor,target))
        return InteractionTestResult.Social_ImaginaryFriendProhibitedSocialization;
                    if(isAutonomous){
                        Conversation conversation=target.Conversation;
                                  if(conversation!=null&&!OccultImaginaryFriend.CanSimSocializeWithListOfSims(actor,conversation.Members))
        return InteractionTestResult.Social_ImaginaryFriendProhibitedSocialization;
                    }
                }
                if(!target.IsSelectable&&!isAutonomous&&(target.CareerManager.Occupation!=null&&target.CareerManager.Occupation.IsAtWork)&&!target.CanSocializeAtWork){
                    if(target.OccupationAsAcademicCareer!=null){
                        greyedOutTooltipCallback=new GreyedOutTooltipCallback(new GrayedOutTooltipHelper(target.IsFemale,"CannotSocializeInClass",(object)target).GetTooltip);
        return InteractionTestResult.Social_TargetAtWork;
                    }
                    if(target.School==null||!target.School.IsAllowedToWork()){
                        greyedOutTooltipCallback=new GreyedOutTooltipCallback(new GrayedOutTooltipHelper(target.IsFemale,"CannotSocializeAtWork",(object)target).GetTooltip);
        return InteractionTestResult.Social_TargetAtWork;
                    }
                }
                if(!(target.InteractionQueue.GetHeadInteraction()is IPreventSocialization headInteraction2)||headInteraction2.SocializationAllowed(actor,target))
        return InteractionTestResult.Pass;
                greyedOutTooltipCallback=new GreyedOutTooltipCallback(new GrayedOutTooltipHelperIPS(headInteraction2,actor,target).GetTooltip);
        return InteractionTestResult.Social_TargetInteractionPreventsSocialization;
        }
    }
    public class AutonomyFix{
        public static InteractionTestResult CommonTests1(InteractionDefinition interactionDefinition,Sim actor,IGameObject target,InteractionInstanceParameters parameters){
            if(parameters.Autonomous){
                if(actor.Autonomy.IsCacheActive()){
                    if(target.IsBlocked)
        return(InteractionTestResult.Common_Blocked);
                    if(target.Charred)
        return(InteractionTestResult.Common_Charred);
                    if(!target.InWorld&&!target.InInventory)
        return(InteractionTestResult.Common_OutOfWorld);
                }
                Lot lot=target.LotCurrent;
                int num=target.RoomId;
                if(lot==null){
                    LotLocation location=new LotLocation();
                    long lotLocation=(long)World.GetLotLocation(parameters.Hit.mPoint,ref location);
                    num=(int)location.mRoom;
                    lot=LotManager.GetLotAtPoint(parameters.Hit.mPoint);
                }
                if(lot.LotId!=ulong.MaxValue){
                    bool flag=!(interactionDefinition is IIgnoreIsAllowedInRoomCheck);
                    InteractionTuning tuning=parameters.InteractionObjectPair.Tuning;
                    if(flag&&tuning!=null)
                         flag=!tuning.Availability.HasFlags(Availability.FlagField.AllowEvenIfNotAllowedInRoomAutonomous);
                    if(flag&&!(actor.SimDescription!=null&&actor.SimDescription.IsBonehilda)){
                        Sim sim1=actor;
                        int roomId=num;
                        ulong lotId=lot.LotId;
                        if(interactionDefinition is IUseTargetForAutonomousIsAllowedInRoomCheck&&target is Sim sim2){
                            sim1=sim2;
                            roomId=actor.RoomId;
                            lotId=actor.LotCurrent.LotId;
                        }
                        if(!sim1.IsAllowedInRoom(lotId,roomId))
        return(InteractionTestResult.Common_DisallowedInRoom);
                    }
                }
            }
            if(actor!=null){
                if(!actor.IsSelectable&&actor.IsActiveSim&&(actor.SimDescription.DeathStyle!=SimDescription.DeathType.None&&actor.GetHiddenFlags()!=HiddenFlags.Nothing))
        return InteractionTestResult.Common_LastActiveSimDied;
                switch(actor.SimDescription.Age){
                    case CASAgeGenderFlags.Baby:
                        if((InteractionDefinitionUtilities.GetSpecialCaseAgeTests(interactionDefinition)&SpecialCaseAgeTests.DisallowIfActorIsBaby)!=SpecialCaseAgeTests.None)
        return InteractionTestResult.Common_ActorIsBaby;
                        break;
                    case CASAgeGenderFlags.Toddler:
                        SpecialCaseAgeTests specialCaseAgeTests=InteractionDefinitionUtilities.GetSpecialCaseAgeTests(interactionDefinition);
                        if((specialCaseAgeTests&SpecialCaseAgeTests.DisallowIfTargetIsOnDifferentLevel)!=SpecialCaseAgeTests.None&&!actor.Inventory.Contains(target)){
                            if(target is Terrain){
                                LotLocation invalid=LotLocation.Invalid;
                                long lotLocation=(long)World.GetLotLocation(parameters.Hit.mPoint,ref invalid);
                                if((int)invalid.mLevel!=actor.Level)
        return InteractionTestResult.Common_TargetIsOnDifferentLevel;
                            }else if(target.Level!=actor.Level)
        return InteractionTestResult.Common_TargetIsOnDifferentLevel;
                        }
                        if((specialCaseAgeTests&SpecialCaseAgeTests.DisallowIfNotStanding)!=SpecialCaseAgeTests.None&&parameters.Autonomous&&(!actor.Posture.Satisfies(CommodityKind.Standing,(IGameObject)null)&&!actor.Posture.Satisfies(CommodityKind.WalkingToddler,(IGameObject)null)&&!actor.Posture.Satisfies(CommodityKind.InFairyHouse,(IGameObject)null)))
        return InteractionTestResult.Common_NotStanding;
                        break;
                }
            }
            InteractionPriorityLevel level=parameters.Priority.Level;
            if(level<InteractionPriorityLevel.Pregnancy){
                if(actor.Autonomy.BabyIsComing&&!(interactionDefinition is IUsableDuringBirthSequence))
        return InteractionTestResult.Common_Birth;
                if(level<=InteractionPriorityLevel.Fire&&!(actor.Service is GrimReaper)&&(actor.Autonomy.IsOnFire&&!(interactionDefinition is IUsableWhileOnFire)||level<InteractionPriorityLevel.Fire&&actor.Autonomy.IsFireOnLot&&(!target.LotCurrent.FireManager.HasFireStartedByWitch()||!actor.SimDescription.IsWitch)&&!(interactionDefinition is IUsableDuringFire)))
        return InteractionTestResult.Common_OnFire;
            }
            bool flag1=target is Terrain?TombRoomManager.IsPointInAFoggedRoom(parameters.Hit.mPoint):TombRoomManager.IsObjectInAFoggedRoom(target,true);
            if(!(interactionDefinition is IUsableInFoggedRoom)&&flag1&&TombRoomManager.IsObjectFoggable(target))
        return InteractionTestResult.Common_InFoggedRoom;
            if(GameUtils.IsInstalled(ProductVersion.EP6)&&(actor.OccupationAsPerformanceCareer!=null&&actor.OccupationAsPerformanceCareer.IsPerformingAShow&&!(interactionDefinition is IUsableDuringShow)||target is Sim sim&&sim.OccupationAsPerformanceCareer!=null&&(sim.OccupationAsPerformanceCareer.IsPerformingAShow&&!(interactionDefinition is IUsableDuringShow))))
        return InteractionTestResult.Special_IsPerformingShow;
            if(GameUtils.IsInstalled(ProductVersion.EP7)&&actor.SimDescription.IsZombie&&(target!=actor&&target is Sim)&&!(interactionDefinition is IZombieAllowedDefinition))
        return InteractionTestResult.Special_IsZombieForbidden;
            InteractionTestResult interactionTestResult=InteractionDefinitionUtilities.ScubaLotTests(interactionDefinition,actor,target,parameters);
        return interactionTestResult!=InteractionTestResult.Pass?interactionTestResult:InteractionTestResult.Pass;
        }
          public AutonomyFix(Sim actor,Motives motives,AutonomySearchType currentSearchType,bool isActorInTombRoom){
                          mActor=actor;Motives=motives; CurrentSearchType=currentSearchType;
                                                                               IsActorInTombRoom=isActorInTombRoom;
          }
        public Sim mActor;
         public Sim Actor{get{return mActor;}}
            public Motives Motives;
        public AutonomySearchType CurrentSearchType;
            public bool IsActorInTombRoom;
        public InteractionTestResult CheckAvailability1(bool autonomous,Availability availability,InteractionObjectPair iop){
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
            if(availability.CareerThresholdType!=OccupationNames.Undefined){
                bool flag=true;
                if(availability.HasFlags(Availability.FlagField.IncludePastCareers)&&this.mActor.CareerManager.PreviouslySatisfied(availability.CareerThresholdType,availability.CareerThresholdValue))
                     flag=false;
                if(flag){
                    if(this.mActor.Occupation==null)
        return(InteractionTestResult.Tuning_NoCareer);
                    if(this.mActor.Occupation.Guid!=availability.CareerThresholdType)
        return(InteractionTestResult.Tuning_WrongCareer);
                    if(this.mActor.Occupation.HighestLevelAchieved<availability.CareerThresholdValue)
        return(InteractionTestResult.Tuning_CareerLevelTooLow);
                }
            }
            if(availability.ExcludingBuffs!=null){
                foreach(BuffNames excludingBuff in availability.ExcludingBuffs){
                    if(this.mActor.BuffManager.HasElement(excludingBuff))
        return(InteractionTestResult.Tuning_HasBuff);
                }
            }
            if(availability.RequiredBuffs!=null){
                bool flag=false;
                foreach(BuffNames requiredBuff in availability.RequiredBuffs){
                    if(this.mActor.BuffManager.HasElement(requiredBuff)){
                     flag=true;
                break;
                    }
                }
                 if(!flag)
        return(InteractionTestResult.Tuning_MissingBuff);
            }
            Lot lot=iop.Target.GetOwnerLot();
            if(iop.InteractionDefinition is ISoloInteractionDefinition)
                lot=this.mActor.LotCurrent;
            DaycareSituation daycareSituationForSim=DaycareSituation.GetDaycareSituationForSim(this.mActor);
            if((lot==null||!lot.IsResidentialOwnedBy(this.Actor))&&(daycareSituationForSim==null||daycareSituationForSim.Lot!=lot)&&!this.CheckAvailabilityOnLot1(iop,availability,iop.Target.LotCurrent,autonomous))
        return(InteractionTestResult.Tuning_LotAvailability);
            if(availability.HasFlags(Availability.FlagField.DisallowedFromInventory)&&this.mActor.Inventory.Contains(iop.Target))
        return(InteractionTestResult.Tuning_InInventory);
            if(this.CurrentSearchType!=AutonomySearchType.PostureTransition&&autonomous&&!availability.HasFlags(Availability.FlagField.AllowInTombRoomAutonomous)&&(this.IsActorInTombRoom||TombRoomManager.IsObjectInATombRoom(iop.Target)))
        return(InteractionTestResult.Tuning_AutonomousDisableInTombRoom);
            switch(availability.MoodThresholdType){
                case MoodThresholdType.TrueOnlyIfMoodBelowBad:
                    if((double)this.mActor.MoodManager.MoodValue>(double)MoodManager.MoodStrongNegativeValue)
        return(InteractionTestResult.Tuning_MoodIsNotBad);
                    break;
                case MoodThresholdType.TrueOnlyIfMoodAboveBad:
                    if((double)this.mActor.MoodManager.MoodValue<(double)MoodManager.MoodStrongNegativeValue)
        return(InteractionTestResult.Tuning_MoodIsBad);
                    break;
                case MoodThresholdType.TrueOnlyIfMoodBelowThreshold:
                    if((double)this.mActor.MoodManager.MoodValue>(double)availability.MoodThresholdValue)
        return(InteractionTestResult.Tuning_MoodTooHigh);
                    break;
                case MoodThresholdType.TrueOnlyIfMoodAboveThreshold:
                    if((double)this.mActor.MoodManager.MoodValue<(double)availability.MoodThresholdValue)
        return(InteractionTestResult.Tuning_MoodTooLow);
                    break;
            }
        return(availability.OccultRestrictionType!=OccultRestrictionType.Ignore&&(availability.HasFlags(Availability.FlagField.OccultRestrictionsHumanDisallowed)&&this.mActor.CurrentOccultType==Sims3.UI.Hud.OccultTypes.None&&availability.OccultRestrictionType==OccultRestrictionType.Inclusive||!availability.HasFlags(Availability.FlagField.OccultRestrictionsHumanDisallowed)&&this.mActor.CurrentOccultType==Sims3.UI.Hud.OccultTypes.None&&availability.OccultRestrictionType==OccultRestrictionType.Exclusive||((availability.OccultRestrictions^(availability.OccultRestrictions|this.mActor.OccultManager.CurrentOccultTypes))!=Sims3.UI.Hud.OccultTypes.None&&availability.OccultRestrictionType==OccultRestrictionType.Inclusive||(availability.OccultRestrictions&this.mActor.OccultManager.CurrentOccultTypes)!=Sims3.UI.Hud.OccultTypes.None&&availability.OccultRestrictionType==OccultRestrictionType.Exclusive))?InteractionTestResult.Tuning_OccultTypeNotAllowed:InteractionTestResult.Pass);
        }
        public bool CheckAvailabilityOnLot1(InteractionObjectPair iop,Availability availability,Lot lot,bool autonomous){
            if(lot!=null){
                if(lot.IsCommunityLot&&iop.Target.Level!=int.MaxValue&&(iop.Target.Level!=0||!iop.Target.IsOutside)&&(!lot.IsOpenVenue()&&!(iop.InteractionDefinition is IAllowedOnClosedVenues)))
        return false;
                if(this.mActor.SimDescription!=null&&this.mActor.SimDescription.IsBonehilda)
        return true;
            InteractionDefinition interactionDefinition=iop.InteractionDefinition;
                if(autonomous&&!availability.HasFlags(Availability.FlagField.AllowEvenIfNotAllowedInRoomAutonomous)&&!(interactionDefinition is IIgnoreIsAllowedInRoomCheck)){
                    Sim sim=this.mActor;
                    int roomId=iop.Target.RoomId;
                    ulong lotId=lot.LotId;
                    if(interactionDefinition is IUseTargetForAutonomousIsAllowedInRoomCheck&&iop.Target is Sim target){
                        sim=target;
                        roomId=this.mActor.RoomId;
                        lotId=this.mActor.LotCurrent.LotId;
                    }
                    if(!sim.IsAllowedInRoom(lotId,roomId))
        return false;
                }
                if(this.mActor.IsStray&&!this.mActor.IsGreetedOnLot(lot)&&iop.Target.RoomId!=0)
        return false;
                if(lot.LotId!=ulong.MaxValue&&lot!=this.mActor.LotHome&&!availability.HasFlags(Availability.FlagField.AllowOnAllLots)){
                    if(autonomous){
                        if(availability.HasFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideAutonomous)&&iop.Target.IsOutside)
        return true;
                    }else 
                    if(availability.HasFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideUserDirected)&&iop.Target.IsOutside)
        return true;
        return(availability.HasFlags(Availability.FlagField.AllowGreetedSims)&&(this.mActor.Household!=null&&lot.IsResidentialLot&&this.mActor.IsGreetedOnLot(lot)||iop.Target.IsInPublicResidentialRoom)||(availability.HasFlags(Availability.FlagField.AllowOnCommunityLots)&&lot!=null&&lot.IsCommunityLot||this.mActor.LotCurrent==lot&&!this.mActor.IsOutside&&!this.mActor.IsInPublicResidentialRoom));
                }
            }
        return true;}
    }
}