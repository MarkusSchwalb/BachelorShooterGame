using UnityEngine;
using static ScreenFx.ScreenFx;

public abstract class BaseSoldierState : EnemyBaseState<SoldierStateMashine>
{
    //protected SoldierStateMashine soldierStateMashine;
    protected BaseSoldierState(SoldierStateMashine sM) : base(sM)
    {
        
    }


    
}
