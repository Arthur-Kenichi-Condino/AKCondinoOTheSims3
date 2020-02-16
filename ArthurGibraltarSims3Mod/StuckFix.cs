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
using Sims3.Gameplay.ObjectComponents;
using Sims3.Gameplay.Objects;
using Sims3.Gameplay.Objects.Appliances;
using Sims3.Gameplay.Objects.Beds;
using Sims3.Gameplay.Objects.Electronics;
using Sims3.Gameplay.Objects.Elevator;
using Sims3.Gameplay.Objects.Environment;
using Sims3.Gameplay.Objects.FoodObjects;
using Sims3.Gameplay.Objects.Lighting;
using Sims3.Gameplay.Objects.Miscellaneous;
using Sims3.Gameplay.Objects.Plumbing;
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
    public partial class Alive{
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
                                                          if(stuckSim.Detections<=5||sim.SimDescription.IsBonehilda){
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
        static void ResetPortalsAndRouting(){
                try{
                Sim.kAutonomyThoughtBalloonPercentageChance=100f;
                Sim.kDistanceForSocials=15f;
                Sim.kMaxDistanceForSocials=75f;
                Sim.kBabyToddlerLikingThresholdToAllowInteractionsWithStrangers=-101;
                Sim.kMaxStandBackFromDoorDistance=16f;
                Sim.kPortalIntersectRadius=2f;
                Sim.kPortalPushMinDistance=0f;
                Sim.kPortalPushMaxDistance=16f;
                Sim.kIdleTimeAmount=1f;
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
                                Autonomy.kRandomness=.027f;
                                //------------------------------------------------------
                                Autonomy.kPreferSeatedSocialsMultiplier=1.5f;
                                Autonomy.kSocialThatWillCauseDismountMultiplier=.025f;
                                //------------------------------------------------------
                                Autonomy.kAllowEvenIfNotAllowedInRoomAutonomousMultiplier=0.5f;
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
                                SocialInteraction.kSocialRouteRadiusMax=6;
                                SocialInteraction.kSocialRouteMinDist=10;
                                //------------------------------------------------------
                                SocialInteraction.kSocialTimeoutTime=45;
                                //------------------------------------------------------
                                SocialInteraction.kSocialJigPlacementLimit=30f;
                                SocialInteraction.kSocialSyncGiveupTime=45;
                                //------------------------------------------------------
                                SocialInteraction.kSocialBumpDistMin=0.175f;
                                SocialInteraction.kSocialBumpDistMax=1.0f;
                                //------------------------------------------------------
                                SocialInteraction.kApproachGreetDistance=6.0f;
                                //------------------------------------------------------
                }
            catch(Exception exception){
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
    }
}