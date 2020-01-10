using System;
using System.Collections.Generic;
using System.Text;
namespace ArthurGibraltarSims3Mod{
    public abstract class ModdedInteraction<SimObj>:Sims3.Gameplay.Interactions.ImmediateInteractionGameObjectHit<Sims3.Gameplay.Interfaces.IActor,SimObj>,IAddInteraction where SimObj:class,Sims3.Gameplay.Interfaces.IGameObject{
                                                                                                                                       public abstract void AddInteraction(InteractionInjectorList interactions);
        public override string GetInteractionName(){
            return"Modded";
        }
      protected virtual string GetInteractionName(Sims3.Gameplay.Interfaces.IActor actor,SimObj target,Sims3.SimIFace.GameObjectHit hit){
                        return GetInteractionName();
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
    }
    //-----------------------------------------------------------------------------------------------------------
    public interface IAddInteraction{
                 void AddInteraction(InteractionInjectorList interactions);
    }
    //-----------------------------------------------------------------------------------------------------------
    public class InteractionInjectorList{
          static InteractionInjectorList sMasterList=null;
                 public static List<Type>sAlwaysTypes;
          static InteractionInjectorList(){
                                         sAlwaysTypes=new List<Type>();
                                         sAlwaysTypes.Add(typeof(Sims3.Gameplay.Actors.Sim    ));
                                         sAlwaysTypes.Add(typeof(Sims3.Gameplay.Core  .Lot    ));
                                         sAlwaysTypes.Add(typeof(Sims3.Gameplay.Core  .Terrain));
          }
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
    }
    public interface IInteractionInjector{
        List<Type>GetTypes();
        void Perform(Sims3.Gameplay.Abstracts.GameObject obj,Dictionary<Type,bool>existing);
    }
    //====================================================================================================================================================
    //
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

                        ListOptions(null, sPopupOptions, actor, target, mHit, mPopup, path, results);
                    }
                    else
                    {
                        mPath = defPath;

                        base.AddInteractions(iop, actor, target, results);
                    }
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source);
                }
            }
        }
    }
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
    }
    //-----------------------------------------------------------------------------------------------------------
    public interface IInteractionProxy<TActor,TTarget,TParameters>where TParameters:InteractionOptionParameters<TActor,TTarget>{
                OptionResult Perform(IInteractionOptionItem<TActor,TTarget,TParameters>option,TParameters parameters);
    }
    //-----------------------------------------------------------------------------------------------------------
    public interface IInteractionOptionItem<TActor,TTarget,TParameters>:IModdedOptionItem where TParameters:InteractionOptionParameters<TActor,TTarget>{
        string GetTitlePrefix();
        bool Test(TParameters parameters);
                OptionResult Perform(TParameters parameters);
    }
    public enum OptionResult{
        Unset,
        SuccessRetain,
        SuccessLevelDown,
        SuccessClose,
        Failure,
    }
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
    }
    //-----------------------------------------------------------------------------------------------------------
    public interface IModdedOptionItem{
        string Name{get;}
        Sims3.SimIFace.ThumbnailKey Thumbnail{get;}
        string DisplayValue{get;}
        bool UsingCount{get;}
        int Count{get;set;}
        string DisplayKey{get;}
        int ValueWidth{get;}
        IModdedOptionItem Clone();
    }
    //====================================================================================================================================================
    //
    //====================================================================================================================================================
    public abstract class ProtoVersionStamp{public static bool sPopupMenuStyle=(false);}
                  public class VersionStamp:ProtoVersionStamp{
        public static readonly string sNamespace="Interaction.General";
                  public class Version:ProtoVersion<Sims3.Gameplay.Abstracts.GameObject>{
            protected override bool Allow(GameHitParameters<Sims3.Gameplay.Abstracts.GameObject>parameters){
                                                                           if(!IsRootMenuObject(parameters.mTarget))return(false);
                                                                              return base.Allow(parameters);
            }
                  }
        public static readonly int sVersion=0;
                  }
}