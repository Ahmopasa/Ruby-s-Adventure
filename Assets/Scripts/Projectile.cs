using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D theRB;

    // Start is called before the first frame update
    void Awake()
    {
        theRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.magnitude > 1000.0f) // Remember that position can be seen as a vector from the center of our world
                                                    // to where your object is, and magnitude is the length of that vector.
                                                    // So the magnitude of the position is the distance to the center.
        {
            Destroy(gameObject);
        }
    }

    public void Launch(Vector2 direction, float force)
    {
        theRB.AddForce(direction * force);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyController e = collision.collider.GetComponent<EnemyController>();
        if (e != null)
        {
            e.Fix();
        }

        Destroy(gameObject);
    }
}
