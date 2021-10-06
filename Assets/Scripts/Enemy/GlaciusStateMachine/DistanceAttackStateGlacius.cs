using UnityEngine;
using System.Collections;

public class DistanceAttackStateGlacius : IGlaciusState {

    private readonly StatePatternGlacius glacius;
    private RaycastHit hit;

    public DistanceAttackStateGlacius(StatePatternGlacius statePatternGlacius)
    {
        glacius = statePatternGlacius;
    }

    public void UpdateState()
    {
        RotateToTarget(glacius.rotSpeed);
        glacius.timer -= Time.deltaTime;

        if (glacius.timer <= 0)
        {
            Vector3 direction = new Vector3(glacius.shootPoint.transform.forward.x + Random.Range(-10 / glacius.precision, 10 / glacius.precision), 
                                            glacius.shootPoint.transform.forward.y, 
                                            glacius.shootPoint.transform.forward.z + Random.Range(-10 / glacius.precision, 10 / glacius.precision));
            Transform ball = (Transform)GameObject.Instantiate(glacius.shot, glacius.shootPoint.position, Quaternion.identity);
            ball.rotation = Quaternion.LookRotation(direction, Vector3.up);
            ball.GetComponent<Rigidbody>().AddForce(ball.forward * glacius.shootForce, ForceMode.Impulse);
            glacius.timer = glacius.msBetweenShots/1000;
        }

        /*glacius.distance = Vector3.Distance(glacius.transform.position, glacius.target.position);
        glacius.timer3 -= Time.deltaTime;
        if (glacius.timer3 <= 0)
        {
            glacius.timer3 = glacius.checkIfPlayerIsForwardTimer;
            Debug.DrawLine(glacius.transform.up + glacius.transform.position, glacius.transform.up + glacius.transform.forward * glacius.distanceAttackRange + glacius.transform.position);
            if (Physics.Raycast(glacius.transform.up + glacius.transform.position, glacius.transform.up + glacius.transform.forward * glacius.distanceAttackRange, out hit, glacius.distanceAttackRange + 1))
            {
                if (hit.transform.tag == "Enemy" || hit.transform.tag == "Obstacle")
                    ToApproachState();
            }
        }*/

        glacius.distance = Vector3.Distance(glacius.transform.position, glacius.target.position);
        if (glacius.distance > glacius.distanceAttackRange)
            ToApproachState();

        if (glacius.distance < glacius.meleeAttackRange)
        {
            ToMeleeAttackState();
        }
    }

    public void EnterState()
    {
        glacius.shootForce = glacius.distanceAttackRange * 4f; 
        //4 è circa il rapporto corretto tra distanza e forza per far giungere il colpo alla massima distanza di attacco
    }

    public void FixedUpdateState()
    {

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