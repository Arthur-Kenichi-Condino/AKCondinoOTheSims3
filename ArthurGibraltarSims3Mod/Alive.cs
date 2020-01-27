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
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Careers;
using Sims3.Gameplay.CAS;
using Sims3.Gameplay.CAS.Locale;
using Sims3.Gameplay.Controllers;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.EventSystem;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Interfaces;
using Sims3.Gameplay.ObjectComponents;
using Sims3.Gameplay.Objects;
using Sims3.Gameplay.Objects.Appliances;
using Sims3.Gameplay.Objects.Beds;
using Sims3.Gameplay.Objects.Elevator;
using Sims3.Gameplay.Objects.Environment;
using Sims3.Gameplay.Objects.FoodObjects;
using Sims3.Gameplay.Objects.Lighting;
using Sims3.Gameplay.Objects.Miscellaneous;
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
namespace ArthurGibraltarSims3Mod{
    public class Alive{
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
                }
          }
        private static void OnPreLoad(){
                try{
                       using(TestSpan span=new TestSpan()){
                        List<IPreLoad>helpers=DerivativeSearch.Find<IPreLoad>();
                     foreach(IPreLoad helper in helpers){
                    try{
                       using(TestSpan helperSpan=new TestSpan()){
                        try{
                                      helper.OnPreLoad();
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
                        }
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
                    }
                     }
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
                }
        }
        public interface IPreLoad{
                   void OnPreLoad();
        }
        private static void OnWorldLoadFinished(object sender,EventArgs e){
                try{
            Sims3.Gameplay.StoryProgression.EmigrateHousehold.kMultiplierForEmigratingKnownHousehold                  =0;
            Sims3.Gameplay.StoryProgression.EmigrateHousehold.kMultiplierForEmigratingProtectedHousehold              =0;
            Sims3.Gameplay.StoryProgression.EmigrateHousehold.kMultiplierForEmigratingAcademicCareerProfessorHousehold=0;
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
                }
                try{
                    using(TestSpan span=new TestSpan()){
                    try{
                           RegAllInteractions();//  Perform prior to calling the derivatives
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
                    }
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
                }
                try{
                new DelayedEventListener(Sims3.Gameplay.EventSystem.EventTypeId.kBoughtObject,OnNewObject);
                new DelayedEventListener(Sims3.Gameplay.EventSystem.EventTypeId.kSimInstantiated,OnNewSim);
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
                }
             //---------------------------------------------------------------
                try{
            Route.AboutToPlanCallback+=OnAboutToPlan;
            Route.   PostPlanCallback+=   OnPostPlan;
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
                }
             //---------------------------------------------------------------
                try{
                bool[]tunned=new bool[18];
            foreach(var tuning in InteractionTuning.sAllTunings.Values){
                    try{
                     if(!tunned[0]&&
                        tuning.FullInteractionName=="Sims3.Gameplay.Objects.Gardening.Plant+Graaiins+Definition"){
                         tunned[0]=true;
                        tuning.AddFlags(InteractionTuning.FlagField.DisallowAutonomous);
                     }else
                     if(!tunned[1]&&
                        tuning.FullInteractionName=="Sims3.Gameplay.Core.SwipeSomethingAutonomous+Definition"){
                         tunned[1]=true;
                        tuning.AddFlags(InteractionTuning.FlagField.DisallowAutonomous);
                     }else
                     if(!tunned[2]&&
                        tuning.FullInteractionName=="Sims3.Gameplay.ActorSystems.TraitFunctions+SwipeSomething+Definition"){
                         tunned[2]=true;
                        tuning.AddFlags(InteractionTuning.FlagField.DisallowAutonomous);
                     }else
                     if(!tunned[3]&&
                        tuning.FullInteractionName=="Sims3.Gameplay.Objects.Miscellaneous.ClothingPileDry+CleanUp+Definition"){
                         tunned[3]=true;
                        tuning.RemoveFlags(InteractionTuning.FlagField.DisallowAutonomous);
              for(int i=tuning.mTradeoff.mOutputs.Count-1;i>=0;i--){
                     if(tuning.mTradeoff.mOutputs[i].Commodity==CommodityKind.BeBonehilda){
                        tuning.mTradeoff.mOutputs.RemoveAt(i);
                     }
              }
                        tuning.mTradeoff.mOutputs.Add(new CommodityChange(CommodityKind.BeBonehilda,300,true,300,OutputUpdateType.ContinuousFlow,false,false,UpdateAboveAndBelowZeroType.Either));
                        tuning.Availability.OccultRestrictions=Sims3.UI.Hud.OccultTypes.None;
tuning.Availability.AddFlags(Availability.FlagField.AllowGreetedSims);
tuning.Availability.AddFlags(Availability.FlagField.AllowEvenIfNotAllowedInRoomAutonomous);
tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideAutonomous);
tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideUserDirected);
tuning.Availability.AddFlags(Availability.FlagField.AllowOnCommunityLots);
tuning.Availability.AddFlags(Availability.FlagField.AllowOnAllLots);
                        tuning.RemoveFlags(InteractionTuning.FlagField.ConsiderCodeVersion);
                     }else
                     if(!tunned[4]&&
                        tuning.FullInteractionName=="Sims3.Gameplay.Objects.Miscellaneous.ClothingPileWet+DryClothesInDryer+Definition"){
                         tunned[4]=true;
                        tuning.RemoveFlags(InteractionTuning.FlagField.DisallowAutonomous);
              for(int i=tuning.mTradeoff.mOutputs.Count-1;i>=0;i--){
                     if(tuning.mTradeoff.mOutputs[i].Commodity==CommodityKind.BeBonehilda){
                        tuning.mTradeoff.mOutputs.RemoveAt(i);
                     }
              }
                        tuning.mTradeoff.mOutputs.Add(new CommodityChange(CommodityKind.BeBonehilda,300,true,300,OutputUpdateType.ContinuousFlow,false,false,UpdateAboveAndBelowZeroType.Either));
                        tuning.Availability.OccultRestrictions=Sims3.UI.Hud.OccultTypes.None;
tuning.Availability.AddFlags(Availability.FlagField.AllowGreetedSims);
tuning.Availability.AddFlags(Availability.FlagField.AllowEvenIfNotAllowedInRoomAutonomous);
tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideAutonomous);
tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideUserDirected);
tuning.Availability.AddFlags(Availability.FlagField.AllowOnCommunityLots);
tuning.Availability.AddFlags(Availability.FlagField.AllowOnAllLots);
                        tuning.RemoveFlags(InteractionTuning.FlagField.ConsiderCodeVersion);
                     }else
                     if(!tunned[5]&&
                        tuning.FullInteractionName=="Sims3.Gameplay.Objects.Miscellaneous.ClothingPileWet+DryClothesOnClothesline+Definition"){
                         tunned[5]=true;
                        tuning.RemoveFlags(InteractionTuning.FlagField.DisallowAutonomous);
              for(int i=tuning.mTradeoff.mOutputs.Count-1;i>=0;i--){
                     if(tuning.mTradeoff.mOutputs[i].Commodity==CommodityKind.BeBonehilda){
                        tuning.mTradeoff.mOutputs.RemoveAt(i);
                     }
              }
                        tuning.mTradeoff.mOutputs.Add(new CommodityChange(CommodityKind.BeBonehilda,200,true,200,OutputUpdateType.ContinuousFlow,false,false,UpdateAboveAndBelowZeroType.Either));
                        tuning.Availability.OccultRestrictions=Sims3.UI.Hud.OccultTypes.None;
tuning.Availability.AddFlags(Availability.FlagField.AllowGreetedSims);
tuning.Availability.AddFlags(Availability.FlagField.AllowEvenIfNotAllowedInRoomAutonomous);
tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideAutonomous);
tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideUserDirected);
tuning.Availability.AddFlags(Availability.FlagField.AllowOnCommunityLots);
tuning.Availability.AddFlags(Availability.FlagField.AllowOnAllLots);
                        tuning.RemoveFlags(InteractionTuning.FlagField.ConsiderCodeVersion);
                     }else
                     if(!tunned[6]&&
                        tuning.FullInteractionName=="Sims3.Gameplay.Objects.Miscellaneous.ClothingPileDry+PutDown+Definition"){
                         tunned[6]=true;
                        tuning.RemoveFlags(InteractionTuning.FlagField.DisallowAutonomous);
              for(int i=tuning.mTradeoff.mOutputs.Count-1;i>=0;i--){
                     if(tuning.mTradeoff.mOutputs[i].Commodity==CommodityKind.BeBonehilda){
                        tuning.mTradeoff.mOutputs.RemoveAt(i);
                     }
              }
                        //tuning.mTradeoff.mOutputs.Add(new CommodityChange(CommodityKind.BeBonehilda,200,true,200,OutputUpdateType.ContinuousFlow,false,false,UpdateAboveAndBelowZeroType.Either));
                        tuning.Availability.OccultRestrictions=Sims3.UI.Hud.OccultTypes.None;
tuning.Availability.AddFlags(Availability.FlagField.AllowGreetedSims);
tuning.Availability.AddFlags(Availability.FlagField.AllowEvenIfNotAllowedInRoomAutonomous);
tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideAutonomous);
tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideUserDirected);
tuning.Availability.AddFlags(Availability.FlagField.AllowOnCommunityLots);
tuning.Availability.AddFlags(Availability.FlagField.AllowOnAllLots);
                        tuning.RemoveFlags(InteractionTuning.FlagField.ConsiderCodeVersion);
                     }else
                     if(!tunned[7]&&
                        tuning.FullInteractionName=="Sims3.Gameplay.Objects.Miscellaneous.ClothingPileWet+PutDown+Definition"){
                         tunned[7]=true;
                        tuning.RemoveFlags(InteractionTuning.FlagField.DisallowAutonomous);
              for(int i=tuning.mTradeoff.mOutputs.Count-1;i>=0;i--){
                     if(tuning.mTradeoff.mOutputs[i].Commodity==CommodityKind.BeBonehilda){
                        tuning.mTradeoff.mOutputs.RemoveAt(i);
                     }
              }
                        //tuning.mTradeoff.mOutputs.Add(new CommodityChange(CommodityKind.BeBonehilda,200,true,200,OutputUpdateType.ContinuousFlow,false,false,UpdateAboveAndBelowZeroType.Either));
                        tuning.Availability.OccultRestrictions=Sims3.UI.Hud.OccultTypes.None;
tuning.Availability.AddFlags(Availability.FlagField.AllowGreetedSims);
tuning.Availability.AddFlags(Availability.FlagField.AllowEvenIfNotAllowedInRoomAutonomous);
tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideAutonomous);
tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideUserDirected);
tuning.Availability.AddFlags(Availability.FlagField.AllowOnCommunityLots);
tuning.Availability.AddFlags(Availability.FlagField.AllowOnAllLots);
                        tuning.RemoveFlags(InteractionTuning.FlagField.ConsiderCodeVersion);
                     }else
                     if(!tunned[8]&&
                        tuning.FullInteractionName=="Sims3.Gameplay.Objects.Miscellaneous.Hamper+DoLaundry+Definition"){
                         tunned[8]=true;
                        tuning.RemoveFlags(InteractionTuning.FlagField.DisallowAutonomous);
              for(int i=tuning.mTradeoff.mOutputs.Count-1;i>=0;i--){
                     if(tuning.mTradeoff.mOutputs[i].Commodity==CommodityKind.BeBonehilda){
                        tuning.mTradeoff.mOutputs.RemoveAt(i);
                     }
              }
                        tuning.mTradeoff.mOutputs.Add(new CommodityChange(CommodityKind.BeBonehilda,500,true,500,OutputUpdateType.ContinuousFlow,false,false,UpdateAboveAndBelowZeroType.Either));
                        tuning.Availability.OccultRestrictions=Sims3.UI.Hud.OccultTypes.None;
tuning.Availability.AddFlags(Availability.FlagField.AllowGreetedSims);
tuning.Availability.AddFlags(Availability.FlagField.AllowEvenIfNotAllowedInRoomAutonomous);
tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideAutonomous);
tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideUserDirected);
tuning.Availability.AddFlags(Availability.FlagField.AllowOnCommunityLots);
tuning.Availability.AddFlags(Availability.FlagField.AllowOnAllLots);
                        tuning.RemoveFlags(InteractionTuning.FlagField.ConsiderCodeVersion);
                     }else
                     if(!tunned[9]&&
                        tuning.FullInteractionName=="Sims3.Gameplay.Objects.Miscellaneous.Hamper+PickUp+Definition"){
                         tunned[9]=true;
                        tuning.RemoveFlags(InteractionTuning.FlagField.DisallowAutonomous);
              for(int i=tuning.mTradeoff.mOutputs.Count-1;i>=0;i--){
                     if(tuning.mTradeoff.mOutputs[i].Commodity==CommodityKind.BeBonehilda){
                        tuning.mTradeoff.mOutputs.RemoveAt(i);
                     }
              }
                        //tuning.mTradeoff.mOutputs.Add(new CommodityChange(CommodityKind.BeBonehilda,200,true,200,OutputUpdateType.ContinuousFlow,false,false,UpdateAboveAndBelowZeroType.Either));
                        tuning.Availability.OccultRestrictions=Sims3.UI.Hud.OccultTypes.None;
tuning.Availability.AddFlags(Availability.FlagField.AllowGreetedSims);
tuning.Availability.AddFlags(Availability.FlagField.AllowEvenIfNotAllowedInRoomAutonomous);
tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideAutonomous);
tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideUserDirected);
tuning.Availability.AddFlags(Availability.FlagField.AllowOnCommunityLots);
tuning.Availability.AddFlags(Availability.FlagField.AllowOnAllLots);
                        tuning.RemoveFlags(InteractionTuning.FlagField.ConsiderCodeVersion);
                     }else
                     if(!tunned[10]&&
                        tuning.FullInteractionName=="Sims3.Gameplay.Objects.Miscellaneous.Hamper+DropClothes+Definition"){
                         tunned[10]=true;
                        tuning.RemoveFlags(InteractionTuning.FlagField.DisallowAutonomous);
              for(int i=tuning.mTradeoff.mOutputs.Count-1;i>=0;i--){
                     if(tuning.mTradeoff.mOutputs[i].Commodity==CommodityKind.BeBonehilda){
                        tuning.mTradeoff.mOutputs.RemoveAt(i);
                     }
              }
                        //tuning.mTradeoff.mOutputs.Add(new CommodityChange(CommodityKind.BeBonehilda,200,true,200,OutputUpdateType.ContinuousFlow,false,false,UpdateAboveAndBelowZeroType.Either));
                        tuning.Availability.OccultRestrictions=Sims3.UI.Hud.OccultTypes.None;
tuning.Availability.AddFlags(Availability.FlagField.AllowGreetedSims);
tuning.Availability.AddFlags(Availability.FlagField.AllowEvenIfNotAllowedInRoomAutonomous);
tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideAutonomous);
tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideUserDirected);
tuning.Availability.AddFlags(Availability.FlagField.AllowOnCommunityLots);
tuning.Availability.AddFlags(Availability.FlagField.AllowOnAllLots);
                        tuning.RemoveFlags(InteractionTuning.FlagField.ConsiderCodeVersion);
                     }else
                     if(!tunned[11]&&
                        tuning.FullInteractionName=="Sims3.Gameplay.Objects.Appliances.WashingMachine+DoLaundry+Definition"){
                         tunned[11]=true;
                        tuning.RemoveFlags(InteractionTuning.FlagField.DisallowAutonomous);
              for(int i=tuning.mTradeoff.mOutputs.Count-1;i>=0;i--){
                     if(tuning.mTradeoff.mOutputs[i].Commodity==CommodityKind.BeBonehilda){
                        tuning.mTradeoff.mOutputs.RemoveAt(i);
                     }
              }
                        tuning.mTradeoff.mOutputs.Add(new CommodityChange(CommodityKind.BeBonehilda,600,true,600,OutputUpdateType.ContinuousFlow,false,false,UpdateAboveAndBelowZeroType.Either));
                        tuning.Availability.OccultRestrictions=Sims3.UI.Hud.OccultTypes.None;
tuning.Availability.AddFlags(Availability.FlagField.AllowGreetedSims);
tuning.Availability.AddFlags(Availability.FlagField.AllowEvenIfNotAllowedInRoomAutonomous);
tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideAutonomous);
tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideUserDirected);
tuning.Availability.AddFlags(Availability.FlagField.AllowOnCommunityLots);
tuning.Availability.AddFlags(Availability.FlagField.AllowOnAllLots);
                        tuning.RemoveFlags(InteractionTuning.FlagField.ConsiderCodeVersion);
                     }else
                     if(!tunned[12]&&
                        tuning.FullInteractionName=="Sims3.Gameplay.Objects.Appliances.Clothesline+DryClothing+Definition"){
                         tunned[12]=true;
                        tuning.RemoveFlags(InteractionTuning.FlagField.DisallowAutonomous);
              for(int i=tuning.mTradeoff.mOutputs.Count-1;i>=0;i--){
                     if(tuning.mTradeoff.mOutputs[i].Commodity==CommodityKind.BeBonehilda){
                        tuning.mTradeoff.mOutputs.RemoveAt(i);
                     }
              }
                        tuning.mTradeoff.mOutputs.Add(new CommodityChange(CommodityKind.BeBonehilda,700,true,700,OutputUpdateType.ContinuousFlow,false,false,UpdateAboveAndBelowZeroType.Either));
                        tuning.Availability.OccultRestrictions=Sims3.UI.Hud.OccultTypes.None;
tuning.Availability.AddFlags(Availability.FlagField.AllowGreetedSims);
tuning.Availability.AddFlags(Availability.FlagField.AllowEvenIfNotAllowedInRoomAutonomous);
tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideAutonomous);
tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideUserDirected);
tuning.Availability.AddFlags(Availability.FlagField.AllowOnCommunityLots);
tuning.Availability.AddFlags(Availability.FlagField.AllowOnAllLots);
                        tuning.RemoveFlags(InteractionTuning.FlagField.ConsiderCodeVersion);
                     }else
                     if(!tunned[13]&&
                        tuning.FullInteractionName=="Sims3.Gameplay.Objects.Appliances.Clothesline+GetCleanLaundry+Definition"){
                         tunned[13]=true;
                        tuning.RemoveFlags(InteractionTuning.FlagField.DisallowAutonomous);
              for(int i=tuning.mTradeoff.mOutputs.Count-1;i>=0;i--){
                     if(tuning.mTradeoff.mOutputs[i].Commodity==CommodityKind.BeBonehilda){
                        tuning.mTradeoff.mOutputs.RemoveAt(i);
                     }
              }
                        tuning.mTradeoff.mOutputs.Add(new CommodityChange(CommodityKind.BeBonehilda,800,true,800,OutputUpdateType.ContinuousFlow,false,false,UpdateAboveAndBelowZeroType.Either));
                        tuning.Availability.OccultRestrictions=Sims3.UI.Hud.OccultTypes.None;
tuning.Availability.AddFlags(Availability.FlagField.AllowGreetedSims);
tuning.Availability.AddFlags(Availability.FlagField.AllowEvenIfNotAllowedInRoomAutonomous);
tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideAutonomous);
tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideUserDirected);
tuning.Availability.AddFlags(Availability.FlagField.AllowOnCommunityLots);
tuning.Availability.AddFlags(Availability.FlagField.AllowOnAllLots);
                        tuning.RemoveFlags(InteractionTuning.FlagField.ConsiderCodeVersion);
                     }else
                     if(!tunned[14]&&
                        tuning.FullInteractionName=="Sims3.Gameplay.Objects.Appliances.Dryer+DryClothing+Definition"){
                         tunned[14]=true;
                        tuning.RemoveFlags(InteractionTuning.FlagField.DisallowAutonomous);
              for(int i=tuning.mTradeoff.mOutputs.Count-1;i>=0;i--){
                     if(tuning.mTradeoff.mOutputs[i].Commodity==CommodityKind.BeBonehilda){
                        tuning.mTradeoff.mOutputs.RemoveAt(i);
                     }
              }
                        tuning.mTradeoff.mOutputs.Add(new CommodityChange(CommodityKind.BeBonehilda,800,true,800,OutputUpdateType.ContinuousFlow,false,false,UpdateAboveAndBelowZeroType.Either));
                        tuning.Availability.OccultRestrictions=Sims3.UI.Hud.OccultTypes.None;
tuning.Availability.AddFlags(Availability.FlagField.AllowGreetedSims);
tuning.Availability.AddFlags(Availability.FlagField.AllowEvenIfNotAllowedInRoomAutonomous);
tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideAutonomous);
tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideUserDirected);
tuning.Availability.AddFlags(Availability.FlagField.AllowOnCommunityLots);
tuning.Availability.AddFlags(Availability.FlagField.AllowOnAllLots);
                        tuning.RemoveFlags(InteractionTuning.FlagField.ConsiderCodeVersion);
                     }else
                     if(!tunned[15]&&
                        tuning.FullInteractionName=="Sims3.Gameplay.Objects.Appliances.Dryer+GetCleanLaundry+Definition"){
                         tunned[15]=true;
                        tuning.RemoveFlags(InteractionTuning.FlagField.DisallowAutonomous);
              for(int i=tuning.mTradeoff.mOutputs.Count-1;i>=0;i--){
                     if(tuning.mTradeoff.mOutputs[i].Commodity==CommodityKind.BeBonehilda){
                        tuning.mTradeoff.mOutputs.RemoveAt(i);
                     }
              }
                        tuning.mTradeoff.mOutputs.Add(new CommodityChange(CommodityKind.BeBonehilda,900,true,900,OutputUpdateType.ContinuousFlow,false,false,UpdateAboveAndBelowZeroType.Either));
                        tuning.Availability.OccultRestrictions=Sims3.UI.Hud.OccultTypes.None;
tuning.Availability.AddFlags(Availability.FlagField.AllowGreetedSims);
tuning.Availability.AddFlags(Availability.FlagField.AllowEvenIfNotAllowedInRoomAutonomous);
tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideAutonomous);
tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideUserDirected);
tuning.Availability.AddFlags(Availability.FlagField.AllowOnCommunityLots);
tuning.Availability.AddFlags(Availability.FlagField.AllowOnAllLots);
                        tuning.RemoveFlags(InteractionTuning.FlagField.ConsiderCodeVersion);
                     }else
                     if(!tunned[16]&&
                        tuning.FullInteractionName=="Sims3.Gameplay.Objects.Appliances.WashingMachine+DryClothesInDryer+Definition"){
                         tunned[16]=true;
                        tuning.RemoveFlags(InteractionTuning.FlagField.DisallowAutonomous);
              for(int i=tuning.mTradeoff.mOutputs.Count-1;i>=0;i--){
                     if(tuning.mTradeoff.mOutputs[i].Commodity==CommodityKind.BeBonehilda){
                        tuning.mTradeoff.mOutputs.RemoveAt(i);
                     }
              }
                        tuning.mTradeoff.mOutputs.Add(new CommodityChange(CommodityKind.BeBonehilda,800,true,800,OutputUpdateType.ContinuousFlow,false,false,UpdateAboveAndBelowZeroType.Either));
                        tuning.Availability.OccultRestrictions=Sims3.UI.Hud.OccultTypes.None;
tuning.Availability.AddFlags(Availability.FlagField.AllowGreetedSims);
tuning.Availability.AddFlags(Availability.FlagField.AllowEvenIfNotAllowedInRoomAutonomous);
tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideAutonomous);
tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideUserDirected);
tuning.Availability.AddFlags(Availability.FlagField.AllowOnCommunityLots);
tuning.Availability.AddFlags(Availability.FlagField.AllowOnAllLots);
                        tuning.RemoveFlags(InteractionTuning.FlagField.ConsiderCodeVersion);
                     }else
                     if(!tunned[17]&&
                        tuning.FullInteractionName=="Sims3.Gameplay.Objects.Appliances.WashingMachine+DryClothesOnClothesline+Definition"){
                         tunned[17]=true;
                        tuning.RemoveFlags(InteractionTuning.FlagField.DisallowAutonomous);
              for(int i=tuning.mTradeoff.mOutputs.Count-1;i>=0;i--){
                     if(tuning.mTradeoff.mOutputs[i].Commodity==CommodityKind.BeBonehilda){
                        tuning.mTradeoff.mOutputs.RemoveAt(i);
                     }
              }
                        tuning.mTradeoff.mOutputs.Add(new CommodityChange(CommodityKind.BeBonehilda,700,true,700,OutputUpdateType.ContinuousFlow,false,false,UpdateAboveAndBelowZeroType.Either));
                        tuning.Availability.OccultRestrictions=Sims3.UI.Hud.OccultTypes.None;
tuning.Availability.AddFlags(Availability.FlagField.AllowGreetedSims);
tuning.Availability.AddFlags(Availability.FlagField.AllowEvenIfNotAllowedInRoomAutonomous);
tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideAutonomous);
tuning.Availability.AddFlags(Availability.FlagField.AllowNonGreetedSimsIfObjectOutsideUserDirected);
tuning.Availability.AddFlags(Availability.FlagField.AllowOnCommunityLots);
tuning.Availability.AddFlags(Availability.FlagField.AllowOnAllLots);
                        tuning.RemoveFlags(InteractionTuning.FlagField.ConsiderCodeVersion);
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
                    }
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
                }finally{
                }
            //Sims3.Gameplay.Actors.SimRoutingComponent.;
            //Sims3.Gameplay.Actors.Sim.
            //Sims3.Gameplay.Abstracts.GameObject.
            //foreach(var speciesData in){
            //}
            //Sims3.Metadata.SpeciesDefinitions
            //Sims3.SimIFace.Route.
            //Sims3.SimIFace.
             //---------------------------------------------------------------
                try{
                     string recipeNames="Alive_recipeNames_LOG:NOT_ERROR\n";
                foreach(var recipe in Recipe.NameToRecipeHash){
                            recipeNames+="Recipe.NameToRecipeHash["+recipe.Key+"]="+recipe.Value+"\n";
                }
             Alive.WriteLog(recipeNames);
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
                }finally{
                }
                try{
             new AlarmTask( 2,TimeUnit.Hours,DoHourlyTasks,1,TimeUnit.Hours);
             //---------------------------------------------------------------
             new AlarmTask(07,DaysOfTheWeek.All,FeedSims);//  Breakfast...
             new AlarmTask(13,DaysOfTheWeek.All,FeedSims);//  ...Lunch...
             new AlarmTask(19,DaysOfTheWeek.All,FeedSims);//  Dinner...
             //---------------------------------------------------------------
             new AlarmTask( 5,DaysOfTheWeek.All,AutoPause);
             //---------------------------------------------------------------
             new AlarmTask( 1,TimeUnit.Hours  ,CheckShowVenues       ,1,TimeUnit.Hours);
             //---------------------------------------------------------------
             new AlarmTask( 7,TimeUnit.Minutes,RecoverMissingSims    ,1,TimeUnit.Hours);
             //---------------------------------------------------------------
             new AlarmTask( 5,TimeUnit.Minutes,ResetPortalsAndRouting,1,TimeUnit.Hours);
             //---------------------------------------------------------------
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
                               string debugInfo="Alive_debugInfo_LOG:NOT_ERROR\n";
                                      debugInfo+=cookingApplianceGameObject.ObjectInstanceName+"_l1\n";
                                      debugInfo+=cookingApplianceGameObject.ObjectId+"_l2\n";
                                      debugInfo+=cookingApplianceGameObject.ActorsUsingMe?.Count+"_l3\n";
                                      debugInfo+=cookingApplianceGameObject.SimLine?.SimsInQueue?.Count+"_l4\n";
                                      debugInfo+=cookingApplianceGameObject.SimLine?.FirstSim?.Name+"_l5\n";
                       //Alive.WriteLog(debugInfo);
                                              }
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
                    }
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
                }
                try{
                        foreach(var washingMachine in Sims3.Gameplay.Queries.GetObjects<WashingMachine>()){
                                    washingMachine.SetObjectToReset();
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
                }
                try{
                        foreach(var fridge in Sims3.Gameplay.Queries.GetObjects<Fridge>()){
                                    fridge.SetObjectToReset();
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
                }
        }
        private static void OnWorldQuit(object sender,EventArgs e){
                try{
            Route.AboutToPlanCallback-=OnAboutToPlan;
            Route.   PostPlanCallback-=   OnPostPlan;
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
                }finally{
                }
                try{
    AlarmTask.DisposeAll();
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
                }finally{
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
                }finally{
                }
                try{
                                                                          (e.TargetObject as Sim).PlayRouteFailFrequency=Sim.RouteFailFrequency.NeverPlayRouteFail;
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
                }finally{
                }
             }
        protected static void OnNewObject(Sims3.Gameplay.EventSystem.Event e){
                try{
                                InteractionInjectorList.MasterList.Perform(e.TargetObject as GameObject);
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
                }finally{
                }
        }
        protected static void 
                           RegAllInteractionsDelayed(){
                try{
                           RegAllInteractions(0,true);
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
                    using(TestSpan span=new TestSpan()){
                        try{
      list.Perform(obj);
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
                        }finally{
                        }
                    }
  if(localFull){
    count++;
 if(count>250){
                Sleep();
    count=0;
 }
  }
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
                    }finally{
                    }
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
                }finally{
                }
        }
        public class TestSpan:IDisposable{
            DateTime mThen;
              public TestSpan(){
                     mThen=DateTime.Now;
              }
       public static TestSpan CreateSimple(){
          return new TestSpan();
       }
            public long Duration{get{return(DateTime.Now-mThen).Ticks/TimeSpan.TicksPerMillisecond;}}
            public void Dispose(){
                try{
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
                }finally{
                }
            }
        }
        //==================================================================================================================
        public class DelayedEventListener:EventListenerTask{
              public DelayedEventListener(Sims3.Gameplay.EventSystem.EventTypeId id,Func func):base(id,func){}
protected override Sims3.Gameplay.EventSystem.ListenerAction OnProcess(Sims3.Gameplay.EventSystem.Event e){
                DelayTask.Perform(e,mFunc);
return Sims3.Gameplay.EventSystem.ListenerAction.Keep;}
            public class DelayTask:ModTask{
                Sims3.Gameplay.EventSystem.Event mEvent;
                                            Func mFunc;
                protected DelayTask(Sims3.Gameplay.EventSystem.Event e,Func func){
                                                              mEvent=e;
                                                                      mFunc=func;
                }
                  public static void Perform(Sims3.Gameplay.EventSystem.Event e,Func func){
                try{
                      new DelayTask(e,func).AddToSimulator();
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
                }finally{
                }
                  }
           protected override void OnPerform(){
                try{
                        mFunc(mEvent);
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
                }finally{
                }
           }
            }
        }
        public abstract class EventListenerTask{
            public delegate void Func(Sims3.Gameplay.EventSystem.Event e);
                       protected Func mFunc;
protected Sims3.Gameplay.EventSystem.EventListener mListener;
   public Sims3.Gameplay.EventSystem.EventListener  Listener{get{return mListener;}}
                       public EventListenerTask(Sims3.Gameplay.EventSystem.EventTypeId id,Func func){
                                                                                            if(func==null){
                                      mFunc=OnPerform;
                                                                                            }else{
                                      mFunc=func;
                                                                                            }
                try{
            
                                                   mListener=Sims3.Gameplay.EventSystem.EventTracker.AddListener(id,OnProcess);//  Must be immediate
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
                }finally{
                }
                       }
protected abstract Sims3.Gameplay.EventSystem.ListenerAction OnProcess(Sims3.Gameplay.EventSystem.Event e);
            protected virtual void OnPerform(Sims3.Gameplay.EventSystem.Event e){
                try{
                    throw new NotImplementedException();
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
                }finally{
                }
            }
            public void Dispose(){
                try{
      Sims3.Gameplay.EventSystem.EventTracker.RemoveListener(mListener);
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
                }finally{
                }
            }
        }
        //==================================================================================================================
