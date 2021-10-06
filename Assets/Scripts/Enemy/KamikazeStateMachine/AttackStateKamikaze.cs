using UnityEngine;
using System.Collections;

public class AttackStateKamikaze : IKamikazeState
{
    private readonly StatePatternKamikaze kamikaze;

    public AttackStateKamikaze(StatePatternKamikaze statePatternKamikaze)
    {
        kamikaze = statePatternKamikaze;
    }

    public void UpdateState()
    {

    }

    public void EnterState()
    {
        kamikaze.pathTimer = kamikaze.updatePathTimer;
        kamikaze.agent.speed = kamikaze.attackSpeed;
    }

    public void FixedUpdateState()
    {
        kamikaze.pathTimer -= Time.deltaTime;
        if (kamikaze.pathTimer <= 0)
        {
            kamikaze.agent.SetDestination(kamikaze.target.position);
            kamikaze.pathTimer = kamikaze.updatePathTimer;
            kamikaze.distance = Vector3.Distance(kamikaze.transform.position, kamikaze.target.transform.position);
        }
        
        if (kamikaze.distance < kamikaze.rangeMin)
        {
            ToBoomState();
        }

        if (!kamikaze.transform.GetChild(0).transform.GetChild(1).transform.GetComponent<MeshRenderer>().enabled)
        {
            ToBoomState();
        }
    }

    public void ToApproachState()
    {
        kamikaze.approachState.EnterState();
        kamikaze.currentState = kamikaze.approachState;
    }

    public void ToAttackState()
    {
        kamikaze.attackState.EnterState();
        kamikaze.currentState = kamikaze.attackState;
    }

    public void ToBoomState()
    {
        kamikaze.boomState.EnterState();
        kamikaze.currentState = kamikaze.boomState;
    }
}
