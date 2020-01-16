using Sims3.Gameplay.CAS;
using Sims3.SimIFace.CAS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
namespace ArthurGibraltarSims3Mod{
    public class CASParts{
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
    }
}
