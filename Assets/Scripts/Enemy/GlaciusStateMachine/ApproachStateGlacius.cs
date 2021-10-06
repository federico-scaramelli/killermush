using UnityEngine;
using System.Collections;

public class ApproachStateGlacius : IGlaciusState {
	
	private readonly StatePatternGlacius glacius;
    private RaycastHit hit;

    public ApproachStateGlacius(StatePatternGlacius statePatternGlacius)
    {
        glacius = statePatternGlacius;
    }
	
	public void UpdateState()
    {
        
    }

    public void EnterState()
    {
        glacius.pathTimer = glacius.updatePathTimer;
        glacius.agent.speed = glacius.moveSpeed;
        glacius.agent.enabled = true;
        glacius.agent.updatePosition = true;
        glacius.agent.updateRotation = true;
    }

    public void FixedUpdateState()
    {
        glacius.pathTimer -= Time.deltaTime;
        if (glacius.pathTimer <= 0)
        {
            glacius.agent.SetDestination(glacius.target.position);
            glacius.pathTimer = glacius.updatePathTimer;
        }

        glacius.distance = Vector3.Distance(glacius.transform.position, glacius.target.position);

        if (glacius.distance < glacius.distanceAttackRange)
        {
            ToDistanceAttackState();

            /*glacius.timer3 -= Time.deltaTime;
            if (glacius.timer3 <= 0)
            {
                glacius.timer3 = glacius.checkIfPlayerIsForwardTimer;
                Debug.DrawLine(glacius.transform.up + glacius.transform.position, glacius.transform.up + glacius.transform.forward * glacius.distanceAttackRange + glacius.transform.position);
                if (Physics.Raycast(glacius.transform.up + glacius.transform.position, glacius.transform.up + glacius.transform.forward * glacius.distanceAttackRange, out hit, glacius.distanceAttackRange + 1))
                {
                    Debug.Log(hit.transform);
                    if (hit.transform.tag != "Enemy" && hit.transform.tag != "Obstacle")
                        ToDistanceAttackState();
                }else{
                    ToApproachState();
                }
            }*/
        }
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

    void RotateToTarget(float rotSpeed)
    {
        Quaternion rotation = Quaternion.LookRotation(glacius.target.position - glacius.transform.position);
        rotation.x = 0; rotation.z = 0;
        glacius.transform.rotation = Quaternion.Slerp(glacius.transform.rotation, rotation, Time.deltaTime * rotSpeed);
    }
}