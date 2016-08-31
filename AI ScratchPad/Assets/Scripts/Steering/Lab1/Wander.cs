using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Wander : MonoBehaviour {

    public float speed = 1.0f;
    public float radius;
    [Range(1.0f, 100.0f)]
    public float chanceToTurn = 1.0f;
    public Vector3 initialVelocity;

    private Transform theTransform;
    private Rigidbody rigid;

	// Use this for initialization
	void Start () {
        theTransform = GetComponent<Transform>();
        theTransform.forward = Random.insideUnitSphere;
        rigid = GetComponent<Rigidbody>();
        rigid.velocity = initialVelocity.normalized * speed;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 steering = GetWanderForce() - rigid.velocity;
        steering /= rigid.mass;
        rigid.velocity += steering;
        rigid.velocity = rigid.velocity.normalized * speed;

        theTransform.forward = rigid.velocity.normalized;
    }

    private Vector3 GetWanderForce()
    {
        float chance = Random.Range(1.0f, 100.0f);
        if (chance <= chanceToTurn)
        {
            return RandomWander();
        }
        else
        {
            return rigid.velocity.normalized * radius;
        }
    }

    private Vector3 RandomWander()
    {
        Vector2 randomPoint = Random.insideUnitCircle;
        Vector3 displacement = randomPoint * radius;
        displacement = Quaternion.LookRotation(rigid.velocity) * displacement;
        return rigid.velocity.normalized * radius + displacement;
    }
}
