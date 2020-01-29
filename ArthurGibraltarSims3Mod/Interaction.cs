using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Objects.Electronics;
using Sims3.SimIFace;
using System;
using System.Collections.Generic;
using System.Text;
using static ArthurGibraltarSims3Mod.Alive;
using static ArthurGibraltarSims3Mod.Interaction;
namespace ArthurGibraltarSims3Mod{
    public class Interaction{
    //====================================================================================================================================================
    //       ModdedInteraction:the injecting part:from NRaas mods
    //====================================================================================================================================================
    public abstract class ModdedInteraction<SimObj>:Sims3.Gameplay.Interactions.ImmediateInteractionGameObjectHit<Sims3.Gameplay.Interfaces.IActor,SimObj>,IAddInteraction where SimObj:class,Sims3.Gameplay.Interfaces.IGameObject{
                                                                                                                                       public abstract void AddInteraction(InteractionInjectorList interactions);
        public override string GetInteractionName(){
                try{
                   return Alive.Localize("Root:MenuName");
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source);
                   return"ERROR";
                }
        }
      protected virtual string GetInteractionName(Sims3.Gameplay.Interfaces.IActor actor,SimObj target,Sims3.SimIFace.GameObjectHit hit){
                try{
                        return GetInteractionName();
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source);
                   return"ERROR_0_";
                }
      }
        public override Sims3.Gameplay.Core.Lot GetTargetLot(){
                   Sims3.Gameplay.Core.Lot lot=Target as Sims3.Gameplay.Core.Lot;
                       if(lot!=null)return(lot);
                                    return base.GetTargetLot();
        }
  protected virtual bool Test(Sims3.Gameplay.Interfaces.IActor actor,SimObj target,Sims3.SimIFace.GameObjectHit hit,ref Sims3.SimIFace.GreyedOutTooltipCallback greyedOutTooltipCallback){return(true);}
        [Sims3.Gameplay.Autonomy.DoesntRequireTuning]
        public abstract class ModdedDefinition<SimInteraction>:Sims3.Gameplay.Interactions.ImmediateInteractionDefinition<Sims3.Gameplay.Interfaces.IActor,SimObj,SimInteraction>where SimInteraction:ModdedInteraction<SimObj>,Sims3.Gameplay.Interactions.IImmediateInteraction,new(){
                              protected string[] mPath=null;
                       public ModdedDefinition():this(new string[0]){}
                    protected ModdedDefinition(string[]path){
                                                 mPath=path;
                    }
            public override string[] GetPath(bool isFemale){
                                          return mPath;
            }
                                                                                                     protected Sims3.SimIFace.GameObjectHit mHit;
            public override Sims3.Gameplay.Interactions.InteractionTestResult Test(ref Sims3.Gameplay.Interactions.InteractionInstanceParameters parameters,ref Sims3.SimIFace.GreyedOutTooltipCallback greyedOutTooltipCallback){
                try{
                                                                                                                                            mHit=parameters.Hit;
                            if(Test(parameters.Actor,
                                    parameters.Target as SimObj,
                                    parameters.Autonomous,
                                        ref greyedOutTooltipCallback)){
            return(Sims3.Gameplay.Interactions.InteractionTestResult.Pass          );
                            }
                }catch(Sims3.SimIFace.ResetException exception){
                                      Alive.WriteLog(exception.Message+"\n\n"+
                                                     exception.StackTrace+"\n\n"+
                                                     exception.Source);
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source);
                }
            return(Sims3.Gameplay.Interactions.InteractionTestResult.Def_TestFailed);}
        }
    }//  COPY COMPLETED
    //-----------------------------------------------------------------------------------------------------------
    public interface IAddInteraction{
                 void AddInteraction(InteractionInjectorList interactions);
    }//  COPY COMPLETED
    //-----------------------------------------------------------------------------------------------------------
    //  InteractionInjectors
    //-----------------------------------------------------------------------------------------------------------
    public class InteractionInjectorList{
          static InteractionInjectorList sMasterList=null;
   public static InteractionInjectorList  MasterList{
    get{
                                      if(sMasterList==null){
                     string injectionNames="InteractionInjectorList_LOG:NOT_ERROR\n";
                List<IAddInteraction>addInteractions=DerivativeSearch.Find<IAddInteraction>();
             foreach(IAddInteraction interaction in addInteractions){
                            injectionNames+="AddInteraction "+interaction.GetType().ToString()+"\n";
             }
             Alive.WriteLog(injectionNames);
                                         sMasterList=new InteractionInjectorList(addInteractions);
                                      }
                                  return(sMasterList);
    }
   }
                 public static List<Type>sAlwaysTypes;
          static InteractionInjectorList(){
                                         sAlwaysTypes=new List<Type>();
                                         sAlwaysTypes.Add(typeof(Sims3.Gameplay.Actors.Sim    ));
                                         sAlwaysTypes.Add(typeof(Sims3.Gameplay.Core  .Lot    ));
                                         sAlwaysTypes.Add(typeof(Sims3.Gameplay.Core  .Terrain));
          }
                     public static bool IsAlwaysType(Type type){
              foreach(Type alwaysType in sAlwaysTypes){
                        if(alwaysType.IsAssignableFrom(type)){
       return( true);
                        }
              }
       return(false);}
        public InteractionInjectorList(List<IAddInteraction> interactions){
                      foreach(IAddInteraction interaction in interactions){
                try{
                                              interaction.AddInteraction(this);
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source);
                }finally{
                }
                      }
        }
        public void AddInjector(IInteractionInjector injector){
                                foreach(Type type in injector.GetTypes()){
                                     AddType(type,injector);
                                }
        }
           Dictionary<Type,List<IInteractionInjector>>mTypes=new Dictionary<Type,List<IInteractionInjector>>();
                      protected void AddType(Type type,IInteractionInjector injector){
                           List<IInteractionInjector>injectors;
                                                  if(!mTypes.TryGetValue(type,out injectors)){
                                                                                  injectors=new List<IInteractionInjector>();
                                                      mTypes.Add(type,injectors);
                                                  }
                                                     injectors.Add(injector);
                      }
