using UnityEngine;

public abstract class EnemyBaseState : BaseState
{
    protected BaseEnemyStateMashine stateMashine;
    public EnemyBaseState(BaseEnemyStateMashine sM)
    {
        stateMashine = sM;
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
}
