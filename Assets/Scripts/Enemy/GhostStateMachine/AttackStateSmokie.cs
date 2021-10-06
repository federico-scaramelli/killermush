using UnityEngine;
using System.Collections;

public class AttackStateSmokie : ISmokieState
{
    private readonly StatePatternSmokie smokie;
    private bool isOnThePlayer=false;
    private float oldSpeedValue;

    public AttackStateSmokie(StatePatternSmokie statePatternSmokie)
    {
        smokie = statePatternSmokie;
    }

    public void UpdateState()
    {
        smokie.text.text = smokie.clickValue.ToString();

        if (isOnThePlayer)
        {
            smokie.playerHealth.Damage(smokie.damage);
            smokie.target.transform.GetComponent<PlayerController>().speed = 0.7f;

            smokie.substractValueFunction();

            if (Input.GetKeyDown(smokie.key))
            {
                smokie.clickValue += 10;
                if (smokie.clickValue >= 100)
                {
                    smokie.target.transform.GetComponent<PlayerController>().speed = oldSpeedValue;
                    smokie.myHealth.Damage(100);
                }
            }
        }
    }

   

    public void EnterState()
    {
        smokie.agent.enabled = false;
        smokie.agent.updatePosition = false;
        smokie.agent.updateRotation = false;
        oldSpeedValue = smokie.target.transform.GetComponent<PlayerController>().speed;
    }

    public void FixedUpdateState()
    {
        smokie.distance = Vector3.Distance(smokie.transform.position, smokie.target.position);
        smokie.transform.position = Vector3.MoveTowards(smokie.transform.position, smokie.target.GetChild(3).position, 20 * Time.deltaTime);
        if(smokie.distance < 2)
        {
            isOnThePlayer = true;
            smokie.transform.SetParent(smokie.target.transform);
        }

        smokie.transform.rotation = smokie.target.transform.rotation;  
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
}
