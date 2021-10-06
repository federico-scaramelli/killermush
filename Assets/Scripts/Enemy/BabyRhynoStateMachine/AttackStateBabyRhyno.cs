using UnityEngine;
using System.Collections;

public class AttackStateBabyRhyno : IBabyRhynoState
{
    private readonly StatePatternBabyRhyno babyRhyno;

    public AttackStateBabyRhyno(StatePatternBabyRhyno statePatternBabyRhyno)
    {
        babyRhyno = statePatternBabyRhyno;
    }

    public void UpdateState()
    {
        babyRhyno.timer -= Time.deltaTime;

        if (babyRhyno.timer <= 0)
            ToRestState();

        Move(babyRhyno.attackSpeed);
        
        RaycastHit hit;
        if (Physics.Raycast(babyRhyno.transform.up + babyRhyno.transform.position, babyRhyno.transform.up + babyRhyno.transform.forward * babyRhyno.range, out hit, 1.5f))
        {
            Health health = hit.transform.GetComponent<Health>();
            if (hit.transform == babyRhyno.target.transform)
            {
                health.Damage(babyRhyno.damage);
            }
            if (hit.rigidbody)
            {
                hit.rigidbody.AddForceAtPosition(babyRhyno.transform.forward * babyRhyno.hitForce, hit.point, ForceMode.Impulse);
            }
            babyRhyno.myRigidbody.AddForce(-babyRhyno.transform.forward * babyRhyno.hitForce*2, ForceMode.Impulse);
            ToRestState();
        }
    }

    public void EnterState()
    {
        babyRhyno.timer = babyRhyno.attackTimer;
        RotateToTarget(babyRhyno.rotSpeed);

        babyRhyno.agent.enabled = false;
        babyRhyno.agent.updatePosition = false;
        babyRhyno.agent.updateRotation = false;
    }

    public void FixedUpdateState()
    {

    }

    public void ToApproachState()
    {
        babyRhyno.approachState.EnterState();
        babyRhyno.currentState = babyRhyno.approachState;
    }

    public void ToAttackState()
    {
        babyRhyno.attackState.EnterState();
        babyRhyno.currentState = babyRhyno.attackState;
    }

    public void ToRestState()
    {
        babyRhyno.restState.EnterState();
        babyRhyno.currentState = babyRhyno.restState;
    }

    void RotateToTarget(float rotSpeed)
    {
        Quaternion rotation = Quaternion.LookRotation(babyRhyno.target.position - babyRhyno.transform.position);
        rotation.x = 0; rotation.z = 0;
        babyRhyno.transform.rotation = Quaternion.Slerp(babyRhyno.transform.rotation, rotation, Time.deltaTime * rotSpeed);
    }

    void Move(float speed)
    {
        Vector3 movement = babyRhyno.transform.forward * speed;
        babyRhyno.myRigidbody.AddForce(movement / Time.deltaTime);
    }
}
