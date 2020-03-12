using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Objects.Environment;
using Sims3.Gameplay.Objects.Gardening;
using Sims3.Gameplay.Skills;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using System;
using System.Collections.Generic;
using System.Text;
using static ArthurGibraltarSims3Mod.Alive;
using static ArthurGibraltarSims3Mod.Interaction;
using static Sims3.Gameplay.Objects.Gardening.Plant;
namespace ArthurGibraltarSims3Mod{
    public class WaterPlantFix:WaterPlant,IPreLoad,IAddInteraction{
        static InteractionDefinition sOldSingleton;
                                              public void AddInteraction(InteractionInjectorList interactions){
                                                                                                 interactions.Replace<Plant,WaterPlant.Definition>(Singleton);
                                              }
                                   public void OnPreLoad(){
            Tunings.Inject<Plant,WaterPlant.Definition,Definition>(false);
                                     sOldSingleton=Singleton;
                                                   Singleton=new Definition();
                                   }
        public override bool Run(){
            bool previousInteractionSuccessful=false;
            bool flag=this.Actor.CurrentInteraction is Plant.ITendGarden;
            if(this.Target.RouteSimToMeAndCheckInUse(this.Actor)&&(flag?(Plant.WaterTest(this.Target,this.Actor)?1:0):(WaterTestDisregardGardeningSkill1(this.Target,this.Actor)?1:0))!=0){
                this.ConfigureInteraction();
                Plant.TryConfigureTendGardenInteraction(this.Actor.CurrentInteraction);
             if(!flag)
                    this.Actor.SkillManager.AddElement(SkillNames.Gardening);
                 previousInteractionSuccessful=this.DoWater();
            }
            if(this.IsChainingPermitted(previousInteractionSuccessful)){
                this.IgnorePlants.Add(this.Target);
              if(flag)
                    this.PushNextInteractionInChain(Plant.WaterPlant.Singleton,new Plant.GardeningInteractionTest(Plant.WaterTest),this.Target.LotCurrent);
              else
                    this.PushNextInteractionInChain(Plant.WaterPlant.Singleton,new Plant.GardeningInteractionTest(WaterTestDisregardGardeningSkill1),this.Target.LotCurrent);
            }
          return previousInteractionSuccessful;
        }
        public new class Definition:WaterPlant.Definition{
            public override InteractionInstance CreateInstance(ref InteractionInstanceParameters parameters){
                            InteractionInstance na=new WaterPlantFix();
                                                na.Init(ref parameters);
                                         return na;
            }
            public override bool Test(Sim a,Plant target,bool isAutonomous,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
                if(!WaterTestDisregardGardeningSkill1(target,a))
            return false;
                if(!target.mDormant)
            return true;
                                                                                                        greyedOutTooltipCallback=InteractionInstance.CreateTooltipCallback(Localization.LocalizeString("Gameplay/Objects/Gardening:DormantPlant"));
            return false;}
        }
        public static bool WaterTestDisregardGardeningSkill1(Plant plant,Sim actor){
        return WaterTestCommon1(plant,actor);
        }
        public static bool WaterTestCommon1(Plant plant,Sim actor){
       PlantFix fix=new PlantFix(plant);
            if(!fix.GardenInteractionLotValidityTest1(actor))
        return false;
            Soil soil=plant.GetSoil();
        return soil!=null&&
               soil.Dampness!=SoilDampness.Wet&&
                plant.Alive;
        }
        public class PlantFix{
              public PlantFix(Plant plant){
                         this.plant=plant;
              }
                        Plant plant;
            public bool GardenInteractionLotValidityTest1(Sim sim){
                if(sim==null)
            return false;
                if(sim.LotHome!=null&&plant.LotCurrent.CanSimTreatAsHome(sim))
            return true;
                if(sim.SimDescription!=null&&sim.SimDescription.IsBonehilda){
                   var coffin=BonehildaCoffin.FindBonehildaCoffin(sim);
                    if(coffin!=null&&coffin.LotCurrent==plant.LotCurrent)
            return true;
                }
            return false;
            }
        }
    }
}