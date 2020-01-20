using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.ActiveCareer;
using Sims3.Gameplay.ActiveCareer.ActiveCareers;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Careers;
using Sims3.Gameplay.CAS;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.DreamsAndPromises;
using Sims3.Gameplay.Opportunities;
using Sims3.Gameplay.Roles;
using Sims3.Gameplay.StoryProgression;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using static ArthurGibraltarSims3Mod.SafeStore;

namespace ArthurGibraltarSims3Mod{
    public class SafeStore:IDisposable{
        public enum Flag:uint{
            None              =0x00,
            LoadFixup         =0x01,
            Selectable        =0x02,
            Unselectable      =0x04,
            StoreOpportunities=0x08,
            School            =0x10,
            OnlyAcademic      =0x20,
        }
                          Flag mFlags;
public readonly SimDescription mSim;
                          Role mRole;
                    Occupation mCareer;
                        School mSchool;
OpportunityStore mOpportunities=null;
                          bool mNeverSelectable=false;
          public SafeStore(SimDescription sim,Flag flags){
                                     mSim=sim;
                    mNeverSelectable=mSim.IsNeverSelectable;
                                     mSim.IsNeverSelectable=false;
                                            mFlags=flags;
                                        if((mFlags&Flag.StoreOpportunities)==Flag.StoreOpportunities){
                 mOpportunities=new OpportunityStore(mSim,false);
                                        }
                               mRole=mSim.AssignedRole;
                                               if((flags&Flag.OnlyAcademic)==Flag.OnlyAcademic){
if(!GameUtils.IsUniversityWorld()){
                    //  [NRaas:]Bypass for a removal during OnBecameSelectable()
                             mCareer=mSim.OccupationAsAcademicCareer;
}
                                               }else{
                             mCareer=mSim.Occupation;
                                               }
                          if(mCareer!=null){
                                     mSim.CareerManager.mJob=null;
                          }
                                 if((mSim.CareerManager!=null)&&((flags&Flag.School)==Flag.School)){
                             mSchool=mSim.CareerManager.School;
                          if(mSchool!=null){
                                     mSim.CareerManager.mSchool=null;
                          }
                                 }
                                     mSim.AssignedRole=null;
          }
        public void Dispose(){
                try{
                                     mSim.AssignedRole=mRole;
                                     mSim.IsNeverSelectable=mNeverSelectable;
                                  if(mSim.CelebrityManager!=null){
                                     mSim.CelebrityManager.ScheduleOpportunityCallIfNecessary();
                                  }
              if(mOpportunities!=null){
                 mOpportunities.Dispose();
              }
                         if((mSchool!=null)&&
                                    (mSim.CareerManager!=null)){
                                     mSim.CareerManager.mSchool=mSchool;
                         }
                         if((mCareer!=null)&&
                                    (mSim.CareerManager!=null)&&
                                    (mSim.Occupation==null)){
                                     mSim.CareerManager.mJob=mCareer;
                                  if(mSim.Occupation!=null){
using(BaseWorldReversion reversion=new BaseWorldReversion()){
                           if((mFlags&Flag.LoadFixup   )==Flag.LoadFixup   ){
                                     mSim.Occupation.OnLoadFixup(false);
                           }
                          if(((mFlags&Flag.Selectable  )==Flag.Selectable  )&&
                                   (!mSim.IsNeverSelectable&&
                                     mSim.Household==Household.ActiveHousehold)){
using(SuppressCreateHousehold suppress=new SuppressCreateHousehold()){
                     bool careerLoc=true;
                                  if(mSim.Occupation is Career){
                          careerLoc=(mSim.Occupation.CareerLoc!=null);
                                  }
                       if(careerLoc){
                           FixCareer(mSim.Occupation,false);
                                     mSim.Occupation.OnOwnerBecameSelectable();
                       }
}
                          }else 
                           if((mFlags&Flag.Unselectable)==Flag.Unselectable){
using(SuppressCreateHousehold suppress=new SuppressCreateHousehold()){
                                        //  [NRaas:]Fix for script error in GhostHunter:ApplyCareerSpecificModifiersToXp where
                                        // owner Sim is not null checked and breaks when this is ran during age up.
ActiveCareerLevelStaticData 
                         data=null;
          int experience=0;
                  GhostHunter hunter=null;
                    try{
                              hunter=mSim.Occupation as GhostHunter;
                           if(hunter!=null){
                         data=hunter.GetCurrentLevelStaticDataForActiveCareer();
                      if(data!=null){
              experience=data.DailyPerfectJobBonusExperience;
                         data.DailyPerfectJobBonusExperience=0;
                      }
                           }
                                     mSim.Occupation.OnOwnerBecameUnselectable();
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
                           if(hunter!=null&&
                         data!=null){
                         data.DailyPerfectJobBonusExperience=experience;
                           }
                    }
}
                           }
}

                            mSim.CareerManager.UpdateCareerUI();
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
        public static void FixCareer(Occupation job,bool allowDrop){
                try{
                                             if(job==null)return;
                                             if(job.Coworkers!=null){
                                      for(int i=job.Coworkers.Count-1;i>=0;i--){
                        SimDescription coworker=job.Coworkers[i];
                  if((!IsValidCoworker(coworker,job is School))||(coworker==job.OwnerDescription)){
                                                job.Coworkers.RemoveAt(i);
                  }
                                      }
                                             }
                                                     if((allowDrop)&&(job is Career)){
            bool replace=(false);
                                             if(job.CareerLoc==null){
                 replace=( true);
                                             }else{
                                RabbitHole hole=job.CareerLoc.Owner;
                                        if(hole==null){
                 replace=( true);
                                        }else{
                          RabbitHole proxy=hole.RabbitHoleProxy;
                                  if(proxy==null){
                 replace=( true);
                                  }else{
                                 if((proxy.EnterSlots==null)||(proxy.EnterSlots.Count==0)||
                                    (proxy.ExitSlots ==null)||(proxy.ExitSlots .Count==0)){
                 replace=( true);
                                 }
                                  }
                                        }
                                             }
              if(replace){
                              SimDescription me=job.OwnerDescription;
                       Occupation retiredJob=me.CareerManager.mRetiredCareer;
                try{
                                               CareerLocation location=Sims3.Gameplay.Careers.Career.FindClosestCareerLocation(me,job.Guid);
                                                           if(location!=null){
                                             if(job.CareerLoc!=null){
                                                job.CareerLoc.Workers.Remove(me);
                                             }
                                                job.CareerLoc=location;
                                                              location.Workers.Add(me);
                                                           }else{
                                                job.LeaveJobNow(Career.LeaveJobReason.kJobBecameInvalid);
                                                           }
                }finally{
                                             me.CareerManager.mRetiredCareer=retiredJob;
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
            public static bool IsValidCoworker(SimDescription sim, bool isSchool)
            {
                if (sim == null) return false;

                if (!sim.IsValidDescription) return false;

                if (sim.Household == null) return false;

                if (isSchool)
                {
                    if (sim.CareerManager == null) return false;

                    if (sim.CareerManager.School == null) return false;
                }
                else
                {
                    if (sim.Occupation == null) return false;
                }

                return true;
            }
        public class OpportunityStore:IDisposable{
                                 SimDescription mSim;
OpportunityHistory.OpportunityExportInfo[]mOpportunities;
              public OpportunityStore(SimDescription sim,bool storeForTravel){
                                                mSim=sim;
                                                          if((storeForTravel)&&
                                              (!mSim.NeedsOpportunityImport)&&
                                               (mSim.CreatedSim!=null)&&
                                               (mSim.CreatedSim.OpportunityManager!=null)){
                                                mSim.CreatedSim.OpportunityManager.StoreOpportunitiesForTravel();
                                                          }
                                             if(mSim.NeedsOpportunityImport){
                                          mOpportunities=sim.OpportunityHistory.mCurrentOpportunities;
                                                mSim.OpportunityHistory.mCurrentOpportunities=new OpportunityHistory.OpportunityExportInfo[9];
                                                mSim.NeedsOpportunityImport=(false);
                                             }
              }
            public void Dispose(){
                                      if((mOpportunities!=null)&&
                                               (mSim.OpportunityHistory!=null)){
                                                mSim.OpportunityHistory.mCurrentOpportunities = mOpportunities;
                                                mSim.NeedsOpportunityImport=( true);
                                             if(mSim.CreatedSim!=null){
                                             if(mSim.CreatedSim.mOpportunityManager==null){
                                                mSim.CreatedSim.mOpportunityManager=new OpportunityManager(mSim.CreatedSim);
                                                mSim.CreatedSim.mOpportunityManager.SetupLocationBasedOpportunities();
                                             }
                try{
                            //   [NRaas:]Due to an odd bit of coding at the bottom of AcceptOpportunityFromTravel(), 
                            // the expiration time for non-expirying opportunities is checked
foreach(OpportunityHistory.OpportunityExportInfo info in mSim.OpportunityHistory.GetCurrentOpportunities()){
                                              if(info.ExpirationTime<SimClock.CurrentTime()){
Opportunity opp=OpportunityManager.GetStaticOpportunity(info.Guid);
         if(opp!=null){
           bool requiresTimeout=(false);
     switch(opp.Timeout){
        case Opportunity.OpportunitySharedData.TimeoutCondition.SimDays:
        case Opportunity.OpportunitySharedData.TimeoutCondition.SimHours:
        case Opportunity.OpportunitySharedData.TimeoutCondition.SimTime:
        case Opportunity.OpportunitySharedData.TimeoutCondition.Gig:
        case Opportunity.OpportunitySharedData.TimeoutCondition.AfterschoolRecitalOrAudition:
                requiresTimeout=( true);
            break;
     }
            if(!requiresTimeout){
                                                 info.ExpirationTime=SimClock.Add(SimClock.CurrentTime(),TimeUnit.Hours,1);
            }
         }
                                              }
}
                                                mSim.CreatedSim.OpportunityManager.TravelFixup();
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
                                                mSim.NeedsOpportunityImport=(false);
                                             }
                                      }
            }
        }
    }
    public class HouseholdStore:IDisposable{
                 Household mOldHouse;
      List<SimDescription>mNewSims;
                    Dictionary<Sim,DreamStore>mDreamStore=new Dictionary<Sim,DreamStore>();
          Dictionary<SimDescription,SafeStore>mSafeStore=new Dictionary<SimDescription,SafeStore>();
          public HouseholdStore(Household newHouse,bool catchDreams):this(All(newHouse),catchDreams){}
          public HouseholdStore(IEnumerable<SimDescription> newSims,bool catchDreams){
                                                         if(newSims!=null){
                          mNewSims=new List<SimDescription>(newSims);
                                                         }
                           mOldHouse=PlumbBob.sCurrentNonNullHousehold;
                       if((mOldHouse!=null)&&(catchDreams)){
foreach(Sim member in AllSims(mOldHouse)){
                                   DreamStore element=new DreamStore(member,false,true);
                                              mDreamStore.Add(member,element);
}
                       }
                       if(mNewSims!=null){
foreach(SimDescription member in mNewSims){
Alive.ResetClearSimTask.
   CleanupBrokenSkills(member);
                                              mSafeStore.Add(member,new SafeStore(member,SafeStore.Flag.Selectable|SafeStore.Flag.Unselectable|SafeStore.Flag.StoreOpportunities));
                   if((member.CreatedSim         !=null)&&
                      (member.CreatedSim.Autonomy!=null)){
       Motives motives=member.CreatedSim.Autonomy.Motives;
           if((motives==null)||(motives.GetMotive(CommodityKind.Hunger)==null)){
                       member.CreatedSim.Autonomy.RecreateAllMotives();
           }
                   }
}
                       }
          }
        public void Dispose(){
                       if(mNewSims!=null){
foreach(SimDescription member in mNewSims){
                            SafeStore element=mSafeStore[member];
                                      element.Dispose();
}
                       }
                        if(mOldHouse!=null){
foreach(Sim member in AllSims(mOldHouse)){
                                          if(!mDreamStore.ContainsKey(member))continue;
                                              mDreamStore[member].Restore(member);
}
                        }
        }
            public static ICollection<SimDescription>All(Household house){
                                                                if(house==null)return new List<SimDescription>();
                                                            return house.AllSimDescriptions;
            }
                public static List<Sim> AllSims(Household house){
                              List<Sim>sims=new List<Sim>();
                                                       if(house!=null){
                            foreach(SimDescription sim in house.AllSimDescriptions){
                                                if(sim.CreatedSim==null)continue;
                                       sims.Add(sim.CreatedSim);
                            }
                                                       }
                                return sims;
                }
    }
    public class DreamStore{
                      bool mInitialStore=false;
            SimDescription mSim;
             DnPExportData mDnPExportData;
  DreamsAndPromisesManager mDnpManager;
        OpportunityManager mOpportunityManager;
          OpportunityStore mOppStore;
          public DreamStore(Sim sim,bool initialStore,bool simpleRetain){
                           mInitialStore=initialStore;
                           mSim=sim.SimDescription;
                           mDnPExportData=null;
                  bool storeDnP=(false);
                            if((sim.mDreamsAndPromisesManager             !=null)&&
                               (sim.DreamsAndPromisesManager.mPromiseNodes!=null)){
foreach(ActiveDreamNode node in sim.DreamsAndPromisesManager.mPromiseNodes){
                     if(node!=null){
                       storeDnP=( true);
                        break;
                     }
}
                            }
                    if(storeDnP){
                    OnLoadFixup(sim,false);
                                                        if(simpleRetain){
                           mDnpManager=sim.DreamsAndPromisesManager;
                                       sim.mDreamsAndPromisesManager=null;
                                                        }else{
                try{
                           mDnPExportData=new DnPExportData(mSim);
                                sim.NullDnPManager();
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

                                    if(sim.HasOpportunity()){
            foreach(Opportunity opp in sim.OpportunityManager.List){
                        if(mSim.OpportunityHistory.GetCurrentOpportunity(opp.OpportunityCategory)==null){
                           mSim.OpportunityHistory.AddCurrentOpportunity(opp.OpportunityCategory,
                                                                         opp.Guid,null,
                                                                         opp.TargetObject as Sim,
                                                                         opp.ParentOpportunities,
                                                                         opp.TargetInteractionNumberItemsRequiredList(),
                                                                         opp.Name,
                                                                         opp.Description,
                                                                         opp.DeadlineString);
                        }
                    }
Alive.ResetClearSimTask.
      CleanupOpportunities(mSim,false);
                                                        if(simpleRetain){
                           mOpportunityManager=sim.OpportunityManager;
                                                        }else{
                           mOppStore=new OpportunityStore(sim.SimDescription,true);
                                                        }
                                               sim.mOpportunityManager=null;
                                    }else{
                           mSim.NeedsOpportunityImport=false;
                                    }
          }
            protected static void OnLoadFixup(Sim sim,bool initialLoad){
                                        for(int i=sim.DreamsAndPromisesManager.mActiveNodes.Count-1;i>=0;i--){
                              ActiveNodeBase node=sim.DreamsAndPromisesManager.mActiveNodes[i];
                                                       if((initialLoad)||
                                 (NeedsFixup(node))){
                try{
                                             node.OnLoadFixup();
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

                               if(NeedsFixup(node)){
                                                  sim.DreamsAndPromisesManager.mActiveNodes.RemoveAt(i);
                               }else{
                try{
                                                  sim.DreamsAndPromisesManager.AddToReferenceList(node);
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
                                    for(int i=sim.DreamsAndPromisesManager.mSleepingNodes.Count-1;i>=0;i--){
                          ActiveNodeBase node=sim.DreamsAndPromisesManager.mSleepingNodes[i];
                           if(NeedsFixup(node)){
                try{
                            node.OnLoadFixup();
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
                           if(NeedsFixup(node)){
                                              sim.DreamsAndPromisesManager.mSleepingNodes.RemoveAt(i);
                           }
                           }
                                    }
            }
            protected static bool NeedsFixup(ActiveNodeBase node){
                                  ActiveDreamNode dreamNode=node as ActiveDreamNode;
                                               if(dreamNode!=null){
                                               if(dreamNode.NodeInstance==null)return true;
                                               }else{
                                  ActiveTimerNode timerNode=node as ActiveTimerNode;
                                               if(timerNode!=null){
                                               if(timerNode.NodeInstance==null)return true;
                                               }
                                               }
            return false;}
            public void Restore(Sim sim){
                try{
                       bool dnpUpdated=(false);
                        if(mDnPExportData!=null){
                                    sim.NullDnPManager();
DreamsAndPromisesManager.CreateFromExportData(sim,mDnPExportData);
                                    sim.SimDescription.DnPExportData=null;
                            dnpUpdated=( true);
                        }else 
                        if(mDnpManager!=null){
                                    sim.NullDnPManager();
                                    sim.mDreamsAndPromisesManager=mDnpManager;
                            dnpUpdated=( true);
                        }
                        if((dnpUpdated)&&(sim.DreamsAndPromisesManager!=null)){
                        OnLoadFixup(sim,mInitialStore);
                                    sim.DreamsAndPromisesManager.SetToUpdate(true,true);
                        }
                        if(mOpportunityManager!=null){
                                 if(sim.mOpportunityManager!=null){
                                    sim.mOpportunityManager.CancelAllOpportunities();
                                    sim.mOpportunityManager.TearDownLocationBasedOpportunities();
                                 }
                                    sim.mOpportunityManager=mOpportunityManager;
                        }else 
                                 if(sim.mOpportunityManager==null){
                                    sim.mOpportunityManager=new OpportunityManager(sim);
                                    sim.mOpportunityManager.SetupLocationBasedOpportunities();
                                 if(sim.mOpportunitiesAlarmHandle==AlarmHandle.kInvalidHandle){
                                    sim.ScheduleOpportunityCall();
                                 }
                                 }
                    try{
                        if(mOppStore!=null){
                           mOppStore.Dispose();
                        }
                                 if(sim.OpportunityManager!=null){
                                    sim.OpportunityManager.Fixup();
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
                    //  [NRaas:]Remove the opportunity alarm for inactive sims, as there is no check for selectability within it
                                 if(sim.CelebrityManager!=null){
                                    sim.CelebrityManager.RemoveOppotunityAlarm();
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
    public class BaseWorldReversion:IDisposable{
                     WorldName mPreviousCheatOverride;
                     WorldType mPreviousWorldType;
                          bool mAltered;
          public BaseWorldReversion(){
                               mPreviousCheatOverride=GameUtils.CheatOverrideCurrentWorld;
                               mPreviousWorldType    =GameUtils.GetCurrentWorldType();
            switch(mPreviousWorldType){
                case WorldType.Vacation  :
                case WorldType.University:
                case WorldType.Future    :
                                                      GameUtils.CheatOverrideCurrentWorld=GameUtils.GetCurrentWorld();
                                                      GameUtils.WorldNameToType[GameUtils.CheatOverrideCurrentWorld]=WorldType.Base;
                               mAltered=true;
                    break;
            }
          }
        public void Dispose(){
            if(mAltered){
                                                      GameUtils.WorldNameToType[GameUtils.CheatOverrideCurrentWorld]=mPreviousWorldType;
            }
                                                      GameUtils.CheatOverrideCurrentWorld=mPreviousCheatOverride;
        }
    }
    public class SuppressCreateHousehold:IDisposable{
          public SuppressCreateHousehold(){
            DisableCreateHousehold();
          }
        public void Dispose(){
             EnableCreateHousehold();
        }
                                                                                                       static ActionTuning sOriginalDemographicTuning=null;
                                                                                  static StoryActionFactory sOriginalFactoryTuning=null;
        public static bool DisableCreateHousehold(){
                try{
                    if(!IsCreateHouseholdAvailable()){
        return(false);}
                    if(StoryProgressionService.sService.mDemographicTuning.ActionTuning.TryGetValue("Create Household",out sOriginalDemographicTuning)){
                       StoryProgressionService.sService.mDemographicTuning.ActionTuning.Remove(     "Create Household");
                    }
                    if(StoryProgressionService.sService.mActionFactories.TryGetValue("Create Household",out sOriginalFactoryTuning)){
                       StoryProgressionService.sService.mActionFactories.Remove(     "Create Household");
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
        return( true);}
        public static void EnableCreateHousehold(){
                try{
                    if(StoryProgressionService.sService==null)return;
                    if(StoryProgressionService.sService.mDemographicTuning.ActionTuning.ContainsKey("Create Household"))return;
                                                                                                                        if(sOriginalDemographicTuning!=null){
                       StoryProgressionService.sService.mDemographicTuning.ActionTuning["Create Household"]=               sOriginalDemographicTuning;
                                                                                                                        }
                                                                                                         if(sOriginalFactoryTuning!=null){
                       StoryProgressionService.sService.mActionFactories["Create Household"]=               sOriginalFactoryTuning;
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
            public static bool IsCreateHouseholdAvailable(){
                    if(StoryProgressionService.sService==null){
            return(false);}
                   if(!StoryProgressionService.sService.mDemographicTuning.ActionTuning.ContainsKey("Create Household")){
            return(false);}
            return( true);
            }
    }
}
