using Sims3.Gameplay.Actors;
using Sims3.Gameplay.CAS;
using Sims3.SimIFace;
using Sims3.SimIFace.CAS;
using Sims3.UI.CAS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
namespace ArthurGibraltarSims3Mod{
    public class CASParts{
static List<BodyTypes>sBodyHairTypes=null;
public static ICollection<BodyTypes>BodyHairTypes{
    get{
                   if(sBodyHairTypes==null){
                      sBodyHairTypes=new List<BodyTypes>(Sim.kHairBodyTypes);
                   }
               return sBodyHairTypes;
    }
}
static List<BodyTypes>sGeneticBodyTypes=null;
public static ICollection<BodyTypes>GeneticBodyTypes{
    get{
                   if(sGeneticBodyTypes==null){
                      sGeneticBodyTypes=new List<BodyTypes>();
                      sGeneticBodyTypes.AddRange(BodyHairTypes);
                      sGeneticBodyTypes.Add(BodyTypes.BasePeltLayer);
                      sGeneticBodyTypes.Add(BodyTypes.BirthMark    );
                      sGeneticBodyTypes.Add(BodyTypes.Freckles     );
                      sGeneticBodyTypes.Add(BodyTypes.EyeColor     );                    
                      sGeneticBodyTypes.Add(BodyTypes.Moles        );
                      sGeneticBodyTypes.Add(BodyTypes.PeltLayer    );
                      sGeneticBodyTypes.Add(BodyTypes.PetBody      );
                      sGeneticBodyTypes.Add(BodyTypes.PetEars      );
                      sGeneticBodyTypes.Add(BodyTypes.PetHorn      );
                      sGeneticBodyTypes.Add(BodyTypes.PetHooves    );
                      sGeneticBodyTypes.Add(BodyTypes.PetMane      );
                      sGeneticBodyTypes.Add(BodyTypes.PetTail      );
                   }
               return sGeneticBodyTypes;
    }
}
          static CASParts(){
                  foreach(BodyTypes type in Enum.GetValues(typeof(BodyTypes))){
                             switch(type){
                     case BodyTypes.Last:
                     case BodyTypes.None:
                     case BodyTypes.TattooTemplate:
                                 break;
                             }
                      sAllTypes.Add(type);
                  }
          }
static List<BodyTypes>sAllTypes=new List<BodyTypes>();
        public static int AddOutfit(SimDescriptionCore sim,OutfitCategories category,SimBuilder builder,                           bool alternate){
               return ReplaceOutfit(                   sim,         new Key(category,-1),       builder,                                alternate);
        }
    public static int ReplaceOutfit(SimDescriptionCore sim,             Key key,     SimBuilder builder,                           bool alternate){
               return ReplaceOutfit(                   sim,                 key,                builder,  ulong.MaxValue,               alternate);
    }
    public static int ReplaceOutfit(SimDescriptionCore sim,             Key key,     SimBuilder builder,ulong components          ,bool alternate){
               return ReplaceOutfit(                   sim,                 key,CreateOutfit(sim,key,builder,components,alternate),     alternate);
    }
    public static int ReplaceOutfit(SimDescriptionCore sim,             Key key,SimOutfit newOutfit                               ,bool alternate){
                                      bool maternity=((sim.IsUsingMaternityOutfits)&&(!alternate));
                          ArrayList outfits=GetOutfits(sim,key.mCategory,alternate);
                                 if(outfits==null){
                                    outfits=new ArrayList();
                                        if(maternity){
                                                       sim.mMaternityOutfits[key.mCategory]=outfits;
                                        }else{
                                                       sim.Outfits[key.mCategory]=outfits;
                                        }
                                 }
                                int index=key.GetIndex(sim,alternate);
                                if((index==-1)||(index>=outfits.Count)){
                                    outfits.Add(newOutfit);
                              index=outfits.Count-1;
                                }else 
                                if( index<outfits.Count){
                SimOutfit oldOutfit=outfits[index] as SimOutfit;
                       if(oldOutfit!=null){
                    bool inUse=(false);
                foreach(OutfitCategories categories in sim.GetCurrentOutfits().Keys){
                                       ArrayList list2=sim.GetOutfits(categories);
                                              if(list2!=null){
                     foreach(SimOutfit outfit in list2){
                                    if(outfit.Key==oldOutfit.Key){
                         inUse=( true);
                                    break;
                                    }
                     }
                                              }

                      if(inUse)break;
                }

                     if(!inUse){
                                       oldOutfit.Uncache();
                     }
                       }
                                    outfits[index]=newOutfit;
                                }else{
                                    outfits.Insert(index,newOutfit);
                                }
                                   switch(key.mCategory){
                case OutfitCategories.Special:
                 if(!string.IsNullOrEmpty(key.mSpecialKey)){
                                                    if(sim.mSpecialOutfitIndices==null){
                                                       sim.mSpecialOutfitIndices=new Dictionary<uint,int>();
                                                    }
                                                       sim.mSpecialOutfitIndices[ResourceUtils.HashString32(key.mSpecialKey)]=index;
                 }
                    break;
                case OutfitCategories.Everyday:
                                SimDescription simDesc=sim as SimDescription;
                                            if(simDesc!=null){
                                               simDesc.mDefaultOutfitKey=sim.GetOutfit(OutfitCategories.Everyday,0).Key;
                                            }
                    break;
                                   }
                             return(index);
    }
    public static SimOutfit CreateOutfit(SimDescriptionCore sim,Key key,SimBuilder builder,ulong components,bool alternate){
                                                                                   builder.UseCompression=true;
            bool maternity=((sim.IsUsingMaternityOutfits)&&(!alternate));
            string outfitName=null;
                                                                if((key.mCategory==OutfitCategories.Special)||(key.GetIndex() != -1)){
                   outfitName=GetOutfitName(sim,key,maternity);
                                                                }else{
                   outfitName=GetOutfitName(sim,key.mCategory,maternity);
                                                                }
       return new SimOutfit(builder.CacheOutfit(outfitName,components,false));
    }
        public static string GetOutfitName(SimDescriptionCore sim,OutfitCategories category,bool maternity){
                      return GetOutfitName(                   sim,         new Key(category,sim.GetOutfitCount(category)),maternity);
        }
        public static string GetOutfitName(SimDescriptionCore sim,Key key,bool maternity){
                                                          string name=key.ToString()+Simulator.TicksElapsed();
                                                                            if(maternity){
                                                                 name+="Maternity";
                                                                            }
                                       SimDescription simDesc=sim as SimDescription;
                                                   if(simDesc!=null){
                                               return simDesc.SimDescriptionId.ToString()+name;
                                                   }else{
                                                       return sim.FullName+name;
                                                   }
        }
        public static SimOutfit GetOutfit(SimDescriptionCore sim,Key key,bool alternate){
                                                          if(sim==null)return null;
              ArrayList outfits=GetOutfits(sim,key.mCategory,alternate);
                     if(outfits==null)return null;
                                                           int index=key.GetIndex(sim,alternate);
                                                            if(index==-1)return null;
                                                            if(index<outfits.Count){
                                                              return outfits[index] as SimOutfit;
                                                            }else{
                                                              return null;
                                                            }
        }
        public static ArrayList GetOutfits(SimDescriptionCore sim,OutfitCategories category,bool alternate){
                                                                                              if(alternate){
                                                       return sim.Outfits[category] as ArrayList;
                                                                                              }else{
                                                       return sim.GetCurrentOutfits()[category] as ArrayList;
                                                                                              }
        }
        [Persistable]
        public class Key{
            public readonly OutfitCategories mCategory  ;
                   readonly int              mIndex     ;
            public readonly string           mSpecialKey;
           protected Key(){
                                             mCategory  =OutfitCategories.None;
                                             mIndex     =-1                   ;
                                             mSpecialKey=null                 ;
           }
           public Key(OutfitCategories category,int index){
                                             mCategory  =category;
                                             mIndex     =index   ;
                                             mSpecialKey=null    ;
           }
           public Key(OutfitCategories category,SimDescriptionCore sim){
                                             mCategory  =category                    ;
                                             mIndex     =sim.GetOutfitCount(category);
                                             mSpecialKey=null                        ;
           }
           public Key(Sim sim){
                                             mCategory  =OutfitCategories.None    ;
                                             mIndex     =0                        ;
                                             mSpecialKey=null                     ;
                       if(sim!=null){
                    try{
                                             mCategory  =sim.CurrentOutfitCategory;
                                             mIndex     =sim.CurrentOutfitIndex   ;
                    }catch{}
                       }
           }
           public Key(string specialKey){
                                             mCategory  =OutfitCategories.Special;
                                             mIndex     =-1                      ;
                                             mSpecialKey=specialKey              ;
           }
            public override bool Equals(object obj){
                                       Key key=obj as Key;
                                        if(key==null)return(false);
                                          if(mCategory  !=key.mCategory)return(false);
                                          if(mIndex     !=key.mIndex   )return(false);
                                      return(mSpecialKey==key.mSpecialKey);
            }
            public override 
                   int GetHashCode(){
                        return(int)ResourceUtils.HashString32(mCategory.ToString()+mIndex+mSpecialKey);
            }
            public int GetIndex(){
                                      return(mIndex);
            }
            public int GetIndex(SimDescriptionCore sim,bool alternate){
                                                          if((mCategory==OutfitCategories.Special)&&(!string.IsNullOrEmpty(mSpecialKey))){
                                            return sim.GetSpecialOutfitIndexFromKey(ResourceUtils.HashString32(mSpecialKey));
                                                          }else{
                                      return(mIndex);
                                                          }
            }
            public override string ToString(){
                if(string.IsNullOrEmpty(mSpecialKey)){
                                      return mCategory.ToString()+mIndex;
                }else{
                                      return mSpecialKey;
                }
            }
        }
        public class OutfitBuilder:IDisposable{
            public static void CopyGeneticParts(SimBuilder builder,SimOutfit sourceOutfit){
                builder.RemoveParts(new List<BodyTypes>(CASParts.GeneticBodyTypes).ToArray());
                                                     foreach(CASPart part in sourceOutfit.Parts){
                              if(!CASParts.GeneticBodyTypes.Contains(part.BodyType))continue;
                                                                 if((part.Age   &builder.Age   )!=builder.Age   )continue;
                                                                 if((part.Gender&builder.Gender)!=builder.Gender)continue;
                                                      new PartPreset(part,sourceOutfit).Apply(builder);
                                                     }
                builder.SetSecondaryNormalMapWeights(sourceOutfit.SecondaryNormalMapWeights);
                builder.FurMap         =sourceOutfit.FurMap         ;
                builder.NumCurls       =sourceOutfit.NumCurls       ;
                builder.CurlPixelRadius=sourceOutfit.CurlPixelRadius;
            }
              public OutfitBuilder(SimDescriptionCore sim,Key key,SimOutfit outfit):this(sim,key,outfit,false){}
              public OutfitBuilder(SimDescriptionCore sim,Key key,SimOutfit outfit,bool alternate){
                mBuilder=new SimBuilder();
                mSim=sim;
                mKey=key;
                mAlternate=alternate;
                mOutfit=outfit;
             if(mOutfit!=null){
OutfitUtils.SetOutfit(mBuilder,mOutfit,sim);
             }
              }
              public OutfitBuilder(SimDescriptionCore sim,Key key):this(sim,key,false){}
              public OutfitBuilder(SimDescriptionCore sim,Key key,bool alternate):this(sim,key,GetOutfit(sim,key,alternate),alternate){}
SimDescriptionCore 
                mSim;
      SimOutfit mOutfit;
     SimBuilder mBuilder;
            Key mKey;
           bool mAlternate=false;
          ulong mComponents=ulong.MaxValue;
            public void Dispose(){
             if(mOutfit!=null){
                int index=ReplaceOutfit(mSim,mKey,mBuilder,mComponents,mAlternate);
                mOutfit=mSim.GetOutfit(mKey.mCategory,index);
     SimDescription sim=mSim as SimDescription;
                if((sim!=null)&&(sim.CreatedSim!=null)){
                        try{
                    sim.CreatedSim.RefreshCurrentOutfit(false);
                        }catch(Exception exception){
                          Alive.WriteLog(exception.Message+"\n\n"+
                                         exception.StackTrace+"\n\n"+
                                         exception.Source);
                        }
                }
             }
               mBuilder.Dispose();
Alive.Sleep();
            }
        }
        public class PartPreset:CASPartPreset{
              public PartPreset(CASPart part){
                mPart=part;
                mPresetId=uint.MaxValue;
ResourceKey resKey=new ResourceKey(part.Key.InstanceId,0x333406c,part.Key.GroupId);
                mPresetString=Simulator.LoadXMLString(resKey);
              }
              public PartPreset(CASPart part,string preset):base(part,preset){}
              public PartPreset(CASPart part,SimOutfit sourceOutfit):base(part,sourceOutfit.GetPartPreset(part.Key)){}
              public PartPreset(CASPart part,uint index):base(part,CASUtils.PartDataGetPresetId(part.Key,index),CASUtils.PartDataGetPreset(part.Key,index)){}
            public bool Apply(SimBuilder builder){
                                      if(builder.AddPart(mPart)){
                    if(!string.IsNullOrEmpty(mPresetString)){
     OutfitUtils.ApplyPresetStringToPart(builder,mPart,mPresetString);
                    }
                    return true;
                                      }else{
                    return false;
                                      }
            }
        }
    }
}
