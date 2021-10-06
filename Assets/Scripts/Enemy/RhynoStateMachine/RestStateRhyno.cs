using UnityEngine;
using System.Collections;

public class RestStateRhyno : IRhynoState
{
    private readonly StatePatternRhyno rhyno;

    public RestStateRhyno(StatePatternRhyno statePatternRhyno)
    {
        rhyno = statePatternRhyno;
    }

    public void UpdateState()
    {
        //Animation
        rhyno.timer -= Time.deltaTime;
        //rhyno.distance = Vector3.Distance(rhyno.transform.position, rhyno.target.position);
        if (rhyno.timer <= 0)
             ToApproachState();
    }

    public void EnterState()
    {
        rhyno.timer = rhyno.restTimer;
        rhyno.myHealth.damageable = true;
        rhyno.agent.enabled = true;
        rhyno.agent.updatePosition = true;
        rhyno.agent.updateRotation = true;
    }

    public void FixedUpdateState()
    {

    }

    public void OnCollisionEnterState(Collision col)
    {

    }

    public void ToApproachState()
    {
        rhyno.approachState.EnterState();
        rhyno.currentState = rhyno.approachState;
    }

    public void ToPreAttackState()
    {
        rhyno.preAttackState.EnterState();
        rhyno.currentState = rhyno.preAttackState;
    }

    public void ToAttackState()
    {
        rhyno.attackState.EnterState();
        rhyno.currentState = rhyno.attackState;
    }

    public void ToRestState()
    {
        rhyno.restState.EnterState();
        rhyno.currentState = rhyno.restState;
    }
}