public IEnumerable<KeyValuePair<Type,List<IInteractionInjector>>>Types{get{return(mTypes);}}
                       public bool IsEmpty{get{return(mTypes.Count==0);}}
        public void Add<ObjType>(Sims3.Gameplay.Interactions.InteractionDefinition definition)where ObjType:Sims3.Gameplay.Interfaces.IGameObject{
                try{
                                      AddInjector(new InteractionInjector<ObjType>(definition,true));
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source);
                }
        }
        public void AddRoot(Sims3.Gameplay.Interactions.InteractionDefinition definition){
                try{
                                      AddInjector(new InteractionInjectorRoot(definition));
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source);
                }
        }
        public void AddCustom(IInteractionInjector injector){
                try{
                                       AddInjector(injector);
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source);
                }
        }
        public void AddNoDupTest<ObjType>(Sims3.Gameplay.Interactions.InteractionDefinition definition)where ObjType:Sims3.Gameplay.Interfaces.IGameObject{
                try{
                                      AddInjector(new InteractionInjectorNoDupTest<ObjType>(definition));
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source);
                }
        }
            public void Replace<ObjType,T>(Sims3.Gameplay.Interactions.InteractionDefinition definition)where ObjType:Sims3.Gameplay.Interfaces.IGameObject where T:Sims3.Gameplay.Interactions.InteractionDefinition{
                try{
                                              AddInjector(new InteractionReplacer<ObjType,T>(definition,true));
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source);
                }
            }
            public void ReplaceNoTest<ObjType,T>(Sims3.Gameplay.Interactions.InteractionDefinition definition)where ObjType:Sims3.Gameplay.Interfaces.IGameObject where T:Sims3.Gameplay.Interactions.InteractionDefinition{
                try{
                                                    AddInjector(new InteractionReplacer<ObjType,T>(definition,false));
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source);
                }
            }
        public void Perform(Sims3.Gameplay.Abstracts.GameObject obj){
                try{
                           List<IInteractionInjector>injectors=new List<IInteractionInjector>();
 foreach(KeyValuePair<Type,List<IInteractionInjector>>type in mTypes){
                                                   if(type.Key.IsAssignableFrom(obj.GetType())){
                                                     injectors.AddRange(type.Value);
                                                   }
 }
                                                  if(injectors.Count==0)return;
                    Perform(obj,injectors);
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source);
                }
        }
     protected void Perform(Sims3.Gameplay.Abstracts.GameObject obj,IEnumerable<IInteractionInjector>injectors){
                try{
                                                             if(obj==null)return;
                    Dictionary<Type,bool>existing=new Dictionary<Type,bool>();
  foreach(Sims3.Gameplay.Autonomy.InteractionObjectPair pair in obj.Interactions){
                                                     if(pair.InteractionDefinition is IWasHereDefinition)return;
                                              Type type=pair.InteractionDefinition.GetType();
                                          existing[type]=true;
  }
                                                             if(obj.ItemComp!=null){
  foreach(Sims3.Gameplay.Autonomy.InteractionObjectPair pair in obj.ItemComp.InteractionsInventory){
                                                     if(pair.InteractionDefinition is IWasHereDefinition)return;
                                              Type type=pair.InteractionDefinition.GetType();
                                          existing[type]=true;
  }
                                                             }
                                                            foreach(IInteractionInjector injector in injectors){
                                                                                         injector.Perform(obj,existing);
                                                            }
                                                                obj.AddInteraction(   IWasHereDefinition.Singleton);
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source);
                }
     }
    }//  COPY COMPLETED
    public interface IInteractionInjector{
                       List<Type>GetTypes();
        void Perform(Sims3.Gameplay.Abstracts.GameObject obj,Dictionary<Type,bool>existing);
    }//  COPY COMPLETED
    public class InteractionInjector<ObjType>:IInteractionInjector where ObjType:Sims3.Gameplay.Interfaces.IGameObject{
                         Sims3.Gameplay.Interactions.InteractionDefinition mDefinition;
       protected InteractionInjector(){}
       protected InteractionInjector(Sims3.Gameplay.Interactions.InteractionDefinition definition):this(definition,true){}
          public InteractionInjector(Sims3.Gameplay.Interactions.InteractionDefinition definition,bool throwError){
                                                                           mDefinition=definition;
                                                         if((throwError)&&(mDefinition==null)){
                                         throw new NullReferenceException("mDefinition");
                                                         }
          }
        public virtual List<Type>GetTypes(){
                       List<Type>list=new List<Type>();
                                 list.Add(typeof(ObjType));
                          return list;
        }
 public void Perform(Sims3.Gameplay.Abstracts.GameObject obj,Dictionary<Type,bool>existing){
             Perform(obj,mDefinition,existing);
 }
protected virtual bool 
             Perform(Sims3.Gameplay.Abstracts.GameObject obj,Sims3.Gameplay.Interactions.InteractionDefinition definition,Dictionary<Type,bool>existing){
                try{
                                                      if(obj is ObjType){
                                                                                                     Type type=definition.GetType();
                                                                                                                                            if(existing.ContainsKey(type))return(false);
                                                                                                                                               existing.Add(        type,true);
                                                                                            obj.AddInteraction(definition);
                                                                                   obj.AddInventoryInteraction(definition);
                                                                                                                                                                          return( true);
                                                      }
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source);
                }
                                                                                                                                                                          return(false);
}
    }//  COPY COMPLETED
    public class InteractionInjectorNoDupTest<ObjType>:InteractionInjector<ObjType>where ObjType:Sims3.Gameplay.Interfaces.IGameObject{
       protected InteractionInjectorNoDupTest(){}
          public InteractionInjectorNoDupTest(Sims3.Gameplay.Interactions.InteractionDefinition definition):base(definition){}
protected override bool 
             Perform(Sims3.Gameplay.Abstracts.GameObject obj,Sims3.Gameplay.Interactions.InteractionDefinition definition,Dictionary<Type,bool>existing){
                try{
                                                      if(obj is ObjType){
                                                                                            obj.AddInteraction(definition);
                                                                                   obj.AddInventoryInteraction(definition);
                                                                                                                                                                          return( true);
                                                      }
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source);
                }
                                                                                                                                                                          return(false);
}
    }//  COPY COMPLETED
    public class InteractionInjectorRoot:InteractionInjector<Sims3.Gameplay.Abstracts.GameObject>{
          public InteractionInjectorRoot(Sims3.Gameplay.Interactions.InteractionDefinition definition):base(definition,true){}
       public override List<Type>GetTypes(){
                       List<Type>list=new List<Type>();
                                 list.Add(typeof(Sims3.Gameplay.Actors.Sim));
                                 list.Add(typeof(Sims3.Gameplay.Core.  Lot));
                          return list;
       }
protected override bool 
             Perform(Sims3.Gameplay.Abstracts.GameObject obj,Sims3.Gameplay.Interactions.InteractionDefinition definition, Dictionary<Type, bool> existing){
                              if(!Alive.IsRootMenuObject(obj))return(false);
 return base.Perform(obj,definition,existing);
}
    }//  COPY COMPLETED
    public class InteractionReplacer<ObjType,T>:InteractionInjector<ObjType>where ObjType:Sims3.Gameplay.Interfaces.IGameObject where T:Sims3.Gameplay.Interactions.InteractionDefinition{
          public InteractionReplacer(Sims3.Gameplay.Interactions.InteractionDefinition definition,bool testExistence):base(definition){
                                                                                        mTestExistence=testExistence;
          }
                                                                                   bool mTestExistence;
protected override bool 
             Perform(Sims3.Gameplay.Abstracts.GameObject obj,Sims3.Gameplay.Interactions.InteractionDefinition definition,Dictionary<Type,bool>existing){
                                                                                                                                     Type type=typeof(T);
                try{
                                                                                     if(mTestExistence){
                                                                                                                                           if(!existing.ContainsKey(type))return(false);
                                                                                     }
    if(!base.Perform(obj,definition,existing))return(false);
                                                                                             RemoveInteraction(obj,type);
                                                                                                                                               existing.Remove(type);
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source);
                }
return( true);}
        public static void RemoveInteraction<Y>(Sims3.Gameplay.Abstracts.GameObject obj)where Y:Sims3.Gameplay.Interactions.InteractionDefinition{
                           RemoveInteraction(obj,typeof(Y));
        }
        public static void RemoveInteraction   (Sims3.Gameplay.Abstracts.GameObject obj,Type type){
                try{
                                                                                 if(obj.Interactions!=null){
                                                                          int index=0;
                                                                        while(index<obj.Interactions.Count){
                                 Sims3.Gameplay.Autonomy.InteractionObjectPair pair=obj.Interactions[index];
                                                                            if(pair.InteractionDefinition.GetType()==type){
                                                                                    obj.Interactions.RemoveAt(index);
                                                                            }else{
                                                                              index++;
                                                                            }
                                                                        }
                                                                                 }
                             Sims3.Gameplay.ObjectComponents.ItemComponent itemComp=obj.ItemComp;
                                                                        if(itemComp!=null){
                                                                          int index=0;
                                                                        while(index<itemComp.InteractionsInventory.Count){
                                 Sims3.Gameplay.Autonomy.InteractionObjectPair pair=itemComp.InteractionsInventory[index];
                                                                            if(pair.InteractionDefinition.GetType()==type){
                                                                                    itemComp.InteractionsInventory.RemoveAt(index);
                                                                            }else{
                                                                              index++;
                                                                            }
                                                                        }
                                                                        }
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source);
                }
        }
    }//  COPY COMPLETED
    //-----------------------------------------------------------------------------------------------------------
    //====================================================================================================================================================
    //  AlikeModdedInteraction:the detect and execute interaction part:from NRaas mods
    //====================================================================================================================================================
    public abstract class AlikeModdedInteraction<TOption,TTarget>:ModdedInteraction<TTarget>where TOption:class,IInteractionOptionItem<Sims3.Gameplay.Interfaces.IActor,TTarget,GameHitParameters<TTarget>>where TTarget:class,Sims3.Gameplay.Interfaces.IGameObject{
        protected virtual bool SingleSelection{get{return(true);}}
        public override bool Run(){
                try{
                                       IPerform definition=InteractionDefinition as IPerform;
                                         return(definition.Perform(this,Actor,Target,Hit)!=OptionResult.Failure);
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source);
                                         return(false);
                }finally{
                }
        }
        protected abstract OptionResult Perform(Sims3.Gameplay.Interfaces.IActor actor,TTarget target,Sims3.SimIFace.GameObjectHit hit);
                   protected interface IPerform{
                           OptionResult Perform(AlikeModdedInteraction<TOption,TTarget>interaction,Sims3.Gameplay.Interfaces.IActor actor,TTarget target,Sims3.SimIFace.GameObjectHit hit);
                   }
        public class UIMouseEventArgsEx:Sims3.UI.UIMouseEventArgs{
              public UIMouseEventArgsEx(){
                Sims3.SimIFace.Vector2 position=Sims3.UI.UIManager.GetCursorPosition();
                              base.mF1=position.x;
                              base.mF2=position.y;
              }
        }
        [Sims3.Gameplay.Autonomy.DoesntRequireTuning]
        public class AlikeModdedDefinition<SimInteraction>:ModdedDefinition<SimInteraction>,IPerform where SimInteraction:AlikeModdedInteraction<TOption,TTarget>,Sims3.Gameplay.Interactions.IImmediateInteraction,new(){
                           static readonly SimInteraction sTest=new SimInteraction();
          static List<IInteractionOptionItem<Sims3.Gameplay.Interfaces.IActor,TTarget,GameHitParameters<TTarget>>>sPopupOptions;
            public static void UnloadPopupOptions(){
                                                                                                               if(sPopupOptions!=null){
                                                                                                                  sPopupOptions.Clear();
                                                                                                               }
                                                                                                                  sPopupOptions=(null);
            }
            protected IInteractionOptionItem<Sims3.Gameplay.Interfaces.IActor,TTarget,GameHitParameters<TTarget>>mOption;
                      IInteractionProxy<Sims3.Gameplay.Interfaces.IActor,TTarget,GameHitParameters<TTarget>>mProxy;
                                                                                                  bool mPopup;
                                                                                             bool mAddRoot;
              public AlikeModdedDefinition():this(false,false){}
              public AlikeModdedDefinition(bool popup,bool addRoot):this(null,null,popup,new string[0]){
                                                  mAddRoot=addRoot;
              }
             private AlikeModdedDefinition(IInteractionProxy<Sims3.Gameplay.Interfaces.IActor,TTarget,GameHitParameters<TTarget>>proxy,IInteractionOptionItem<Sims3.Gameplay.Interfaces.IActor,TTarget,GameHitParameters<TTarget>>option,bool popup,string[]path):base(path){
                                                                                                                          mProxy=proxy;
                                                                                                                                                                                                                          mOption=option;
                                                                                                                                                                                                                                       mPopup=popup;
             }
            public override void AddInteractions(Sims3.Gameplay.Autonomy.InteractionObjectPair iop,Sims3.Gameplay.Interfaces.IActor actor,TTarget target,List<Sims3.Gameplay.Autonomy.InteractionObjectPair>results){
                try{
                    string[]defPath=mPath;
                        if((defPath==null)||(defPath.Length==0)){
                            if(mAddRoot){
                            defPath=new string[]{"Mod Interactions",sTest.GetInteractionName(actor,target,mHit)};
                            }else{
                            defPath=new string[]{"Mod Interactions"};
                            }
                        }
                        if((mPopup)||((VersionStamp.sPopupMenuStyle))&&(iop==null)){
                   List<string>path=null;
                         if(mPopup){
                               path=new List<string>(defPath);
                         }else{
                               path=new List<string>();
                         }
                         if(sPopupOptions==null){
                            sPopupOptions=new List<IInteractionOptionItem<Sims3.Gameplay.Interfaces.IActor,TTarget,GameHitParameters<TTarget>>>();
                              foreach(TOption option in ModdedOptionList<TOption>.AllOptions()){
                            sPopupOptions.Add(option.Clone() as TOption);
                              }
                         }
                                ListOptions(null,sPopupOptions,actor,target,mHit,mPopup,path,results);
                        }else{
                      mPath=defPath;
                            base.AddInteractions(iop,actor,target,results);
                        }
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source);
                }
            }
            private static void ListOptions(IInteractionProxy<Sims3.Gameplay.Interfaces.IActor,TTarget,GameHitParameters<TTarget>>proxy,
                                        IEnumerable<IInteractionOptionItem<Sims3.Gameplay.Interfaces.IActor,TTarget,GameHitParameters<TTarget>>>options,
                                    Sims3.Gameplay.Interfaces.IActor actor,
                                TTarget target,
                            Sims3.SimIFace.GameObjectHit hit,
                        bool popup,
                    List<string>path,
                List<Sims3.Gameplay.Autonomy.InteractionObjectPair>results)
            {
                try{
                GameHitParameters<TTarget>parameters=new GameHitParameters<TTarget>(actor,target,hit);
                                   foreach(IInteractionOptionItem<Sims3.Gameplay.Interfaces.IActor,TTarget,GameHitParameters<TTarget>>option in options){
                                                                                                                                  if(!option.Test(parameters))continue;
                                                                                                  IInteractionOptionList<TTarget>list=option as IInteractionOptionList<TTarget>;
                                                                                                                              if(list!=null){
                                                                                                              List<string>newPath=new List<string>(path);
                                                                                                                          newPath.Add(option.Name);
                                ListOptions(list,list.IOptions(),actor,target,hit,popup,newPath,results);
                                                                                                                              }else{
                                                                   results.Add(new Sims3.Gameplay.Autonomy.InteractionObjectPair(new AlikeModdedDefinition<SimInteraction>(proxy,option,popup,path.ToArray()),target));
                                                                                                                              }
                                   }
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source);
                }
            }
            public override bool Test(Sims3.Gameplay.Interfaces.IActor a,TTarget target,bool isAutonomous,ref Sims3.SimIFace.GreyedOutTooltipCallback greyedOutTooltipCallback){
                       if(!sTest.Test(a,target,mHit,ref greyedOutTooltipCallback))return(false);
                                                                                  return(!isAutonomous);
            }
