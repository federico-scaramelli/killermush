using UnityEngine;
using System.Collections;

public class ApproachStateRhyno : IRhynoState
{
    private readonly StatePatternRhyno rhyno;
    private RaycastHit hit;

    public ApproachStateRhyno(StatePatternRhyno statePatternRhyno)
    {
        rhyno = statePatternRhyno;
    }

    public void UpdateState()
    {

    }

    public void EnterState()
    {
        rhyno.pathTimer = rhyno.updatePathTimer;
        rhyno.agent.speed = rhyno.moveSpeed;
        rhyno.agent.enabled = true;
        rhyno.agent.updatePosition = true;
        rhyno.agent.updateRotation = true;
    }

    public void FixedUpdateState()
    {
        rhyno.pathTimer -= Time.deltaTime;
        if (rhyno.pathTimer <= 0)
        {
            rhyno.pathTimer = rhyno.updatePathTimer;
            rhyno.distance = Vector3.Distance(rhyno.transform.position, rhyno.target.position);
            if (rhyno.distance < rhyno.range)
                if (Physics.Raycast(rhyno.transform.position, rhyno.transform.forward, out hit, rhyno.range))
                    if (hit.transform != rhyno.target.transform)
                        rhyno.agent.SetDestination(rhyno.target.position);
                    else
                        ToPreAttackState();
                else
                    ToPreAttackState();
            else
                rhyno.agent.SetDestination(rhyno.target.position);
        }
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