static Dictionary<Sim,Vector3>stuckPositions=new Dictionary<Sim,Vector3>();
        static void AutoPause(){
                try{
      Sims3.Gameplay.Gameflow.SetGameSpeed(Sims3.Gameplay.Gameflow.GameSpeed.Pause,Sims3.Gameplay.Gameflow.SetGameSpeedContext.GameStates);
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
                }
                try{
                    for(int i=Situation.sAllSituations.Count-1;i>=0;i--){
          Situation situation=Situation.sAllSituations[i];
                    try{
                    bool tested=(false),
                         remove=(false);
                 if(situation is FieldTripSituation){
                         tested=( true);
                         remove=( true);
                 }
                        try{
                    situation.Exit();
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
                        }
foreach(Sim sim in LotManager.Actors){
     sim.RemoveRole(situation);
}
                          if((Situation.sAllSituations.Count>i)&&
                             (Situation.sAllSituations[i]==situation)){
                              Situation.sAllSituations.RemoveAt(i);
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
                    }
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
                }
                try{
List<Sim>
   toRemove=new List<Sim>();
    foreach(var simPosData in stuckPositions){
             if(simPosData.Key==null||
                simPosData.Key.HasBeenDestroyed){
   toRemove.Add(simPosData.Key);
             }
    }
                                 for(int i=0;i<toRemove.Count;i++){
                              stuckPositions.Remove(toRemove[i]);
                                 }
   toRemove.Clear();
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
                             if(currentInteraction!=null&&
                               (currentInteraction is BedSleep||
                                currentInteraction is WorkInRabbitHole)){
goto _Skipped;
                             }else
                             if(sim.Service!=null&&
                               (sim.Service.ServiceType==ServiceType.GrimReaper||
                                sim.Service.ServiceType==ServiceType.Burglar   ||
                                sim.Service.ServiceType==ServiceType.Repoman   ||
                                sim.Service.ServiceType==ServiceType.Police    ||
                                sim.Service.ServiceType==ServiceType.Firefighter||
                                sim.Service.ServiceType==ServiceType.SocialWorkerChildProtection)){
goto _Skipped;
                             }else 
            if(!Objects.IsValid(sim.ObjectId)||
             Simulator.GetProxy(sim.ObjectId)==null||
                                sim.SimDescription==null||
                                sim.SimDescription.CreatedSim!=sim){
                                     new ResetClearSimTask(sim);
goto _DidReset;
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
                                sim.SimDescription.ClearOutfits(OutfitCategories.MartialArts,true);
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
                    }
                    try{
                      if(sim.InteractionQueue!=null&&sim.InteractionQueue.mInteractionList!=null){
               for(int i=sim.InteractionQueue.mInteractionList.Count-1;i>=1;i--){
                      if(sim.InteractionQueue.mInteractionList[i]is BedSleep        )continue;
                      if(sim.InteractionQueue.mInteractionList[i]is WorkInRabbitHole)continue;
                         //
                      if(sim.InteractionQueue.mInteractionList[i].Target!=null){
        var targetObject=sim.InteractionQueue.mInteractionList[i].Target as GameObject;
         if(targetObject!=null){
            targetObject.SimLine?.DoReset();
         }
                      }
                         sim.InteractionQueue.mInteractionList[i].Target?.SetObjectToReset();
                         sim.InteractionQueue.RemoveInteraction(i,false);
                         //
                         //sim.SetObjectToReset();
               }
InteractionInstance 
      currentInteraction;
  if((currentInteraction=sim.InteractionQueue.GetCurrentInteraction())!=null){
//InteractionInstance clone=null;
 if(!(currentInteraction is BedSleep        ||
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
                    }
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
                    }
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
                }
                try{
                        foreach(var washingMachine in Sims3.Gameplay.Queries.GetObjects<WashingMachine>()){
                                    washingMachine.SetObjectToReset();
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
                }
                try{
                        foreach(var fridge in Sims3.Gameplay.Queries.GetObjects<Fridge>()){
                                    fridge.SetObjectToReset();
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
                    }finally{
                    }
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
                    }finally{
                    }
                }
                foreach(var jig in Sims3.Gameplay.Queries.GetObjects<Jig>()){
                    try{
                            jig.SetObjectToReset();//
                            //
                            jig.Destroy();
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
                    }finally{
                    }
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
                }finally{
                }
        }
        //==================================================================================================================
        static void DoHourlyTasks(){
                try{
            foreach(var coffin in Sims3.Gameplay.Queries.GetObjects<BonehildaCoffin>()){
          Sim bonehilda;
          if((bonehilda=coffin.BonehildaSim)!=null){
                     if(coffin.LotCurrent!=null&&
                        coffin.LotCurrent==bonehilda.LotCurrent){
                     }
          }
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
                }finally{
                }
        }
        static void FeedSims(){
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
                    }finally{
                    }
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
                }finally{
                }
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
var interaction=Fridge_Have.Singleton.CreateInstanceWithCallbacks(fridges[ModRandom.Next(0,fridges.Length)],sim,new InteractionPriority(InteractionPriorityLevel.UserDirected),true,true,OnFridge_HaveStarted,OnFridge_HaveCompleted,OnFridge_HaveFailed) as Fridge_Have;
 if(interaction!=null){
    interaction.    Quantity=(Recipe.MealQuantity.Single);
    interaction.ChosenRecipe=(Recipe.NameToRecipeHash["VampireJuiceEP7"]);
                         sim.InteractionQueue.AddInteraction(interaction,false);
 }
                        }else
                        if(sim.Motives.IsHungry()){
var interaction=Fridge_Have.Singleton.CreateInstanceWithCallbacks(fridges[ModRandom.Next(0,fridges.Length)],sim,new InteractionPriority(InteractionPriorityLevel.UserDirected),true,true,OnFridge_HaveStarted,OnFridge_HaveCompleted,OnFridge_HaveFailed) as Fridge_Have;
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
                    }finally{
                    }
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
       static readonly Dictionary<ShowVenue,ShowDetectedData>ShowDetected=new Dictionary<ShowVenue,ShowDetectedData>();
        static void CheckShowVenues(){
                try{
                 foreach(ShowVenue showVenue in Sims3.Gameplay.Queries.GetObjects<ShowVenue>()){
                    try{
                                if(showVenue.ShowInProgress||
                                   showVenue.ShowType!=ShowVenue.ShowTypes.kNoShow){
                                                         if(!ShowDetected.ContainsKey(showVenue)){
                                                             ShowDetected.Add(        showVenue,new ShowDetectedData(SimClock.CurrentTicks));
                                                         }
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
                    }finally{
                    }
                 }
List<KeyValuePair<ShowVenue,ShowDetectedData>>toRemove=new List<KeyValuePair<ShowVenue,ShowDetectedData>>();
                             foreach(var showDetectedData in ShowDetected){
                    try{
                                     if( showDetectedData.Key.HasBeenDestroyed){
                                              toRemove.Add(showDetectedData);
                                     }else
                                     if(!showDetectedData.Key.ShowInProgress&&
                                         showDetectedData.Key.ShowType==ShowVenue.ShowTypes.kNoShow){
           foreach(ISearchLight light in showDetectedData.Key.LotCurrent.GetObjects<ISearchLight>()){
                                light.TurnOff();
        SearchLight searchLight=light as SearchLight;
                 if(searchLight!=null){
                    searchLight.mSMC?.Dispose();
                    searchLight.mSMC=null;
                 }
           }
                                              toRemove.Add(showDetectedData);
                                     }else
                                     if(
                                     SimClock.CurrentTicks-showDetectedData.Value.ShowStartTimeTicks>SimClock.kSimulatorTicksPerSimMinute*300){//  Reset
           foreach(ISearchLight light in showDetectedData.Key.LotCurrent.GetObjects<ISearchLight>()){
                                light.TurnOff();
        SearchLight searchLight=light as SearchLight;
                 if(searchLight!=null){
                    searchLight.mSMC?.Dispose();
                    searchLight.mSMC=null;
                 }
           }
                                         showDetectedData.Key.EndPlayerConcert();
                                              toRemove.Add(showDetectedData);
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
                    }finally{
                    }
                             }
                                for(int i=0;i<toRemove.Count;i++){
                          ShowDetected.Remove(toRemove[i].Key);
                                }
                                              toRemove.Clear();
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
                }finally{
                }
        }
        protected class ShowDetectedData{
                 public ShowDetectedData(long showStartTimeTicks){
                           ShowStartTimeTicks=showStartTimeTicks;
                 }
            public        readonly long                          ShowStartTimeTicks;
        }
        //==================================================================================================================
        static void ResetPortalsAndRouting(){
                try{
                Sim.kAutonomyThoughtBalloonPercentageChance=100f;
                Sim.kDistanceForSocials=15f;
                Sim.kMaxDistanceForSocials=75f;
                Sim.kBabyToddlerLikingThresholdToAllowInteractionsWithStrangers=-101;
            //------------------------------------------------------
                ElevatorInterior.kElevatorToElevatorPortalCost=20f;
                Ladder.kPortalCost=22f;
                Door.kMinimumTimeBeforeClosingDoor=30f;
                Door.kDefaultPortalCost=2f;
            //------------------------------------------------------
                                SimRoutingComponent.kNPCSubwayUseChance=0.5f;
            //------------------------------------------------------
            GameObject.kKleptoRespawnTimeDays=1;
            //------------------------------------------------------
            GameObject.kAutonomyMultiplierForObjectSelectionWhenSomeSimIsRouting=0.05f;
                                //------------------------------------------------------
                                Autonomy.kEnergyThresholdBelowWhichTheyWantToSleepWhenSleepyDuringTheirBedTime=0;
                                //------------------------------------------------------
                                Autonomy.kHoursAfterWhichBubbledUpScoreIsMax=.25f;
                                //------------------------------------------------------
                                Autonomy.kAllowEvenIfNotAllowedInRoomAutonomousMultiplier=0f;
                                Autonomy.kAutonomyDelayNormal           =0;
                                Autonomy.kAutonomyDelayWhileMounted     =0;
                                Autonomy.kAutonomyDelayDuringSocializing=0;
                                SimRoutingComponent.kDefaultStandAndWaitDuration=1f;
                                SimRoutingComponent.kMinimumPostPushStandAndWaitDuration=0f;
                                SimRoutingComponent.kMaximumPostPushStandAndWaitDuration=2f;
                                SimRoutingComponent.kTotalSimMinutesToWaitForSimsToBePushed=1f;
                                //------------------------------------------------------
                                SimRoutingComponent.kAvoidanceReplanCheckFrequencyMin=6;
                                SimRoutingComponent.kAvoidanceReplanCheckFrequencyMax=9;
                                SimRoutingComponent.kAvoidanceReplanCheckOffsetMin=1;
                                SimRoutingComponent.kAvoidanceReplanCheckOffsetMax=3;
                                //------------------------------------------------------
                                SimRoutingComponent.kPushHorsesAwayDistanceMin=14.0f;
                                SimRoutingComponent.kPushHorsesAwayDistanceMax=20.0f;
                                SimRoutingComponent. kPushFoalsAwayDistanceMin=14.0f;
                                SimRoutingComponent. kPushFoalsAwayDistanceMax=20.0f;
                                SimRoutingComponent.  kPushDogsAwayDistanceMin= 6.0f;
                                SimRoutingComponent.  kPushDogsAwayDistanceMax=12.0f;
                                SimRoutingComponent.kPushSimsAwayDistanceFromFootprintMin=2.50f;
                                SimRoutingComponent.  kPushSimsAwayDistanceMin= 2.4f;
                                SimRoutingComponent.  kPushSimsAwayDistanceMin=10.0f;
                                //------------------------------------------------------
                                InteractionQueue.kMaxMinutesRemainingNotToAutosolve=30;
                                SimQueue.kMinimumRadialDistanceFromDoor= 2.0f;
                                SimQueue.kMaximumRadialDistanceFromDoor=10.0f;
                                SimQueue.kMinimumRadialDistanceFromSim= 4.0f;
                                SimQueue.kMaximumRadialDistanceFromSim=10.0f;
                                SimQueue.kMinimumRadialDistanceFromObject= 4.8f;
                                SimQueue.kMaximumRadialDistanceFromObject=12.0f;
                                SimQueue.kCheckPeriodInMinutes=0.5f;
                                //------------------------------------------------------
                                InteractionInstance.kNumMinToWaitOnPreSync=60;
                                InteractionInstance.kNumMinToWaitOnSyncStart=90;
                                InteractionInstance.kNumMinToWaitOnSync=10;
                                //------------------------------------------------------
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
                }finally{
                }
                try{
List<Sim>
   toRemove=new List<Sim>();
    foreach(var simPosData in stuckPositions){
             if(simPosData.Key==null||
                simPosData.Key.HasBeenDestroyed||
               !simPosData.Key.InWorld||
                simPosData.Key.Position!=simPosData.Value){
   toRemove.Add(simPosData.Key);
             }
    }
                                 for(int i=0;i<toRemove.Count;i++){
                              stuckPositions.Remove(toRemove[i]);
                                 }
   toRemove.Clear();
                   foreach(Sim sim in Sims3.Gameplay.Queries.GetObjects<Sim>()){
                               sim.PlayRouteFailFrequency=Sim.RouteFailFrequency.NeverPlayRouteFail;
           if(!Objects.IsValid(sim.ObjectId)||
            Simulator.GetProxy(sim.ObjectId)==null||
                               sim.SimDescription==null||
                               sim.SimDescription.CreatedSim!=sim){
         new ResetClearSimTask(sim);
           }
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
                }finally{
                }
                try{
                   foreach(ElevatorDoors elevator in Sims3.Gameplay.Queries.GetObjects<ElevatorDoors>()){
                           ElevatorInterior.ElevatorPortalComponent 
                                                    portal=
                                         elevator.InteriorObj.ElevatorPortal as 
                           ElevatorInterior.ElevatorPortalComponent;
                                                 if(portal!=null){
//  Medium reset sims: reset and put in the same lot
foreach(SimDescription sim in new List<SimDescription>(
                                                    portal.mAssignedSims.Keys)){
                    if(sim.CreatedSim!=null){
                                                                                               StuckSimData stuckSim;
                                                         if(!StuckSims.TryGetValue(sim.SimDescriptionId,out stuckSim)){
                                                                                                            stuckSim=new StuckSimData();
                                                             StuckSims.Add(        sim.SimDescriptionId,    stuckSim);
                                                         }
                                                                                                        if(!stuckSim.Resetting){
                                                                                                            stuckSim.Detections++;
                                                                         Vector3 destination=Vector3.Invalid;
                                                               if(sim.CreatedSim.RoutingComponent!=null){
                                                                  sim.CreatedSim.RoutingComponent.GetDestination(out destination);
                                                               }
                    if(  stuckSim.resetTask==null)
                         stuckSim.resetTask=new ResetStuckSimTask(sim.CreatedSim,destination,"Elevator");
                    else stuckSim.resetTask.Renew();
                                                                                                        }
                    }
}
                                                    portal.FreeAllRoutingLanes();
                                                 }
                                         //
                                      if(elevator.ActorsUsingMe!=null&&
                                         elevator.ActorsUsingMe.Count>0)
                                         elevator.SetObjectToReset();
                   }
                   foreach(Door door in Sims3.Gameplay.Queries.GetObjects<Door>()){
                      Door.DoorPortalComponent 
                               portal=
                                door.PortalComponent as 
                      Door.DoorPortalComponent;
                            if(portal!=null){
//  Soft reset sims: reset and don't change position
foreach(SimDescription sim in new List<SimDescription>(
                                door.ActorsUsingMeAsSimDescriptions)){
                    if(sim.CreatedSim!=null){
                    }
}
                             if(door.ActorsUsingMe!=null&&
                                door.ActorsUsingMe.Count>0)
                               portal.FreeAllRoutingLanes();
                            }
                                //
                             if(door.ActorsUsingMe!=null&&
                                door.ActorsUsingMe.Count>0)
                                door.SetObjectToReset();
                   }
                   foreach(StaircaseSpiral staircaseSpiral in Sims3.Gameplay.Queries.GetObjects<StaircaseSpiral>()){
           StaircaseSpiral.StaircaseSpiralPortalComponent
                                          portal=
                                           staircaseSpiral.PortalComponent as 
           StaircaseSpiral.StaircaseSpiralPortalComponent;
                                       if(portal!=null){
//  Soft reset sims: reset and don't change position
foreach(SimDescription sim in new List<SimDescription>(
                                           staircaseSpiral.ActorsUsingMeAsSimDescriptions)){
                    if(sim.CreatedSim!=null){
                    }
}
                                          portal.FreeAllRoutingLanes();
                                       }
                                           //
                                        if(staircaseSpiral.ActorsUsingMe!=null&&
                                           staircaseSpiral.ActorsUsingMe.Count>0)
                                           staircaseSpiral.SetObjectToReset();
                   }
                   foreach(Ladder ladder in Sims3.Gameplay.Queries.GetObjects<Ladder>()){
                    Ladder.LadderPortalComponent
                                 portal=
                                  ladder.PortalComponent as 
                    Ladder.LadderPortalComponent;
                              if(portal!=null){
//  Soft reset sims: reset and don't change position
foreach(SimDescription sim in new List<SimDescription>(
                                  ladder.ActorsUsingMeAsSimDescriptions)){
                    if(sim.CreatedSim!=null){
                    }
}
                                 portal.FreeAllRoutingLanes();
                              }
                                  //
                               if(ladder.ActorsUsingMe!=null&&
                                  ladder.ActorsUsingMe.Count>0)
                                  ladder.SetObjectToReset();
                   }
                   foreach(Stairs stairs in Sims3.Gameplay.Queries.GetObjects<Stairs>()){
                    Stairs.StairsPortalComponent
                                 portal=
                                  stairs.PortalComponent as 
                    Stairs.StairsPortalComponent;
                              if(portal!=null){
//  Soft reset sims: reset and don't change position
foreach(SimDescription sim in new List<SimDescription>(
                                  stairs.ActorsUsingMeAsSimDescriptions)){
                    if(sim.CreatedSim!=null){
                    }
}
                                 portal.FreeAllRoutingLanes();
                              }
                                  //
                               if(stairs.ActorsUsingMe!=null&&
                                  stairs.ActorsUsingMe.Count>0)
                                  stairs.SetObjectToReset();
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
                }finally{
                }
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
                }finally{
                }
        }
           public class ResetClearSimTask:AlarmTask{
                               Sim                           mSim;
                 public ResetClearSimTask(Sim sim):base(1,TimeUnit.Seconds,null){
                                         mSim=sim;
                 }
                protected override void OnPerform(){
                                         mSim=Perform(mSim,true);
                }
                 public static Sim Perform(Sim sim,bool fadeOut){
                                            if(sim==null)return null;
                try{
                        SimDescription simDesc=sim.SimDescription;
                         if(Simulator.GetProxy(sim.ObjectId)==null){
                                    if(simDesc!=null){
                                               sim.Destroy();
                                    }                  
return null;
                         }
                                    if(simDesc==null){
                                               sim.mSimDescription=new SimDescription();
                                               sim.Destroy();                    
return null;
                                    }
                                            if(sim.LotHome!=null){
                                       simDesc.IsZombie=false;
                                    if(simDesc.CreatedSim!=sim){
                                                           sim.Destroy();
                                       simDesc.CreatedSim=null;                        
return null;
                                    }else{                        
                                        Bed     myBed    =null;
                                        BedData myBedData=null;
                                foreach(Bed bed in sim.LotHome.GetObjects<Bed>()){
                                                myBedData=bed.GetPartOwnedBy(sim);
                                             if(myBedData!=null){
                                                myBed=bed;
                                                break;
                                             }
                                }
                                      ResetPosture(sim);
                                    if(simDesc.TraitManager==null){
                                       simDesc.mTraitManager=new TraitManager();
                                    }
                    try{
                                       simDesc.Fixup();
                   CleanupBrokenSkills(simDesc);
                           ResetCareer(simDesc);
                                       simDesc.ClearSpecialFlags();
                                    if(simDesc.Pregnancy==null){
                        try{
                                    if(simDesc.mMaternityOutfits==null){
                                       simDesc.mMaternityOutfits=new OutfitCategoryMap();
                                    }
                                       simDesc.SetPregnancy(0,false);
                                       simDesc.ClearMaternityOutfits();
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
                        }finally{
                        }
                                    }
                                            if(sim.CurrentCommodityInteractionMap==null){
                        try{
                   LotManager.PlaceObjectOnLot(sim,sim.ObjectId);
                                            if(sim.CurrentCommodityInteractionMap==null){
                                               sim.ChangeCommodityInteractionMap(sim.LotHome.Map);
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
                        }finally{
                        }
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
                    }finally{
                    }
                               ResetSituations(sim);
                                  //
                                  CleanupSlots(sim);
                                ResetInventory(sim);
                                  //
                                                     if(fadeOut){
                                  ResetRouting(sim);
                                  //
                 bool active=(Sim.ActiveActor==sim);
               using(CreationProtection protection=new CreationProtection(simDesc,sim,false,true,false)){
                                                                                  sim.Destroy();
Sleep();
                                               sim=InstantiateAtHome(simDesc,null);
               }
                                            if(sim!=null){
                   if(active){
                    try{
                          List<Sim>allSimsInHousehold=new List<Sim>();
               List<SimDescription>allSimDescriptions=sim.Household?.AllSimDescriptions;
                                if(allSimDescriptions!=null){
         foreach(var memberDesc in allSimDescriptions){
                  if(memberDesc.CreatedSim==null)continue;
                                   allSimsInHousehold.Add(memberDesc.CreatedSim);
         }
                                }
             foreach(Sim member in allSimsInHousehold){
                      if(member.CareerManager==null)continue;
   Occupation occupation=member.CareerManager.Occupation;
           if(occupation==null)continue;
              occupation.FormerBoss=null;
             }
 using(HouseholdStore store=new HouseholdStore(sim.Household,true)){
                        PlumbBob.DoSelectActor(sim,true);
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
                    }finally{
                    }
                   }
                                            if((myBed    !=null)&&
                                               (myBedData!=null)){
                                           if(!(myBed is BedMultiPart)||
                                               (myBed is BedMultiPart&&((sim.Partner!=null)&&(sim.Partner.CreatedSim!=null)))){
                                                myBed.ClaimOwnership(sim,myBedData);
                                           }else{
                            HandleDoubleBed(sim,myBed,myBedData);
                                           }
                                            }
                                            }
                                            if(sim.Inventory==null){
                                               sim.AddComponent<InventoryComponent>(new object[0x0]);
                                            }  
                                                     }else{     
                                            if(sim.Inventory==null){
                                               sim.AddComponent<InventoryComponent>(new object[0x0]);
                                            }                      
                                                                                                   StuckSimData stuckSim;
                                                         if(!StuckSims.TryGetValue(simDesc.SimDescriptionId,out stuckSim)){
                                                                                                                stuckSim=new StuckSimData();
                                                             StuckSims.Add(        simDesc.SimDescriptionId,    stuckSim);
                                                         }
                                                                                                                stuckSim.Detections++;
                                                                     ResetStuckSim(sim,Vector3.Invalid,"Deep",true);
                                  ResetRouting(sim);
                                               sim.SetObjectToReset();  
                                                     }
                                //  [Nraas:]This is necessary to clear certain types of interactions
                                // (it is also called in SetObjectToReset(), though doesn't always work there)
                                            if(sim.InteractionQueue!=null){
                                               sim.InteractionQueue.OnReset();
                                            }
                   ResetSkillModifiers(simDesc);
                                     ResetRole(sim);
                                    if(simDesc.IsEnrolledInBoardingSchool()){
                                       simDesc.BoardingSchool.OnRemovedFromSchool();
                                    }
                        MiniSimDescription miniSim=MiniSimDescription.Find(simDesc.SimDescriptionId);
                                        if(miniSim!=null){
                                           miniSim.Instantiated=true;
                                        }
                               UpdateInterface(sim);
return sim;
                                    }
                                            }
                                            else 
                                            if(simDesc.Service is Butler){
                                                                                                   StuckSimData stuckSim;
                                                         if(!StuckSims.TryGetValue(simDesc.SimDescriptionId,out stuckSim)){
                                                                                                                stuckSim=new StuckSimData();
                                                             StuckSims.Add(        simDesc.SimDescriptionId,    stuckSim);
                                                         }
                                                                                                                stuckSim.Detections++;
                                                                     ResetStuckSim(sim,Vector3.Invalid,"Deep",true);
                  sim.Motives?.RecreateMotives(sim);
                                               sim.SetObjectToReset();  
return sim;
                                            }else 
                                            if(simDesc.IsImaginaryFriend){
                                 OccultImaginaryFriend friend;
  if(OccultImaginaryFriend.TryGetOccultFromSim(sim,out friend)){
                                 if(Simulator.GetProxy(friend.mDollId)!=null){
                                                       friend.TurnBackIntoDoll(OccultImaginaryFriend.Destination.Owner);
return null;
                                 }
  }
                                            }else 
                                            if(simDesc.IsBonehilda){
return sim;
                                            }
                                                     if(fadeOut){
                                               sim.Destroy();
                                                     }
return null;
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
                return sim;
                }finally{
                }
                 }
                //  [NRaas:]EA is genius... Fix for genius coding in Bed.RelinquishOwnershipOfBed
                public static void HandleDoubleBed(Sim newOwner,Bed bed,BedData entryPart){
                                                    if(newOwner!=null&&
                                                                    bed!=null&&
                                                                                entryPart!=null){
                        //  From BedMultiPart:ClaimOwnership
                                            bool flag=(newOwner.Service!=null)&&
                                                      (newOwner.Service.ServiceType==ServiceType.Butler);
                                              if(flag||
                                                     ((newOwner.Household!=null)&&
                                                     ((newOwner.Household.LotHome==bed.LotCurrent)||
                                                                    bed.IsTent))){
                                                                    bed.RelinquishOwnershipOfBeds(newOwner,false);
                                                                                entryPart.Owner=newOwner;
                                             if(!flag){
                                                       newOwner.Household.HouseholdSimsChanged+=new Household.HouseholdSimsChangedCallback(bed.HouseholdSimsChanged);
                                             }
                                              if(flag){
                                                  BedData otherPart=bed.PartComponent.GetOtherPart(entryPart) as BedData;
                                                      if((otherPart!=null)&&
                                                         (otherPart.Owner==null)){
                                                          otherPart.Owner=newOwner;
                                                      }
                                              }
                                                       newOwner.Bed=bed;
                                              }
                                                    }
                }
               public static void ResetPosture(Sim sim){
                try{
                                                if(sim.Posture!=null){
                                                        int count=0;
                                   Posture posture=sim.Posture;
                                    while((posture!=null)&&(count<5)){
                    try{
                                           posture.OnReset(sim);
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
                    }finally{
                    }
                                                            count++;
                                           posture=posture.PreviousPosture;
                                    }
                                        if(posture!=null){
                                           posture.PreviousPosture=null;
                                        }
                    try{
                                                   sim.Posture=null;
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
                                                   sim.mPosture=null;
                    }
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
                }finally{
                }
            }
            public static void ResetInventory(Sim sim){
                try{
                                              if((sim.Inventory!=null)&&
               (ParentsLeavingTownSituation.sAdultsInventories!=null)){
                                                                                                List<InventoryItem>inventory;
             if(ParentsLeavingTownSituation.sAdultsInventories.TryGetValue(sim.SimDescription.SimDescriptionId,out inventory)){
                         RestoreInventoryFromList(sim.Inventory,inventory,false);
                ParentsLeavingTownSituation.sAdultsInventories.Remove(sim.SimDescription.SimDescriptionId);
             }
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
                }finally{
                }
            }
            public static void RestoreInventoryFromList(Inventory ths,List<InventoryItem>items,bool deleteExisting){
                                                                                                 if(deleteExisting){
                                                                  ths.DestroyItems();
                                                                                                 }
                                                                                      if(items!=null){
                                                           foreach(InventoryItem item in items){
                                                              if(!ths.TryToAdd(item.Object,false)){
                                                                               item.Object.Destroy();
                                                              }
                                                           }
                                                                                      }
            }
            public static List<T>InventoryQuickFind<T>(Inventory ths)where T:GameObject{
                       List<T>retList=new List<T>();
                                                              if(ths!=null){
                                 foreach(InventoryStack stack in ths.mItems.Values){
                                                     if(stack.List.Count==0)continue;
                                                     if(stack.List[0].Object is T){
                          foreach(InventoryItem item in stack.List){
                                        T local=item.Object as T;
                                       if(local!=null){
                              retList.Add(local);
                                       }
                          }
                                                     }
                                 }
                                                              }
                       return retList;
            }
               public static void CleanupSlots(Sim sim){
                try{
                              foreach(Slot slot in sim.GetContainmentSlots()){
                                   IGameObject obj=sim.GetContainedObject(slot);
                                            if(obj==null)continue;
                    try{
                                               obj.UnParent();
                                               obj.RemoveFromUseList(sim);
                                          if(!(obj is Sim)){
                                               if((sim.Inventory==null)||(!sim.Inventory.TryToAdd(obj,false))){
                                               obj.Destroy();
                                               }
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
                    }finally{
                    }
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
                }finally{
                }
               }
            public static void CleanupBrokenSkills(SimDescription sim){
                try{
                                                               if(sim.SkillManager==null)return;
                                   List<ulong>remove=new List<ulong>();
                        foreach(KeyValuePair<ulong,Skill>value in sim.SkillManager.mValues){
                                             Skill skill=value.Value;
                                                if(skill==null){
                                              remove.Add(value.Key);
                                                }else{
     Skill staticSkill=SkillManager.GetStaticSkill(skill.Guid);
        if(staticSkill!=null){
                                                if(skill.NonPersistableData==null){
                                                   skill.mNonPersistableData=staticSkill.mNonPersistableData;
                                                if(skill.NonPersistableData!=null){
                                                }else{
                                              remove.Add(value.Key);
                                                }
                                                }
                                                if(skill.SkillLevel>staticSkill.MaxSkillLevel){
                                                   skill.SkillLevel=staticSkill.MaxSkillLevel;
                                                }
        }else{
                                              remove.Add(value.Key);
        }
                                                }
                        }
                        foreach(ulong guid in remove){
                                                                  sim.SkillManager.mValues.Remove(guid);
                        }
                                          NectarSkill nectarSkill=sim.SkillManager.GetSkill<NectarSkill>(SkillNames.Nectar);
                                                   if(nectarSkill!=null){
                                                   if(nectarSkill.mSimIDsServed==null){
                                                      nectarSkill.mSimIDsServed=new List<ulong>();
                                                   }
                                                   if(nectarSkill.mHashesMade==null){
                                                      nectarSkill.mHashesMade=new List<uint>();
                                                   }
                                                   }
                                               RockBand bandSkill=sim.SkillManager.GetSkill<RockBand>(SkillNames.RockBand);
                                                     if(bandSkill!=null){
                                                     if(bandSkill.mBandGigsStats!=null){
                                                        bandSkill.mBandGigsStats.Remove(null);
                                                     }
                                                     }
                                BroomRidingSkill broomRidingSkill=sim.SkillManager.GetSkill<BroomRidingSkill>(SkillNames.BroomRiding);
                                              if(broomRidingSkill!=null){
                                              if(broomRidingSkill.mLotsVisited==null){
                                                 broomRidingSkill.mLotsVisited=new List<Lot>();
                                              }
                                       for(int i=broomRidingSkill.mLotsVisited.Count-1;i>=0;i--){
                                              if(broomRidingSkill.mLotsVisited[i]==null){
                                                 broomRidingSkill.mLotsVisited.RemoveAt(i);
                                              }
                                       }
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
                }finally{
                }
            }
            public static void ResetSkillModifiers(SimDescription sim){
                try{
                                                               if(sim.SkillManager==null)return;
                                      CorrectOverallSkillModifier(sim);
                                                                  sim.SkillManager.mSkillModifiers=new Dictionary<SkillNames,float>();
                                        TraitManager traitManager=sim.TraitManager;
                                                  if(traitManager!=null){
                              foreach(Trait trait in traitManager.List){
                                                     traitManager.AddTraitSkillGainModifiers(sim,trait.Guid);
                              }
                                                  }
Dictionary<GameObject,bool>inventory=new Dictionary<GameObject,bool>();
                                                              if((sim.CreatedSim!=null)&&
                                                                 (sim.CreatedSim.Inventory!=null)){
         foreach(GameObject obj in InventoryQuickFind<GameObject>(sim.CreatedSim.Inventory)){
              inventory.Add(obj,true);
         }
                                                              }
                                                           ulong eventId=(ulong)EventTypeId.kSkillLearnedSkill;
                                        Dictionary<ulong,List<EventListener>>events;
                if(!EventTracker.Instance.mListeners.TryGetValue(eventId,out events)){
                                                                             events=null;
                }else{
                    EventTracker.Instance.mListeners.Remove(     eventId);
                }
                                                              if((sim.CreatedSim!=null)&&
                                                                (!sim.CreatedSim.HasBeenDestroyed)){
                                           foreach(Skill skill in sim.SkillManager.List){
                  bool isChangingWorlds=GameStates.sIsChangingWorlds;
                        //  Workaround for issue in IsIdTravelling
                                        GameStates.sIsChangingWorlds=false;
                    try{
                                                         skill.OnSkillAddition(true);
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
                    }finally{
                                        GameStates.sIsChangingWorlds=isChangingWorlds;
                    }
                                           }
                                                              }
                                                                          if(events!=null){
                    EventTracker.Instance.mListeners.Add(        eventId,    events);
                                                                          }
                                                              if((sim.CreatedSim!=null)&&
                                                                 (sim.CreatedSim.Inventory!=null)){
         foreach(GameObject obj in InventoryQuickFind<GameObject>(sim.CreatedSim.Inventory)){
                        if(inventory.ContainsKey(obj))continue;
                        try{
                            sim.CreatedSim.Inventory.RemoveByForce(obj);
                            obj.Destroy(); // Do not use FadeOut(), it hangs the game
                        }catch{}
         }
                                                              }
                                                               if(sim.OccultManager!=null){
                                            OccultVampire vampire=sim.OccultManager.GetOccultType(Sims3.UI.Hud.OccultTypes.Vampire) as OccultVampire;
                                                      if((vampire!=null)&&
                                                         (vampire.AppliedNightBenefits)){
                    try{
                                                              if((sim.CreatedSim!=null)&&
                                                                (!sim.CreatedSim.HasBeenDestroyed)){
                                                          vampire.SunsetCallback();
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
                    }finally{
                    }
                                                      }
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
                }finally{
                }
            }
                public static void CorrectOverallSkillModifier(SimDescription sim){
                                                                              sim.SkillManager.mOverallModifier=sim.SkillManager.mMoodModifier;
                                                                          if((sim.CreatedSim!=null)&&
                                                                             (sim.CreatedSim.BuffManager!=null)){
                                                                           if(sim.CreatedSim.BuffManager.HasElement(BuffNames.IncreasedUnderstanding)){
                                                                              sim.SkillManager.mOverallModifier+=ImprovedProtestSituation.kSkillIncreasePctRewardFromCause;
                                                                           }
                                                                          }
                }
            public static void ResetCareer(SimDescription sim){
                try{
                        Sims3.Gameplay.Careers.Career career=sim.Occupation as Sims3.Gameplay.Careers.Career;
                                                   if(career!=null){
                                                   if(career.HighestCareerLevelAchieved==null){
                                                  if((career.mHighestLevelAchievedBranchName!=null)&&(career.mHighestLevelAchievedVal!=-1)){
                                                      career.HighestCareerLevelAchieved=career.SharedData.CareerLevels[career.mHighestLevelAchievedBranchName][career.mHighestLevelAchievedVal];
                                                  }else{
                                                      career.HighestCareerLevelAchieved=career.CurLevel;
                                                  }
                                                   }
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
                }finally{
                }
            }
            public static void ResetRole(Sim sim){
                try{
                                          if(sim==null){                            return;}
                                          if(sim.SimDescription.AssignedRole==null){return;}
                                   Role role=sim.SimDescription.AssignedRole;
                                     if(role.IsActive){
                    try{
                                        role.mIsActive=false;
                                        role.StartRole();
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
                    }finally{
                    }              
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
                }finally{
                }
            }
            public static bool CleanupOpportunities(SimDescription sim,bool clean){
                try{
                                                                if(sim.OpportunityHistory==null){                      return false;}
                                                                if(sim.OpportunityHistory.mCurrentOpportunities==null){return false;}
                                        OpportunityManager manager=null;
                                                                if(sim.CreatedSim!=null){
                                                           manager=sim.CreatedSim.OpportunityManager;
                                                                }
                                                     for(int i=0;i<sim.OpportunityHistory.mCurrentOpportunities.Length;i++){
                     OpportunityHistory.OpportunityExportInfo info=sim.OpportunityHistory.mCurrentOpportunities[i];
                                                           if(info==null){
                                                        if(manager!=null){
                                                              info=new OpportunityHistory.OpportunityExportInfo();
                                                                   sim.OpportunityHistory.mCurrentOpportunities[i]=info;
                                                        }
                                                           }else if(clean){
                                                        if(manager==null){
                                                                   sim.OpportunityHistory.mCurrentOpportunities[i]=null;
                                                        }
                                                           }
                                                           if(info==null)continue;
                                                           if(info.ListenerStates==null){
                                                              info.ListenerStates=new EventListenerExportInfo[3];
                                                           }
                                                                          for(int index=0;index<3;index++){
                                                           if(info.ListenerStates[index]==null){
                                                              info.ListenerStates[index]=new EventListenerExportInfo();
                                                           }
                                                                          }
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
                }finally{
                }
                return true;
            }
            public static void ResetSituations(Sim sim){
                try{
                                               if((sim.Autonomy!=null)&&
                                                  (sim.Autonomy.SituationComponent!=null)&&
                                                  (sim.Autonomy.SituationComponent.Situations!=null)){
     List<Situation>situations=new List<Situation>(sim.Autonomy.SituationComponent.Situations);
  foreach(Situation situation in situations){
                    try{
                FilmCareerSituation filmSituation=situation as FilmCareerSituation;
                                 if(filmSituation!=null){
  List<Sim>jobTargets=new List<Sim>();
              foreach(Sim target in filmSituation.mJobTargets){
                       if(target==null)continue;
           jobTargets.Add(target);
              }
                                    filmSituation.mJobTargets=jobTargets;
                                 }
                    situation.Exit();
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
                    }finally{
                    }
  }
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
                }finally{
                }
            }
            public static void ResetRouting(Sim sim,bool deep=true){
                try{
                                             if(sim==null)return;
                  SimRoutingComponent component=sim.SimRoutingComponent;
                                   if(component!=null){
                                   if(component.LockedDoorsDuringPlan!=null&&
                                      component.LockedDoorsDuringPlan.Count>0){
                 foreach(Door door in component.LockedDoorsDuringPlan){
                           if(door==null){continue;}                        
PortalComponent portalComponent=
                              door.PortalComponent;
             if(portalComponent!=null){
                portalComponent.FreeAllRoutingLanes();
             }
                              door.SetObjectToReset();
                 }
                                   }
                                      component.LockedDoorsDuringPlan=new List<Door>();
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
                }finally{
                }
            }
            public static void UpdateInterface(Sim sim){
                try{
                                               if((sim==Sim.ActiveActor)&&(Sims3.UI.Hud.HudController.Instance!=null)){
                                       Sims3.Gameplay.UI.HudModel hudModel=Sims3.UI.Hud.HudController.Instance.mHudModel as Sims3.Gameplay.UI.HudModel;
                                                               if(hudModel!=null){
                                                                  hudModel.OnInteractionQueueDirtied();
                                                               }
                                               }
                                               if((sim.Household!=null)&&
                                                  (sim.Household.HouseholdSimsChanged!=null)){
                                                   sim.Household.HouseholdSimsChanged(Sims3.Gameplay.CAS.HouseholdEvent.kSimAdded,sim,null);
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
                }finally{
                }
            }
        public class CreationProtection:IDisposable{
static ChangingWorldsSuppression sChangingWorldsSuppression=new ChangingWorldsSuppression();
              public CreationProtection(SimDescription sim,Sim createdSim,bool performLoadFixup,bool performSelectable,bool performUnselectable){
                try{
                                                  mSim=sim;
                                  RemoveFreeStuffAlarm(sim);
                    //  Stops an issue in "GrantFutureObjects" regarding the use of sIsChangingWorlds=true
                                         mWasFutureSim=sim.TraitManager.   HasElement(TraitNames.FutureSim);
                                                       sim.TraitManager.RemoveElement(TraitNames.FutureSim);
                                                    if(sim!=null&&
                                                      !sim.IsNeverSelectable&&
                                                       sim.Household==Household.ActiveHousehold){
                              CleanupBrokenSkills(mSim);
                                                    }
                                                                     if(OpportunityTrackerModel.gSingleton!=null){
                                                  mOpportunitiesChanged=OpportunityTrackerModel.gSingleton.OpportunitiesChanged;
                                                                        OpportunityTrackerModel.gSingleton.OpportunitiesChanged=null;
                                                                     }
                                               if(mSim.TraitChipManager!=null){
                                                  mChips=mSim.TraitChipManager.GetAllTraitChips();
                                                  mSim.TraitChipManager.mTraitChipSlots=new TraitChip[7];
                                                  mSim.TraitChipManager.mValues.Clear();
                                               }
                                                            if(createdSim!=null){
                                                            if(createdSim.BuffManager!=null){
                                            mBuffs=new List<BuffInstance>();
                                  foreach(BuffInstance buff in createdSim.BuffManager.List){
                                                  if(!(buff is BuffCustomizable.BuffInstanceCustomizable)){
                                            mBuffs.Add(buff);
                                                  }
                                  }
                                                            }
                                                            if(createdSim.Motives    !=null){
                                                 Motive motive=createdSim.Motives.GetMotive(CommodityKind.AcademicPerformance);
                                                     if(motive!=null){
                                   mAcademicPerformance=motive.Value;
                                                     }
                                                        motive=createdSim.Motives.GetMotive(CommodityKind.UniversityStudy    );
                                                     if(motive!=null){
                                   mUniversityStudy    =motive.Value;
                                                     }
                                                            }
                                                            if(createdSim.Inventory  !=null){
                                                    mInventory=createdSim.Inventory.DestroyInventoryAndStoreInList();
                                                            }
                                    mDreamStore=new DreamStore(createdSim,false,false);
                                              mReservedVehicle=createdSim.GetReservedVehicle();
                                                               createdSim.ReservedVehicle=null;
                                                            }   
SafeStore.Flag flags=(SafeStore.Flag.None);
                                                                                                  if(performSelectable){
               flags|=SafeStore.Flag.Selectable;
                                                                                                  }
                                                                            if(performLoadFixup){
               flags|=SafeStore.Flag.LoadFixup;
                                                                            }
                                                                                                                         if(performUnselectable){
               flags|=SafeStore.Flag.Unselectable;
                                                                                                                         }
                         mSafeStore=new SafeStore(mSim,flags);
                    //=========================================================================
                    //  Stops the startup errors when the Imaginary Friend is broken
                                   mDoll=GetDollForSim(sim);
                                if(mDoll!=null){
                                   mDoll.mOwner=null;
                                }
                                            mGenealogy=sim.mGenealogy;
                             mRelations=StoreRelations(sim);
                    //  Stops all event processing during the creation process
                    EventTracker.sCurrentlyUpdatingDreamsAndPromisesManagers=true;
                    //  Stops the interface from updating during OnCreation
                                                    if(sim.Household!=null){
                                      mChangedCallback=sim.Household.HouseholdSimsChanged;
                                     mChangedHousehold=sim.Household;
                                                       sim.Household.HouseholdSimsChanged=null;
                                                    }
                    sChangingWorldsSuppression.Push();
                    //  Stops SetGeneologyRelationshipBits()
                                                       sim.mGenealogy=new Genealogy(sim);
                    //=========================================================================
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
                }finally{
                }
              }
                                   SimDescription mSim;
Household.HouseholdSimsChangedCallback 
                                      mChangedCallback=null;
                           Household mChangedHousehold=null;
Dictionary<SimDescription,Relationship>
                             mRelations=null;
                                  Genealogy mGenealogy = null;
                     ImaginaryDoll mDoll=null;
                                 List<InventoryItem>mInventory=null;
                         DreamStore mDreamStore=null;
SafeStore mSafeStore=null;
                                      Vehicle mReservedVehicle=null;
                     OpportunitiesChangedCallback mOpportunitiesChanged=null;
                          List<BuffInstance>mBuffs;
                                       TraitChip[]mChips;
                                    bool mWasFutureSim=false;
                             float mAcademicPerformance=-101f;
                             float mUniversityStudy    =-101f;
            protected static ImaginaryDoll GetDollForSim(SimDescription owner){
                                                 ulong simDescriptionId=owner.SimDescriptionId;
                                                   foreach(ImaginaryDoll doll in Sims3.Gameplay.Queries.GetObjects<ImaginaryDoll>()){
                                                    if(simDescriptionId==doll.GetOwnerSimDescriptionId()){
                                                                  return doll;
                                                    }
                                                   }
            return null;}
            public void Dispose(){
                        Dispose(true, true);
            }
            public void Dispose(bool postLoad,bool isReset){
                try{
                                      if(mWasFutureSim){
                                                  mSim.TraitManager.AddHiddenElement(TraitNames.FutureSim);
                                      }
                                               if(mSim.CreatedSim!=null){
                          BuffManager buffManager=mSim.CreatedSim.BuffManager;
                                  if((buffManager!=null)&&
                                           (mBuffs!=null)){
               foreach(BuffInstance buff in mBuffs){
                                      buffManager.AddBuff(
                                    buff.Guid          , 
                                    buff.mEffectValue  , 
                                    buff.mTimeoutCount , 
                                    buff.mTimeoutPaused, 
                                    buff.mAxisEffected , 
                                    buff.mBuffOrigin   ,false);
               }
                                  }
                                                if((mInventory!=null)&&
                                                 (mSim.CreatedSim.Inventory!=null)){
                         RestoreInventoryFromList(mSim.CreatedSim.Inventory,mInventory,true);
                                                }
                                 if(mDreamStore!=null){
                                    mDreamStore.Restore(mSim.CreatedSim);
                                 }
       if(mSafeStore!=null){
          mSafeStore.Dispose();
       }
                                               if(mSim.DeathStyle!=SimDescription.DeathType.None){
                   Urnstone stone=FindGhostsGrave(mSim);
                         if(stone!=null){
                            stone.GhostSetup(mSim.CreatedSim, true);
                         }
                                               }
                                                  mSim.CreatedSim.ReservedVehicle=mReservedVehicle;
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
                }finally{
                }
                try{
                //  Must be after the opportunities are restored
                                              if((mOpportunitiesChanged!=null)&&
                                                                       (OpportunityTrackerModel.gSingleton!=null)){
                                                                        OpportunityTrackerModel.gSingleton.OpportunitiesChanged=mOpportunitiesChanged;
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
                }finally{
                }
                try{
                                 if(!postLoad){
                                              if((mSim.CreatedSim!=null)&&
                                                 (mSim.CreatedSim.OpportunityManager!=null)&&
                                                 (mSim.CreatedSim.OpportunityManager.Count>0)){
                                                                        OpportunityTrackerModel.FireOpportunitiesChanged();
                                              }
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
                }finally{
                }
                try{
                    EventTracker.sCurrentlyUpdatingDreamsAndPromisesManagers=false;
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
                }finally{
                }
                try{
                                                  mSim.mGenealogy=mGenealogy;
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
                }finally{
                }
                try{
                                if(mDoll!=null){
                                   mDoll.mOwner=mSim;
                                             if(mSim!=null&&
                                               !mSim.IsNeverSelectable&&
                                                mSim.Household==Household.ActiveHousehold){
                    try{
                                   mDoll.OnOwnerBecameSelectable();
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
                        try{
                SimDescription sim=mDoll.GetLiveFormSimDescription();
                            if(sim!=null){
            new DeepFixSimTask(sim).AddToSimulator();
                            }
                        }catch(Exception exception_){
             //  Get stack trace for the exception. with source file information
                  var st_=new StackTrace(exception_,true);
             //  Get the top stack frame
            var frame_=st_.GetFrame(0);
             //  Get the line number from the stack frame
       var line_=frame_.GetFileLineNumber();
                          Alive.WriteLog(exception_.Message+"\n\n"+
                                         exception_.StackTrace+"\n\n"+
                                         exception_.Source+"\n\n"+
                                         line_);
                        }
                    }
                                             }
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
                }finally{
                }
                try{
                                            if((mSim.CreatedSim!=null)&&
                                               (mSim.CreatedSim.Motives!=null)){
                                if(mAcademicPerformance!=-101){
                                                mSim.CreatedSim.Motives.CreateMotive(CommodityKind.AcademicPerformance);
                                  Motive motive=mSim.CreatedSim.Motives.GetMotive(   CommodityKind.AcademicPerformance);
                                      if(motive!=null){
                                         motive.Value=mAcademicPerformance;
                                      }
                                }
                                if(mUniversityStudy    !=-101){
                                                mSim.CreatedSim.Motives.CreateMotive(CommodityKind.UniversityStudy    );
                                  Motive motive=mSim.CreatedSim.Motives.GetMotive(   CommodityKind.UniversityStudy    );
                                      if(motive!=null){
                                         motive.Value=mUniversityStudy    ;
                                      }
                                }
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
                }finally{
                }
                try{
                               RestoreRelations(mSim,mRelations);
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
                }finally{
                }
                try{
                                            if((mSim.TraitChipManager!=null)&&
                                                 (mChips!=null)){
                                    for(int i=0;i<mChips.Length;i++){
                                               if(mChips[i]==null)continue;
                    try{
                                                mSim.TraitChipManager.AddTraitChip(mChips[i],i);
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
                    }finally{
                    }
                                    }
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
                }finally{
                }
                try{
                    sChangingWorldsSuppression.Pop();
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
                }finally{
                }
                try{
                                 if((mChangedHousehold!=null)&&
                                     (mChangedCallback!=null)){
                                     mChangedHousehold.HouseholdSimsChanged=mChangedCallback;
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
                }finally{
                }
            }
            public class ChangingWorldsSuppression{
                 bool mIsChangingWorlds=false;
                  int mCount=(0);
                  public ChangingWorldsSuppression(){}
                                   public void Push(){
                   if(mCount==0){
                      mIsChangingWorlds=GameStates.sIsChangingWorlds;
                                        GameStates.sIsChangingWorlds=true;
                   }
                      mCount++;
                                   }
                                   public void Pop(){
                      mCount--;
                   if(mCount<=0){
                      mCount=(0);
                                        GameStates.sIsChangingWorlds=mIsChangingWorlds;
                   }
                                   }
            }
        }
            public static void RemoveFreeStuffAlarm(SimDescription sim){
                //  Workaround for error in CelebrityManager:RemoveFreeStuffAlarm
                                                               if((sim.CelebrityManager!=null)&&
                                                                  (sim.CelebrityManager.mFreeStuffAlarmHandle!=AlarmHandle.kInvalidHandle)){
                                   AlarmManager.Global.RemoveAlarm(sim.CelebrityManager.mFreeStuffAlarmHandle);
                                                                   sim.CelebrityManager.mFreeStuffAlarmHandle=(AlarmHandle.kInvalidHandle);
                                                               }
            }
            public static Dictionary<SimDescription,Relationship>StoreRelations(SimDescription sim){
                          Dictionary<SimDescription,Relationship>relations;
                                                 if(Relationship.sAllRelationships.TryGetValue(sim,out relations)){
                          Dictionary<SimDescription,Relationship>newRelations=new Dictionary<SimDescription,Relationship>();
                    Dictionary<ulong,SimDescription>required=new Dictionary<ulong,SimDescription>();
                             foreach(SimDescription member in GetChildren(sim)){
                           if(!required.ContainsKey(member.SimDescriptionId)){
                               required.Add(        member.SimDescriptionId,member);
                           }
                             }
                             foreach(SimDescription member in GetParents( sim)){
                           if(!required.ContainsKey(member.SimDescriptionId)){
                               required.Add(        member.SimDescriptionId,member);
                           }
                             }
                             foreach(SimDescription member in GetSiblings(sim)){
                           if(!required.ContainsKey(member.SimDescriptionId)){
                               required.Add(        member.SimDescriptionId,member);
                           }
                             }
                                                                                 Career career=sim.Occupation as Career;
                                                                                    if((career!=null)&&
                                                                                       (career.Boss!=null)){
                           if(!required.ContainsKey(career.Boss.SimDescriptionId)){
                               required.Add(        career.Boss.SimDescriptionId,career.Boss);
                           }
                                                                                    }
                    Dictionary<ulong,SimDescription>existing=new Dictionary<ulong,SimDescription>();
                foreach(KeyValuePair<SimDescription,Relationship>relation in new Dictionary<SimDescription,Relationship>(relations)){
                                                              if(relation.Value.LTR.CurrentLTR==Sims3.UI.Controller.LongTermRelationshipTypes.Stranger){
                                          SafeRemoveRelationship(relation.Value);
                                                              }
                                         if(existing.ContainsKey(relation.Key.SimDescriptionId))continue;
                                                                                    SimDescription requiredSim;
                                         if(required.TryGetValue(relation.Key.SimDescriptionId,out requiredSim)){
                                                                        if(!object.ReferenceEquals(requiredSim,relation.Key))continue;
                                         }
                                                    existing.Add(relation.Key.SimDescriptionId,relation.Key);
                                              RepairRelationship(relation.Value);
                                                                 newRelations.Add(relation.Key,relation.Value);
                }
                                                         Relationship.sAllRelationships.Remove(sim);
                                                         Relationship.sAllRelationships.Add(   sim,newRelations);

                                                          return relations;
                                                 }
            return null;}
            public static void RestoreRelations(SimDescription sim,Dictionary<SimDescription,Relationship>relations){
                                                                                                       if(relations!=null){
                                                                Dictionary<SimDescription,Relationship>oldRelations;
                                                 if(Relationship.sAllRelationships.TryGetValue(sim,out oldRelations)){
                                                               List<SimDescription>remove=new List<SimDescription>();
                                             foreach(KeyValuePair<SimDescription,Relationship>relation in relations){
                                                                                                   if(!oldRelations.ContainsKey(relation.Key)){
                                                                                   remove.Add(relation.Key);
                                                                                                   }
                                             }
                                                   foreach(SimDescription other in remove){
                                                                                                          relations.Remove(other);
                                                   }
                                                 }
                         Relationship.sAllRelationships.Remove(sim);
                         Relationship.sAllRelationships.Add(   sim,relations);
                                                                                                       }
            }
            public static bool RepairRelationship(Relationship r){
                          bool result=(false);
                                              Dictionary<SimDescription,Relationship>relations;
                if(!Relationship.sAllRelationships.TryGetValue(r.SimDescriptionA,out relations)){
                                                                                     relations=new Dictionary<SimDescription,Relationship>();
                    Relationship.sAllRelationships.Add(        r.SimDescriptionA,    relations);
                }
                                                                                 if(!relations.ContainsKey(r.SimDescriptionB)){
                                                                                     relations.Add(        r.SimDescriptionB,r);

                               result=( true);
                                                                                 }
                if(!Relationship.sAllRelationships.TryGetValue(r.SimDescriptionB,out relations)){
                                                                                     relations=new Dictionary<SimDescription,Relationship>();
                    Relationship.sAllRelationships.Add(        r.SimDescriptionB,    relations);
                }
                                                                                 if(!relations.ContainsKey(r.SimDescriptionA)){
                                                                                     relations.Add(        r.SimDescriptionA,r);

                               result=( true);
                                                                                 }
                        return result;
            }
            public static void SafeRemoveRelationship(Relationship r){
                                                  Dictionary<SimDescription,Relationship>relations;
                     if(Relationship.sAllRelationships.TryGetValue(r.SimDescriptionA,out relations)){
                                                                                         relations.Remove(r.SimDescriptionB);
                     }
                     if(Relationship.sAllRelationships.TryGetValue(r.SimDescriptionB,out relations)){
                                                                                         relations.Remove(r.SimDescriptionA);
                     }
            }
                public static List<SimDescription>GetParents (SimDescription sim){
                                                                        if((sim==null)||(sim.Genealogy==null)){
                   return new List<SimDescription>();
                                                                        }
                   return GetSims(sim.Genealogy.Parents );
                }
                public static List<SimDescription>GetChildren(SimDescription sim){
                                                                         if((sim==null)||(sim.Genealogy==null)){
                   return new List<SimDescription>();
                                                                         }
                   return GetSims(sim.Genealogy.Children);
                }
                public static List<SimDescription>GetSiblings(SimDescription sim){
                                                                         if((sim==null)||(sim.Genealogy==null)){
                   return new List<SimDescription>();
                                                                         }
                   return GetSims(sim.Genealogy.Siblings);
                }
                    public static List<SimDescription>GetSims(List<Genealogy>genes){
                                  List<SimDescription>list=new List<SimDescription>();
                                                                          if(genes!=null){
                                                   foreach(Genealogy gene in genes){
                                           SimDescription sim=GetSim(gene);
                                                       if(sim==null)continue;
                                                 list.Add(sim);
                                                   }
                                                                          }
                                               return list;
                    }
                        public static SimDescription GetSim(Genealogy genealogy){
                                                                   if(genealogy==null)return null;
                try{
                                                               return genealogy.SimDescription;
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
                }finally{
                }
                                                               return null;
                        }
            public static Urnstone FindGhostsGrave(SimDescription sim){
                  foreach(Urnstone urnstone in Sims3.Gameplay.Queries.GetObjects<Urnstone>()){
         if(object.ReferenceEquals(urnstone.DeadSimsDescription,sim)){
                            return urnstone;
         }
                  }
                            return null;
            }
           }
            public static Vector3 FindSafeLocation(Lot lot,bool isHorse){
                                                    if(lot==null)return Vector3.Invalid;
                                                             if(isHorse){
                                       Mailbox mailbox=lot.FindMailbox();
                                            if(mailbox!=null){
                                        return mailbox.Position;
                                            }else{
                                        Door frontDoor=lot.FindFrontDoor();
                                          if(frontDoor!=null){
                                  int roomId=frontDoor.GetRoomIdOfDoorSide(CommonDoor.tSide.Front);
                                   if(roomId!=0){
                                      roomId=frontDoor.GetRoomIdOfDoorSide(CommonDoor.tSide.Back );
                                   }
                                   if(roomId==0){
                               List<GameObject>objects=lot.GetObjectsInRoom<GameObject>(roomId);
                                            if(objects.Count>0){
     return RandomUtil.GetRandomObjectFromList(objects).Position;
                                            }
                                   }
                                          }
                                            }
                                                             }
            return lot.EntryPoint();}
            public static Vector3 FindSafeLocationInRandomLot(Lot lot){
                                             List<Lot>lots=new List<Lot>(LotManager.sLots.Values);
                                                      lots.Remove(lot);
                                                   if(lots.Count==0){
                                                      lots.Add(lot);
                                                   }
            return Service.GetPositionInRandomLot(RandomUtil.GetRandomObjectFromList(lots));}
        public static Sim InstantiateAtHome(SimDescription ths,SimOutfit outfit){
                                                        if(ths==null)return null;
                                                   Lot lot=ths.LotHome;
                                                    if(lot==null){
                                                       lot=ths.VirtualLotHome;
                                                    }   
                                        Vector3 position=(Vector3.Invalid);
                                                    if(lot!=null){
                             position=FindSafeLocation(lot,ths.IsHorse);
                                             if(position==Vector3.Invalid){
                                              position=lot.EntryPoint();
                                             }
                                                    }else{
                  position=FindSafeLocationInRandomLot(lot);
                                                    }
                          Vector3 foundPosition=position;
                try{
                                                        if(ths.CreatedSim!=null){
                                           Vector3 forward=ths.CreatedSim.ForwardVector;
                                                        if(ths.IsHorse){
                                 FindGoodLocationBooleans fglBooleans=FindGoodLocationBooleans.Routable|
                                                                      FindGoodLocationBooleans.PreferEmptyTiles|
                                                                      FindGoodLocationBooleans.AllowOnSlopes|
                                                                      //FindGoodLocationBooleans.AllowIntersectionWithPlatformWalls|
                                                                      //FindGoodLocationBooleans.AllowInFrontOfDoors          |
                                                                      //FindGoodLocationBooleans.AllowOnStairTopAndBottomTiles|
                                                                      FindGoodLocationBooleans.AllowOffLot        |
                                                                      FindGoodLocationBooleans.AllowOnStreets     |
                                                                      FindGoodLocationBooleans.AllowOnBridges     |
                                                                      FindGoodLocationBooleans.AllowOnSideWalks   ;
if(!GlobalFunctions.FindGoodLocationNearbyOnLevel(ths.CreatedSim,ths.CreatedSim.Level,ref position,ref forward,fglBooleans)){
    GlobalFunctions.FindGoodLocationNearbyOnLevel(ths.CreatedSim,ths.CreatedSim.Level,ref position,ref forward,FindGoodLocationBooleans.None);
}
World.FindGoodLocationParams fglParams=new World.FindGoodLocationParams(position);
                             fglParams.BooleanConstraints=fglBooleans;
if(!GlobalFunctions.FindGoodLocation(ths.CreatedSim,fglParams,out position,out forward)){
                                                    fglParams.BooleanConstraints=FindGoodLocationBooleans.None;
    GlobalFunctions.FindGoodLocation(ths.CreatedSim,fglParams,out position,out forward);
}
                                                        }else{
World.FindGoodLocationParams fglParams=new World.FindGoodLocationParams(position);
                             fglParams.BooleanConstraints=FindGoodLocationBooleans.Routable|
                                                          FindGoodLocationBooleans.PreferEmptyTiles|
                                                          FindGoodLocationBooleans.AllowOnSlopes|
                                                          FindGoodLocationBooleans.AllowIntersectionWithPlatformWalls|
                                                          FindGoodLocationBooleans.AllowInFrontOfDoors          |
                                                          FindGoodLocationBooleans.AllowOnStairTopAndBottomTiles|
                                                          FindGoodLocationBooleans.AllowOffLot        |
                                                          FindGoodLocationBooleans.AllowOnStreets     |
                                                          FindGoodLocationBooleans.AllowOnBridges     |
                                                          FindGoodLocationBooleans.AllowOnSideWalks   ;
if(!GlobalFunctions.FindGoodLocation(ths.CreatedSim,fglParams,out position,out forward)){
                                                    fglParams.BooleanConstraints=FindGoodLocationBooleans.None;
    GlobalFunctions.FindGoodLocation(ths.CreatedSim,fglParams,out position,out forward);
}
                                                        }
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
                                                position=foundPosition; 
                }
                                                bool noSim=(false);
                try{
                                     ResourceKey outfitKey=ths.mDefaultOutfitKey;
                                                                      if(outfit==null){
                                                        if(ths.IsHorse){
                                                                         outfit=ths.GetOutfit(OutfitCategories.Naked   ,0);
                                                        }
                                                                      if(outfit==null){
                                                                         outfit=ths.GetOutfit(OutfitCategories.Everyday,0);
                                                                      }
                                                                     if((outfit==null)||
                                                                       (!outfit.IsValid)){
                                                     noSim=( true);
                                                                     }
                                                                      }
                                                 if(!noSim){
                                                                      if(outfit!=null){
                                                 outfitKey=outfit.Key;
                                                                      }
                                    return DeepInstantiate(ths,position,outfitKey,outfit);
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
                                                           ths.mSim=null;
                                                           ths.mDefaultOutfitKey=ResourceKey.kInvalidResourceKey;
                                                     noSim=( true);
                }
                                                  if(noSim){
                                                DeepFixSim(ths);
                                                  }
                                    return DeepInstantiate(ths,position,ResourceKey.kInvalidResourceKey);
        }
            public static void DeepFixSim(SimDescription sim){
                try{
                    OutfitCategories[]categoriesArray=null;
                                                  switch(sim.Species){
                    case CASAgeGenderFlags.Human:
                                      categoriesArray=new OutfitCategories[]{OutfitCategories.Everyday,OutfitCategories.Naked,OutfitCategories.Athletic,OutfitCategories.Formalwear,OutfitCategories.Sleepwear,OutfitCategories.Swimwear};
                        break;
                    case CASAgeGenderFlags.Horse:
                                      categoriesArray=new OutfitCategories[]{OutfitCategories.Everyday,OutfitCategories.Naked,OutfitCategories.Racing  ,OutfitCategories.Bridle    ,OutfitCategories.Jumping};
                        break;
                    default:
                                      categoriesArray=new OutfitCategories[]{OutfitCategories.Everyday,OutfitCategories.Naked};
                        break;
                                                  }
                                          SimOutfit sourceOutfit=null;
                    for(int i=0;i<2;i++){
                                             OutfitCategoryMap map=null;
                         if(i==0){
                                                               map=sim.mOutfits;
                         }else{
                                                               map=sim.mMaternityOutfits;
                         }
                                                            if(map==null)continue;
             foreach(OutfitCategories category in Enum.GetValues(typeof(OutfitCategories))){
                                   if(category==OutfitCategories.Supernatural)continue;
                                             ArrayList outfits=map[category]as ArrayList;
                                                    if(outfits==null)continue;
                        foreach(SimOutfit anyOutfit in outfits){
                                      if((anyOutfit!=null)&&
                                         (anyOutfit.IsValid)){
                                                    sourceOutfit=anyOutfit;
                                break;
                                      }
                        }
             }
                    }
                SimBuilder builder=new SimBuilder();
                           builder.UseCompression=true;
                                             var simTone=sim.SkinToneKey;
                             List<ResourceKey>choiceTones=new List<ResourceKey>();
                                          KeySearch tones=new KeySearch(0x0354796a);
                        foreach(ResourceKey tone in tones){
                            choiceTones.Add(tone);
                        }
                                                    tones.Reset();
                                             if((simTone.InstanceId==0)||(!choiceTones.Contains(simTone))){
                                                 simTone=RandomUtil.GetRandomObjectFromList(choiceTones);
                                             }
                                     ResourceKey newTone=simTone;
                           builder.Age          =sim.Age               ;
                           builder.Gender       =sim.Gender            ;
                           builder.Species      =sim.Species           ;
                           builder.SkinTone     =newTone               ;
                           builder.SkinToneIndex=sim.SkinToneIndex     ;
                           builder.MorphFat     =sim.mCurrentShape.Fat ;
                           builder.MorphFit     =sim.mCurrentShape.Fit ;
                           builder.MorphThin    =sim.mCurrentShape.Thin;
GeneticsPet.SpeciesSpecificData speciesData=OutfitUtils.GetSpeciesSpecificData(sim);

                    try{
                                                 if(sourceOutfit!=null){
               foreach(SimOutfit.BlendInfo blend in sourceOutfit.Blends){
                           builder.SetFacialBlend(blend.key,blend.amount);
               }
    CASParts.OutfitBuilder.CopyGeneticParts(builder,sourceOutfit);
                                                 }else{
                                                      if(sim.Genealogy!=null){
                                     List<SimDescription>     parents=new List<SimDescription>();
                                     List<SimDescription>grandParents=new List<SimDescription>();
                                                   foreach(SimDescription parent in ResetClearSimTask.GetParents(sim)){
                                                              parents.Add(parent);
                                                   foreach(SimDescription grandParent in ResetClearSimTask.GetParents(parent)){
                                                         grandParents.Add(grandParent);
                                                   }
                                                   }
                                                           if(parents.Count>0){
                                                      if(sim.IsHuman){
                         Genetics.InheritFacialBlends(builder,parents.ToArray(),new Random());
                                                      }else{
                     GeneticsPet.InheritBodyShape    (builder,parents,grandParents,new Random());
                     GeneticsPet.InheritBasePeltLayer(builder,parents,grandParents,new Random());
                     GeneticsPet.InheritPeltLayers   (builder,parents,grandParents,new Random());
                                                      }
                                                           }
                                                      }
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
return;
                    }
                                                      if(sim.IsRobot){
                    OutfitUtils.AddMissingPartsBots(builder,(OutfitCategories)0x200002,true,sim);
Sleep();
                    OutfitUtils.AddMissingPartsBots(builder, OutfitCategories.Everyday,true,sim);
Sleep();
                                                      }else 
                                                      if(sim.IsHuman){
                    OutfitUtils.AddMissingParts    (builder,(OutfitCategories)0x200002,true,sim,sim.IsAlien);
Sleep();
                    OutfitUtils.AddMissingParts    (builder, OutfitCategories.Everyday,true,sim,sim.IsAlien);
Sleep();
                                                      }else{
                    OutfitUtils.AddMissingPartsPet (builder, OutfitCategories.Everyday|(OutfitCategories)0x200000,true,sim,speciesData);
Sleep();
                    OutfitUtils.AddMissingPartsPet (builder, OutfitCategories.Everyday,true,sim,speciesData);
Sleep();
                                                      }
                ResourceKey uniformKey=new ResourceKey();
                                                      if(sim.IsHuman){
if(LocaleConstraints.GetUniform(ref uniformKey,sim.HomeWorld,builder.Age,builder.Gender,OutfitCategories.Everyday)){
                    OutfitUtils.SetOutfit          (builder,new SimOutfit(uniformKey),sim);
}
                                                      }
                    OutfitUtils.SetAutomaticModifiers(builder);
                                                         sim.ClearOutfits(OutfitCategories.Career     ,false);
                                                         sim.ClearOutfits(OutfitCategories.MartialArts,false);
                                                         sim.ClearOutfits(OutfitCategories.Special    ,false);
 foreach(OutfitCategories category in categoriesArray){
                                       ArrayList outfits=null;
                                                 outfits=sim.Outfits[category]as ArrayList;
                                              if(outfits!=null){
                                       int index=0;
                                     while(index<outfits.Count){
                             SimOutfit anyOutfit=outfits[index]as SimOutfit;
                                    if(anyOutfit==null){
                                                 outfits.RemoveAt(index);
                                    }else 
                                   if(!anyOutfit.IsValid){
                                                 outfits.RemoveAt(index);
                                   }else{
                                           index++;
                                   }
                                     }
                                              }
                                             if((outfits==null)||
                                                (outfits.Count==0)){
                    OutfitUtils.MakeCategoryAppropriate(builder,category,sim);
                                                      if(sim.IsHuman){
if(LocaleConstraints.GetUniform(ref uniformKey,sim.HomeWorld,builder.Age,builder.Gender,category)){
                    OutfitUtils.SetOutfit(builder,new SimOutfit(uniformKey),sim);
}
                                                      }
                                                         sim.RemoveOutfits(category,false);
                                      CASParts.AddOutfit(sim,category,builder,true);
                                             }
                                                      if(sim.IsUsingMaternityOutfits){
                                                         sim.BuildPregnantOutfit(category);
                                                      }
 }
                                                      if(sim.IsMummy){
                                     OccultMummy.OnMerge(sim);
                                                      }else 
                                                      if(sim.IsFrankenstein){
                              OccultFrankenstein.OnMerge(sim,sim.OccultManager.mIsLifetimeReward);
                                                      }else 
                                                      if(sim.IsGenie){
                              OccultGenie.OverlayUniform(sim,OccultGenie.CreateUniformName(sim.Age,sim.Gender),ProductVersion.EP6,OutfitCategories.Everyday,CASSkinTones.BlueSkinTone,0.68f);
                                                      }else 
                                                      if(sim.IsImaginaryFriend){
                            OccultImaginaryFriend friend=sim.OccultManager.GetOccultType(Sims3.UI.Hud.OccultTypes.ImaginaryFriend) as OccultImaginaryFriend;
                          OccultBaseClass.OverlayUniform(sim,OccultImaginaryFriend.CreateUniformName(sim.Age,friend.Pattern),ProductVersion.EP4,OutfitCategories.Special,CASSkinTones.NoSkinTone,0f);
                                                      }
                                                      if(sim.IsMermaid){
                                OccultMermaid.AddOutfits(sim,null);
                                                      }
                                                      if(sim.IsWerewolf){
                                                      if(sim.ChildOrAbove){
                                                             SimOutfit newWerewolfOutfit=OccultWerewolf.GetNewWerewolfOutfit(sim.Age,sim.Gender);
                                                                    if(newWerewolfOutfit!=null){
                                                         sim.AddOutfit(newWerewolfOutfit,OutfitCategories.Supernatural,0x0);
                                                                    }
                                                      }
                                                      }
                                 SimOutfit currentOutfit=null;
                                                      if(sim.CreatedSim!=null){
//========================================================================================================================================================
                try{
                        SimDescription simDesc=sim.CreatedSim.SimDescription;
                         if(Simulator.GetProxy(sim.CreatedSim.ObjectId)==null){
                                    if(simDesc!=null){
                                               sim.CreatedSim.Destroy();
                                    }                  
goto _Reset;
                         }
                                    if(simDesc==null){
                                               sim.CreatedSim.mSimDescription=new SimDescription();
                                               sim.CreatedSim.Destroy();                    
goto _Reset;
                                    }
                                            if(sim.LotHome!=null){
                                       simDesc.IsZombie=false;
                                    if(simDesc.CreatedSim!=sim.CreatedSim){
                                                           sim.CreatedSim.Destroy();
                                       simDesc.CreatedSim=null;                        
goto _Reset;
                                    }else{                        
                                        Bed     myBed    =null;
                                        BedData myBedData=null;
                                foreach(Bed bed in sim.LotHome.GetObjects<Bed>()){
                                                myBedData=bed.GetPartOwnedBy(sim.CreatedSim);
                                             if(myBedData!=null){
                                                myBed=bed;
                                                break;
                                             }
                                }
                    ResetClearSimTask.ResetPosture(sim.CreatedSim);
                                    if(simDesc.TraitManager==null){
                                       simDesc.mTraitManager=new TraitManager();
                                    }
                    try{
                                       simDesc.Fixup();
 ResetClearSimTask.CleanupBrokenSkills(simDesc);
         ResetClearSimTask.ResetCareer(simDesc);
                                       simDesc.ClearSpecialFlags();
                                    if(simDesc.Pregnancy==null){
                        try{
                                    if(simDesc.mMaternityOutfits==null){
                                       simDesc.mMaternityOutfits=new OutfitCategoryMap();
                                    }
                                       simDesc.SetPregnancy(0,false);
                                       simDesc.ClearMaternityOutfits();
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
                        }finally{
                        }
                                    }
                                            if(sim.CreatedSim.CurrentCommodityInteractionMap==null){
                        try{
                   LotManager.PlaceObjectOnLot(sim.CreatedSim,sim.CreatedSim.ObjectId);
                                            if(sim.CreatedSim.CurrentCommodityInteractionMap==null){
                                               sim.CreatedSim.ChangeCommodityInteractionMap(sim.LotHome.Map);
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
                        }finally{
                        }
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
                    }finally{
                    }
             ResetClearSimTask.ResetSituations(sim.CreatedSim);
                                  //
                ResetClearSimTask.CleanupSlots(sim.CreatedSim);
              ResetClearSimTask.ResetInventory(sim.CreatedSim);
                                  //    
                                            if(sim.CreatedSim.Inventory==null){
                                               sim.CreatedSim.AddComponent<InventoryComponent>(new object[0x0]);
                                            }                      
                                                                                                   StuckSimData stuckSim;
                                                         if(!StuckSims.TryGetValue(simDesc.SimDescriptionId,out stuckSim)){
                                                                                                                stuckSim=new StuckSimData();
                                                             StuckSims.Add(        simDesc.SimDescriptionId,    stuckSim);
                                                         }
                                                                                                                stuckSim.Detections++;
                                                                     ResetStuckSim(sim.CreatedSim,Vector3.Invalid,"Deep",true);
                ResetClearSimTask.ResetRouting(sim.CreatedSim);
                                               sim.CreatedSim.SetObjectToReset();  
                                //  [Nraas:]This is necessary to clear certain types of interactions
                                // (it is also called in SetObjectToReset(), though doesn't always work there)
                                            if(sim.CreatedSim.InteractionQueue!=null){
                                               sim.CreatedSim.InteractionQueue.OnReset();
                                            }
 ResetClearSimTask.ResetSkillModifiers(simDesc);
                   ResetClearSimTask.ResetRole(sim.CreatedSim);
                                    if(simDesc.IsEnrolledInBoardingSchool()){
                                       simDesc.BoardingSchool.OnRemovedFromSchool();
                                    }
                        MiniSimDescription miniSim=MiniSimDescription.Find(simDesc.SimDescriptionId);
                                        if(miniSim!=null){
                                           miniSim.Instantiated=true;
                                        }
             ResetClearSimTask.UpdateInterface(sim.CreatedSim);
goto _Reset;
                                    }
                                            }
                                            else 
                                            if(simDesc.Service is Butler){
                                                                                                   StuckSimData stuckSim;
                                                         if(!StuckSims.TryGetValue(simDesc.SimDescriptionId,out stuckSim)){
                                                                                                                stuckSim=new StuckSimData();
                                                             StuckSims.Add(        simDesc.SimDescriptionId,    stuckSim);
                                                         }
                                                                                                                stuckSim.Detections++;
                                                                     ResetStuckSim(sim.CreatedSim,Vector3.Invalid,"Deep",true);
       sim.CreatedSim.Motives?.RecreateMotives(sim.CreatedSim);
                                               sim.CreatedSim.SetObjectToReset();  
goto _Reset;
                                            }else 
                                            if(simDesc.IsImaginaryFriend){
                                            OccultImaginaryFriend friend;
  if(OccultImaginaryFriend.TryGetOccultFromSim(sim.CreatedSim,out friend)){
                                            if(Simulator.GetProxy(friend.mDollId)!=null){
                                                                  friend.TurnBackIntoDoll(OccultImaginaryFriend.Destination.Owner);
goto _Reset;
                                            }
  }
                                            }else 
                                            if(simDesc.IsBonehilda){
    foreach(BonehildaCoffin coffin in Sims3.Gameplay.Queries.GetObjects<BonehildaCoffin>()){
                         if(coffin.mBonehilda==simDesc){
                            coffin.mBonehildaSim=null;
                            break;
                         }
    }
goto _Reset;
                                            }
goto _Reset;
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
goto _Reset;
                }finally{
                }
_Reset:{}                 
//========================================================================================================================================================
                    try{
                                                         sim.CreatedSim.SwitchToOutfitWithoutSpin(Sim.ClothesChangeReason.GoingOutside,OutfitCategories.Everyday,true);
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
                    }
                                           currentOutfit=sim.CreatedSim.CurrentOutfit;
                                                      }else{
                                           currentOutfit=sim.GetOutfit(OutfitCategories.Everyday,0);
                                                      }
                                        if(currentOutfit!=null){
ThumbnailManager.GenerateHouseholdSimThumbnail(currentOutfit.Key,currentOutfit.Key.InstanceId,0x0,ThumbnailSizeMask.Large|ThumbnailSizeMask.ExtraLarge|ThumbnailSizeMask.Medium|ThumbnailSizeMask.Small,ThumbnailTechnique.Default,true,false,sim.AgeGenderSpecies);
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
                }
            }
        //  [NRaas]From SimDescription.Instantiate
            private static Sim DeepInstantiate(SimDescription ths,Vector3 position,ResourceKey outfitKey,SimOutfit outfit=null,bool forceAlwaysAnimate=true){
                                                                                            if(outfitKey==ResourceKey.kInvalidResourceKey||
                                                                                                                   outfit==null){
                try{
                                                                                               outfitKey=ths.mDefaultOutfitKey;
                                                                                                                if(outfit==null){
                                                           if(ths.IsHorse){
                                                                                                                   outfit=ths.GetOutfit(OutfitCategories.Naked   ,0);
                                                           }
                                                                                                                if(outfit==null){
                                                                                                                   outfit=ths.GetOutfit(OutfitCategories.Everyday,0);
                                                                                                                }
                                                                                                               if((outfit==null)||
                                                                                                                 (!outfit.IsValid)){
            return null;
                                                                                                               }
                                                                                                                }
                                                                                                                if(outfit!=null){
                                                                                               outfitKey=outfit.Key;
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
                                                           ths.mSim=null;
                                                           ths.mDefaultOutfitKey=ResourceKey.kInvalidResourceKey;
            return null;
                }
                                                                                            }
                Household.HouseholdSimsChangedCallback changedCallback=null;
                Household                              changedHousehold = null;
                  bool isChangingWorlds=GameStates.sIsChangingWorlds;
         bool isLifeEventManagerEnabled=LifeEventManager.sIsLifeEventManagerEnabled;
                    ResetClearSimTask.RemoveFreeStuffAlarm(ths);
using(SafeStore store=new SafeStore(ths,SafeStore.Flag.LoadFixup|SafeStore.Flag.Selectable|SafeStore.Flag.Unselectable)){
                try{
                                        //  Stops the memories system from interfering
                                        LifeEventManager.sIsLifeEventManagerEnabled=false;
                                        //  Stops UpdateInformationKnownAboutRelationships()
                                        GameStates.sIsChangingWorlds=true;
                                                        if(ths.Household!=null){
                                                       changedCallback=ths.Household.HouseholdSimsChanged;
                                                       changedHousehold=ths.Household;
                                                           ths.Household.HouseholdSimsChanged=null;
                                                        }
                                                        if(ths.CreatedSim!=null){
                                                                                               StuckSimData stuckSim;
                                                         if(!StuckSims.TryGetValue(ths.SimDescriptionId,out stuckSim)){
                                                                                                            stuckSim=new StuckSimData();
                                                             StuckSims.Add(        ths.SimDescriptionId,    stuckSim);
                                                         }
                                                                                                            stuckSim.Detections++;
                                                                     ResetStuckSim(ths.CreatedSim,Vector3.Invalid,"Deep",true);
                                                           ths.CreatedSim.SetObjectToReset();
                                                    return ths.CreatedSim;
                                                        }
                                                        if(ths.AgingState!=null){
                                      bool flag=outfitKey==ths.mDefaultOutfitKey;
                                                           ths.AgingState.SimBuilderTaskDeferred=false;
                                                           ths.AgingState.PreInstantiateSim(ref outfitKey);
                                        if(flag){
                                                           ths.mDefaultOutfitKey=outfitKey;
                                        }
                                                        }
                                                                                                                       int capacity=forceAlwaysAnimate?0x4:0x2;
                                                                                         Hashtable overrides=new Hashtable(capacity);
                                                                                                   overrides["simOutfitKey"]=outfitKey;
                                                                                                   overrides["rigKey"]=CASUtils.GetRigKeyForAgeGenderSpecies((ths.Age|ths.Gender)|ths.Species);
                                                                                                                                 if(forceAlwaysAnimate){
                                                                                                   overrides["enableSimPoseProcessing"]=0x1;
                                                                                                   overrides["animationRunsInRealtime"]=0x1;
                                                                                                                                 }
                                string instanceName="GameSim";
ProductVersion version=ProductVersion.BaseGame;
                                                        if(ths.Species!=CASAgeGenderFlags.Human){
                                       instanceName="Game"+ths.Species;
               version=ProductVersion.EP5;
                                                        }
          SimInitParameters initData=new SimInitParameters(ths);
   Sim target=GlobalFunctions.CreateObjectWithOverrides(instanceName,version,position,0x0,Vector3.UnitZ,overrides,initData)as Sim;
    if(target!=null){
    if(target.SimRoutingComponent==null){//  [NRaas:]Performed to ensure that a useful error message is produced when the Sim construction fails
       target.OnCreation();
       target.OnStartup();
    }
       target.SimRoutingComponent.EnableDynamicFootprint();
       target.SimRoutingComponent.ForceUpdateDynamicFootprint();
                                                           ths.PushAgingEnabledToAgingManager();
                        //  [NRaas:]
                            /* This code is idiotic
                            if ((ths.Teen) && (target.SkillManager != null))
                            {
                                Skill skill = target.SkillManager.AddElement(SkillNames.Homework);
                                while (skill.SkillLevel < SimDescription.kTeenHomeworkSkillStartLevel)
                                {
                                    skill.ForceGainPointsForLevelUp();
                                }
                            }
                            */
                        //  [Me:]Why? What is happening exactly there? o.o
                            //  [NRaas:]Custom [Me:]TO DO: REimplement this later
                            //OccultTypeHelper.SetupForInstantiatedSim(ths.OccultManager);
                                                        if(ths.IsAlien){
World.ObjectSetVisualOverride(target.ObjectId,eVisualOverrideTypes.Alien,null);
                                                        }
                                                                                               StuckSimData stuckSim;
                                                         if(!StuckSims.TryGetValue(ths.SimDescriptionId,out stuckSim)){
                                                                                                            stuckSim=new StuckSimData();
                                                             StuckSims.Add(        ths.SimDescriptionId,    stuckSim);
                                                         }
                                                                                                            stuckSim.Detections++;
                                                                     ResetStuckSim(target,Vector3.Invalid,"Deep",true);
                    EventTracker.SendEvent(EventTypeId.kSimInstantiated,null,target);
                            /*
                            MiniSimDescription description = MiniSimDescription.Find(ths.SimDescriptionId);
                            if ((description == null) || (!GameStates.IsTravelling && (ths.mHomeWorld == GameUtils.GetCurrentWorld())))
                            {
                                return target;
                            }
                            description.UpdateInWorldRelationships(ths);
                            */
                                                        if(ths.HealthManager!=null){
                                                           ths.HealthManager.Startup();
                                                        }
                                                      if(((ths.SkinToneKey.InstanceId==15475186560318337848L)&&!ths.OccultManager.HasOccultType(Sims3.UI.Hud.OccultTypes.Vampire))&&(!ths.OccultManager.HasOccultType(Sims3.UI.Hud.OccultTypes.Werewolf)&&!ths.IsGhost)){
World.ObjectSetVisualOverride(ths.CreatedSim.ObjectId,eVisualOverrideTypes.Genie,null);
                                                      }
                                                        if(ths.Household.IsAlienHousehold){
(Sims3.UI.Responder.Instance.HudModel as HudModel).OnSimCurrentWorldChanged(true,ths);
                                                        }
                if(Household.RoommateManager.IsNPCRoommate(ths.SimDescriptionId)){
                   Household.RoommateManager.AddRoommateInteractions(target);
                }
    }
return target;
                }finally{
                                        LifeEventManager.sIsLifeEventManagerEnabled=isLifeEventManagerEnabled;
                                        GameStates.sIsChangingWorlds=isChangingWorlds;

                                                   if((changedHousehold!=null)&&
                                                      (changedCallback!=null)){
                                                       changedHousehold.HouseholdSimsChanged=changedCallback;
                                                    if(changedHousehold.HouseholdSimsChanged!=null){
                                                       changedHousehold.HouseholdSimsChanged(Sims3.Gameplay.CAS.HouseholdEvent.kSimAdded,ths.CreatedSim,null);
                                                    }
                                                   }
                }
}
            }  
           public class ResetStuckSimTask:AlarmTask{
        const                  string                        _CLASS_NAME=".ResetStuckSimTask:AlarmTask.";
                      readonly string                        Suffix;
                      readonly Sim                           _Sim;
                               Vector3                       Destination;
                 public ResetStuckSimTask(Sim sim,Vector3 destination,string suffix):base(1,TimeUnit.Minutes,null){
                                         _Sim=sim;
                                              Destination=destination;
                                                                      Suffix=suffix;
                 }
            protected override void OnPerform(){
                      //Alive.WriteLog("perform:ResetStuckSimTask");
                              ResetStuckSim(_Sim,Destination,Suffix);
                               base.OnPerform();
            }
           }
        protected static void ResetStuckSim(Sim sim,Vector3 destination,string suffix,bool deep=false){
                                             if(sim!=null&&
                                               !sim.HasBeenDestroyed&&
                                                sim.SimDescription!=null){
                  //Alive.WriteLog("sim_is_stuck:reset_sim_in_progress:sim:"+sim.Name+";destination:"+destination);
                                        Lot lot=null;
                                             if(sim.SimDescription.IsBonehilda){
        lot=BonehildaCoffin.FindBonehildaCoffin(sim)?.LotCurrent;
                                         if(lot==null){
                                            lot=sim.LotHome;
                                         }
                                             }else{
                                            lot=sim.LotHome;
                                             }
                                         if(lot==null){
                                            lot=sim.VirtualLotHome;
                                         }
                                         if(lot==null){
                                            lot=Sim.ActiveActor?.LotHome;
                                         }
                                                    bool addToWorld=( true);
                                        Vector3 resetRawDest=destination;
                                                             StuckSims.TryGetValue(sim.SimDescription.SimDescriptionId,out StuckSimData stuckSim);   
                                                          if(stuckSim!=null){
                                                          //-------------------------
                                                          if(stuckSim.Detections<=2){
                                Daycare daycare;
                                            if((sim.Household==null||
                                              (!sim.Household.InWorld&&
                                               !sim.Household.IsSpecialHousehold))&&
                 (!Passport.IsHostedPassportSim(sim)&&
                                                sim.SimDescription.AssignedRole==null)&&
   (LunarCycleManager.sFullMoonZombies==null||  
   !LunarCycleManager.sFullMoonZombies.Contains(sim.SimDescription))&&
                                      ((daycare=sim.SimDescription.Occupation as Daycare)==null||
                        !daycare.IsDaycareChild(sim.SimDescription))&&
                                               !sim.SimDescription.IsBonehilda&&
                                                sim.Service==null){
                                                         addToWorld=(false);
                                            }
                                                          if(destination!=Vector3.Invalid){
                                                   goto DestSet;
                                                          }
                                                          }
                                                          //-------------------------
                                                          if(stuckSim.Detections<=3){
                                         if(lot==null){
                                            lot=RandomUtil.GetRandomObjectFromList(Sims3.Gameplay.Queries.GetObjects<Lot>());
                                         }
                                         if(lot!=null){
                                                resetRawDest=lot.EntryPoint();
                                                   goto DestSet;
                                         }
                                                          }
                                                          //-------------------------
                                                          if(stuckSim.Detections<=4){
                                         if(lot==null){
                                            lot=RandomUtil.GetRandomObjectFromList(Sims3.Gameplay.Queries.GetObjects<Lot>());
                                         }
                                         if(lot!=null){
                            Mailbox mailbox=lot.FindMailbox();
                                 if(mailbox!=null){
                                                resetRawDest=mailbox.Position;
                                                   goto DestSet;
                                 }
                                         }
                                                          }
                                                          //-------------------------
                                                          if(stuckSim.Detections<=5){
                                         if(lot==null){
                                            lot=RandomUtil.GetRandomObjectFromList(Sims3.Gameplay.Queries.GetObjects<Lot>());
                                         }
                                         if(lot!=null){
                             Door frontDoor=lot.FindFrontDoor();
                               if(frontDoor!=null){
                       int roomId=frontDoor.GetRoomIdOfDoorSide(CommonDoor.tSide.Front);
                        if(roomId!=0){
                           roomId=frontDoor.GetRoomIdOfDoorSide(CommonDoor.tSide.Back);
                        }
                        if(roomId==0){
                    List<GameObject>objects=lot.GetObjectsInRoom<GameObject>(roomId);
                                 if(objects.Count>0){
                                                resetRawDest=RandomUtil.GetRandomObjectFromList(objects).Position;
                                                   goto DestSet;
                                 }
                        }
                               }
                                         }
                                                          }
                                                          //-------------------------
                                                          if(stuckSim.Detections<=6){
                                            lot=RandomUtil.GetRandomObjectFromList(Sims3.Gameplay.Queries.GetObjects<Lot>());
                                         if(lot!=null){
                                                resetRawDest=lot.EntryPoint();
                                                   goto DestSet;
                                         }
                                                          }
                                                          //-------------------------
                                                          if(stuckSim.Detections<=7){
                                            lot=RandomUtil.GetRandomObjectFromList(Sims3.Gameplay.Queries.GetObjects<Lot>());
                                         if(lot!=null){
                            Mailbox mailbox=lot.FindMailbox();
                                 if(mailbox!=null){
                                                resetRawDest=mailbox.Position;
                                                   goto DestSet;
                                 }
                                         }
                                                          }
                                                          //-------------------------
                                                          if(stuckSim.Detections<=8){
                                            lot=RandomUtil.GetRandomObjectFromList(Sims3.Gameplay.Queries.GetObjects<Lot>());
                                         if(lot!=null){
                             Door frontDoor=lot.FindFrontDoor();
                               if(frontDoor!=null){
                       int roomId=frontDoor.GetRoomIdOfDoorSide(CommonDoor.tSide.Front);
                        if(roomId!=0){
                           roomId=frontDoor.GetRoomIdOfDoorSide(CommonDoor.tSide.Back);
                        }
                        if(roomId==0){
                    List<GameObject>objects=lot.GetObjectsInRoom<GameObject>(roomId);
                                 if(objects.Count>0){
                                                resetRawDest=RandomUtil.GetRandomObjectFromList(objects).Position;
                                                   goto DestSet;
                                 }
                        }
                               }
                                         }
                                                          }
                                                          //-------------------------
                                                          if(stuckSim.Detections<=9){
                                                resetRawDest=Sim.ActiveActor.Position;
                                                             stuckSim.Detections=(1);
                                                   goto DestSet;
                                                          }
                                                          //-------------------------
                                                        DestSet:{}
                                                          }
                                               Vector3 resetValidatedDest;
                                                                      Vector3 forward=sim.ForwardVector;
                                             if(sim.SimDescription.IsHorse){
                                 FindGoodLocationBooleans fglBooleans=FindGoodLocationBooleans.Routable|
                                                                      FindGoodLocationBooleans.PreferEmptyTiles|
                                                                      FindGoodLocationBooleans.AllowOnSlopes|
                                                                      //FindGoodLocationBooleans.AllowIntersectionWithPlatformWalls|
                                                                      //FindGoodLocationBooleans.AllowInFrontOfDoors          |
                                                                      //FindGoodLocationBooleans.AllowOnStairTopAndBottomTiles|
                                                                      FindGoodLocationBooleans.AllowOffLot        |
                                                                      FindGoodLocationBooleans.AllowOnStreets     |
                                                                      FindGoodLocationBooleans.AllowOnBridges     |
                                                                      FindGoodLocationBooleans.AllowOnSideWalks   ;
                                                          if(stuckSim!=null){
                                                          if(stuckSim.Detections<=5){
if(!GlobalFunctions.FindGoodLocationNearbyOnLevel(sim,sim.Level,ref resetRawDest,ref forward,fglBooleans)){
    GlobalFunctions.FindGoodLocationNearbyOnLevel(sim,sim.Level,ref resetRawDest,ref forward,FindGoodLocationBooleans.None);
}
                                                          }
                                                          }
World.FindGoodLocationParams fglParams=new World.FindGoodLocationParams(resetRawDest);
                             fglParams.BooleanConstraints=fglBooleans;
if(!GlobalFunctions.FindGoodLocation(sim,fglParams,out resetValidatedDest,out forward)){
                                         fglParams.BooleanConstraints=FindGoodLocationBooleans.None;
    GlobalFunctions.FindGoodLocation(sim,fglParams,out resetValidatedDest,out forward);
}
                                             }else{
World.FindGoodLocationParams fglParams=new World.FindGoodLocationParams(resetRawDest);
                             fglParams.BooleanConstraints=FindGoodLocationBooleans.Routable|
                                                          FindGoodLocationBooleans.PreferEmptyTiles|
                                                          FindGoodLocationBooleans.AllowOnSlopes|
                                                          FindGoodLocationBooleans.AllowIntersectionWithPlatformWalls|
                                                          FindGoodLocationBooleans.AllowInFrontOfDoors          |
                                                          FindGoodLocationBooleans.AllowOnStairTopAndBottomTiles|
                                                          FindGoodLocationBooleans.AllowOffLot        |
                                                          FindGoodLocationBooleans.AllowOnStreets     |
                                                          FindGoodLocationBooleans.AllowOnBridges     |
                                                          FindGoodLocationBooleans.AllowOnSideWalks   ;
if(!GlobalFunctions.FindGoodLocation(sim,fglParams,out resetValidatedDest,out forward)){
                                         fglParams.BooleanConstraints=FindGoodLocationBooleans.None;
    GlobalFunctions.FindGoodLocation(sim,fglParams,out resetValidatedDest,out forward);
}
                                             }
                                                                 if(!deep){
                      if(sim.InteractionQueue!=null&&sim.InteractionQueue.mInteractionList!=null){
InteractionInstance 
      currentInteraction;
  if((currentInteraction=sim.InteractionQueue.GetCurrentInteraction())!=null){
                         //
                         sim.InteractionQueue.CancelInteraction(currentInteraction.Id,ExitReason.CanceledByScript);
  }
                      }
                                                                 }
                                       sim.SetPosition(resetValidatedDest);
                                                               sim.SetForward(forward);
                                                sim.RemoveFromWorld();
                                                      if(addToWorld||deep){
                                                                 if(!deep){
                try{
                                                sim.Posture?.CancelPosture(sim);
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source);
                }
                                                                 }
                                                sim.     AddToWorld();
                                                    sim.SetHiddenFlags(HiddenFlags.Nothing);
                                                        sim.SetOpacity(1f,0f);
                                                      }else{
                                                    sim.SetHiddenFlags(HiddenFlags.Everything);
                                                        sim.SetOpacity(0f,0f);
                                                      }
                                       sim.SimRoutingComponent?.ForceUpdateDynamicFootprint();
                                                          if(stuckSim!=null){
                                                             stuckSim.Resetting=(false);//  Pode detectar novos eventos Stuck
                                                          }
                                             }
        }
               static Dictionary<ulong,StuckSimData>         StuckSims=new Dictionary<ulong,StuckSimData>();
        protected class StuckSimData{
        const                  string                        _CLASS_NAME=".Alive.StuckSimData.";
        public                 int                           Detections=0;
        public                 bool                          Resetting=(false);
        public                 Vector3                          PositionPreceding=Vector3.Invalid;
        public                 long                             PositionPrecedingTicks=0;
        public                 InteractionInstance           InteractionPreceding;
                 public StuckSimData(){}
            public ResetStuckSimTask resetTask=null;
        }
        //==================================================================================================================
        static void RecoverMissingSims(){
                try{
                      List<Sim>sims=new List<Sim>(LotManager.Actors);
            foreach(Sim sim in sims){
                    try{
                    SimDescription simDesc=sim.SimDescription;
                                if(simDesc==null)continue;
                   if((!sim.SimDescription.IsValidDescription)||(sim.Household==null)){
                        try{
                   if(SimIsMissing(sim.SimDescription,true)){
                                   simDesc.Fixup();
                        sim.Autonomy?.Motives?.RecreateMotives(sim);
                        sim.SetObjectToReset();
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
                        }finally{
                        }
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
                    }finally{
                    }
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
                }finally{
                }
                try{
                        List<Household>houses=new List<Household>(Household.sHouseholdList);
            foreach(Household house in houses){
                    try{
                           if(house.LotHome==null)continue;
                 var numSims=(house.AllSimDescriptions.Count);
            List<Sim>allSims=new List<Sim>();
foreach(SimDescription sim in house.AllSimDescriptions){
                    if(sim.CreatedSim==null)continue;
                     allSims.Add(sim.CreatedSim);
}
                  if(numSims!=allSims.Count){
          List<SimDescription>allSimDescriptions=house.AllSimDescriptions;
foreach(SimDescription description in allSimDescriptions){
    bool flag=( true);
  foreach(Sim sim in allSims){
           if(sim.SimDescription==description){
         flag=(false);
            break;
           }
  }
      if(flag){
                  FixInvisibleSim(description);
                RecoverMissingSim(description,true);
      }
}
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
                    }finally{
                    }
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
                }finally{
                }
        }
        public static bool SimIsMissing(SimDescription sim,bool ignorePlaceholders){
            if((!ignorePlaceholders)&&(FindPlaceholderForSim(sim)!=null)){
                return(false);
            }
            else 
            if(sim.IsEnrolledInBoardingSchool()){
                return(false);
            }
            else 
            if((ParentsLeavingTownSituation.Adults!=null)&&
               (ParentsLeavingTownSituation.Adults.Contains(sim.SimDescriptionId))){
                return(false);
            }
            else 
            if(GameStates.IsOnVacation){
                if(GameStates.sTravelData.mEarlyDepartureIds!=null){
                    if(GameStates.sTravelData.mEarlyDepartureIds.Contains(sim.SimDescriptionId)){
                return(false);
                    }
                }
                if(GameStates.sTravelData.mEarlyDepartures!=null){
                    foreach(Sim member in GameStates.sTravelData.mEarlyDepartures){
                             if(member.SimDescription==sim){
                return(false);
                             }
                    }
                }
            }
            else 
            if(sim.HasFlags(SimDescription.FlagField.IsTravelingForPassport)||
               sim.HasFlags(SimDescription.FlagField.IsAwayForPassport)){
                return(false);
            }
                return( true);
        }
        public static Sim.Placeholder FindPlaceholderForSim(SimDescription simDesc){
                                                                        if(simDesc.LotHome!=null){
              foreach(Sim.Placeholder placeholder in simDesc.LotHome.GetObjects<Sim.Placeholder>()){
                                   if(placeholder.SimDescription==simDesc){
                               return placeholder;
                                   }
              }
                                                                        }
            return null;
        }
        public static void RecoverMissingSim(SimDescription sim,bool ignorePlaceholders){
                try{
                                           if(!SimIsMissing(sim,ignorePlaceholders))return;
                                                         if(sim.CreatedSim==null){  return;}
                                                                                               StuckSimData stuckSim;
                                                         if(!StuckSims.TryGetValue(sim.SimDescriptionId,out stuckSim)){
                                                                                                            stuckSim=new StuckSimData();
                                                             StuckSims.Add(        sim.SimDescriptionId,    stuckSim);
                                                         }
                                                                                                        if(!stuckSim.Resetting){
                                                                                                            stuckSim.Detections++;
                                                                         Vector3 destination=Vector3.Invalid;
                                                               if(sim.CreatedSim.RoutingComponent!=null){
                                                                  sim.CreatedSim.RoutingComponent.GetDestination(out destination);
                                                               }
                    if(  stuckSim.resetTask==null)
                         stuckSim.resetTask=new ResetStuckSimTask(sim.CreatedSim,destination,"Missing");
                    else stuckSim.resetTask.Renew();
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
                }finally{
                }
        }
        public static void   FixInvisibleSim(SimDescription sim,bool force=false,bool reset=false){
                try{
                OutfitCategories[]categoriesArray=null;
                                                   switch(sim.Species){
                                                    case CASAgeGenderFlags.Human:
                                  categoriesArray=new OutfitCategories[]{OutfitCategories.Everyday  ,
                                                                         OutfitCategories.Naked     ,
                                                                         OutfitCategories.Athletic  ,
                                                                         OutfitCategories.Formalwear,
                                                                         OutfitCategories.Sleepwear ,
                                                                         OutfitCategories.Swimwear  
                                  };
                                                        break;
                                                    case CASAgeGenderFlags.Horse:
                                  categoriesArray=new OutfitCategories[]{OutfitCategories.Everyday  ,
                                                                         OutfitCategories.Naked     ,
                                                                         OutfitCategories.Racing    , 
                                                                         OutfitCategories.Bridle    , 
                                                                         OutfitCategories.Jumping   
                                  };
                                                        break;
                                                    default:
                                  categoriesArray=new OutfitCategories[]{OutfitCategories.Everyday,
                                                                         OutfitCategories.Naked 
                                  };
                                                        break;
                                                   }
                                                    bool necessary=force;
                                                     if(!necessary){
         foreach(OutfitCategories category in categoriesArray){
                                                       if(sim.IsHuman){
                               if(category==OutfitCategories.Naked)continue;
                                                       }
                                        SimOutfit outfit2=sim.GetOutfit(category,0);
                                              if((outfit2==null)||(!outfit2.IsValid)){
                                                         necessary=true;
                                              }
         }
                                                     }
                                                     if(!necessary){
                                                        return;
                                                     }
          SimOutfit sourceOutfit=null;
                for(int i=0;i<2;i++){
                    OutfitCategoryMap map=null;
                     if(i==0){
                                      map=sim.mOutfits;
                     }else{
                                      map=sim.mMaternityOutfits;
                     }
                                   if(map==null)continue;
                    foreach(OutfitCategories category in Enum.GetValues(typeof(OutfitCategories))){
                                          if(category==OutfitCategories.Supernatural)continue;
                       ArrayList outfits=map[category] as ArrayList;
                              if(outfits==null)continue;
  foreach(SimOutfit anyOutfit in outfits){
                if((anyOutfit!=null)&&(anyOutfit.IsValid)){
       sourceOutfit=anyOutfit;
                                break;
                }
  }
                    }
                }
                SimBuilder builder=new SimBuilder();
                           builder.UseCompression=true;
                            var simTone=sim.SkinToneKey;
            List<ResourceKey>choiceTones=new List<ResourceKey>();
                         KeySearch tones=new KeySearch(0x0354796a);
       foreach(ResourceKey tone in tones){
           choiceTones.Add(tone);
       }
                                   tones.Reset();
                            if((simTone.InstanceId==0)||(!choiceTones.Contains(simTone))){
                                simTone=RandomUtil.GetRandomObjectFromList(choiceTones);
                            }
                    ResourceKey newTone=simTone;

                           builder.Age          =sim.Age          ;
                           builder.Gender       =sim.Gender       ;
                           builder.Species      =sim.Species      ;
                           builder.SkinTone     =newTone          ;
                           builder.SkinToneIndex=sim.SkinToneIndex;
                           builder.MorphFat     =sim.mCurrentShape.Fat ;
                           builder.MorphFit     =sim.mCurrentShape.Fit ;
                           builder.MorphThin    =sim.mCurrentShape.Thin;
GeneticsPet.SpeciesSpecificData speciesData=OutfitUtils.GetSpeciesSpecificData(sim);
                try{
                 if(sourceOutfit!=null){
foreach(SimOutfit.BlendInfo blend in sourceOutfit.Blends){
                           builder.SetFacialBlend(blend.key,blend.amount);
}
CASParts.OutfitBuilder.CopyGeneticParts(builder,sourceOutfit);
                 }else{
                                                       if(sim.Genealogy!=null){
List<SimDescription>     parents=new List<SimDescription>();
List<SimDescription>grandParents=new List<SimDescription>();
                                                       if(sim.Genealogy.Parents!=null){
                                foreach(Genealogy gene in sim.Genealogy.Parents){
                          SimDescription relative=gene.SimDescription;
                                      if(relative==null)continue;
                         parents.Add(relative);
                                      if(relative.Genealogy!=null){
                                      if(relative.Genealogy.Parents!=null){
          foreach(Genealogy grandGene in relative.Genealogy.Parents){
          var grandRelative=grandGene.SimDescription;
           if(grandRelative==null)continue;
                    grandParents.Add(grandRelative);
          }
                                      }
                                      }
                                }
                                                       }
                      if(parents.Count>0){
                                                       if(sim.IsHuman){
Genetics.InheritFacialBlends(    builder,parents.ToArray(),new Random());
                                                       }else{
GeneticsPet.InheritBodyShape(    builder,parents,grandParents,new Random());
GeneticsPet.InheritBasePeltLayer(builder,parents,grandParents,new Random());
GeneticsPet.InheritPeltLayers(   builder,parents,grandParents,new Random());
                                                       }
                      }
                                                       }
                 }
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source);
                }
                if(sim.IsRobot){
OutfitUtils.AddMissingPartsBots(builder,(OutfitCategories)0x200002,true,sim);
Sleep();
OutfitUtils.AddMissingPartsBots(builder, OutfitCategories.Everyday,true,sim);
Sleep();
                }else 
                if(sim.IsHuman){
OutfitUtils.AddMissingParts(builder,(OutfitCategories)0x200002,true,sim,sim.IsAlien);
Sleep();
OutfitUtils.AddMissingParts(builder, OutfitCategories.Everyday,true,sim,sim.IsAlien);
Sleep();
                }else{
OutfitUtils.AddMissingPartsPet(builder,OutfitCategories.Everyday|(OutfitCategories)0x200000,true,sim,speciesData);
Sleep();
OutfitUtils.AddMissingPartsPet(builder,OutfitCategories.Everyday                           ,true,sim,speciesData);
Sleep();
                }
                                ResourceKey uniformKey=new ResourceKey();
                if(sim.IsHuman){
        if(LocaleConstraints.GetUniform(ref uniformKey,sim.HomeWorld,builder.Age,builder.Gender,OutfitCategories.Everyday)){
OutfitUtils.SetOutfit(builder,new SimOutfit(uniformKey),sim);
        }
                }
OutfitUtils.SetAutomaticModifiers(builder);
                   sim.ClearOutfits(OutfitCategories.Career     ,false);
                   sim.ClearOutfits(OutfitCategories.MartialArts,false);
                   sim.ClearOutfits(OutfitCategories.Special    ,false);
         foreach(OutfitCategories category in categoriesArray){
    ArrayList outfits=null;
                if(!force){
              outfits=sim.Outfits[category] as ArrayList;
           if(outfits!=null){
    int index=0;
  while(index<outfits.Count){
                    SimOutfit anyOutfit=outfits[index] as SimOutfit;
                           if( anyOutfit==null){
              outfits.RemoveAt(index);
                           }else 
                           if(!anyOutfit.IsValid){
              outfits.RemoveAt(index);
                           }else{
                               index++;
                           }
  }
           }
                }

          if((outfits==null)||(outfits.Count==0)){
OutfitUtils.MakeCategoryAppropriate(builder,category,sim);
                if(sim.IsHuman){
        if(LocaleConstraints.GetUniform(ref uniformKey,sim.HomeWorld,builder.Age,builder.Gender,category)){
OutfitUtils.SetOutfit(builder,new SimOutfit(uniformKey),sim);
        }
                }
                   sim.RemoveOutfits(category,false);
CASParts.AddOutfit(sim,category,builder,true);
          }
                if(sim.IsUsingMaternityOutfits){
                   sim.BuildPregnantOutfit(category);
                }
         }
                if(sim.IsMummy){
OccultMummy       
          .OnMerge(sim);
                }else 
                if(sim.IsFrankenstein){
OccultFrankenstein
          .OnMerge(sim,sim.OccultManager.mIsLifetimeReward);
                }else 
                if(sim.IsGenie){
OccultGenie
   .OverlayUniform(sim,OccultGenie.CreateUniformName(sim.Age,sim.Gender),ProductVersion.EP6,OutfitCategories.Everyday,CASSkinTones.BlueSkinTone,0.68f);
                }
                else 
                if(sim.IsImaginaryFriend){
OccultImaginaryFriend friend=sim.OccultManager.GetOccultType(Sims3.UI.Hud.OccultTypes.ImaginaryFriend) as OccultImaginaryFriend;
OccultBaseClass
   .OverlayUniform(sim,OccultImaginaryFriend.CreateUniformName(sim.Age,friend.Pattern),ProductVersion.EP4,OutfitCategories.Special,CASSkinTones.NoSkinTone,0f);
                }
                if(sim.IsMermaid){
OccultMermaid
       .AddOutfits(sim,null);
                }
                if(sim.IsWerewolf){
                if(sim.ChildOrAbove){
            SimOutfit newWerewolfOutfit=
OccultWerewolf
.GetNewWerewolfOutfit(sim.Age,sim.Gender);
                   if(newWerewolfOutfit!=null){
                      sim.AddOutfit(newWerewolfOutfit,OutfitCategories.Supernatural,0x0);
                   }
                }
                }
                      SimOutfit currentOutfit=null;
                   if(sim.CreatedSim!=null){
                    if(reset){
                                                                                               StuckSimData stuckSim;
                                                         if(!StuckSims.TryGetValue(sim.SimDescriptionId,out stuckSim)){
                                                                                                            stuckSim=new StuckSimData();
                                                             StuckSims.Add(        sim.SimDescriptionId,    stuckSim);
                                                         }
                                                                                                        if(!stuckSim.Resetting){
                                                                                                            stuckSim.Detections++;
                                                                         Vector3 destination=Vector3.Invalid;
                                                               if(sim.CreatedSim.RoutingComponent!=null){
                                                                  sim.CreatedSim.RoutingComponent.GetDestination(out destination);
                                                               }
                    if(  stuckSim.resetTask==null)
                         stuckSim.resetTask=new ResetStuckSimTask(sim.CreatedSim,destination,"Invisible");
                    else stuckSim.resetTask.Renew();
                                                                                                        }
                    }
                    try{
                      sim.CreatedSim.SwitchToOutfitWithoutSpin(Sim.ClothesChangeReason.GoingOutside, OutfitCategories.Everyday, true);
                    }catch(Exception exception){
                      Alive.WriteLog(exception.Message+"\n\n"+
                                     exception.StackTrace+"\n\n"+
                                     exception.Source);
                    }
        currentOutfit=sim.CreatedSim.CurrentOutfit;
                }else{
        currentOutfit=sim.GetOutfit(OutfitCategories.Everyday,0);
                }
     if(currentOutfit!=null){
ThumbnailManager.GenerateHouseholdSimThumbnail(currentOutfit.Key,currentOutfit.Key.InstanceId,0x0,ThumbnailSizeMask.Large|ThumbnailSizeMask.ExtraLarge|ThumbnailSizeMask.Medium|ThumbnailSizeMask.Small,ThumbnailTechnique.Default,true,false,sim.AgeGenderSpecies);
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
                           flag=(false);
                }
                    return(flag );
        }
        public static bool Sleep(){
                    return Sleep(0);
        }
        //==================================================================================================================
    }
    public class AlarmTask{
          public AlarmTask(float time,
                        TimeUnit timeUnit,
                        Sims3.Gameplay.Function func):this(func){
                   handle=AlarmManager.Global.AddAlarm(mTime    =time,
                                                       mTimeUnit=timeUnit,
                                                     OnTimer,
                                                     "ArthurGibraltarSims3Mod_Once",AlarmType.NeverPersisted,null);
                                              disposeOnTimer=true;
          }
                                                 float mTime;
                                              TimeUnit mTimeUnit;
            public void Renew(){
Simulator.DestroyObject(runningTask);
                        runningTask=ObjectGuid.InvalidObjectGuid;
                   handle=AlarmManager.Global.AddAlarm(mTime,
                                                       mTimeUnit,
                                                     OnTimer,
                                                     "ArthurGibraltarSims3Mod_Once",AlarmType.NeverPersisted,null);
                                              disposeOnTimer=true;
            }
          public AlarmTask(float time,
                        TimeUnit timeUnit,
                        Sims3.Gameplay.Function func,
                     float repeatTime,
                  TimeUnit repeatTimeUnit):this(func){
                   handle=AlarmManager.Global.AddAlarmRepeating(time,
                                                                timeUnit,
                                                              OnTimer,
                                                          repeatTime,
                                                          repeatTimeUnit,
                                                          "ArthurGibraltarSims3Mod_Repeat",AlarmType.NeverPersisted,null);
          }
          public AlarmTask(float hourOfDay,
                         DaysOfTheWeek days,
                         Sims3.Gameplay.Function func):this(func){
                   handle=AlarmManager.Global.AddAlarmDay(hourOfDay,
                                                                days,
                                                        OnTimer,"ArthurGibraltarSims3Mod_Daily",AlarmType.NeverPersisted,null);
          }
       AlarmHandle handle;
       protected AlarmTask(Sims3.Gameplay.Function func){
                                       AllScheduledTasks.Add(this);
                                                if(func==null){
                                                   func=OnPerform;
                                                }
                                     AlarmFunction=func;
       }
    readonly Sims3.Gameplay.Function AlarmFunction;
        ObjectGuid runningTask=ObjectGuid.InvalidObjectGuid;
         bool disposeOnTimer;
        private void OnTimer(){
                try{
           if(disposeOnTimer){
                          AlarmManager.Global.RemoveAlarm(handle);
                                                          handle=(AlarmHandle.kInvalidHandle);
           }
                   runningTask=ModTask.Perform(AlarmFunction);
           if(disposeOnTimer){
                    //
           }
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source);
                }finally{
                }
        }
                              protected virtual void OnPerform(){/*Alive.WriteLog("perform:empty_0");*/}
        readonly static List<AlarmTask>AllScheduledTasks=new List<AlarmTask>();
  public void Dispose(){
Simulator.DestroyObject(runningTask);
                        runningTask=ObjectGuid.InvalidObjectGuid;
                                                       if(handle!=AlarmHandle.kInvalidHandle){
                          AlarmManager.Global.RemoveAlarm(handle);
                                                          handle=(AlarmHandle.kInvalidHandle);
                                                       }
                                       AllScheduledTasks.Remove(this);
  }
public static 
         void DisposeAll(){
                    List<AlarmTask>tasks=new List<AlarmTask>(AllScheduledTasks);
         foreach(AlarmTask task in tasks){
                           task.Dispose();
         }
}
    }
        public class DeepFixSimTask:ModTask{
              public DeepFixSimTask(SimDescription sim){
               mSim=sim;
              }
SimDescription mSim=null;
            protected override void OnPerform(){
               Alive.DeepFixSim(mSim);
            }
        }
    public class ModTask:Task{
       protected ModTask(){
                                     ModTaskFunction=OnPerform;
       }
       protected ModTask(Sims3.Gameplay.Function func){
                                     ModTaskFunction=func;
       }
    readonly Sims3.Gameplay.Function ModTaskFunction;
                              protected virtual void OnPerform(){/*Alive.WriteLog("perform:empty_1");*/}
                              public static ObjectGuid Perform(Sims3.Gameplay.Function func){
      return new ModTask(func).AddToSimulator();
                              }
             public ObjectGuid AddToSimulator(){return Simulator.AddObject(this);}
        public override void Simulate(){
            try{
                                     ModTaskFunction();
            }catch(ResetException exception){
                   Alive.WriteLog(exception.Message+"\n\n"+
                                  exception.StackTrace+"\n\n"+
                                  exception.Source);
            }catch(     Exception exception){
                   Alive.WriteLog(exception.Message+"\n\n"+
                                  exception.StackTrace+"\n\n"+
                                  exception.Source);
            }finally{
                                                       Simulator.DestroyObject(ObjectId);                    
            }
        }
        public override string ToString(){
                                  if(ModTaskFunction==null){
            return"_(ModTaskOnce):null_function_";
                                  }else{
            return("_(ModTaskOnce)_function_Method:"+this.ModTaskFunction.Method.ToString()+"_,_DeclaringType:"+this.ModTaskFunction.Method.DeclaringType.ToString());
                                  }
        } 
    }
}