using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Sims3.Gameplay;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.ActiveCareer.ActiveCareers;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.ActorSystems.Children;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Careers;
using Sims3.Gameplay.CAS;
using Sims3.Gameplay.CAS.Locale;
using Sims3.Gameplay.Controllers;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.EventSystem;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Interfaces;
using Sims3.Gameplay.Moving;
using Sims3.Gameplay.ObjectComponents;
using Sims3.Gameplay.Objects;
using Sims3.Gameplay.Objects.Appliances;
using Sims3.Gameplay.Objects.Beds;
using Sims3.Gameplay.Objects.CookingObjects;
using Sims3.Gameplay.Objects.Electronics;
using Sims3.Gameplay.Objects.Elevator;
using Sims3.Gameplay.Objects.Environment;
using Sims3.Gameplay.Objects.FoodObjects;
using Sims3.Gameplay.Objects.Gardening;
using Sims3.Gameplay.Objects.Lighting;
using Sims3.Gameplay.Objects.Miscellaneous;
using Sims3.Gameplay.Objects.Pets;
using Sims3.Gameplay.Objects.Plumbing;
using Sims3.Gameplay.Objects.Seating;
using Sims3.Gameplay.Objects.Vehicles;
using Sims3.Gameplay.Opportunities;
using Sims3.Gameplay.Passport;
using Sims3.Gameplay.Roles;
using Sims3.Gameplay.Services;
using Sims3.Gameplay.Situations;
using Sims3.Gameplay.Skills;
using Sims3.Gameplay.Socializing;
using Sims3.Gameplay.UI;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.SimIFace.CAS;
using Sims3.UI;
using static ArthurGibraltarSims3Mod.Interaction;
using static Sims3.Gameplay.Objects.Environment.BonehildaCoffin;
namespace ArthurGibraltarSims3Mod{
    public partial class Alive{
        const                  string                        _CLASS_NAME=".Alive.";
        public static readonly Random                        ModRandom=new Random();
        [Tunable]
        protected static bool kIExistNow=(false);
          static Alive(){
                try{
            LoadSaveManager.ObjectGroupsPreLoad+=OnPreLoad;
            World.sOnWorldLoadFinishedEventHandler+=OnWorldLoadFinished;
            World.sOnWorldQuitEventHandler        +=OnWorldQuit;
            LotManager.EnteringBuildBuyMode+=OnEnteringBuildBuyMode;
            LotManager. ExitingBuildBuyMode+= OnExitingBuildBuyMode;
            World.sOnObjectPlacedInLotEventHandler+=OnObjectPlacedInLot;
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source+"\n\n"+
                                 "Alive");
                }
          }
        private static void OnPreLoad(){
                try{
                        List<IPreLoad>helpers=DerivativeSearch.Find<IPreLoad>();
                     foreach(IPreLoad helper in helpers){
                        try{
                                      helper.OnPreLoad();
                        }catch(Exception exception){
                          Alive.WriteLog(exception.Message+"\n\n"+
                                         exception.StackTrace+"\n\n"+
                                         exception.Source+"\n\n"+
                                         "DerivativeSearch_foreach");
                        }
                     }
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source+"\n\n"+
                                 "DerivativeSearch");
                }
        }
        public interface IPreLoad{
                   void OnPreLoad();
        }
        private static void OnWorldLoadFinished(object sender,EventArgs e){
             //---------------------------------------------------------------
                    try{
                           RegAllInteractions();//  Perform prior to calling the derivatives
                    }catch(Exception exception){
                      Alive.WriteLog(exception.Message+"\n\n"+
                                     exception.StackTrace+"\n\n"+
                                     exception.Source+"\n\n"+
                                     "RegAllInteractions");
                    }
                try{
            Sims3.Gameplay.StoryProgression.EmigrateHousehold.kMultiplierForEmigratingKnownHousehold                  =0;
            Sims3.Gameplay.StoryProgression.EmigrateHousehold.kMultiplierForEmigratingProtectedHousehold              =0;
            Sims3.Gameplay.StoryProgression.EmigrateHousehold.kMultiplierForEmigratingAcademicCareerProfessorHousehold=0;
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source+"\n\n"+
                                 "disableEmigrateHousehold");
                }
             //---------------------------------------------------------------
                try{
            Route.AboutToPlanCallback+=OnAboutToPlan;
            Route.   PostPlanCallback+=   OnPostPlan;
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source+"\n\n"+
                                 "setRouteEventActions");
                }
             //---------------------------------------------------------------
                try{
                new DelayedEventListener(Sims3.Gameplay.EventSystem.EventTypeId.kBoughtObject,OnNewObject);
                new DelayedEventListener(Sims3.Gameplay.EventSystem.EventTypeId.kSimInstantiated,OnNewSim);
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source+"\n\n"+
                                 "setDelayedEventListeners");
                }
                try{
             new AlarmTask( 1,TimeUnit.Hours,DoHourlyTasks,1,TimeUnit.Hours);
             //---------------------------------------------------------------
             new AlarmTask(07,DaysOfTheWeek.All,FeedSims);//  Breakfast...
             new AlarmTask(13,DaysOfTheWeek.All,FeedSims);//  ...Lunch...
             new AlarmTask(19,DaysOfTheWeek.All,FeedSims);//  Dinner...
             //---------------------------------------------------------------
             new AlarmTask( 5,DaysOfTheWeek.All,AutoPause);
             //---------------------------------------------------------------
             new AlarmTask( 1,TimeUnit.Hours  ,CheckShowVenues       ,3,TimeUnit.Hours);
             //---------------------------------------------------------------
             new AlarmTask( 7,TimeUnit.Minutes,RecoverMissingSims    ,1,TimeUnit.Hours);
             //---------------------------------------------------------------
             new AlarmTask( 5,TimeUnit.Minutes,ResetPortalsAndRouting,1,TimeUnit.Hours);
             //---------------------------------------------------------------
             new AlarmTask( 1,TimeUnit.Seconds,StartUp);
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source+"\n\n"+
                                 "setAlarmTasks");
                }finally{
                }
        }
        private static void OnWorldQuit(object sender,EventArgs e){
                try{
            Route.AboutToPlanCallback-=OnAboutToPlan;
            Route.   PostPlanCallback-=   OnPostPlan;
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source+"\n\n"+
                                 "unsetRouteEventActions");
                }finally{
                }
                try{
    AlarmTask.DisposeAll();
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source+"\n\n"+
                                 "disposeAlarmTasks");
                }finally{
                }
        }
        static void StartUp(){
             //---------------------------------------------------------------
                try{
                    foreach(var tuning in InteractionTuning.sAllTunings.Values){
                     if(tuning.FullInteractionName==                                 "Sims3.Gameplay.Objects.Gardening.Plant+Graaiins+Definition"){
                        tuning.AddFlags(InteractionTuning.FlagField.DisallowAutonomous);
                    }if(tuning.FullInteractionName==                                    "Sims3.Gameplay.Core.SwipeSomethingAutonomous+Definition"){
                        tuning.AddFlags(InteractionTuning.FlagField.DisallowAutonomous);
                    }if(tuning.FullInteractionName==                       "Sims3.Gameplay.ActorSystems.TraitFunctions+SwipeSomething+Definition"){
                        tuning.AddFlags(InteractionTuning.FlagField.DisallowAutonomous);
                //-----------------------------------------
                        }if(tuning.FullInteractionName==                "Sims3.Gameplay.Objects.Miscellaneous.ClothingPileDry+CleanUp+Definition"){
                            tuning.RemoveFlags(InteractionTuning.FlagField.DisallowAutonomous);
                  for(int i=tuning.mTradeoff.mOutputs.Count-1;i>=0;i--)
                         if(tuning.mTradeoff.mOutputs[i].Commodity==CommodityKind.BeBonehilda)
                            tuning.mTradeoff.mOutputs.RemoveAt(i);
                            tuning.mTradeoff.mOutputs.Add(new CommodityChange(CommodityKind.BeBonehilda,300,true,300,OutputUpdateType.ContinuousFlow,false,false,UpdateAboveAndBelowZeroType.Either));
                            tuning.Availability.OccultRestrictionType=OccultRestrictionType.Ignore;
                            tuning.Availability.AddFlags(Availability.FlagField.AllowGreetedSims);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowEvenIfNotAllowedInRoomAutonomous);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideAutonomous);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideUserDirected);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowOnCommunityLots);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowOnAllLots);
                            tuning.RemoveFlags(InteractionTuning.FlagField.ConsiderCodeVersion);
                        }if(tuning.FullInteractionName==      "Sims3.Gameplay.Objects.Miscellaneous.ClothingPileWet+DryClothesInDryer+Definition"){
                            tuning.RemoveFlags(InteractionTuning.FlagField.DisallowAutonomous);
                  for(int i=tuning.mTradeoff.mOutputs.Count-1;i>=0;i--)
                         if(tuning.mTradeoff.mOutputs[i].Commodity==CommodityKind.BeBonehilda)
                            tuning.mTradeoff.mOutputs.RemoveAt(i);
                            tuning.mTradeoff.mOutputs.Add(new CommodityChange(CommodityKind.BeBonehilda,300,true,300,OutputUpdateType.ContinuousFlow,false,false,UpdateAboveAndBelowZeroType.Either));
                            tuning.Availability.OccultRestrictionType=OccultRestrictionType.Ignore;
                            tuning.Availability.AddFlags(Availability.FlagField.AllowGreetedSims);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowEvenIfNotAllowedInRoomAutonomous);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideAutonomous);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideUserDirected);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowOnCommunityLots);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowOnAllLots);
                            tuning.RemoveFlags(InteractionTuning.FlagField.ConsiderCodeVersion);
                        }if(tuning.FullInteractionName=="Sims3.Gameplay.Objects.Miscellaneous.ClothingPileWet+DryClothesOnClothesline+Definition"){
                            tuning.RemoveFlags(InteractionTuning.FlagField.DisallowAutonomous);
                  for(int i=tuning.mTradeoff.mOutputs.Count-1;i>=0;i--)
                         if(tuning.mTradeoff.mOutputs[i].Commodity==CommodityKind.BeBonehilda)
                            tuning.mTradeoff.mOutputs.RemoveAt(i);
                            tuning.mTradeoff.mOutputs.Add(new CommodityChange(CommodityKind.BeBonehilda,300,true,300,OutputUpdateType.ContinuousFlow,false,false,UpdateAboveAndBelowZeroType.Either));
                            tuning.Availability.OccultRestrictionType=OccultRestrictionType.Ignore;
                            tuning.Availability.AddFlags(Availability.FlagField.AllowGreetedSims);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowEvenIfNotAllowedInRoomAutonomous);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideAutonomous);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideUserDirected);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowOnCommunityLots);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowOnAllLots);
                            tuning.RemoveFlags(InteractionTuning.FlagField.ConsiderCodeVersion);
                        }if(tuning.FullInteractionName==                "Sims3.Gameplay.Objects.Miscellaneous.ClothingPileDry+PutDown+Definition"){
                            tuning.RemoveFlags(InteractionTuning.FlagField.DisallowAutonomous);
                            tuning.Availability.OccultRestrictionType=OccultRestrictionType.Ignore;
                            tuning.Availability.AddFlags(Availability.FlagField.AllowGreetedSims);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowEvenIfNotAllowedInRoomAutonomous);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideAutonomous);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideUserDirected);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowOnCommunityLots);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowOnAllLots);
                            tuning.RemoveFlags(InteractionTuning.FlagField.ConsiderCodeVersion);
                        }if(tuning.FullInteractionName==                "Sims3.Gameplay.Objects.Miscellaneous.ClothingPileWet+PutDown+Definition"){
                            tuning.RemoveFlags(InteractionTuning.FlagField.DisallowAutonomous);
                            tuning.Availability.OccultRestrictionType=OccultRestrictionType.Ignore;
                            tuning.Availability.AddFlags(Availability.FlagField.AllowGreetedSims);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowEvenIfNotAllowedInRoomAutonomous);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideAutonomous);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideUserDirected);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowOnCommunityLots);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowOnAllLots);
                            tuning.RemoveFlags(InteractionTuning.FlagField.ConsiderCodeVersion);
                        }if(tuning.FullInteractionName==                       "Sims3.Gameplay.Objects.Miscellaneous.Hamper+DoLaundry+Definition"){
                            tuning.RemoveFlags(InteractionTuning.FlagField.DisallowAutonomous);
                  for(int i=tuning.mTradeoff.mOutputs.Count-1;i>=0;i--)
                         if(tuning.mTradeoff.mOutputs[i].Commodity==CommodityKind.BeBonehilda)
                            tuning.mTradeoff.mOutputs.RemoveAt(i);
                            tuning.mTradeoff.mOutputs.Add(new CommodityChange(CommodityKind.BeBonehilda,500,true,500,OutputUpdateType.ContinuousFlow,false,false,UpdateAboveAndBelowZeroType.Either));
                            tuning.Availability.OccultRestrictionType=OccultRestrictionType.Ignore;
                            tuning.Availability.AddFlags(Availability.FlagField.AllowGreetedSims);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowEvenIfNotAllowedInRoomAutonomous);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideAutonomous);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideUserDirected);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowOnCommunityLots);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowOnAllLots);
                            tuning.RemoveFlags(InteractionTuning.FlagField.ConsiderCodeVersion);
                        }if(tuning.FullInteractionName==                          "Sims3.Gameplay.Objects.Miscellaneous.Hamper+PickUp+Definition"){
                            tuning.RemoveFlags(InteractionTuning.FlagField.DisallowAutonomous);
                            tuning.Availability.OccultRestrictionType=OccultRestrictionType.Ignore;
                            tuning.Availability.AddFlags(Availability.FlagField.AllowGreetedSims);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowEvenIfNotAllowedInRoomAutonomous);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideAutonomous);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideUserDirected);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowOnCommunityLots);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowOnAllLots);
                            tuning.RemoveFlags(InteractionTuning.FlagField.ConsiderCodeVersion);
                        }if(tuning.FullInteractionName==                     "Sims3.Gameplay.Objects.Miscellaneous.Hamper+DropClothes+Definition"){
                            tuning.RemoveFlags(InteractionTuning.FlagField.DisallowAutonomous);
                            tuning.Availability.OccultRestrictionType=OccultRestrictionType.Ignore;
                            tuning.Availability.AddFlags(Availability.FlagField.AllowGreetedSims);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowEvenIfNotAllowedInRoomAutonomous);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideAutonomous);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideUserDirected);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowOnCommunityLots);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowOnAllLots);
                            tuning.RemoveFlags(InteractionTuning.FlagField.ConsiderCodeVersion);
                        }if(tuning.FullInteractionName==                  "Sims3.Gameplay.Objects.Appliances.WashingMachine+DoLaundry+Definition"){
                            tuning.RemoveFlags(InteractionTuning.FlagField.DisallowAutonomous);
                  for(int i=tuning.mTradeoff.mOutputs.Count-1;i>=0;i--)
                         if(tuning.mTradeoff.mOutputs[i].Commodity==CommodityKind.BeBonehilda)
                            tuning.mTradeoff.mOutputs.RemoveAt(i);
                            tuning.mTradeoff.mOutputs.Add(new CommodityChange(CommodityKind.BeBonehilda,600,true,600,OutputUpdateType.ContinuousFlow,false,false,UpdateAboveAndBelowZeroType.Either));
                            tuning.Availability.OccultRestrictionType=OccultRestrictionType.Ignore;
                            tuning.Availability.AddFlags(Availability.FlagField.AllowGreetedSims);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowEvenIfNotAllowedInRoomAutonomous);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideAutonomous);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideUserDirected);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowOnCommunityLots);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowOnAllLots);
                            tuning.RemoveFlags(InteractionTuning.FlagField.ConsiderCodeVersion);
                        }if(tuning.FullInteractionName==                   "Sims3.Gameplay.Objects.Appliances.Clothesline+DryClothing+Definition"){
                            tuning.RemoveFlags(InteractionTuning.FlagField.DisallowAutonomous);
                  for(int i=tuning.mTradeoff.mOutputs.Count-1;i>=0;i--)
                         if(tuning.mTradeoff.mOutputs[i].Commodity==CommodityKind.BeBonehilda)
                            tuning.mTradeoff.mOutputs.RemoveAt(i);
                            tuning.mTradeoff.mOutputs.Add(new CommodityChange(CommodityKind.BeBonehilda,700,true,700,OutputUpdateType.ContinuousFlow,false,false,UpdateAboveAndBelowZeroType.Either));
                            tuning.Availability.OccultRestrictionType=OccultRestrictionType.Ignore;
                            tuning.Availability.AddFlags(Availability.FlagField.AllowGreetedSims);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowEvenIfNotAllowedInRoomAutonomous);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideAutonomous);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideUserDirected);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowOnCommunityLots);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowOnAllLots);
                            tuning.RemoveFlags(InteractionTuning.FlagField.ConsiderCodeVersion);
                        }if(tuning.FullInteractionName==    "Sims3.Gameplay.Objects.Appliances.WashingMachine+DryClothesOnClothesline+Definition"){
                            tuning.RemoveFlags(InteractionTuning.FlagField.DisallowAutonomous);
                  for(int i=tuning.mTradeoff.mOutputs.Count-1;i>=0;i--)
                         if(tuning.mTradeoff.mOutputs[i].Commodity==CommodityKind.BeBonehilda)
                            tuning.mTradeoff.mOutputs.RemoveAt(i);
                            tuning.mTradeoff.mOutputs.Add(new CommodityChange(CommodityKind.BeBonehilda,700,true,700,OutputUpdateType.ContinuousFlow,false,false,UpdateAboveAndBelowZeroType.Either));
                            tuning.Availability.OccultRestrictionType=OccultRestrictionType.Ignore;
                            tuning.Availability.AddFlags(Availability.FlagField.AllowGreetedSims);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowEvenIfNotAllowedInRoomAutonomous);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideAutonomous);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideUserDirected);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowOnCommunityLots);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowOnAllLots);
                            tuning.RemoveFlags(InteractionTuning.FlagField.ConsiderCodeVersion);
                        }if(tuning.FullInteractionName==               "Sims3.Gameplay.Objects.Appliances.Clothesline+GetCleanLaundry+Definition"){
                            tuning.RemoveFlags(InteractionTuning.FlagField.DisallowAutonomous);
                  for(int i=tuning.mTradeoff.mOutputs.Count-1;i>=0;i--)
                         if(tuning.mTradeoff.mOutputs[i].Commodity==CommodityKind.BeBonehilda)
                            tuning.mTradeoff.mOutputs.RemoveAt(i);
                            tuning.mTradeoff.mOutputs.Add(new CommodityChange(CommodityKind.BeBonehilda,800,true,800,OutputUpdateType.ContinuousFlow,false,false,UpdateAboveAndBelowZeroType.Either));
                            tuning.Availability.OccultRestrictionType=OccultRestrictionType.Ignore;
                            tuning.Availability.AddFlags(Availability.FlagField.AllowGreetedSims);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowEvenIfNotAllowedInRoomAutonomous);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideAutonomous);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideUserDirected);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowOnCommunityLots);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowOnAllLots);
                            tuning.RemoveFlags(InteractionTuning.FlagField.ConsiderCodeVersion);
                        }if(tuning.FullInteractionName==                         "Sims3.Gameplay.Objects.Appliances.Dryer+DryClothing+Definition"){
                            tuning.RemoveFlags(InteractionTuning.FlagField.DisallowAutonomous);
                  for(int i=tuning.mTradeoff.mOutputs.Count-1;i>=0;i--)
                         if(tuning.mTradeoff.mOutputs[i].Commodity==CommodityKind.BeBonehilda)
                            tuning.mTradeoff.mOutputs.RemoveAt(i);
                            tuning.mTradeoff.mOutputs.Add(new CommodityChange(CommodityKind.BeBonehilda,800,true,800,OutputUpdateType.ContinuousFlow,false,false,UpdateAboveAndBelowZeroType.Either));
                            tuning.Availability.OccultRestrictionType=OccultRestrictionType.Ignore;
                            tuning.Availability.AddFlags(Availability.FlagField.AllowGreetedSims);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowEvenIfNotAllowedInRoomAutonomous);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideAutonomous);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideUserDirected);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowOnCommunityLots);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowOnAllLots);
                            tuning.RemoveFlags(InteractionTuning.FlagField.ConsiderCodeVersion);
                        }if(tuning.FullInteractionName==          "Sims3.Gameplay.Objects.Appliances.WashingMachine+DryClothesInDryer+Definition"){
                            tuning.RemoveFlags(InteractionTuning.FlagField.DisallowAutonomous);
                  for(int i=tuning.mTradeoff.mOutputs.Count-1;i>=0;i--)
                         if(tuning.mTradeoff.mOutputs[i].Commodity==CommodityKind.BeBonehilda)
                            tuning.mTradeoff.mOutputs.RemoveAt(i);
                            tuning.mTradeoff.mOutputs.Add(new CommodityChange(CommodityKind.BeBonehilda,800,true,800,OutputUpdateType.ContinuousFlow,false,false,UpdateAboveAndBelowZeroType.Either));
                            tuning.Availability.OccultRestrictionType=OccultRestrictionType.Ignore;
                            tuning.Availability.AddFlags(Availability.FlagField.AllowGreetedSims);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowEvenIfNotAllowedInRoomAutonomous);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideAutonomous);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideUserDirected);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowOnCommunityLots);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowOnAllLots);
                            tuning.RemoveFlags(InteractionTuning.FlagField.ConsiderCodeVersion);
                        }if(tuning.FullInteractionName==                     "Sims3.Gameplay.Objects.Appliances.Dryer+GetCleanLaundry+Definition"){
                            tuning.RemoveFlags(InteractionTuning.FlagField.DisallowAutonomous);
                  for(int i=tuning.mTradeoff.mOutputs.Count-1;i>=0;i--)
                         if(tuning.mTradeoff.mOutputs[i].Commodity==CommodityKind.BeBonehilda)
                            tuning.mTradeoff.mOutputs.RemoveAt(i);
                            tuning.mTradeoff.mOutputs.Add(new CommodityChange(CommodityKind.BeBonehilda,900,true,900,OutputUpdateType.ContinuousFlow,false,false,UpdateAboveAndBelowZeroType.Either));
                            tuning.Availability.OccultRestrictionType=OccultRestrictionType.Ignore;
                            tuning.Availability.AddFlags(Availability.FlagField.AllowGreetedSims);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowEvenIfNotAllowedInRoomAutonomous);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideAutonomous);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideUserDirected);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowOnCommunityLots);
                            tuning.Availability.AddFlags(Availability.FlagField.AllowOnAllLots);
                            tuning.RemoveFlags(InteractionTuning.FlagField.ConsiderCodeVersion);
                                }if(tuning.FullInteractionName==                       "Sims3.Gameplay.Objects.Counters.Bar+MakeDrink+Definition"){
                                    tuning.RemoveFlags(InteractionTuning.FlagField.DisallowAutonomous);
                          for(int i=tuning.mTradeoff.mOutputs.Count-1;i>=0;i--)
                                 if(tuning.mTradeoff.mOutputs[i].Commodity==CommodityKind.BeBonehilda)
                                    tuning.mTradeoff.mOutputs.RemoveAt(i);
                                    tuning.mTradeoff.mOutputs.Add(new CommodityChange(CommodityKind.BeBonehilda,50,true,50,OutputUpdateType.ContinuousFlow,false,false,UpdateAboveAndBelowZeroType.Either));
                                    tuning.Availability.OccultRestrictionType=OccultRestrictionType.Ignore;
                                    tuning.Availability.AddFlags(Availability.FlagField.AllowGreetedSims);
                                    tuning.Availability.AddFlags(Availability.FlagField.AllowEvenIfNotAllowedInRoomAutonomous);
                                    tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideAutonomous);
                                    tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideUserDirected);
                                    tuning.Availability.AddFlags(Availability.FlagField.AllowOnCommunityLots);
                                    tuning.Availability.AddFlags(Availability.FlagField.AllowOnAllLots);
                                    tuning.RemoveFlags(InteractionTuning.FlagField.ConsiderCodeVersion);
                                }if(tuning.FullInteractionName==                "Sims3.Gameplay.Objects.Decorations.Mirror+AdmireSelf+Definition"){
                                    tuning.RemoveFlags(InteractionTuning.FlagField.DisallowAutonomous);
                          for(int i=tuning.mTradeoff.mOutputs.Count-1;i>=0;i--)
                                 if(tuning.mTradeoff.mOutputs[i].Commodity==CommodityKind.BeBonehilda)
                                    tuning.mTradeoff.mOutputs.RemoveAt(i);
                                    tuning.mTradeoff.mOutputs.Add(new CommodityChange(CommodityKind.BeBonehilda,50,true,50,OutputUpdateType.ContinuousFlow,false,false,UpdateAboveAndBelowZeroType.Either));
                                    tuning.Availability.OccultRestrictionType=OccultRestrictionType.Ignore;
                                    tuning.Availability.AddFlags(Availability.FlagField.AllowGreetedSims);
                                    tuning.Availability.AddFlags(Availability.FlagField.AllowEvenIfNotAllowedInRoomAutonomous);
                                    tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideAutonomous);
                                    tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideUserDirected);
                                    tuning.Availability.AddFlags(Availability.FlagField.AllowOnCommunityLots);
                                    tuning.Availability.AddFlags(Availability.FlagField.AllowOnAllLots);
                                    tuning.RemoveFlags(InteractionTuning.FlagField.ConsiderCodeVersion);
                            }if(tuning.FullInteractionName==                           "Sims3.Gameplay.Objects.Plumbing.Toilet+Repair+Definition"){
                                tuning.RemoveFlags(InteractionTuning.FlagField.DisallowAutonomous);
                      for(int i=tuning.mTradeoff.mOutputs.Count-1;i>=0;i--)
                             if(tuning.mTradeoff.mOutputs[i].Commodity==CommodityKind.BeBonehilda)
                                tuning.mTradeoff.mOutputs.RemoveAt(i);
                                tuning.mTradeoff.mOutputs.Add(new CommodityChange(CommodityKind.BeBonehilda,500,true,500,OutputUpdateType.ContinuousFlow,false,false,UpdateAboveAndBelowZeroType.Either));
                                tuning.Availability.OccultRestrictionType=OccultRestrictionType.Ignore;
                                tuning.Availability.AddFlags(Availability.FlagField.AllowGreetedSims);
                                tuning.Availability.AddFlags(Availability.FlagField.AllowEvenIfNotAllowedInRoomAutonomous);
                                tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideAutonomous);
                                tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideUserDirected);
                                tuning.Availability.AddFlags(Availability.FlagField.AllowOnCommunityLots);
                                tuning.Availability.AddFlags(Availability.FlagField.AllowOnAllLots);
                                tuning.RemoveFlags(InteractionTuning.FlagField.ConsiderCodeVersion);
                            }if(tuning.FullInteractionName== "Sims3.Gameplay.Objects.Plumbing.AllInOneBathroom+RepairAllInOneBathroom+Definition"){
                                tuning.RemoveFlags(InteractionTuning.FlagField.DisallowAutonomous);
                      for(int i=tuning.mTradeoff.mOutputs.Count-1;i>=0;i--)
                             if(tuning.mTradeoff.mOutputs[i].Commodity==CommodityKind.BeBonehilda)
                                tuning.mTradeoff.mOutputs.RemoveAt(i);
                                tuning.mTradeoff.mOutputs.Add(new CommodityChange(CommodityKind.BeBonehilda,500,true,500,OutputUpdateType.ContinuousFlow,false,false,UpdateAboveAndBelowZeroType.Either));
                                tuning.Availability.OccultRestrictionType=OccultRestrictionType.Ignore;
                                tuning.Availability.AddFlags(Availability.FlagField.AllowGreetedSims);
                                tuning.Availability.AddFlags(Availability.FlagField.AllowEvenIfNotAllowedInRoomAutonomous);
                                tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideAutonomous);
                                tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideUserDirected);
                                tuning.Availability.AddFlags(Availability.FlagField.AllowOnCommunityLots);
                                tuning.Availability.AddFlags(Availability.FlagField.AllowOnAllLots);
                                tuning.RemoveFlags(InteractionTuning.FlagField.ConsiderCodeVersion);
                                }if(tuning.FullInteractionName==                         "Sims3.Gameplay.Objects.Beds.MurphyBed+Close+Definition"){
                                    tuning.AddFlags(InteractionTuning.FlagField.DisallowAutonomous);
                                    tuning.Availability.OccultRestrictionType=OccultRestrictionType.Ignore;
                                    tuning.Availability.AddFlags(Availability.FlagField.AllowGreetedSims);
                                    tuning.Availability.AddFlags(Availability.FlagField.AllowEvenIfNotAllowedInRoomAutonomous);
                                    tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideAutonomous);
                                    tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideUserDirected);
                                    tuning.Availability.AddFlags(Availability.FlagField.AllowOnCommunityLots);
                                    tuning.Availability.AddFlags(Availability.FlagField.AllowOnAllLots);
                                    tuning.RemoveFlags(InteractionTuning.FlagField.ConsiderCodeVersion);    
                                    }
                    }
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source+"\n\n"+
                                 "tunningFixes");
                }finally{
                }
                try{
                                                                 var cookingAppliances=Sims3.Gameplay.Queries.GetObjects<ICookingAppliance>();
                                                                  if(cookingAppliances!=null)
                       foreach(ICookingAppliance cookingAppliance in cookingAppliances){
                    try{
                                             var cookingApplianceGameObject=(cookingAppliance as GameObject);
                                              if(cookingApplianceGameObject!=null){
                                      var parent=cookingApplianceGameObject.Parent;
                                bool slotted=(false);
                                     Slot parentSlot=Slot.None;
                                       if(parent!=null){
                                var slots=parent.GetContainmentSlots();
                foreach(var slot in slots){
                                       if(parent.GetContainedObject(slot)==cookingApplianceGameObject){
                                          parentSlot=slot;
                                     slotted=( true);
                                            break;
                                       }
                }
                                       }
                                                 cookingApplianceGameObject.SimLine?.DoReset();
                                                 cookingApplianceGameObject.SetObjectToReset();
                                                 cookingApplianceGameObject.ResetParentingHierarchy(true);
                                                 cookingApplianceGameObject.RemoveFromWorld();
                                                 cookingApplianceGameObject.     AddToWorld();
                                                 cookingApplianceGameObject.SetHiddenFlags(HiddenFlags.Nothing);
                                                 cookingApplianceGameObject.SetOpacity(1f,0.0f);
                                                 cookingApplianceGameObject.AddToLot();
                     LotManager.PlaceObjectOnLot(cookingApplianceGameObject,cookingApplianceGameObject.ObjectId);
                                       if(parent!=null&&
                                     slotted){
                                                 cookingApplianceGameObject.ParentToSlot(parent,parentSlot);
                                       }
                                              if(cookingApplianceGameObject.LotCurrent==Household.ActiveHouseholdLot){
                                              }
                                              }
                    }catch(Exception exception){
                      Alive.WriteLog(exception.Message+"\n\n"+
                                     exception.StackTrace+"\n\n"+
                                     exception.Source+"\n\n"+
                                     "cookingApplianceGameObjectFixes_foreach_Start");
                    }
                       }
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source+"\n\n"+
                                 "cookingApplianceGameObjectFixes_Start");
                }
                try{
                        foreach(var miniFridge in Sims3.Gameplay.Queries.GetObjects<MiniFridge>()){
                         var parent=miniFridge.Parent;
                                bool slotted=(false);
                        Slot parentSlot=Slot.None;
                          if(parent!=null){
                   var slots=parent.GetContainmentSlots();
   foreach(var slot in slots){
                          if(parent.GetContainedObject(slot)==miniFridge){
                             parentSlot=slot;
                                     slotted=( true);
                            break;
                          }
   }
                          }
                                    miniFridge.SimLine?.DoReset();
                                    miniFridge.Line=new SimQueue(SimQueue.WaitLocation.HangAroundNearObject,2);
                                    miniFridge.SetObjectToReset();
                                    miniFridge.ResetParentingHierarchy(true);
                                    miniFridge.RemoveFromWorld();
                                    miniFridge.     AddToWorld();
                                    miniFridge.SetHiddenFlags(HiddenFlags.Nothing);
                                    miniFridge.SetOpacity(1f,0.0f);
                                    miniFridge.AddToLot();
                          if(parent!=null&&
                                     slotted){
                                    miniFridge.ParentToSlot(parent,parentSlot);
                          }
                        }
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source+"\n\n"+
                                 "miniFridgeFixes_Start");
                }
                try{
                        foreach(var washingMachine in Sims3.Gameplay.Queries.GetObjects<WashingMachine>()){
                                    washingMachine.SetObjectToReset();
                        }
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source+"\n\n"+
                                 "washingMachineFixes_Start");
                }
                try{
                        foreach(var fridge in Sims3.Gameplay.Queries.GetObjects<Fridge>()){
                                    fridge.SetObjectToReset();
                        }
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source+"\n\n"+
                                 "fridgeFixes_Start");
                }
        }
        //------------------------------------------------------------------------------------------------------------------
        private static void OnEnteringBuildBuyMode(){
        }
        private static void  OnExitingBuildBuyMode(){
        }
        //------------------------------------------------------------------------------------------------------------------
             private static void OnNewSim(Sims3.Gameplay.EventSystem.Event e){
                try{
                                InteractionInjectorList.MasterList.Perform(e.TargetObject as Sim);
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source+"\n\n"+
                                 "InteractionInjectorList_MasterList_Perform");
                }finally{
                }
                try{
                                                                          (e.TargetObject as Sim).PlayRouteFailFrequency=Sim.RouteFailFrequency.NeverPlayRouteFail;
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source+"\n\n"+
                                 "changePlayRouteFailFrequency");
                }finally{
                }
             }
        protected static void OnNewObject(Sims3.Gameplay.EventSystem.Event e){
                try{
                                InteractionInjectorList.MasterList.Perform(e.TargetObject as GameObject);
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source+"\n\n"+
                                 "InteractionInjectorList_MasterList_Perform");
                }finally{
                }
        }
        protected static void OnObjectPlacedInLot(object sender,EventArgs e){
               GameObject obj=null;
                try{
                World.OnObjectPlacedInLotEventArgs args=e as World.OnObjectPlacedInLotEventArgs;
                                                if(args!=null){
                          obj=GameObject.GetObject(args.ObjectId);
                       if(obj!=null){
                                InteractionInjectorList.MasterList.Perform(obj);
                       }
                                                }
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source+"\n\n"+
                                 "InteractionInjectorList_MasterList_Perform");
                }finally{
                }
        }
        //==================================================================================================================
        public static void RegAllInteractions(){
                try{
                           RegAllInteractions(0,false);
                                          Lot activeLot=LotManager.ActiveLot;
                                           if(activeLot!=null){
                           RegAllInteractions(activeLot.LotId,false);
                                           }
                                          if((Household.ActiveHousehold!=null)&&
                                             (Household.ActiveHousehold.LotHome!=null)&&
                                             (Household.ActiveHousehold.LotHome!=activeLot)){
                           RegAllInteractions(Household.ActiveHousehold.LotHome.LotId,false);
                                          }
           ModTask.Perform(RegAllInteractionsDelayed);
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source+"\n\n"+
                                 "RegAllInteractions_Set");
                }finally{
                }
        }
        protected static void 
                           RegAllInteractionsDelayed(){
                try{
                           RegAllInteractions(0,true);
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source+"\n\n"+
                                 "RegAllInteractions_Set_Delayed");
                }finally{
                }
        }
        protected static void 
                           RegAllInteractions(ulong lotId,bool full){
                try{
                                                 InteractionInjectorList list=InteractionInjectorList.MasterList;
                                                                      if(list.IsEmpty)return;
bool localFull=full&(lotId==0);
            foreach(KeyValuePair<Type,List<IInteractionInjector>>type in list.Types){
                    try{
                    Array results=null;
                  if(lotId==0){
           if(!full){
                if(!InteractionInjectorList.IsAlwaysType(type.Key))continue;
           }else{
                Sleep();
           }
                          results=Sims3.SimIFace.Queries.GetObjects(type.Key);
                  }else{
           if(!full){
                if(InteractionInjectorList.IsAlwaysType(type.Key))continue;//  These would have been handled by the (lotId==0) load
           }
                          results=Sims3.SimIFace.Queries.GetObjects(type.Key,lotId);
                  }
                       if(results==null)continue;
int count=0;
foreach(GameObject obj in results){
                    //using(TestSpan span=new TestSpan()){
                        try{
      list.Perform(obj);
                        }catch(Exception exception){
                          Alive.WriteLog(exception.Message+"\n\n"+
                                         exception.StackTrace+"\n\n"+
                                         exception.Source+"\n\n"+
                                         "RegAllInteractions_foreach_Perform");
                        }finally{
                        }
                    //}
  if(localFull){
    count++;
 if(count>250){
                Sleep();
    count=0;
 }
  }
}
                    }catch(Exception exception){
                      Alive.WriteLog(exception.Message+"\n\n"+
                                     exception.StackTrace+"\n\n"+
                                     exception.Source+"\n\n"+
                                     "RegAllInteractions_foreach");
                    }finally{
                    }
            }
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source+"\n\n"+
                                 "RegAllInteractions");
                }finally{
                }
        }
        //==================================================================================================================
        static void DoHourlyTasks(){
                try{
            foreach(var coffin in Sims3.Gameplay.Queries.GetObjects<BonehildaCoffin>()){
                    try{
          Sim bonehilda;
          if((bonehilda=coffin.BonehildaSim)!=null){
           if(bonehilda.LotCurrent!=coffin.LotCurrent&&
             (bonehilda.LotCurrent!=null||bonehilda.InteractionQueue==null||bonehilda.InteractionQueue.Count==0)){
                                                                                                                    StuckSimData stuckSim;
                                                         if(!StuckSims.TryGetValue(bonehilda.SimDescription.SimDescriptionId,out stuckSim)){
                                                                                                                                 stuckSim=new StuckSimData();
                                                             StuckSims.Add(        bonehilda.SimDescription.SimDescriptionId,    stuckSim);
                                                         }
                                                                                                                       if(!stuckSim.Resetting){
                                                                                                                           stuckSim.Detections++;
                                                                                                        Vector3 destination=Vector3.Invalid;
                                                               if(bonehilda.RoutingComponent!=null){
                                                                  bonehilda.RoutingComponent.GetDestination(out destination);
                                                               }
                    if(  stuckSim.resetTask==null)
                         stuckSim.resetTask=new ResetStuckSimTask(bonehilda,destination,"Bonehilda");
                    else stuckSim.resetTask.Renew();
                                                                                                                       }
           }
          }
                    }catch(Exception exception){
                      Alive.WriteLog(exception.Message+"\n\n"+
                                     exception.StackTrace+"\n\n"+
                                     exception.Source+"\n\n"+
                                     "checkBonehildaLocation_foreach");
                    }finally{
                    }
            }
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source+"\n\n"+
                                 "checkBonehildaLocation");
                }finally{
                }
                try{
                foreach(var lot in Sims3.Gameplay.Queries.GetObjects<Lot>()){
                 BonehildaCoffin[]coffins;
                                  coffins=Sims3.Gameplay.Queries.GetObjects<BonehildaCoffin>(lot);
                Sim[]tasks=new Sim[3];int count=0;
            foreach(var coffin in coffins){
          Sim bonehilda;
          if((bonehilda=coffin.BonehildaSim)!=null){
                     if(coffin.LotCurrent!=null&&
                        coffin.LotCurrent==bonehilda.LotCurrent&&
                                           bonehilda.InteractionQueue!=null){
                            var currentInteraction=bonehilda.InteractionQueue.GetCurrentInteraction();
                             if(currentInteraction==null||
                              !(currentInteraction is BonehildaReturnToCoffin||
                                           bonehilda.InteractionQueue.HasInteractionOfType(typeof(BonehildaReturnToCoffin)))){
                     tasks[count++]=bonehilda;
                        if(count>=2)break;
                             }
                     }
          }
            }
                  if(tasks[0]!=null){
                  if(tasks[1]==null)
                     tasks[1]=tasks[0];
                  if(tasks[2]==null)
                     tasks[2]=tasks[1];
                  }
            foreach(var coffin in coffins){
          Sim bonehilda;
          if((bonehilda=coffin.BonehildaSim)!=null){
                     if(coffin.LotCurrent!=null&&
                        coffin.LotCurrent==bonehilda.LotCurrent&&
                                           bonehilda.InteractionQueue!=null){
                            var currentInteraction=bonehilda.InteractionQueue.GetCurrentInteraction();
                             if(currentInteraction==null||
                              !(currentInteraction is BonehildaReturnToCoffin||
                                           bonehilda.InteractionQueue.HasInteractionOfType(typeof(BonehildaReturnToCoffin)))){
           if(bonehilda==tasks[0]&&bonehilda.InteractionQueue.Count<=8){
                    try{
                            foreach (var toilet in coffin.LotCurrent.GetObjects<Toilet>()){
                                     if(toilet.Repairable!=null&&toilet.Repairable.Broken){
                                                                      var repair=Toilet.Repair.Singleton.CreateInstance(toilet,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(repair);
                                     }
                            }
                            foreach(var shower in coffin.LotCurrent.GetObjects<Shower>()){
                                     if(shower.Repairable!=null&&shower.Repairable.Broken){
                                                                      var repair=Shower.RepairShower.Singleton.CreateInstance(shower,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(repair);
                                     }
                            }
                            foreach(var hotTub in coffin.LotCurrent.GetObjects<HotTubBase>()){
                                     if(hotTub.Repairable!=null&&hotTub.Repairable.Broken){
                                                                      var repair=HotTubBase.RepairHotTub.Singleton.CreateInstance(hotTub,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(repair);
                                     }
                            }
                            foreach(var allInOneBathroom in coffin.LotCurrent.GetObjects<AllInOneBathroom>()){
                                     if(allInOneBathroom.Repairable!=null&&allInOneBathroom.Repairable.Broken){
                                                                      var repair=AllInOneBathroom.RepairAllInOneBathroom.Singleton.CreateInstanceWithCallbacks(allInOneBathroom,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true,RepairAllInOneBathroomOnStarted,RepairAllInOneBathroomOnCompleted,RepairAllInOneBathroomOnFailed);
 //Alive.WriteLog("Can Bonehilda AllInOneBathroom.RepairAllInOneBathroom? "+repair.Test());
                                           bonehilda.InteractionQueue.Add(repair);
                                     }
                            }
                            foreach(var TV in coffin.LotCurrent.GetObjects<TV>()){
                                     if(TV.Repairable!=null&&TV.Repairable.Broken){
                                                                      var repair=TV.RepairTV.Singleton.CreateInstance(TV,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(repair);
                                     }
                            }
                            foreach(var computer in coffin.LotCurrent.GetObjects<Computer>()){
                                     if(computer.Repairable!=null&&computer.Repairable.Broken){
                                                                      var repair=Computer.RepairComputer.Singleton.CreateInstance(computer,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(repair);
                                     }
                            }
                            foreach(var partOfCookingProcess in coffin.LotCurrent.GetObjects<IPartOfCookingProcess>()){
                                     if(partOfCookingProcess is   BakingDish){
                                                                      var cleanUp=  BakingDish_CleanUp.Singleton.CreateInstance(partOfCookingProcess,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(cleanUp);
                                     }
                                     if(partOfCookingProcess is CuttingBoard){
                                                                      var cleanUp=Cuttingboard_CleanUp.Singleton.CreateInstance(partOfCookingProcess,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(cleanUp);
                                     }
                                     if(partOfCookingProcess is     FoodTray){
                                                                      var cleanUp=    FoodTray_CleanUp.Singleton.CreateInstance(partOfCookingProcess,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(cleanUp);
                                     }
                                     if(partOfCookingProcess is    FryingPan){
                                                                      var cleanUp=   FryingPan_CleanUp.Singleton.CreateInstance(partOfCookingProcess,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(cleanUp);
                                     }
                                     if(partOfCookingProcess is          Pot){
                                                                      var cleanUp=         Pot_CleanUp.Singleton.CreateInstance(partOfCookingProcess,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(cleanUp);
                                     }
                                     if(partOfCookingProcess is ServingContainer){
                                                                      var cleanUp=ServingContainer.ServingContainer_CleanUp.Singleton.CreateInstance(partOfCookingProcess,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(cleanUp);
                                     }
                                     if(partOfCookingProcess is   Grillable){
                                                                      var cleanUp=     Grillable_Grill.Singleton.CreateInstance(partOfCookingProcess,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(cleanUp);
                                     }
                            }
                            foreach(var barTray in coffin.LotCurrent.GetObjects<BarTray>()){
                                                                      var cleanUp=BarTray.CleanUp.Singleton.CreateInstance(barTray,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(cleanUp);
                            }
                    }catch(Exception exception){
                      Alive.WriteLog(exception.Message+"\n\n"+
                                     exception.StackTrace+"\n\n"+
                                     exception.Source+"\n\n"+
                                     "bonehildaTasks_0");
                    }finally{
                    }
           }
           if(bonehilda==tasks[1]&&bonehilda.InteractionQueue.Count<=8){
                    try{
                            foreach(var sim in coffin.LotCurrent.GetObjects<Sim>()){
                                     if(sim.SimDescription==null||
                                        sim.Motives==null){
                                        continue;
                                     }
                                     if(sim.Motives.IsHungry()){
                                     if(sim.SimDescription.Baby){
                                       if(!bonehilda.InteractionQueue.HasInteractionOfTypeAndTarget(GiveBottle.Singleton,sim)){
                                                                      var pickUp=PickUpChild.Singleton.CreateInstance(sim,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(pickUp);
                                                                      var changeDiaper=ChangeDiaper.Singleton.CreateInstanceWithCallbacks(sim,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true,ChangeDiaperOnStarted,ChangeDiaperOnCompleted,ChangeDiaperOnFailed);
                                           bonehilda.InteractionQueue.Add(changeDiaper);
                                                                      var feed=GiveBottle.Singleton.CreateInstanceWithCallbacks(sim,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true,GiveBottleOnStarted,GiveBottleOnCompleted,GiveBottleOnFailed);
                                           bonehilda.InteractionQueue.Add(feed);
                                                                      var snuggle=Snuggle.Singleton.CreateInstance(sim,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(snuggle);
                                                                      var playWith=PlayWith.Singleton.CreateInstance(sim,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(playWith);
                                       }
                                     }
                                     if(sim.SimDescription.Toddler){
                                            if(sim.Posture!=null&&
                                               sim.Posture is HighChair.InChairPosture){
                                       if(!bonehilda.InteractionQueue.HasInteractionOfTypeAndTarget(HighChairBase.GiveBottle  .Singleton,(GameObject)sim.Posture.Container)&&
                                          !bonehilda.InteractionQueue.HasInteractionOfTypeAndTarget(HighChairBase.GiveBabyFood.Singleton,(GameObject)sim.Posture.Container)){
                                                            if(RandomUtil.CoinFlip()){
                                                                      var giveBottle  =HighChairBase.GiveBottle  .Singleton.CreateInstance(sim.Posture.Container,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(giveBottle);
                                                            }else{
                                                                      var giveBabyFood=HighChairBase.GiveBabyFood.Singleton.CreateInstance(sim.Posture.Container,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(giveBabyFood);
                                                            }
                                       }
                                            }else{
                                       if(!bonehilda.InteractionQueue.HasInteractionOfTypeAndTarget(FeedOnFloor           .Singleton,sim)&&
                                          !bonehilda.InteractionQueue.HasInteractionOfTypeAndTarget(FeedToddlerInHighChair.Singleton,sim)){
                                                                      var pickUp=PickUpChild.Singleton.CreateInstance(sim,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(pickUp);
                                                                      var changeDiaper=ChangeDiaper.Singleton.CreateInstanceWithCallbacks(sim,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true,ChangeDiaperOnStarted,ChangeDiaperOnCompleted,ChangeDiaperOnFailed);
                                           bonehilda.InteractionQueue.Add(changeDiaper);
                                                                      var snuggle=Snuggle.Singleton.CreateInstance(sim,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(snuggle);
                                                                      var tossInAir=TossInAir.Singleton.CreateInstance(sim,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(tossInAir);
                                                            if(RandomUtil.CoinFlip()){
                                                                      var feedOnFloor=FeedOnFloor.Singleton.CreateInstance(sim,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(feedOnFloor);
                                                            }else{
                                                                      var feedToddlerInHighChair=FeedToddlerInHighChair.Singleton.CreateInstanceWithCallbacks(sim,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true,FeedToddlerInHighChairOnStarted,FeedToddlerInHighChairOnCompleted,FeedToddlerInHighChairOnFailed);
                                           bonehilda.InteractionQueue.Add(feedToddlerInHighChair);
                                                            }
                                       }
                                            }
                                     }
                                     }else 
                                     if(sim.Motives.IsSleepy()){
                                     if(sim.SimDescription.Toddler){
                                    if(!sim.IsSleeping){
                                     if(sim.Posture!=null&&
                                      !(sim.Posture is Crib.InCribPosture)){
                                       if(!bonehilda.InteractionQueue.HasInteractionOfTypeAndTarget(PutChildInCrib.Singleton,sim)){
                                                                      var pickUp=PickUpChild.Singleton.CreateInstance(sim,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(pickUp);
                                                                      var changeDiaper=ChangeDiaper.Singleton.CreateInstanceWithCallbacks(sim,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true,ChangeDiaperOnStarted,ChangeDiaperOnCompleted,ChangeDiaperOnFailed);
                                           bonehilda.InteractionQueue.Add(changeDiaper);
                                                                      var putInCrib=PutChildInCrib.Singleton.CreateInstance(sim,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(putInCrib);
                                       }
                                     }
                                    }
                                     }
                                     }else 
                                     if(sim.Motives.HasMotive(CommodityKind.Hygiene)&&sim.Motives.GetValue(CommodityKind.Hygiene)<=0){
                                    if(!sim.IsSleeping){
                                       if(!bonehilda.InteractionQueue.HasInteractionOfTypeAndTarget(ChangeDiaper.Singleton,sim)){
                                     if(sim.SimDescription.Baby){
                                                                      var pickUp=PickUpChild.Singleton.CreateInstance(sim,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(pickUp);
                                                                      var changeDiaper=ChangeDiaper.Singleton.CreateInstanceWithCallbacks(sim,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true,ChangeDiaperOnStarted,ChangeDiaperOnCompleted,ChangeDiaperOnFailed);
                                           bonehilda.InteractionQueue.Add(changeDiaper);
                                                                      var feed=GiveBottle.Singleton.CreateInstanceWithCallbacks(sim,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true,GiveBottleOnStarted,GiveBottleOnCompleted,GiveBottleOnFailed);
                                           bonehilda.InteractionQueue.Add(feed);
                                                                      var snuggle=Snuggle.Singleton.CreateInstance(sim,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(snuggle);
                                                                      var playWith=PlayWith.Singleton.CreateInstance(sim,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(playWith);
                                     }
                                     if(sim.SimDescription.Toddler){
                                                                      var pickUp=PickUpChild.Singleton.CreateInstance(sim,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(pickUp);
                                                                      var changeDiaper=ChangeDiaper.Singleton.CreateInstanceWithCallbacks(sim,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true,ChangeDiaperOnStarted,ChangeDiaperOnCompleted,ChangeDiaperOnFailed);
                                           bonehilda.InteractionQueue.Add(changeDiaper);
                                                                      var putDownChild=PutDownChild.Singleton.CreateInstance(sim,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(putDownChild);
                                     }
                                     }
                                    }
                                     }else 
                                     if(sim.Motives.HasMotive(CommodityKind.Social)&&sim.Motives.GetValue(CommodityKind.Social)<=0){
                                    if(!sim.IsSleeping){
                                       if(!bonehilda.InteractionQueue.HasInteractionOfTypeAndTarget(PickUpChild.Singleton,sim)){
                                     if(sim.SimDescription.Baby){
                                                                      var pickUp=PickUpChild.Singleton.CreateInstance(sim,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(pickUp);
                                                                      var snuggle=Snuggle.Singleton.CreateInstance(sim,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(snuggle);
                                                                      var playWith=PlayWith.Singleton.CreateInstance(sim,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(playWith);
                                     }
                                     if(sim.SimDescription.Toddler){
                                                                      var pickUp=PickUpChild.Singleton.CreateInstance(sim,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(pickUp);
                                                                      var snuggle=Snuggle.Singleton.CreateInstance(sim,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(snuggle);
                                                                      var tossInAir=TossInAir.Singleton.CreateInstance(sim,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(tossInAir);
                                                                      var putDownChild=PutDownChild.Singleton.CreateInstance(sim,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(putDownChild);
                                     }
                                       }
                                    }
                                     }else{
                                     if(sim.SimDescription.Toddler){
                                    if(!sim.IsSleeping){
                                     if(sim.Posture!=null&&
                                       !sim.Posture.Satisfies(CommodityKind.Standing      ,(IGameObject)null)&&
                                       !sim.Posture.Satisfies(CommodityKind.BeingCarried  ,(IGameObject)null)&&
                                       !sim.Posture.Satisfies(CommodityKind.WalkingToddler,(IGameObject)null)){
                                       if(!bonehilda.InteractionQueue.HasInteractionOfTypeAndTarget(LetChildOut.Singleton,sim)){
                                                                      var letChildOut=LetChildOut.Singleton.CreateInstance(sim,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(letChildOut);
                                       }
                                     }
                                    }
                                     }
                                     }
                                     if(sim.Motives.IsSleepy()){
                                     if(sim.SimDescription.ChildOrAbove){
                                         
                                     }
                                     }
                            }
                    }catch(Exception exception){
                      Alive.WriteLog(exception.Message+"\n\n"+
                                     exception.StackTrace+"\n\n"+
                                     exception.Source+"\n\n"+
                                     "bonehildaTasks_1");
                    }finally{
                    }
           }
           if(bonehilda==tasks[2]&&bonehilda.InteractionQueue.Count<=8){
                    try{
                            foreach(var petBowl in coffin.LotCurrent.GetObjects<PetBowl>()){
                                    if(!petBowl.IsFull){
                                       if(!bonehilda.InteractionQueue.HasInteractionOfTypeAndTarget(PetBowl.FillBowl.FillSingleton,petBowl)){
                                                                      var feed=PetBowl.FillBowl.FillSingleton.CreateInstance(petBowl,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(feed);
                                       }
                                    }
                            }
                            foreach(var litterBox in coffin.LotCurrent.GetObjects<LitterBox>()){
                                     if(litterBox.Cleanable.NeedsToBeCleaned){
                                       if(!bonehilda.InteractionQueue.HasInteractionOfTypeAndTarget(LitterBox.CleanLitterBox.Singleton,litterBox)){
                                                                      var clean=LitterBox.CleanLitterBox.Singleton.CreateInstance(litterBox,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(clean);
                                       }
                                     }
                            }
                            foreach(var waterTrough in coffin.LotCurrent.GetObjects<WaterTrough>()){
                        try{
                                     if(waterTrough.GetCurrentState()!=WaterTrough.TroughState.Full){
                                       if(!bonehilda.InteractionQueue.HasInteractionOfTypeAndTarget(WaterTrough.FillWater.Singleton,waterTrough)){
                                                                      var feed=WaterTrough.FillWater.Singleton.CreateInstance(waterTrough,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(feed);
                                       }
                                     }
                        }catch(Exception exception){
                          Alive.WriteLog(exception.Message+"\n\n"+
                                         exception.StackTrace+"\n\n"+
                                         exception.Source+"\n\n"+
                                         "bonehildaTasks_2_pets_WaterTrough");
                        }finally{
                        }
                            }
                            foreach(var boxStall in coffin.LotCurrent.GetObjects<BoxStall>()){
                                     if(boxStall.IsDirty){
                                       if(!bonehilda.InteractionQueue.HasInteractionOfTypeAndTarget(BoxStall.Clean.Singleton,boxStall)){
                                                                      var clean=BoxStall.Clean.Singleton.CreateInstance(boxStall,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(clean);
                                       }
                                     }
                            }
                            foreach(var sim in coffin.LotCurrent.GetObjects<Sim>()){
                                     if(sim.SimDescription==null||
                                        sim.Motives==null){
                                        continue;
                                     }
                                     if(sim.Motives.IsHungry()){
                                     if(sim.SimDescription.IsFoal){
                                       if(!bonehilda.InteractionQueue.HasInteractionOfTypeAndTarget(BottleFeedFoalFix.SimActor_Singleton,sim)){
                                                                      var feed=BottleFeedFoalFix.SimActor_Singleton.CreateInstance(sim,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(feed);
                                       }
                                     }else
                                     if(sim.SimDescription.IsCat||
                                        sim.SimDescription.IsADogSpecies||
                                        sim.SimDescription.IsHorse){
                                       if(!bonehilda.InteractionQueue.HasInteractionOfTypeAndTarget(FeedTreatFix.Singleton,sim)){
                                                                      var feed=new FeedTreatFix.FeedTreatDefinition(true).CreateInstance(sim,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(feed);
                                       }
                                     }
                                     }
                                    if((sim.Motives.HasMotive(CommodityKind.Social)&&sim.Motives.GetValue(CommodityKind.Social)<=0)){
                                     if(sim.SimDescription.IsFoal){
                                       if(!bonehilda.InteractionQueue.HasInteractionOfTypeAndTarget(BottleFeedFoalFix.SimActor_Singleton,sim)){
                                                                      var feed=BottleFeedFoalFix.SimActor_Singleton.CreateInstance(sim,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(feed);
                                       }
                                       if(!bonehilda.InteractionQueue.HasInteractionOfTypeAndTarget(BrushPetFix.BrushHorseSingleton,sim)){
                                                                      var brush=BrushPetFix.BrushHorseSingleton.CreateInstance(sim,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(brush);
                                       }
                                     }else
                                     if(sim.SimDescription.IsCat||
                                        sim.SimDescription.IsADogSpecies||
                                        sim.SimDescription.IsHorse){
                                       if(!bonehilda.InteractionQueue.HasInteractionOfTypeAndTarget(FeedTreatFix.Singleton,sim)){
                                                                      var feed=new FeedTreatFix.FeedTreatDefinition(true).CreateInstance(sim,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(feed);
                                       }
                                     if(sim.SimDescription.IsCat){
                                       if(!bonehilda.InteractionQueue.HasInteractionOfTypeAndTarget(BrushPetFix.BrushCatSingleton,sim)){
                                                                      var brush=BrushPetFix.BrushCatSingleton.CreateInstance(sim,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(brush);
                                       }
                                     }
                                     if(sim.SimDescription.IsADogSpecies){
                                       if(!bonehilda.InteractionQueue.HasInteractionOfTypeAndTarget(BrushPetFix.BrushDogSingleton,sim)){
                                                                      var brush=BrushPetFix.BrushDogSingleton.CreateInstance(sim,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(brush);
                                       }
                                     }
                                     if(sim.SimDescription.IsHorse){
                                       if(!bonehilda.InteractionQueue.HasInteractionOfTypeAndTarget(BrushPetFix.BrushHorseSingleton,sim)){
                                                                      var brush=BrushPetFix.BrushHorseSingleton.CreateInstance(sim,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(brush);
                                       }
                                     }
                                     }
                                    }
                            }
                    }catch(Exception exception){
                      Alive.WriteLog(exception.Message+"\n\n"+
                                     exception.StackTrace+"\n\n"+
                                     exception.Source+"\n\n"+
                                     "bonehildaTasks_2_pets");
                    }finally{
                    }
                    try{
                              Soil soil;
                            foreach(var plant in coffin.LotCurrent.GetObjects<Plant>()){
                               if((soil=plant.GetSoil())!=null&&
                                   soil.Dampness!=SoilDampness.Wet&&
                                        plant.Alive){
                                                                      var water=Plant.WaterPlant.Singleton.CreateInstance(plant,bonehilda,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                           bonehilda.InteractionQueue.Add(water);
                               }
                            }
                    }catch(Exception exception){
                      Alive.WriteLog(exception.Message+"\n\n"+
                                     exception.StackTrace+"\n\n"+
                                     exception.Source+"\n\n"+
                                     "bonehildaTasks_2_plants");
                    }finally{
                    }
           }
                             }
                     }
          }
            }
                }
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source+"\n\n"+
                                 "bonehildaTasks");
                }finally{
                }
        }
        private static void FeedToddlerInHighChairOnStarted(Sim s,float x){
        }
        private static void FeedToddlerInHighChairOnCompleted(Sim s,float x){
                try{
                                                  if(s.LotCurrent!=null)
                            foreach(var highChair in s.LotCurrent.GetObjects<HighChair>()){
                                     if(highChair.Toddler!=null&&
                                        highChair.Toddler.Motives!=null&&
                                        highChair.Toddler.Motives.IsHungry()){
                                                 if(!s.InteractionQueue.HasInteractionOfTypeAndTarget(HighChairBase.GiveBottle  .Singleton,highChair)&&
                                                    !s.InteractionQueue.HasInteractionOfTypeAndTarget(HighChairBase.GiveBabyFood.Singleton,highChair)){
                                                                    if(RandomUtil.CoinFlip()){
                                                                              var giveBottle  =HighChairBase.GiveBottle  .Singleton.CreateInstance(highChair,s,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                                     s.InteractionQueue.Add(giveBottle);
                                                                    }else{
                                                                              var giveBabyFood=HighChairBase.GiveBabyFood.Singleton.CreateInstance(highChair,s,new InteractionPriority(InteractionPriorityLevel.UserDirected),false,true);
                                                     s.InteractionQueue.Add(giveBabyFood);
                                                                    }
                                                 }
                                     }
                            }
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source+"\n\n"+
                                 "FeedToddlerInHighChairOnCompletedTryAgainFix");
                }finally{
                }
        }
        private static void FeedToddlerInHighChairOnFailed(Sim s,float x){
            Alive.WriteLog("Failed to FeedToddlerInHighChairOnFailed:"+s.ExitReason);
        }
        private static void GiveBottleOnStarted(Sim s,float x){
        }
        private static void GiveBottleOnCompleted(Sim s,float x){
        }
        private static void GiveBottleOnFailed(Sim s,float x){
        }
        private static void ChangeDiaperOnStarted(Sim s,float x){
        }
        private static void ChangeDiaperOnCompleted(Sim s,float x){
        }
        private static void ChangeDiaperOnFailed(Sim s,float x){
        }
        private static void RepairAllInOneBathroomOnStarted(Sim s,float x){
        }
        private static void RepairAllInOneBathroomOnCompleted(Sim s,float x){
        }
        private static void RepairAllInOneBathroomOnFailed(Sim s,float x){
        }
        static void FeedSims(){
                try{
            foreach(var sim in Sims3.Gameplay.Queries.GetObjects<Sim>()){
                    try{
                      if(sim.SimDescription!=null&&
                         sim.InteractionQueue!=null&&sim.InteractionQueue.mInteractionList!=null&&
                         sim.Motives!=null){
                       if(sim.SimDescription.TeenOrAbove){
                                            if((sim.Household==null||
                                              (!sim.Household.InWorld&&
                                               !sim.Household.IsSpecialHousehold))&&
                 (!Passport.IsHostedPassportSim(sim)&&
                                                sim.SimDescription.AssignedRole==null)){
                                continue;
                                            }
if(LunarCycleManager.sFullMoonZombies!=null&&  
   LunarCycleManager.sFullMoonZombies.Contains(sim.SimDescription)){
                                continue;
}
                                            if(sim.SimDescription.IsBonehilda||
                                              (sim.Service!=null&&
                                              (sim.Service.ServiceType==ServiceType.GrimReaper||
                                               sim.Service.ServiceType==ServiceType.Burglar   ||
                                               sim.Service.ServiceType==ServiceType.Repoman   ||
                                               sim.Service.ServiceType==ServiceType.SocialWorkerChildProtection))){
                                continue;
                                            }
                                        Lot lot=LotManager.ActiveLot;
                                         if(lot==null){
                                            lot=Household.ActiveHousehold?.LotHome;
                                         }
                                         if(lot!=null){
                     if(!sim.IsGreetedOnLot(lot)&&
                                           !lot.IsCommunityLot)continue;
                            Fridge[]fridges;
                                if((fridges=lot.GetObjects<Fridge>())!=null&&
                                    fridges.Length>0){
                        if(sim.SimDescription.IsVampire&&
                           sim.Motives.IsVampireThirsty()){
var interaction=Fridge_Have.Singleton.CreateInstanceWithCallbacks(GlobalFunctions.GetClosestObject<Fridge>(fridges,sim),sim,new InteractionPriority(InteractionPriorityLevel.UserDirected),true,true,OnFridge_HaveStarted,OnFridge_HaveCompleted,OnFridge_HaveFailed) as Fridge_Have;
 if(interaction!=null){
    interaction.    Quantity=(Recipe.MealQuantity.Single);
    interaction.ChosenRecipe=(Recipe.NameToRecipeHash["VampireJuiceEP7"]);
                         sim.InteractionQueue.AddInteraction(interaction,false);
 }
                        }else
                        if(sim.Motives.IsHungry()){
var interaction=Fridge_Have.Singleton.CreateInstanceWithCallbacks(GlobalFunctions.GetClosestObject<Fridge>(fridges,sim),sim,new InteractionPriority(InteractionPriorityLevel.UserDirected),true,true,OnFridge_HaveStarted,OnFridge_HaveCompleted,OnFridge_HaveFailed) as Fridge_Have;
 if(interaction!=null){
           Cooking cooking;
                        if(sim.SkillManager!=null&&
                  (cooking=sim.SkillManager.GetElement(Sims3.Gameplay.Skills.SkillNames.Cooking) as Cooking)!=null&&
                   cooking.KnownRecipes!=null&&
                   cooking.KnownRecipes.Count>0){
    interaction.ChosenRecipe=(Recipe.NameToRecipeHash[cooking.KnownRecipes[ModRandom.Next(0,cooking.KnownRecipes.Count)]]);
                        }else{
    interaction.    Quantity=(Recipe.MealQuantity.Single);
    interaction.ChosenRecipe=(Recipe.NameToRecipeHash["MicrowaveMeal"]);
                        }
 if(interaction.ChosenRecipe==Recipe.NameToRecipeHash["Ambrosia"]){
    interaction.    Quantity=(Recipe.MealQuantity.Single);
    interaction.ChosenRecipe=(Recipe.NameToRecipeHash["MicrowaveMeal"]);
 }
 if(interaction.ChosenRecipe.IsPetFood){
    interaction.    Quantity=(Recipe.MealQuantity.Single);
    interaction.ChosenRecipe=(Recipe.NameToRecipeHash["MicrowaveMeal"]);
 }
                         sim.InteractionQueue.AddInteraction(interaction,false);
 }
                        }
                                }
                                         }
                       }
                      }
                    }catch(Exception exception){
                      Alive.WriteLog(exception.Message+"\n\n"+
                                     exception.StackTrace+"\n\n"+
                                     exception.Source+"\n\n"+
                                     "cookSomething_foreach");
                    }finally{
                    }
            }
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source+"\n\n"+
                                 "cookSomething_foreach");
                }finally{
                }
        }
        private static void OnFridge_HaveFailed(Sim s,float x){
        }
        private static void OnFridge_HaveStarted(Sim s,float x){
        }
        private static void OnFridge_HaveCompleted(Sim s,float x){
        }
        private static void OnDoLaundryFailed(Sim s,float x){
        }
        private static void OnDoLaundryStarted(Sim s,float x){
        }
        private static void OnDoLaundryCompleted(Sim s,float x){
        }
        //==================================================================================================================
        protected static void OnAboutToPlan(Route r,string routeType,Vector3 point){
                try{
 Sims3.SimIFace.Route.SetAvoidanceFieldRangeScale(r.Follower.ObjectId,0.5f);
 Sims3.SimIFace.Route.SetAvoidanceFieldSmoothing( r.Follower.ObjectId,1.0f);
                                                  r.CanPlayReactionsAtEndOfRoute=(false);
                                          var sim=r.Follower.Target as Sim;
                                           if(sim!=null){
                                              sim.PlayRouteFailFrequency=Sim.RouteFailFrequency.NeverPlayRouteFail;
                                           }
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source+"\n\n"+
                                 "fixAvoidanceField");
                }finally{
                }
        }
        protected static void    OnPostPlan(Route r,string routeType,string result){
                try{
                                          var sim=r.Follower.Target as Sim;
                                          if((sim!=null)&&
                                             (sim.SimDescription!=null)){
                                                                                                              StuckSimData stuckSim;
                                                         if(!StuckSims.TryGetValue(sim.SimDescription.SimDescriptionId,out stuckSim)){
                                                                                                                           stuckSim=new StuckSimData();
                                                             StuckSims.Add(        sim.SimDescription.SimDescriptionId,    stuckSim);
                                                         }
                                                                                                                        if(stuckSim.Resetting)return;
                                                                                                                               bool detected=(false);
                                              if(!r.PlanResult.Succeeded()){
                                                                                                                                    detected=( true);
                                                                                                                           stuckSim.Detections++;
                                              }else{
                                                                                                                           stuckSim.Detections=0;
                                              }
                                                                                                                        if(stuckSim.PositionPreceding!=sim.Position){
                                                                                                                           stuckSim.PositionPrecedingTicks=SimClock.CurrentTicks;
                                                                                                                           stuckSim.PositionPreceding=(sim.Position);
                                                                                                                        }
                                                                                                  if(SimClock.CurrentTicks-stuckSim.PositionPrecedingTicks>SimClock.kSimulatorTicksPerSimMinute*10){
                                                                                                                                if(!detected){
                                                                                                                           stuckSim.Detections++;
                                                                                                                                }
                                                                                                  }
                                                                                                                        if(stuckSim.Detections>1){
                                                                                                                           stuckSim.Resetting=( true);
                  //Alive.WriteLog("detected_a_stuck_sim:reset:"+sim.Name);
                    if(  stuckSim.resetTask==null)
                         stuckSim.resetTask=new ResetStuckSimTask(sim,r.GetDestPoint(),"Unroutable");
                    else stuckSim.resetTask.Renew();
                                                                                                                        }
                                          }
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source+"\n\n"+
                                 "stuckSimCheck");
                }finally{
                }
        }
        //==================================================================================================================
static Dictionary<Sim,Vector3>stuckPositions=new Dictionary<Sim,Vector3>();
        static void AutoPause(){
                try{
      Sims3.Gameplay.Gameflow.SetGameSpeed(Sims3.Gameplay.Gameflow.GameSpeed.Pause,Sims3.Gameplay.Gameflow.SetGameSpeedContext.GameStates);
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source+"\n\n"+
                                 "pauseGame");
                }
                try{
                    for(int i=Situation.sAllSituations.Count-1;i>=0;i--){
          Situation situation=Situation.sAllSituations[i];
                    try{
                 if(situation is GrimReaperSituation        ){continue;}
                 //
                 if(situation is ParentsLeavingTownSituation){continue;}
                 //
               if(!(situation is MovingSituation)){
         bool ignore=false;
 foreach(var sim in situation.mSimsWithInteractions){
          if(sim.Key.IsSelectable&&
            (sim.Key.IsSleeping||
            (sim.Key.InteractionQueue?.GetCurrentInteraction() is WorkInRabbitHole)||
            (sim.Key.InteractionQueue?.GetCurrentInteraction() is BedSleep))){
              ignore=true;
                    break;
          }else 
          if(sim.Key.SimDescription!=null&&
             sim.Key.SimDescription.IsBonehilda){
                    var coffin=BonehildaCoffin.FindBonehildaCoffin(sim.Key);
                     if(coffin!=null&&
                        coffin.LotCurrent==sim.Key.LotCurrent){
              ignore=true;
                    break;
                     }
          }else 
          if(sim.Key.IsSelectable&&
             sim.Key.Posture?.Container!=null&&
             sim.Key.Posture?.Container!=sim.Key&&
             sim.Key.SimDescription!=null&&
             sim.Key.SimDescription.ToddlerOrBelow){
              ignore=true;
                    break;
          }else 
          if(sim.Key.IsSelectable&&
             sim.Key.InteractionQueue!=null&&
             sim.Key.InteractionQueue.HasInteractionOfType(typeof(Sleep))&&
             sim.Key.SimDescription.ToddlerOrBelow){
              ignore=true;
                    break;
          }
 }
           if(ignore){continue;}
               }
                        try{
                    situation.Exit();
                        }catch(Exception exception){
                          Alive.WriteLog(exception.Message+"\n\n"+
                                         exception.StackTrace+"\n\n"+
                                         exception.Source+"\n\n"+
                                         "deleteSituations_foreach_exit");
                        }
foreach(Sim sim in LotManager.Actors){
     sim.RemoveRole(situation);
}
                          if((Situation.sAllSituations.Count>i)&&
                             (Situation.sAllSituations[i]==situation)){
                              Situation.sAllSituations.RemoveAt(i);
                          }
                    }catch(Exception exception){
                      Alive.WriteLog(exception.Message+"\n\n"+
                                     exception.StackTrace+"\n\n"+
                                     exception.Source+"\n\n"+
                                     "deleteSituations_foreach");
                    }
                    }
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source+"\n\n"+
                                 "deleteSituations");
                }
                try{
List<Sim>
   toRemove=new List<Sim>();
    foreach(var simPosData in stuckPositions){
             if(simPosData.Key==null||
                simPosData.Key.HasBeenDestroyed){
   toRemove.Add(simPosData.Key);
             }else 
             if(simPosData.Key.Posture?.Container!=null&&
                simPosData.Key.Posture?.Container!=simPosData.Key&&
                simPosData.Key.SimDescription!=null&&
                simPosData.Key.SimDescription.ToddlerOrBelow){
   toRemove.Add(simPosData.Key);
             }
    }
                                 for(int i=0;i<toRemove.Count;i++){
                              stuckPositions.Remove(toRemove[i]);
                                 }
   toRemove.Clear();
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source+"\n\n"+
                                 "stuckCheck_notMovingSims_simPosData");
                }
                try{
                    foreach(Sim sim in Sims3.Gameplay.Queries.GetObjects<Sim>()){
                    try{
      if(!stuckPositions.TryGetValue(sim,out Vector3 position)){
          stuckPositions.Add(        sim,
                                     sim.Position);
      }else{
          stuckPositions[sim]=(sim.Position);
                                     new ResetClearSimTask(sim);
goto _DidReset;
      }
                          InteractionInstance 
                                currentInteraction=sim.InteractionQueue?.GetCurrentInteraction();
            if(!Objects.IsValid(sim.ObjectId)||
             Simulator.GetProxy(sim.ObjectId)==null||
                                sim.SimDescription==null||
                                sim.SimDescription.CreatedSim!=sim){
                                     new ResetClearSimTask(sim);
goto _DidReset;
            }else
                             if(currentInteraction!=null&&
                               (currentInteraction is BedSleep||
                                sim.InteractionQueue.HasInteractionOfType(typeof(Sleep)))){
                             if(currentInteraction.Cancelled||
                                sim.Motives==null){
goto _SoftReset;
                             }else 
                            if(!sim.Motives.IsSleepy()||
                               !sim.IsSleeping){
                      for(int i=sim.InteractionQueue.mInteractionList.Count-1;i>=0;i--){
                           if(!(sim.InteractionQueue.mInteractionList[i] is BedSleep||
                                sim.InteractionQueue.mInteractionList[i] is Sleep))continue;
                                sim.InteractionQueue.CancelInteraction(sim.InteractionQueue.mInteractionList[i].Id,ExitReason.CanceledByScript);
                      }
                            }
goto _Skipped;
                             }else
                             if(currentInteraction!=null&&
                                currentInteraction is WorkInRabbitHole){
goto _Skipped;
                             }else
                             if(sim.Service!=null&&
                               (sim.Service.ServiceType==ServiceType.GrimReaper||
                                sim.Service.ServiceType==ServiceType.Burglar   ||
                                sim.Service.ServiceType==ServiceType.Repoman   ||
                                sim.Service.ServiceType==ServiceType.Police    ||
                                sim.Service.ServiceType==ServiceType.Firefighter||
                                sim.Service.ServiceType==ServiceType.SocialWorkerAdoption||
                                sim.Service.ServiceType==ServiceType.SocialWorkerChildProtection||
                                sim.Service.ServiceType==ServiceType.SocialWorkerPetAdoption||
                                sim.Service.ServiceType==ServiceType.SocialWorkerPetPutUp)){
goto _Skipped;
                             }else 
                             if(sim.SimDescription.IsGhost||
(LunarCycleManager.sFullMoonZombies!=null&&  
 LunarCycleManager.sFullMoonZombies.Contains(sim.SimDescription))){
goto _SoftReset;
                             }else
                             if(sim.Household==null||
                               !sim.Household.InWorld||
                                sim.Household.IsSpecialHousehold){
                                     new ResetClearSimTask(sim);
goto _DidReset;
                             }else
                             if(sim.LotHome==null&&
                                sim.VirtualLotHome==null){
                                     new ResetClearSimTask(sim);
goto _DidReset; 
                             }else
                            if(Household.ActiveHouseholdLot!=null&&
                                sim.LotCurrent==Household.ActiveHouseholdLot&&
                               !sim.IsGreetedOnLot(Household.ActiveHouseholdLot)&&
                              !Household.RoommateManager.IsNPCRoommate(sim)){
                                     new ResetClearSimTask(sim);
goto _DidReset; 
                            }else 
                             if(!sim.LotCurrent.IsCommunityLot&&
                                sim.LotCurrent!=null&&
                                sim.LotCurrent!=sim.LotHome&&
                                sim.LotCurrent!=sim.VirtualLotHome&&
                                Household.ActiveHousehold!=null&&
                                Household.ActiveHousehold.Sims!=null&&
                               !Household.ActiveHousehold.Sims.Contains(sim)){
                                     new ResetClearSimTask(sim);
goto _DidReset;
                             }else{
goto _SoftReset;
                             }
     _SoftReset:{
                          InteractionInstance 
                                currentInteraction1=sim.InteractionQueue?.GetCurrentInteraction();
                             if(currentInteraction1!=null){
                                sim.InteractionQueue.CancelInteraction(currentInteraction1.Id,ExitReason.CanceledByScript);
                             if(currentInteraction1.Target!=null){
               var targetObject=currentInteraction1.Target as GameObject;
                if(targetObject!=null){
                   targetObject.SimLine?.DoReset();
                }
                             }
                                currentInteraction1.Target?.SetObjectToReset();
                             }
                                sim.SimDescription.ClearOutfits(OutfitCategories.MartialArts,true);
                             if(sim.SimDescription.IsBonehilda){
try{
      Sim.SwitchOutfitHelper switchOutfitHelper=new Sim.SwitchOutfitHelper(sim,OutfitCategories.Everyday,0);
      switchOutfitHelper.Start();
      switchOutfitHelper.Wait(false);
      switchOutfitHelper.ChangeOutfit();
      switchOutfitHelper.Dispose();
}catch{
}
                             }
                                         ResetClearSimTask.CleanupBrokenSkills(sim.SimDescription);
                                             ResetClearSimTask.ResetCareer        (sim.SimDescription);
                                                 ResetClearSimTask.ResetSituations    (sim);
                                                     ResetClearSimTask.CleanupSlots       (sim);
                                                         ResetClearSimTask.ResetInventory     (sim);
                                                             ResetClearSimTask.ResetRouting       (sim);
                                                                 ResetClearSimTask.ResetSkillModifiers(sim.SimDescription);
                                                                     ResetClearSimTask.ResetRole          (sim);
                                                                         ResetClearSimTask.CleanupOpportunities(sim.SimDescription,false);
                                         SafeStore.FixCareer(sim.Occupation,true);
     }
     _DidReset:{}
     _Skipped:{}
                    }catch(Exception exception){
                      Alive.WriteLog(exception.Message+"\n\n"+
                                     exception.StackTrace+"\n\n"+
                                     exception.Source+"\n\n"+
                                     "ResetClearSimTasks");
                    }
                    try{
                             if(sim.Service!=null&&
                               (sim.Service.ServiceType==ServiceType.GrimReaper||
                                sim.Service.ServiceType==ServiceType.Burglar   ||
                                sim.Service.ServiceType==ServiceType.Repoman   ||
                                sim.Service.ServiceType==ServiceType.Police    ||
                                sim.Service.ServiceType==ServiceType.Firefighter||
                                sim.Service.ServiceType==ServiceType.SocialWorkerAdoption||
                                sim.Service.ServiceType==ServiceType.SocialWorkerChildProtection||
                                sim.Service.ServiceType==ServiceType.SocialWorkerPetAdoption||
                                sim.Service.ServiceType==ServiceType.SocialWorkerPetPutUp)){
                             }else{
                      if(sim.InteractionQueue!=null&&sim.InteractionQueue.mInteractionList!=null){
               for(int i=sim.InteractionQueue.mInteractionList.Count-1;i>=1;i--){
                     if(!sim.InteractionQueue.mInteractionList[i].Autonomous        )continue;
                      if(sim.InteractionQueue.mInteractionList[i]is BedSleep        )continue;
                      if(sim.InteractionQueue.mInteractionList[i]is Sleep           )continue;
                      if(sim.InteractionQueue.mInteractionList[i]is WorkInRabbitHole)continue;
                         //
                      if(sim.InteractionQueue.mInteractionList[i].Target!=null){
        var targetObject=sim.InteractionQueue.mInteractionList[i].Target as GameObject;
         if(targetObject!=null){
            targetObject.SimLine?.DoReset();
         }
                      }
                         sim.InteractionQueue.RemoveInteraction(i,false);
                         //
                         //sim.SetObjectToReset();
               }
InteractionInstance 
      currentInteraction;
  if((currentInteraction=sim.InteractionQueue.GetCurrentInteraction())!=null){
//InteractionInstance clone=null;
 if(!(currentInteraction is BedSleep        ||
      currentInteraction is Sleep           ||
      currentInteraction is WorkInRabbitHole)){
                    //clone=currentInteraction.Clone();
                         sim.InteractionQueue.CancelInteraction(currentInteraction.Id,ExitReason.CanceledByScript);
   if(currentInteraction.Target!=null){
var targetObject=currentInteraction.Target as GameObject;
 if(targetObject!=null){
    targetObject.SimLine?.DoReset();
 }
   }
      currentInteraction.Target?.SetObjectToReset();
                         sim.InteractionQueue.OnReset();
                         //
                         //sim.SetObjectToReset();
                 //if(clone!=null){
                 //        sim.InteractionQueue.Add(clone);
                 //}
 }
  }
                      }
                             }
                    }catch(Exception exception){
                      Alive.WriteLog(exception.Message+"\n\n"+
                                     exception.StackTrace+"\n\n"+
                                     exception.Source+"\n\n"+
                                     "CancelInteractions");
                    }
                    }
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source+"\n\n"+
                                 "deepFixSims");
                }
                try{
                                                                 var cookingAppliances=Sims3.Gameplay.Queries.GetObjects<ICookingAppliance>();
                                                                  if(cookingAppliances!=null)
                       foreach(ICookingAppliance cookingAppliance in cookingAppliances){
                    try{
                                             var cookingApplianceGameObject=(cookingAppliance as GameObject);
                                              if(cookingApplianceGameObject!=null){
                                      var parent=cookingApplianceGameObject.Parent;
                                bool slotted=(false);
                                     Slot parentSlot=Slot.None;
                                       if(parent!=null){
                                var slots=parent.GetContainmentSlots();
                foreach(var slot in slots){
                                       if(parent.GetContainedObject(slot)==cookingApplianceGameObject){
                                          parentSlot=slot;
                                     slotted=( true);
                                            break;
                                       }
                }
                                       }
                                                 cookingApplianceGameObject.SimLine?.DoReset();
                                                 cookingApplianceGameObject.SetObjectToReset();
                                                 cookingApplianceGameObject.ResetParentingHierarchy(true);
                                                 cookingApplianceGameObject.RemoveFromWorld();
                                                 cookingApplianceGameObject.     AddToWorld();
                                                 cookingApplianceGameObject.SetHiddenFlags(HiddenFlags.Nothing);
                                                 cookingApplianceGameObject.SetOpacity(1f,0.0f);
                                                 cookingApplianceGameObject.AddToLot();
                     LotManager.PlaceObjectOnLot(cookingApplianceGameObject,cookingApplianceGameObject.ObjectId);
                                       if(parent!=null&&
                                     slotted){
                                                 cookingApplianceGameObject.ParentToSlot(parent,parentSlot);
                                       }
                                              }
                    }catch(Exception exception){
                      Alive.WriteLog(exception.Message+"\n\n"+
                                     exception.StackTrace+"\n\n"+
                                     exception.Source+"\n\n"+
                                     "cookingApplianceGameObjectFixes_foreach");
                    }
                       }
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source+"\n\n"+
                                 "cookingApplianceGameObjectFixes");
                }finally{
                }
                try{
                        foreach(var miniFridge in Sims3.Gameplay.Queries.GetObjects<MiniFridge>()){
                         var parent=miniFridge.Parent;
                                bool slotted=(false);
                        Slot parentSlot=Slot.None;
                          if(parent!=null){
                   var slots=parent.GetContainmentSlots();
   foreach(var slot in slots){
                          if(parent.GetContainedObject(slot)==miniFridge){
                             parentSlot=slot;
                                     slotted=( true);
                            break;
                          }
   }
                          }
                                    miniFridge.SimLine?.DoReset();
                                    miniFridge.Line=new SimQueue(SimQueue.WaitLocation.HangAroundNearObject,2);
                                    miniFridge.SetObjectToReset();
                                    miniFridge.ResetParentingHierarchy(true);
                                    miniFridge.RemoveFromWorld();
                                    miniFridge.     AddToWorld();
                                    miniFridge.SetHiddenFlags(HiddenFlags.Nothing);
                                    miniFridge.SetOpacity(1f,0.0f);
                                    miniFridge.AddToLot();
                          if(parent!=null&&
                                     slotted){
                                    miniFridge.ParentToSlot(parent,parentSlot);
                          }
                        }
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source+"\n\n"+
                                 "miniFridgeFixes");
                }
                try{
                        foreach(var washingMachine in Sims3.Gameplay.Queries.GetObjects<WashingMachine>()){
                                    washingMachine.SetObjectToReset();
                        }
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source+"\n\n"+
                                 "washingMachineFixes");
                }
                try{
                        foreach(var fridge in Sims3.Gameplay.Queries.GetObjects<Fridge>()){
                                    fridge.SetObjectToReset();
                        }
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source+"\n\n"+
                                 "fridgeFixes");
                }
                try{
                       foreach(ShowVenue showVenue in Sims3.Gameplay.Queries.GetObjects<ShowVenue>()){
                    try{
           foreach(ISearchLight light in showVenue.LotCurrent.GetObjects<ISearchLight>()){
                                light.TurnOff();
        SearchLight searchLight=light as SearchLight;
                 if(searchLight!=null){
                    searchLight.mSMC?.Dispose();
                    searchLight.mSMC=null;
                 }
           }
                                         showVenue.EndPlayerConcert();
                    }catch(Exception exception){
                      Alive.WriteLog(exception.Message+"\n\n"+
                                     exception.StackTrace+"\n\n"+
                                     exception.Source+"\n\n"+
                                     "concertFixes_foreach");
                    }finally{
                    }
                       }
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source+"\n\n"+
                                 "concertFixes");
                }finally{
                }
                try{
                               var stuckSimDataInvalid=new List<ulong>();
                       foreach(var stuckSimData in StuckSims){
                               if(!stuckSimData.Value.Resetting){
                               Sim sim;
                               if((sim=SimDescription.GetCreatedSim(stuckSimData.Key))==null||sim.HasBeenDestroyed){
                                   stuckSimDataInvalid.
                               Add(stuckSimData.Key);
                               }
                               }
                       }
         foreach(var invalidSim in stuckSimDataInvalid){
    StuckSims.Remove(invalidSim);
         }
                                   stuckSimDataInvalid.Clear();
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source+"\n\n"+
                                 "stuckSimData_refresh");
                }finally{
                }
                try{
                foreach(var jig in Sims3.Gameplay.Queries.GetObjects<SocialJig>()){
                    try{
                         if(jig.SimA!=null){
InteractionInstance 
         currentInteraction;
                         if(jig.SimA.InteractionQueue!=null&&
        (currentInteraction=jig.SimA.InteractionQueue.GetCurrentInteraction())!=null){
                            jig.SimA.InteractionQueue.CancelInteraction(currentInteraction.Id,ExitReason.CanceledByScript);
                            }
                         }
                         if(jig.SimB!=null){
InteractionInstance 
         currentInteraction;
                         if(jig.SimB.InteractionQueue!=null&&
        (currentInteraction=jig.SimB.InteractionQueue.GetCurrentInteraction())!=null){
                            jig.SimB.InteractionQueue.CancelInteraction(currentInteraction.Id,ExitReason.CanceledByScript);
                            }
                         }
                            jig.ClearParticipants();
                    }catch(Exception exception){
                      Alive.WriteLog(exception.Message+"\n\n"+
                                     exception.StackTrace+"\n\n"+
                                     exception.Source+"\n\n"+
                                     "deleteJigs_clear");
                    }finally{
                    }
                }
                foreach(var jig in Sims3.Gameplay.Queries.GetObjects<Jig>()){
                    try{
                            jig.SetObjectToReset();//
                            //
                            jig.Destroy();
                    }catch(Exception exception){
                      Alive.WriteLog(exception.Message+"\n\n"+
                                     exception.StackTrace+"\n\n"+
                                     exception.Source+"\n\n"+
                                     "deleteJigs_destroy");
                    }finally{
                    }
                }
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source+"\n\n"+
                                 "deleteJigs");
                }finally{
                }
        }
        //==================================================================================================================
        public static bool IsRootMenuObject(Sims3.Gameplay.Interfaces.IGameObject obj){
                                                                               if(obj is Sims3.Gameplay.Core.  Lot){
                                                                                return( true);
                                                                               }
                                                                               else 
                                                                               if(obj is Sims3.Gameplay.Actors.Sim){
                                                                                return( true);
                                                                               }
                                                                                return(false);
        }
        //==================================================================================================================
        public static List<T>CloneList<T>(IEnumerable<T>old){
                                                     if(old==null)return null;
           return new List<T>(old);
        }
        //==================================================================================================================
        public static string Localize(string key){
                      return Localize(       key,    false,new object[0]);
        }
        public static string Localize(string key,bool isFemale,object[] parameters){
                      return Localize(       key,     isFemale,  false, parameters);
        }
        public static string Localize(string key,bool isActorFemale,bool isTargetFemale,object[] parameters){
                                                                                                         string result;
                          if(Localize(       key,     isActorFemale,     isTargetFemale,         parameters,out result)){
                                                                                                         return result;
                          }else{
                                                                                                         return Interaction.VersionStamp.sNamespace+" "+"Mod Empty Interaction";
                          }
        }
          public static bool Localize(string key,bool isActorFemale,bool isTargetFemale,object[] parameters,out string result){
                                                                                                                       result=null;
            try{
                                                                                                                       result=Localization.LocalizeString(new bool[]{isActorFemale,isTargetFemale},Interaction.VersionStamp.sNamespace+"."+key,parameters);
            }catch(Exception exception){
              Alive.WriteLog(exception.Message+"\n\n"+
                             exception.StackTrace+"\n\n"+
                             exception.Source);
            }
                                                                                               if(string.IsNullOrEmpty(result))return(false);
                                                                                                                    if(result.StartsWith(Interaction.VersionStamp.sNamespace+"."))return(false);
          return( true);}
        //==================================================================================================================
        public static void WriteLog(string text){
            try{
                                                      uint fileHandle=(0x0);
            string str=Simulator.CreateScriptErrorFile(ref fileHandle);
                                                        if(fileHandle!=0x0){
             CustomXmlWriter xmlWriter=new CustomXmlWriter(fileHandle);
                             xmlWriter.WriteToBuffer(text);
                             xmlWriter.WriteEndDocument();
                                                        }
            }catch{}
        }
        //==================================================================================================================
        public static bool Sleep(uint value){
                      bool flag;
                try{
                if(Simulator.CheckYieldingContext(false)){
                   Simulator.Sleep(value);
                    return( true);
                }
                           flag=(false);
                }catch(ResetException exception){
                       Alive.WriteLog(exception.Message+"\n\n"+
                                      exception.StackTrace+"\n\n"+
                                      exception.Source);
                           flag=(false);
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source+"\n\n"+
                                 "Sleep");
                           flag=(false);
                }
                    return(flag );
        }
        public static bool Sleep(){
                    return Sleep(0);
        }
        //==================================================================================================================
    }
}