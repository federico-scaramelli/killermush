using UnityEngine;
using System.Collections;

public interface IGlaciusState {
	
	void UpdateState();

    void EnterState();

    void FixedUpdateState();

    void ToApproachState();

    void ToDistanceAttackState();

    void ToMeleeAttackState();

    void ToIceShieldState();
}