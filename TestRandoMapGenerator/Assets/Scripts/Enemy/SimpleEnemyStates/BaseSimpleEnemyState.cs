using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSimpleEnemyState : BaseState
{
    protected SimpleEnemy stateMashine;

    public BaseSimpleEnemyState(SimpleEnemy simpleEnemy)
    {
        stateMashine = simpleEnemy;
    }

    protected void FaceToDirection(Vector3 direction, float deltaTime)
    {
        stateMashine.transform.rotation = Quaternion.Lerp(stateMashine.transform.rotation, Quaternion.LookRotation(direction), deltaTime * stateMashine.RotationSpeed);    //rotate to the direction of movement
    }

    protected void FaceToPlayer(float deltaTime)
    {
        if (stateMashine.Player == null) { return; }

        Vector3 direction = stateMashine.Player.transform.position - stateMashine.transform.position;

        stateMashine.transform.rotation = Quaternion.Lerp(stateMashine.transform.rotation, Quaternion.LookRotation(direction), deltaTime * stateMashine.RotationSpeed);    //rotate to the direction of movement
    }

    protected void FacePlayer()
    {
        if (stateMashine.Player == null) { return; }

        Vector3 direction = stateMashine.Player.transform.position - stateMashine.transform.position;

        stateMashine.transform.rotation = Quaternion.LookRotation(direction);
    }

    protected void FacePlayerDirection(Vector3 direction)
    {
        stateMashine.transform.rotation = Quaternion.LookRotation(direction);    //rotate to the direction of movement
    }
    /*
    private void TryApplyForce()
    {
        if (appliedForce) { return; }
        stateMashine.EnemyForceReceiver.AddForce(stateMashine.transform.forward * currentAttack.ForceAmount);

        appliedForce = true;
    }
    */
    /*
    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMashine.CharacterController.Move((motion + stateMashine.EnemyForceReceiver.Movement) * deltaTime);
    }

    protected void MoveMitRoot(Vector3 motion, float deltaTime)
    {
        stateMashine.CharacterController.Move(motion + (stateMashine.EnemyForceReceiver.Movement * deltaTime));
    }
    protected void MoveMitRoot()
    {
        stateMashine.CharacterController.Move(stateMashine.EnemyAnimator.deltaPosition + (stateMashine.EnemyForceReceiver.Movement * Time.deltaTime));
    }

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }*/
    /*
    protected void RootMotionNavMeshUpdatePosRot()
    {
        Vector3 rootPosition = stateMashine.Animator.rootPosition;
        rootPosition.y = stateMashine.meshAgent.nextPosition.y;
        stateMashine.transform.position = rootPosition;
        stateMashine.transform.rotation = stateMashine.EnemyAnimator.rootRotation;
        stateMashine.Agent.nextPosition = rootPosition;
    }*/
}
