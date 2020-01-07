using System;
using System.Collections.Generic;
using System.Text;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
namespace ArthurGibraltarSims3Mod{
    public class Alive{
        [Tunable]
        protected static bool kIExistNow=(false);
          static Alive(){
            World.sOnWorldLoadFinishedEventHandler+=OnWorldLoadFinished;
            World.sOnWorldQuitEventHandler        +=OnWorldQuit;
          }
        private static void OnWorldLoadFinished(object sender,EventArgs e){
             new AlarmTask(5,DaysOfTheWeek.All,AutoPause);
             //---------------------------------------------------------------
             new AlarmTask(1,TimeUnit.Hours,CheckShowVenues,1,TimeUnit.Hours);
        }
        private static void OnWorldQuit(object sender,EventArgs e){
    AlarmTask.DisposeAll();
        }
        static void AutoPause(){
      Sims3.Gameplay.Gameflow.SetGameSpeed(Gameflow.GameSpeed.Pause,Sims3.Gameplay.Gameflow.SetGameSpeedContext.GameStates);
        }
        static void CheckShowVenues(){
                 foreach(ShowVenue show in Sims3.Gameplay.Queries.GetObjects<ShowVenue>()){
                 }
        }
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