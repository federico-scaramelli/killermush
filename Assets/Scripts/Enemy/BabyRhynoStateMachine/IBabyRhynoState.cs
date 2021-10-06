using UnityEngine;
using System.Collections;

public interface IBabyRhynoState
{
    void UpdateState();

    void EnterState();

    void FixedUpdateState();

    void ToApproachState();

    void ToAttackState();

    void ToRestState();
}
