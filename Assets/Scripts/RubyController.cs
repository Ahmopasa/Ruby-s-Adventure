//# The effects of Time.deltaTime #

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    Rigidbody2D theRB;
    float horizontalInput;
    float verticalInput;
    // Start is called before the first frame update
    void Start()
    {
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 10; // Makes the FPS 10.

        theRB = GetComponent<Rigidbody2D>();
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
        position.x = position.x + 3.0f * horizontalInput * Time.deltaTime;
        position.y = position.y + 3.0f * verticalInput * Time.deltaTime;
        
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
    }
}
