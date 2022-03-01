using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float movementSpeed;
    public bool vertical;
    public float changeTime = 3.0f;
    float timer;
    int direction = 1;

    Rigidbody2D theRB;

    Animator animator;

    bool broken = true;

    public ParticleSystem someEffect;

    // Start is called before the first frame update
    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();

        timer = changeTime;

        animator = GetComponent<Animator>();

        someEffect.Play();
    }

    private void Update()
    {
        if (!broken)
        {
            return;
        }

        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!broken)
        {
            return;
        }

        Vector2 position = theRB.position;
        if (vertical)
        {
            position.y = position.y + Time.deltaTime * movementSpeed * direction;

            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x = position.x + Time.deltaTime * movementSpeed * direction;

            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }

        theRB.MovePosition(position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        RubyController controller = collision.gameObject.GetComponent<RubyController>();
        if (controller != null)
        {
            controller.ChangeHealth(-3);
        }
    }

    public void Fix()
    {
        broken = false;
        theRB.simulated = false; // Rigidbody from the Physics System simulation,
                                 // so it won’t be taken into account by the system for collision

        animator.SetTrigger("Fixed");

        someEffect.Stop();
    }
}
