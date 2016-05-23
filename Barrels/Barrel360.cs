using UnityEngine;
using System.Collections;

public class Barrel360 : Barrel {

    // Rotation speed
    public float rotate360Speed = 0f;


    void Start()
    {
        // Call base Start
        base.Start();
    }

	// Update is called once per frame
	void FixedUpdate () {

        // Update the base
        base.FixedUpdate();

        // Rotate the barrel once the main character has entered
        if(isEntered)
        {
            transform.Rotate(new Vector3(0f, 0f, -rotate360Speed) * Time.fixedDeltaTime);
        }

        
        
	
	}
}
