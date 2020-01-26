using Sims3.Gameplay;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Controllers;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Interfaces;
using Sims3.Gameplay.Objects.Appliances;
using Sims3.Gameplay.Objects.Miscellaneous;
using Sims3.SimIFace;
using System;
using System.Collections.Generic;
using System.Text;
using static ArthurGibraltarSims3Mod.Alive;
using static ArthurGibraltarSims3Mod.Interaction;
namespace ArthurGibraltarSims3Mod{
    public class ClothingPileDryCleanUpEx:ClothingPileDry.CleanUp,IPreLoad,IAddInteraction{
        static InteractionDefinition sOldSingleton;
                                              public void AddInteraction(InteractionInjectorList interactions){
                                                                                                 interactions.ReplaceNoTest<ClothingPileDry,ClothingPileDry.CleanUp.Definition>(Singleton);
                                              }
                                   public void OnPreLoad(){
            Tunings.Inject<Sims3.Gameplay.Objects.Miscellaneous.ClothingPileDry,ClothingPileDry.CleanUp.Definition,Definition>(false);
                                     sOldSingleton=Singleton;
                                                   Singleton=new Definition();
                                   }
            public override bool Run(){
                if(this.Target.Parent!=this.Actor&&!(CarrySystem.PickUpWithoutRouting(this.Actor,(ICustomCarryable)this.Target,new SacsEventHandler(this.Target.OnPickUp),0U))){
                Alive.WriteLog("WTF?");
            return(false);
                }
                this.Actor.PlayRouteFailFrequency=Sim.RouteFailFrequency.NeverPlayRouteFail;
this.BeginCommodityUpdates();
            if (this.ActiveStage != null)
            {
              ClothingPileDry currentPile;
              do
              {
                currentPile = (this.ActiveStage as RoomVsLotStage<ClothingPileDry>).GetNext();
                if (currentPile != null)
                {
                  if (this.Actor.RouteToObjectRadiusAndCheckInUse((IGameObject) currentPile, currentPile.CarryRouteToObjectRadius) && currentPile.Parent == null)
                  {
                    this.Actor.CarryStateMachine.AddOneShotScriptEventHandler(113U, (SacsEventHandler) ((A_1, A_2) => currentPile.FadeOut(false)));
                    this.Actor.CarryStateMachine.RequestState("x", "PickUpAnother");
                    this.Actor.CarryStateMachine.RequestState("x", "Carry");
                    this.Target.AddClothingPile(currentPile);
                    currentPile.Destroy();
                  }
                  this.Actor.RemoveExitReason(ExitReason.RouteFailed | ExitReason.ObjectInUse);
                }
              }
              while (currentPile != null && !this.Actor.HasExitReason());
            }
            this.Actor.PlayRouteFailFrequency = Sim.RouteFailFrequency.AlwaysPlayRouteFail;
            this.Stages = (List<Sims3.Gameplay.Interactions.Stage>) null;
            this.Actor.InteractionQueue.FireQueueChanged();
            if (!this.Actor.HasExitReason())
            {
              this.EndCommodityUpdates(true);
              Hamper closestObject1 = GlobalFunctions.GetClosestObject<Hamper>((IEnumerable<Hamper>) Sims3.Gameplay.Queries.GetObjects<Hamper>(this.Actor.Position, ClothingPileDry.kRadiusToConsiderHampers), (IGameObject) this.Actor, new Predicate<Hamper>(ClothingPileDry.CleanUp.DoesHamperHaveSpaceLeft));
              if (closestObject1 != null)
              {
                this.Actor.InteractionQueue.PushAsContinuation(Hamper.DropClothes.Singleton, (IGameObject) closestObject1, true);
                return true;
              }
              if (!this.Autonomous || !this.Target.LotCurrent.LaundryManager.GivesFreshClothingBuff)
              {
                WashingMachine closestObject2 = GlobalFunctions.GetClosestObject<WashingMachine>((IEnumerable<WashingMachine>) Sims3.Gameplay.Queries.GetObjects<WashingMachine>(this.Actor.LotCurrent), (IGameObject) this.Actor, new Predicate<WashingMachine>(ClothingPileDry.CleanUp.IsWashingMachineUsable));
                if (closestObject2 != null)
                {
                  this.Actor.InteractionQueue.PushAsContinuation(WashingMachine.DoLaundry.SingletonNoStages, (IGameObject) closestObject2, true);
                  return true;
                }
              }
              Hamper closestObject3 = GlobalFunctions.GetClosestObject<Hamper>((IEnumerable<Hamper>) Sims3.Gameplay.Queries.GetObjects<Hamper>(this.Actor.LotCurrent), (IGameObject) this.Actor);
              if (closestObject3 != null)
              {
                this.Actor.InteractionQueue.PushAsContinuation(Hamper.DropClothes.Singleton, (IGameObject) closestObject3, true);
                return true;
              }
              WashingMachine closestObject4 = GlobalFunctions.GetClosestObject<WashingMachine>((IEnumerable<WashingMachine>) Sims3.Gameplay.Queries.GetObjects<WashingMachine>(this.Actor.LotCurrent), (IGameObject) this.Actor);
              if (closestObject4 == null)
                return this.Target.PutInInventory(this.Actor);
              this.Actor.InteractionQueue.PushAsContinuation(WashingMachine.DoLaundry.SingletonNoStages, (IGameObject) closestObject4, false);
              return true;
            }
            this.EndCommodityUpdates(false);
            return false;
            }
        public new class Definition:ClothingPileDry.CleanUp.Definition{
            public override InteractionInstance CreateInstance(ref InteractionInstanceParameters parameters){
                            InteractionInstance na=new ClothingPileDryCleanUpEx();
                                                na.Init(ref parameters);
                                         return na;
            }
            public override bool Test(Sim a,ClothingPileDry target,bool isAutonomous,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
            return(!target.mIsBag);
            }
        }
    }
    public class ClothingPileWetDryClothesInDryerEx:ClothingPileWet.DryClothesInDryer,IPreLoad,IAddInteraction{
        static InteractionDefinition sOldSingleton;
                                              public void AddInteraction(InteractionInjectorList interactions){
                                                                                                 interactions.ReplaceNoTest<ClothingPileWet,ClothingPileWet.DryClothesInDryer.Definition>(Singleton);
                                              }
                                   public void OnPreLoad(){
            Tunings.Inject<Sims3.Gameplay.Objects.Miscellaneous.ClothingPileWet,ClothingPileWet.DryClothesInDryer.Definition,Definition>(false);
                                     sOldSingleton=Singleton;
                                                   Singleton=new Definition();
                                   }
        public new class Definition:ClothingPileWet.DryClothesInDryer.Definition{
            public override InteractionInstance CreateInstance(ref InteractionInstanceParameters parameters){
                            InteractionInstance na=new ClothingPileWetDryClothesInDryerEx();
                                                na.Init(ref parameters);
                                         return na;
            }
            public override bool Test(Sim a,ClothingPileWet target,bool isAutonomous,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
                if(Sims3.Gameplay.Queries.CountObjects<Dryer>(target.LotCurrent)==0U){
                                                                                                                  greyedOutTooltipCallback=new GreyedOutTooltipCallback(ClothingPileWet.DryClothesInDryer.NoDryersOnLotToolTip);
            return(false);
                }
                if(Dryer.FindClosestAvailibleDryer((GameObject)target,a)==null){
                                                                                                                  greyedOutTooltipCallback=new GreyedOutTooltipCallback(ClothingPileWet.DryClothesInDryer.NoAvailibleDryersOnLot);
            return(false);
                }
            return( true);
            }
        }
    }
    public class ClothingPileWetDryClothesOnClotheslineEx:ClothingPileWet.DryClothesOnClothesline,IPreLoad,IAddInteraction{
        static InteractionDefinition sOldSingleton;
                                              public void AddInteraction(InteractionInjectorList interactions){
                                                                                                 interactions.ReplaceNoTest<ClothingPileWet,ClothingPileWet.DryClothesOnClothesline.Definition>(Singleton);
                                              }
                                   public void OnPreLoad(){
            Tunings.Inject<Sims3.Gameplay.Objects.Miscellaneous.ClothingPileWet,ClothingPileWet.DryClothesOnClothesline.Definition,Definition>(false);
                                     sOldSingleton=Singleton;
                                                   Singleton=new Definition();
                                   }
        public new class Definition:ClothingPileWet.DryClothesOnClothesline.Definition{
            public override InteractionInstance CreateInstance(ref InteractionInstanceParameters parameters){
                            InteractionInstance na=new ClothingPileWetDryClothesOnClotheslineEx();
                                                na.Init(ref parameters);
                                         return na;
            }
            public override bool Test(Sim a,ClothingPileWet target,bool isAutonomous,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
                if(Sims3.Gameplay.Queries.CountObjects<Clothesline>(target.LotCurrent)==0U){
                                                                                                                  greyedOutTooltipCallback=new GreyedOutTooltipCallback(ClothingPileWet.DryClothesOnClothesline.NoClotheslinesOnLotToolTip);
            return(false);
                }
                if(Clothesline.FindClosestAvailibleClothesline((GameObject)target,a)==null){
                                                                                                                  greyedOutTooltipCallback=new GreyedOutTooltipCallback(ClothingPileWet.DryClothesOnClothesline.NoAvailibleClotheslinesOnLot);
            return(false);
                }
            return( true);
            }
        }
    }
    public class HamperPickUpEx:Hamper.PickUp,IPreLoad,IAddInteraction{
        static InteractionDefinition sOldSingleton;
                                              public void AddInteraction(InteractionInjectorList interactions){
                                                                                                 interactions.ReplaceNoTest<Hamper,Hamper.PickUp.Definition>(Singleton);
                                              }
                                   public void OnPreLoad(){
            Tunings.Inject<Sims3.Gameplay.Objects.Miscellaneous.Hamper,Hamper.PickUp.Definition,Definition>(false);
                                     sOldSingleton=Singleton;
                                                   Singleton=new Definition();
                                   }
            public override bool Run(){
if(!this.Target.RouteToHamper(this.Actor)||!this.Target.HasClothingPiles())
            return(false);
                this.BeginCommodityUpdates();
                this.Target.PickUpClothes(this.Actor);
                this.EndCommodityUpdates(true);
            return( true);
            }
        public new class Definition:Hamper.PickUp.Definition{
            public override InteractionInstance CreateInstance(ref InteractionInstanceParameters parameters){
                            InteractionInstance na=new HamperPickUpEx();
                                                na.Init(ref parameters);
                                         return na;
            }
            public override bool Test(Sim a,Hamper target,bool isAutonomous,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
        return(target.HasClothingPiles());
            }
        }
    }
    public class HamperDoLaundryEx:Hamper.DoLaundry,IPreLoad,IAddInteraction{
        static InteractionDefinition sOldSingleton;
                                              public void AddInteraction(InteractionInjectorList interactions){
                                                                                                 interactions.ReplaceNoTest<Hamper,Hamper.DoLaundry.Definition>(Singleton);
                                              }
                                   public void OnPreLoad(){
            Tunings.Inject<Sims3.Gameplay.Objects.Miscellaneous.Hamper,Hamper.DoLaundry.Definition,Definition>(false);
                                     sOldSingleton=Singleton;
                                                   Singleton=new Definition();
                                   }
            public override bool Run(){
WashingMachine closestObject=GlobalFunctions.GetClosestObject<WashingMachine>((IEnumerable<WashingMachine>)this.Target.LotCurrent.GetObjects<WashingMachine>(),(IGameObject)this.Target,new Predicate<WashingMachine>(Hamper.DoLaundry.IsWashingMachineUsable));
            if(closestObject==null)
            return(false);
this.Actor.InteractionQueue.PushAsContinuation(Hamper.PickUp           .Singleton,(IGameObject)this.Target  ,true);
this.Actor.InteractionQueue.PushAsContinuation(WashingMachine.DoLaundry.Singleton,(IGameObject)closestObject,true);
            return( true);
            }
        public new class Definition:Hamper.DoLaundry.Definition{
            public override InteractionInstance CreateInstance(ref InteractionInstanceParameters parameters){
                            InteractionInstance na=new HamperDoLaundryEx();
                                                na.Init(ref parameters);
                                         return na;
            }
            public override bool Test(Sim a,Hamper target,bool isAutonomous,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
                                               if(!target.HasClothingPiles()||
                                                   target.LotCurrent==null)
            return(false);
if(Sims3.Gameplay.Queries.CountObjects<WashingMachine>(target.LotCurrent)==0U||target.LotCurrent.GetObjects<WashingMachine>(new Predicate<WashingMachine>(Hamper.DoLaundry.IsWashingMachineUsable)).Count==0){
                                                                                                         greyedOutTooltipCallback=new GreyedOutTooltipCallback(Hamper.DoLaundry.Definition.NoWashingMachinesTooltip);
            return(false);
}
            return( true);
            }
        }
    }
    public class WashingMachineDoLaundryEx:WashingMachine.DoLaundry,IPreLoad,IAddInteraction{
        static InteractionDefinition sOldSingleton;
                                              public void AddInteraction(InteractionInjectorList interactions){
                                                                                                 interactions.ReplaceNoTest<WashingMachine,WashingMachine.DoLaundry.Definition>(Singleton);
                                              }
                                   public void OnPreLoad(){
            Tunings.Inject<Sims3.Gameplay.Objects.Appliances.WashingMachineCheap    ,WashingMachine.DoLaundry.Definition,Definition>(false);
            Tunings.Inject<Sims3.Gameplay.Objects.Appliances.WashingMachineExpensive,WashingMachine.DoLaundry.Definition,Definition>(false);
            Tunings.Inject<Sims3.Gameplay.Objects.Appliances.WashingMachine,WashingMachine.DoLaundry.Definition,Definition>(false);
                                     sOldSingleton=Singleton;
                                                   Singleton=new Definition();
                                   }
        public new class Definition:WashingMachine.DoLaundry.Definition{
            public override InteractionInstance CreateInstance(ref InteractionInstanceParameters parameters){
                            InteractionInstance na=new WashingMachineDoLaundryEx();
                                                na.Init(ref parameters);
                                         return na;
            }
            public override bool Test(Sim a,WashingMachine target,bool isAutonomous,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
                if(!target.CanDoLaundry())
            return(false);
                if(target.mWashState==WashingMachine.WashState.HasDirtyLaundry)
            return( true);
                if(a.IsSelectable&&target.LotCurrent.IsCommunityLot&&a.FamilyFunds<target.Tuning.kCostToOperate){
                                                                                                                 greyedOutTooltipCallback=InteractionInstance.CreateTooltipCallback(WashingMachine.LocalizeString("InsufficientFunds"));
            return(false);
                }
                if(!target.LotCurrent.IsResidentialLot)
            return( true);
                if(Sims3.Gameplay.Queries.CountObjects<ClothingPileDry>(target.LotCurrent)>0U)
            return( true);
                foreach(Hamper hamper in target.LotCurrent.GetObjects<Hamper>()){
                            if(hamper.HasClothingPiles())
            return( true);
                }
                if(a.Inventory.ContainsType(typeof(ClothingPileDry),1))
            return( true);
                                                                                                                 greyedOutTooltipCallback=new GreyedOutTooltipCallback(WashingMachine.DoLaundry.Definition.NoLaundryTooltip);
            return(false);
            }
        }
    }
    public class WashingMachineDryClothesInDryerEx:WashingMachine.DryClothesInDryer,IPreLoad,IAddInteraction{
        static InteractionDefinition sOldSingleton;
                                              public void AddInteraction(InteractionInjectorList interactions){
                                                                                                 interactions.ReplaceNoTest<WashingMachine,WashingMachine.DryClothesInDryer.Definition>(Singleton);
                                              }
                                   public void OnPreLoad(){
            Tunings.Inject<Sims3.Gameplay.Objects.Appliances.WashingMachineCheap    ,WashingMachine.DryClothesInDryer.Definition,Definition>(false);
            Tunings.Inject<Sims3.Gameplay.Objects.Appliances.WashingMachineExpensive,WashingMachine.DryClothesInDryer.Definition,Definition>(false);
            Tunings.Inject<Sims3.Gameplay.Objects.Appliances.WashingMachine,WashingMachine.DryClothesInDryer.Definition,Definition>(false);
                                     sOldSingleton=Singleton;
                                                   Singleton=new Definition();
                                   }
        public new class Definition:WashingMachine.DryClothesInDryer.Definition{
            public override InteractionInstance CreateInstance(ref InteractionInstanceParameters parameters){
                            InteractionInstance na=new WashingMachineDryClothesInDryerEx();
                                                na.Init(ref parameters);
                                         return na;
            }
            public override bool Test(Sim a,WashingMachine target,bool isAutonomous,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
                if(target.mWashState!=WashingMachine.WashState.HasCleanLaundry)
            return(false);
                if(!WashingMachine.DryClothesBase.DoesDryerExist(target.LotCurrent)){
                    if(!target.LotCurrent.IsCommunityLot&&!WashingMachine.DryClothesBase.DoesClotheslineExist(target.LotCurrent))
                                                                                                                 greyedOutTooltipCallback=new GreyedOutTooltipCallback(WashingMachine.DryClothesInDryer.NoDryersOnLotToolTip);
            return(false);
                }
                if(Dryer.FindClosestAvailibleDryer((GameObject)target,a)==null){
                                                                                                                 greyedOutTooltipCallback = new GreyedOutTooltipCallback(WashingMachine.DryClothesInDryer.NoAvailibleDryersOnLot);
            return(false);
                }
            return( true);
            }
        }
    }
    public class WashingMachineDryClothesOnClotheslineEx:WashingMachine.DryClothesOnClothesline,IPreLoad,IAddInteraction{
        static InteractionDefinition sOldSingleton;
                                              public void AddInteraction(InteractionInjectorList interactions){
                                                                                                 interactions.ReplaceNoTest<WashingMachine,WashingMachine.DryClothesOnClothesline.Definition>(Singleton);
                                              }
                                   public void OnPreLoad(){
            Tunings.Inject<Sims3.Gameplay.Objects.Appliances.WashingMachineCheap    ,WashingMachine.DryClothesOnClothesline.Definition,Definition>(false);
            Tunings.Inject<Sims3.Gameplay.Objects.Appliances.WashingMachineExpensive,WashingMachine.DryClothesOnClothesline.Definition,Definition>(false);
            Tunings.Inject<Sims3.Gameplay.Objects.Appliances.WashingMachine,WashingMachine.DryClothesOnClothesline.Definition,Definition>(false);
                                     sOldSingleton=Singleton;
                                                   Singleton=new Definition();
                                   }
        public new class Definition:WashingMachine.DryClothesInDryer.Definition{
            public override InteractionInstance CreateInstance(ref InteractionInstanceParameters parameters){
                            InteractionInstance na=new WashingMachineDryClothesOnClotheslineEx();
                                                na.Init(ref parameters);
                                         return na;
            }
            public override bool Test(Sim a,WashingMachine target,bool isAutonomous,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
                if(target.mWashState!=WashingMachine.WashState.HasCleanLaundry)
            return(false);
                if(!WashingMachine.DryClothesBase.DoesClotheslineExist(target.LotCurrent)){
                    if(!target.LotCurrent.IsCommunityLot&&!WashingMachine.DryClothesBase.DoesDryerExist(target.LotCurrent))
                                                                                                                 greyedOutTooltipCallback=new GreyedOutTooltipCallback(WashingMachine.DryClothesOnClothesline.NoClotheslinesOnLotToolTip);
            return(false);
                }
                if(Clothesline.FindClosestAvailibleClothesline((GameObject)target,a)==null){
                                                                                                                 greyedOutTooltipCallback=new GreyedOutTooltipCallback(WashingMachine.DryClothesOnClothesline.NoAvailibleClotheslinesOnLot);
            return(false);
                }
            return( true);
            }
        }
    }
    public class ClotheslineDryClothingEx:Clothesline.DryClothing,IPreLoad,IAddInteraction{
        static InteractionDefinition sOldSingleton;
                                              public void AddInteraction(InteractionInjectorList interactions){
                                                                                                 interactions.ReplaceNoTest<Clothesline,Clothesline.DryClothing.Definition>(Singleton);
                                              }
                                   public void OnPreLoad(){
            Tunings.Inject<Sims3.Gameplay.Objects.Appliances.Clothesline,Clothesline.DryClothing.Definition,Definition>(false);
                                     sOldSingleton=Singleton;
                                                   Singleton=new Definition();
                                   }
        public new class Definition:Clothesline.DryClothing.Definition{
            public override InteractionInstance CreateInstance(ref InteractionInstanceParameters parameters){
                            InteractionInstance na=new ClotheslineDryClothingEx();
                                                na.Init(ref parameters);
                                         return na;
            }
            public override bool Test(Sim a,Clothesline target,bool isAutonomous,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
            return(target.CurClothesState==Dryer.DryerState.Empty);
            }
        }
    }
    public class DryerDryClothingEx:Dryer.DryClothing,IPreLoad,IAddInteraction{
        static InteractionDefinition sOldSingleton;
                                              public void AddInteraction(InteractionInjectorList interactions){
                                                                                                 interactions.ReplaceNoTest<Dryer,Dryer.DryClothing.Definition>(Singleton);
                                              }
                                   public void OnPreLoad(){
            Tunings.Inject<Sims3.Gameplay.Objects.Appliances.DryerCheap    ,Dryer.DryClothing.Definition,Definition>(false);
            Tunings.Inject<Sims3.Gameplay.Objects.Appliances.DryerExpensive,Dryer.DryClothing.Definition,Definition>(false);
            Tunings.Inject<Sims3.Gameplay.Objects.Appliances.Dryer,Dryer.DryClothing.Definition,Definition>(false);
                                     sOldSingleton=Singleton;
                                                   Singleton=new Definition();
                                   }
        public new class Definition:Dryer.DryClothing.Definition{
            public override InteractionInstance CreateInstance(ref InteractionInstanceParameters parameters){
                            InteractionInstance na=new DryerDryClothingEx();
                                                na.Init(ref parameters);
                                         return na;
            }
            public override bool Test(Sim a,Dryer target,bool isAutonomous,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
            return(target.CurDryerState==Dryer.DryerState.Empty);
            }
        }
    }
    public class ClotheslineGetCleanLaundryEx:Clothesline.GetCleanLaundry,IPreLoad,IAddInteraction{
        static InteractionDefinition sOldSingleton;
                                              public void AddInteraction(InteractionInjectorList interactions){
                                                                                                 interactions.ReplaceNoTest<Clothesline,Clothesline.GetCleanLaundry.Definition>(Singleton);
                                              }
                                   public void OnPreLoad(){
            Tunings.Inject<Sims3.Gameplay.Objects.Appliances.Clothesline,Clothesline.GetCleanLaundry.Definition,Definition>(false);
                                     sOldSingleton=Singleton;
                                                   Singleton=new Definition();
                                   }
        public new class Definition:Clothesline.GetCleanLaundry.Definition{
            public override InteractionInstance CreateInstance(ref InteractionInstanceParameters parameters){
                            InteractionInstance na=new ClotheslineGetCleanLaundryEx();
                                                na.Init(ref parameters);
                                         return na;
            }
            public override bool Test(Sim a,Clothesline target,bool isAutonomous,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
            return(target.CurClothesState==Dryer.DryerState.Done);
            }
        }
    }
    public class DryerGetCleanLaundryEx:Dryer.GetCleanLaundry,IPreLoad,IAddInteraction{
        static InteractionDefinition sOldSingleton;
                                              public void AddInteraction(InteractionInjectorList interactions){
                                                                                                 interactions.ReplaceNoTest<Dryer,Dryer.GetCleanLaundry.Definition>(Singleton);
                                              }
                                   public void OnPreLoad(){
            Tunings.Inject<Sims3.Gameplay.Objects.Appliances.DryerCheap    ,Dryer.GetCleanLaundry.Definition,Definition>(false);
            Tunings.Inject<Sims3.Gameplay.Objects.Appliances.DryerExpensive,Dryer.GetCleanLaundry.Definition,Definition>(false);
            Tunings.Inject<Sims3.Gameplay.Objects.Appliances.Dryer,Dryer.GetCleanLaundry.Definition,Definition>(false);
                                     sOldSingleton=Singleton;
                                                   Singleton=new Definition();
                                   }
        public new class Definition:Dryer.GetCleanLaundry.Definition{
            public override InteractionInstance CreateInstance(ref InteractionInstanceParameters parameters){
                            InteractionInstance na=new DryerGetCleanLaundryEx();
                                                na.Init(ref parameters);
                                         return na;
            }
            public override bool Test(Sim a,Dryer target,bool isAutonomous,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
                if(target.CurDryerState!=Dryer.DryerState.Done)
            return(false);
            return( true);
            }
        }
    }
}
