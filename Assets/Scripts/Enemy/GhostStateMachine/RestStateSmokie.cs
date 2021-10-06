using UnityEngine;
using System.Collections;

public class RestStateSmokie : ISmokieState
{
    private readonly StatePatternSmokie smokie;

    public RestStateSmokie(StatePatternSmokie statePatternSmokie)
    {
        smokie = statePatternSmokie;
    }

    public void UpdateState()
    {
        //Animation
        smokie.timer -= Time.deltaTime;
        //smokie.distance = Vector3.Distance(smokie.transform.position, smokie.target.position);
        if (smokie.timer <= 0)
            ToApproachState();
    }

    public void EnterState()
    {
        Debug.Log("REST");
        smokie.timer = smokie.timeBetweenAttacks;
        smokie.myHealth.damageable = true;
        smokie.agent.enabled = true;
        smokie.agent.updatePosition = true;
        smokie.agent.updateRotation = true;
    }

    public void FixedUpdateState()
    {
        
    }

    public void OnCollisionEnterState(Collision col)
    {

    }

    public void ToApproachState()
    {
        smokie.approachState.EnterState();
        smokie.currentState = smokie.approachState;
    }

    public void ToMeleeAttackState()
    {
        smokie.meleeAttackState.EnterState();
        smokie.currentState = smokie.meleeAttackState;
    }

    public void ToAttackState()
    {
        smokie.attackState.EnterState();
        smokie.currentState = smokie.attackState;
    }

    public void ToRestState()
    {
        smokie.restState.EnterState();
        smokie.currentState = smokie.restState;
    }
}