public OptionResult 
             Perform(AlikeModdedInteraction<TOption,TTarget>interaction,Sims3.Gameplay.Interfaces.IActor actor,TTarget target,Sims3.SimIFace.GameObjectHit hit){
                try{
                if(mOption==null){
               if((mPopup)||(VersionStamp.sPopupMenuStyle)){
         List<Sims3.Gameplay.Autonomy.InteractionObjectPair>interactions=new List<Sims3.Gameplay.Autonomy.InteractionObjectPair>();
                                 AddInteractions(null,actor,target,interactions);
Sims3.Gameplay.UI.PieMenu.TestAndBringUpPieMenu(actor,new UIMouseEventArgsEx(),hit,interactions,Sims3.Gameplay.Core.InteractionMenuTypes.Normal);
return(OptionResult.SuccessClose);
               }else{
                                                     return interaction.Perform(actor,target,hit);
               }
                }else 
                if(mProxy!=null){
            return mProxy.Perform(mOption,new GameHitParameters<TTarget>(actor,target,hit));
                }else{
            return mOption.Perform(new GameHitParameters<TTarget>(actor,target,hit));
                }
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source);
return(OptionResult.Failure);
                }
}
            public override string GetInteractionName(Sims3.Gameplay.Interfaces.IActor actor,TTarget target,Sims3.Gameplay.Autonomy.InteractionObjectPair iop){
                try{
                if(mOption==null){
                      return sTest.GetInteractionName(actor,target,mHit);
                }else{
            return mOption.ToString();
                }
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source);
                   return"ERROR_1_";
                }
            }
        }
    }//  COPY COMPLETED
    //-----------------------------------------------------------------------------------------------------------
    //  
    //-----------------------------------------------------------------------------------------------------------
    public interface IInteractionProxy<TActor,TTarget,TParameters>where TParameters:InteractionOptionParameters<TActor,TTarget>{
                OptionResult Perform(IInteractionOptionItem<TActor,TTarget,TParameters>option,TParameters parameters);
    }//  COPY COMPLETED
    //-----------------------------------------------------------------------------------------------------------
    public interface IInteractionOptionList<TTarget>:IInteractionOptionItem<Sims3.Gameplay.Interfaces.IActor,TTarget,GameHitParameters<TTarget>>,IInteractionProxy<Sims3.Gameplay.Interfaces.IActor,TTarget,GameHitParameters<TTarget>>where TTarget:class,Sims3.Gameplay.Interfaces.IGameObject{
                List<IInteractionOptionItem<Sims3.Gameplay.Interfaces.IActor,TTarget,GameHitParameters<TTarget>>>IOptions();
    }//  COPY COMPLETED
    public abstract class InteractionOptionItem<TActor,TTarget,TParameters>:ModdedOptionItem,IInteractionOptionItem<TActor,TTarget,TParameters>where TParameters:InteractionOptionParameters<TActor,TTarget>{
                   public InteractionOptionItem(){}
                   public InteractionOptionItem(string name,int count)
                    :base(name,count){
                   }
                   public InteractionOptionItem(string name,int count,string icon,Sims3.SimIFace.ProductVersion version)
                    :base(name,count,icon,version){
                   }
                   public InteractionOptionItem(string name,int count,Sims3.SimIFace.ResourceKey icon)
                    :base(name,count,icon){
                   }
                   public InteractionOptionItem(string name,int count,Sims3.SimIFace.ThumbnailKey thumbnail)
                    :base(name,count,thumbnail){
                   }
        public abstract string GetTitlePrefix();
        public override string Name{get{
                  string title=GetTitlePrefix();
               if(string.IsNullOrEmpty(title)){
                       return mName;
               }
               if(Sims3.Gameplay.Utilities.Localization.HasLocalizationString(title+":Title")){
                         title+=":Title";
               }else{
                         title+=":MenuName";
               }
                       return Alive.Localize(title);
            }
        }
        protected abstract OptionResult Run(TParameters parameters);
        public static Sims3.Gameplay.Core.Lot GetLot(Sims3.Gameplay.Abstracts.GameObject target,Sims3.SimIFace.GameObjectHit hit){
                      Sims3.Gameplay.Core.Lot lot=target as Sims3.Gameplay.Core.Lot;
                                           if(lot!=null){
                                       return lot;
                                           }
                                               if(target!=null){
                                               if(target.LotCurrent!=null)
                                           return target.LotCurrent;
                                               }
               return Sims3.Gameplay.Core.LotManager.GetLotAtPoint(hit.mPoint);
        }
        public bool Test(TParameters parameters){
            try{
                if(string.IsNullOrEmpty(Name))return(false);
                        return Allow(parameters);
            }catch(Exception exception){
              Alive.WriteLog(exception.Message+"\n\n"+
                             exception.StackTrace+"\n\n"+
                             exception.Source);
                InteractionOptionParameters<TActor,TTarget>.Exception(parameters,exception);
                                              return(false);
            }
        }
        protected virtual bool Allow(TParameters parameters){
            Reset();
                                              return( true);
        }
        public OptionResult Perform(TParameters parameters){
                        Task.Result result=new Task.Result();
            //  Used as a method of shortening the stack frame
                           new Task(result,this,parameters).AddToSimulator();
                              while(result.mResult==OptionResult.Unset){
              Alive.Sleep();
                              }
                             return result.mResult;
        }
        public class Task:ModTask{
InteractionOptionItem<TActor,TTarget,TParameters>mItem;
                                     TParameters mParameters;
              public Task(Result result,InteractionOptionItem<TActor,TTarget,TParameters>item,TParameters parameters){
                                                 mResult=result;
                                                                                   mItem=item;
                                                                                              mParameters=parameters;
              }
                                          Result mResult;
                             public class Result{
                                public OptionResult mResult=OptionResult.Unset;
                             }
            protected override void OnPerform(){
                try{
                                                    mResult.mResult=mItem.Run(mParameters);
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source);
                InteractionOptionParameters<TActor,TTarget>.Exception(mParameters,exception);
                                                    mResult.mResult=OptionResult.Failure;
                }
            }
        }
    }
    //-----------------------------------------------------------------------------------------------------------
    public interface IPrimaryOption<TTarget>:IInteractionOptionItem<Sims3.Gameplay.Interfaces.IActor,TTarget,GameHitParameters<TTarget>>where TTarget:class,Sims3.Gameplay.Interfaces.IGameObject{}
    //-----------------------------------------------------------------------------------------------------------
    public interface IInteractionOptionItem<TActor,TTarget,TParameters>:IModdedOptionItem where TParameters:InteractionOptionParameters<TActor,TTarget>{
        string GetTitlePrefix();
        bool Test(TParameters parameters);
                OptionResult Perform(TParameters parameters);
    }//  COPY COMPLETED
    public enum OptionResult{
        Unset,
        SuccessRetain,
        SuccessLevelDown,
        SuccessClose,
        Failure,
    }//  COPY COMPLETED
    public abstract class InteractionOptionParameters<TActor,TTarget>{
                                      public readonly TActor mActor;
                                             public readonly TTarget mTarget;
                   public InteractionOptionParameters(TActor actor,TTarget target){
                                                      mActor=actor;
                                                                   mTarget=target;
                   }
        public static void Exception(InteractionOptionParameters<TActor,TTarget> parameters,Exception exception){
                                                                              if(parameters==null){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source);
                                                                              }else{
                                                                                 parameters.PrivateException(exception);
                                                                              }
        }
        protected abstract void PrivateException(Exception e);
    }//  COPY COMPLETED
    [Sims3.SimIFace.Persistable]
    public abstract class ModdedOptionItem:IModdedOptionItem{
                     protected string mName;
        [Sims3.SimIFace.Persistable(false)]
                        protected int mCount=-1;
        [Sims3.SimIFace.Persistable(false)]
protected Sims3.SimIFace.ThumbnailKey mThumbnail=Sims3.SimIFace.ThumbnailKey.kInvalidThumbnailKey;
                   public ModdedOptionItem(){}
                   public ModdedOptionItem(string name):this(name,-1){}
                   public ModdedOptionItem(string name,int count){
                                            mName=name;
                                                    mCount=count;
                   }
                   public ModdedOptionItem(string name,int count,string icon,Sims3.SimIFace.ProductVersion version)
                    :this(name,count){
                    SetThumbnail(icon,version);
                   }
                   public ModdedOptionItem(string name,int count,Sims3.SimIFace.ResourceKey icon)
                    :this(name,count){
                    SetThumbnail(icon);
                   }
                   public ModdedOptionItem(string name, int count,Sims3.SimIFace.ThumbnailKey thumbnail)
                    :this(name,count){
                      mThumbnail=thumbnail;
                   }
                   public ModdedOptionItem(ModdedOptionItem source)
                    :this(source.mName,source.mCount,source.mThumbnail){
                   }
        public void SetThumbnail(string icon,Sims3.SimIFace.ProductVersion version){
                    SetThumbnail(Sims3.SimIFace.ResourceKey.CreatePNGKey(icon,Sims3.SimIFace.ResourceUtils.ProductVersionToGroupId(version)));
        }
        public void SetThumbnail(Sims3.SimIFace.ThumbnailKey key){
                      mThumbnail=key;
        }
        public void SetThumbnail(Sims3.SimIFace.ResourceKey icon){
                      mThumbnail=new Sims3.SimIFace.ThumbnailKey(icon,Sims3.SimIFace.ThumbnailSize.Medium);
        }
        public virtual string Name{get{return mName;}}
        public bool IsSet{get{return(Count>0);}}
        public virtual bool UsingCount{get{return(Count!=-1);}}
        public int Count{get{return mCount;      }
                         set{       mCount=value;}}
    public void IncCount(){mCount++;}
    public void IncCount(int count){mCount+=count;}
        public virtual void Reset(){if(UsingCount){mCount=0;}}
        public Sims3.SimIFace.ThumbnailKey Thumbnail{get{return mThumbnail;}}
         public virtual string DisplayKey{get{return null;}}
        public abstract string DisplayValue{get;}
                   public virtual int ValueWidth{get{return 0;}}
        public static int SortByName(ModdedOptionItem l,ModdedOptionItem r){
            return l.Name.CompareTo(r.Name);
        }
        public virtual IModdedOptionItem Clone(){
                        return MemberwiseClone() as IModdedOptionItem;
        }
        public interface IModdedOptionListProxy<T>where T:class,IModdedOptionItem{
            void GetOptions(List<T>items);
        }
        public override string ToString(){
            string displayValue=DisplayValue;
                if(displayValue!=null){
            return Name+" = "+displayValue;
                }else{
            return Name;
                }
        }
    }//  COPY COMPLETED
    //-----------------------------------------------------------------------------------------------------------
    public interface IVersionOption:IModdedOptionItem{
        string Prompt{get;}
    }
    public interface IModdedOptionItem{
        string Name{get;}
        Sims3.SimIFace.ThumbnailKey Thumbnail{get;}
        string DisplayValue{get;}
        bool UsingCount{get;}
        int Count{get;set;}
        string DisplayKey{get;}
        int ValueWidth{get;}
        IModdedOptionItem Clone();
    }//  COPY COMPLETED
    public abstract class ModdedOptionList<T>:ModdedOptionItem where T:class,IModdedOptionItem{
        public static List<T>AllOptions(){
                      List<T>items=new List<T>();
                   foreach(T item in DerivativeSearch.Find<T>()){
IModdedOptionListProxy<T>proxy=
                             item as IModdedOptionListProxy<T>;
                      if(proxy!=null){
                         proxy.GetOptions(items);
                      }else{
                             items.Add(item);
                      }
                   }
                             items.Sort(new Comparison<T>(OnNameCompare));
                      return items;
        }
                   public ModdedOptionList(){}
                   public ModdedOptionList(string name):base(name){}
        protected virtual int NumSelectable{get{return 1;}}
        public static int OnNameCompare(T left,T right){
                try{
            return(left.Name.CompareTo(right.Name));
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source);
            return(0);
                }
        }
        public abstract List<T>GetOptions();
    }//  COPY COMPLETED
    //-----------------------------------------------------------------------------------------------------------
    public interface ICloseDialogOption{
    }//  COPY COMPLETED
    //-----------------------------------------------------------------------------------------------------------
    public class GameHitParameters<TTarget>:InteractionOptionParameters<Sims3.Gameplay.Interfaces.IActor,TTarget>where TTarget:class,Sims3.Gameplay.Interfaces.IGameObject{
          public GameHitParameters(Sims3.Gameplay.Interfaces.IActor actor,TTarget target,Sims3.SimIFace.GameObjectHit hit):base(actor,target){
                                                     mHit=hit;
          }
        public readonly Sims3.SimIFace.GameObjectHit mHit;
        protected override void PrivateException(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source);
        }
    }//  COPY COMPLETED
    //-----------------------------------------------------------------------------------------------------------
    //====================================================================================================================================================
    //
    //====================================================================================================================================================
    public class DerivativeSearch{
        public enum Caching{Default,
                            NoCache,
        }
        public enum Scope{
                          Module,
                          Global,
        }
        static Dictionary<Type,List<object>>sItems=new Dictionary<Type,List<object>>();
     static List<System.Reflection.Assembly>sModules=null;
                     readonly static string sModuleName=null;
          static DerivativeSearch(){
                                            sModuleName=VersionStamp.sNamespace+"Module";
          }
   public static List<T>Find<T>()where T:class{
                 return Find<T>(Caching.Default,Scope.Module);
   }
   public static List<T>Find<T>(Caching caching)where T:class{
                 return Find<T>(caching,Scope.Module);
   }
   public static List<T>Find<T>(Scope scope)where T:class{
                 return Find<T>(Caching.Default,scope);
   }
   public static List<T>Find<T>(Caching caching,Scope scope)where T:class{
                 return FindOfType<T>(typeof(T),caching,scope);
   }
   public static List<T>FindOfType<T>(Type searchType)where T:class{
                 return FindOfType<T>(searchType,Caching.Default,Scope.Global);
   }
   public static List<T>FindOfType<T>(Type searchType,Caching caching,Scope scope)where T:class{
                 List<T>list=new List<T>();
                 List<object>existing=null;
    if((caching==Caching.Default)&&(sItems.TryGetValue(searchType,out existing))){
                                                    foreach(T item in existing){
                        list.Add(item);
                                                    }
    }else{
                      System.Reflection.Assembly myAssembly=typeof(Interaction).Assembly;
     if(caching==Caching.Default){
                             existing=new List<object>();
       sItems.Add(searchType,existing);
     }
                 List<System.Reflection.Assembly>assemblies=sModules;
     if(scope!=Scope.Module){
                                                 assemblies=new List<System.Reflection.Assembly>(AppDomain.CurrentDomain.GetAssemblies());
     }else 
     if(assemblies==null){
        assemblies=new List<System.Reflection.Assembly>();
                    foreach(System.Reflection.Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()){
                                                   if((assembly!=myAssembly)&&(assembly.GetType(sModuleName)==null))continue;
        assemblies.Add(assembly);
                    }
                                                            sModules=assemblies;
     }
                    foreach(System.Reflection.Assembly assembly in assemblies){
                        try{
                                  foreach(Type type in assembly.GetTypes()){
                                            if(type.IsAbstract)             continue;
                                            if(type.IsGenericTypeDefinition)continue;
               if(!searchType.IsAssignableFrom(type))                       continue;
                                try{
 System.Reflection.ConstructorInfo constructor=type.GetConstructor(System.Reflection.BindingFlags.Instance|
                                                                   System.Reflection.BindingFlags.Public  |
                                                                   System.Reflection.BindingFlags.NonPublic,null,new Type[0],null);
                                if(constructor!=null){
                            T item=constructor.Invoke(new object[0]) as T;
                           if(item!=null){
                        list.Add(item);
              if(existing!=null){
                 existing.Add(item);
              }
                           }
                                }
                                }catch(Exception exception){
                                  Alive.WriteLog(exception.Message+"\n\n"+
                                                 exception.StackTrace+"\n\n"+
                                                 exception.Source);
                                }
                                  }
                        }catch(Exception exception){
                          Alive.WriteLog(exception.Message+"\n\n"+
                                         exception.StackTrace+"\n\n"+
                                         exception.Source);
                        }
                    }
    }
                 return list;
   }
    }
    //-----------------------------------------------------------------------------------------------------------
    //====================================================================================================================================================
    //
    //====================================================================================================================================================
    public abstract class ProtoVersionStamp{public static bool sPopupMenuStyle=(false);}
    //-----------------------------------------------------------------------------------------------------------
                  public class VersionStamp:ProtoVersionStamp{
        public static readonly string sNamespace="Alive";
                  public class Version:ProtoVersion<Sims3.Gameplay.Abstracts.GameObject>{
            protected override bool Allow(GameHitParameters<Sims3.Gameplay.Abstracts.GameObject>parameters){
                                                                     if(!Alive.IsRootMenuObject(parameters.mTarget))return(false);
                                                                              return base.Allow(parameters);
            }
                  }
           public static readonly int sVersion=0;
                  }
    //-----------------------------------------------------------------------------------------------------------
    public abstract class ProtoVersion<TTarget>:InteractionOptionItem<Sims3.Gameplay.Interfaces.IActor,TTarget,GameHitParameters<TTarget>>,IPrimaryOption<TTarget>,IVersionOption where TTarget:class,Sims3.Gameplay.Interfaces.IGameObject{
        public override string DisplayValue{get{return Sims3.SimIFace.EAText.GetNumberString(VersionStamp.sVersion);}}
        public override string GetTitlePrefix(){
            return"Version";
        }
        public virtual string Prompt{get{return Alive.Localize(GetTitlePrefix()+":Prompt",false,new object[]{VersionStamp.sVersion});}}
        protected override OptionResult Run(GameHitParameters<TTarget>parameters){
            Sims3.UI.SimpleMessageDialog.Show(Name,Prompt);
                    return OptionResult.SuccessClose;
        }
    }
    //-----------------------------------------------------------------------------------------------------------
    //====================================================================================================================================================
    //
    //====================================================================================================================================================
    public class IWasHereDefinition:Sims3.Gameplay.Interactions.ImmediateInteractionDefinition<Sims3.Gameplay.Interfaces.IActor,Sims3.Gameplay.Interfaces.IGameObject,Sims3.Gameplay.Abstracts.GameObject.DEBUG_Reset>{
public static readonly Sims3.Gameplay.Interactions.InteractionDefinition Singleton=new IWasHereDefinition();
        public override bool Test(Sims3.Gameplay.Interfaces.IActor actor,Sims3.Gameplay.Interfaces.IGameObject target,bool isAutonomous,ref Sims3.SimIFace.GreyedOutTooltipCallback greyedOutTooltipCallback){return(false);}
    }//  COPY COMPLETED
    //-----------------------------------------------------------------------------------------------------------
    //====================================================================================================================================================
    //  
    //====================================================================================================================================================
    public class SpeedTrap{
        public static void SetDelegates(Delegate onSleep){
                                         OnSleep=onSleep;
        }
                         static Delegate OnSleep;
                        public static void Sleep(){
                                           Sleep(0);
                        }
                        public static void Sleep(uint tickCount){
            try{
                if(Sims3.SimIFace.Simulator.CheckYieldingContext(false)){
                           End();
                   Sims3.SimIFace.Simulator.Sleep(tickCount);
                           Begin();
                }
            }catch(Sims3.SimIFace.ResetException exception){
                                  Alive.WriteLog(exception.Message+"\n\n"+
                                                 exception.StackTrace+"\n\n"+
                                                 exception.Source);
            }catch(     Exception exception){
                   Alive.WriteLog(exception.Message+"\n\n"+
                                  exception.StackTrace+"\n\n"+
                                  exception.Source);
            }
                        }
        public static void Begin(){
                                      if(OnSleep!=null){
                                         OnSleep.DynamicInvoke(new object[]{"Begin"});
                                      }
        }
        public static void End(){
                                      if(OnSleep!=null){
                                         OnSleep.DynamicInvoke(new object[]{"End"});
                                      }
        }
    }
    //-----------------------------------------------------------------------------------------------------------
    }
    public class Tunings{ 
        public static Sims3.Gameplay.Autonomy.InteractionTuning Inject<Target,OldType,NewType>(bool clone)where Target:Sims3.Gameplay.Interfaces.IGameObject
            where OldType:Sims3.Gameplay.Interactions.InteractionDefinition
            where NewType:Sims3.Gameplay.Interactions.InteractionDefinition
        {
            return Inject(typeof(OldType),typeof(Target),typeof(NewType),typeof(Target),clone);
        }
        protected static Sims3.Gameplay.Autonomy.InteractionTuning Inject(Type oldType,Type oldTarget,Type newType,Type newTarget,bool clone){
                         Sims3.Gameplay.Autonomy.InteractionTuning tuning=null;
            try{
                                                                   tuning=Sims3.Gameplay.Autonomy.AutonomyTuning.GetTuning(newType.FullName,newTarget.FullName);
                                                                if(tuning==null){
                                                                   tuning=Sims3.Gameplay.Autonomy.AutonomyTuning.GetTuning(oldType,
                                                                                                                           oldType.FullName,
                                                                                                                           oldTarget);
                                                                if(tuning==null)return null;
                                                                                                                                    if(clone){
                                                                   tuning=CloneTuning(tuning);
                                                                                                                                    }
                                                                          Sims3.Gameplay.Autonomy.AutonomyTuning.AddTuning(newType.FullName,newTarget.FullName,tuning);
                                                                }
                                                                          Sims3.Gameplay.Autonomy.InteractionObjectPair.sTuningCache.Remove(new Sims3.Gameplay.Utilities.Pair<Type,Type>(newType,newTarget));
            }catch(Exception exception){
              Alive.WriteLog(exception.Message+"\n\n"+
                             exception.StackTrace+"\n\n"+
                             exception.Source);
            }
                                                            return tuning;
        }
        private static Sims3.Gameplay.Autonomy.InteractionTuning CloneTuning(Sims3.Gameplay.Autonomy.InteractionTuning oldTuning){
Sims3.Gameplay.Autonomy.InteractionTuning newTuning=new Sims3.Gameplay.Autonomy.InteractionTuning();
                                          newTuning.mFlags                                       =oldTuning.mFlags;
                                          newTuning.ActionTopic                                  =oldTuning.ActionTopic;
                                          newTuning.AlwaysChooseBest                             =oldTuning.AlwaysChooseBest;
                                          newTuning.Availability               =CloneAvailability(oldTuning.Availability);
                                          newTuning.CodeVersion                                  =oldTuning.CodeVersion;
                                          newTuning.FullInteractionName                          =oldTuning.FullInteractionName;
                                          newTuning.FullObjectName                               =oldTuning.FullObjectName;
                                          newTuning.mChecks                      =Alive.CloneList(oldTuning.mChecks);
                                          newTuning.mTradeoff                      =CloneTradeoff(oldTuning.mTradeoff);
                                          newTuning.PosturePreconditions                         =oldTuning.PosturePreconditions;
                                          newTuning.ScoringFunction                              =oldTuning.ScoringFunction;
                                          newTuning.ScoringFunctionOnlyAppliesToSpecificCommodity=oldTuning.ScoringFunctionOnlyAppliesToSpecificCommodity;
                                          newTuning.ScoringFunctionString                        =oldTuning.ScoringFunctionString;
                                          newTuning.ShortInteractionName                         =oldTuning.ShortInteractionName;
                                          newTuning.ShortObjectName                              =oldTuning.ShortObjectName;
                                   return newTuning;
        }
        private static Sims3.Gameplay.Autonomy.Availability CloneAvailability(Sims3.Gameplay.Autonomy.Availability old){
Sims3.Gameplay.Autonomy.Availability result=new Sims3.Gameplay.Autonomy.Availability();
                                     result.mFlags                         =old.mFlags;
                                     result.AgeSpeciesAvailabilityFlags    =old.AgeSpeciesAvailabilityFlags;
                                     result.CareerThresholdType            =old.CareerThresholdType;
                                     result.CareerThresholdValue           =old.CareerThresholdValue;
                                     result.ExcludingBuffs =Alive.CloneList(old.ExcludingBuffs);
                                     result.ExcludingTraits=Alive.CloneList(old.ExcludingTraits);
                                     result.MoodThresholdType              =old.MoodThresholdType;
                                     result.MoodThresholdValue             =old.MoodThresholdValue;
                                     result.MotiveThresholdType            =old.MotiveThresholdType;
                                     result.MotiveThresholdValue           =old.MotiveThresholdValue;
                                     result.RequiredBuffs  =Alive.CloneList(old.RequiredBuffs);
                                     result.RequiredTraits =Alive.CloneList(old.RequiredTraits);
                                     result.SkillThresholdType             =old.SkillThresholdType;
                                     result.SkillThresholdValue            =old.SkillThresholdValue;
                                     result.WorldRestrictionType           =old.WorldRestrictionType;
                                     result.OccultRestrictions             =old.OccultRestrictions;
                                     result.OccultRestrictionType          =old.OccultRestrictionType;
                                     result.SnowLevelValue                 =old.SnowLevelValue;
                                     result.WorldRestrictionWorldNames=Alive.CloneList(old.WorldRestrictionWorldNames);
                                     result.WorldRestrictionWorldTypes=Alive.CloneList(old.WorldRestrictionWorldTypes);
                              return result;
        }
        private static Sims3.Gameplay.Autonomy.Tradeoff CloneTradeoff(Sims3.Gameplay.Autonomy.Tradeoff old){
Sims3.Gameplay.Autonomy.Tradeoff result=new Sims3.Gameplay.Autonomy.Tradeoff();
                                 result.mFlags                  =old.mFlags;
                                 result.mInputs =Alive.CloneList(old.mInputs);
                                 result.mName                   =old.mName;
                                 result.mNumParameters          =old.mNumParameters;
                                 result.mOutputs=Alive.CloneList(old.mOutputs);
                                 result.mVariableRestrictions   =old.mVariableRestrictions;
                                 result.TimeEstimate            =old.TimeEstimate;
                          return result;
        }
    }
    //-----------------------------------------------------------------------------------------------------------
    public class TVRepairTVFix:TV.RepairTV,IPreLoad,IAddInteraction{
        static InteractionDefinition sOldSingleton;
                                              public void AddInteraction(InteractionInjectorList interactions){
                                                                                                 interactions.Replace<TV,TV.RepairTV.Definition>(Singleton);
                                              }
                                   public void OnPreLoad(){
            Tunings.Inject<Sims3.Gameplay.Objects.Electronics.TVCheap             ,TV.RepairTV.Definition,Definition>(false);
            Tunings.Inject<Sims3.Gameplay.Objects.Electronics.TVModerate          ,TV.RepairTV.Definition,Definition>(false);
            Tunings.Inject<Sims3.Gameplay.Objects.Electronics.TVModerateFlatscreen,TV.RepairTV.Definition,Definition>(false);
            Tunings.Inject<Sims3.Gameplay.Objects.Electronics.TVExpensive         ,TV.RepairTV.Definition,Definition>(false);
            Tunings.Inject<Sims3.Gameplay.Objects.Electronics.TVWall              ,TV.RepairTV.Definition,Definition>(false);
            Tunings.Inject<Sims3.Gameplay.Objects.Electronics.TV,TV.RepairTV.Definition,Definition>(false);
                                     sOldSingleton=Singleton;
                                                   Singleton=new Definition();
                                   }
        public new class Definition:TV.RepairTV.Definition{
            public override InteractionInstance CreateInstance(ref InteractionInstanceParameters parameters){
                            InteractionInstance na=new TVRepairTVFix();
                                                na.Init(ref parameters);
                                         return na;
            }
            public override bool Test(Sim a,TV target,bool isAutonomous,ref GreyedOutTooltipCallback greyedOutTooltipCallback){
            return(target.Repairable!=null&&(target.Repairable.Broken&&target.CanPerformRepairsOrUpgrades))&&target.CanSimRepairThisTV(a,ref greyedOutTooltipCallback);
            }
        }
    }
}