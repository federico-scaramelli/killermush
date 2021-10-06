using UnityEngine;
using System.Collections;

public class MeleeAttackStateGlacius : IGlaciusState {

    private readonly StatePatternGlacius glacius;

    public MeleeAttackStateGlacius(StatePatternGlacius statePatternGlacius)
    {
        glacius = statePatternGlacius;
    }

    public void UpdateState()
    {
        glacius.distance = Vector3.Distance(glacius.transform.position, glacius.target.transform.position);
        if (glacius.distance > glacius.meleeAttackRange)
            ToDistanceAttackState();
    }

    public void EnterState()
    {

    }

    public void FixedUpdateState()
    {

    }

    public void ToApproachState()
    {
        glacius.approachState.EnterState();
        glacius.currentState = glacius.approachState;
    }

    public void ToDistanceAttackState()
    {
        glacius.distanceAttackState.EnterState();
        glacius.currentState = glacius.distanceAttackState;
    }

    public void ToMeleeAttackState()
    {
        glacius.meleeAttackState.EnterState();
        glacius.currentState = glacius.meleeAttackState;
    }

    public void ToIceShieldState()
    {
        glacius.iceShieldState.EnterState();
        glacius.currentState = glacius.iceShieldState;
    }
}