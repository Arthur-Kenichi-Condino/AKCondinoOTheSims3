﻿using Sims3.Gameplay;
using Sims3.Gameplay.ActiveCareer.ActiveCareers;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.ActorSystems.Children;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Careers;
using Sims3.Gameplay.CAS;
using Sims3.Gameplay.Controllers;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Interfaces;
using Sims3.Gameplay.Seasons;
using Sims3.Gameplay.Services;
using Sims3.Gameplay.Skills;
using Sims3.Gameplay.Socializing;
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
        public override bool Run(){
          this.Actor.LookAtManager.DisableLookAts();
          string angle = "";
          if (this.Actor.IsActiveFirefighter && Occupation.SimIsWorkingAtJobOnLot(this.Actor, this.Target.LotCurrent))
          {
            this.SetPriority(new InteractionPriority(InteractionPriorityLevel.Fire, float.MinValue));
            this.RequestWalkStyle(Sim.WalkStyle.FastJog);
          }
          SocialJigTwoPerson objectOutOfWorld;
          if (this.Target.SimDescription.Baby)
          {
            Route route = this.Actor.CreateRoute();
            route.PlanToPointRadialRange((IHasScriptProxy) this.Target, this.Target.Posture.Container.Position, 0.0f, SocialInteraction.kSocialRouteMinDist, Vector3.UnitZ, 360f, RouteDistancePreference.PreferNearestToRouteOrigin, RouteOrientationPreference.TowardsObject, this.Target.LotCurrent.LotId, new int[1]
            {
              this.Target.RoomId
            });
            if(!this.Actor.DoRoute(route)){
                    if(this.Actor.SimDescription!=null&&this.Actor.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:Run:FAIL[0]");
                    }
        return false;
            }
            if(!ChildUtils.StartObjectInteractionWithChild((InteractionInstance)this,this.Target,CommodityKind.Standing,"BePickedUp")){
                    if(this.Actor.SimDescription!=null&&this.Actor.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:Run:FAIL[1.0]");
                    }
            if(!StartObjectInteractionWithChild((InteractionInstance)this,this.Target,CommodityKind.Standing,"BePickedUp")){
                    if(this.Actor.SimDescription!=null&&this.Actor.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:Run:FAIL[1.1]");
                    }
        return false;
            }
            }
            if(!this.Actor.RouteToObjectRadius((IGameObject)this.Target,0.7f)){
                    if(this.Actor.SimDescription!=null&&this.Actor.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:Run:FAIL[2]");
                    }
        return false;
            }
            this.SocialJig = (SocialJig) (GlobalFunctions.CreateObjectOutOfWorld("SocialJigBabyToddler") as SocialJigTwoPerson);
            this.SocialJig.SetOpacity(0.0f, 0.0f);
            Vector3 position = this.Actor.Position;
            Vector3 forwardVector1 = this.Actor.ForwardVector;
            this.SocialJig.SetPosition(position);
            this.SocialJig.SetForward(forwardVector1);
            this.SocialJig.RegisterParticipants(this.Actor, this.Target);
            this.SocialJig.AddToWorld();
            Vector3 forwardOfSlot = this.SocialJig.GetForwardOfSlot(SocialJigTwoPerson.RoutingSlots.SimB);
            Vector3 forwardVector2 = this.Target.ForwardVector;
            double x1 = (double) forwardOfSlot.x;
            double z1 = (double) forwardOfSlot.z;
            double x2 = (double) forwardVector2.x;
            double z2 = (double) forwardVector2.z;
            double x3 = x1 * x2 + z1 * z2;
            double y = x1 * z2 - z1 * x2;
            float num1 = (float) Math.Atan2(y, x3) - 0.7853982f;
            while ((double) num1 < 0.0)
              num1 += 6.283185f;
            float num2 = 0.0f;
            float num3 = (float) (2.0 * (double) num1 / 3.14159274101257);
            if ((double) num3 < 1.0)
            {
              angle = "_270";
              num2 = 1.570796f;
            }
            else if ((double) num3 < 2.0)
            {
              angle = "_180";
              num2 = 3.141593f;
            }
            else if ((double) num3 < 3.0)
            {
              angle = "_90";
              num2 = -1.570796f;
            }
            else
              angle = "";
            this.BabyJig = objectOutOfWorld = GlobalFunctions.CreateObjectOutOfWorld("SocialJigBabyToddler") as SocialJigTwoPerson;
            objectOutOfWorld.SetOpacity(0.0f, 0.0f);
            Vector3 forwardVector3 = this.SocialJig.ForwardVector;
            Vector3 forward = Quaternion.MakeFromEulerAngles(0.0f, (float) Math.Atan2(y, x3) - num2, 0.0f).ToMatrix().TransformVector(forwardVector3);
            objectOutOfWorld.SetPosition(this.Target.Position - forward * 0.7f);
            objectOutOfWorld.SetForward(forward);
            objectOutOfWorld.AddToWorld();
          }
          else
          {
            if (this.Actor.IsAtHome && this.Actor.LotCurrent.HasVirtualResidentialSlots && (!this.Actor.IsInPublicResidentialRoom || this.Actor.Level != this.Target.Level) && !this.Actor.RouteToObjectRadius((IGameObject) this.Target, 0.7f))
              return false;
            this.SocialJig = (SocialJig) (objectOutOfWorld = GlobalFunctions.CreateObjectOutOfWorld("SocialJigBabyToddler") as SocialJigTwoPerson);
            this.SocialJig.SetOpacity(0.0f, 0.0f);
            if (!this.BeginSocialInteraction((InteractionDefinition) new SocialInteractionB.NoAgeOrClosedVenueCheckDefinition(ChildUtils.Localize(this.Target.SimDescription.IsFemale, "BePickedUp"), false), true, false))
              return false;
          }
          if (this.LinkedInteractionInstance.InstanceActor.CurrentInteraction != this.LinkedInteractionInstance)
            return false;
          this.BeginCommodityUpdates();
          ChildUtils.PickUpChild(this.Actor, this.Target, (SocialJig) objectOutOfWorld, angle);
          if (GameUtils.IsInstalled(ProductVersion.EP8) && this.Target.SimDescription.Toddler && (this.Target.CurrentOutfitCategory != OutfitCategories.Outerwear && this.Target.IsOutside) && (double) SeasonsManager.Temperature <= (double) ChangeToddlerClothes.kTemperatureToSwitchToddlerToOuterwear)
          {
            ChangeToddlerClothes instance = ChangeToddlerClothes.Singleton.CreateInstance((IGameObject) this.Target, (IActor) this.Actor, this.GetPriority(), this.Autonomous, this.CancellableByPlayer) as ChangeToddlerClothes;
            instance.Reason = Sim.ClothesChangeReason.TemperatureTooCold;
            int num = (int) instance.StartBuildingOutfit();
            this.TryPushAsContinuation((InteractionInstance) instance);
          }
          this.FinishLinkedInteraction();
          this.EndCommodityUpdates(true);
          this.WaitForSyncComplete();
          return true;
        }
        public static bool StartObjectInteractionWithChild(InteractionInstance interactionA,Sim child,CommodityKind childPosture,string receptiveInteractionNameKey){
            if(child==null||child.HasBeenDestroyed||!interactionA.SafeToSync()){
                Sim instanceActor0=interactionA.InstanceActor;
                    if(instanceActor0.SimDescription!=null&&instanceActor0.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:StartObjectInteractionWithChild:FAIL[0]");
                    }
        return false;
            }
            Sim instanceActor=interactionA.InstanceActor;
            InteractionInstance instance=new ChildUtils.ChildPlaceholderInteraction.Definition(ChildUtils.Localize(child.IsFemale,receptiveInteractionNameKey)).CreateInstance((IGameObject)instanceActor,(IActor)child,interactionA.GetPriority(),interactionA.Autonomous,interactionA.CancellableByPlayer);
                                instance.LinkedInteractionInstance=interactionA;
            ChildUtils.SetPosturePrecondition(instance,childPosture);
            if(!child.InteractionQueue.Add(instance)){
                    if(instanceActor.SimDescription!=null&&instanceActor.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:StartObjectInteractionWithChild:FAIL[1]");
                    }
        return false;
            }
            if(!interactionA.StartSync(true)){
                    if(instanceActor.SimDescription!=null&&instanceActor.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:StartObjectInteractionWithChild:FAIL[2.0]");
                    }
            if(!(interactionA is PickUpChildFix pickUp)||pickUp==null||!pickUp.StartSync1(true,false,(SyncLoopCallbackFunction)null,0.0f,true)){
                    if(instanceActor.SimDescription!=null&&instanceActor.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:StartObjectInteractionWithChild:FAIL[2.1]");
                    }
        return false;
            }
            }
            instanceActor.SynchronizationLevel=Sim.SyncLevel.Committed;
            if(instanceActor.WaitForSynchronizationLevelWithSim(child,Sim.SyncLevel.Committed,SocialInteraction.kSocialSyncGiveupTime))
        return true;
                    if(instanceActor.SimDescription!=null&&instanceActor.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:StartObjectInteractionWithChild:FAIL[3]");
                    }
            instanceActor.ClearSynchronizationData();
        return false;}
        public bool StartSync1(bool shouldBeMaster,bool ignoreExitReasons,SyncLoopCallbackFunction loopCallback,float notifySimMinutes,bool performSocializeWithTest){
            if(!this.SafeToSync()){
                    if(this.InstanceActor.SimDescription!=null&&this.InstanceActor.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:StartSync1:FAIL[0]");
                    }
        return false;
            }
            Sim syncTarget=this.GetSyncTarget();
            if(syncTarget==null){
                    if(this.InstanceActor.SimDescription!=null&&this.InstanceActor.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:StartSync1:FAIL[1]");
                    }
        return false;
            }
            ExitReason exitReason=ignoreExitReasons?ExitReason.None:ExitReason.Default;
            DateAndTime previousDateAndTime1=SimClock.CurrentTime();
            DateAndTime previousDateAndTime2=previousDateAndTime1;
            while((double)SimClock.ElapsedTime(TimeUnit.Minutes,previousDateAndTime1)<(double)InteractionInstance.kNumMinToWaitOnPreSync&&(this.GetTargetCurrentInteraction()==null||this.GetTargetCurrentInteraction().LinkedInteractionInstance!=this)){
                if(this.InstanceActor.HasExitReason(exitReason)){
                    if(this.InstanceActor.SimDescription!=null&&this.InstanceActor.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:StartSync1:FAIL[2.0]:"+this.InstanceActor.ExitReason);
                    }
                    if(this.InstanceActor.SimDescription==null||!this.InstanceActor.SimDescription.IsBonehilda||this.InstanceActor.ExitReason!=ExitReason.CanceledByScript){
                    if(this.InstanceActor.SimDescription!=null&&this.InstanceActor.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:StartSync1:FAIL[2.1]:"+this.InstanceActor.ExitReason);
                    }
        return false;
                    }
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
                    if(this.InstanceActor.SimDescription!=null&&this.InstanceActor.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:StartSync1:FAIL[3]:"+flag);
                    }
        return false;
                }
                if(loopCallback!=null&&(double)SimClock.ElapsedTime(TimeUnit.Minutes,previousDateAndTime2)>=(double)notifySimMinutes){
                    if(!loopCallback()){
                    if(this.InstanceActor.SimDescription!=null&&this.InstanceActor.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:StartSync1:FAIL[4]");
                    }
        return false;
                    }
                    previousDateAndTime2=SimClock.CurrentTime();
                }
                Simulator.Sleep(0U);
            }
            this.InstanceActor.SynchronizationRole=shouldBeMaster?Sim.SyncRole.Initiator:Sim.SyncRole.Receiver;
            this.InstanceActor.SynchronizationTarget=syncTarget;
            this.InstanceActor.SynchronizationLevel=Sim.SyncLevel.Started;
            if(!this.InstanceActor.WaitForSynchronizationLevelWithSim(syncTarget,Sim.SyncLevel.Started,exitReason,(float)InteractionInstance.kNumMinToWaitOnSyncStart,loopCallback,notifySimMinutes,performSocializeWithTest)){
                    if(this.InstanceActor.SimDescription!=null&&this.InstanceActor.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:StartSync1:FAIL[5]");
                    }
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
                    //if(actor.SimDescription!=null&&actor.SimDescription.IsBonehilda){
                    //    Alive.WriteLog("PickUpChild:Bonehilda:Test:FAIL[4]:"+target.Name);
                    //}
            return(false);
                }
                if(!(!actor.SimDescription.ChildOrBelow&&target.Posture.Container==target)){
                    //if(actor.SimDescription!=null&&actor.SimDescription.IsBonehilda){
                    //    Alive.WriteLog("PickUpChild:Bonehilda:Test:FAIL[0]:"        +target.Name+":"+target.Posture+":"+target.Posture.Container+":"+actor.SimDescription.ChildOrBelow);
                    //}
            return(false);
                }else{
                        //Alive.WriteLog("PickUpChild:"+actor.Name+":Test:SUCCESS[0]:"+target.Name+":"+target.Posture+":"+target.Posture.Container+":"+actor.SimDescription.ChildOrBelow);
                }
                if(!(actor.CarryingChildPosture==null)){
                    //if(actor.SimDescription!=null&&actor.SimDescription.IsBonehilda){
                    //    Alive.WriteLog("PickUpChild:Bonehilda:Test:FAIL[1]:"+target.Name);
                    //}
            return(false);
                }
                if(!(target!=actor.Posture.Container)){
                    //if(actor.SimDescription!=null&&actor.SimDescription.IsBonehilda){
                    //    Alive.WriteLog("PickUpChild:Bonehilda:Test:FAIL[2]:"        +target.Name+":"+actor.Posture+":"+actor.Posture.Container);
                    //}
            return(false);
                }else{
                        //Alive.WriteLog("PickUpChild:"+actor.Name+":Test:SUCCESS[2]:"+target.Name+":"+actor.Posture+":"+actor.Posture.Container);
                }
                if(!(!target.Posture.Satisfies(CommodityKind.InFairyHouse,(IGameObject)null))){
                    //if(actor.SimDescription!=null&&actor.SimDescription.IsBonehilda){
                    //    Alive.WriteLog("PickUpChild:Bonehilda:Test:FAIL[3]:"+target.Name);
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
                    //if(parameters.Actor is Sim actor1&&actor1.SimDescription!=null&&actor1.SimDescription.IsBonehilda){
                    //    Alive.WriteLog("PickUpChild:Bonehilda:InteractionTest:FAIL[1]:"+result);
                    //}
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
     InteractionTestResult interactionTestResult=actor1.Autonomy.CheckAvailability(parameters.Autonomous,tuning.Availability,parameters.InteractionObjectPair);
                        if(interactionTestResult!=InteractionTestResult.Pass){
                            if(actor1.SimDescription!=null&&actor1.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:TestFix:FAIL[0.0]:"+interactionTestResult);
                            }
                    try{
                                     AutonomyFix fix=new AutonomyFix(actor1.Autonomy.mActor,actor1.Autonomy.Motives,actor1.Autonomy.CurrentSearchType,actor1.Autonomy.IsActorInTombRoom);
                           interactionTestResult=fix.CheckAvailability1(parameters.Autonomous,tuning.Availability,parameters.InteractionObjectPair);
                        if(interactionTestResult!=InteractionTestResult.Pass){
                            if(actor1.SimDescription!=null&&actor1.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:TestFix:FAIL[0.1]:"+interactionTestResult);
                            }
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
                }
                actor1.Autonomy.UpdateCacheIfNeeded((IGameObject)target1);
               var interactionTestResult1=InteractionDefinitionUtilities.CommonTests((InteractionDefinition)this,actor2,target2,parameters);
                if(interactionTestResult1!=InteractionTestResult.Pass){
                            if(actor1.SimDescription!=null&&actor1.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:TestFix:FAIL[1.0]:"+interactionTestResult1);
                            }
                    try{
                   interactionTestResult1=AutonomyFix.CommonTests1((InteractionDefinition)this,actor2,target2,parameters);
                if(interactionTestResult1!=InteractionTestResult.Pass){
                            if(actor1.SimDescription!=null&&actor1.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:TestFix:FAIL[1.1]:"+interactionTestResult1);
                            }
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
                            if(actor1.SimDescription!=null&&actor1.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:TestFix:SUCCESS!");
                            }
            return(InteractionTestResult.Pass);
            }
            //----------------------------------------
        }
    }
    public class SimFix{
          public SimFix(Sim sim){
                   this.sim=sim;
          }
          public Sim sim;
        public bool WaitForSynchronizationLevelWithSim1(Sim targetSim,Sim.SyncLevel desiredSynchLevel,ExitReason exitReasonInterrupt,float giveupTime,SyncLoopCallbackFunction loopCallback,float notifySimMinutes,bool performSocializeWithTest){
            DateAndTime previousDateAndTime1=SimClock.CurrentTime();
            DateAndTime previousDateAndTime2=previousDateAndTime1;
            GreyedOutTooltipCallback greyedOutTooltipCallback=(GreyedOutTooltipCallback)null;
            sim.SynchronizationSleeping=sim.SynchronizationLevel>=desiredSynchLevel;
            while(!sim.IsAtSynchronizationLevelWith(targetSim,desiredSynchLevel)){
                if((double)SimClock.ElapsedTime(TimeUnit.Minutes,previousDateAndTime1)>=(double)giveupTime){
                    sim.OnWaitForSynchronizationLevelWithSimFailed(targetSim);
                    if(sim.SimDescription!=null&&sim.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:WaitForSynchronizationLevelWithSim1:FAIL[0]");
                    }
        return false;
                }
                if(sim.HasExitReason(exitReasonInterrupt)){
                    sim.OnWaitForSynchronizationLevelWithSimFailed(targetSim);
                    if(sim.SimDescription!=null&&sim.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:WaitForSynchronizationLevelWithSim1:FAIL[1]");
                    }
        return false;
                }
                if(performSocializeWithTest&&!IUtil.IsPass(SocialInteractionA.Definition.CanSocializeWithSyncCheck((string)null,sim,targetSim,sim.IsInAutonomousInteraction(),ref greyedOutTooltipCallback,true,true))){
                    sim.OnWaitForSynchronizationLevelWithSimFailed(targetSim);
                    if(sim.SimDescription!=null&&sim.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:WaitForSynchronizationLevelWithSim1:FAIL[2]");
                    }
        return false;
                }
            if (loopCallback != null && (double) SimClock.ElapsedTime(TimeUnit.Minutes, previousDateAndTime2) >= (double) notifySimMinutes)
            {
              if (loopCallback())
              {
                previousDateAndTime2 = SimClock.CurrentTime();
              }
              else
              {
                this.OnWaitForSynchronizationLevelWithSimFailed(targetSim);
                return false;
              }
            }
            Simulator.Sleep(0U);
            }
          this.SynchronizationSleeping = false;
          ExitReason reason = exitReasonInterrupt;
          if (this.SynchronizationRole == Sim.SyncRole.Receiver)
            reason &= ExitReason.SynchronizationFailed;
          if (this.HasExitReason(reason))
          {
            this.OnWaitForSynchronizationLevelWithSimFailed(targetSim);
            return false;
          }
          if (this.IsAtSynchronizationLevelWith(targetSim, desiredSynchLevel))
            return true;
          this.OnWaitForSynchronizationLevelWithSimFailed(targetSim);
          return false;
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
          if (actor != null)
          {
            if (!actor.IsSelectable && actor.IsActiveSim && (actor.SimDescription.DeathStyle != SimDescription.DeathType.None && actor.GetHiddenFlags() != HiddenFlags.Nothing))
              return InteractionTestResult.Common_LastActiveSimDied;
            switch (actor.SimDescription.Age)
            {
              case CASAgeGenderFlags.Baby:
                if ((InteractionDefinitionUtilities.GetSpecialCaseAgeTests(interactionDefinition) & SpecialCaseAgeTests.DisallowIfActorIsBaby) != SpecialCaseAgeTests.None)
                  return InteractionTestResult.Common_ActorIsBaby;
                break;
              case CASAgeGenderFlags.Toddler:
                SpecialCaseAgeTests specialCaseAgeTests = InteractionDefinitionUtilities.GetSpecialCaseAgeTests(interactionDefinition);
                if ((specialCaseAgeTests & SpecialCaseAgeTests.DisallowIfTargetIsOnDifferentLevel) != SpecialCaseAgeTests.None && !actor.Inventory.Contains(target))
                {
                  if (target is Terrain)
                  {
                    LotLocation invalid = LotLocation.Invalid;
                    long lotLocation = (long) World.GetLotLocation(parameters.Hit.mPoint, ref invalid);
                    if ((int) invalid.mLevel != actor.Level)
                      return InteractionTestResult.Common_TargetIsOnDifferentLevel;
                  }
                  else if (target.Level != actor.Level)
                    return InteractionTestResult.Common_TargetIsOnDifferentLevel;
                }
                if ((specialCaseAgeTests & SpecialCaseAgeTests.DisallowIfNotStanding) != SpecialCaseAgeTests.None && parameters.Autonomous && (!actor.Posture.Satisfies(CommodityKind.Standing, (IGameObject) null) && !actor.Posture.Satisfies(CommodityKind.WalkingToddler, (IGameObject) null) && !actor.Posture.Satisfies(CommodityKind.InFairyHouse, (IGameObject) null)))
                  return InteractionTestResult.Common_NotStanding;
                break;
            }
          }
          InteractionPriorityLevel level = parameters.Priority.Level;
          if (level < InteractionPriorityLevel.Pregnancy)
          {
            if (actor.Autonomy.BabyIsComing && !(interactionDefinition is IUsableDuringBirthSequence))
              return InteractionTestResult.Common_Birth;
            if (level <= InteractionPriorityLevel.Fire && !(actor.Service is GrimReaper) && (actor.Autonomy.IsOnFire && !(interactionDefinition is IUsableWhileOnFire) || level < InteractionPriorityLevel.Fire && actor.Autonomy.IsFireOnLot && (!target.LotCurrent.FireManager.HasFireStartedByWitch() || !actor.SimDescription.IsWitch) && !(interactionDefinition is IUsableDuringFire)))
              return InteractionTestResult.Common_OnFire;
          }
          bool flag1 = target is Terrain ? TombRoomManager.IsPointInAFoggedRoom(parameters.Hit.mPoint) : TombRoomManager.IsObjectInAFoggedRoom(target, true);
          if (!(interactionDefinition is IUsableInFoggedRoom) && flag1 && TombRoomManager.IsObjectFoggable(target))
            return InteractionTestResult.Common_InFoggedRoom;
          if (GameUtils.IsInstalled(ProductVersion.EP6) && (actor.OccupationAsPerformanceCareer != null && actor.OccupationAsPerformanceCareer.IsPerformingAShow && !(interactionDefinition is IUsableDuringShow) || target is Sim sim && sim.OccupationAsPerformanceCareer != null && (sim.OccupationAsPerformanceCareer.IsPerformingAShow && !(interactionDefinition is IUsableDuringShow))))
            return InteractionTestResult.Special_IsPerformingShow;
          if (GameUtils.IsInstalled(ProductVersion.EP7) && actor.SimDescription.IsZombie && (target != actor && target is Sim) && !(interactionDefinition is IZombieAllowedDefinition))
            return InteractionTestResult.Special_IsZombieForbidden;
          InteractionTestResult interactionTestResult = InteractionDefinitionUtilities.ScubaLotTests(interactionDefinition, actor, target, parameters);
          return interactionTestResult != InteractionTestResult.Pass ? interactionTestResult : InteractionTestResult.Pass;
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