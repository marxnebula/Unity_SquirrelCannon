using UnityEngine;
using System.Collections;

public class AutoBarrel : Barrel {

    // Barrel auto rotates to this position
    public float rotateTo = 0f;

    void Start()
    {
        // Call the base start
        base.Start();

        // Make sure they are not equal
        enterRotateTo = transform.rotation.eulerAngles.z - 10f;

    }


	// Update is called once per frame
	void FixedUpdate () {

        // Update the base
        base.FixedUpdate();


        // Auto barrel will rotate and then launch when complete
        RotateAndLaunch();

	}


    /*
     * When main character enters the barrel, the barrel will auto rotate to a rotation
     * and then launch the main character.
     */
    void RotateAndLaunch()
    {
        // If main character has entered the barrel
        if (isEntered)
        {
            
            // Rotate to this position
            transform.rotation = Quaternion.RotateTowards(transform.rotation,
                    Quaternion.Euler(0f, 0f, rotateTo), Time.deltaTime * rotateWhenEnterSpeed);


            // If current rotation is equal to rotateTo the auto launch the barrel
            if ((transform.rotation.eulerAngles.z == rotateTo) && (isEntered))
            {

                // Turn main character sprite renderer on
                mainCharacter.GetComponent<SpriteRenderer>().enabled = true;

                // Set that is has left barrel
                mainCharacter.GetComponent<Donkey>().SetIsEntered(false);
                isEntered = false;

                // Vector for adding force
                Vector2 v = new Vector2(-barrelStrength * Mathf.Sin(transform.rotation.eulerAngles.z * (Mathf.PI / 180)),
                    barrelStrength * Mathf.Cos(transform.rotation.eulerAngles.z * (Mathf.PI / 180)));

                // Add the force to the main character
                mainCharacter.GetComponent<Rigidbody2D>().AddForce(v);

                print("SHOOT");
            }
        }
    }
}
