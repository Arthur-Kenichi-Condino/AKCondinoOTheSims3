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
                        try{
           BonehildaCoffin.FindBonehildaCoffin(sim)?.SetUpBonehildaOutfit();
                        }catch{
                        }
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
}