using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class addAnimationEvents {

  
    private static List<AddedAnimationEvent> addedAnimationEvents = new List<AddedAnimationEvent>();
    

	// Use this for initialization
    public static void addAnimationEvent(AnimationClip animation, string function, float time)
    {
        AddedAnimationEvent addedAnimEvent = new AddedAnimationEvent(animation,function,time);
        if (isNewAnimationEvent(addedAnimEvent))
        {
            AnimationEvent animationEvent = new AnimationEvent();
            animationEvent.functionName = function;
            animationEvent.time = time;
            animation.AddEvent(animationEvent);
            addedAnimationEvents.Add(addedAnimEvent);

        }
        
        

       
    }
    private static bool isNewAnimationEvent(AddedAnimationEvent animationEvent)
    {
        bool isNew = true;
        foreach(AddedAnimationEvent AAE in addedAnimationEvents){
            if (AAE.isEqual(animationEvent))
            {
                isNew = false;
            }
        }
        return isNew;
    }


	
}
public class AddedAnimationEvent
{
   public AnimationClip animation;
   public string function;
   public float time;
   public AddedAnimationEvent(AnimationClip animation, string function, float time){
       this.animation = animation;
       this.function = function;
       this.time = time;
   }
   
    public bool isEqual(AddedAnimationEvent animEvt)
    {
        return this.animation == animEvt.animation && this.function == animEvt.function && this.time == animEvt.time;
    }
    public bool isEqual(AnimationClip animation, string function, float time)
    {
        return this.animation == animation && this.function == function && this.time == time;
    }
}
