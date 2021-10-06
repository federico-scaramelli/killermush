using UnityEngine;
using System.Collections;

public class AttackStateMelmoso : IMelmosoState
{
    private readonly StatePatternMelmoso melmoso;

    public AttackStateMelmoso(StatePatternMelmoso statePatternMelmoso)
    {
        melmoso = statePatternMelmoso;
    }

    public void UpdateState()
    {
        RotateToTarget(melmoso.rotSpeed);
        melmoso.timer -= Time.deltaTime;

        if (melmoso.timer <= 0)
        {
            Transform ball = (Transform)GameObject.Instantiate(melmoso.skifBall, melmoso.shootPoint.position, Quaternion.identity);
            ball.transform.GetComponent<Rigidbody>().velocity = BallisticVel(melmoso.target, melmoso.shootAngle);
            melmoso.timer = melmoso.attackTimer;
        }

        melmoso.distance = Vector3.Distance(melmoso.transform.position, melmoso.target.position);
        if (melmoso.distance > melmoso.attackRange)
            ToApproachState();

        if(melmoso.distance < melmoso.dangerRange)
        {
            ToSuicideState();
        }
    }

    public void EnterState()
    {

    }

    public void FixedUpdateState()
    {

    }

    public void ToApproachState()
    {
        melmoso.approachState.EnterState();
        melmoso.currentState = melmoso.approachState;
    }

    public void ToAttackState()
    {
        melmoso.attackState.EnterState();
        melmoso.currentState = melmoso.attackState;
    }

    public void ToSuicideState()
    {
        melmoso.suicideState.EnterState();
        melmoso.currentState = melmoso.suicideState;
    }

    Vector3 BallisticVel(Transform target, float angle)
    {
        Vector3 pos = new Vector3();
        pos.z = target.position.z + Random.Range(-10/melmoso.precision, 10/melmoso.precision);
        pos.x = target.position.x + Random.Range(-10/melmoso.precision, 10/melmoso.precision);
        pos.y = target.position.y;
        Vector3 dir = pos - melmoso.transform.position;  // get target direction
        //float h = dir.y;  // get height difference
        dir.y = 0;  // retain only the horizontal direction
        float dist = dir.magnitude;  // get horizontal distance
        //angle = angle + Random.Range(-10, 10);
        float a = angle * Mathf.Deg2Rad;  // convert angle to radians
        dir.y = dist * Mathf.Tan(a);  // set dir to the elevation angle
        //dist += h / Mathf.Tan(a);  // correct for small height differences
        // calculate the velocity magnitude
        float vel = Mathf.Sqrt(dist * melmoso.gravity / Mathf.Sin(2 * a));
        return vel * dir.normalized;
    }

    void RotateToTarget(float rotSpeed)
    {
        Quaternion rotation = Quaternion.LookRotation(melmoso.target.position - melmoso.transform.position);
        rotation.x = 0; rotation.z = 0;
        melmoso.transform.rotation = Quaternion.Slerp(melmoso.transform.rotation, rotation, Time.deltaTime * rotSpeed);
    }
}
