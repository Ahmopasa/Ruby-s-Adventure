//# The effects of Time.deltaTime #

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    /* # Character HP # */
    public int health { get { return currentHealth; } } // A property to handle 'currentHealth'.
    public int maxHealth = 5;
    int currentHealth;
    bool isInvincible;
    public float timeInvincible = 2.0f;
    float invincibleTimer;

    /* # Character Movement # */
    public float movementSpeed = 3.0f;
    Rigidbody2D theRB;
    float horizontalInput;
    float verticalInput;
    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);

    /* # Projecttile Section # */
    public GameObject projectilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 10; // Makes the FPS 10.

        theRB = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;

        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        /*  - Why did we used 'FixedUpdate()' ?
            The physics system is updated at a different rate than the game. 
            Update is called every time the game computes a new image, the problem is that this is called at an uncertain rate.
            It could be 20 images per second on a slow computer, or 3000 on a very fast one.
            For the physics computation to be stable, it needs to update at regular intervals (for example, every 16ms).
            However, you shouldn’t read input in the Fixedupdate function. FixedUpdate isn’t continuously running,
            so there’s a chance a User Input will be missed.
        */
        Vector2 position = theRB.position;
        position.x = position.x + movementSpeed * horizontalInput * Time.deltaTime;
        position.y = position.y + movementSpeed * verticalInput * Time.deltaTime;
        
        /*  - Why did we used 'Rigidbody2D.MovePosition()' instead of 'transform.position' ? 
            Because, Rigidbody moves before the game object. If it collides with any collider, it tries to move back the game object.
            If we keep moving in the same direction, there'll be a fighting between game object and the rigid body.
            That fighting cause Jittering effect oh the game object.         
        */
        theRB.MovePosition(position);
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontalInput, verticalInput);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0.0f)
            {
                isInvincible = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            /* - Raycasting is the action of casting a ray in the Scene and checking to see if that ray intersects with a Collider. 
               A ray has a starting point, a direction and a length.
               The term to “cast” a ray is used because the test is made from the starting point all along the ray until its end.
             
               - Here you'll cast a ray,
               from your main character’s position, 
               in the direction she is looking, 
               from a small distance, for example 1 or 1.5 units. 
            */
            RaycastHit2D hit = Physics2D.Raycast(
                                    theRB.position + Vector2.up * 0.2f,
                                    lookDirection,
                                    1.5f,
                                    LayerMask.GetMask("NPC")
                                    );

            /* - If the Raycast didn’t intersect anything, 
               this will be null so do nothing. 
               Otherwise, RaycastHit2D will contain the Collider the Raycast intersected
            */
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            animator.SetTrigger("Hit");

            if (isInvincible)
            {
                return;
            }

            isInvincible = true;
            invincibleTimer = timeInvincible;
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        // Clamping ensures that the first parameter (here currentHealth + amount)
        // never goes lower than the second parameter (here 0) and
        // never goes above the third parameter (maxHealth).
        // So Ruby’s health will always stay between 0 and maxHealth.

        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(
                                        projectilePrefab, 
                                        theRB.position + Vector2.up * 0.5f, 
                                        Quaternion.identity
                                        ); // Quaternion.identity mean "no rotation".

        Projectile projectile = projectileObject.GetComponent<Projectile>();

        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
    }
}
