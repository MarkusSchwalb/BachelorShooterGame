using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStateMashine : MonoBehaviour
{
    private BaseState currentState;
    

    private bool isTransitioning = false;

    // Update is called once per frame
    void Update()
    {

        currentState?.UpdateState(Time.deltaTime); //Question mark checks if it isn't null
    }

    public void SwitchState(BaseState nextState)
    {
        //Debug.Log("switchState");

        if (isTransitioning) {
            Debug.Log("isAlreadyTransitioning");
            Debug.LogWarning("probably tries to switch in an enter state you moron XD greetings past me just switch in update. Just telling you so you don't repeat the mistake, lots of love");
            Debug.LogWarning("transitioning between " + currentState + nextState + "has failed");
            return; 
        }
        isTransitioning = true;  //warum bist du auf true wenn go back to standard state gegangen wird wurde in respawn gefixt problem in enter darf man nicht switchen

        if (currentState != null)
        {
            currentState.ExitState();
            currentState = nextState;
            currentState.EnterState();
            isTransitioning = false;
        }
        else
        {
            currentState = nextState;
            currentState?.EnterState();
            isTransitioning = false;
        }

    }
    
    /*
    private void OnAnimatorMove()
    {
        currentState?.OnAnimatorMoveState();
    }
    */
}
