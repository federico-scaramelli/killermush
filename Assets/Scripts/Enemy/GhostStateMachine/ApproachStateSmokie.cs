using UnityEngine;
using System.Collections;

public class ApproachStateSmokie : ISmokieState
{
    private readonly StatePatternSmokie smokie;
    

    public ApproachStateSmokie(StatePatternSmokie statePatternSmokie)
    {
        smokie = statePatternSmokie;
    }

    public void UpdateState()
    {

    }

    public void EnterState()
    {
        smokie.pathTimer = smokie.updatePathTimer;
        smokie.agent.speed = smokie.moveSpeed;
        smokie.agent.enabled = true;
        smokie.agent.updatePosition = true;
        smokie.agent.updateRotation = true;
    }

    public void FixedUpdateState()
    {
        RaycastHit hit;

        smokie.pathTimer -= Time.deltaTime;
        smokie.distance = Vector3.Distance(smokie.transform.position, smokie.target.position);

        if (smokie.distance <= smokie.range+ smokie.addedToRange)
        {
            if (Physics.Raycast(smokie.transform.position, smokie.transform.forward, out hit, smokie.range+ smokie.addedToRange))
            {
                if (hit.transform == smokie.target.transform)
                    ToMeleeAttackState();
                else if (hit.transform != smokie.target.transform)
                {
                    if (smokie.pathTimer <= 0)
                    {
                        smokie.pathTimer = smokie.updatePathTimer;
                        smokie.agent.SetDestination(smokie.target.position);
                    }
                }
                else if (hit.collider.transform == smokie.target.transform.GetChild(2))
                    ToAttackState();
            }
        }
        else
        {
            if (smokie.pathTimer <= 0)
            {
                smokie.pathTimer = smokie.updatePathTimer;
                smokie.agent.SetDestination(smokie.target.position);
            }
        }      
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
