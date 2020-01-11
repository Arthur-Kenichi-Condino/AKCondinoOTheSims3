using System;
using System.Collections.Generic;
using System.Text;
using Sims3.Gameplay;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.ActiveCareer.ActiveCareers;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.CAS;
using Sims3.Gameplay.Controllers;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Objects;
using Sims3.Gameplay.Objects.Elevator;
using Sims3.Gameplay.Objects.Lighting;
using Sims3.Gameplay.Passport;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
namespace ArthurGibraltarSims3Mod{
    public class Alive{
        const                  string                        _CLASS_NAME=".Alive.";
        [Tunable]
        protected static bool kIExistNow=(false);
          static Alive(){
            LoadSaveManager.ObjectGroupsPreLoad+=OnPreLoad;
            World.sOnWorldLoadFinishedEventHandler+=OnWorldLoadFinished;
            World.sOnWorldQuitEventHandler        +=OnWorldQuit;
          }
        private static void OnPreLoad(){
        }
        private static void OnWorldLoadFinished(object sender,EventArgs e){
            Route.AboutToPlanCallback+=OnAboutToPlan;
            Route.   PostPlanCallback+=   OnPostPlan;
             //---------------------------------------------------------------
                try{
            foreach(var tuning in InteractionTuning.sAllTunings.Values){
                     if(tuning.FullInteractionName=="Sims3.Gameplay.Objects.Gardening.Plant+Graaiins+Definition"){
                        tuning.AddFlags(InteractionTuning.FlagField.DisallowAutonomous);
                     }
                     if(tuning.FullInteractionName=="Sims3.Gameplay.Services.ResortMaintenance+AutonomousSweep+Definition"){
                        tuning.AddFlags(InteractionTuning.FlagField.DisallowAutonomous);
                     }
            }
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source);
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
             new AlarmTask(5,DaysOfTheWeek.All,AutoPause);
             //---------------------------------------------------------------
             new AlarmTask(1,TimeUnit.Hours,CheckShowVenues       ,1,TimeUnit.Hours);
             //---------------------------------------------------------------
             new AlarmTask(1,TimeUnit.Hours,ResetPortalsAndRouting,1,TimeUnit.Hours);
             //---------------------------------------------------------------
        }
        private static void OnWorldQuit(object sender,EventArgs e){
            Route.AboutToPlanCallback-=OnAboutToPlan;
            Route.   PostPlanCallback-=   OnPostPlan;
    AlarmTask.DisposeAll();
        }
        //==================================================================================================================
        static void AutoPause(){
                try{
      Sims3.Gameplay.Gameflow.SetGameSpeed(Sims3.Gameplay.Gameflow.GameSpeed.Pause,Sims3.Gameplay.Gameflow.SetGameSpeedContext.GameStates);
                       foreach(ShowVenue showVenue in Sims3.Gameplay.Queries.GetObjects<ShowVenue>()){
           foreach(ISearchLight light in showVenue.LotCurrent.GetObjects<ISearchLight>()){
                                light.TurnOff();
        SearchLight searchLight=light as SearchLight;
                 if(searchLight!=null){
                    searchLight.mSMC?.Dispose();
                    searchLight.mSMC=null;
                 }
           }
                                         showVenue.EndPlayerConcert();
                       }
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
                                 exception.Source);
                }finally{
                }
        }
        //==================================================================================================================
       static readonly Dictionary<ShowVenue,ShowDetectedData>ShowDetected=new Dictionary<ShowVenue,ShowDetectedData>();
        static void CheckShowVenues(){
                try{
                 foreach(ShowVenue showVenue in Sims3.Gameplay.Queries.GetObjects<ShowVenue>()){
                                if(showVenue.ShowInProgress||
                                   showVenue.ShowType!=ShowVenue.ShowTypes.kNoShow){
                                                         if(!ShowDetected.ContainsKey(showVenue)){
                                                             ShowDetected.Add(        showVenue,new ShowDetectedData(SimClock.CurrentTicks));
                                                         }
                                }
                 }
List<KeyValuePair<ShowVenue,ShowDetectedData>>toRemove=new List<KeyValuePair<ShowVenue,ShowDetectedData>>();
                             foreach(var showDetectedData in ShowDetected){
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
                             }
                                for(int i=0;i<toRemove.Count;i++){
                          ShowDetected.Remove(toRemove[i].Key);
                                }
                                              toRemove.Clear();
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source);
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
                                Autonomy.kAllowEvenIfNotAllowedInRoomAutonomousMultiplier=0;
                                Autonomy.kAutonomyDelayNormal           =0;
                                Autonomy.kAutonomyDelayWhileMounted     =0;
                                Autonomy.kAutonomyDelayDuringSocializing=0;
                                SimRoutingComponent.kDefaultStandAndWaitDuration=3f;
                                SimRoutingComponent.kMinimumPostPushStandAndWaitDuration=1f;
                                SimRoutingComponent.kMaximumPostPushStandAndWaitDuration=2f;
                                SimRoutingComponent.kTotalSimMinutesToWaitForSimsToBePushed=1f;
                                //------------------------------------------------------
                                SimRoutingComponent.kAvoidanceReplanCheckFrequencyMin=6;
                                SimRoutingComponent.kAvoidanceReplanCheckFrequencyMax=9;
                                SimRoutingComponent.kAvoidanceReplanCheckOffsetMin=1;
                                SimRoutingComponent.kAvoidanceReplanCheckOffsetMax=3;
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source);
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
                    }
}
                                                    portal.FreeAllRoutingLanes();
                                                 }
                                         //
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
                               portal.FreeAllRoutingLanes();
                            }
                                //
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
                                  stairs.SetObjectToReset();
                   }
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source);
                }finally{
                }
        }
        //==================================================================================================================
        protected static void OnAboutToPlan(Route r,string routeType,Vector3 point){
                try{
 Sims3.SimIFace.Route.SetAvoidanceFieldRangeScale(r.Follower.ObjectId,0.5f);
 Sims3.SimIFace.Route.SetAvoidanceFieldSmoothing( r.Follower.ObjectId,1.0f);
                                                  r.CanPlayReactionsAtEndOfRoute=(false);
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source);
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
                                                                                                  if(SimClock.CurrentTicks-stuckSim.PositionPrecedingTicks>SimClock.kSimulatorTicksPerSimMinute*5){
                                                                                                                                if(!detected){
                                                                                                                           stuckSim.Detections++;
                                                                                                                                }
                                                                                                  }
                                                                                                                        if(stuckSim.Detections>1){
                                                                                                                           stuckSim.Resetting=( true);
                  //Alive.WriteLog("detected_a_stuck_sim:reset:"+sim.Name);
                    new ResetStuckSimTask(sim,r.GetDestPoint(),"Unroutable");
                                                                                                                        }
                                          }
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source);
                }finally{
                }
        }
        protected class ResetStuckSimTask:AlarmTask{
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
        protected static void ResetStuckSim(Sim sim,Vector3 destination,string suffix){
                                             if(sim!=null&&
                                               !sim.HasBeenDestroyed&&
                                                sim.SimDescription!=null){
                  //Alive.WriteLog("sim_is_stuck:reset_sim_in_progress:sim:"+sim.Name+";destination:"+destination);
                                        Lot lot=sim.LotHome;
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
                        !daycare.IsDaycareChild(sim.SimDescription))){
                                                         addToWorld=(false);
                                            }
                                                   goto DestSet;
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
                                                resetRawDest=Sim.ActiveActor.Position;
                                                             stuckSim.Detections=(1);
                                                   goto DestSet;
                                                          }
                                                          //-------------------------
                                                        DestSet:{}
                                                          }
                                               Vector3 resetValidatedDest;
                                                                      Vector3 forward;
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
                                       sim.SetPosition(resetValidatedDest);
                                                               sim.SetForward(forward);
                                                sim.RemoveFromWorld();
                                                      if(addToWorld){
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
                                                                                                         return Interaction.VersionStamp.sNamespace+"."+key;
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
            }catch(     Exception exception){
                   Alive.WriteLog(exception.Message+"\n\n"+
                                  exception.StackTrace+"\n\n"+
                                  exception.Source);
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
                   handle=AlarmManager.Global.AddAlarm(time,
                                                       timeUnit,
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
              Dispose();
           }
                   runningTask=ModTask.Perform(AlarmFunction);
           if(disposeOnTimer){
                        runningTask=ObjectGuid.InvalidObjectGuid;//  After Dispose, which needs to be called before Perform for some reason
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
                          AlarmManager.Global.RemoveAlarm(handle);
                                                          handle=AlarmHandle.kInvalidHandle;
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