using UnityEngine;
using System.Collections;

public interface IRhynoState
{
    void UpdateState();

    void EnterState();

    void FixedUpdateState();

    void OnCollisionEnterState(Collision col);

    void ToApproachState();

    void ToPreAttackState();

    void ToAttackState();

    void ToRestState();
}
