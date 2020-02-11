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
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Interfaces;
using Sims3.Gameplay.Scuba;
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
            if(!CarriedChildInteractionBFix.StartInteractionWithCarriedChild1((SocialInteraction)this,"BeGivenBottle")){
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
    public class CarriedChildInteractionBFix:CarriedChildInteractionB,IPreLoad,IAddInteraction{
                                              public void AddInteraction(InteractionInjectorList interactions){
                                              }
                                   public void OnPreLoad(){
                                   }
        public new class Definition:CarriedChildInteractionB.Definition{
            public Definition(string name):base(name){}
            public Definition(){}
        }




        public static bool StartInteractionWithCarriedChild1(SocialInteraction interactionA,string receptiveInteractionNameKey){
            SocialInteractionB instance=new CarriedChildInteractionBFix.Definition(ChildUtils.Localize(interactionA.Target.IsFemale,receptiveInteractionNameKey)).CreateInstance((IGameObject)interactionA.Actor,(IActor)interactionA.Target,interactionA.GetPriority(),interactionA.EffectivelyAutonomous,interactionA.CancellableByPlayer) as SocialInteractionB;
                    return StartInteractionWithCarriedChild1(interactionA,instance);
        }
        public static bool StartInteractionWithCarriedChild1(SocialInteraction interactionA,SocialInteractionB interactionB){
                        Alive.WriteLog("CarriedChildInteractionBFix:Bonehilda:StartInteractionWithCarriedChild1:GOT HERE");
            interactionB.LinkedInteractionInstance=(InteractionInstance)interactionA;
            interactionA.Target.InteractionQueue.Add((InteractionInstance)interactionB);
            if(!interactionB.CancelNonBSocialInteractionsFromQueue(interactionB.Actor)){
                    if(interactionA.Actor.SimDescription!=null&&interactionA.Actor.SimDescription.IsBonehilda){
                        Alive.WriteLog("CarriedChildInteractionBFix:Bonehilda:StartInteractionWithCarriedChild1:FAIL[0]");
                    }
        return false;
            }
            interactionA.SetInitialRouteComplete();
            while(interactionA.Target.CurrentInteraction!=interactionB){
                if(interactionA.InstanceActor.HasExitReason()){
                    if(interactionA.Actor.SimDescription!=null&&interactionA.Actor.SimDescription.IsBonehilda){
                        Alive.WriteLog("CarriedChildInteractionBFix:Bonehilda:StartInteractionWithCarriedChild1:FAIL[1]:"+interactionA.InstanceActor.ExitReason);
                    }
        return false;
                }
                if(!interactionA.Target.InteractionQueue.HasInteraction((InteractionInstance)interactionB)){
                    if(interactionA.Actor.SimDescription!=null&&interactionA.Actor.SimDescription.IsBonehilda){
                        Alive.WriteLog("CarriedChildInteractionBFix:Bonehilda:StartInteractionWithCarriedChild1:FAIL[2]");
                    }
        return false;
                }
                if(!interactionA.Test()){
                    if(interactionA.Actor.SimDescription!=null&&interactionA.Actor.SimDescription.IsBonehilda){
                        Alive.WriteLog("CarriedChildInteractionBFix:Bonehilda:StartInteractionWithCarriedChild1:FAIL[3]");
                    }
        return false;
                }
Simulator.Sleep(0U);
            }
            interactionA.Actor.SynchronizationRole=Sim.SyncRole.Initiator;
            interactionA.Actor.SynchronizationTarget=interactionA.Target;
            interactionA.Actor.SynchronizationLevel=Sim.SyncLevel.Started;
      if (!interactionA.Actor.WaitForSynchronizationLevelWithSim(interactionA.Target, Sim.SyncLevel.Started, 10f))
      {
        interactionA.Actor.ClearSynchronizationData();
        return false;
      }
      interactionA.Actor.SynchronizationLevel = Sim.SyncLevel.Committed;
      if (interactionA.Actor.WaitForSynchronizationLevelWithSim(interactionA.Target, Sim.SyncLevel.Committed, 10f))
        return true;
      interactionA.Actor.ClearSynchronizationData();
      return false;
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
                    if(this.Actor.SimDescription!=null&&this.Actor.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:Run:FAIL[0]");
                    }
        return false;
                }
                //if(!ChildUtils.StartObjectInteractionWithChild((InteractionInstance)this,this.Target,CommodityKind.Standing,"BePickedUp")){
                        //if(this.Actor.SimDescription!=null&&this.Actor.SimDescription.IsBonehilda){
                        //    Alive.WriteLog("PickUpChild:Bonehilda:Run:FAIL[1.0]");
                        //}
                if(!StartObjectInteractionWithChild1((InteractionInstance)this,this.Target,CommodityKind.Standing,"BePickedUp")){
                        if(this.Actor.SimDescription!=null&&this.Actor.SimDescription.IsBonehilda){
                            Alive.WriteLog("PickUpChild:Bonehilda:Run:FAIL[1.1]");
                        }
        return false;
                }
                //}
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
            }else{
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
        public static bool StartObjectInteractionWithChild1(InteractionInstance interactionA,Sim child,CommodityKind childPosture,string receptiveInteractionNameKey){
            if(child==null||child.HasBeenDestroyed||!interactionA.SafeToSync()){
                Sim instanceActor0=interactionA.InstanceActor;
                    if(instanceActor0.SimDescription!=null&&instanceActor0.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:StartObjectInteractionWithChild1:FAIL[0]");
                    }
        return false;
            }
            Sim instanceActor=interactionA.InstanceActor;
            InteractionInstance instance=new /*ChildUtils.*/ChildPlaceholderInteractionFix.Definition(ChildUtils.Localize(child.IsFemale,receptiveInteractionNameKey)).CreateInstance((IGameObject)instanceActor,(IActor)child,interactionA.GetPriority(),interactionA.Autonomous,interactionA.CancellableByPlayer);
                                instance.LinkedInteractionInstance=interactionA;
            ChildUtils.SetPosturePrecondition(instance,childPosture);
            if(!child.InteractionQueue.Add(instance)){
                    if(instanceActor.SimDescription!=null&&instanceActor.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:StartObjectInteractionWithChild1:FAIL[1]");
                    }
        return false;
            }
            //if(!interactionA.StartSync(true)){
                    //if(instanceActor.SimDescription!=null&&instanceActor.SimDescription.IsBonehilda){
                    //    Alive.WriteLog("PickUpChild:Bonehilda:StartObjectInteractionWithChild1:FAIL[2.0]");
                    //}
            if(!(interactionA is PickUpChildFix pickUp)||pickUp==null||!pickUp.StartSync1(true,false,(SyncLoopCallbackFunction)null,0.0f,true)){
                    if(instanceActor.SimDescription!=null&&instanceActor.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:StartObjectInteractionWithChild1:FAIL[2.1]:"+(interactionA is PickUpChildFix));
                    }
        return false;
            }
            //}
            instanceActor.SynchronizationLevel=Sim.SyncLevel.Committed;
        SimFix fix=new SimFix(instanceActor);
            //if(instanceActor.WaitForSynchronizationLevelWithSim(child,Sim.SyncLevel.Committed,SocialInteraction.kSocialSyncGiveupTime))
            if(fix.WaitForSynchronizationLevelWithSim1(child,Sim.SyncLevel.Committed,SocialInteraction.kSocialSyncGiveupTime))
        return true;
                    if(instanceActor.SimDescription!=null&&instanceActor.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:StartObjectInteractionWithChild1:FAIL[3]");
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
                if(flag){
                    if(this.InstanceActor.SimDescription!=null&&this.InstanceActor.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:StartSync1:flag:"+flag+":"+this.LinkedInteractionInstance);
                    }
                }
                if(syncTarget.InteractionQueue.GetHeadInteraction() is IPreventSocialization headInteraction&&headInteraction!=null&&!headInteraction.SocializationAllowed(this.InstanceActor,syncTarget)||!flag){
                    if(this.InstanceActor.SimDescription!=null&&this.InstanceActor.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:StartSync1:FAIL[3]:"+flag+":"+syncTarget.ExitReason+":"+syncTarget.InteractionQueue.GetHeadInteraction()+" "+(syncTarget.InteractionQueue.GetHeadInteraction() as IPreventSocialization)?.SocializationAllowed(this.InstanceActor,syncTarget));
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
                    if(this.InstanceActor.SimDescription!=null&&this.InstanceActor.SimDescription.IsBonehilda){
            this.InstanceActor.RemoveExitReason(ExitReason.CanceledByScript);
                        //Alive.WriteLog("PickUpChild:Bonehilda:StartSync1:GOT HERE");
                    }
            this.InstanceActor.SynchronizationRole=shouldBeMaster?Sim.SyncRole.Initiator:Sim.SyncRole.Receiver;
            this.InstanceActor.SynchronizationTarget=syncTarget;
            this.InstanceActor.SynchronizationLevel=Sim.SyncLevel.Started;
         SimFix fix=new SimFix(this.InstanceActor);
            if(!fix.WaitForSynchronizationLevelWithSim1(syncTarget,Sim.SyncLevel.Started,exitReason,(float)InteractionInstance.kNumMinToWaitOnSyncStart,loopCallback,notifySimMinutes,performSocializeWithTest)){
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
     //InteractionTestResult interactionTestResult=actor1.Autonomy.CheckAvailability(parameters.Autonomous,tuning.Availability,parameters.InteractionObjectPair);
                        //if(interactionTestResult!=InteractionTestResult.Pass){
                        //    if(actor1.SimDescription!=null&&actor1.SimDescription.IsBonehilda){
                        //Alive.WriteLog("PickUpChild:Bonehilda:TestFix:FAIL[0.0]:"+interactionTestResult);
                        //    }
                    try{
                                     AutonomyFix fix=new AutonomyFix(actor1.Autonomy.mActor,actor1.Autonomy.Motives,actor1.Autonomy.CurrentSearchType,actor1.Autonomy.IsActorInTombRoom);
     InteractionTestResult interactionTestResult=fix.CheckAvailability1(parameters.Autonomous,tuning.Availability,parameters.InteractionObjectPair);
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
                        //}
                    }
                }
                actor1.Autonomy.UpdateCacheIfNeeded((IGameObject)target1);
               //var interactionTestResult1=InteractionDefinitionUtilities.CommonTests((InteractionDefinition)this,actor2,target2,parameters);
                //if(interactionTestResult1!=InteractionTestResult.Pass){
                        //    if(actor1.SimDescription!=null&&actor1.SimDescription.IsBonehilda){
                        //Alive.WriteLog("PickUpChild:Bonehilda:TestFix:FAIL[1.0]:"+interactionTestResult1);
                        //    }
                    try{
               var interactionTestResult1=AutonomyFix.CommonTests1((InteractionDefinition)this,actor2,target2,parameters);
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
                //}
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
                        //    if(actor1.SimDescription!=null&&actor1.SimDescription.IsBonehilda){
                        //Alive.WriteLog("PickUpChild:Bonehilda:TestFix:SUCCESS!");
                        //    }
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
                        //Alive.WriteLog("GOT HERE");
            if(!this.StartSync2(false,false,(SyncLoopCallbackFunction)null,0.0f,true)){
                    if(this.Target.SimDescription!=null&&this.Target.SimDescription.IsBonehilda){
                        Alive.WriteLog("ChildPlaceholderInteraction:Bonehilda:Run:FAIL[0]");
                    }
        return false;
            }
                        //Alive.WriteLog("GOT HERE 2");
            this.Actor.SynchronizationLevel=Sim.SyncLevel.Committed;
         SimFix fix=new SimFix(this.Actor);
            if(!fix.WaitForSynchronizationLevelWithSim1(this.Target,Sim.SyncLevel.Committed,SocialInteraction.kSocialSyncGiveupTime)){
                    if(this.Target.SimDescription!=null&&this.Target.SimDescription.IsBonehilda){
                        Alive.WriteLog("ChildPlaceholderInteraction:Bonehilda:Run:FAIL[1]");
                    }
        return false;
            }
                        //Alive.WriteLog("GOT HERE 3");
        bool flag = this.DoLoop(ExitReason.Default);
        this.CopyExitReasonToLinkedInteraction();
        this.WaitForMasterInteractionToFinish();
        return this.WaitForSyncComplete() && flag;
        }
        public bool StartSync2(bool shouldBeMaster,bool ignoreExitReasons,SyncLoopCallbackFunction loopCallback,float notifySimMinutes,bool performSocializeWithTest){
            if(!this.SafeToSync()){
                    if(this.Target.SimDescription!=null&&this.Target.SimDescription.IsBonehilda){
                        Alive.WriteLog("ChildPlaceholderInteraction:Bonehilda:StartSync2:FAIL[0]");
                    }
        return false;
            }
            Sim syncTarget=this.GetSyncTarget();
            if(syncTarget==null){
                    if(this.Target.SimDescription!=null&&this.Target.SimDescription.IsBonehilda){
                        Alive.WriteLog("ChildPlaceholderInteraction:Bonehilda:StartSync2:FAIL[1]");
                    }
        return false;
            }
            ExitReason exitReason=ignoreExitReasons?ExitReason.None:ExitReason.Default;
            DateAndTime previousDateAndTime1=SimClock.CurrentTime();
            DateAndTime previousDateAndTime2=previousDateAndTime1;
            while((double)SimClock.ElapsedTime(TimeUnit.Minutes,previousDateAndTime1)<(double)InteractionInstance.kNumMinToWaitOnPreSync&&(this.GetTargetCurrentInteraction()==null||this.GetTargetCurrentInteraction().LinkedInteractionInstance!=this)){
                if(this.InstanceActor.HasExitReason(exitReason)){
                    if(this.Target.SimDescription!=null&&this.Target.SimDescription.IsBonehilda){
                        Alive.WriteLog("ChildPlaceholderInteraction:Bonehilda:StartSync2:FAIL[2.0]:"+this.InstanceActor.ExitReason);
                    }
                    if(this.Target.SimDescription==null||!this.Target.SimDescription.IsBonehilda||this.InstanceActor.ExitReason!=ExitReason.CanceledByScript){
                    if(this.Target.SimDescription!=null&&this.Target.SimDescription.IsBonehilda){
                        Alive.WriteLog("ChildPlaceholderInteraction:Bonehilda:StartSync2:FAIL[2.1]:"+this.InstanceActor.ExitReason);
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
                if(flag){
                    if(this.Target.SimDescription!=null&&this.Target.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:StartSync2:flag:"+flag+":"+this.LinkedInteractionInstance);
                    }
                }
                if(syncTarget.InteractionQueue.GetHeadInteraction() is IPreventSocialization headInteraction&&!headInteraction.SocializationAllowed(this.InstanceActor,syncTarget)||!flag){
                    if(this.Target.SimDescription!=null&&this.Target.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:StartSync2:FAIL[3]:"+flag+":"+syncTarget.ExitReason+":"+syncTarget.InteractionQueue.GetHeadInteraction()+" "+(syncTarget.InteractionQueue.GetHeadInteraction() as IPreventSocialization)?.SocializationAllowed(this.InstanceActor,syncTarget));
                    }
        return false;
                }
                if(loopCallback!=null&&(double)SimClock.ElapsedTime(TimeUnit.Minutes,previousDateAndTime2)>=(double)notifySimMinutes){
                    if(!loopCallback()){
                    if(this.Target.SimDescription!=null&&this.Target.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:StartSync2:FAIL[4]");
                    }
        return false;
                    }
              previousDateAndTime2 = SimClock.CurrentTime();
                }
Simulator.Sleep(0U);
            }
                    if(this.Target.SimDescription!=null&&this.Target.SimDescription.IsBonehilda){
            this.InstanceActor.RemoveExitReason(ExitReason.CanceledByScript);
                        //Alive.WriteLog("PickUpChild:Bonehilda:StartSync2:GOT HERE");
                    }
            this.InstanceActor.SynchronizationRole=shouldBeMaster?Sim.SyncRole.Initiator:Sim.SyncRole.Receiver;
            this.InstanceActor.SynchronizationTarget=syncTarget;
            this.InstanceActor.SynchronizationLevel=Sim.SyncLevel.Started;
         SimFix fix=new SimFix(this.InstanceActor);
            if(!fix.WaitForSynchronizationLevelWithSim1(syncTarget,Sim.SyncLevel.Started,exitReason,(float)InteractionInstance.kNumMinToWaitOnSyncStart,loopCallback,notifySimMinutes,performSocializeWithTest)){
                    if(this.Target.SimDescription!=null&&this.Target.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:StartSync2:FAIL[5]");
                    }
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
                    if(sim.SimDescription!=null&&sim.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:WaitForSynchronizationLevelWithSim1:FAIL[0.0]");
                    }
                    if(targetSim.SimDescription!=null&&targetSim.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:WaitForSynchronizationLevelWithSim1:FAIL[0.1]");
                    }
                    sim.OnWaitForSynchronizationLevelWithSimFailed(targetSim);
        return false;
                }
                if(sim.HasExitReason(exitReasonInterrupt)){
                    if(sim.SimDescription!=null&&sim.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:WaitForSynchronizationLevelWithSim1:FAIL[1.0]:"+sim.ExitReason);
                    }
                    if(targetSim.SimDescription!=null&&targetSim.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:WaitForSynchronizationLevelWithSim1:FAIL[1.1]:"+sim.ExitReason);
                    }
                    sim.OnWaitForSynchronizationLevelWithSimFailed(targetSim);
        return false;
                }
                InteractionTestResult result;
                if(performSocializeWithTest&&!IUtil.IsPass(result=CanSocializeWithSyncCheck1((string)null,sim,targetSim,sim.IsInAutonomousInteraction(),ref greyedOutTooltipCallback,true,true))){
                    if(sim.SimDescription!=null&&sim.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:WaitForSynchronizationLevelWithSim1:FAIL[2.0]:"+result);
                    }
                    if(targetSim.SimDescription!=null&&targetSim.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:WaitForSynchronizationLevelWithSim1:FAIL[2.1]:"+result);
                    }
                    sim.OnWaitForSynchronizationLevelWithSimFailed(targetSim);
        return false;
                }
                if(loopCallback!=null&&(double)SimClock.ElapsedTime(TimeUnit.Minutes,previousDateAndTime2)>=(double)notifySimMinutes){
                    if(loopCallback()){
                        previousDateAndTime2=SimClock.CurrentTime();
                    }else{
                    if(sim.SimDescription!=null&&sim.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:WaitForSynchronizationLevelWithSim1:FAIL[3.0]");
                    }
                    if(targetSim.SimDescription!=null&&targetSim.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:WaitForSynchronizationLevelWithSim1:FAIL[3.1]");
                    }
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
                    if(sim.SimDescription!=null&&sim.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:WaitForSynchronizationLevelWithSim1:FAIL[4.0]:"+sim.ExitReason);
                    }
                    if(targetSim.SimDescription!=null&&targetSim.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:WaitForSynchronizationLevelWithSim1:FAIL[4.1]:"+sim.ExitReason);
                    }
                sim.OnWaitForSynchronizationLevelWithSimFailed(targetSim);
        return false;
            }
            if(sim.IsAtSynchronizationLevelWith(targetSim,desiredSynchLevel))
        return true;
                    if(sim.SimDescription!=null&&sim.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:WaitForSynchronizationLevelWithSim1:FAIL[5.0]:"+sim.ExitReason);
                    }
                    if(targetSim.SimDescription!=null&&targetSim.SimDescription.IsBonehilda){
                        Alive.WriteLog("PickUpChild:Bonehilda:WaitForSynchronizationLevelWithSim1:FAIL[5.1]:"+sim.ExitReason);
                    }
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
            if (target.IsSleeping && !SocialInteraction.CanInteractWithSleepingSim(actor, target))
              return InteractionTestResult.Social_TargetSleeping;
            if (target.SimRoutingComponent.IsInCarSequence())
              return InteractionTestResult.Social_TargetInCar;
            InteractionInstance headInteraction1 = target.InteractionQueue.GetHeadInteraction();
            if (!(headInteraction1 is IIgnoreLonerRestrictionsInteraction) && isAutonomous && (actor.SimDescription.ChildOrAbove && target.SimDescription.ChildOrAbove) && (actor.HasTrait(TraitNames.Loner) && actor.Conversation == null))
            {
              Relationship relationship = Relationship.Get(actor, target, false);
              if (relationship == null || !relationship.AreFriends())
                return InteractionTestResult.Social_LonersMustBeFriends;
            }
            if (!interactionHasAlreadyStarted)
            {
              if (!actor.IsSelectable && headInteraction1 != null && headInteraction1.Target is Sims3.Gameplay.Abstracts.RabbitHole)
                return InteractionTestResult.Social_TargetIsGoingToRabbitHole;
              if (isAutonomous && headInteraction1 != null && (headInteraction1.SatisfiesCommodity(CommodityKind.Hunger) || headInteraction1.SatisfiesCommodity(CommodityKind.Bladder)))
                return InteractionTestResult.Social_TargetIsEatingOrPeeing;
              if (isAutonomous && headInteraction1 != null && headInteraction1.GetPriority().Level >= InteractionPriorityLevel.UserDirected)
                return InteractionTestResult.Social_TargetRunningUserDirectedInteraction;
            }
            ActionData actionData = (ActionData) null;
            if (social != null)
              actionData = ActionData.Get(social);
            if (isAutonomous)
            {
              if (actor.IsPet && target.IsPet)
              {
                CASAgeGenderFlags age1 = actor.SimDescription.Age;
                CASAgeGenderFlags age2 = target.SimDescription.Age;
                if (actor.HasTrait(TraitNames.ProudPet) && age1 > age2 || target.HasTrait(TraitNames.ProudPet) && age2 > age1)
                  return InteractionTestResult.Social_ProhibitedByProudPetTraitAgeConsiderations;
              }
              bool isOutside1 = target.IsOutside;
              bool isOutside2 = actor.IsOutside;
              Lot lotCurrent1 = target.LotCurrent;
              Lot lotCurrent2 = actor.LotCurrent;
              bool flag3 = lotCurrent1 == target.LotHome;
              bool flag4 = lotCurrent2 == actor.LotHome;
              if (lotCurrent2.IsResidentialLot)
              {
                if (!isOutside2 && isOutside1 && !target.IsGreetedOnLot(lotCurrent2))
                  return InteractionTestResult.Social_TargetIsUngreetedOnCurrentLot;
                if (!isOutside1 && !flag4 && (isOutside2 && !actor.IsGreetedOnLot(lotCurrent2)))
                  return InteractionTestResult.Social_ActorIsUngreetedOnCurrentLot;
              }
              if (lotCurrent1.IsResidentialLot)
              {
                if (isOutside2 && !isOutside1 && !actor.IsGreetedOnLot(lotCurrent1))
                  return InteractionTestResult.Social_ActorIsUngreetedOnTargetLot;
                if (flag4 && !isOutside2 && (!flag3 && isOutside1) && !target.IsGreetedOnLot(lotCurrent1) && (social == null || actionData != null && !actionData.CanUngreetedSimsReceiveThisSocial))
                  return InteractionTestResult.Social_TargetIsUngreetedOnTargetLot;
              }
              if (!(headInteraction1 is IIgnoreCelebrityRestrictions) && (actor.SimDescription.IsCelebrity || target.SimDescription.IsCelebrity) && ((long) Math.Abs(actor.CelebrityManager.GetCelebrityLevelDelta(target.SimDescription)) > (long) CelebrityManager.kCelebrityLevelsApartForAutonomousSocialization && actor.Household != target.Household && !target.CelebrityManager.HasBeenImpressedBy(actor.SimDescription)))
              {
                Relationship relationship = Relationship.Get(actor, target, false);
                if (relationship == null || !relationship.AreFriendsOrRomantic())
                  return InteractionTestResult.Social_CelebrityLevelDifferenceTooGreat;
              }
              if (actionData != null)
              {
                if (!actionData.IsAllowedWhileHoldingADrink && (actor.GetObjectInRightHand() is IGlass || target.GetObjectInRightHand() is IGlass))
                  return InteractionTestResult.Social_ActorOrTargetIsHoldingADrink;
                if (actionData.DisallowWhileCarryingMinorPet && (actor.Posture is MinorPetCarryPosture || target.Posture is MinorPetCarryPosture))
                  return InteractionTestResult.Social_ActorOrTargetIsCarryingMinorPet;
                if (!actionData.IsAllowedWhileCarryingUmbrella && (actor.Posture.Satisfies(CommodityKind.HoldingUmbrella, (IGameObject) null) || target.Posture.Satisfies(CommodityKind.HoldingUmbrella, (IGameObject) null)))
                  return InteractionTestResult.Social_ActorOrTargetIsCarryingUmbrella;
              }
              if (target.IsInBeingRiddenPosture && actor.IsHuman && (!target.IsBeingRiddenBy(actor) && actionData != null))
                return InteractionTestResult.Social_MountedTargetProhibitedSocialization;
              if (target.Posture is TiedToPost && !(headInteraction1 is IHitchingPostBeUntied))
                return InteractionTestResult.Social_TargetTiedToHitchingPost;
              if (actionData != null && actionData.DisallowAutonomousWhileSunburnt && actor.BuffManager.HasElement(BuffNames.Sunburnt))
                return InteractionTestResult.Social_DisallowAutonomousWhileActorIsSunburnt;
            }
            if (actionData != null && !actionData.IsAllowedWhileScubaDiving && (actor.Posture is ScubaDiving || target.Posture is ScubaDiving))
              return InteractionTestResult.Special_ScubaTestsFailed;
            if (!isAutonomous && actionData != null && (actionData.DoCelebrityImpressCheck && GameUtils.IsInstalled(ProductVersion.EP3)) && !CelebrityManager.CanSocialize(actor, target))
              return InteractionTestResult.Social_CannotSocializeWithCelebrity;
            if (target.Posture is InRabbitHolePosture)
              return InteractionTestResult.Social_TargetInRabbitHole;
            if (GameUtils.IsInstalled(ProductVersion.EP4))
            {
              if (!OccultImaginaryFriend.CanSimSocializeWithSim(actor, target))
                return InteractionTestResult.Social_ImaginaryFriendProhibitedSocialization;
              if (isAutonomous)
              {
                Conversation conversation = target.Conversation;
                if (conversation != null && !OccultImaginaryFriend.CanSimSocializeWithListOfSims(actor, conversation.Members))
                  return InteractionTestResult.Social_ImaginaryFriendProhibitedSocialization;
              }
            }
            if (!target.IsSelectable && !isAutonomous && (target.CareerManager.Occupation != null && target.CareerManager.Occupation.IsAtWork) && !target.CanSocializeAtWork)
            {
              if (target.OccupationAsAcademicCareer != null)
              {
                greyedOutTooltipCallback = new GreyedOutTooltipCallback(new GrayedOutTooltipHelper(target.IsFemale, "CannotSocializeInClass", (object) target).GetTooltip);
                return InteractionTestResult.Social_TargetAtWork;
              }
              if (target.School == null || !target.School.IsAllowedToWork())
              {
                greyedOutTooltipCallback = new GreyedOutTooltipCallback(new GrayedOutTooltipHelper(target.IsFemale, "CannotSocializeAtWork", (object) target).GetTooltip);
                return InteractionTestResult.Social_TargetAtWork;
              }
            }
            if (!(target.InteractionQueue.GetHeadInteraction() is IPreventSocialization headInteraction2) || headInteraction2.SocializationAllowed(actor, target))
              return InteractionTestResult.Pass;
            greyedOutTooltipCallback = new GreyedOutTooltipCallback(new GrayedOutTooltipHelperIPS(headInteraction2, actor, target).GetTooltip);
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