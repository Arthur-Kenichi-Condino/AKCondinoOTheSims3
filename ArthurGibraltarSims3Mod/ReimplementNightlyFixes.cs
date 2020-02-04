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
    }
}