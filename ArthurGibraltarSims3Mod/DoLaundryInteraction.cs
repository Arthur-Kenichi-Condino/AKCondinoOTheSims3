using Sims3.Gameplay;
using Sims3.Gameplay.Actors;
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
}
