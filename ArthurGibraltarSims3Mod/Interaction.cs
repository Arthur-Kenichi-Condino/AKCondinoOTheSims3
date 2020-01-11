using System;
using System.Collections.Generic;
using System.Text;
namespace ArthurGibraltarSims3Mod{
    //====================================================================================================================================================
    //       ModdedInteraction:the injecting part:from NRaas mods
    //====================================================================================================================================================
    public abstract class ModdedInteraction<SimObj>:Sims3.Gameplay.Interactions.ImmediateInteractionGameObjectHit<Sims3.Gameplay.Interfaces.IActor,SimObj>,IAddInteraction where SimObj:class,Sims3.Gameplay.Interfaces.IGameObject{
                                                                                                                                       public abstract void AddInteraction(InteractionInjectorList interactions);
        public override string GetInteractionName(){
                try{
                   return base.GetInteractionName()+", Modded";
                }catch(Exception exception){
                  Alive.WriteLog(exception.Message+"\n\n"+
                                 exception.StackTrace+"\n\n"+
                                 exception.Source);
                   return"ERROR";
                }
        }
      protected virtual string GetInteractionName(Sims3.Gameplay.Interfaces.IActor actor,SimObj target,Sims3.SimIFace.GameObjectHit hit){
                        return GetInteractionName()+", user: "+actor.Name;
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
    public class InteractionInjectorList{
          static InteractionInjectorList sMasterList=null;
   public static InteractionInjectorList  MasterList{
    get{
                                      if(sMasterList==null){
                List<IAddInteraction>addInteractions=DerivativeSearch.Find<IAddInteraction>();
             foreach(IAddInteraction interaction in addInteractions){
             }
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
                                      AddInjector(new InteractionInjector<ObjType>(definition,true));
        }
        public void AddRoot(Sims3.Gameplay.Interactions.InteractionDefinition definition){
                                      AddInjector(new InteractionRootInjector(definition));
        }
        public void AddCustom(IInteractionInjector injector){
                                       AddInjector(injector);
        }
        public void AddNoDupTest<ObjType>(Sims3.Gameplay.Interactions.InteractionDefinition definition)where ObjType:Sims3.Gameplay.Interfaces.IGameObject{
                                      AddInjector(new InteractionNoDupTestInjector<ObjType>(definition));
        }
            public void Replace<ObjType,T>(Sims3.Gameplay.Interactions.InteractionDefinition definition)where ObjType:Sims3.Gameplay.Interfaces.IGameObject where T:Sims3.Gameplay.Interactions.InteractionDefinition{
                                              AddInjector(new InteractionReplacer<ObjType,T>(definition,true));
            }
            public void ReplaceNoTest<ObjType,T>(Sims3.Gameplay.Interactions.InteractionDefinition definition)where ObjType:Sims3.Gameplay.Interfaces.IGameObject where T:Sims3.Gameplay.Interactions.InteractionDefinition{
                                                    AddInjector(new InteractionReplacer<ObjType,T>(definition,false));
            }
        public void Perform(Sims3.Gameplay.Abstracts.GameObject obj){
                           List<IInteractionInjector>injectors=new List<IInteractionInjector>();
 foreach(KeyValuePair<Type,List<IInteractionInjector>>type in mTypes){
                                                   if(type.Key.IsAssignableFrom(obj.GetType())){
                                                     injectors.AddRange(type.Value);
                                                   }
 }
                                                  if(injectors.Count==0)return;

                    Perform(obj,injectors);
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
                                                      if(obj is ObjType){
                                                                                                     Type type=definition.GetType();
                                                                                                                                            if(existing.ContainsKey(type))return(false);
                                                                                                                                               existing.Add(        type,true);
                                                                                            obj.AddInteraction(definition);
                                                                                   obj.AddInventoryInteraction(definition);
                                                                                                                                                                          return( true);
                                                      }
                                                                                                                                                                          return(false);
}
    }//  COPY COMPLETED
    public class InteractionNoDupTestInjector<ObjType>:InteractionInjector<ObjType>where ObjType:Sims3.Gameplay.Interfaces.IGameObject{
       protected InteractionNoDupTestInjector(){}
          public InteractionNoDupTestInjector(Sims3.Gameplay.Interactions.InteractionDefinition definition):base(definition){}
protected override bool 
             Perform(Sims3.Gameplay.Abstracts.GameObject obj,Sims3.Gameplay.Interactions.InteractionDefinition definition,Dictionary<Type,bool>existing){
                                                      if(obj is ObjType){
                                                                                            obj.AddInteraction(definition);
                                                                                   obj.AddInventoryInteraction(definition);
                                                                                                                                                                          return( true);
                                                      }
                                                                                                                                                                          return(false);
}
    }//  COPY COMPLETED
    public class InteractionRootInjector:InteractionInjector<Sims3.Gameplay.Abstracts.GameObject>{
          public InteractionRootInjector(Sims3.Gameplay.Interactions.InteractionDefinition definition):base(definition,true){}
       public override List<Type>GetTypes(){
                       List<Type>list=new List<Type>();
                                 list.Add(typeof(Sims3.Gameplay.Actors.Sim));
                                 list.Add(typeof(Sims3.Gameplay.Core.  Lot));
                          return list;
       }
protected override bool 
             Perform(Sims3.Gameplay.Abstracts.GameObject obj,Sims3.Gameplay.Interactions.InteractionDefinition definition, Dictionary<Type, bool> existing){
                                               if(!IsRootMenuObject(obj))return(false);
 return base.Perform(obj,definition,existing);
}
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
    }//  COPY COMPLETED
    public class InteractionReplacer<ObjType,T>:InteractionInjector<ObjType>where ObjType:Sims3.Gameplay.Interfaces.IGameObject where T:Sims3.Gameplay.Interactions.InteractionDefinition{
          public InteractionReplacer(Sims3.Gameplay.Interactions.InteractionDefinition definition,bool testExistence):base(definition){
                                                                                        mTestExistence=testExistence;
          }
                                                                                   bool mTestExistence;
protected override bool 
             Perform(Sims3.Gameplay.Abstracts.GameObject obj,Sims3.Gameplay.Interactions.InteractionDefinition definition,Dictionary<Type,bool>existing){
                                                                                                                                     Type type=typeof(T);
                                                                                     if(mTestExistence){
                                                                                                                                           if(!existing.ContainsKey(type))return(false);
                                                                                     }
    if(!base.Perform(obj,definition,existing))return(false);
                                                                                             RemoveInteraction(obj,type);
                                                                                                                                               existing.Remove(type);
return( true);}
        public static void RemoveInteraction<Y>(Sims3.Gameplay.Abstracts.GameObject obj)where Y:Sims3.Gameplay.Interactions.InteractionDefinition{
                           RemoveInteraction(obj,typeof(Y));
        }
        public static void RemoveInteraction   (Sims3.Gameplay.Abstracts.GameObject obj,Type type){
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
        }
    }//  COPY COMPLETED
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
            }
            public override bool Test(Sims3.Gameplay.Interfaces.IActor a,TTarget target,bool isAutonomous,ref Sims3.SimIFace.GreyedOutTooltipCallback greyedOutTooltipCallback){
                       if(!sTest.Test(a,target,mHit,ref greyedOutTooltipCallback))return(false);
                                                                                  return(!isAutonomous);
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
    public interface ICloseDialogOption
    { }
    [Persistable]
    public abstract class CommonOptionItem : ICommonOptionItem
    {
        protected string mName;

        [Persistable(false)]
        protected int mCount = -1;

        [Persistable(false)]
        protected ThumbnailKey mThumbnail = ThumbnailKey.kInvalidThumbnailKey;

        public CommonOptionItem()
        { }
        public CommonOptionItem(string name)
            : this(name, -1)
        { }
        public CommonOptionItem(string name, int count)
        {
            mName = name;
            mCount = count;
        }
        public CommonOptionItem(string name, int count, string icon, ProductVersion version)
            : this(name, count)
        {
            SetThumbnail(icon, version);
        }
        public CommonOptionItem(string name, int count, ResourceKey icon)
            : this(name, count)
        {
            SetThumbnail(icon);
        }
        public CommonOptionItem(string name, int count, ThumbnailKey thumbnail)
            : this(name, count)
        {
            mThumbnail = thumbnail;
        }
        public CommonOptionItem(CommonOptionItem source)
            : this(source.mName, source.mCount, source.mThumbnail)
        { }

        public void SetThumbnail(string icon, ProductVersion version)
        {
            SetThumbnail (ResourceKey.CreatePNGKey(icon, ResourceUtils.ProductVersionToGroupId(version)));
        }
        public void SetThumbnail(ThumbnailKey key)
        {
            mThumbnail = key;
        }
        public void SetThumbnail(ResourceKey icon)
        {
            mThumbnail = new ThumbnailKey(icon, ThumbnailSize.Medium);
        }

        public virtual string Name
        {
            get
            {
                return mName;
            }
        }

        public ThumbnailKey Thumbnail
        {
            get
            {
                return mThumbnail;
            }
        }

        public abstract string DisplayValue
        {
            get;
        }

        public bool IsSet
        {
            get { return (Count > 0); }
        }

        public virtual bool UsingCount
        {
            get { return (Count != -1); }
        }

        public int Count
        {
            get { return mCount; }
            set { mCount = value; }
        }

        public virtual string DisplayKey
        {
            get
            {
                return null;
            }
        }

        public virtual int ValueWidth
        {
            get { return 0; }
        }

        public void IncCount()
        {
            mCount++;
        }
        public void IncCount(int count)
        {
            mCount += count;
        }

        public virtual void Reset()
        {
            if (UsingCount)
            {
                mCount = 0;
            }
        }

        public override string ToString()
        {
            string displayValue = DisplayValue;
            if (displayValue != null)
            {
                return Name + " = " + displayValue;
            }
            else
            {
                return Name;
            }
        }

        public static int SortByName(CommonOptionItem l, CommonOptionItem r)
        {
            return l.Name.CompareTo(r.Name);
        }

        public virtual ICommonOptionItem Clone()
        {
            return MemberwiseClone() as ICommonOptionItem;
        }
    }
    public abstract class ModdedOptionList<T> : CommonOptionItem
        where T : class, ICommonOptionItem
    {
        public CommonOptionList()
        { }
        public CommonOptionList(string name)
            : base(name)
        { }

        protected virtual int NumSelectable
        {
            get { return 1; }
        }

        public static int OnNameCompare(T left, T right)
        {
            try
            {
                return left.Name.CompareTo(right.Name);
            }
            catch (Exception e)
            {
                Common.Exception(Common.NewLine + "Left: " + left.GetType() + Common.NewLine + "Right: " + right.GetType(), e);
                return 0;
            }
        }

        public static List<T> AllOptions()
        {
            List<T> items = new List<T>();
            foreach (T item in Common.DerivativeSearch.Find<T>())
            {
                ICommonOptionListProxy<T> proxy = item as ICommonOptionListProxy<T>;
                if (proxy != null)
                {
                    proxy.GetOptions(items);
                }
                else
                {
                    items.Add(item);
                }
            }

            items.Sort(new Comparison<T>(OnNameCompare));

            return items;
        }

        public abstract List<T> GetOptions();
    }
    public interface IInteractionOptionList<TTarget> : IInteractionOptionItem<IActor, TTarget, GameHitParameters< TTarget>>, IInteractionProxy<IActor, TTarget, GameHitParameters< TTarget>>
        where TTarget : class, IGameObject
    {
        List<IInteractionOptionItem<IActor, TTarget, GameHitParameters< TTarget>>> IOptions();
    }
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
                                            sModuleName=VersionStamp.sNamespace+" Base Module";
          }

            public static List<T> Find<T>()
                where T : class
            {
                return Find<T>(Caching.Default, Scope.Module);
            }
            public static List<T> Find<T>(Caching caching)
                where T : class
            {
                return Find<T>(caching, Scope.Module);
            }
            public static List<T> Find<T>(Scope scope)
                where T : class
            {
                return Find<T>(Caching.Default, scope);
            }
            public static List<T> Find<T>(Caching caching, Scope scope)
                where T : class
            {
                return FindOfType<T>(typeof(T), caching, scope);
            }

            public static List<T> FindOfType<T>(Type searchType)
                where T : class
            {
                return FindOfType<T>(searchType, Caching.Default, Scope.Global);
            }
            public static List<T> FindOfType<T>(Type searchType, Caching caching, Scope scope)
                where T : class
            {
                List<T> list = new List<T>();

                List<object> existing = null;
                if ((caching == Caching.Default) && (sItems.TryGetValue(searchType, out existing)))
                {
                    foreach (T item in existing)
                    {
                        list.Add(item);
                    }
                }
                else
                {
                    Assembly myAssembly = typeof(Common).Assembly;

                    if (caching == Caching.Default)
                    {
                        existing = new List<object>();
                        sItems.Add(searchType, existing);
                    }

                    Common.StringBuilder msg = new Common.StringBuilder();

                    List<Assembly> assemblies = sModules;

                    if (scope != Scope.Module)
                    {
                        assemblies = new List<Assembly>(AppDomain.CurrentDomain.GetAssemblies());
                    }
                    else if (assemblies == null)
                    {
                        assemblies = new List<Assembly>();

                        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                        {
                            if ((assembly != myAssembly) && (assembly.GetType(sModuleName) == null)) continue;

                            assemblies.Add(assembly);
                        }

                        sModules = assemblies;
                    }

                    foreach (Assembly assembly in assemblies)
                    {
                        try
                        {
                            foreach (Type type in assembly.GetTypes())
                            {
                                if (type.IsAbstract) continue;

                                if (type.IsGenericTypeDefinition) continue;

                                if (!searchType.IsAssignableFrom(type)) continue;

                                try
                                {
                                    System.Reflection.ConstructorInfo constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[0], null);
                                    if (constructor != null)
                                    {
                                        T item = constructor.Invoke(new object[0]) as T;
                                        if (item != null)
                                        {
                                            list.Add(item);

                                            if (existing != null)
                                            {
                                                existing.Add(item);
                                            }
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    msg += Common.NewLine + type.ToString();
                                    msg += Common.NewLine + e.Message;
                                    msg += Common.NewLine + e.StackTrace;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            msg += Common.NewLine + assembly.FullName;
                            msg += Common.NewLine + e.Message;
                            msg += Common.NewLine + e.StackTrace;
                        }
                    }

                    WriteLog(msg);
                }

                return list;
            }
        }
    //====================================================================================================================================================
    //
    //====================================================================================================================================================
    public abstract class ProtoVersionStamp{public static bool sPopupMenuStyle=(false);}
                  public class VersionStamp:ProtoVersionStamp{
        public static readonly string sNamespace="Interaction";
                  public class Version:ProtoVersion<Sims3.Gameplay.Abstracts.GameObject>{
            protected override bool Allow(GameHitParameters<Sims3.Gameplay.Abstracts.GameObject>parameters){
                                                                           if(!IsRootMenuObject(parameters.mTarget))return(false);
                                                                              return base.Allow(parameters);
            }
                  }
        public static readonly int sVersion=0;
                  }
    //====================================================================================================================================================
    //
    //====================================================================================================================================================
    public class IWasHereDefinition:Sims3.Gameplay.Interactions.ImmediateInteractionDefinition<Sims3.Gameplay.Interfaces.IActor,Sims3.Gameplay.Interfaces.IGameObject,GameObject.DEBUG_Reset>{
public static readonly Sims3.Gameplay.Interactions.InteractionDefinition Singleton=new IWasHereDefinition();
        public override bool Test(Sims3.Gameplay.Interfaces.IActor actor,Sims3.Gameplay.Interfaces.IGameObject target,bool isAutonomous,ref Sims3.SimIFace.GreyedOutTooltipCallback greyedOutTooltipCallback){return(false);}
    }//  COPY COMPLETED
}