using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState 
{

    public abstract void EnterState();

    public abstract void UpdateState(float DeltaTime);
    public abstract void ExitState();

    public abstract void OnAnimatorMoveState();

    protected float GetNormalizedTime(Animator animator, string AnimationTag) 
    {
        //get info from the current and the next animation
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        //if we are transitioning (in Animation Layer 0) and the next animation has the wanted Tag than return the progression time of the next Animation
        if (animator.IsInTransition(0) && nextInfo.IsTag(AnimationTag))
        {
            return nextInfo.normalizedTime;
        }
        //if we are not transitioning and the current tag is the wanted tag than retur the current progresstime
        else if (!animator.IsInTransition(0) && currentInfo.IsTag(AnimationTag))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }
}
