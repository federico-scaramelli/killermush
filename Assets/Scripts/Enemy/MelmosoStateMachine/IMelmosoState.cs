using UnityEngine;
using System.Collections;

public interface IMelmosoState
{
    void UpdateState();

    void EnterState();

    void FixedUpdateState();

    void ToApproachState();

    void ToAttackState();

    void ToSuicideState();
}
