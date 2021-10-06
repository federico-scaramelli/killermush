using UnityEngine;
using System.Collections;

public class PrepareAttackStateRhyno : IRhynoState
{
    private readonly StatePatternRhyno rhyno;

    public PrepareAttackStateRhyno(StatePatternRhyno statePatternRhyno)
    {
        rhyno = statePatternRhyno;
    }

    public void UpdateState()
    {
        RotateToTarget(rhyno.rotSpeed/100);
        //Play animation
        rhyno.timer -= Time.deltaTime;
        if (rhyno.timer <= 0)
            ToAttackState();
    }

    public void EnterState()
    {
        rhyno.timer = rhyno.prepareAttackTimer;
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

    void RotateToTarget(float rotSpeed)
    {
        Quaternion rotation = Quaternion.LookRotation(rhyno.target.position - rhyno.transform.position);
        rotation.x = 0; rotation.z = 0;
        rhyno.transform.rotation = Quaternion.Slerp(rhyno.transform.rotation, rotation, Time.deltaTime * rotSpeed);
    }
}
