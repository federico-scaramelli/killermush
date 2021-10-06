using UnityEngine;
using System.Collections;

public class MeleeAtkStateSmokie : ISmokieState
{
    private readonly StatePatternSmokie smokie;
    private RaycastHit hit;

    public MeleeAtkStateSmokie(StatePatternSmokie statePatternSmokie)
    {
        smokie = statePatternSmokie;
    }

    public void UpdateState()
    {
        smokie.agent.SetDestination(smokie.target.transform.position);
    }

    public void EnterState()
    {
        smokie.timer = smokie.range / 8 / smokie.attackSpeed;
        smokie.agent.enabled = true;
        smokie.agent.updatePosition = true;
        smokie.agent.updateRotation = true;
    }

    public void FixedUpdateState()
    {
        if (Physics.Raycast(smokie.transform.position, smokie.transform.forward, out hit, smokie.range))
        {
            if (hit.collider.transform == smokie.target.transform.GetChild(2))
            {
                ToAttackState();
                return;
            }
        }

        if (smokie.canAttack)
        {
            smokie.timer -= Time.deltaTime;
            Move(smokie.attackSpeed * smokie.myRigidbody.mass);
            RotateToTarget(smokie.rotSpeed / 10);
            if (smokie.timer <= 0)
            {
                smokie.timer = smokie.timeBetweenAttacks;
                smokie.canAttack = false;
            }
            if(smokie.collideWithPlayer)
                smokie.playerHealth.Damage(smokie.meleeDamage);
        }
        else
        {
            smokie.timer -= Time.deltaTime;
            if(smokie.timer <= 0)
            {
                smokie.canAttack = true;
                ToMeleeAttackState();
            }
        }
    }

    public void OnCollisionEnterState(Collision col)
    {
        
    }

    public void ToApproachState()
    {
        smokie.approachState.EnterState();
        smokie.currentState = smokie.approachState;
    }

    public void ToMeleeAttackState()
    {
        smokie.meleeAttackState.EnterState();
        smokie.currentState = smokie.meleeAttackState;
    }

    public void ToAttackState()
    {
        smokie.attackState.EnterState();
        smokie.currentState = smokie.attackState;
    }

    public void ToRestState()
    {
        smokie.restState.EnterState();
        smokie.currentState = smokie.restState;
    }

    void RotateToTarget(float rotSpeed)
    {
        Quaternion rotation = Quaternion.LookRotation(smokie.target.position - smokie.transform.position);
        rotation.x = 0; rotation.z = 0;
        smokie.transform.rotation = Quaternion.Slerp(smokie.transform.rotation, rotation, Time.deltaTime * rotSpeed);
    }

    void Move(float speed)
    {
        Vector3 movement = smokie.transform.forward * speed;
        smokie.myRigidbody.AddForce(movement / Time.deltaTime);
    }
}
