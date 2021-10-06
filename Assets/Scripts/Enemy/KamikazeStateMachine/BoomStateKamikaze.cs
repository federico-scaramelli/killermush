using UnityEngine;
using System.Collections;

public class BoomStateKamikaze : IKamikazeState
{
    private readonly StatePatternKamikaze kamikaze;

    public BoomStateKamikaze(StatePatternKamikaze statePatternKamikaze)
    {
        kamikaze = statePatternKamikaze;
    }

    public void UpdateState()
    {

    }

    public void EnterState()
    {
        
    }

    private bool exploded=false;
    private float dist;
    public void FixedUpdateState()
    {
        if(exploded==false)
        {
            Vector3 explosionPos = kamikaze.transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, kamikaze.explosionRadius);

            foreach (Collider hit in colliders)
            {
                if (hit.transform != kamikaze.transform)
                {
                    Health health = hit.GetComponent<Health>();
                    if (health!=null)
                    {
                        dist = Vector3.Distance(kamikaze.transform.position, hit.transform.position);
                        dist -= kamikaze.explosionRadius;
                        dist *= -1;
                        if (hit.transform == kamikaze.target.transform)
                            health.Damage(dist * kamikaze.damageMultiplierToPlayer);
                        else
                            health.Damage(dist * kamikaze.damageMultiplier);
                    }

                    UnityEngine.AI.NavMeshAgent agent = hit.GetComponent<UnityEngine.AI.NavMeshAgent>();
                    if (agent != null)
                    {
                        agent.enabled = false;
                        agent.updatePosition = false;
                        agent.updateRotation = false;
                    }

                    Rigidbody rb = hit.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.AddExplosionForce(kamikaze.explosionForce, explosionPos, kamikaze.explosionRadius, 3.0F);
                        if (hit.transform == kamikaze.target.transform)
                            rb.AddForce(kamikaze.transform.forward * kamikaze.explosionForceToPlayer, ForceMode.Impulse);
                    }

                    if (agent != null)
                    {
                        agent.enabled = true;
                        agent.updatePosition = true;
                        agent.updateRotation = true;
                    }
                }
            }
            exploded = true;
        }
        else
        {
            kamikaze.myHealth.Damage(kamikaze.myHealth.maxHealth);
            GameObject.Destroy(kamikaze.gameObject, 1f);
        }
    }

    public void ToApproachState()
    {
        kamikaze.approachState.EnterState();
        kamikaze.currentState = kamikaze.approachState;
    }

    public void ToAttackState()
    {
        kamikaze.attackState.EnterState();
        kamikaze.currentState = kamikaze.attackState;
    }

    public void ToBoomState()
    {
        kamikaze.boomState.EnterState();
        kamikaze.currentState = kamikaze.boomState;
    }
}
