using UnityEngine;
using System.Collections;

public class AttackStateRhyno : IRhynoState
{
    private readonly StatePatternRhyno rhyno;

    public AttackStateRhyno(StatePatternRhyno statePatternRhyno)
    {
        rhyno = statePatternRhyno;
    }

    public void UpdateState()
    {

    }

    public void EnterState()
    {
        rhyno.timer = rhyno.range / 9 / rhyno.attackSpeed;
        rhyno.agent.enabled = false;
        rhyno.myHealth.damageable = false;
        rhyno.agent.updatePosition = false;
        rhyno.agent.updateRotation = false;
    }

    public void FixedUpdateState()
    {
        rhyno.timer -= Time.deltaTime;
        Move(rhyno.attackSpeed*rhyno.myRigidbody.mass);
        RotateToTarget(rhyno.rotSpeed / 2);
        if (rhyno.timer <= 0)
        {
            rhyno.restTimer = rhyno.shortRestTimer;
            ToRestState();
        }
        Ray ray = new Ray(rhyno.attackCollisionChecker.transform.position, rhyno.attackCollisionChecker.transform.forward);
        RaycastHit hit;
        Debug.DrawRay(rhyno.attackCollisionChecker.transform.position, rhyno.attackCollisionChecker.transform.forward);
        if (Physics.Raycast(ray, out hit, 2.25f))
        {
            Health health = hit.transform.GetComponent<Health>();
            if (health)
            {
                health.Damage(rhyno.damage);
            }
        }
    }

    public void OnCollisionEnterState(Collision col)
    {
        
    }

    public void ToApproachState()
    {
        rhyno.approachState.EnterState();
        rhyno.currentState = rhyno.approachState;
    }

    public void ToPreAttackState()
    {
        rhyno.preAttackState.EnterState();
        rhyno.currentState = rhyno.preAttackState;
    }

    public void ToAttackState()
    {
        rhyno.attackState.EnterState();
        rhyno.currentState = rhyno.attackState;
    }

    public void ToRestState()
    {
        rhyno.restState.EnterState();
        rhyno.currentState = rhyno.restState;
    }

    void RotateToTarget(float rotSpeed)
    {
        Quaternion rotation = Quaternion.LookRotation(rhyno.target.position - rhyno.transform.position);
        rotation.x = 0; rotation.z = 0;
        rhyno.transform.rotation = Quaternion.Slerp(rhyno.transform.rotation, rotation, Time.deltaTime * rotSpeed);
    }

    void Move(float speed)
    {
        Vector3 movement = rhyno.transform.forward * speed;
        rhyno.myRigidbody.AddForce(movement / Time.deltaTime);
    }
}
