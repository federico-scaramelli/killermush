using UnityEngine;
using System.Collections;

public class ApproachStateMelmoso : IMelmosoState
{
    private readonly StatePatternMelmoso melmoso;

    public ApproachStateMelmoso (StatePatternMelmoso statePatternMelmoso)
    {
        melmoso = statePatternMelmoso;
    }

    public void UpdateState()
    {

    }

    public void EnterState()
    {
        melmoso.pathTimer = melmoso.updatePathTimer;
    }

    public void FixedUpdateState()
    {
        melmoso.pathTimer -= Time.deltaTime;
        if (melmoso.pathTimer <= 0)
        {
            melmoso.agent.SetDestination(melmoso.target.position);
            melmoso.pathTimer = melmoso.updatePathTimer;
        }

        melmoso.distance = Vector3.Distance(melmoso.transform.position, melmoso.target.position);
        if (melmoso.distance < melmoso.attackRange)
            ToAttackState();
    }

    public void ToApproachState()
    {
        melmoso.approachState.EnterState();
        melmoso.currentState = melmoso.approachState;
    }

    public void ToAttackState()
    {
        melmoso.attackState.EnterState();
        melmoso.currentState = melmoso.attackState;
    }

    public void ToSuicideState()
    {
        melmoso.suicideState.EnterState();
        melmoso.currentState = melmoso.suicideState;
    }
}
