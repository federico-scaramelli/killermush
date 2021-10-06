using UnityEngine;
using System.Collections;

public class SkifBall : MonoBehaviour {

    public float ttl;
    public float radius;
    public float explosionForce;
    public float explDamageMultiplier;
    public Transform[] skifDecals;
    private int decalIndex;

	void Start ()
    {
        Destroy(gameObject, ttl);
	}

    void OnCollisionEnter(Collision col)
    {
        Collider[] colliders = Physics.OverlapSphere(col.contacts[0].point, radius); 
        foreach(Collider c in colliders)
        {
            Transform t = c.transform;
            if (t.GetComponent<Health>())
            {
                Health h = t.GetComponent<Health>();
                float dist = (transform.position - t.position).magnitude;
                float damage = (dist - radius) * (-1) * explDamageMultiplier;
                h.Damage(damage);
            }
            if (!c.GetComponent<Rigidbody>()) continue;
            c.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, col.contacts[0].point, radius, 1, ForceMode.Impulse);
        }

        if(col.collider.tag=="Floor")
        {
            Quaternion hitRotation = Quaternion.FromToRotation(Vector3.up, col.contacts[0].normal);
            Vector3 point = col.contacts[0].point;
            point.y += 0.1f;
            decalIndex = Random.Range(0, skifDecals.Length);
            Instantiate(skifDecals[decalIndex], point, hitRotation);
        }

        Destroy(gameObject);
    }
}
