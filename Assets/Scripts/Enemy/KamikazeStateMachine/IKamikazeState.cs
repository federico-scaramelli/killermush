using UnityEngine;
using System.Collections;

public interface IKamikazeState
{
    void UpdateState();

    void EnterState();

    void FixedUpdateState();

    void ToApproachState();

    void ToAttackState();

    void ToBoomState();
}
