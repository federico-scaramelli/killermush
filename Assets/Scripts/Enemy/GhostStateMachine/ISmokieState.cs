using UnityEngine;
using System.Collections;

public interface ISmokieState 
{
    void UpdateState();

    void EnterState();

    void FixedUpdateState();

    void OnCollisionEnterState(Collision col);

    void ToApproachState();

    void ToMeleeAttackState();

    void ToAttackState();

    void ToRestState();
}
