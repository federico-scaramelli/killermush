using UnityEngine;
using System.Collections;

public class SuicideStateMelmoso : IMelmosoState
{
    private readonly StatePatternMelmoso melmoso;

    Collider[] colliders;

    public SuicideStateMelmoso(StatePatternMelmoso statePatternMelmoso)
    {
        melmoso = statePatternMelmoso;
    }

    public void UpdateState()
    {
        //AT ANIM END..
        GameObject.Instantiate(melmoso.suicideSmoke, melmoso.transform.position, Quaternion.identity);
        colliders = Physics.OverlapSphere(melmoso.transform.position, melmoso.dangerRange*2);
        foreach (Collider c in colliders)
        {
            Transform t = c.transform;
            if (t.GetComponent<Health>())
            {
                Health h = t.GetComponent<Health>();
                float dist = (melmoso.transform.position - t.position).magnitude;
                float damage = (dist - melmoso.dangerRange*2) * (-1) * melmoso.explDamageMultiplier;
                h.Damage(damage);
            }
            if (!c.GetComponent<Rigidbody>()) continue;
            c.GetComponent<Rigidbody>().AddExplosionForce(melmoso.explosionForce, melmoso.transform.position, melmoso.dangerRange*2, 1, ForceMode.Impulse);
        }
        GameObject.Destroy(melmoso.gameObject);
    }

    public void EnterState()
    {
        //PLAY ANIM
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
}
