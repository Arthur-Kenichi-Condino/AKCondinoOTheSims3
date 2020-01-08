using System;
using System.Collections.Generic;
using System.Text;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.CAS;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.Objects;
using Sims3.Gameplay.Objects.Elevator;
using Sims3.Gameplay.Objects.Lighting;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
namespace ArthurGibraltarSims3Mod{
    public class Alive{
        [Tunable]
        protected static bool kIExistNow=(false);
          static Alive(){
            LoadSaveManager.ObjectGroupsPreLoad+=OnPreLoad;
            World.sOnWorldLoadFinishedEventHandler+=OnWorldLoadFinished;
            World.sOnWorldQuitEventHandler        +=OnWorldQuit;
          }
        private static void OnPreLoad(){
            Route.AboutToPlanCallback+=OnAboutToPlan;
            Route.   PostPlanCallback+=   OnPostPlan;
        }
        private static void OnWorldLoadFinished(object sender,EventArgs e){
             //---------------------------------------------------------------
            foreach(var tuning in InteractionTuning.sAllTunings.Values){
                     if(tuning.FullInteractionName=="Sims3.Gameplay.Objects.Gardening.Plant+Graaiins+Definition"){
                        tuning.AddFlags(InteractionTuning.FlagField.DisallowAutonomous);
                     }
                     if(tuning.FullInteractionName=="Sims3.Gameplay.Services.ResortMaintenance+AutonomousSweep+Definition"){
                        tuning.AddFlags(InteractionTuning.FlagField.DisallowAutonomous);
                     }
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
             new AlarmTask(1,TimeUnit.Hours,CheckShowVenues,1,TimeUnit.Hours);
             //---------------------------------------------------------------
             new AlarmTask(1,TimeUnit.Hours,RecoverElevator,1,TimeUnit.Hours);
             //---------------------------------------------------------------
        }
        private static void OnWorldQuit(object sender,EventArgs e){
    AlarmTask.DisposeAll();
        }
        //==================================================================================================================
        static void AutoPause(){
      Sims3.Gameplay.Gameflow.SetGameSpeed(Gameflow.GameSpeed.Pause,Sims3.Gameplay.Gameflow.SetGameSpeedContext.GameStates);
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
        }
        //==================================================================================================================
       static readonly Dictionary<ShowVenue,ShowDetectedData>ShowDetected=new Dictionary<ShowVenue,ShowDetectedData>();
        static void CheckShowVenues(){
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
        }
        protected class ShowDetectedData{
                 public ShowDetectedData(long showStartTimeTicks){
                           ShowStartTimeTicks=showStartTimeTicks;
                 }
            public        readonly long                          ShowStartTimeTicks;
        }
        //==================================================================================================================
        static void RecoverElevator(){
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
        }
        //==================================================================================================================
        protected static void OnAboutToPlan(Route r,string routeType,Vector3 point){
 Sims3.SimIFace.Route.SetAvoidanceFieldRangeScale(r.Follower.ObjectId,0);
 Sims3.SimIFace.Route.SetAvoidanceFieldSmoothing( r.Follower.ObjectId,0);
                                                  r.CanPlayReactionsAtEndOfRoute=(false);
        }
        protected static void    OnPostPlan(Route r,string routeType,string result){
                                                  r.CanPlayReactionsAtEndOfRoute=(false);
                                              if(!r.PlanResult.Succeeded()){
                                              }
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
                   runningTask=ModTask.Perform(AlarmFunction);
                }catch(Exception exception){
                }finally{
           if(disposeOnTimer){
              Dispose();
           }
                }
        }
        private void OnPerform(){}
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
                              protected virtual void OnPerform(){}
                              public static ObjectGuid Perform(Sims3.Gameplay.Function func){
      return new ModTask(func).AddToSimulator();
                              }
             public ObjectGuid AddToSimulator(){return Simulator.AddObject(this);}
        public override void Simulate(){
            try{
                                     ModTaskFunction();
            }catch(ResetException exception){
            }catch(     Exception exception){
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