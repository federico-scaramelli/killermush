using UnityEngine;
using System.Collections;

public class RestStateBabyRhyno : IBabyRhynoState
{
    private readonly StatePatternBabyRhyno babyRhyno;

    public RestStateBabyRhyno(StatePatternBabyRhyno statePatternBabyRhyno)
    {
        babyRhyno = statePatternBabyRhyno;
    }

    public void UpdateState()
    {
        babyRhyno.timer -= Time.deltaTime;

        if (babyRhyno.timer <= 0)
            ToApproachState();

        babyRhyno.pathTimer -= Time.deltaTime;
        if (babyRhyno.pathTimer <= 0)
        {
            babyRhyno.agent.SetDestination(babyRhyno.target.position);
            babyRhyno.pathTimer = babyRhyno.updatePathTimer;
        }
    }

    public void EnterState()
    {
        babyRhyno.pathTimer = babyRhyno.updatePathTimer;
        babyRhyno.timer = babyRhyno.restTimer;
        babyRhyno.agent.speed = babyRhyno.restSpeed;
        babyRhyno.agent.enabled = true;
        babyRhyno.agent.updatePosition = true;
        babyRhyno.agent.updateRotation = true;
    }

    public void FixedUpdateState()
    {

    }

    public void ToApproachState()
    {
        babyRhyno.approachState.EnterState();
        babyRhyno.currentState = babyRhyno.approachState;
    }

    public void ToAttackState()
    {
        babyRhyno.attackState.EnterState();
        babyRhyno.currentState = babyRhyno.attackState;
    }

    public void ToRestState()
    {
        babyRhyno.restState.EnterState();
        babyRhyno.currentState = babyRhyno.restState;
    }
}
